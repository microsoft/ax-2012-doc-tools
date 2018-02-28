using              System;
using SysCollGen = System.Collections.Generic;
using SysIo      = System.IO;
using              System.Linq;  // Operators need this namespace to appear without an alias (can also appear with alias, no harm in having both).
using SysLinq    = System.Linq;


namespace AxErd
{
	class CInputterFromDelimFiles
				: IInputter
	{
		internal CRunProperties m_runProperties;
		//

		public CInputterFromDelimFiles  // .ctor
					(
					CRunProperties _runProperties
					)
		{
			this.m_runProperties = _runProperties;
			//this.PopulateOwnerToModuleTranslations();
		}



		public void BuildSortDictForColumnsFkyPky()
		{
			SysIo.FileStream fileStream;
			SysIo.StreamReader streamReader;
			SysCollGen.List<CValueColumnColumn> listColumnColumn;
			CValueColumnColumn valueColumnColumn;
			char[] cDelims = {'\t'};
			string[] sSplits;
			string sLine = "Initialize.";
			int nLineReadCount = 0;
			//

			if (null != this.m_runProperties.m_sdictColumnsFkyPky)
			{
				goto LABEL_RETURN_csh89_LABEL;
			}
			else
			{
				this.m_runProperties.m_sdictColumnsFkyPky = new SysCollGen.SortedDictionary
					<int, SysCollGen.List<CValueColumnColumn>>();
			}

			using
					(
					fileStream = new SysIo.FileStream
							(
							this.m_runProperties.GetPathToInputDelimFiles()
								+ this.m_runProperties.GetInputDelimFilesCommonPrefix()
								+ this.m_runProperties.GetDelimFileColumnColumn(),
							SysIo.FileMode.Open,
							SysIo.FileAccess.Read,
							SysIo.FileShare.Read
							)
					)
			{
				streamReader = new SysIo.StreamReader(fileStream);


				while (streamReader.Peek() >= 0)
				{
					try
					{
						nLineReadCount++;
						sLine = streamReader.ReadLine();
						sSplits = sLine.Split(cDelims, StringSplitOptions.RemoveEmptyEntries);
						if (4 > sSplits.Length) { continue; }
						if (sSplits[0].StartsWith("//")) { continue; }
						if (sSplits[0].Trim().Length == 0) { continue; }

						valueColumnColumn = new CValueColumnColumn
							(Convert.ToInt32(sSplits[0]), Convert.ToInt32(sSplits[1]), sSplits[2], sSplits[3]);


						if (this.m_runProperties.m_sdictColumnsFkyPky.ContainsKey(valueColumnColumn.GetIdentityTT()))
						{
							listColumnColumn = this.m_runProperties.m_sdictColumnsFkyPky[valueColumnColumn.GetIdentityTT()];
							listColumnColumn.Add(valueColumnColumn);
						}
						else  // ! ContainsKey
						{
							listColumnColumn = new SysCollGen.List<CValueColumnColumn>();
							listColumnColumn.Add(valueColumnColumn);
							this.m_runProperties.m_sdictColumnsFkyPky.Add
									(
									valueColumnColumn.GetIdentityTT(),
									listColumnColumn
									);
						}
					}
					catch (Exception e)
					{
						Console.WriteLine("Error_qmc41: nLineReadCount={0}, sLine={1}", nLineReadCount, sLine);
						throw e;
					}
				} // EOWhile peek.
			} // EOUsing FileStream.

		LABEL_RETURN_csh89_LABEL: ;

			if (null == this.m_runProperties.m_sdictColumnsFkyPky)
			{
				this.m_runProperties.m_sdictColumnsFkyPky = new SysCollGen.SortedDictionary
						<int, SysCollGen.List<CValueColumnColumn>>();
			}
			return;
		}




		/// <summary>
		/// Such as CustTable might be a child of three other tables, according to Dynamics AX 2012 AOT
		/// (thus three key-value pairs for CustTable).
		/// </summary>
		/// <returns>SortedDictionary, key=ChildTableName (+" "+ParentTableName), value=CValueTableTable.
		/// Never null.</returns>
		/// <remarks>Caution: In this method pair, GetSortDictFor* ChildParents & ParentChilds,
		/// each method initializes both itself and the other sdict, when appropriate.
		/// </remarks>
		public void BuildSortDictForChildParents_and_ParentChilds()
		{
			SysIo.FileStream fileStream;
			SysIo.StreamReader streamReader;
			SysCollGen.List<CValueTableTable> listTableTable;
			CValueTableTable valueTableTable_CP, valueTableTable_PC;
			char[] cDelims = {'\t'};
			string[] sSplits;
			string sLine = "Initialize.";
			string sChildTableName, sParentTableName;
			int nKeyTT;
			int nLineReadCount = 0;
			//

			if (null != this.m_runProperties.m_sdictChildParents)
			{
				goto LABEL_RETURN_msj58_LABEL;
			}
			else
			{
				this.m_runProperties.m_sdictChildParents = new SysCollGen.SortedDictionary  // C-P
					<string, SysCollGen.List<CValueTableTable>>();

				this.m_runProperties.m_sdictParentChilds = new SysCollGen.SortedDictionary  // P-C
					<string, SysCollGen.List<CValueTableTable>>();
			}

			using
					(
					fileStream = new SysIo.FileStream
							(
							this.m_runProperties.GetPathToInputDelimFiles()
								+ this.m_runProperties.GetInputDelimFilesCommonPrefix()
								+ this.m_runProperties.GetDelimFileChildParents(),
							SysIo.FileMode.Open,
							SysIo.FileAccess.Read,
							SysIo.FileShare.Read
							)
					)
			{
				streamReader = new SysIo.StreamReader(fileStream);


				while (streamReader.Peek() >= 0)
				{
					try
					{
						nLineReadCount++;
						sLine = streamReader.ReadLine();
						sSplits = sLine.Split(cDelims, StringSplitOptions.RemoveEmptyEntries);
						if (3 > sSplits.Length) { continue; }
						if (sSplits[0].StartsWith("//")) { continue; }
						if (sSplits[0].Trim().Length == 0) { continue; }

						nKeyTT = Convert.ToInt32(sSplits[0]);
						sChildTableName = sSplits[1];
						sParentTableName = sSplits[2];

						valueTableTable_CP = new CValueTableTable
							(
							sParentTableName, // IComparable.
							nKeyTT,
							sChildTableName,
							sParentTableName
							);


						// Child-Parents
						if (this.m_runProperties.m_sdictChildParents.ContainsKey(valueTableTable_CP.GetKeyChildDelimParent()))
						{
							listTableTable = this.m_runProperties.m_sdictChildParents[valueTableTable_CP.GetKeyChildDelimParent()];
							listTableTable.Add(valueTableTable_CP);  // Also SORTED, by Parent, earlier during X++ time.
						}
						else  // ! ContainsKey
						{
							listTableTable = new SysCollGen.List<CValueTableTable>();
							listTableTable.Add(valueTableTable_CP);
							this.m_runProperties.m_sdictChildParents.Add
									(
									valueTableTable_CP.GetKeyChildDelimParent(),
									listTableTable
									);
						}


						// Parent-Childs

						// Clone vtt, need different IComparable string to sort by.
						valueTableTable_PC = valueTableTable_CP.CloneThis(valueTableTable_CP.GetTableNameChild());

						if (this.m_runProperties.m_sdictParentChilds.ContainsKey(valueTableTable_PC.GetKeyParentDelimChild()))
						{
							listTableTable = this.m_runProperties.m_sdictParentChilds[valueTableTable_PC.GetKeyParentDelimChild()];
							listTableTable.Add(valueTableTable_PC);  // SORTED, by Child, by IComparable.
						}
						else  // ! ContainsKey
						{
							listTableTable = new SysCollGen.List<CValueTableTable>();
							listTableTable.Add(valueTableTable_PC);
							this.m_runProperties.m_sdictParentChilds.Add
									(
									valueTableTable_PC.GetKeyParentDelimChild(),
									listTableTable
									);
						}
					}
					catch (Exception e)
					{
						Console.WriteLine("Error_cys78: nLineReadCount={0}, sLine={1}", nLineReadCount, sLine);
						throw e;
					}
				} // EOWhile peek.
			} // EOUsing FileStream.

		LABEL_RETURN_msj58_LABEL: ;

			if (null == this.m_runProperties.m_sdictChildParents)
			{
				this.m_runProperties.m_sdictChildParents = 
					new SysCollGen.SortedDictionary
						<string, SysCollGen.List<CValueTableTable>>();

					this.m_runProperties.m_sdictParentChilds = 
					new SysCollGen.SortedDictionary
						<string, SysCollGen.List<CValueTableTable>>();
			}

			return;
		}




		/// <summary>
		/// This build of sdict TM must be called BEFORE the build of sdict MT is called!
		/// And, the build of sdict CP must be called BEFORE the build of sdict MT is called!
		/// </summary>
		public void BuildSortDictForTableModules()
		{
			SysIo.FileStream fileStream;
			SysIo.StreamReader streamReader;
			CValueTableModule valueTableModule_TM;
			char[] cDelims = { '\t' };
			string[] sSplits;
			string sLine = "Initialize";
			int nLineReadCount = 0;
			//

			if (null != this.m_runProperties.m_sdictTableModules)
			{
				goto LABEL_RETURN_kds67_LABEL;
			}
			else
			{
				this.m_runProperties.m_sdictTableModules = new SysCollGen.SortedDictionary
					<string, CValueTableModule>();
			}

			using
					(
					fileStream = new SysIo.FileStream
							(
							this.m_runProperties.GetPathToInputDelimFiles()
								+ this.m_runProperties.GetInputDelimFilesCommonPrefix()
								+ this.m_runProperties.GetDelimFileTableModules(),
							SysIo.FileMode.Open,
							SysIo.FileAccess.Read,
							SysIo.FileShare.Read
							)
					)
			{
				streamReader = new SysIo.StreamReader(fileStream);


				while (streamReader.Peek() >= 0)  // Loop thru input records.
				{
					try
					{
						nLineReadCount++;
						sLine = streamReader.ReadLine();
						sSplits = sLine.Split(cDelims, StringSplitOptions.RemoveEmptyEntries);
						if (2 > sSplits.Length) { continue; }
						if (sSplits[0].StartsWith("//")) { continue; }
						if (sSplits[0].Trim().Length == 0) { continue; }

						valueTableModule_TM = new CValueTableModule
							(
							sSplits[1], // IComparable
							sSplits[0], // TableName
							sSplits[1]  // ModuleName
							);


						// T-M
						this.m_runProperties.m_sdictTableModules.Add
								(
								valueTableModule_TM.GetKeyTableDelimModule(),
								valueTableModule_TM
								);

					}
					catch (Exception e)
					{
						Console.WriteLine("Error_cyb50: nLineReadCount={0}, sLine={1}", nLineReadCount, sLine);
						throw e;
					}
				}
			}

		LABEL_RETURN_kds67_LABEL: ;

			if (null == this.m_runProperties.m_sdictTableModules)
			{
				this.m_runProperties.m_sdictTableModules = new SysCollGen.SortedDictionary
						<string, CValueTableModule>();
			}
			return;
		}



		/// <summary>
		/// Call this only AFTER the build methods for both sdict's CP and TM have been called!
		/// </summary>
		public void BuildSortDictForModuleTables()
		{
			CValueTableModule valueTableModule_TM;
			CValueTableModule valueTableModule_MT;
			SysCollGen.List<CValueTableModule> listTableModules_UnkMod;
			SysCollGen.List<CValueTableModule> listTableModules_MT = null;
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>>.KeyCollection keyColl_CP;
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>>.KeyCollection keyColl_PC;
			bool boolUnknownModuleKey_HasBeenAddedToMT;
			//

			// Skip whole method if sdict MT already exists.
			if (null != this.m_runProperties.m_sdictModuleTables)
			{
				goto LABEL_RETURN_kdx67_LABEL;
			}
			else
			{
				this.m_runProperties.m_sdictModuleTables = new SysCollGen.SortedDictionary
					<string, SysCollGen.List<CValueTableModule>>();
			}


			// First, a simple copy of data from sdict TM to sdict MT.

			foreach (SysCollGen.KeyValuePair<string, CValueTableModule> eachKvpTM
					in this.m_runProperties.m_sdictTableModules
					)
			{
				valueTableModule_TM = eachKvpTM.Value;
				valueTableModule_MT = new CValueTableModule
					(
					valueTableModule_TM.GetTableName(),
					valueTableModule_TM.GetTableName(),
					valueTableModule_TM.GetModuleName()
					);

				if (! this.m_runProperties.m_sdictModuleTables.ContainsKey
						(valueTableModule_MT .GetModuleNameLowercaseNoSpaces())
					)
				{
					// New key.
					listTableModules_MT = new SysCollGen.List<CValueTableModule>();
					listTableModules_MT.Add(valueTableModule_MT);

					this.m_runProperties.m_sdictModuleTables.Add
						(valueTableModule_MT.GetModuleNameLowercaseNoSpaces(),
						listTableModules_MT
						);
				}
				else  // Key ALREADY exists.
				{
					listTableModules_MT = this.m_runProperties.m_sdictModuleTables
						[valueTableModule_MT .GetModuleNameLowercaseNoSpaces()];
					listTableModules_MT.Add(valueTableModule_MT);
				}
			}

			//________________________________________________
			//________________________________________________


			// Create the special "UnknownModule" key in sdict MT.

			// First, loop through CP for all C.
			// Second, loop through PC for all P.

			//__________ Loop CP

			boolUnknownModuleKey_HasBeenAddedToMT = false;

			keyColl_CP = this.m_runProperties.GetSDictOfChildParents().Keys;

			foreach (string eachKey_C in keyColl_CP)
			{
				if (! this.m_runProperties.GetSDictOfTableModules().ContainsKey(eachKey_C))
				{
					// This child table name was NOT in the T-M data gathered from Dynamics AX6 (AX 2012) by X++.
					// Thus put this child table into the pretend Unknown Module.

					valueTableModule_MT = new CValueTableModule
						(
						eachKey_C, // IComparable
						eachKey_C, // TableName
						CRunProperties.M_readonly_sUnknownModule // ModuleName
						);

					try
					{
						if ((! boolUnknownModuleKey_HasBeenAddedToMT) &&
								! this.m_runProperties.GetSDictOfModuleTables().ContainsKey
									(CRunProperties.M_readonly_sUnknownModule_LowercaseNoSpaces)
							)
						{
							// This is the first missing table name yet encountered, so
							// create the key etc in the sdict MT.

							listTableModules_UnkMod = new SysCollGen.List<CValueTableModule>();
							listTableModules_UnkMod .Add(valueTableModule_MT);

							this.m_runProperties.GetSDictOfModuleTables().Add
								(
								CRunProperties.M_readonly_sUnknownModule_LowercaseNoSpaces,
								listTableModules_UnkMod
								);

							boolUnknownModuleKey_HasBeenAddedToMT = true;
						}
						else
						{
							// sdict MT already has the Unknown Module key, so
							// simply add the current table name to the corresponding value list.

							listTableModules_UnkMod = this.m_runProperties.GetSDictOfModuleTables()
								[CRunProperties.M_readonly_sUnknownModule_LowercaseNoSpaces];

							listTableModules_UnkMod .Add(valueTableModule_MT);
						}
					}
					catch (Exception ee)
					{
						Console.WriteLine("Error_1742sc5: {0}", ee);
						throw ee;
					}
				}
			} // EOForeach eachKey_C


			//__________ Loop PC

			//boolUnknownModuleKey_HasBeenAddedToMT = false;

			keyColl_PC = this.m_runProperties.GetSDictOfParentChilds().Keys;

			foreach (string eachKey_P in keyColl_PC)
			{
				if (! this.m_runProperties.GetSDictOfTableModules().ContainsKey(eachKey_P))
				{
					// This child table name was NOT in the T-M data gathered from Dynamics AX6 (AX 2012) by X++.
					// Thus put this child table into the pretend Unknown Module.

					valueTableModule_MT = new CValueTableModule
						(
						eachKey_P, // IComparable
						eachKey_P, // TableName
						CRunProperties.M_readonly_sUnknownModule // ModuleName
						);

					try
					{
						if ((! boolUnknownModuleKey_HasBeenAddedToMT) &&
								! this.m_runProperties.GetSDictOfModuleTables().ContainsKey
									(CRunProperties.M_readonly_sUnknownModule_LowercaseNoSpaces)
							)
						{
							// This is the first missing table name yet encountered, so
							// create the key etc in the sdict MT.

							listTableModules_UnkMod = new SysCollGen.List<CValueTableModule>();
							listTableModules_UnkMod .Add(valueTableModule_MT);

							this.m_runProperties.GetSDictOfModuleTables().Add
								(
								CRunProperties.M_readonly_sUnknownModule_LowercaseNoSpaces,
								listTableModules_UnkMod
								);

							boolUnknownModuleKey_HasBeenAddedToMT = true;
						}
						else
						{
							// sdict MT already has the Unknown Module key, so
							// simply add the current table name to the corresponding value list.

							listTableModules_UnkMod = this.m_runProperties.GetSDictOfModuleTables()
								[CRunProperties.M_readonly_sUnknownModule_LowercaseNoSpaces];

							listTableModules_UnkMod .Add(valueTableModule_MT);
						}
					}
					catch (Exception ee)
					{
						Console.WriteLine("Error_1742sc5: {0}", ee);
						throw ee;
					}
				}
			} // EOForeach eachKey_P


			//________________________________________________
			//________________________________________________


			// Add the "UnknownModule" tables to T-M sdict.

			if (boolUnknownModuleKey_HasBeenAddedToMT)
			{
				listTableModules_UnkMod = this.m_runProperties.GetSDictOfModuleTables()
					[CRunProperties.M_readonly_sUnknownModule_LowercaseNoSpaces];

				foreach (CValueTableModule eachValue_MT_UnkMod_T in listTableModules_UnkMod)
				{
					valueTableModule_TM = new CValueTableModule
						(
						eachValue_MT_UnkMod_T.GetModuleName(), // IComparable
						eachValue_MT_UnkMod_T.GetTableName(),
						eachValue_MT_UnkMod_T.GetModuleName()
						);

					if (this.m_runProperties.GetSDictOfTableModules()
							.ContainsKey( eachValue_MT_UnkMod_T.GetTableNameLowercase() )
						)
					{
						//Console.WriteLine("??? Caution_1733qf7: Semi-Unexpected duplicate key from PC loop into TM for UnkMod , {0}",
						//	eachValue_MT_UnkMod_T.GetTableNameLowercase());
					}
					else
					{
						this.m_runProperties.GetSDictOfTableModules()
							.Add(eachValue_MT_UnkMod_T.GetTableNameLowercase(), valueTableModule_TM);
					}
				} // EOForeach eachValue_MT_UnkMod_T
			}

		LABEL_RETURN_kdx67_LABEL: ;

			if (null == this.m_runProperties.m_sdictModuleTables)
			{
				this.m_runProperties.m_sdictModuleTables = new SysCollGen.SortedDictionary
						<string, SysCollGen.List<CValueTableModule>>();
			}
			return;
		}



		public void AssignModuleFileNameSubNodes()
		{
			SysCollGen.SortedDictionary<string, CValueTableModule> sdictOfTableModules;
			SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> sdictOfModuleName_to_FileNameNodes;
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> sdictOfParentChilds;
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> sdictOfChildParents;
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableModule>> sdictOfModuleTables;
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableModule>>.KeyCollection sKeysModuleNames_MT;
			SysCollGen.List<CValueTableModule> listTableModules_MT;
			SysCollGen.List<CValueTableTable> listTableTable_CP, listTableTable_PC;
			CValueTableTable valueTableTable_CP = null, valueTableTable_PC;
			CValueTableModule valueTableModule_MT, valueTableModule_TM;
			CModuleFileOwnerInfo moduleFileOwnerInfo;
			int nSubNodeTracker, uuu, nCountLoops;
			int nDebugging_SubNode_Previous = -3;
			string sSubNodeTracker;
			string eachTableNameLowercase_CP_Previous = "";
			//

			sdictOfChildParents = this.m_runProperties.GetSDictOfChildParents();
			sdictOfParentChilds = this.m_runProperties.GetSDictOfParentChilds();

			sdictOfTableModules = m_runProperties.GetSDictOfTableModules();
			sdictOfModuleTables = m_runProperties.GetSDictOfModuleTables();

			sdictOfModuleName_to_FileNameNodes = this.m_runProperties
				.GetSDictOfModuleName_to_FileNameNodes();


			//___ 1 __________ LOOP ______ CP ____________________________


			// Reset all per-module counters of single-line entry items, for SubNodeTracker to utilize.
			foreach (CModuleFileOwnerInfo eachMFOInfo
					in sdictOfModuleName_to_FileNameNodes.Values
				)
			{
				eachMFOInfo.RunningCountOfTablesForSubNodeGeneration = 0;
			}


			foreach (string eachTableNameLowercase_CP in sdictOfChildParents.Keys)  // Merely alphabetical.
			{
				listTableTable_CP = sdictOfChildParents[eachTableNameLowercase_CP];


				// Get the correct per-module entry item counter, for the Left (C) table.
				valueTableModule_TM = sdictOfTableModules[eachTableNameLowercase_CP];

				moduleFileOwnerInfo = sdictOfModuleName_to_FileNameNodes
					[valueTableModule_TM.GetModuleNameLowercaseNoSpaces()];


				// Manage the nSubNodeTracker.

				//  77 / 100 == 0.
				nSubNodeTracker = 11 + 
					moduleFileOwnerInfo .RunningCountOfTablesForSubNodeGeneration /
							this.m_runProperties.GetMaxItems_SubNode_CPPC_File_PerModule();
				sSubNodeTracker = nSubNodeTracker.ToString(); // Yuk ??, tons of string objects created, bashes the GC.

				valueTableModule_TM .SubNode_CP_PerModule = sSubNodeTracker;

				for (int sss=0; sss < listTableTable_CP.Count; sss++)
				{
					valueTableTable_CP = listTableTable_CP[sss];
					valueTableTable_CP .SubNode_CP_PerModule = sSubNodeTracker;

					++(moduleFileOwnerInfo .RunningCountOfTablesForSubNodeGeneration);
				}

				// DEBUGGING ONLY, TEMPORARY.    2013-02-08 13:10pm
				if (-1 < String.Compare(eachTableNameLowercase_CP_Previous, eachTableNameLowercase_CP)) // Debugging only.
				{
					Console.WriteLine("DEBUGGING_WARNING_2752xw3: Bad nonAlphabetical sequence of Left table names in sdict CP!!");
				}
				if (nDebugging_SubNode_Previous > Convert.ToInt32(valueTableTable_CP.SubNode_CP_PerModule))
				{
					Console.WriteLine("DEBUGGING_WARNING_2763xw4: Bad SubNode sequence of Left table names in sdict CP!!");
				}
				eachTableNameLowercase_CP_Previous = eachTableNameLowercase_CP; // Debugging
			} // EOForeach eachTableNameLowercase_CP



			//___ 2 __________ LOOP ______ PC ____________________________


			// Reset all per-module counters of single-line entry items, for SubNodeTracker to utilize.
			foreach (CModuleFileOwnerInfo eachMFOInfo
					in sdictOfModuleName_to_FileNameNodes.Values
				)
			{
				eachMFOInfo.RunningCountOfTablesForSubNodeGeneration = 0;
			}


			foreach (string eachTableNameLowercase_PC in sdictOfParentChilds.Keys)  // Merely alphabetical.
			{
				listTableTable_PC = sdictOfParentChilds[eachTableNameLowercase_PC];


				// Get the correct per-module entry item counter, for the Left (P) table.
				valueTableModule_TM = sdictOfTableModules[eachTableNameLowercase_PC];

				moduleFileOwnerInfo = sdictOfModuleName_to_FileNameNodes
					[valueTableModule_TM.GetModuleNameLowercaseNoSpaces()];


				// Manage the nSubNodeTracker.

				//  77 / 100 == 0.
				nSubNodeTracker = 11 + 
					moduleFileOwnerInfo .RunningCountOfTablesForSubNodeGeneration /
							this.m_runProperties.GetMaxItems_SubNode_CPPC_File_PerModule();
				sSubNodeTracker = nSubNodeTracker.ToString();

				valueTableModule_TM .SubNode_PC_PerModule = sSubNodeTracker;

				for (int sss=0; sss < listTableTable_PC.Count; sss++)
				{
					valueTableTable_PC = listTableTable_PC[sss];
					valueTableTable_PC  .SubNode_PC_PerModule = sSubNodeTracker;

					++(moduleFileOwnerInfo .RunningCountOfTablesForSubNodeGeneration);
				}

			} // EOForeach eachTableNameLowercase_PC


			//______ 3 ___________ LOOPs ___________ MT _____________


			// These nested loops, thru list _MT objects, meet the upcoming needs of
			// 'SubNode_MT_PerModule', for MT .htm file content boundaries.

			sKeysModuleNames_MT = sdictOfModuleTables.Keys;


			foreach(string sModuleNameLowercaseNoSpaces_MT in sKeysModuleNames_MT)
			{

				listTableModules_MT = sdictOfModuleTables[sModuleNameLowercaseNoSpaces_MT];
				nSubNodeTracker = 11;
				sSubNodeTracker = nSubNodeTracker.ToString();


				//____ 3.1 _____________ LOOP _______ MT __________________


				// Thru table names, from sdict MT .Value items, for one module.

				// Nested loop.
				for (uuu=0; uuu < listTableModules_MT.Count; uuu++)
				{

					valueTableModule_MT = listTableModules_MT[uuu];
					valueTableModule_MT .SubNode_MT_PerModule = sSubNodeTracker;

//					// While we have all this info handy, also populate the subnode _MT
//					// info into the other TM SortedDictionary that has Values of type List for CValueTableModule.
//					// This additional location will be very handy later to the Outputter of MT .htm.

					valueTableModule_TM = sdictOfTableModules[valueTableModule_MT.GetTableNameLowercase()];
					valueTableModule_TM .SubNode_MT_PerModule =
							valueTableModule_MT .SubNode_MT_PerModule;

					if ( (uuu >= this.m_runProperties.GetMaxItems_SubNode_CPPC_File_PerModule() )
							&& (0 == (uuu % this.m_runProperties.GetMaxItems_SubNode_CPPC_File_PerModule() ))
						)
					{
						sSubNodeTracker = (++nSubNodeTracker).ToString();						
					}
				} //EOFor uuu
			} // EOForeach sModuleNameLowercaseNoSpaces_MT


			//_______ 4 ___________ LOOP ___________ TM ____________________


			// This loop, thru the one list _TM object, meets the needs of
			// 'SubNode_TM_All', for .htm files
			// 'All-TableModule_*.htm'.
			// (Not that the plural of Modules here is a legacy misnomer.)

			nSubNodeTracker = 11;
			sSubNodeTracker = nSubNodeTracker.ToString();
			nCountLoops = 0;


			foreach (CValueTableModule eachValueTableModule_TM in sdictOfTableModules.Values)
			{
				++ nCountLoops;

				eachValueTableModule_TM.SubNode_TM_All = sSubNodeTracker;


				if ( (nCountLoops >= this.m_runProperties .GetMaxItems_SubNode_TM_TablesChunk() )
						&& (0 == (nCountLoops % this.m_runProperties .GetMaxItems_SubNode_TM_TablesChunk() ))
					)
				{
					sSubNodeTracker = (++nSubNodeTracker).ToString();						
				}
			} //EOForeach vTM

			return;
		}



		public void EnrichTheData()
		{
			SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> sdict_CP, sdict_PC;
			SysCollGen.SortedDictionary<string,                 CValueTableModule> sdict_TM;
			// ?? SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> sdict_CP_accountspayable;
			// ?? SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableModule>> sdict_MT;
			// ?? SysCollGen.List<CValueTableModule> list_MT;
			SysCollGen.List<CValueTableTable> listTableTable_CP, listTableTable_PC;
			CValueTableTable valueTableTable_CP, valueTableTable_PC;
			CValueTableModule valueTableModule;
			//

			sdict_CP = this.m_runProperties.GetSDictOfChildParents();
			sdict_PC = this.m_runProperties.GetSDictOfParentChilds();
			sdict_TM = this.m_runProperties.GetSDictOfTableModules();

			// DEBUGGING ONLY ??
			// ?? sdict_MT = this.m_runProperties.GetSDictOfModuleTables();
			//list_MT = sdict_MT["accountspayable"];


			//___ 1 ____________ LOOP ________________ CP sdict _____________


			foreach (SysCollGen.KeyValuePair<string, SysCollGen.List<CValueTableTable>> eachKvp_CP
					in sdict_CP)
			{
				listTableTable_CP = eachKvp_CP.Value;


				//___ 1.1 ____________ LOOP ________________ CP list _____________


				for (int ccc=0; ccc < listTableTable_CP.Count; ccc++)
				{
					valueTableTable_CP = listTableTable_CP[ccc];

					valueTableModule = sdict_TM
						[valueTableTable_CP .GetTableNameChildLowercase()]; // Child
					valueTableTable_CP .ModuleName_of_LeftTable_LoNoSp = // Left
						valueTableModule .GetModuleNameLowercaseNoSpaces();

					valueTableModule = sdict_TM
						[valueTableTable_CP .GetTableNameParentLowercase()]; // Parent
					valueTableTable_CP .ModuleName_of_RightTable_LoNoSp = // Right
						valueTableModule .GetModuleNameLowercaseNoSpaces();
				} // EOFor ccc
			} // EOForeach eachKvp_CP



			//___ 2 ____________ LOOP ________________ PC sdict _____________


			foreach (SysCollGen.KeyValuePair<string, SysCollGen.List<CValueTableTable>> eachKvp_PC
					in sdict_PC)
			{
				listTableTable_PC = eachKvp_PC.Value;


				//___ 2.1 ____________ LOOP ________________ PC list _____________


				for (int ppp=0; ppp < listTableTable_PC.Count; ppp++)
				{
					valueTableTable_PC = listTableTable_PC[ppp];
					valueTableModule = sdict_TM
						[valueTableTable_PC .GetTableNameParentLowercase()]; // Parent

					valueTableTable_PC .ModuleName_of_LeftTable_LoNoSp = // Left
						valueTableModule .GetModuleNameLowercaseNoSpaces();

					//
					valueTableModule = sdict_TM
						[valueTableTable_PC .GetTableNameChildLowercase()]; // Child

					valueTableTable_PC .ModuleName_of_RightTable_LoNoSp = // Right
						valueTableModule .GetModuleNameLowercaseNoSpaces();
				} // EOFor ppp
			} // EOForeach eachKvp_PC


			// DEBUGGING ONLY.
			// ?? sdict_CP_accountspayable = new SysCollGen.SortedDictionary<string,SysCollGen.List<CValueTableTable>>();
			//foreach (SysCollGen.KeyValuePair<string, SysCollGen.List<CValueTableTable>> eachKvpTemp2 in sdict_CP)
			//{
			//	valueTableTable_CP = eachKvpTemp2.Value[0];
			//	if ("accountspayable" == valueTableTable_CP.ModuleName_of_LeftTable_LoNoSp)
			//	{
			//		sdict_CP_accountspayable.Add(eachKvpTemp2.Key, eachKvpTemp2.Value);
			//	}
			//}


			return;
		}



		//private void PopulateOwnerToModuleTranslations()
		//{
		//	// TODO: ???? this.m_sdictOwnerToModule.  Or manually edit .xlsx, treat this whole thing as one-timer for AX6.2?
		//	// Eh, more engineering than this one-time AX6 effort needs.
		//	// See CModuleFileOwnerInfo.cs
		//}

	} // EOClass
}
