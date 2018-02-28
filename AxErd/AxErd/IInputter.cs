//  2012/11/30  Friday  11:12am

using               System;
using SysCollGen  = System.Collections.Generic;


namespace AxErd
{
	interface IInputter
	{

/*** JUNK Getters, see instead CRunProperties.
		/// <summary>
		/// Simple Getter. Such as - CustTable might be a Fky child of three other tables, according to Dynamics AX 2012 AOT
		/// (thus three key-value pairs for CustTable). Two other table might have CustTable as their Pky parent.
		/// </summary>
		/// <returns>SortedDictionary, key=ChildTableName, value=ListOf_ParentTableNames. Never null, maybe Count==0.
		/// </returns>
		SysCollGen.SortedDictionary<string, SysCollGen.List<CValueTableTable>> GetSortDictForChildParents();




		/// <summary>
		/// Simple Getter.
		/// </summary>
		/// <returns>SortedDictionary, key=Unique numeric sequence num, value=ListOf_ColCol objects. Never null, maybe Count > 0.
		/// </returns>
		SysCollGen.SortedDictionary<int, SysCollGen.List<CValueColumnColumn>> GetSortDictForColumnsFkyPky();




		/// <summary>
		/// Simple Getter. Such as - CustTable can only be in one module.
		/// </summary>
		/// <returns>SortedDictionary, key=TableName, value=ModuleName. Never null.
		/// </returns>
		SysCollGen.SortedDictionary<string, CValueTableModule> GetSortDictForTableModules();
***/

		//_______________________________



		/// <summary>
		/// Must call this Build method one-time, before calling Get*CP or Get*PC method.
		/// Any second or third etc call to this Build method will behave as a NoOp.
		/// </summary>
		void BuildSortDictForChildParents_and_ParentChilds();



		/// <summary>
		/// No call sequence restrictions. Call this only one time, further times are NoOp.
		/// See corresponding simple Getter method.
		/// </summary>
		void BuildSortDictForColumnsFkyPky();



		/// <summary>
		/// MUST call this method before calling similar Build method for MT!
		/// Must call this Build method one-time, before calling Get*TM method.
		/// Any second or third etc call to this Build method will behave as a NoOp.
		/// </summary>
		void BuildSortDictForTableModules();



		/// <summary>
		/// MUST call this method after calling similar Build method for TM!
		/// Must call this Build method one-time, before calling Get*MT method.
		/// Any second or third etc call to this Build method will behave as a NoOp.
		/// </summary>
		void BuildSortDictForModuleTables();


	
		void AssignModuleFileNameSubNodes();

		void EnrichTheData();

	}
}

//eof
