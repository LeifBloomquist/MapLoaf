@echo off
cls
del *.o
del *.prg
cl65 -Osir -t c64          MapLoafClient.c -o maploaf.prg
cl65 -Osir -t c64 -D LOCAL MapLoafClient.c -o maploaf-local.prg
echo.
dir *.prg
copy *.prg w:\
