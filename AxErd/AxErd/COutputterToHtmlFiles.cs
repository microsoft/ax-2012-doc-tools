//     2013/March/29  Friday  10:33am   Fixed MT .htm off-by-one-record bug. Adding <h4> MS Dyn AX 2012 R2.

using System;
using SysCollGen = System.Collections.Generic;
using SysIo      = System.IO;
using SysTex     = System.Text;


namespace AxErd
{
	class COutputterToHtmlFiles
				: IOutputter
	{

		internal const string M_sCss_Stylesheet_FileName = "Aa_Stylesheet_AxErd.css";

		internal IRunPropertiesReader m_irunPropRead;
		internal CRunProperties m_crunPropertiesDebugger; // For read-only easier Watch debugging, only.
		//



		public COutputterToHtmlFiles  // .ctor
				(
				IRunPropertiesReader _irunPropRead
				)
		{
			this.m_irunPropRead = _irunPropRead;
			this.m_crunPropertiesDebugger = (CRunProperties) this.m_irunPropRead;
			return;
		}


		/// <summary>
		/// For example, write every .html file that this object knows how to write.
		/// </summary>
		public void DoOutputAll()
		{
			this.CreateHtmlFile_CP_210();
			this.CreateHtmlFile_PC_310();


			// Yes works. 2013/02/02 20:44pm. ANSWER: Yes still works as before, but also still needs to be subnoded!!
			// Thus a separate SubNode field and concept for SubNodeAllTM and SubNodeAllMT (ModuleSubNodeCPPC is what we already have!?).
			this.CreateHtmlFileTableModules();

			this.CreateHtmlFileModuleTables();


			return;
		}



//__________________________________________________________________________________________________________
//___________________________________  CP _210 etc  ________________________________________________________


		
		/// <summary>
		/// _2** (TWO hundreds) mean CP, as opposed to 3** meaning PC.  1** means generic CPPC.
		/// 'CP' means 'ChildParents'.
		/// </summary>
		private void CreateHtmlFile_CP_210() // this.IRunPropertiesReader has info available.
		{
			CModSubNodeState1 modSubNodeState1;
			//

			modSubNodeState1 = Initiate_ModSubNodeState1_CPPC_112();


			this.CreateHtmlFile_CPPC_120_ForOneModule
				(
					Enum_CP_PC_TM_MT.CP,  //!CP
					ref modSubNodeState1
				);

			return;
		}


		
		/// <summary>
		/// _2** (TWO hundreds) mean CP, as opposed to 3** meaning PC.  1** means generic CPPC.
		/// 'CP' means 'ChildParents'.
		/// </summary>
		private void CreateHtmlFile_PC_310() // this.IRunPropertiesReader has info available.
		{
			CModSubNodeState1 modSubNodeState1;
			//

			modSubNodeState1 = Initiate_ModSubNodeState1_CPPC_112();


			this.CreateHtmlFile_CPPC_120_ForOneModule
				(
					Enum_CP_PC_TM_MT.PC,  //!CP
					ref modSubNodeState1
				);

			return;
		}




		private CModSubNodeState1 Initiate_ModSubNodeState1_CPPC_112()
		{
			CModSubNodeState1 modSubNodeState1;
			SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> sdictModuleName_to_FileNameNodes;
			//

			sdictModuleName_to_FileNameNodes = this.m_irunPropRead
				.GetSDictOfModuleName_to_FileNameNodes();


			modSubNodeState1 = new CModSubNodeState1();

			modSubNodeState1.m_moduleFileOwnerInfo = sdictModuleName_to_FileNameNodes
				[
				CRunProperties.BuildModuleNameKey_From_ModuleName  // LowercaseNoSpaces
						(
						this.m_irunPropRead.GetRestrictToModulesCPPC()[0]
						)
				];

			return modSubNodeState1;
		}




		private void CreateHtmlFile_CPPC_120_ForOneModule
				(
				Enum_CP_PC_TM_MT _enum_CP_PC,
				ref CModSubNodeState1 _modSubNodeState1 //string _sModuleNameLowercaseNoSpaces
				)
		{
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableModule>> sdictOfModuleTables;
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> sdictOf_CPPC = null;
			SysCollGen.List<CValueTableModule> listTableModule_MT;
			//

			if (Enum_CP_PC_TM_MT .CP == _enum_CP_PC)
			{
				sdictOf_CPPC = this.m_irunPropRead.GetSDictOfChildParents();
			}
			else if (Enum_CP_PC_TM_MT .PC == _enum_CP_PC)
			{
				sdictOf_CPPC = this.m_irunPropRead.GetSDictOfParentChilds();
			}


			sdictOfModuleTables = this.m_irunPropRead.GetSDictOfModuleTables();
			listTableModule_MT = sdictOfModuleTables
				[
				_modSubNodeState1.m_moduleFileOwnerInfo .GetFormalModuleNameLowercaseNoSpaces()
				];


			//__________ LOOP __________ thru Tables of one Module ____________


			for (int ttmm=0; ttmm < listTableModule_MT.Count; ttmm++)
			{
				_modSubNodeState1.m_valueTableModule_MT_Previous = _modSubNodeState1.m_valueTableModule_MT;
				_modSubNodeState1.m_valueTableModule_MT = listTableModule_MT[ttmm];

				// Skip this T (in MT for one M) unless T is found as a key in the sdict for CP
				// (or in sdict for PC.  See _enum_CP_PC).

				if (! sdictOf_CPPC .ContainsKey
						(_modSubNodeState1.m_valueTableModule_MT .GetTableNameLowercase())
					)
				{
					continue;
				}


				this.CreateHtmlFile_CPPC_124_ManageSubNodeFileReuseVsCreation
					(
					_enum_CP_PC,
					ref _modSubNodeState1
					);
			}

			if (null != _modSubNodeState1.m_streamWriter)
			{
				_modSubNodeState1.m_streamWriter.WriteLine();
				_modSubNodeState1.m_streamWriter.WriteLine("</tbody>");
				_modSubNodeState1.m_streamWriter.WriteLine("</table>");
				_modSubNodeState1.m_streamWriter.WriteLine("</body>");
				_modSubNodeState1.m_streamWriter.WriteLine("</html>");

				_modSubNodeState1.m_streamWriter.Close();
				_modSubNodeState1.m_streamWriter.Dispose();
				_modSubNodeState1.m_streamWriter = null;
			}

			return;
		}





		/// <summary>
		/// Handles one M-T pair.
		/// </summary>
		/// <param name="_modSubNodeState1">ref structure of parameters.
		/// </param>
		private void CreateHtmlFile_CPPC_124_ManageSubNodeFileReuseVsCreation
			(
				Enum_CP_PC_TM_MT _enum_CP_PC,
				ref CModSubNodeState1 _modSubNodeState1
			)
		{
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> sdictOf_CPPC = null;
			SysCollGen.List<CValueTableTable> listTableTable_CPPC;
			CValueTableTable valueTableTable_CPPC;
			CPreviousNextFileSubNodes previousNextFileSubNodes = new CPreviousNextFileSubNodes();
			string sOneEntry;
			string sNextFileName = null, sHtmlNavigLinks = "";
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(512);
			bool boolNeedNewSubNodeFile;
			//

			if (Enum_CP_PC_TM_MT .CP == _enum_CP_PC)
			{
				sdictOf_CPPC = this.m_irunPropRead.GetSDictOfChildParents();
			}
			else if (Enum_CP_PC_TM_MT .PC == _enum_CP_PC)
			{
				sdictOf_CPPC = this.m_irunPropRead.GetSDictOfParentChilds();
			}

			listTableTable_CPPC = sdictOf_CPPC
				[_modSubNodeState1.m_valueTableModule_MT .GetTableNameLowercase()];
			valueTableTable_CPPC = listTableTable_CPPC[0];


			// For the one T Table in any one pairing of M-T, we always
			// write all the corresponding C-P entries together into the same module subnode file
			// (here T == C, and there are many P's for the C key).
			// Typically several M-T pairings become represented inside one subnode file.
			//
			// SubNode portion of sequential file name might SKIP-GAP, such as _11, _13, skipping _12.
			// Skips are Not a problem.


			// Reuse the subnode file, or create the next subnode file.

			boolNeedNewSubNodeFile = false;


			if (Enum_CP_PC_TM_MT .CP == _enum_CP_PC)
			{
				if (_modSubNodeState1.SubNode_of_LatestCreatedFile_String !=
						valueTableTable_CPPC.SubNode_CP_PerModule
					)
				{
					_modSubNodeState1.SubNode_of_LatestCreatedFile_String =
						valueTableTable_CPPC.SubNode_CP_PerModule;

					boolNeedNewSubNodeFile = true;
				}
			}
			else if (Enum_CP_PC_TM_MT .PC == _enum_CP_PC)
			{
				if (_modSubNodeState1.SubNode_of_LatestCreatedFile_String !=
						valueTableTable_CPPC.SubNode_PC_PerModule
					)
				{
					_modSubNodeState1.SubNode_of_LatestCreatedFile_String =
						valueTableTable_CPPC.SubNode_PC_PerModule;

					boolNeedNewSubNodeFile = true;
				}
			}


			if (boolNeedNewSubNodeFile)
			{
				// Create a new subnode file.

				// First, complete, then close the subnode file that we have completed.
				if (null != _modSubNodeState1.m_streamWriter)
				{
					_modSubNodeState1.m_streamWriter.WriteLine();
					_modSubNodeState1.m_streamWriter.WriteLine("</tbody>");
					_modSubNodeState1.m_streamWriter.WriteLine("</table>");


					// Write the Next-File link.
					if ("0" != _modSubNodeState1.m_previousNextFileSubNodes.m_sSubNode_Next) // Next
					{
						sbuilder.Length = 0;
						sbuilder.Append(this.m_irunPropRead .GetOutputHtmlFilesCommonPrefixCPPC());  // "Fky"
						sbuilder.Append("-");
						sbuilder.Append(_modSubNodeState1.m_moduleFileOwnerInfo .GetFileNameNode());  // "GenLen"
						sbuilder.Append("-");
						if (_enum_CP_PC == Enum_CP_PC_TM_MT.CP)
						{
							sbuilder.Append(this.m_irunPropRead .GetHtmlFileChildParents());  // "ChildParents"
						}
						else if (_enum_CP_PC == Enum_CP_PC_TM_MT.PC)
						{
							sbuilder.Append(this.m_irunPropRead .GetHtmlFileParentChilds());  // "ParentChilds"
						}
						sbuilder.Append("-");
						sbuilder.Append(_modSubNodeState1.m_previousNextFileSubNodes.m_sSubNode_Next); // Next
						sbuilder.Append(this.m_irunPropRead .GetHtmlFileExtension());

						sNextFileName = sbuilder.ToString();

						sHtmlNavigLinks = this.WritePreviousNextNavigationsAtFileTopOrBottom
							(
							 null
							,sNextFileName
							,true
							,false
							);

						_modSubNodeState1.m_streamWriter.WriteLine("<br />");
						_modSubNodeState1.m_streamWriter.WriteLine(sHtmlNavigLinks);
					}

					_modSubNodeState1.m_streamWriter.WriteLine("</body>");
					_modSubNodeState1.m_streamWriter.WriteLine("</html>");

					_modSubNodeState1.m_streamWriter .Flush();
					_modSubNodeState1.m_streamWriter .Close();
					_modSubNodeState1.m_streamWriter .Dispose();
					_modSubNodeState1.m_streamWriter = null;
				}

				//----------------------------------------------------

				// Close, then create the next subnode file?
				this.CreateFileForOutHtmlModule_CPPC_SubNode_133
					(
					_enum_CP_PC,
					ref _modSubNodeState1
					);  // streamWriter will be assigned by callee.

				this.WriteHtmlFileHeader_CPPC_134
					(
					_enum_CP_PC,
					ref _modSubNodeState1
					);

			}

			//----------------------------------------------------------

			// For CP case: Get the list of Parents for the C-P dictionary item, but
			// only if the T in M-T is a child of any parent table.

			if (Enum_CP_PC_TM_MT .CP == _enum_CP_PC)
			{
				if (this.m_irunPropRead.GetSDictOfChildParents() .ContainsKey
						(_modSubNodeState1.m_valueTableModule_MT .GetTableNameLowercase())
					)
				{
					listTableTable_CPPC = this.m_irunPropRead .GetSDictOfChildParents()
						[_modSubNodeState1.m_valueTableModule_MT .GetTableNameLowercase()];
				}
				else
				{
					return;
				}
			}
			else if (Enum_CP_PC_TM_MT .PC == _enum_CP_PC)
			{
				if (this.m_irunPropRead.GetSDictOfParentChilds() .ContainsKey
						(_modSubNodeState1.m_valueTableModule_MT .GetTableNameLowercase())
					)
				{
					listTableTable_CPPC = this.m_irunPropRead .GetSDictOfParentChilds()
						[_modSubNodeState1.m_valueTableModule_MT .GetTableNameLowercase()];
				}
				else
				{
					return;
				}
			}
			else
			{
				throw new Exception_AxErd
					(
					"AxErd_Error_1841nq4",
					"Enum_CP_PC value is invalid, in method CPPC_124.",
					_enum_CP_PC.ToString(),
					null
					);
			}


			//___________  LOOP, thru T-T pairs.  ________________________________

			for (int ttt=0; ttt < listTableTable_CPPC.Count; ttt++)
			{

				_modSubNodeState1.m_valueTableTable = listTableTable_CPPC[ttt];

				sOneEntry = this.BuildOneEntryFor_CPPC_137_SubNode
					(
					_enum_CP_PC,
					ref _modSubNodeState1
					);

				_modSubNodeState1.m_streamWriter.WriteLine(sOneEntry);
			}

			return;
		}




		private void CreateFileForOutHtmlModule_CPPC_SubNode_133
			(
			Enum_CP_PC_TM_MT _enum_CP_PC,
			ref CModSubNodeState1 _modSubNodeState1  // Assigns .m_streamWriter.  Has CPreviousNextFileSubNodes.
			)
		{
			SysIo.FileStream fileStreamCreate;
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(512);
			//

			// Build the complete file name string.
			sbuilder.Append(this.m_irunPropRead .GetPathToOutputHtmlFiles());  // "C:\Blah\"
			sbuilder.Append(this.m_irunPropRead .GetOutputHtmlFilesCommonPrefixCPPC());  // "Modu"
			sbuilder.Append("-");
			sbuilder.Append(_modSubNodeState1.m_moduleFileOwnerInfo .GetFileNameNode());  // "GenLen"
			sbuilder.Append("-");

			if (_enum_CP_PC == Enum_CP_PC_TM_MT.CP)
			{
				sbuilder.Append(this.m_irunPropRead .GetHtmlFileChildParents());  // "ChildParents"
			}
			else if (_enum_CP_PC == Enum_CP_PC_TM_MT.PC)
			{
				sbuilder.Append(this.m_irunPropRead .GetHtmlFileParentChilds());  // "ParentChilds"
			}
			else
			{
				throw new Exception_AxErd
					(
					"AxErd_Error_1568zw1",
					"In outputter method _033, enum value is neither CP nor PC, thus is invalid in this context.",
					_enum_CP_PC.ToString(),
					null
					);
			}


			sbuilder.Append("-");
			sbuilder.Append(_modSubNodeState1.SubNode_of_LatestCreatedFile_String);  // "11", "12" etc.
			sbuilder.Append(this.m_irunPropRead .GetHtmlFileExtension());


			fileStreamCreate = new SysIo.FileStream
					(
					sbuilder.ToString(),
					SysIo.FileMode.Create,  // .Create means Overwrite if file pre-exists.
					SysIo.FileAccess.Write,
					SysIo.FileShare.None
					);
			_modSubNodeState1.m_streamWriter = new SysIo.StreamWriter(fileStreamCreate);

			_modSubNodeState1.m_previousNextFileSubNodes = this.AscertainPreviousNextSubNodes_CPPC
				(
					_enum_CP_PC,
					_modSubNodeState1.SubNode_of_LatestCreatedFile_String,
					_modSubNodeState1.m_moduleFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces()
				);

			return;
		}



		/// <summary>
		/// Operates only within one .htm file type, such as CP or PC.
		/// And, operates only within one module.
		/// </summary>
		/// <param name="_enum_CP_PC"></param>
		/// <param name="_sNewestSubNode"></param>
		/// <param name="_sModuleNameLowercaseNoSpaces"></param>
		/// <returns>
		/// </returns>
		private CPreviousNextFileSubNodes AscertainPreviousNextSubNodes_CPPC
			(
				Enum_CP_PC_TM_MT _enum_CP_PC,
				string _sNewestCreatedSubNode,  // "11" or "12" etc, must Convert.ToInt32().
				string _sModuleNameLowercaseNoSpaces
			)
		{
			CPreviousNextFileSubNodes previousNextFileSubNodes = new CPreviousNextFileSubNodes(); // to return
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> sdictOf_CPPC = null;
			SysCollGen.List<CValueTableTable> listTableTable;
			CValueTableTable valueTableTable;
			string sDebuggerTableName;
			int nNewestCreatedSubNode, nSubNode_OfLoop = 0, nSubNode_OfPreviousLoop,
				nDebuggerLoopCounter = 0;
			//


			if (Enum_CP_PC_TM_MT .CP == _enum_CP_PC)
			{
				sdictOf_CPPC = this.m_irunPropRead.GetSDictOfChildParents();
			}
			else if (Enum_CP_PC_TM_MT .PC == _enum_CP_PC)
			{
				sdictOf_CPPC = this.m_irunPropRead.GetSDictOfParentChilds();
			}


			nNewestCreatedSubNode = Convert.ToInt32(_sNewestCreatedSubNode);


			//____________________ LOOP _____________________________________


			nSubNode_OfPreviousLoop = 0;


			foreach (SysCollGen.KeyValuePair<string, SysCollGen.List<CValueTableTable>> eachKvp
					in sdictOf_CPPC)
			{
				listTableTable = eachKvp.Value;
				valueTableTable = listTableTable[0];

				if (_sModuleNameLowercaseNoSpaces != valueTableTable.ModuleName_of_LeftTable_LoNoSp)
				{
					continue;
				}

				++nDebuggerLoopCounter; // Within the one target module only.
				sDebuggerTableName = valueTableTable.GetTableNameChild();


				if (Enum_CP_PC_TM_MT .CP == _enum_CP_PC)
				{
					if (nSubNode_OfLoop != Convert.ToInt32(valueTableTable .SubNode_CP_PerModule))
					{
						nSubNode_OfPreviousLoop = nSubNode_OfLoop;
						nSubNode_OfLoop = Convert.ToInt32(valueTableTable .SubNode_CP_PerModule);
					}
				}
				else if (Enum_CP_PC_TM_MT .PC == _enum_CP_PC)
				{
					if (nSubNode_OfLoop != Convert.ToInt32(valueTableTable .SubNode_PC_PerModule))
					{
						nSubNode_OfPreviousLoop = nSubNode_OfLoop;
						nSubNode_OfLoop = Convert.ToInt32(valueTableTable .SubNode_PC_PerModule);
					}
				}


				if (nNewestCreatedSubNode == nSubNode_OfLoop) // Gone passed all lower number subnodes.
				{
					if ("0" == previousNextFileSubNodes.m_sSubNode_Previous)
					{
						previousNextFileSubNodes.m_sSubNode_Previous = nSubNode_OfPreviousLoop.ToString();
					}
				}
				// Gone passed the matching subnode (and passed all lower number subnodes).
				else if (nNewestCreatedSubNode < nSubNode_OfLoop)
				{
					previousNextFileSubNodes.m_sSubNode_Next = nSubNode_OfLoop.ToString();
					break;
				}
			} //EOForeach eachKvp

			return previousNextFileSubNodes;
		}




		private void WriteHtmlFileHeader_CPPC_134
				(
				Enum_CP_PC_TM_MT _enum_CP_PC,
				ref CModSubNodeState1 _modSubNodeState1
				)
		{
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(1024);
			string sPreviousFileName = null, sNextFileName = null;
			string sNavigLinksHtml = "";
			//

			sbuilder.Length = 0;

			sbuilder.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
			sbuilder.AppendLine("<html>");
			sbuilder.AppendLine("<!--");
			sbuilder.AppendLine("\tMicrosoft Dynamics AX 2012 R2, Table ERD and related Application Module info.");
			sbuilder.AppendLine("-->");
			sbuilder.AppendLine("<head>");
			sbuilder.Append("\t<title>AxErd ");

			if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
			{
				sbuilder.Append("CP");
			}
			else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
			{
				sbuilder.Append("PC");
			}

			sbuilder.Append(": ");
			sbuilder.Append(_modSubNodeState1.m_valueTableModule_MT .GetModuleName());
			sbuilder.AppendLine("</title>");
		
			sbuilder.Append("\t<link rel=\"stylesheet\" type=\"text/css\" href=\"");
			sbuilder.Append(M_sCss_Stylesheet_FileName);
			sbuilder.AppendLine("\"/>");


			sbuilder.AppendLine("\t<meta name=\"Ax.Erd.62.All\" content=\"yes\" />");
			sbuilder.Append("\t<meta name=\"Ax.Erd.62.ContentType\" content=\"");
			if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
				{ sbuilder.Append("CP"); }
			else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
				{ sbuilder.Append("PC"); }
			else
				{ sbuilder.Append("All"); } // This line should never be reached.
			sbuilder.AppendLine("\" />");


			sbuilder.AppendLine("</head>");
			sbuilder.AppendLine("<body class=\"cssBackgroundColorBodyCP\">");

			// ?? Here emit href= link to PREVIOUS subnode (within module)!  Already done elsewhere?

			_modSubNodeState1.m_streamWriter.WriteLine(sbuilder.ToString());


			// Print names of modules, if RestrictToModules param was used.
			sbuilder.Length = 0;


			_modSubNodeState1.m_streamWriter.WriteLine("<h4>Microsoft Dynamics AX 2012 R2</h4>");
			sbuilder.Append("<h2 class=\"cssH3Color");

			if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
			{
				sbuilder.Append("CP");
			}
			else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
			{
				sbuilder.Append("PC");
			}

			sbuilder.Append("\">AxErd: ");

			if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
			{
				sbuilder.Append("Child-Parents");
			}
			else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
			{
				sbuilder.Append("Parent-Children");
			}

			sbuilder.Append(": ");



			sbuilder.Append(_modSubNodeState1.m_valueTableModule_MT .GetModuleName());
			sbuilder.Append("</h2>");
			_modSubNodeState1.m_streamWriter.WriteLine(sbuilder.ToString());
			_modSubNodeState1.m_streamWriter.WriteLine();

			// Module names as inter-webpage links, if /RestrictToModulesCPPC param was used.
			// (Later: it must contain exactly one, so yes is used.)
			// See this.m_irunPropRead.GetRestrictToModulesCPPC();

			// ?? Want better CSS formatting here! _modSubNodeState1.m_streamWriter.WriteLine("<pre>");
			_modSubNodeState1.m_streamWriter.WriteLine();

			sbuilder.Length = 0;
			sbuilder.Append("<a class=\"cssAHref2\" href=\"");
			sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT());
			sbuilder.Append(this.m_irunPropRead.GetHtmlFileModuleTables());
			sbuilder.Append("-");
			sbuilder.Append(_modSubNodeState1.m_moduleFileOwnerInfo.GetFileNameNode()); // Module fileNameNode string.
			sbuilder.Append("-11"); // SubNode, hardcoding of first subnode.
			sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension());
			sbuilder.Append("#m-");
			sbuilder.Append(_modSubNodeState1.m_valueTableModule_MT .GetModuleNameLowercaseNoSpaces());
			sbuilder.Append("\">Module: ");
			sbuilder.Append(_modSubNodeState1.m_valueTableModule_MT .GetModuleName());
			sbuilder.Append("</a>");

