using System;


namespace AxErd
{
	internal class CValueTableModule : IComparable<CValueTableModule>
	{
		private string m_sIComparable;

		private string m_sTableName;
		private string m_sModuleName;
		private string m_sTableNameLowercase;
		private string m_sModuleNameLowercase;
		private string m_sModuleNameLowercaseNoSpaces;
		private string m_sKeyTM;
		private string m_sKeyMT;


		// First would be, for example, the '11' in 'Modu-Budgeting-12-ParentChilds.htm'. Next would be '12' etc.

		// ??? OLD private string m_sSubNode_CPPC_File_PerModule;
		//private string m_sSubNode_TM_TablesChunk;

		internal string SubNode_CP_PerModule;  // "11", then "12", etc.
		internal string SubNode_PC_PerModule;  // Data is duplicated in CValueTableTable in sdict MT, as it must be.

		internal string SubNode_TM_All;  // An alphabetical sequence segment, not per module.
		internal string SubNode_MT_PerModule;


		//internal CValueTableModule()  // .ctor
		//{
		//}


		/// <summary>
		/// Must use this constructor, there is no other .ctor for this class.
		/// </summary>
		/// <param name="_sIComparable">Either TableName, or ModuleName. Your choice.</param>
		/// <param name="_sTableName"></param>
		/// <param name="_sModuleName"></param>
		internal CValueTableModule  // .ctor
				(
				string _sIComparable,
				string _sTableName,
				string _sModuleName
				)
		{
			this.m_sIComparable = _sIComparable.ToLower();
			this.m_sTableName = _sTableName;
			this.m_sModuleName = _sModuleName;

			this.m_sTableNameLowercase = this.m_sTableName.ToLower();
			this.m_sModuleNameLowercase = this.m_sModuleName.ToLower();

			this.m_sModuleNameLowercaseNoSpaces = CRunProperties
				.BuildModuleNameKey_From_ModuleName(this.m_sModuleNameLowercase);

			this.SetKeys();
		}



		// System.IComparable<> requires this.
		public int CompareTo(CValueTableModule _valueTableModule_other)
		{
			if (null == _valueTableModule_other) { return 1; } // 'null' sorts earlier than nonNull, or so this code decides.

			return this.m_sIComparable .CompareTo
				( _valueTableModule_other .GetIComparableString() );
		}
		private string GetIComparableString()
		{
			return this.m_sIComparable;
		}



		// Get AND Set properties.

		// ??? OLD public string SubNode_CPPC_File_PerModule  // string, not int.  Works for CP, PC, MT, but not for TM.
		//{
		//	get { return m_sSubNode_CPPC_File_PerModule; }
		//	set { m_sSubNode_CPPC_File_PerModule = value; }
		//}
		//
		//public string SubNode_TM_TablesChunk  // string, not int.  Works for TM only.
		//{
		//	get { return m_sSubNode_TM_TablesChunk; }
		//	set { m_sSubNode_TM_TablesChunk = value; }
		//}




		// Get OR Set simple methods.

		internal string GetModuleNameLowercaseNoSpaces()
		{
			return this.m_sModuleNameLowercaseNoSpaces;
		}
		internal string GetModuleNameLowercase()
		{
			return this.m_sModuleNameLowercase;
		}

		internal string GetTableNameLowercase()
		{
			return this.m_sTableNameLowercase;
		}

		internal string GetModuleName()
		{
			return this.m_sModuleName;
		}
		internal string GetTableName()
		{
			return this.m_sTableName;
		}


		internal string GetKeyTableDelimModule()
		{
			return this.m_sKeyTM;
		}

		internal string GetKeyModuleDelimTable()
		{
			return this.m_sKeyMT;
		}



		internal void SetModuleName(string _sModuleName)
		{
			this.m_sModuleName = _sModuleName;
			this.m_sModuleNameLowercase = this.m_sModuleName.ToLower();

			if (null != this.m_sTableName && null != this.m_sModuleName)
			{
				this.SetKeys(); // ?? Logic flaw, because only one of these might be set at a given moment?
			}
			else
			{
				this.m_sKeyTM = null;
				this.m_sKeyMT = null;
			}
		}

		internal void SetTableName(string _sTableName)
		{
			this.m_sTableName = _sTableName;

			if (null != this.m_sTableName && null != this.m_sModuleName)
			{
				this.SetKeys();
			}
			else
			{
				this.m_sKeyTM = null;
				this.m_sKeyMT = null;
			}
		}



		private void SetKeys()
		{
			this.m_sKeyTM = this.m_sTableNameLowercase.ToLower();
			this.m_sKeyMT = this.m_sModuleNameLowercaseNoSpaces.ToLower();
		}
	}
}
