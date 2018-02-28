/***
GeneMi    ,    2013-March-29  Friday  16:33pm
***/


using System;
using SysThread = System.Threading;


namespace AxErd
{
	class CMainProgram
	{
		/****  

AxErd.exe  /in2 C:\Main\VisStudioProjs\AxErd31\AxErd\AxErd\_RelatedFiles_AxErd\DataFilesCPTM\  /out3 C:\inetpub\wwwroot\Dynamics\AxErd31a\  /in1 AxErd-Input-  /m1 "General ledger"  /s3 false  /s1 16


See AxErd2-run.BAT

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
		rem  /out1  :  /OutputHtmlFilesCommonPrefixCPPC
		rem  /out2  :  /OutputHtmlFilesCommonPrefixTMMT
		rem  /out3  :  /PathToOutputHtmlFiles
		rem  /out4  :  /HtmlFileChildParents
		rem  /out5  :  /HtmlFileParentChilds
		rem  /out6  :  /HtmlFileTableModules
		rem  /out7  :  /HtmlFileModuleTables
		rem
		rem  //  /M
		rem  /m1  :  /RestrictToModulesCPPC
		rem  /m2  :  /RestrictToModulesTMMT

		------------------------ (Values stored in CRunProperties.) --------


		>> AxErd2.exe
		 /PathToInputDelimFiles          C:\Main\ERD-PerModule-Docu\AxErd2\AxErd2\DataFilesCPTM\
		 /PathToOutputHtmlFiles          C:\inetpub\wwwroot\Dynamics\AxErd2\
		 /InputDelimFilesCommonPrefix    Test2-
		 /OutputHtmlFilesCommonPrefixCPPC  Modu-
		 /RestrictToModulesCPPC            "Accounts payable,Accounts receivable"
		 /InitialSleepSeconds            19


Others
		 /DelimFileChildParents TabDelim-Child-Parents.txt
		 /DelimFileTableModules TabDelim-Table-Modules.txt
		 /DelimFileColumnColumn TabDelim-Column-Column.txt

		 /OutputHtmlFilesCommonPrefixTMMT ""
		 /HtmlFileChildParents Child-Parents.htm
		 /HtmlFileParentChilds Parent-Childs.htm
		 /HtmlFileTableModules Table-Modules.htm
		 /HtmlFileModuleTables Module-Tables.htm

		 /RestrictToModules "Accounts payable,Accounts receivable"

		****/
		static int Main(string[] _sArgs)  // .entrypoint
		{

			int nSleepSecs = 0;


			// Process first the /s1 parameter.
			for (int aa=0; aa <= _sArgs.Length - 2; aa = aa + 2)
			{
				if (_sArgs[aa].ToLower() == "/s1"
					|| _sArgs[aa].ToLower() == "/initialsleepseconds"
					)
				{
					nSleepSecs = Convert.ToInt32(_sArgs[aa+1]);
					if (nSleepSecs <= 1)
					{
						break;  // Minimum sleep time is 2 seconds.
					}
					Console.WriteLine("WILL sleep for {0} =seconds, so you can attach debugger...", nSleepSecs);

					for (int bb=nSleepSecs; 0 < bb; bb--)
					{
						SysThread.Thread.Sleep( 1000 ); // 1 second, per loop.
						Console.WriteLine("{0} =seconds of sleep REMAINING...", bb);
					} // EOLoop bb
				}
			} // EOLoop aa


			CRunProperties crunProperties = new CRunProperties();
			int nInitialSleepSeconds_param;  // Must at usage multiply by 1000 for milliseconds for Thread.Sleep.
			int nReturnCode = 1;  // Only =0 means Good, else Error.

			try
			{
				CDllCentral dllCentral;
				//

				ProcessTheCommandLineParameters
					(_sArgs,
					crunProperties,
					out nInitialSleepSeconds_param // Legacy location of this sleep processing, now unused here.
					);

				dllCentral = new CDllCentral(crunProperties);
				dllCentral.MainDll_DoItAll();

				nReturnCode = 0;
			}
			catch (Exception_AxErd exAxErd)
			{
				Console.WriteLine(exAxErd.ToStringApp());
				exAxErd.IsAlreadyCaughtAndProcessed = true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Error_nfh72: Unexpected System.Exception in Main.");
				Console.WriteLine(e.ToString());
			}

			return nReturnCode;
		}



