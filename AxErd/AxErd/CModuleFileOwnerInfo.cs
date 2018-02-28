using              System;
using SysCollGen = System.Collections.Generic;


namespace AxErd
{

	class CModuleFileOwnerInfo : IComparable<CModuleFileOwnerInfo>
	{

		public const string M_sUnknown_ErdModuleFileName_PlaceholderValue = "Erd-Links-.htm";

		// Fields.

		// Reference this field directly.
		// Reset and REUSE this field, for CP tables, then for PC tables.
		// This is a work field, not a storer of permanent state.
		// In sdict MT, module name is the key.  So this field is
		// incremented per table but within module.
		internal int RunningCountOfTablesForSubNodeGeneration = 0;

		//private string m_sIComparable;
		private string m_sFormalModuleNameLowercaseNoSpaces;
		private string m_sFormalModuleNameLowercase;
		private string m_sFormalModuleName;
		private string m_sFileNameNode;
		private string m_sInformalOwnerName;
		private string m_sInformalOwnerNameLowercase;
		private string m_sHighestSubnode_MT = "11";
		private string m_sErdModuleFileName;
		private EnumModuleNameType m_enModuleNameType;



		// Constructor.
		public CModuleFileOwnerInfo
			(
			//string _sIComparable,
			string _sFormalModuleName,
			string _sFileNameNode,
			string _sInformalOwnerName,
			string _sHighestSubnode_MT,
			EnumModuleNameType _enModuleNameType,
			string _sErdModuleFileName
			)
		{
			//this.m_sIComparable = _sIComparable.ToLower();

			this.m_sFormalModuleName  = _sFormalModuleName;
			this.m_sFileNameNode      = _sFileNameNode;
			this.m_sInformalOwnerName = _sInformalOwnerName;
			this.m_sHighestSubnode_MT = _sHighestSubnode_MT;
			this.m_enModuleNameType   = _enModuleNameType;
			this.m_sErdModuleFileName = _sErdModuleFileName;

			this.m_sInformalOwnerNameLowercase        = this.m_sInformalOwnerName.ToLower();
			this.m_sFormalModuleNameLowercase         = this.m_sFormalModuleName.ToLower();
			this.m_sFormalModuleNameLowercaseNoSpaces = this.m_sFormalModuleNameLowercase.Replace(" ","");
		}


		// System.IComparable<> requires this.
		public int CompareTo(CModuleFileOwnerInfo _moduFileOwnerInfo_other)
		{
			if (null == _moduFileOwnerInfo_other) { return 1; } // 'null' sorts earlier than nonNull, or so this code decides.

			return this.m_sFormalModuleName.CompareTo(_moduFileOwnerInfo_other.GetFormalModuleName());
		}



		// Getter methods, simple.
		public string GetFormalModuleNameLowercaseNoSpaces() { return this.m_sFormalModuleNameLowercaseNoSpaces; }
		public string GetFormalModuleNameLowercase() { return this.m_sFormalModuleNameLowercase; }
		public string GetFormalModuleName() { return this.m_sFormalModuleName; }

		public string GetFileNameNode() { return this.m_sFileNameNode; }
		public string GetInformalOwnerName() { return this.m_sInformalOwnerName; }
		public string GetInformalOwnerNameLowercase() { return this.m_sInformalOwnerNameLowercase; }

		public string GetHighestSubnode_MT() { return this.m_sHighestSubnode_MT; }
		public int GetHighestSubnode_MT_AsInt()
		{
			return Convert.ToInt32(this.m_sHighestSubnode_MT);
		}

		public EnumModuleNameType GetEnumModuleNameType() { return this.m_enModuleNameType; }

		public string GetErdModuleFileName() { return this.m_sErdModuleFileName; }




		// Captures hardcoded data into objects.
		static public SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo>
			BuildSortedDictionaryOf_CModuleFileOwnerInfos()
		{
			SysCollGen.SortedDictionary<string, CModuleFileOwnerInfo> sdictModuFileOwnerInfo;
			CModuleFileOwnerInfo moduFileOwnerInfo;
			//

			sdictModuFileOwnerInfo = new SysCollGen.SortedDictionary<string,CModuleFileOwnerInfo>();



			// LIVE EXAMPLE
			moduFileOwnerInfo = new CModuleFileOwnerInfo
				(
				"Accounts payable",      // Formal name of module.
				"AcctPay",               // File name node for module.
				"Accounts Payable",      // Informal name of owner (owner is not 100% exactly same as module, but close).
				"11",
				EnumModuleNameType.FormalModuleName,  // .FormalModuleName means this is a real module in the AX6 app workspace UI.
				"Erd-Links-AcctPay.htm"  // Probably not used?
				);
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);



