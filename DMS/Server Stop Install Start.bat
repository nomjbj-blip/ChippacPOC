echo off

net stop DACruxCore
net stop DACruxSEMDMS
net stop DACruxTEST

C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil -u C:\DBHitek\DACruxV5\101_ServerBin\DACrux.Framework.Service.exe
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil -u C:\DBHitek\DACruxV5\101_ServerBin\DACrux.TEST.Service.exe
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil -u C:\DBHitek\DACruxV5\101_ServerBin\DACrux.SEMDMS.Service.exe


echo --------------------------------------------
echo -                                          -
echo -          FILE COPY Please!!!             -
echo -                                          -
echo --------------------------------------------
pause


C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil -i C:\DBHitek\DACruxV5\101_ServerBin\DACrux.Framework.Service.exe
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil -i C:\DBHitek\DACruxV5\101_ServerBin\DACrux.TEST.Service.exe
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil -i C:\DBHitek\DACruxV5\101_ServerBin\DACrux.SEMDMS.Service.exe

echo off

net start DACruxCore
net start DACruxSEMDMS
net start DACruxTEST

exit