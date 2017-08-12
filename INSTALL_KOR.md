# WorldWide Telescope Windows Client 빌드와 설치

요구사항
-------------

Visual Studio 2015가 설치되어있어야 합니다. `https://www.visualstudio.com/dev-essentials/`에서 무료 커뮤니티 버전을 다운로드 할 수 있습니다. 

인스톨러를 빌드하기 위해 `Visual Studio 2015 Installer Projects`를 다운로드 해야합니다. 아래 링크에서 받으세요:
`https://visualstudiogallery.msdn.microsoft.com/f1cc3f3e-c300-40a7-8797-c509fb8933b9`

만약 인스톨러가 필요하지 않으면 빌드 전 솔루션에서 인스톨러를 제거하시면 됩니다.

GitHub의 100MB 파일 크기 제한 때문에 어쩔 수 없이 만들었던 압축 파일들을 풀어야 합니다. (여기를 참조하세요 `[Setup1/ReadMe_KOR.txt](Setup1/ReadMe_KOR.txt)`)

빌드
--------

저장소를 `Clone`하거나 ZIP 파일의 압축을 푼 뒤에 솔루션을 아래의 방법으로 엽니다.
`파일->열기->프로젝트/솔루션/[local repository path\wwt-windows-client<-master>]\WWTExplorer.sln`

그 뒤, `빌드 -> 솔루션 빌드`

이제 프로젝트를 컴파일하고 적절히 연결할 수 있습니다.

빌드를 성공한 뒤에 WordWide Telescope 윈도우 클라이언트 실행 파일이 다음 경로에 존재할겁니다.
`[local repository path\wwt-windows-client<-master>]\WWTExplorer3D\bin\Debug\WWTExplorer.exe`

즐거운 해킹 되시길^_^