			moduFileOwnerInfo = new CModuleFileOwnerInfo("Accounts receivable", "AcctRecv", "Accounts Receivable",
				"11", EnumModuleNameType.FormalModuleName, "Erd-Links-AcctRecv.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Budgeting", "Budget", "Budget",
				"11", EnumModuleNameType.FormalModuleName, "Erd-Links-Budget.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Cash and bank management", "CashBankMangt", "Bank",
				"11", EnumModuleNameType.FormalModuleName, "Erd-Links-CashBankMangt.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Compliance and internal controls", "ComplianceIntCtrl", "Compliance",
				"11", EnumModuleNameType.FormalModuleName, "Erd-Links-ComplianceIntCtrl.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Financial management", "FinMangt", "FIM",
				"11", EnumModuleNameType.FormalModuleName, "Erd-Links-FinMangt.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Fixed assets", "FixedAssets", "Fixed Assets",
				"11", EnumModuleNameType.FormalModuleName, "Erd-Links-FixedAssets.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("General ledger", "GenLed", "General Ledger",
				"12", EnumModuleNameType.FormalModuleName, "Erd-Links-GenLed.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Human resources", "HumanRes", "HRM",
				"14", EnumModuleNameType.FormalModuleName, "Erd-Links-HumanRes.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Inventory and warehouse management", "InvenWareMangt", "Inventory",
				"13", EnumModuleNameType.FormalModuleName, "Erd-Links-InvenWareMangt.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Procurement and sourcing", "ProcureSrc", "Trade and Source",
				"13", EnumModuleNameType.FormalModuleName, "Erd-Links-ProcureSrc.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Production control", "ProductionCtrl", "Control",
				"11", EnumModuleNameType.FormalModuleName, "Erd-Links-ProductionCtrl.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Project management and accounting", "ProjectMangtAccounting", "Project",
				"13", EnumModuleNameType.FormalModuleName, "Erd-Links-ProjectMangtAccounting.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Retail", "Retail", "Retail",
				"14", EnumModuleNameType.FormalModuleName, "Erd-Links-Retail.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Sales and marketing", "SalesMarket", "CRM",
				"12", EnumModuleNameType.FormalModuleName, "Erd-Links-SalesMarket.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Service management", "ServiceMangt", "Service Management",
				"11", EnumModuleNameType.FormalModuleName, "Erd-Links-ServiceMangt.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Travel and expense", "TravelExp", "Expense Management",
				"11", EnumModuleNameType.FormalModuleName, "Erd-Links-TravelExp.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);



			// Continue, now with Informal Owner Names.



			moduFileOwnerInfo = new CModuleFileOwnerInfo("AIF", "AIF", "AIF",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("AppConfig", "AppConfig", "AppConfig",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Business Intelligence", "BusIntel", "Business Intelligence",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Client", "Client", "Client",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Compiler", "Compiler", "Compiler",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Developer and Partner Tools", "DevPartTool", "DPT",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Dynamics Online", "DynOnline", "Dynamics Online",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Enterprise Portal", "EP", "EP",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Global financial management - _W", "GfmW", "GFM-W",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Global financial management - Asia Pacific", "GfmAPac", "GFM-APAC",
				"13", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Global financial management - Eastern Europe", "GfmEE", "GFM-EE",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Global financial management - Latin America", "GfmLatAm", "GFM-LATAM",
				"12", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Global financial management - North America", "GfmNA", "GFM-NA",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Global financial management - Russia", "GfmRu", "GFM-RU",
				"12", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Global financial management - Western Europe", "GfmWE", "GFM-WE",
				"12", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Infrastructure", "Infrastructure", "Infrastructure",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Manufacturing", "Manufacturing", "Manufacturing",
				"13", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);


			// ??? JUNK entry? see instead "Unknown module".
			moduFileOwnerInfo = new CModuleFileOwnerInfo("No Owner", "NoOwner", "No Owner",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);


			moduFileOwnerInfo = new CModuleFileOwnerInfo("Office Business App", "OfficeBusApp", "OBA",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Public Sector SL1", "PublicSectorSL1", "Public Sector SL1",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Server and Tools", "ServerTools", "Server and Tools",
				"12", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Setup", "Setup", "Setup",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Tax", "Tax", "Tax",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-Tax.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);

			moduFileOwnerInfo = new CModuleFileOwnerInfo("Upgrade framework", "UpgradeFramework", "Upgrade framework",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);


			// Instead of "No Owner".
			moduFileOwnerInfo = new CModuleFileOwnerInfo
				(
				CRunProperties.M_readonly_sUnknownModule,
				"UnknownMod",
				CRunProperties.M_readonly_sUnknownModule,
				"11",
				EnumModuleNameType.InformalOwnerName,
				"Erd-Links-.htm"
				);
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);


			moduFileOwnerInfo = new CModuleFileOwnerInfo("Workflow", "Workflow", "Workflow",
				"11", EnumModuleNameType.InformalOwnerName, "Erd-Links-.htm");
			sdictModuFileOwnerInfo.Add(moduFileOwnerInfo.GetFormalModuleNameLowercaseNoSpaces(), moduFileOwnerInfo);


			return sdictModuFileOwnerInfo;
		}


	} // EOClass
}
