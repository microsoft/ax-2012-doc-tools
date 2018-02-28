using System;


namespace AxErd
{
	interface IOutputter
	{
		/// <summary>
		/// For example, write every .html file that this object knows how to write.
		/// </summary>
		void DoOutputAll();
	}
}
