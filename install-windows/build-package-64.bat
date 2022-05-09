rmdir /S /Q .\files
rmdir /S /Q .\plugins
mkdir .\files

copy ..\video.ico .\files\VideoScreenSaver.ico
copy ..\bin\Release\Interop.WMPLib.dll .\files\
copy ..\bin\Release\AxInterop.WMPLib.dll .\files\
copy "..\bin\Release\Video Screen Saver.exe" .\files\Video-screen-saver.scr


call subscribe "files\Video-screen-saver.scr"

iscc /q create-installation-package-64.iss

call subscribe "output\video-screen-saver-install.exe"

sha256sum "output\video-screen-saver-install.exe" > "output\signature.txt"

pause
