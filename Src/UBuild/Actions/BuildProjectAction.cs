using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;

namespace UBuild.Actions
{
	public class BuildProjectAction : IAction
	{
		private string _dir;
		private string _project;
		private string _toolchain;
	
		private List<BuildAction> _builds;

		internal BuildProjectAction(string dir, string project, string toolchain = "")
		{
			_dir = dir;
			_project = project;
			_toolchain = toolchain;

			_builds = new List<BuildAction>();
		}
		
		public void Init()
		{
			ProjectFile project = new ProjectFile(_dir, _project);
			project.Load();

			foreach (string target in project.Targets)
			{
				_builds.Add(new BuildAction(_dir, target, _toolchain));
			}

			foreach (IAction action in _builds)
			{
				action.Init();
			}
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