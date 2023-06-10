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

		public RunTask(string bin, List<string> args)
		{
			_bin = bin;
			_args = args;
		}

		public bool Run()
		{
			Process process = Process.Start(_bin, string.Join(" ", _args));
			process.WaitForExit();
			return process.ExitCode == 0;
		}

		public void Display()
		{
			Console.WriteLine("{0} {1}\n", _bin, _args != null ? string.Join(" ", _args) : "<none>");
		}
	}
}