			_modSubNodeState1.m_streamWriter.WriteLine(sbuilder.ToString());

//??
			// Build sPreviousFileName (for Navigation).
			//
			if ("0" != _modSubNodeState1.m_previousNextFileSubNodes.m_sSubNode_Previous) // Previous.  First file subnode is 11.
			{
				sbuilder.Length = 0;
				sbuilder.Append(this.m_irunPropRead .GetOutputHtmlFilesCommonPrefixCPPC());  // "Fky"
				sbuilder.Append("-");
				sbuilder.Append(_modSubNodeState1.m_moduleFileOwnerInfo .GetFileNameNode());  // "GenLen"
				sbuilder.Append("-");
				if (_enum_CP_PC == Enum_CP_PC_TM_MT.CP)
				{
					sbuilder.Append(this.m_irunPropRead .GetHtmlFileChildParents());  // "ChildParents"
				}
				else if (_enum_CP_PC == Enum_CP_PC_TM_MT.PC)
				{
					sbuilder.Append(this.m_irunPropRead .GetHtmlFileParentChilds());  // "ParentChilds"
				}
				sbuilder.Append("-");
				sbuilder.Append(_modSubNodeState1.m_previousNextFileSubNodes.m_sSubNode_Previous); // Previous
				sbuilder.Append(this.m_irunPropRead .GetHtmlFileExtension());

				sPreviousFileName = sbuilder.ToString();
			}


