
using              System;
using SysCollGen = System.Collections.Generic;
using SysIo      = System.IO;
using SysTex     = System.Text;


namespace AxErd
{

	/// <summary>
	/// This structure is ref passed between caller-callee methods,
	/// to simplify the parameter signatures.
	/// </summary>
	class CModSubNodeState1
	{

		internal CValueTableModule m_valueTableModule_MT;
		internal CValueTableModule m_valueTableModule_MT_Previous;
		internal CValueTableTable m_valueTableTable;
		internal CModuleFileOwnerInfo m_moduleFileOwnerInfo;
		internal CPreviousNextFileSubNodes m_previousNextFileSubNodes;

		internal SysIo.StreamWriter m_streamWriter;

		internal int m_nCountEntriesWrittenForCurrentModule = 0; // Across subnodes.

		private int    m_nSubNode_of_LatestCreatedFile_Int    =  10;
		private string m_sSubNode_of_LatestCreatedFile_String = "10";

		internal string m_sLatestLeftAnchorTableNameLowercase; // <a name= , for jumpTo #location redundancy prevention.


		//______________________


		internal void SubNode_of_LatestCreatedFile_Reinitialize()
		{
			this.SubNode_of_LatestCreatedFile_Int = 10;
		}
		internal string SubNode_of_LatestCreatedFile_String
		{
			get { return this.m_sSubNode_of_LatestCreatedFile_String; }

			set
			{
				this.m_sSubNode_of_LatestCreatedFile_String = value;
				// Keep Int in sync with String.
				this.m_nSubNode_of_LatestCreatedFile_Int =
					Convert.ToInt32(this.m_sSubNode_of_LatestCreatedFile_String);
			}
		}
		internal int SubNode_of_LatestCreatedFile_Int
		{
			get { return this.m_nSubNode_of_LatestCreatedFile_Int; }

			set
			{
				this.m_nSubNode_of_LatestCreatedFile_Int = value;
				// Keep Int in sync with String.
				this.m_sSubNode_of_LatestCreatedFile_String =
					this.m_nSubNode_of_LatestCreatedFile_Int.ToString();
			}
		}

		//__________________




		internal string ToString_ForDiagnostics()
		{
			SysTex.StringBuilder sbuilder = new SysTex.StringBuilder(1028);
			string sDelim = "  ---  ";
			//

			sbuilder.Append("M-T ModuleName: ");
			sbuilder.Append
				((null != this.m_valueTableModule_MT ) ? this.m_valueTableModule_MT.GetModuleName() : ".Null."
				);

			sbuilder.Append(sDelim);
			sbuilder.Append("M-T TableName: ");
			sbuilder.Append
				((null != this.m_valueTableModule_MT ) ? this.m_valueTableModule_MT.GetTableName() : ".Null."
				);

			sbuilder.Append(sDelim);
			sbuilder.Append("C-P ParentTableName: ");
			sbuilder.Append
				(
				(null != this.m_valueTableTable) ? this.m_valueTableTable.GetTableNameParent() : ".Null."
				);

			sbuilder.Append(sDelim);
			sbuilder.Append("Count of entries written for current M-T module: ");
			sbuilder.Append(this.m_nCountEntriesWrittenForCurrentModule.ToString());

			return sbuilder.ToString();
		}

	}
}
