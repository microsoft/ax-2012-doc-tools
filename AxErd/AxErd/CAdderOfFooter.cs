using System;
using SysCollGen  = System.Collections.Generic;
using SysIo       = System.IO;
using SysSecuPerm = System.Security.Permissions;


namespace AxErd
{

	class CAdderOfFooter
	{

		private IRunPropertiesReader m_irunProperties;



		public CAdderOfFooter(IRunPropertiesReader _irunProperties)  // .ctor
		{
			this.m_irunProperties = _irunProperties;
			return;
		}



		public void LoopThroughAllEligibleFilesThatNeedAddedFooter()
		{
			SysCollGen.IEnumerable<string> ienumerable_ofPathFileNames;
			string sCurrentFileName;
			int nIndex, nLoopCounter = 0;
			//

			ienumerable_ofPathFileNames = SysIo.Directory.EnumerateFiles
				(
				this.m_irunProperties.GetPathToHtmlFilesNeedingFooter_In(),
				this.m_irunProperties.GetRestrictFilesToThisFileNamePatternForFooter_In(),
				SysIo.SearchOption.TopDirectoryOnly
				);

			foreach (string eachPathFileName in ienumerable_ofPathFileNames)
			{
				nIndex = eachPathFileName.LastIndexOf(@"\");
				sCurrentFileName = eachPathFileName.Substring(nIndex + 1);

				this.AddFooterToOneHtmlFile
					(
					this.m_irunProperties.GetPathToHtmlFilesNeedingFooter_In(),
					this.m_irunProperties.GetPathToHtmlFilesWithAddedFooter_Out(),
					sCurrentFileName,
					this.m_irunProperties.GetFooter_MsCom()
					);

				++nLoopCounter;

			} // foreach eachPathFileName

			Console.Out.WriteLine("{0} = Count of files to which FOOTER was added.", nLoopCounter);

			return;
		}




		private void AddFooterToOneHtmlFile
			(
			string _sPathToInputFile,
			string _sPathToOutputFile,
			string _sFileNameInAndOut,
			string _sFooter
			)
		{
			SysIo.FileStream fstreamRead, fstreamWrite;
			SysIo.StreamReader streamRead = null;
			SysIo.StreamWriter streamWrite = null;
			SysSecuPerm.FileIOPermission fileIoPerm;
			string sWholeFileContents, sWholeFilePlusFooter;
			//

			try
			{
				fileIoPerm = new SysSecuPerm.FileIOPermission(SysSecuPerm.PermissionState.Unrestricted);
				fileIoPerm.Assert();

				fstreamRead = new SysIo.FileStream
					(
					_sPathToInputFile + _sFileNameInAndOut,
					SysIo.FileMode.Open,
					SysIo.FileAccess.Read,
					SysIo.FileShare.Read
					);

				streamRead = new SysIo.StreamReader(fstreamRead);
				sWholeFileContents = streamRead.ReadToEnd();
				streamRead.Close();

				sWholeFilePlusFooter = sWholeFileContents.Replace
					(@"</body>", _sFooter + @"</body>");

				fstreamWrite = new SysIo.FileStream
					(
					_sPathToOutputFile + _sFileNameInAndOut,
					SysIo.FileMode.Create,
					SysIo.FileAccess.Write,
					SysIo.FileShare.None
					);

				streamWrite = new SysIo.StreamWriter(fstreamWrite);
				streamWrite.Write(sWholeFilePlusFooter);
				streamWrite.Flush();
				streamWrite.Close();
			}
			finally
			{
				if (null != streamRead) { streamRead.Dispose(); }
				if (null != streamWrite) { streamWrite.Dispose(); }

				SysSecuPerm.FileIOPermission.RevertAssert();
				fileIoPerm = null;
			}

			return;
		}

	} // EOClass CAdderOfFooter
}
