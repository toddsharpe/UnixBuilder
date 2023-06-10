using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Threading.Tasks;

namespace UBuild.Configs
{
	internal class ProjectFile : ConfigFile
	{
		internal static readonly string FileNamePrefix = "PROJECT_";

		public string Name { get; }
		public List<string> Targets { get; set; }
		public List<string> Configs { get; set;}

		internal ProjectFile(string sourceDir, string name) : base(Path.Combine(sourceDir, FileNamePrefix + name))
		{
			Name = name;
			Targets ??= new List<string>();
			Configs ??= new List<string>();
		}
	}
}