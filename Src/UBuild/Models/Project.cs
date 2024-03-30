using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;

namespace UBuild.Models
{
	internal class Project
	{
		internal const string ALL_TOOLCHAINS = "ALL";
		public List<Tuple<string, string>> Targets { get; set; }
		public ProjectFile Config { get; }
		internal Project(ProjectFile config)
		{
			Config = config;
			Targets = ParseTargets(config);
		}

		private static List<Tuple<string, string>> ParseTargets(ProjectFile config)
		{
			return config.Targets.Select( i=>
			{
				string[] parts = i.Split(':');
				string target = parts.Length > 1 ? parts[1] : ALL_TOOLCHAINS;

				return new Tuple<string, string>(parts[0], target);
			}).ToList();
		}
	}
}