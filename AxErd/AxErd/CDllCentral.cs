/***
GeneMi  ,  AxErdModule tool  ,  2012/Dec/08 Saturday 22:51pm

Status:
	[B] Needs table FOREIGN KEY COLUMNS <br /> along side child table name.


***/

using System;
using SysCollGen = System.Collections.Generic;


namespace AxErd
{
	class CDllCentral
	{

		private CRunProperties m_crunProperties;


		public CDllCentral()  // .ctor
		{
		}


		public CDllCentral  // .ctor
				(
				CRunProperties _crunProperties
				)
		{
			this.m_crunProperties = _crunProperties;
		}


		public void MainDll_DoItAll()
		{
			CInputterFromDelimFiles inputterTdf; // 'Tdf' means "Tab delimited files".
			COutputterToHtmlFiles   outHtmlFiles;
			//

			inputterTdf = new CInputterFromDelimFiles(this.m_crunProperties);


			inputterTdf.BuildSortDictForChildParents_and_ParentChilds();

			inputterTdf.BuildSortDictForColumnsFkyPky();

			inputterTdf.BuildSortDictForTableModules();

			inputterTdf.BuildSortDictForModuleTables();


			inputterTdf.AssignModuleFileNameSubNodes();
			inputterTdf.EnrichTheData();


			outHtmlFiles = new COutputterToHtmlFiles(this.m_crunProperties);
			outHtmlFiles.DoOutputAll();

			return;
		}
	}
}
