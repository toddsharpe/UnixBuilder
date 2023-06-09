using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Threading.Tasks;

namespace UBuild.Configs
{
	internal class ProjectFile : ConfigFile
	{
		internal static readonly string FileName = "PROJECT";

		public string OutDir { get; set; }

		public List<string> Toolchains { get; set; }

		internal ProjectFile(string project) : base(Path.Combine(project, FileName))
		{

		}
	}
}