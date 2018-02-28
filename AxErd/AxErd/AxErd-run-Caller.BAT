@echo off
echo .

rem GeneMi   ,   2013/April/26  Friday  17:20pm
rem AxErd-run-Caller.BAT

if not 'runbyhuman'=='%1' echo This BAT will Not run now, missing the ten char first param 'runbyhuman'. (%1)
if not 'runbyhuman'=='%1' goto LAB86END

set paramRun14=runbycallerbat



@echo .
@echo Special Case 1, only to Enhance manually built *.htm files with Footer.
@echo .
@echo on
AxErd.exe  /s1 16  /in81 C:\Main\VisStudioProjs\AxErd33\AxErd\AxErd\_RelatedFiles_AxErd\Deploy-81-Needs-Footer\  /out81 C:\Main\VisStudioProjs\AxErd33\AxErd\AxErd\_RelatedFiles_AxErd\Deploy\  /m81 "*.htm"  /in1 AxErd-Input-  /in2 C:\Main\VisStudioProjs\AxErd33\AxErd\AxErd\_RelatedFiles_AxErd\DataFilesCPTM\  /SkipOutputOfFilesCP true  /SkipOutputOfFilesEnhancedWithFooter false
@echo off
@echo .
@echo .



@echo .
@echo Special Case 2, Accounts payable PC and CP, also builds all TM and MT.
@echo .
@echo on
AxErd.exe  /s1 16  /in2 C:\Main\VisStudioProjs\AxErd33\AxErd\AxErd\_RelatedFiles_AxErd\DataFilesCPTM\  /out3 C:\Main\VisStudioProjs\AxErd33\AxErd\AxErd\_RelatedFiles_AxErd\Deploy\  /in1 AxErd-Input-  /m1 "Accounts payable"  /SkipOutputOfFilesTM false
@echo off
@echo .
@echo .




rem These do NOT rebuild all TM and MT .htm files, just PC and CP .htm files.
rem
set ModuOfficial=Accounts receivable
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Budgeting
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Cash and bank management
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Compliance and internal controls
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Financial management
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Fixed assets
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=General ledger
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Human resources
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Inventory and warehouse management
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Procurement and sourcing
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Production control
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Project management and accounting
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Retail
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Sales and marketing
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Service management
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Travel and expense
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .



echo ----- End of Formals, Start of Informals -----------------------
echo .



set ModuOfficial=AIF
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=AppConfig
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Business Intelligence
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Client
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Compiler
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Developer and Partner Tools
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Dynamics Online
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Enterprise Portal
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Global financial management - _W
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Global financial management - Asia Pacific
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Global financial management - Eastern Europe
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Global financial management - Latin America
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Global financial management - North America
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Global financial management - Russia
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Global financial management - Western Europe
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Infrastructure
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Manufacturing
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=No Owner
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Office Business App
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Public Sector SL1
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Server and Tools
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Setup
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Tax
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Upgrade framework
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .


set ModuOfficial=Workflow
echo %paramRun14%  "%ModuOfficial%"
call AxErd-run.bat  %paramRun14%  "%ModuOfficial%"
echo .



rem ---------------

:LAB86END

set paramRun14=
set ModuFile=
set ModuOfficial=


@echo End of %0
