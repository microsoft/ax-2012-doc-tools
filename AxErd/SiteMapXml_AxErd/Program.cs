
//  2013/05/10  Friday  14:35pm
//
//  This generates a sitemap.XML for AxErd.
//
//  TO RUN THIS:
//    [C:\Main\VisStudioProjs\AxErd33\AxErd\AxErd\_RelatedFiles_AxErd\Deploy\]
//    >> C:\Main\VisStudioProjs\AxErd33\SiteMapXml_AxErd\SiteMapXml_AxErd\bin\Debug\SiteMapXml_AxErd.exe
//
//

using              System;
using SysCollGen = System.Collections.Generic;
using SysIo      = System.IO;
using SysTx      = System.Text;
//using System.Linq;


namespace SiteMapXml_AxErd
{

	public class Program  // SiteMapXml_AxErd.exe
	{

		const string M_sUrl_Loc = "http://www.microsoft.com/dynamics/ax/erd/ax2012r2/";


		static public int Main(string[] args)
		{
			Program progm;
			//

			progm = new Program();
			progm.StartItAll();

			return 0;
		}




		public void StartItAll()
		{
			SysIo.FileStream fsWrite;
			SysIo.StreamWriter streamWriter;
			SysCollGen.IEnumerable<string> ienumerStrings;
			SysCollGen.SortedList<string,string> sorListFilenameNull; // We only use the Key, not need the Value.
			SysTx.StringBuilder sBuilder = new SysTx.StringBuilder(512);
			string sFileName,
				sUrl_OneFileName;
			string sUrlParam_LastMod,
				sUrlParam_ChangeFreq,
				sUrlParam_Priority;
			//

			sorListFilenameNull = new SysCollGen.SortedList<string,string>(1024);

			ienumerStrings = SysIo.Directory.EnumerateFiles
				(
				".\\",
				"*.htm",  // sitemap.xml only wants files that should be crawled, not .png etc.
				SysIo.SearchOption.TopDirectoryOnly
				);

			foreach (string eachFileName in ienumerStrings)
			{
				sorListFilenameNull.Add(eachFileName,null);
			}


			fsWrite = new SysIo.FileStream
				(
				".\\sitemap.xml",
				SysIo.FileMode.Create, // Create-Or-Overwrite.
				SysIo.FileAccess.Write,
				SysIo.FileShare.None
				);

			streamWriter = new SysIo.StreamWriter
				(
				fsWrite,
				SysTx.Encoding.UTF8  // Required for sitemap files.
				);



			sBuilder.Length = 0;
			sBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sBuilder.Append    ("<urlset");
			sBuilder.AppendLine("\txmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

			/***
	xmlns:xsi="www.w3.org/2001/XMLSchema-instance"
	xsi:schemaLocation="http://www.sitemaps.org/schemas/sitemap/0.9
		http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"
			***/
			//
			//sBuilder.AppendLine("\txmlns:xsi=\"www.w3.org/2001/XMLSchema-instance\"");
			//sBuilder.AppendLine("\txsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9");
			//sBuilder.AppendLine("\t\thttp://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\"");
	
			sBuilder.AppendLine();

			sBuilder.Append    ("<!-- GM  ,  SiteMapXml_AxErd.exe  ,  UTC ");
			sBuilder.Append    (DateTime.UtcNow.ToString());
			sBuilder.AppendLine(" -->");

			streamWriter.WriteLine(sBuilder.ToString());  // XML header tags.



			foreach (SysCollGen.KeyValuePair<string,string> eachKVPair in sorListFilenameNull)
			{
				sFileName = eachKVPair.Key;
				if (sFileName.StartsWith(".\\") == true)
				{
					sFileName = sFileName.Substring(2);
				}


				if (sFileName.StartsWith("All-TableModule-") == true) // TM
				{
					sUrlParam_LastMod = "2013-05-08T22:33:44+00:00";
					sUrlParam_ChangeFreq = "yearly";
					sUrlParam_Priority = ".25";
				}
				else if (sFileName.StartsWith("All-TableModule-") == true) // MT
				{
					sUrlParam_LastMod = "2013-05-08T22:33:44+00:00";
					sUrlParam_ChangeFreq = "yearly";
					sUrlParam_Priority = ".55";
				}
				else if (sFileName.StartsWith("Fky-") == true)
				{

					if (sFileName.Contains("ChildParents") == true) // CP
					{
						sUrlParam_LastMod = "2013-05-08T22:33:44+00:00";
						sUrlParam_ChangeFreq = "yearly";
						sUrlParam_Priority = ".35";
					}
					else // "ParentChilds"  PC
					{
						sUrlParam_LastMod = "2013-05-08T22:33:44+00:00";
						sUrlParam_ChangeFreq = "yearly";
						sUrlParam_Priority = ".45";
					}
		
				}
				else if (sFileName.StartsWith("Erd-One-") == true) // Erd-One-
				{
					sUrlParam_LastMod = "2013-05-08T22:33:44+00:00";
					sUrlParam_ChangeFreq = "monthly";
					sUrlParam_Priority = ".66";
				}
				else if (sFileName.StartsWith("Erd-Links-") == true) // Erd-Links-
				{
					sUrlParam_LastMod = "2013-05-08T22:33:44+00:00";
					sUrlParam_ChangeFreq = "monthly";
					sUrlParam_Priority = ".63";
				}
				else if (sFileName.StartsWith("Disclaimer") == true) // Disclaimer
				{
					continue; // robots.txt should Disallow crawl of this file!

					//sUrlParam_LastMod = "2013-05-08T22:33:44+00:00";
					//sUrlParam_ChangeFreq = "never";
					//sUrlParam_Priority = ".00";
				}
				else // Default.htm, Help*.htm etc.  The few specialty files.
				{
					sUrlParam_LastMod = "2013-05-08T22:33:44+00:00";
					sUrlParam_ChangeFreq = "monthly";
					sUrlParam_Priority = ".75";
				}



				sUrl_OneFileName = this.BuildXmlUrlForOneFileName
					(
					Program.M_sUrl_Loc,
					sFileName,
					sUrlParam_LastMod,
					sUrlParam_ChangeFreq,
					sUrlParam_Priority
					);
				streamWriter.Write(sUrl_OneFileName);
			} // EOForEach eachKVPair


			streamWriter.WriteLine("</urlset>");


			streamWriter.Flush();
			streamWriter.Close();
			streamWriter.Dispose();


			return;
		}




		private string BuildXmlUrlForOneFileName
			(
			string _sUrl_Loc,
			string _sFileName,
			string _sUrlParam_LastMod,
			string _sUrlParam_ChangeFreq,
			string _sUrlParam_Priority
			)
		{
			SysTx.StringBuilder sBuilder = new SysTx.StringBuilder(512);
			//

			sBuilder.AppendLine("<url>");

			sBuilder.Append    ("\t<loc>");
			sBuilder.Append    (_sUrl_Loc);
			sBuilder.Append    (_sFileName);
			sBuilder.AppendLine("</loc>");

			sBuilder.Append    ("\t<lastmod>");
			sBuilder.Append    (_sUrlParam_LastMod);
			sBuilder.AppendLine("</lastmod>");

			sBuilder.Append    ("\t<changefreq>");
			sBuilder.Append    (_sUrlParam_ChangeFreq);
			sBuilder.AppendLine("</changefreq>");

			sBuilder.Append    ("\t<priority>");
			sBuilder.Append    (_sUrlParam_Priority);
			sBuilder.AppendLine("</priority>");

			sBuilder.AppendLine("</url>");


			return sBuilder.ToString();
		}


	} //EOClass

} //EONamespace
