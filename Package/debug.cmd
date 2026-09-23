@echo off
set TargetFramework=%1
set ProjectName=%2

XCOPY "..\Client\bin\Debug\%TargetFramework%\%ProjectName%.Client.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\" /Y
XCOPY "..\Client\bin\Debug\%TargetFramework%\%ProjectName%.Client.Oqtane.pdb" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\" /Y
XCOPY "..\Server\bin\Debug\%TargetFramework%\%ProjectName%.Server.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\" /Y
XCOPY "..\Server\bin\Debug\%TargetFramework%\%ProjectName%.Server.Oqtane.pdb" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\" /Y
XCOPY "..\Shared\bin\Debug\%TargetFramework%\%ProjectName%.Shared.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\" /Y
XCOPY "..\Shared\bin\Debug\%TargetFramework%\%ProjectName%.Shared.Oqtane.pdb" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\" /Y
XCOPY "..\Client\bin\Debug\%TargetFramework%\Oqtane.Licensing.Client.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\" /Y
XCOPY "..\Client\bin\Debug\%TargetFramework%\Oqtane.Licensing.Server.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\" /Y
XCOPY "..\Client\bin\Debug\%TargetFramework%\Oqtane.Licensing.Shared.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\" /Y
IF EXIST "..\Client\bin\Debug\%TargetFramework%\it\*.resources.dll" XCOPY "..\Client\bin\Debug\%TargetFramework%\it\*.resources.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\it\" /Y /I
IF EXIST "..\Client\bin\Debug\%TargetFramework%\it-IT\*.resources.dll" XCOPY "..\Client\bin\Debug\%TargetFramework%\it-IT\*.resources.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\%TargetFramework%\it-IT\" /Y /I
XCOPY "..\Server\wwwroot\*" "..\..\oqtane.framework\Oqtane.Server\wwwroot\_content\%ProjectName%\" /Y /S /I
