@echo off
set /p jm="Enter map name: "
copy %jm%.jm Json2wmapConv\bin\Debug
Json2wmapConv\bin\Debug\Json2wmapConv %jm%.jm %jm%.wmap
echo Compiled map!
pause