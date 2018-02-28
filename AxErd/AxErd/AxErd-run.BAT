@echo off
rem    2013/April/26  Friday  17:20pm
rem Deploy\ contents are then manually copied to http://www.microsoft.com/dynamics/ax/erd/ax2012r2/ corresponding directory.

rem AxErd-run.BAT
rem ...\AxErd\bin\Debug\


rem All of these % parameters are REQUIRED.
rem  %1 =run ,  literally those three characters.
rem  %2 = Formal AX6 appmodu name including spaces, such as ="General ledger".
rem  %3 = Free form, last thing before the final param, where final is /s1. 
rem
rem Example-  AxErd-run.bat  run  "General ledger"

if not 'runbycallerbat'=='%1' echo This BAT will Not run now, missing the fourteen char first param 'runbycallerbat'. (%1)
if not 'runbycallerbat'=='%1' goto LAB86END



@echo on
@echo.
AxErd.exe  /in2 C:\Main\VisStudioProjs\AxErd33\AxErd\AxErd\_RelatedFiles_AxErd\DataFilesCPTM\  /out3 C:\Main\VisStudioProjs\AxErd33\AxErd\AxErd\_RelatedFiles_AxErd\Deploy\  /in1 AxErd-Input-  /m1 %2  /s1 1
@echo.
@echo -------------------------------------------
@echo.
@echo off



:LAB86END

@echo %0  done.
 
