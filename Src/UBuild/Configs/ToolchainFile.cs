using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBuild.Configs
{
	internal class ToolchainFile : ConfigFile
	{
		internal static readonly string FileNamePrefix = "TOOLCHAIN_";

		public string Bin { get; set; }
		public string Prefix { get; set; }
		public List<string> Flags { get; set; }

		//Tools
		public string Gcc => Path.Combine(Bin, Prefix + "gcc");
		public string Gpp => Path.Combine(Bin, Prefix + "g++");

		internal ToolchainFile(string project, string name) : base(Path.Combine(project, FileNamePrefix + name))
		{
			Flags = new List<string>();
		}
	}
}