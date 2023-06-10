using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBuild.Configs
{
	internal class ToolchainFile : ConfigFile
	{
		internal static readonly string FileNamePrefix = "TOOLCHAIN_";

		public string Name { get; }
		public string Bin { get; set; }
		public string Prefix { get; set; }
		public List<string> Flags { get; set; }

		internal ToolchainFile(string sourceDir, string name) : base(Path.Combine(sourceDir, FileNamePrefix + name))
		{
			Name = name;
			Flags ??= new List<string>();
		}
	}
}