		static private void ProcessTheCommandLineParameters
					(
					string[] _sArgs,
					CRunProperties _crunProperties,
					out int _nInitialSleepSeconds
					)
		{
			_nInitialSleepSeconds = 0;

			if (0 != (_sArgs.Length % 2))
			{
				throw new ApplicationException("Error_axerd_mwf52: User provided an odd number of command line parameters. Must be 0 or other even number of parameters.");
			}


			// One IF for every recognized command line parameter /Name.
			// For convenience, all parameter names have a short synonym alias.
			for (int pp = 0; pp < _sArgs.Length - 1; pp = pp + 2)
			{
				// /S
				if ("/InitialSleepSeconds".ToLower() == _sArgs[pp].ToLower()
					|| "/s1" == _sArgs[pp].ToLower())
				{
					_nInitialSleepSeconds = Convert.ToInt32(_sArgs[pp + 1]);
				}
				else if ("/SkipOutputOfFilesCP".ToLower() == _sArgs[pp].ToLower()
					|| "/s2" == _sArgs[pp].ToLower())
				{
					// User should give string value of either four characters 'true', or five chars 'false'.
					_crunProperties.m_bSkipOutputOfFilesCP = Convert.ToBoolean(_sArgs[pp + 1]);
				}
				else if ("/SkipOutputOfFilesTM".ToLower() == _sArgs[pp].ToLower()
					|| "/s3" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_bSkipOutputOfFilesTM = Convert.ToBoolean(_sArgs[pp + 1]);
				}
				else if ("/NumEntriesPerModuleSubNodeFileApprox".ToLower() == _sArgs[pp].ToLower()
					|| "/s4" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_nMaxItems_SubNode_CPPC_File_PerModule = Convert.ToInt32(_sArgs[pp + 1]);
				}

				// /IN
				else if ("/InputDelimFilesCommonPrefix".ToLower() == _sArgs[pp].ToLower()
					|| "/in1" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sInputDelimFilesCommonPrefix = _sArgs[pp + 1];
				}
				else if ("/PathToInputDelimFiles".ToLower() == _sArgs[pp].ToLower()
					|| "/in2" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sPathToInputDelimFiles = _sArgs[pp+1];
				}
				else if ("/DelimFileChildParents".ToLower() == _sArgs[pp].ToLower()
					|| "/in3" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sDelimFileChildParents = _sArgs[pp + 1];
				}
				else if ("/DelimFileTableModules".ToLower() == _sArgs[pp].ToLower()
					|| "/in4" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sDelimFileTableModules = _sArgs[pp + 1];
				}
				else if ("/DelimFileColumnColumn".ToLower() == _sArgs[pp].ToLower()
					|| "/in5" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sDelimFileColumnColumn = _sArgs[pp + 1];
				}

				// /OUT
				else if ("/OutputHtmlFilesCommonPrefixCPPC".ToLower() == _sArgs[pp].ToLower() // "Modu-".
					|| "/out1" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sOutputHtmlFilesCommonPrefixCPPC = _sArgs[pp + 1];
				}
				else if ("/OutputHtmlFilesCommonPrefixTMMT".ToLower() == _sArgs[pp].ToLower() // "All-".
					|| "/out2" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sOutputHtmlFilesCommonPrefixTMMT = _sArgs[pp + 1];
				}
				else if ("/PathToOutputHtmlFiles".ToLower() == _sArgs[pp].ToLower()
					|| "/out3" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sPathToOutputHtmlFiles = _sArgs[pp + 1];
				}
				else if ("/HtmlFileChildParents".ToLower() == _sArgs[pp].ToLower()
					|| "/out4" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sHtmlFileChildParents = _sArgs[pp + 1];
				}
				else if ("/HtmlFileParentChilds".ToLower() == _sArgs[pp].ToLower()
					|| "/out5" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sHtmlFileParentChilds = _sArgs[pp + 1];
				}
				else if ("/HtmlFileTableModules".ToLower() == _sArgs[pp].ToLower()
					|| "/out6" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sHtmlFileTableModules = _sArgs[pp + 1];
				}
				else if ("/HtmlFileModuleTables".ToLower() == _sArgs[pp].ToLower()
					|| "/out7" == _sArgs[pp].ToLower())
				{
					_crunProperties.m_sHtmlFileModuleTables = _sArgs[pp + 1];
				}

				// /M
				else if ("/RestrictToModulesCPPC".ToLower() == _sArgs[pp].ToLower()
					|| "/m1" == _sArgs[pp].ToLower())
				{
					// User probably needs to bound the value or values between quote marks, such as "The Module Name1,The Module Name2".
					// The quote marks assist with the embedded spaces (if any), and with the delimiting comma (if any).

					_crunProperties.m_sRestrictToModulesCPPC = _sArgs[pp + 1].Split
						(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

					if (1 != _crunProperties.m_sRestrictToModulesCPPC.Length)
					{
						throw new Exception_AxErd
							("Error-AxErd-8261gw4",
							"/RestrictToModulesCPPC (/m1) limited to max of one module.",
							null,
							null
							);
						//throw new ApplicationException("Error-AxErd-8261gw4: /RestrictToModulesCPPC (/m1) limited to max of one module.");
					}
				}
				else if ("/RestrictToModulesTMMT".ToLower() == _sArgs[pp].ToLower()
					|| "/m2" == _sArgs[pp].ToLower())
				{
					// User probably needs to bound the value or values between quote marks, such as "The Module Name1,The Module Name2".
					// The quote marks assist with the embedded spaces (if any), and with the delimiting comma (if any).

					_crunProperties.m_sRestrictToModulesTMMT = _sArgs[pp + 1].Split
						(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				}

				else
				{
					throw new Exception_AxErd
						("Error_axerd_mwp51",
						"User provided an unrecognized command line parameter /Name.  Also, each parameter /Name must begin with down-left slash /, not with a dash - etc." + _sArgs[pp],
						null,
						null
						);
					//throw new ApplicationException("Error_axerd_mwp51: User provided an unrecognized command line parameter /Name.  Also, each parameter /Name must begin with down-left slash /, not with a dash - etc." + _sArgs[pp]);
				}
			} // EOFor pp.


			// Validations.
			if ("" != _crunProperties.m_sPathToInputDelimFiles)
			{
				if (!_crunProperties.m_sPathToInputDelimFiles.EndsWith("\\"))  // Any user value for path must end with \ (tho not for zero length string).
				{
					_crunProperties.m_sPathToInputDelimFiles += "\\";
				}
			}

			if ("" != _crunProperties.m_sPathToOutputHtmlFiles)
			{
				if (!_crunProperties.m_sPathToOutputHtmlFiles.EndsWith("\\"))
				{
					_crunProperties.m_sPathToOutputHtmlFiles += "\\";
				}
			}

			if (2 > _crunProperties.m_nMaxItems_SubNode_CPPC_File_PerModule)
			{
				_crunProperties.m_nMaxItems_SubNode_CPPC_File_PerModule = 2;
			}

		}

	}
}
