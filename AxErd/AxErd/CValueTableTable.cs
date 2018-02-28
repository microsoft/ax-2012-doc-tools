using              System;
using SysCollGen = System.Collections.Generic;


namespace AxErd
{
	internal class CValueTableTable : IComparable<CValueTableTable>
	{
		private string m_sIComparable = null;
		private string m_sTableNameChild;
		private string m_sTableNameParent;
		private string m_sTableNameChildLowercase;
		private string m_sTableNameParentLowercase;
		private string m_sKeyCP;
		private string m_sKeyPC;
		private int m_nIdentityTT;  // (Just a unique counter originated in X++.)

		internal string SubNode_CP_PerModule;  // "11", then "12", etc.  For the Left (C) table!
		internal string SubNode_PC_PerModule;

		internal string ModuleName_of_LeftTable_LoNoSp; // LowercaseNoSpaces shorted to LoNoSp.
		internal string ModuleName_of_RightTable_LoNoSp;



		//_________________________________________ Start: IComparable
		//
		// IComparable<> requires this.
		public int CompareTo(CValueTableTable _valueTableTable_other)
		{
			if (null == _valueTableTable_other) { return 1; } // 'null' sorts earlier than nonNull, or so this code decides.

			return this.m_sIComparable .CompareTo
				( _valueTableTable_other .GetIComparableString() );
		}

		private string GetIComparableString()
		{
			return this.m_sIComparable;
		}

		internal CValueTableTable CloneThis(string _sIComparable)
		{
			CValueTableTable returnVTT;
			//

			returnVTT = new CValueTableTable
				(
				_sIComparable,
				this.m_nIdentityTT,
				this.m_sTableNameChild,
				this.m_sTableNameParent
				);

			return returnVTT;
		}
		//_________________________________________ End: IComparable


		// ?? CANNOT USE empty .ctor, because need IComparable!    internal CValueTableTable()  // .ctor
		//{
		//}


		internal CValueTableTable  // .ctor
				(
				string _sIComparable,
				int _nIdentityTT,
				string _sTableNameChild,
				string _sTableNameParent
				)
		{
			this.m_sIComparable = _sIComparable.ToLower();
			this.m_nIdentityTT = _nIdentityTT;
			this.m_sTableNameChild = _sTableNameChild;
			this.m_sTableNameParent = _sTableNameParent;

			this.m_sTableNameChildLowercase = this.m_sTableNameChild.ToLower();
			this.m_sTableNameParentLowercase = this.m_sTableNameParent.ToLower();

			this.SetKeys();
		}





		// Get AND Set properties.


		// Simple Getter methods.



		internal int GetIdentityTT()
		{
			return this.m_nIdentityTT;
		}



		internal string GetTableNameParentLowercase()
		{ return this.m_sTableNameParentLowercase;
		}
		internal string GetTableNameChildLowercase()
		{ return this.m_sTableNameChildLowercase;
		}


		internal string GetTableNameParent()
		{ return this.m_sTableNameParent;
		}
		internal string GetTableNameChild()
		{ return this.m_sTableNameChild;
		}


		/// <summary></summary>
		/// <returns>Delimiter is one space character.</returns>
		internal string GetKeyChildDelimParent()
		{
			return this.m_sKeyCP;
		}

		internal string GetKeyParentDelimChild()
		{
			return this.m_sKeyPC;
		}



		internal void SetTableNameParent(string _sTableNameParent)
		{
			this.m_sTableNameParent = _sTableNameParent;

			if (null != this.m_sTableNameChild && null != this.m_sTableNameParent)
			{
				this.SetKeys();
			}
			else
			{
				this.m_sKeyCP = null;
				this.m_sKeyPC = null;
			}
		}

		internal void SetTableNameChild(string _sTableNameChild)
		{
			this.m_sTableNameChild = _sTableNameChild;

			if (null != this.m_sTableNameChild && null != this.m_sTableNameParent)
			{
				this.SetKeys();
			}
			else
			{
				this.m_sKeyCP = null;
				this.m_sKeyPC = null;
			}
		}



		private void SetKeys()
		{
			this.m_sKeyCP = this.m_sTableNameChild.ToLower();
			this.m_sKeyPC = this.m_sTableNameParent.ToLower();
		}
	}
}
