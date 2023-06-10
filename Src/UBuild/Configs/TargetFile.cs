using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBuild.Configs;

namespace UBuild.Configs
{
	internal class TargetFile : ConfigFile
	{
		internal static readonly string FileName = "TARGET";

		public string Dir { get; }
		public string Name { get; set; }
		public string Toolchain { get; set; }
		public List<string> CSources { get; set; }
		public List<string> CppSources { get; set; }
		public List<string> Flags { get; set; }

		internal TargetFile(string dir) : base(Path.Combine(dir, FileName))
		{
			Dir = dir;
			CSources = new List<string>();
			CppSources = new List<string>();
			Flags = new List<string>();
		}
	}
}
