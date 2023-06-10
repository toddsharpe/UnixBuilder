using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Threading.Tasks;

namespace UBuild.Configs
{
	internal class SourcesFile : ConfigFile
	{
		internal static readonly string FileName = "SOURCES";

		public string OutDir { get; set; }

		public List<string> Toolchains { get; set; }

		internal SourcesFile(string project) : base(Path.Combine(project, FileName))
		{

		}
	}
}