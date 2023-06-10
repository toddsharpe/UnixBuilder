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

		public List<string> Targets { get; set; }

		public string Toolchain { get; set; }

		internal ProjectFile(string project, string name) : base(Path.Combine(project, FileNamePrefix + name))
		{

		}
	}
}