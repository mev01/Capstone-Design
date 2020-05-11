# 캡스톤 디자인
## VR
 * Unity 버전: **2019.3.9f1**  
 * 감정분석 파이썬 코드를 Unity 상에서 쓰기 위해서 IronPython 사용  
   IronPython을 Unity에 맞춘 스크립트 https://github.com/exodrifter/unity-python  
   참고자료 https://javart.tistory.com/77
## 감정분석  

## commit시 주의 사항
  100MB이상의 파일의 경우 아래 과정을 따른다. 
  1. 아래 링크에서 Git LFS 다운로드  
  https://git-lfs.github.com/
  2. 적용할 repository에서 다음 명령어 실행(more?이 뜨는 경우 재부팅)  
  ```git lfs install```  
  3. 관리할 파일을 지정  
  ```git lfs track "파일 경로"```  
  ```git add .gitattributes```  
  ```git commit ```  
  4. 이후 다른 파일들은 하던대로 commit
  
