using System;


namespace AxErd
{
	internal class CValueColumnColumn
	{
		// TT identity is used as key when SortedDictionary<int,List> is used.
		private int m_nIdentityTT;  // Fky to TT record. (Just a unique counter originated in X++.)

		private int m_nIdentityCC;  // Pky for this CC record. (Just a unique counter originated in X++.)

		private string m_sFieldFky;
		private string m_sFieldRelPky;



		//private CValueColumnColumn()  // .ctor
		//{
		//}

		internal CValueColumnColumn  // .ctor
				(
				int _nIdentityTT,
				int _nIdentityCC,
				string _sFieldFky,
				string _sFieldRelPky
				)
		{
			this.m_nIdentityTT = _nIdentityTT;
			this.m_nIdentityCC = _nIdentityCC;
			this.m_sFieldFky = _sFieldFky;
			this.m_sFieldRelPky = _sFieldRelPky;
		}



		internal int GetIdentityTT()
		{
			return this.m_nIdentityTT;
		}
		internal int GetIdentityCC()
		{
			return this.m_nIdentityCC;
		}



		internal string GetFieldFky()
		{
			return this.m_sFieldFky;
		}
		internal string GetFieldRelPky()
		{
			return this.m_sFieldRelPky;
		}
	}
}
