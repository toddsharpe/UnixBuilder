using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;
using UBuild.Models;

namespace UBuild.Actions
{
	public class BuildRunAction : IAction
	{
		private readonly BuildAction _action;
		private readonly Target _target;

		internal BuildRunAction(Sources sources, Target target, Toolchain toolchain)
		{
			_action = new BuildAction(sources, target, toolchain);
			_target = target;
		}
		
		public ActionResult Run()
		{
			ActionResult result = _action.Run();
			if (result != ActionResult.Success)
				return result;

			Console.WriteLine("---Run---");
			Process process = Process.Start(_target.BinFile);
			process.WaitForExit();

			return ActionResult.Success;
		}
	}
}