			// Build sNextFileName.
			//
			if ("0" != _modSubNodeState1.m_previousNextFileSubNodes.m_sSubNode_Next) // Next
			{
				sbuilder.Length = 0;
				sbuilder.Append(this.m_irunPropRead .GetOutputHtmlFilesCommonPrefixCPPC());  // "Fky"
				sbuilder.Append("-");
				sbuilder.Append(_modSubNodeState1.m_moduleFileOwnerInfo .GetFileNameNode());  // "GenLen"
				sbuilder.Append("-");
				if (_enum_CP_PC == Enum_CP_PC_TM_MT.CP)
				{
					sbuilder.Append(this.m_irunPropRead .GetHtmlFileChildParents());  // "ChildParents"
				}
				else if (_enum_CP_PC == Enum_CP_PC_TM_MT.PC)
				{
					sbuilder.Append(this.m_irunPropRead .GetHtmlFileParentChilds());  // "ParentChilds"
				}
				sbuilder.Append("-");
				sbuilder.Append(_modSubNodeState1.m_previousNextFileSubNodes.m_sSubNode_Next); // Next
				sbuilder.Append(this.m_irunPropRead .GetHtmlFileExtension());

				sNextFileName = sbuilder.ToString();
			}


			sNavigLinksHtml = this.WritePreviousNextNavigationsAtFileTopOrBottom
				(
				 sPreviousFileName
				,sNextFileName
				,true
				,true
				);


			_modSubNodeState1.m_streamWriter.WriteLine();
			_modSubNodeState1.m_streamWriter.WriteLine(sNavigLinksHtml);
			_modSubNodeState1.m_streamWriter.WriteLine();


			if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
			{
				// Output column headers, CP.

				sbuilder.Length = 0;
				sbuilder.Append(Environment.NewLine);
				sbuilder.AppendLine("<br />");
				sbuilder.Append("<table id=\"idTabMainTest2\">");
				sbuilder.Append(Environment.NewLine);
				sbuilder.AppendLine("<thead>");
				sbuilder.AppendLine("<tr class=\"cssFontForMainColumnHeaders\">");
				sbuilder.Append("    <th>Row-num</th>");

				sbuilder.Append(" <th>Child-table-name</th>");
				sbuilder.Append(" <th>Foreign-key-columns</th>");
				sbuilder.Append(" <th>Module-of-child-table</th>");
		
				sbuilder.Append(" <th>Arrow-to-parent</th>");
				
				sbuilder.Append(" <th>Parent-table-name</th>");
				sbuilder.Append(" <th>Primary-key-columns</th>");
				sbuilder.Append(" <th>Module-of-parent-table</th>");
				sbuilder.Append(Environment.NewLine);
				sbuilder.AppendLine("</tr>");
				sbuilder.AppendLine("</thead>");
				sbuilder.AppendLine("<tbody>");
			}
			else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
			{
				// Output column headers, CP.

				sbuilder.Length = 0;
				sbuilder.Append(Environment.NewLine);
				sbuilder.AppendLine("<br />");
				sbuilder.Append("<table id=\"idTabMainTest2\">");
				sbuilder.Append(Environment.NewLine);
				sbuilder.AppendLine("<thead>");
				sbuilder.Append("<tr class=\"cssFontForMainColumnHeaders\"><th>Row-num</th>");

				sbuilder.Append("<th>Parent-table-name</th>");
				sbuilder.Append("<th>Foreign-key-columns</th>");
				sbuilder.Append("<th>Module-of-parent-table</th>");
		
				sbuilder.Append("<th>Arrow-to-parent</th>");
				
				sbuilder.Append("<th>Child-table-name</th>");
				sbuilder.Append("<th>Primary-key-columns</th>");
				sbuilder.Append("<th>Module-of-child-table</th>");
				sbuilder.Append(Environment.NewLine);
				sbuilder.AppendLine("</tr>");
				sbuilder.AppendLine("</thead>");
				sbuilder.AppendLine("<tbody>");
			}

			_modSubNodeState1.m_streamWriter.WriteLine(sbuilder.ToString());

