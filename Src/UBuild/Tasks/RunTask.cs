using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBuild.Tasks
{
	internal class RunTask : ITask
	{
		private readonly string _bin;
		private readonly List<string> _args;
		private Dictionary<string, string> _env;

		public RunTask(string bin, List<string> args, Dictionary<string, string> env = null)
		{
			_bin = bin;
			_args = args;
			_env = env;
		}

		public bool Run()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(_bin);
			if (_args != null)
			{
				startInfo.Arguments = string.Join(" ", _args);
			}

			if (_env != null)
			{
				foreach (var item in _env)
				{
					startInfo.EnvironmentVariables.Add(item.Key, item.Value);
				}
			}

			Process process = Process.Start(startInfo);
			process.WaitForExit();

			return process.ExitCode == 0;
		}

		public void Display()
		{
			Console.WriteLine("{0} {1}", _bin, _args != null ? string.Join(" ", _args) : "<none>");
			if (_env != null)
			{
				foreach (var item in _env)
				{
					Console.WriteLine("\t{0}: {1}", item.Key, item.Value);
				}
			}
		}
	}
}
