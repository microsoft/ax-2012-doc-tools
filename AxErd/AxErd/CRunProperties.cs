using System;
using SysCollGen = System.Collections.Generic;


namespace AxErd
{
	/// <summary>
	/// Only CMainProgram has an instance of this class that is also declared as this class type.
	/// Other references are given out only as IRunPropertiesReader.
	/// </summary>
	class CRunProperties
				: IRunPropertiesReader
	{
		// INTERNAL, not PRIVATE!  See interface IRunPropertiesReader as the technique
		// for satisfying read-only users without exposing internal fields.

		static public readonly string M_readonly_sUnknownModule = "Unknown module";  // Cannot change unless fix Default.htm and maybe others too.
		static public readonly string M_readonly_sUnknownModule_LowercaseNoSpaces = "unknownmodule";

		// Command line parameter values and defaults.
		internal string m_sInputDelimFilesCommonPrefix = "";
		internal string m_sPathToInputDelimFiles = "";
		internal string m_sDelimFileChildParents = "TabDelim-Child-Parents.txt";
		internal string m_sDelimFileColumnColumn = "TabDelim-Column-Column.txt";
		internal string m_sDelimFileTableModules = "TabDelim-Table-Modules.txt";  // Might be 'Owner' concept instead of 'Module'?
		//
		internal string m_sOutputHtmlFilesCommonPrefixCPPC = "Fky";
		internal string m_sOutputHtmlFilesCommonPrefixTMMT = "";
		internal string m_sPathToOutputHtmlFiles = "";
		internal string m_sHtmlFileChildParents = "ChildParents"; // Not yet including the ".htm" extension.
		internal string m_sHtmlFileParentChilds = "ParentChilds";
		internal string m_sHtmlFileTableModules = "All-TableModule";  // 'TableModules' plural is a misnomer.
		internal string m_sHtmlFileModuleTables = "Module"; // As in Module-Tables, but the shorter prefix is better.
		internal string m_sHtmlFileExtension = ".htm";
		//
		internal string[] m_sRestrictToModulesCPPC = new string[] { };
		internal string[] m_sRestrictToModulesTMMT = new string[] { };
		//
		internal int m_nMaxItems_SubNode_CPPC_File_PerModule = 200; // Kinda should be static, but interface does not allow static specs.
		internal int m_nMaxItems_SubNode_TM_TablesChunk = 200;
		//
		internal bool m_bSkipOutputOfFilesCP = false;
		internal bool m_bSkipOutputOfFilesTM = false;
		//
		//internal int m_nInitialSleepSeconds = 0;  // For this parameter, see CMainProgram.cs.


		//----------------------------

		internal int m_nHighestSubnode_TM = 49;  // Hardcoded, from examining output TM .htm file names.

		//----------------------------


		internal SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> m_sdictChildParents;
		internal SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> m_sdictParentChilds;

		internal SysCollGen.SortedDictionary<int, SysCollGen.List<CValueColumnColumn>> m_sdictColumnsFkyPky;


		// ?? internal SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableModule>> m_sdictTableModules; // ??? Why List, better if just CValueTableModule (one is max).
		internal SysCollGen.SortedDictionary<string, CValueTableModule> m_sdictTableModules;


		internal SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableModule>> m_sdictModuleTables;
		//

		internal SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> m_sdictModuleName_to_FileNameNodes;
		//

		public CRunProperties()  // .ctor
		{
		}



		public string GetInputDelimFilesCommonPrefix()
		{
			return this.m_sInputDelimFilesCommonPrefix;
		}

		public string GetPathToInputDelimFiles()
		{
			return this.m_sPathToInputDelimFiles;
		}

		public string GetDelimFileChildParents()
		{
			return this.m_sDelimFileChildParents;
		}
		public string GetDelimFileColumnColumn()
		{
			return this.m_sDelimFileColumnColumn;
		}
		public string GetDelimFileTableModules()
		{
			return this.m_sDelimFileTableModules;
		}



		public string GetOutputHtmlFilesCommonPrefixCPPC()
		{
			return this.m_sOutputHtmlFilesCommonPrefixCPPC;
		}

		public string GetOutputHtmlFilesCommonPrefixTMMT()
		{
			return this.m_sOutputHtmlFilesCommonPrefixTMMT;
		}



		public string GetPathToOutputHtmlFiles()
		{
			return this.m_sPathToOutputHtmlFiles;
		}

		public string GetHtmlFileChildParents()
		{
			return this.m_sHtmlFileChildParents;
		}

		public string GetHtmlFileParentChilds()
		{
			return this.m_sHtmlFileParentChilds;
		}

		public string GetHtmlFileTableModules()
		{
			return this.m_sHtmlFileTableModules;
		}

		public string GetHtmlFileModuleTables()
		{
			return this.m_sHtmlFileModuleTables;
		}


		public string GetHtmlFileExtension()
		{
			return m_sHtmlFileExtension;
		}




		public string[] GetRestrictToModulesCPPC()
		{
			return this.m_sRestrictToModulesCPPC;
		}

		public string[] GetRestrictToModulesTMMT()
		{
			return this.m_sRestrictToModulesTMMT;
		}


		// STATIC, thus Not part of interface, is beyond the interface.
		static public string BuildModuleNameKey_From_ModuleName(string _sModuleName)
		{
			return _sModuleName.ToLower().Replace(" ","");
		}




		public int GetMaxItems_SubNode_CPPC_File_PerModule()
		{
			return this.m_nMaxItems_SubNode_CPPC_File_PerModule;
		}

		public int GetMaxItems_SubNode_TM_TablesChunk()
		{
			return this.m_nMaxItems_SubNode_TM_TablesChunk;
		}




		public bool GetSkipOutputOfFilesCP()
		{
			return this.m_bSkipOutputOfFilesCP;
		}

		public bool GetSkipOutputOfFilesTM()
		{
			return this.m_bSkipOutputOfFilesTM;
		}



		//------------------------------------


		public int GetHighestSubnode_TM_AsInt()
		{
			return this.m_nHighestSubnode_TM;
		}

		//------------------------------------



		public SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> GetSDictOfChildParents()
		{
			return this.m_sdictChildParents;
		}

		public SysCollGen.SortedDictionary<int, SysCollGen.List<CValueColumnColumn>> GetSDictOfColumnsFkyPky()
		{
			return this.m_sdictColumnsFkyPky;
		}

		public SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> GetSDictOfParentChilds()
		{
			return this.m_sdictParentChilds;
		}




		public SysCollGen.SortedDictionary<string, CValueTableModule> GetSDictOfTableModules()
		{
			return this.m_sdictTableModules;
		}

		public SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableModule>> GetSDictOfModuleTables()
		{
			return this.m_sdictModuleTables;
		}




		// In essence this amounts to a simple Getter.
		public SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo>
		GetSDictOfModuleName_to_FileNameNodes()
		{
			if (null == this.m_sdictModuleName_to_FileNameNodes)
			{
				this.m_sdictModuleName_to_FileNameNodes =
					CModuleFileOwnerInfo.BuildSortedDictionaryOf_CModuleFileOwnerInfos();
			}

			return this.m_sdictModuleName_to_FileNameNodes;
		}


	} // EOClass
}