			return;
		}




		private string BuildOneEntryFor_CPPC_137_SubNode
				(
				Enum_CP_PC_TM_MT _enum_CP_PC,
				ref CModSubNodeState1 _modSubNodeState1
				)
		{
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> sdictOfChildParents;
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> sdictOfParentChilds;
			SysCollGen.SortedDictionary<int, SysCollGen.List<CValueColumnColumn>> sdictOfColumnsFkyPky;
			SysCollGen.SortedDictionary<string, CValueTableModule> sdictOfTableModules;
			SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> sdictOfModuleName_to_FileNameNodes;
			//
			SysCollGen.List<CValueColumnColumn> listColumnColumn;
			//
			CValueTableModule valueTableModule;
			CModuleFileOwnerInfo moduleFileOwnerInfo;
			//
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(1028);
			//
			string sCPPC_Left_TableName_Lowercase = null,
				sCPPC_Left_TableName = null;
			string sCPPC_Right_TableName_Lowercase = null,
				sCPPC_Right_TableName = null;
			string sCPPC_Left_AnchorName_OneCharPrefix_Jumpee = "p",
				sCPPC_AnchorName_OneCharPrefix_Jumper = "c";
			string sCPPC_PseudoArrow = "---";
			//
			int nLoopColumnsCounter;
			//
			bool boolOther_CPPC_ContainsKey = false;
			//___________________________________________

			// Handier.
			sdictOfChildParents = this.m_irunPropRead.GetSDictOfChildParents();
			sdictOfParentChilds = this.m_irunPropRead.GetSDictOfParentChilds();
			sdictOfColumnsFkyPky = this.m_irunPropRead.GetSDictOfColumnsFkyPky();
			sdictOfTableModules = this.m_irunPropRead.GetSDictOfTableModules();
			sdictOfModuleName_to_FileNameNodes = this.m_irunPropRead.GetSDictOfModuleName_to_FileNameNodes();


			// Assign generic variables.

			// ARROW shall point from Child to Parent, to match Visio .ERX import automation, consistency across tools.
			if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
			{
				sCPPC_Left_TableName_Lowercase = _modSubNodeState1.m_valueTableTable
					.GetTableNameChildLowercase();
				sCPPC_Left_TableName = _modSubNodeState1.m_valueTableTable .GetTableNameChild();

				sCPPC_Right_TableName_Lowercase = _modSubNodeState1.m_valueTableTable
					.GetTableNameParentLowercase();
				sCPPC_Right_TableName = _modSubNodeState1.m_valueTableTable .GetTableNameParent();

				sCPPC_Left_AnchorName_OneCharPrefix_Jumpee = "c";
				sCPPC_AnchorName_OneCharPrefix_Jumper = "p";

				sCPPC_PseudoArrow = "<td>&nbsp;&nbsp;&nbsp;&nbsp;&#x2011;&#x2011;&#x2011;&gt;&nbsp;&nbsp;&nbsp;&nbsp;</td>"; // &gt; --->
			}
			else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
			{
				sCPPC_Left_TableName_Lowercase = _modSubNodeState1.m_valueTableTable
					.GetTableNameParentLowercase();
				sCPPC_Left_TableName = _modSubNodeState1.m_valueTableTable .GetTableNameParent();

				sCPPC_Right_TableName_Lowercase = _modSubNodeState1.m_valueTableTable
					.GetTableNameChildLowercase();
				sCPPC_Right_TableName = _modSubNodeState1.m_valueTableTable .GetTableNameChild();

				// _Jumpee means that inside the PC.htm files, the <a name="p-" letter
				// (here 'p') must be used, whereas 'c' is for inside the CP.htm files. 
				sCPPC_Left_AnchorName_OneCharPrefix_Jumpee = "p";
				sCPPC_AnchorName_OneCharPrefix_Jumper = "c";

				sCPPC_PseudoArrow = "<td>&nbsp;&nbsp;&nbsp;&nbsp;&lt;&#x2011;&#x2011;&#x2011;&nbsp;&nbsp;&nbsp;&nbsp;</td>"; // &lt; <---
			}


			#region Left table, CP

			//____________________________________________________________________
			//____________________________________________________________________

			// Left - Child_______________________________________________________


			// Emit: (Left) Line number.____________________________

			sbuilder.Append(Environment.NewLine);
			sbuilder.Append("<tr><td>");
			sbuilder.Append(Convert.ToString(++ _modSubNodeState1 .m_nCountEntriesWrittenForCurrentModule));
			sbuilder.Append("</td>");


			// Emit:  Left-Child name, its jumpTo <a name= anchor.________________

			sbuilder.Append("<td>");

			if (_modSubNodeState1.m_sLatestLeftAnchorTableNameLowercase !=
					_modSubNodeState1.m_valueTableModule_MT .GetTableNameLowercase()
				)
			{
				sbuilder.Append("<a name=\"");
				sbuilder.Append(sCPPC_Left_AnchorName_OneCharPrefix_Jumpee); // 'c' or 'p'
				sbuilder.Append("-");

				sbuilder.Append(_modSubNodeState1.m_valueTableModule_MT .GetTableNameLowercase());
				sbuilder.Append("\"></a>");

				_modSubNodeState1.m_sLatestLeftAnchorTableNameLowercase =
					_modSubNodeState1.m_valueTableModule_MT .GetTableNameLowercase();
			}

			// Emit:  Left-Child name,
			// and as a jumpFrom link (if appropriate).________________________

			// For the Left-Child column on the HTML webpage,
			// emit the child table name as an inter-webpage link to where the table is a parent,
			// but only if the table really is also a parent in another relationship.
			// Else emit the child table's name as plain nonLink text.

			// For the LEFT side of the relationship arrow.
			boolOther_CPPC_ContainsKey = false;

			if (Enum_CP_PC_TM_MT .CP == _enum_CP_PC) // In PC .htm, the table links jumpTo CP .htm.
			{
				boolOther_CPPC_ContainsKey = sdictOfParentChilds
					.ContainsKey(sCPPC_Left_TableName_Lowercase);
			}
			else if (Enum_CP_PC_TM_MT .PC == _enum_CP_PC)
			{
				boolOther_CPPC_ContainsKey = sdictOfChildParents
					.ContainsKey(sCPPC_Left_TableName_Lowercase);
			}

			if (boolOther_CPPC_ContainsKey)
			{
				sbuilder.Append("<a class=\"cssAHref2\" href=\"");
				sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixCPPC()); // "Modu"

				try
				{
					if (sdictOfTableModules.TryGetValue(
								sCPPC_Left_TableName_Lowercase,
								out valueTableModule)
						) //!CP
					{
						sbuilder.Append("-");

						moduleFileOwnerInfo = sdictOfModuleName_to_FileNameNodes
							[valueTableModule.GetModuleNameLowercaseNoSpaces()];
						sbuilder.Append(moduleFileOwnerInfo.GetFileNameNode()); // "GenLed"
					}
				}
				catch (Exception_AxErd eeax)
				{
					Console.WriteLine();
					Console.WriteLine(eeax);
					Console.WriteLine();
					Console.WriteLine("AxErd-Error-8527bs1: File node for C table in CP, module lookup failed.");
					Console.WriteLine();

					eeax.IsAlreadyCaughtAndProcessed = true; // 'false' would be a rare choice for Set.
					throw eeax;
				}
				catch (Exception ee)
				{
					Console.WriteLine();
					Console.WriteLine(ee);
					Console.WriteLine();
					Console.WriteLine("AxErd-Error-8527bt3: File node for C table in CP, module lookup failed.");
					Console.WriteLine();

					throw ee;
				}

				sbuilder.Append("-");

				if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
				{
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileParentChilds()); // "ParentChilds" (not yet include ".htm")
				}
				else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
				{
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileChildParents());
				}

				try
				{
					// Include Module SubNode in file name.
					sbuilder.Append("-");
					valueTableModule = sdictOfTableModules[sCPPC_Left_TableName_Lowercase]; //!CP

					if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
					{
						sbuilder.Append(valueTableModule .SubNode_PC_PerModule); // "11"
					}
					else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
					{
						sbuilder.Append(valueTableModule .SubNode_CP_PerModule);
					}

				}
				catch (Exception ee)
				{
					Console.WriteLine(ee);
					Console.WriteLine("AXERD-Error-1490ur6: " +
						_modSubNodeState1.ToString_ForDiagnostics());
				}

				sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"
				// Thus built substring - "Modu-GenLed-ParentChilds-11.htm" (that total format, before #).

				sbuilder.Append("#");
				sbuilder.Append(sCPPC_AnchorName_OneCharPrefix_Jumper);
				sbuilder.Append("-");

				sbuilder.Append(sCPPC_Left_TableName_Lowercase); //!CP
				sbuilder.Append("\">");
				sbuilder.Append(sCPPC_Left_TableName); //!CP
				sbuilder.Append("</a>");
			}
			else
			{
				sbuilder.Append(sCPPC_Left_TableName); //!CP
			}
			sbuilder.Append("</td>");


			// Emit: Left, Columns, Fky (or Pky).____________________________

			sbuilder.Append(Environment.NewLine);
			sbuilder.Append("<td>");

			nLoopColumnsCounter = 0;
			listColumnColumn = sdictOfColumnsFkyPky[_modSubNodeState1.m_valueTableTable .GetIdentityTT()];

			foreach (CValueColumnColumn valueColColFkyPkyFE in listColumnColumn)
			{
				if (0 != nLoopColumnsCounter++)
					{ sbuilder.Append(", "); }
				sbuilder.Append(".");

				if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
				{
					sbuilder.Append(valueColColFkyPkyFE .GetFieldFky()); //!CP
				}
				else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
				{
					sbuilder.Append(valueColColFkyPkyFE.GetFieldRelPky());
				}
			}
			sbuilder.Append("</td>");


			// Emit: Application Module, for Child._______________

			try
			{
				if (sdictOfTableModules.ContainsKey
						(sCPPC_Left_TableName_Lowercase) //!CP
					)
				{
					// Gather data.
					valueTableModule = sdictOfTableModules
						[sCPPC_Left_TableName_Lowercase]; //!CP
				}
				else
				{
					valueTableModule = null;
				}
			}
			catch (Exception ee)
			{
				Console.WriteLine(_modSubNodeState1.ToString_ForDiagnostics());
				throw ee;
			}

			sbuilder.Append(Environment.NewLine);
			sbuilder.Append("<td><span class=\"css2ModuleLink\">");
			if (null != valueTableModule)
			{
				sbuilder.Append("<a class=\"cssAHref2\" href=\"");
				sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT()); // ""
				sbuilder.Append(this.m_irunPropRead.GetHtmlFileModuleTables()); // "Module"
				sbuilder.Append("-");

				moduleFileOwnerInfo = sdictOfModuleName_to_FileNameNodes
					[valueTableModule.GetModuleNameLowercaseNoSpaces()];
				sbuilder.Append(moduleFileOwnerInfo.GetFileNameNode());

				sbuilder.Append("-");
				sbuilder.Append("11");

				sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"

				sbuilder.Append("\">(");
				sbuilder.Append(valueTableModule.GetModuleName());
				sbuilder.Append(")</a>");
			}
			else
			{
				sbuilder.Append(CRunProperties.M_readonly_sUnknownModule);
			}
			sbuilder.Append("</span></td>");


			// Emit: Arrow._________________________________

			sbuilder.AppendLine(); //(Environment.NewLine);

			sbuilder.Append(sCPPC_PseudoArrow);

			#endregion // Left table, CP
			#region Right table, CP

			//____________________________________________________________________
			//____________________________________________________________________

			// Right - Parent_____________________________________________________


			// Emit:  Right-Parent name.__________________________________________

			sbuilder.Append("<td>");


			// Emit:  Right-Parent name,
			// and as a jumpFrom link (if appropriate).________________________

			// For the Right-Parent column on the HTML webpage,
			// emit the parent table name as an inter-webpage link to where the table is a parent,
			// but only if the table really is also a parent in another relationship.
			// Else emit the parent table's name as plain nonLink text.

			// For the RIGHT side of the relationship arrow.
			boolOther_CPPC_ContainsKey = false;

			if (Enum_CP_PC_TM_MT .CP == _enum_CP_PC)
			{
				boolOther_CPPC_ContainsKey = sdictOfParentChilds
					.ContainsKey(sCPPC_Right_TableName_Lowercase);
			}
			else if (Enum_CP_PC_TM_MT .PC == _enum_CP_PC)
			{
				boolOther_CPPC_ContainsKey = sdictOfChildParents
					.ContainsKey(sCPPC_Right_TableName_Lowercase);
			}

			if (boolOther_CPPC_ContainsKey)
			{
				sbuilder.Append("<a class=\"cssAHref2\" href=\"");
				sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixCPPC()); // "Modu"

				try
				{
					if (sdictOfTableModules.TryGetValue
							(sCPPC_Right_TableName_Lowercase, //!CP //!Right-side
							out valueTableModule)
						)
					{
						sbuilder.Append("-");

						moduleFileOwnerInfo = sdictOfModuleName_to_FileNameNodes
							[valueTableModule.GetModuleNameLowercaseNoSpaces()];
						sbuilder.Append(moduleFileOwnerInfo.GetFileNameNode()); // "GenLed"
					}
				}
				catch (Exception_AxErd eeax)
				{
					Console.WriteLine();
					Console.WriteLine(eeax);
					Console.WriteLine();
					Console.WriteLine("AxErd-Error-8537bt2: File node for P table in PC, module lookup failed."); //!Right-side
					Console.WriteLine();

					eeax.IsAlreadyCaughtAndProcessed = true; // 'false' would be a rare choice for Set.
					throw eeax;
				}
				catch (Exception ee)
				{
					Console.WriteLine();
					Console.WriteLine(ee);
					Console.WriteLine();
					Console.WriteLine("AxErd-Error-8537bv4: File node for P table in PC, module lookup failed."); //!Right-side
					Console.WriteLine();

					throw ee;
				}

				sbuilder.Append("-");

				if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
				{
					// "ParentChilds" (not yet include ".htm")
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileParentChilds()); //!CP
				}
				else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
				{
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileChildParents());
				}

				try
				{
					// Include Module SubNode in file name.
					sbuilder.Append("-");
					valueTableModule = sdictOfTableModules
						[sCPPC_Right_TableName_Lowercase];

					if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
					{
						sbuilder.Append(valueTableModule .SubNode_PC_PerModule); // "11"
					}
					else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
					{
						sbuilder.Append(valueTableModule .SubNode_CP_PerModule);
					}

				}
				catch (Exception ee)
				{
					Console.WriteLine(ee);
					Console.WriteLine("AXERD-Error-1491yr6: " + sCPPC_Right_TableName_Lowercase); //!CP
				}

				sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"

				sbuilder.Append("#");
				sbuilder.Append(sCPPC_AnchorName_OneCharPrefix_Jumper);
				sbuilder.Append("-");

				sbuilder.Append(sCPPC_Right_TableName_Lowercase); //!CP
				sbuilder.Append("\">");
				sbuilder.Append(sCPPC_Right_TableName); //!CP
				sbuilder.Append("</a>");
			}
			else
			{
				sbuilder.Append(sCPPC_Right_TableName); //!CP
			}
			sbuilder.Append("</td>");


			// Emit: Right, Columns, (Fky or) Pky.____________________________

			sbuilder.Append(Environment.NewLine);
			sbuilder.Append("<td>");

			nLoopColumnsCounter = 0;
			listColumnColumn = sdictOfColumnsFkyPky
				[_modSubNodeState1.m_valueTableTable .GetIdentityTT()];

			foreach (CValueColumnColumn valueColColFkyPkyFE in listColumnColumn)
			{
				if (0 != nLoopColumnsCounter++)
					{ sbuilder.Append(", "); }

				sbuilder.Append(".");

				if (Enum_CP_PC_TM_MT.CP == _enum_CP_PC)
				{
					sbuilder.Append(valueColColFkyPkyFE.GetFieldRelPky());
				}
				else if (Enum_CP_PC_TM_MT.PC == _enum_CP_PC)
				{
					sbuilder.Append(valueColColFkyPkyFE .GetFieldFky()); //!CP
				}
			}
			sbuilder.Append("</td>");


			// Emit: Application Module, for Parent._______________  //!Right-side

			try
			{
				if (sdictOfTableModules.ContainsKey
						(sCPPC_Right_TableName_Lowercase) //!CP
					)
				{
					// Gather data.
					valueTableModule = sdictOfTableModules
						[sCPPC_Right_TableName_Lowercase]; //!CP
				}
				else
				{
					valueTableModule = null;
				}
			}
			catch (Exception ee)
			{
				Console.WriteLine(_modSubNodeState1.ToString_ForDiagnostics());
				throw ee;
			}

			sbuilder.Append(Environment.NewLine);
			sbuilder.Append("<td><span class=\"css2ModuleLink\">");
			if (null != valueTableModule)
			{
				sbuilder.Append("<a class=\"cssAHref2\" href=\"");
				sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT()); // ""
				sbuilder.Append(this.m_irunPropRead.GetHtmlFileModuleTables()); // "Module"
				sbuilder.Append("-");

				moduleFileOwnerInfo = sdictOfModuleName_to_FileNameNodes
					[valueTableModule.GetModuleNameLowercaseNoSpaces()];
				sbuilder.Append(moduleFileOwnerInfo.GetFileNameNode());

				sbuilder.Append("-");
				sbuilder.Append("11");

				sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension());

				sbuilder.Append("\">(");
				sbuilder.Append(valueTableModule.GetModuleName());
				sbuilder.Append(")</a>");
			}
			else
			{
				sbuilder.Append(CRunProperties.M_readonly_sUnknownModule);
			}
			sbuilder.Append("</span></td>");
			sbuilder.AppendLine("</tr>");
			sbuilder.AppendLine();

			#endregion // Right table, CP

			return sbuilder.ToString();
		}



