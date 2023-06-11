using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;

namespace UBuild.Models
{
	internal class Sources
	{
		public string OutPath => Path.GetFullPath(Path.Combine(Config.SourcesDir, Config.OutDir));
		public string ConfigsPath => Path.GetFullPath(Path.Combine(Config.SourcesDir, Config.ConfigDir));
		
		internal static Sources Open(string sourcesDir)
		{
			SourcesFile config = new SourcesFile(sourcesDir);
			return new Sources(config);
		}

		public SourcesFile Config { get; }
		internal Sources(SourcesFile config)
		{
			Config = config;
		}

		public Project GetProject(string project)
		{
			ProjectFile projectFile = new ProjectFile(Config.SourcesDir, project);
			return new Project(projectFile);
		}

		public Target GetTarget(string target)
		{
			TargetFile targetFile = new TargetFile(Config.SourcesDir, target);
			string outDir = Path.Combine(OutPath, target);
			return new Target(targetFile, outDir);
		}

		public Toolchain GetToolchain(string toolchain)
		{
			ToolchainFile toolchainFile = new ToolchainFile(Config.SourcesDir, toolchain);
			return new Toolchain(toolchainFile);
		}

		//NOTE(tsharpe): Combine with Target::ResolveSourcePath
		public string ResolvePath(string entry)
		{
			if (Path.IsPathRooted(entry))
			{
				return entry;
			}
			else if (entry.StartsWith('^'))
			{
				//Path is relative to sources dir (current directory)
				return Path.Combine(ConfigsPath, entry.Substring(1));
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}