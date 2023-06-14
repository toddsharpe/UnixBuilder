using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;

namespace UBuild.Models
{
	internal class Target
	{
		private static readonly string SourceRootPathPrefix = ">";
		public string BinFile => Path.Combine(_outDir, Config.BinName);
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
			if (source.StartsWith(SourceRootPathPrefix))
			{
				//Path is relative to sources dir (current directory)
				return source.Substring(SourceRootPathPrefix.Length);
			}
			else
			{
				//Otherwise, assume path is relative to target
				return Path.Combine(Config.Name, source);
			}
		}
	}
}