//__________________________________________________________________________________________________________
//__________________________________________________________________________________________________________
//  T-M



		private void CreateHtmlFileTableModules()
		{
			SysIo.StreamWriter streamWriter = null;
			SysCollGen.SortedDictionary<string, CValueTableModule> sdictOfTableModules;
			SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> sdictOfModuleName_to_FileNameNodes;
			// ?? SysCollGen.List<CValueTableModule> listTableModule;
			SysCollGen.List<CValueTableTable> listTableTable;
			CModuleFileOwnerInfo moduFileOwnerInfo;
			CValueTableModule valueTableModule_TM = null, valueTableModule_TM_Previous;
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(256);
			char[] charArraySpace = new char[1] { ' ' };  // One space character, only.
			string[] sAryModParamsCP;
			string s2TableNameLowercase;
			int nLoopCount = 0, nOutputLineNumber = 0;
			bool boolModuleMatchFound;
			//

			if (true == this.m_irunPropRead.GetSkipOutputOfFilesTM())
			{
				goto LABEL_ENDOFMETHOD_nte72_LABEL;
			}

			sdictOfModuleName_to_FileNameNodes = this.m_irunPropRead.GetSDictOfModuleName_to_FileNameNodes();


			// Next, file data driven lines.

			sdictOfTableModules = this.m_irunPropRead.GetSDictOfTableModules();


			//____________ LOOP, all sdict _TM .Value items. ________________________________


			foreach (SysCollGen.KeyValuePair<string, CValueTableModule> kvpTM in sdictOfTableModules)
			{
				valueTableModule_TM_Previous = valueTableModule_TM;

				valueTableModule_TM = kvpTM.Value;
				nLoopCount++; // Unused, just maybe helpful for debugging.

				//----------------------
				// Is the module of the present record filtered out by the restrict to modules parameter?

				boolModuleMatchFound = false;  // Assume this module should be skipped.
				sAryModParamsCP = this.m_irunPropRead.GetRestrictToModulesTMMT();
				if (null == sAryModParamsCP || 0 == sAryModParamsCP.Length)
				{
					boolModuleMatchFound = true;  // Not literally true, but equivalent.
				}
				else
				{
					foreach (string sMod in sAryModParamsCP)
					{
						if (valueTableModule_TM.GetModuleNameLowercase() == sMod.ToLower())
						{
							boolModuleMatchFound = true;
							break;
						}
					}
				}
				if (!boolModuleMatchFound)
				{
					continue;  // Thus skipping the current module record, and not outputting it to HTML.
				}
				//----------------------


				// Next part is driven by the data derived earlier from the input delimited files.
				sbuilder.Length = 0;

				// Table

				s2TableNameLowercase = valueTableModule_TM.GetTableNameLowercase();

				sbuilder.AppendLine();
				sbuilder.Append("<tr><td>");
				sbuilder.Append(Convert.ToString(++nOutputLineNumber));
				sbuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;</td>");

				sbuilder.Append("<td><a name=\"t-");
				sbuilder.Append(s2TableNameLowercase);
				sbuilder.Append("\"></a><a class=\"cssAHref2\" href=\"");
				sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixCPPC()); // "Modu"

				sbuilder.Append("-");
				if (sdictOfModuleName_to_FileNameNodes.TryGetValue
						(valueTableModule_TM.GetModuleNameLowercaseNoSpaces(), out moduFileOwnerInfo)
					)
				{
					sbuilder.Append(moduFileOwnerInfo.GetFileNameNode()); // "GenLed"
				}
				else
				{
					sbuilder.Append(CRunProperties.M_readonly_sUnknownModule);
				}

				sbuilder.Append("-");
				// Give preference to linking to PC (over CP), with # link to the specific table.
				if (this.m_irunPropRead.GetSDictOfParentChilds().TryGetValue
						(s2TableNameLowercase, out listTableTable)
					)
				{
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileParentChilds()); // "ParentChilds"

					sbuilder.Append("-");
					sbuilder.Append(listTableTable[0].SubNode_PC_PerModule);

					sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"

					sbuilder.Append("#p-");
					sbuilder.Append(s2TableNameLowercase);
				}
				// Next best option is to...
				// ...Give preference to linking to CP, with # link to the specific table.
				else if (this.m_irunPropRead.GetSDictOfChildParents().TryGetValue
						(s2TableNameLowercase, out listTableTable)
					)
				{
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileChildParents());

					sbuilder.Append("-");
					sbuilder.Append(listTableTable[0].SubNode_CP_PerModule);

					sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"

					sbuilder.Append("#c-");
					sbuilder.Append(s2TableNameLowercase);
				}
				// Last option is to link to top of first PC subnode file (meaning "11").
				else
				{
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileParentChilds()); // "ParentChilds"

					sbuilder.Append("-");
					sbuilder.Append("11");

					sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"
				}

				sbuilder.Append("\">");
				sbuilder.Append(valueTableModule_TM.GetTableName());
				sbuilder.Append("</a></td>");

				// Module
				sbuilder.Append(Environment.NewLine);
				sbuilder.Append("<td>&nbsp;&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;</td>");
				sbuilder.Append("<td></a>   <a class=\"cssAHref2\" href=\"");
				sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT());
				sbuilder.Append(this.m_irunPropRead.GetHtmlFileModuleTables());

				sbuilder.Append("-");
				sbuilder.Append
					(
					sdictOfModuleName_to_FileNameNodes
						[valueTableModule_TM.GetModuleNameLowercaseNoSpaces()]
							.GetFileNameNode()
					);

				// Simply jump to the first subnode file for the MT .htm files.
				sbuilder.Append("-");
				sbuilder.Append("11");

				sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension());

				// Maybe the # blah is unneeded here, as a practical matter,
				// just as subnode 11 is hardcoded shortly above.
				//sbuilder.Append("#m-");
				//sbuilder.Append(valueTableModule_TM.GetModuleNameLowercaseNoSpaces());

				sbuilder.Append("\">");
				sbuilder.Append(valueTableModule_TM.GetModuleName());
				sbuilder.Append("</a></td></tr>");


				if (
						(null == valueTableModule_TM_Previous)
								||
						(valueTableModule_TM .SubNode_TM_All !=
						valueTableModule_TM_Previous .SubNode_TM_All)
					)
				{
					this.ManageStreamWriter_TM
						(
						valueTableModule_TM .SubNode_TM_All,
						ref streamWriter
						);
				}

				streamWriter.WriteLine(sbuilder.ToString());

			} // EOFor Loop, SortedDict TM.


			streamWriter.WriteLine("</tbody>"); // ??????_thead
			streamWriter.WriteLine("</table>");
			streamWriter.WriteLine("</body>");
			streamWriter.WriteLine("</html>");

			streamWriter.Flush();
			streamWriter.Close();
			streamWriter.Dispose();
			streamWriter = null;

		LABEL_ENDOFMETHOD_nte72_LABEL: ;
			return;
		}




		private void ManageStreamWriter_TM
			(
			string _sSubNode_TM_All,
			ref SysIo.StreamWriter _streamWriter_ref
			)
		{
			SysIo.FileStream fileStream;
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(512);
			string sHtmlNavigLinks, sPreviousFileName, sNextFileName;
			int nSubNode_TM_All;
			//

			nSubNode_TM_All = Convert.ToInt32(_sSubNode_TM_All);


			if (null != _streamWriter_ref)
			{
				// Write the at-bottom Next-File link.
				//if (nSubNode_TM_All >= MAX ???? TODO) // Next  OR, MANUALLY fix the last TM bottom-of-file 'NextFile' link (remove it)! Yes, manually fix.
				//{
					_streamWriter_ref.WriteLine("</tbody>");
					_streamWriter_ref.WriteLine("</table>");
					_streamWriter_ref.WriteLine();

					sbuilder.Length = 0;
					//sbuilder.AppendLine();
					//????? sbuilder.AppendLine("<br />");
					//sbuilder.Append("<a href=\"");

					// Build the complete file name string.
					//??sbuilder.Append(this.m_irunPropRead .GetPathToOutputHtmlFiles());  // "C:\Blah\"
					sbuilder.Append(this.m_irunPropRead .GetOutputHtmlFilesCommonPrefixTMMT());
					sbuilder.Append(this.m_irunPropRead .GetHtmlFileTableModules()); // "All-TableModule"
					sbuilder.Append("-");
					sbuilder.Append((nSubNode_TM_All).ToString());
					sbuilder.Append(this.m_irunPropRead .GetHtmlFileExtension());
					//????? sbuilder.AppendLine("\">Next File</a>"); // Next
			
					sNextFileName = sbuilder.ToString();
					sHtmlNavigLinks = this.WritePreviousNextNavigationsAtFileTopOrBottom
						(
						 null               // "Previous File" filename, for an <a href= link; or null.
						,sNextFileName      // "Next File" filename, for an <a href= link.
						,true               // "Navigation:" literal is wanted?
						,false              // Link to Default.htm (Home) is wanted?
						);

				
					_streamWriter_ref.WriteLine("<br />");
					_streamWriter_ref.WriteLine(sHtmlNavigLinks);
					_streamWriter_ref.WriteLine("<br />");
				//}

				_streamWriter_ref.WriteLine("<br />");
				_streamWriter_ref.WriteLine("</body>");
				_streamWriter_ref.WriteLine("</html>");

				_streamWriter_ref.Flush();
				_streamWriter_ref.Close();
				_streamWriter_ref.Dispose();
				_streamWriter_ref = null;
			}

			sbuilder.Length = 0;
			sbuilder.Append(this.m_irunPropRead.GetPathToOutputHtmlFiles());
			sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT()); // ""
			sbuilder.Append(this.m_irunPropRead.GetHtmlFileTableModules()); // "All-TableModule"
			sbuilder.Append("-");
			sbuilder.Append(_sSubNode_TM_All);
			sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"

			fileStream = new SysIo.FileStream
					(
					sbuilder.ToString(),
					SysIo.FileMode.Create,  // .Create means Overwrite if file pre-exists.
					SysIo.FileAccess.Write,
					SysIo.FileShare.None
					);
			_streamWriter_ref = new SysIo.StreamWriter(fileStream);

			_streamWriter_ref.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");

			_streamWriter_ref.WriteLine("<html>");
			_streamWriter_ref.WriteLine("<!--");
			_streamWriter_ref.WriteLine("\tMicrosoft Dynamics AX 2012 R2, Table ERD and related Application Module info.");
			_streamWriter_ref.WriteLine("-->");
			_streamWriter_ref.WriteLine("<head>");
			_streamWriter_ref.WriteLine("\t<title>AxErd TM</title>");

			_streamWriter_ref.Write("\t<link rel=\"stylesheet\" type=\"text/css\" href=\"");
			_streamWriter_ref.Write(COutputterToHtmlFiles.M_sCss_Stylesheet_FileName);
			_streamWriter_ref.WriteLine("\"/>");

			_streamWriter_ref.WriteLine("\t<meta name=\"Ax.Erd.62.All\" content=\"yes\" />");
			_streamWriter_ref.WriteLine("\t<meta name=\"Ax.Erd.62.ContentType\" content=\"TM\" />");

			_streamWriter_ref.WriteLine("</head>");
			_streamWriter_ref.WriteLine("<body class=\"cssBackgroundColorBodyTM\">");
			_streamWriter_ref.WriteLine("<h4>Microsoft Dynamics AX 2012 R2</h4>");
			_streamWriter_ref.WriteLine("<h2 class=\"cssH3ColorTM\">AxErd: Tables Listed Alphabetically</h2>");

			_streamWriter_ref.WriteLine();

			
			//????? _streamWriter_ref.WriteLine("<a href=\"Default.htm\">AxErd Home</a>");

			// Write the Previous-File link.
			sPreviousFileName = null;
			if (11 < nSubNode_TM_All)
			{
				sbuilder.Length = 0;

				//????? sbuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
				//sbuilder.Append("<a href=\"");

				// Build the complete file name string.
				//sbuilder.Append(this.m_irunPropRead .GetPathToOutputHtmlFiles());  // "C:\Blah\"
				sbuilder.Append(this.m_irunPropRead .GetOutputHtmlFilesCommonPrefixTMMT());
				sbuilder.Append(this.m_irunPropRead .GetHtmlFileTableModules()); // "All-TableModule"
				sbuilder.Append("-");
				sbuilder.Append((nSubNode_TM_All - 1).ToString()); // Subtraction math means Previous.
				sbuilder.Append(this.m_irunPropRead .GetHtmlFileExtension());
				//????? sbuilder.AppendLine("\">Previous File</a>"); // Previous

				sPreviousFileName = sbuilder.ToString();

				//????? _streamWriter_ref.WriteLine(sbuilder.ToString());

			}


			sNextFileName = null;
			if (nSubNode_TM_All < this.m_irunPropRead.GetHighestSubnode_TM_AsInt())  // All, not per module.
			{
				sbuilder.Length = 0;

				//sbuilder.AppendLine();
				//????? sbuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
				//sbuilder.Append("<a href=\"");

				// Build the complete file name string.
				sbuilder.Append(this.m_irunPropRead .GetOutputHtmlFilesCommonPrefixTMMT());
				sbuilder.Append(this.m_irunPropRead .GetHtmlFileTableModules()); // "All-TableModule"
				sbuilder.Append("-");
				sbuilder.Append((nSubNode_TM_All + 1).ToString()); // ??? Fixed yet? Error. _49.htm, Should suppress the final Next which will point to a nonExistent file.
				sbuilder.Append(this.m_irunPropRead .GetHtmlFileExtension());

				sNextFileName = sbuilder.ToString();

				//????? sbuilder.AppendLine("\">Next File</a>"); // Next
				//sbuilder.AppendLine("<br /><br />");

				//????? _streamWriter_ref.WriteLine(sbuilder.ToString());

			}

			sHtmlNavigLinks = this.WritePreviousNextNavigationsAtFileTopOrBottom
				(
				 sPreviousFileName  // "Previous File" filename, for an <a href= link; or null.
				,sNextFileName      // "Next File" filename, for an <a href= link.
				,true               // "Navigation:" literal is wanted?
				,true               // Link to Default.htm (Home) is wanted?
				);

			_streamWriter_ref.WriteLine(sHtmlNavigLinks);
			_streamWriter_ref.WriteLine("<br />");
			_streamWriter_ref.WriteLine("<table>");

			sbuilder.Length = 0;
			sbuilder.AppendLine();
			sbuilder.AppendLine("<thead>");
			sbuilder.AppendLine("<tr class=\"cssFontForMainColumnHeaders\">");
			sbuilder.Append    ("    <th>Row-num&nbsp;&nbsp;</th>");
			sbuilder.Append    (" <th>Table-name</th>");
			sbuilder.Append    (" <th>&nbsp;</th>");
			sbuilder.AppendLine(" <th>Module-name</th>");
			sbuilder.AppendLine("</tr>");
			sbuilder.AppendLine("</thead>");
			sbuilder.AppendLine("</tbody>");

			_streamWriter_ref.WriteLine(sbuilder.ToString());
			_streamWriter_ref.WriteLine();

			return;
		}





		private void CreateHtmlFileModuleTables()
		{
			SysIo.StreamWriter streamWriter = null;
			SysCollGen.SortedDictionary<string, CValueTableModule> sdictOfTableModules; // Really singular 'Module', but legacy name, eh.
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableModule>> sdictOfModuleTables;
			SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> sdictModuFileNodes;
			CValueTableModule valueTableModule_TM = null, valueTableModule_TM_Previous,
				eachValueTableModule_MT_Previous = null;
			CModuleFileOwnerInfo moduFileOwnerInfo;
			SysCollGen.List<CValueTableModule> listTableModule;
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(256);
			char[] charArraySpace = new char[1] { ' ' };  // One space character, only.
			string[] sAryModParamsCP;
			int nLoopCount = 0, nCountEntryItemsWritten;
			bool boolModuleMatchFound, bModuleNameHasChangedFromPrevious;
			//

			if (true == this.m_irunPropRead.GetSkipOutputOfFilesTM())
			{
				goto LABEL_ENDOFMETHOD_nte73_LABEL;
			}

			// Next, file data driven lines.

			sdictOfModuleTables = this.m_irunPropRead.GetSDictOfModuleTables();


			//__________ LOOP, outer loop (thru Modules) ________________________


			foreach (SysCollGen.KeyValuePair<string, SysCollGen.List<CValueTableModule>> kvpMT in sdictOfModuleTables)
			{

				listTableModule = kvpMT.Value;

				//___________ LOOP, inner loop (T's in .Value items) ___________________


				nCountEntryItemsWritten = 0;


				foreach (CValueTableModule eachValueTableModule_MT in listTableModule)
				{
					nLoopCount++;

					if (1 == nLoopCount)
					{
						eachValueTableModule_MT_Previous = eachValueTableModule_MT; // ???? Test this!
					}

					if (eachValueTableModule_MT_Previous .GetModuleNameLowercaseNoSpaces() ==
							eachValueTableModule_MT .GetModuleNameLowercaseNoSpaces()
						)
					{
						bModuleNameHasChangedFromPrevious = false;
					}
					else
					{
						bModuleNameHasChangedFromPrevious = true;
					}
					//---------------------------------------------

					if (null == streamWriter)
					{
						this.ManageStreamWriter_MT
							(
							eachValueTableModule_MT_Previous,
							eachValueTableModule_MT,
							ref streamWriter
							);
					}

					//----------------------
					// Is the module of the present record filtered out by the restrict to modules parameter?

					boolModuleMatchFound = false;  // Assume this module should be skipped.
					sAryModParamsCP = this.m_irunPropRead.GetRestrictToModulesTMMT();
					if (null == sAryModParamsCP || 0 == sAryModParamsCP.Length)
					{
						boolModuleMatchFound = true;  // Not literally true, but equivalent.
					}
					else
					{
						foreach (string sMod in sAryModParamsCP)
						{
							if (eachValueTableModule_MT.GetModuleNameLowercase() == sMod.ToLower())
							{
								boolModuleMatchFound = true;
								break;
							}
						}
					}
					if (!boolModuleMatchFound)
					{
						continue;  // Thus skipping the current module record, and not outputting it to HTML.
					}
					//----------------------


					// Next part is driven by the data derived earlier from the input delimited files.
					sbuilder.Length = 0;

					sbuilder.Append("<tr><td>"); // css class??
					sbuilder.Append((++nCountEntryItemsWritten).ToString());
					sbuilder.Append("&nbsp;&nbsp;</td><td>");

					// Module
					if (bModuleNameHasChangedFromPrevious)
					{
						sbuilder.Append("<a name=\"m-");
						sbuilder.Append(eachValueTableModule_MT.GetModuleNameLowercaseNoSpaces());
						sbuilder.Append("\"></a>");
					}

					// ERD link!
					sdictModuFileNodes = this.m_irunPropRead.GetSDictOfModuleName_to_FileNameNodes();
					if (sdictModuFileNodes.TryGetValue
							(eachValueTableModule_MT.GetModuleNameLowercaseNoSpaces(),
							out moduFileOwnerInfo
							)
						)
					{
						if (moduFileOwnerInfo .GetErdModuleFileName() !=
							CModuleFileOwnerInfo .M_sUnknown_ErdModuleFileName_PlaceholderValue
							)
						{
							sbuilder.Append("<a href=\"");
							sbuilder.Append(moduFileOwnerInfo.GetErdModuleFileName());
							sbuilder.Append("\"<a>ERD</a>  ");
						}
					}

					sbuilder.Append("</td><td>&nbsp;&nbsp;&nbsp;</td><td>");

					sbuilder.Append(eachValueTableModule_MT.GetModuleName());

					sbuilder.Append("&nbsp;:&nbsp;</td><td>&nbsp;&nbsp;&nbsp;</td><td>");


					// Table
					sbuilder.Append("<a class=\"cssAHref2\" href=\"");
					sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT()); // ""
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileTableModules()); // "All-TableModule"

					// SubNode
					sbuilder.Append("-");
					sdictOfTableModules = this.m_irunPropRead.GetSDictOfTableModules();

					valueTableModule_TM_Previous = valueTableModule_TM;
					valueTableModule_TM = sdictOfTableModules
						[eachValueTableModule_MT.GetTableNameLowercase()];
					sbuilder.Append(valueTableModule_TM.SubNode_TM_All);

					// #t
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"
					sbuilder.Append("#t-");
					sbuilder.Append(eachValueTableModule_MT.GetTableNameLowercase());
					sbuilder.Append("\">");
					sbuilder.Append(eachValueTableModule_MT.GetTableName());
					sbuilder.Append("</a>");
					sbuilder.Append("</td></tr>");

					// SubNode file?

					if (
							(null == eachValueTableModule_MT_Previous)
									||
							(eachValueTableModule_MT_Previous .SubNode_MT_PerModule !=
								eachValueTableModule_MT .SubNode_MT_PerModule)
									||
							(eachValueTableModule_MT_Previous .GetModuleNameLowercaseNoSpaces() !=
								eachValueTableModule_MT .GetModuleNameLowercaseNoSpaces())
						)
					{
						streamWriter.WriteLine("</tbody>");
						streamWriter.WriteLine("</table>");

						this.ManageStreamWriter_MT
							(
							eachValueTableModule_MT_Previous,
							eachValueTableModule_MT,
							ref streamWriter
							);
					}

					streamWriter.WriteLine(sbuilder.ToString()); // ???? test this

					eachValueTableModule_MT_Previous = eachValueTableModule_MT;

				} // EOFor, listTableTable, each valueTableModule per iteration.
			} // EOFor SortedDict MT.

			streamWriter.WriteLine("</tbody>");
			streamWriter.WriteLine("</table>");
			streamWriter.WriteLine("</body>");
			streamWriter.WriteLine("</html>");

			streamWriter.Flush();
			streamWriter.Close();
			streamWriter.Dispose();

		LABEL_ENDOFMETHOD_nte73_LABEL: ;
			return;
		}




		private void ManageStreamWriter_MT
			(
			CValueTableModule _valueTableModule_MT_Previous,
			CValueTableModule _valueTableModule_MT,
			ref SysIo.StreamWriter _streamWriter_ref
			)
		{
			SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> sdictModuleName_to_FileNameNodes;
			CModuleFileOwnerInfo moduleFileOwnerInfo;
			SysIo.FileStream fileStream;
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(512);
			string sHtmlNavigLinks, sPreviousFileName, sNextFileName;
			int nSubNodeTemp;
			//

			sdictModuleName_to_FileNameNodes = this.m_irunPropRead.GetSDictOfModuleName_to_FileNameNodes();
			moduleFileOwnerInfo = sdictModuleName_to_FileNameNodes
				[_valueTableModule_MT .GetModuleNameLowercaseNoSpaces()];


			if (null != _streamWriter_ref)
			{
				// Is link to NEXT subnode needed, at BOTTOM of to-be-closed file?
				if (moduleFileOwnerInfo.GetHighestSubnode_MT_AsInt() >=
						Convert.ToInt32(_valueTableModule_MT .SubNode_MT_PerModule)  // Data known from pre-run hardcode calculation.
						&&
					_valueTableModule_MT_Previous.GetModuleNameLowercaseNoSpaces() == _valueTableModule_MT.GetModuleNameLowercaseNoSpaces()
					)
				{
					sbuilder.Length = 0;
					sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT()); // ""
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileModuleTables());

					sbuilder.Append("-");
					sbuilder.Append(moduleFileOwnerInfo.GetFileNameNode());

					sbuilder.Append("-");
					nSubNodeTemp = Convert.ToInt32(_valueTableModule_MT .SubNode_MT_PerModule);  // +1
					sbuilder.Append(nSubNodeTemp.ToString());
					sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"

					sNextFileName = sbuilder.ToString();
					sHtmlNavigLinks = this.WritePreviousNextNavigationsAtFileTopOrBottom
						(
						 null               // "Previous File" filename, for an <a href= link; or null.
						,sNextFileName      // "Next File" filename, for an <a href= link.
						,true               // "Navigation:" literal is wanted?
						,false              // Link to Default.htm (Home) is wanted?
						);

					_streamWriter_ref.WriteLine("<br />");
					_streamWriter_ref.WriteLine(sHtmlNavigLinks);
					_streamWriter_ref.Flush();

					//_streamWriter_ref.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
					//_streamWriter_ref.WriteLine("<br />");
					//????? _streamWriter_ref.Write("<a href=\"");
					//_streamWriter_ref.Write(sbuilder.ToString());
					//_streamWriter_ref.WriteLine("\">Next File</a><br />");

				}


				_streamWriter_ref.WriteLine();
				_streamWriter_ref.WriteLine("</body>");
				_streamWriter_ref.WriteLine("</html>");

				_streamWriter_ref.Flush();
				_streamWriter_ref.Close();
				_streamWriter_ref.Dispose();
				_streamWriter_ref = null;
			}

			sbuilder.Length = 0;
			sbuilder.Append(this.m_irunPropRead.GetPathToOutputHtmlFiles());
			sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT()); // ""
			sbuilder.Append(this.m_irunPropRead.GetHtmlFileModuleTables()); // "Modu"

			sbuilder.Append("-");
			sbuilder.Append(moduleFileOwnerInfo.GetFileNameNode());

			sbuilder.Append("-");
			sbuilder.Append(_valueTableModule_MT .SubNode_MT_PerModule);
			sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"


			fileStream = new SysIo.FileStream
					(
					sbuilder.ToString(),
					SysIo.FileMode.Create,  // .Create means Overwrite if file pre-exists.
					SysIo.FileAccess.Write,
					SysIo.FileShare.None
					);
			_streamWriter_ref = new SysIo.StreamWriter(fileStream);

			_streamWriter_ref.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");

			_streamWriter_ref.WriteLine("<html>");
			_streamWriter_ref.WriteLine("<!--");
			_streamWriter_ref.WriteLine("\tMicrosoft Dynamics AX 2012 R2, Table ERD and related Application Module info.");
			_streamWriter_ref.WriteLine("-->");
			_streamWriter_ref.WriteLine("<head>");
			_streamWriter_ref.WriteLine("\t<title>AxErd MT</title>");

			_streamWriter_ref.Write("\t<link rel=\"stylesheet\" type=\"text/css\" href=\"");
			_streamWriter_ref.Write(COutputterToHtmlFiles.M_sCss_Stylesheet_FileName);
			_streamWriter_ref.WriteLine("\"/>");

			_streamWriter_ref.WriteLine("\t<meta name=\"Ax.Erd.62.All\" content=\"yes\" />");
			_streamWriter_ref.WriteLine("\t<meta name=\"Ax.Erd.62.ContentType\" content=\"MT\" />");

			_streamWriter_ref.WriteLine("</head>");
			_streamWriter_ref.WriteLine("<body class=\"cssBackgroundColorBodyTM\">");
			_streamWriter_ref.WriteLine("<h4>Microsoft Dynamics AX 2012 R2</h4>");
			_streamWriter_ref.Write("<h2 class=\"cssH3ColorTM\">AxErd: Tables in One Module: ");
			_streamWriter_ref.Write(moduleFileOwnerInfo.GetFormalModuleName());
			_streamWriter_ref.WriteLine("</h2>");
			_streamWriter_ref.WriteLine("<br />");


			//????? _streamWriter_ref.WriteLine("<a href=\"Default.htm\">AxErd Home</a>");

			//sdictModuleName_to_FileNameNodes = this.m_irunPropRead.GetSDictOfModuleName_to_FileNameNodes();
			//??moduleFileOwnerInfo = sdictModuleName_to_FileNameNodes
			//	[_valueTableModule_MT .GetModuleNameLowercaseNoSpaces()];

			// Is link to PREVIOUS subnode needed?
			sPreviousFileName = null;
			if (11 < Convert.ToInt32(_valueTableModule_MT .SubNode_MT_PerModule))  // Hardcoded 11 as first subnode.
			{
				sbuilder.Length = 0;
				sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT()); // ""
				sbuilder.Append(this.m_irunPropRead.GetHtmlFileModuleTables()); // "Modu"

				sbuilder.Append("-");
				sbuilder.Append(moduleFileOwnerInfo.GetFileNameNode());

				sbuilder.Append("-");
				nSubNodeTemp = Convert.ToInt32(_valueTableModule_MT .SubNode_MT_PerModule) - 1;
				sbuilder.Append(nSubNodeTemp.ToString());
				sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"

				sPreviousFileName = sbuilder.ToString();
				//????? _streamWriter_ref.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
				//_streamWriter_ref.Write("<a href=\"");
				//_streamWriter_ref.Write(sbuilder.ToString());
				//_streamWriter_ref.WriteLine("\">Previous File</a>");

			}


			// Is link to NEXT subnode needed?
			sNextFileName = null;
			if (moduleFileOwnerInfo.GetHighestSubnode_MT_AsInt() >
					Convert.ToInt32(_valueTableModule_MT .SubNode_MT_PerModule)  // Data known from pre-run hardcode calculation.
				)
			{
				sbuilder.Length = 0;
				sbuilder.Append(this.m_irunPropRead.GetOutputHtmlFilesCommonPrefixTMMT()); // ""
				sbuilder.Append(this.m_irunPropRead.GetHtmlFileModuleTables()); // "Modu"

				sbuilder.Append("-");
				sbuilder.Append(moduleFileOwnerInfo.GetFileNameNode());

				sbuilder.Append("-");
				nSubNodeTemp = Convert.ToInt32(_valueTableModule_MT .SubNode_MT_PerModule) + 1;
				sbuilder.Append(nSubNodeTemp.ToString());
				sbuilder.Append(this.m_irunPropRead.GetHtmlFileExtension()); // ".htm"

				sNextFileName = sbuilder.ToString();
				//????? _streamWriter_ref.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
				//_streamWriter_ref.Write("<a href=\"");
				//_streamWriter_ref.Write(sbuilder.ToString());
				//_streamWriter_ref.WriteLine("\">Next File</a>");

			}


			sHtmlNavigLinks = this.WritePreviousNextNavigationsAtFileTopOrBottom
				(
				 sPreviousFileName  // "Previous File" filename, for an <a href= link; or null.
				,sNextFileName      // "Next File" filename, for an <a href= link.
				,true               // "Navigation:" literal is wanted?
				,true               // Link to Default.htm (Home) is wanted?
				);


			_streamWriter_ref.WriteLine(sHtmlNavigLinks);
			_streamWriter_ref.WriteLine("<br />");
			_streamWriter_ref.WriteLine("<table>");

			sbuilder.Length = 0;
			sbuilder.AppendLine();
			sbuilder.AppendLine("<thead>");
			sbuilder.AppendLine("<tr class=\"cssFontForMainColumnHeaders\">");
			sbuilder.Append    ("    <th>Row-num&nbsp;&nbsp;</th>");
			sbuilder.Append    (" <th>(Link)</th>");
			sbuilder.Append    (" <th>&nbsp;</th>");
			sbuilder.Append    (" <th>Module-name</th>");
			sbuilder.Append    (" <th>&nbsp;</th>");
			sbuilder.AppendLine(" <th>Table-name</th>");
			sbuilder.AppendLine("</tr>");
			sbuilder.AppendLine("</thead>");
			sbuilder.AppendLine("</tbody>");

			_streamWriter_ref.WriteLine(sbuilder.ToString());

			return;
		}


