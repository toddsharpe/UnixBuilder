using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;
using UBuild.Models;

namespace UBuild.Actions
{
	public class BuildProjectAction : IAction
	{
		private readonly IEnumerable<BuildAction> _builds;

		internal BuildProjectAction(Sources sources, Project project, Toolchain toolchain)
		{
			_builds = project.Config.Targets
						.Select(i => sources.GetTarget(i))
						.Select(i => new BuildAction(sources, i, toolchain));
		}
		
		public bool Run()
		{
			foreach (IAction action in _builds)
			{
				action.Run();
			}

			return true;
		}
	}
}