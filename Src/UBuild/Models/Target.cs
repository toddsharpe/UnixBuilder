using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;

namespace UBuild.Models
{
	internal class Target
	{
		public string BinFile => Path.Combine(_outDir, Config.BinName + ".elf");
		public TargetFile Config { get; }

		private readonly string _outDir;
		internal Target(TargetFile config, string outDir)
		{
			Config = config;
			_outDir = outDir;
		}

		public bool CompatibleWith(Toolchain toolchain)
		{
			//Empty toolchain means fully compatible
			if (String.IsNullOrEmpty(Config.Toolchain))
				return true;

			return Config.Toolchain == toolchain.Config.Name;
		}

		public string ResolveSourcePath(string source)
		{
			if (source.StartsWith('^'))
			{
				//Path is relative to sources dir (current directory)
				return source.Substring(1);
			}
			else
			{
				//Otherwise, assume path is relative to target
				return Path.Combine(Config.Name, source);
			}
		}

		public string GetObjectPath(string source)
		{
			string objectFile = source.Replace(".cc", ".o").Replace(".cpp", ".o").Replace(".c", ".o");
			return Path.Combine(_outDir, objectFile);
		}
	}
}