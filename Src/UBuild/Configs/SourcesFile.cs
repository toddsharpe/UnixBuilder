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

		public string SourcesDir { get; }
		public string OutDir { get; set; }
		public string ConfigDir { get; set; }
		public List<string> Toolchains { get; set; }

		internal SourcesFile(string sourcesDir) : base(Path.Combine(sourcesDir, FileName))
		{
			SourcesDir = sourcesDir;
		}
	}
}