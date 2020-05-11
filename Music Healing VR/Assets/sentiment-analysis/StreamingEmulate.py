from __future__ import division
# __future__ 모듈은 파이썬 2버전과 3버전을 동시에 동작하도록 함. division은 python 3 스타일의 나누기 지원.

import os
    # 환경변수 설정
#os.environ["GOOGLE_APPLICATION_CREDENTIALS"]="/Users/hyunwoo/Desktop/speech-to-text-273518-4131322696e3.json"
os.environ["GOOGLE_APPLICATION_CREDENTIALS"]="C:/Users/YOU-DB/Code/git/Capstone-Design/sentiment-analysis/capstone-275211-e60e60421509.json"


import re
# 정규 표현식을 지원하기 위한 re(regular expression)모듈
import sys
# 파이썬 인터프리터 제어
import time

# [IMPORTS the Google Cloud clinet library, 밑 세줄]
from google.cloud import speech
from google.cloud.speech import enums
# enmus는 오디오 인코딩 타입 목록(enumerations)들이 포함되어있는 모듈
from google.cloud.speech import types
# types는 요청에 필요한 클래스(Ex. types.RecognitionAudio)들 포함

import pyaudio
# 사용자 음성 녹음을 위한 모듈
from six.moves import queue

# six는 Python 2, Python3 코드가 호환되도록 함.
# (설명) Python 3 recognized the standard library and moved several functions to different modules.
# Six provides a consistent interface to them through the fake six.moves module.
# https://six.readthedocs.io/#module-six.moves 참조
# queue : queue모듈이며 큐, 우선순위큐, 스택을 제공.

# Audio recording parameters
RATE = 16000
CHUNK = int(RATE / 10)  # 100ms, 버퍼는 1600


class MicrophoneStream(object):
    """마이크 입력 클래스"""
    """Opens a recording stream as a generator yielding the audio chunks."""

    # 객체를 초기화하는 special method
    # self.속성 = 매개변수형태로 속성 초기화
    def __init__(self, rate, chunk):
        self._rate = rate
        self._chunk = chunk

        # Create a thread-safe buffer of audio data
        self._buff = queue.Queue()
        self.closed = True

    # with구문 진입시점에 자동으로 호출되는 __enter__(special method임)
    def __enter__(self):
        # pyaudio 인터페이스 생성
        self._audio_interface = pyaudio.PyAudio()

        # 16비트, 모노로 마이크 열기
        # _fill_buffer 함수가 callback 함수인데
        # 실제 버퍼가 쌓이면 callback함수인 _fill_buffer 함수가 호출됨.
        # 즉, 마이크 입력으로 버퍼가 쌓이면 콜백함수인 _fill_buffer 함수로 전달 받음
        self._audio_stream = self._audio_interface.open(
            format=pyaudio.paInt16,
            # The API currently only supports 1-channel (mono) audio
            # https://goo.gl/z757pE
            channels=1, rate=self._rate,
            input=True, frames_per_buffer=self._chunk,
            # Run the audio stream asynchronously to fill the buffer object.
            # This is necessary so that the input device's buffer doesn't
            # overflow while the calling thread makes network requests, etc.
            stream_callback=self._fill_buffer,
        )

        self.closed = False

        return self

    # with 구문을 빠져나오기 직전에 호출되는 special method 즉, 클래스 종료 시 발생
    # type, value, traceback은 with 문을 빠져나오기 전에 예외가 발생했을 때의 정보를 나타냄
    def __exit__(self, type, value, traceback):
        # pyaudio 종료
        self._audio_stream.stop_stream()
        self._audio_stream.close()

        self.closed = True
        # Signal the generator to terminate so that the client's
        # streaming_recognize method will not block the process termination.
        self._buff.put(None)
        self._audio_interface.terminate()

    # 마이크 버퍼(chunk=1600)가 쌓이면 호출 됨
    def _fill_buffer(self, in_data, frame_count, time_info, status_flags):
        """Continuously collect data from the audio stream, into the buffer."""
        # 마이크 입력을 큐(queue)에 넣고 리턴. self_buff는 __init__ 메서드에서 큐 인스턴스로 초기화 됨
        self._buff.put(in_data)
        return None, pyaudio.paContinue

    # 제너레이터 함수
    def generator(self):
        # 클래스 종료될 때까지 무한 루프
        while not self.closed:
            # Use a blocking get() to ensure there's at least one chunk of
            # data, and stop iteration if the chunk is None, indicating the
            # end of the audio stream.

            # 큐에 데이터를 기다리는 block 상태
            # get(block=True, timeout=None) : 큐에서 항목을 제거하고 반환. block이 참이고 timeout이 None(기본값)이면,
            # 항목이 사용가능할 때까지 필요하면 블럭(대기). timeout이 양수면, 최대 timeout초 동안 블럭하고 그 시간 내에 사용가능
            # 한 항목이 없으면 Empty 예외 발생. block이 거짓일 때, 즉시 사용할 수 있는 항목이 있으면 반환하고, 그렇지 않으면
            # Empty 예외 발생
            chunk = self._buff.get()

            # 데이터가 없으면 문제 있음
            if chunk is None:
                return

            # data에 마이크 입력 받기
            data = [chunk]

            # Now consume whatever other data's still buffered.
            # 추가로 받을 마이크 데이터가 있는지 체크
            while True:
                try:
                    chunk = self._buff.get(block=False)
                    if chunk is None:
                        return
                    # 데이터 추가
                    data.append(chunk)
                except queue.Empty:
                    # 큐에 데이터가 없으면 break
                    break

            # 마이크 데이터 리턴
            yield b''.join(data)


