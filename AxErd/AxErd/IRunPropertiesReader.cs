using System;
using SysCollGen = System.Collections.Generic;

namespace AxErd
{
	interface IRunPropertiesReader
	{
		// Command line parameter values are probably stored behind here.
		string GetInputDelimFilesCommonPrefix();
		string GetPathToInputDelimFiles();
		string GetDelimFileChildParents();  // Means NAME - Get Delim File NAME blah.
		string GetDelimFileColumnColumn();
		string GetDelimFileTableModules();
		//
		string GetOutputHtmlFilesCommonPrefixCPPC();
		string GetOutputHtmlFilesCommonPrefixTMMT();
		string GetPathToOutputHtmlFiles();
		string GetHtmlFileChildParents();
		string GetHtmlFileParentChilds();
		string GetHtmlFileTableModules();
		string GetHtmlFileModuleTables();
		string GetHtmlFileExtension();

		string[] GetRestrictToModulesCPPC(); // Involves both CP and PC.
		string[] GetRestrictToModulesTMMT(); // Involves both TM and MT.

		// STATIC
		//static string BuildModuleNameKey_From_ModuleName(string _sModuleName);

		int GetMaxItems_SubNode_CPPC_File_PerModule(); // Also use for subnode _MT (but not for subnode _TM).
		int GetMaxItems_SubNode_TM_TablesChunk(); // Used for TM .htm files.

		bool GetSkipOutputOfFilesCP();
		bool GetSkipOutputOfFilesTM();

		//------------------------------------

		int GetHighestSubnode_TM_AsInt();

		//------------------------------------

		SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> GetSDictOfChildParents();
		SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> GetSDictOfParentChilds();

		SysCollGen.SortedDictionary<int, SysCollGen.List<CValueColumnColumn>> GetSDictOfColumnsFkyPky();

		SysCollGen.SortedDictionary<string, CValueTableModule> GetSDictOfTableModules();

		SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableModule>> GetSDictOfModuleTables();

	
		SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> GetSDictOfModuleName_to_FileNameNodes();
	}
}
