@echo off
rem    2013/Jan/26  Saturday  13:33pm b

rem AxErd-run.BAT
rem ...\AxErd\bin\Debug\


rem All of these % parameters are REQUIRED.
rem  %1 =run ,  literally those three characters.
rem  %2 = Output filename node for appmodu, such as =GeneralLedger (not "General ledger").
rem  %3 = Formal AX6 appmodu name including spaces, such as ="General ledger". 
rem
rem Example-  AxErd-run.bat  run  GeneralLedger  "General ledger"

if ''=='%1' echo This BAT will Not run now, missing three char param 'run'.
if ''=='%1' goto LAB86END



REM AxErd.exe  /in2 C:\Main\ERD-PerModule-Docu\AxErd2\AxErd2-SkyDri\DataFilesCPTM\  /out3 C:\inetpub\wwwroot\Dynamics\AxErd2\  /in1 Test-b5-  /out1 Modu-GeneralLedger-  /m1 "General ledger"  /s3 true  /s1 1


@echo on
@echo.
AxErd.exe  /in2 C:\Main\VisStudioProjs\AxErd31\AxErd\AxErd\_RelatedFiles_AxErd\DataFilesCPTM\  /out3 C:\inetpub\wwwroot\Dynamics\AxErd31a\  /in1 AxErd-Input-  /out1 Modu  /m1 %2  /s3 true  /s1 1
@echo.
@echo off


rem  //  /S
rem  /s1  :  /InitialSleepSeconds
rem  /s2  :  /SkipOutputOfFilesCP
rem  /s3  :  /SkipOutputOfFilesTM
rem
rem  //  /IN
rem  /in1  :  /InputDelimFilesCommonPrefix
rem  /in2  :  /PathToInputDelimFiles
rem  /in3  :  /DelimFileChildParents
rem  /in4  :  /DelimFileTableModules
rem  /in5  :  /DelimFileColumnColumn
rem
rem  //  /OUT
rem  /out1  :  /OutputHtmlFilesCommonPrefixCP
rem  /out2  :  /OutputHtmlFilesCommonPrefixTM
rem  /out3  :  /PathToOutputHtmlFiles
rem  /out4  :  /HtmlFileChildParents
rem  /out5  :  /HtmlFileParentChilds
rem  /out6  :  /HtmlFileTableModules
rem  /out7  :  /HtmlFileModuleTables
rem
rem  //  /M
rem  /m1  :  /RestrictToModulesCP
rem  /m2  :  /RestrictToModulesTM



:LAB86END

@echo %0  done.
 