/*** Never called.
		private string BuildIntraPageLinksToModuleNames
				(
				SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> _sdictMods
				)
		{
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(2048);
			int nLinksWritten = 0, nMaxLinksPerRow = 2;
			//

			sbuilder.AppendLine();
			sbuilder.AppendLine("<div class=\"cssDiv_MT_IntraPage_ModuLinks\">");
			sbuilder.AppendLine("&nbsp;&nbsp;&nbsp;<i>Links to sections below:</i><br />");
			sbuilder.AppendLine("<table>");
			sbuilder.AppendLine();

			foreach (SysCollGen.KeyValuePair<string, CModuleFileOwnerInfo> kvpMods in _sdictMods)
			{
				if ( 0 == (nLinksWritten % nMaxLinksPerRow) )
				{
					// New <tr> row must be started.
					sbuilder.Append("<tr>");
				}
				else
				{
					// Add a short horizontal buffer between previous <td> cell and this new cell.
					sbuilder.AppendLine("<td style=\"border: 0px\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>");
				}

				sbuilder.Append("<td style=\"border: 0px\"><a href=\"#m-");
				sbuilder.Append(kvpMods.Value.GetFormalModuleNameLowercaseNoSpaces()); // ??? What? Extension seems only part of the problem of missing stuff for this href=.
				sbuilder.Append("\">");
				sbuilder.Append(kvpMods.Value.GetFormalModuleName());
				sbuilder.AppendLine("</a></td>");

				if ( (nMaxLinksPerRow - 1) == (nLinksWritten % nMaxLinksPerRow) )
				{
					// This was the final <td> cell for this <tr> row.
					sbuilder.AppendLine("</tr>");
				}

				nLinksWritten++;
			}

			sbuilder.AppendLine();
			sbuilder.AppendLine("</table></div>");
			sbuilder.AppendLine();

			return sbuilder.ToString();
		}
***/

	
		
		/// <summary>
		/// Common code because all file types need Previous and Next file links for navigation.
		/// </summary>
		/// <param name="_sPreviousFileName"></param>
		/// <param name="_sNextFileName"></param>
		/// <param name="_boolWantLabelNavigation"></param>
		/// <param name="_boolWantLinkToDefaultWebpage"></param>
		/// <returns>String of HTML.
		/// </returns>
		private string WritePreviousNextNavigationsAtFileTopOrBottom
			(
			 string _sPreviousFileName
			,string _sNextFileName
			,bool   _boolWantLabelNavigation
			,bool   _boolWantLinkToDefaultWebpage
			)
		{
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(1024);
			//

			sbuilder.Length = 0;
			sbuilder.AppendLine();
			sbuilder.AppendLine("<p class=\"cssPPreviousNextNavigation\">");

			if (_boolWantLabelNavigation)
			{
				sbuilder.AppendLine("<i>Navigation:</i>&nbsp;&nbsp;&nbsp;&nbsp;");
			}


			if (_boolWantLinkToDefaultWebpage)
			{
				sbuilder.AppendLine("<a href=\"Default.htm\">AxErd_Home</a>&nbsp;&nbsp;&nbsp;&nbsp;");
			}

	
			if (null != _sPreviousFileName)
			{
				sbuilder.Append("<a href=\"");
				sbuilder.Append(_sPreviousFileName);
				sbuilder.AppendLine("\">Previous_File</a>&nbsp;&nbsp;&nbsp;&nbsp;"); // Previous
			}


			if (null != _sNextFileName)
			{
				sbuilder.Append("<a href=\"");
				sbuilder.Append(_sNextFileName);
				sbuilder.AppendLine("\">Next_File</a>"); // Next
			}


			sbuilder.AppendLine("</p>");

			return sbuilder.ToString();
		}


	} // EOClass
}
