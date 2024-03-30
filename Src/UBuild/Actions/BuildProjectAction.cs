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
		public bool Verbose { private get; set; }
		private readonly List<BuildAction> _builds;

		internal BuildProjectAction(Sources sources, Project project)
		{
			_builds = new List<BuildAction>();

			foreach (var i in project.Targets)
			{
				if (i.Item2 != Project.ALL_TOOLCHAINS)
				{
					Target target = sources.GetTarget(i.Item1);
					Toolchain toolchain = sources.GetToolchain(i.Item2);
					_builds.Add(new BuildAction(sources, target, toolchain));
				}
				else
				{
					foreach (string toolchainName in sources.Config.Toolchains)
					{
						Target target = sources.GetTarget(i.Item1);
						Toolchain toolchain = sources.GetToolchain(toolchainName);
						_builds.Add(new BuildAction(sources, target, toolchain));
					}
				}
			}
		}
		
		public ActionResult Run()
		{
			foreach (IAction action in _builds)
			{
				action.Verbose = Verbose;
				if (action.Run() == ActionResult.Failed)
					return ActionResult.Failed;
			}

			return ActionResult.Success;
		}
	}
}