def listen_print_loop(responses):
    """Iterates through server responses and prints them.

    The responses passed is a generator that will block until a response
    is provided by the server.

    Each response may contain multiple results, and each result may contain
    multiple alternatives; for details, see https://goo.gl/tjCPAU.  Here we
    print only the transcription for the top alternative of the top result.

    In this case, responses are provided for interim results as well. If the
    response is an interim one, print a line feed at the end of it, to allow
    the next result to overwrite it, until the response is a final one. For the
    final one, print a newline to preserve the finalized transcription.
    """
    num_chars_printed = 0
    for response in responses:
        if response.speech_event_type:
            time.sleep(2)
            text = transcript + overwrite_chars
            print('here is final text: {}'.format(text))
            return text
        if not response.results:
            continue

        # The `results` list is consecutive. For streaming, we only care about
        # the first result being considered, since once it's `is_final`, it
        # moves on to considering the next utterance.
        result = response.results[0]
        if not result.alternatives:
            continue

        # Display the transcription of the top alternative.
        transcript = result.alternatives[0].transcript

        # Display interim results, but with a carriage return at the end of the
        # line, so subsequent lines will overwrite them.
        #
        # If the previous result was longer than this one, we need to print
        # some extra spaces to overwrite the previous result
        overwrite_chars = ' ' * (num_chars_printed - len(transcript))

        if not result.is_final:
            sys.stdout.write(transcript + overwrite_chars + '\r')  # \r : 커서를 맨 앞으로 위치시키기
            sys.stdout.flush()

            num_chars_printed = len(transcript)

        else:
            print(transcript + overwrite_chars)

            # Exit recognition if any of the transcribed phrases could be
            # one of our keywords.
            if re.search(r'\b(끝내자|exit|quit)\b', transcript, re.I):
                print('Exiting..')
                break

            num_chars_printed = 0


def main():
    # See http://g.co/cloud/speech/docs/languages
    # for a list of supported languages.
    # 변환 언어 'en-US' or 'ko-KR'
    language_code = 'ko-KR'  # a BCP-47 language tag

    client = speech.SpeechClient()

    # 모듈의 메서드와 특성(Fields)는 아래 참조
    # https://cloud.google.com/speech-to-text/docs/reference/rpc/google.cloud.speech.v1#streamingrecognitionconfig
    # RecognitionConfig provides imformation to the recognizer that specifes how to process the request.
    config = types.RecognitionConfig(
        encoding=enums.RecognitionConfig.AudioEncoding.LINEAR16,
        sample_rate_hertz=RATE,
        enable_automatic_punctuation=True,  # (optional) 구두점 삽입 옵션
        language_code=language_code)

    # Streaming information to the recognizer that specifes how to process the request.
    streaming_config = types.StreamingRecognitionConfig(
        config=config,
        single_utterance=True,  # (optional) utterance 감지 시 텍스트 변환 종료
        interim_results=True)

    with MicrophoneStream(RATE, CHUNK) as stream:
        audio_generator = stream.generator()

        # StreamingRecognizeRequest : Top level message sent by the client for the StreamingRecognize method.
        # Multiple StreamingRecognizeRequest messages are sent.
        # 제너레이터 표현식으로 되어있음
        requests = (types.StreamingRecognizeRequest(audio_content=content)
                    for content in audio_generator)

        # streaming_recognize에서 리턴값으로 음성 분석 결과(results)를 리턴한다
        responses = client.streaming_recognize(streaming_config, requests)

        # Now, put the transcription responses to use.
        listen_print_loop(responses)

        # sentiment = api_request(Converted_text)


if __name__ == '__main__':  # __name__ : 현재 실행중인 모듈이름, 현재 실행 중인 모듈이 main모듈(프로그램 시작점)인지 확인
    main()