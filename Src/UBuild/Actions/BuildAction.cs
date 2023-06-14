using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBuild.Configs;
using UBuild.Tasks;
using UBuild.Models;

namespace UBuild.Actions
{
	internal class BuildAction : IAction
	{
		private readonly Sources _sources;
		private readonly Target _target;
		private readonly Toolchain _toolchain;

		internal BuildAction(Sources sources, Target target, Toolchain toolchain)
		{
			_sources = sources;
			_target = target;
			_toolchain = toolchain;
		}

		public bool Run()
		{
			//Get toolchain
			if (!_target.CompatibleWith(_toolchain))
			{
				Console.WriteLine("Skipping {0}, Toolchain differs {1}, {2}", _target.Config.Name, _target.Config.Toolchain, _toolchain.Config.Name);
				return false;
			}

			Console.WriteLine("Building {0} for {1}", _target.Config.Name, _toolchain.Config.Name);

			List<ITask> tasks = new List<ITask>();
			List<string> objects = new List<string>();

			//Build C sources
			foreach (string source in _target.Config.CSources)
			{
				//Get absolute paths
				string input = _target.ResolveSourcePath(source);
				string output = _sources.GetObjectPath(input);
				objects.Add(output);

				List<string> flags = new List<string>
				{
					"-c",
					input,
					$"-o {output}",
					$"-I {_sources.Config.SourcesDir}"
				};
				flags.AddRange(_toolchain.Config.Flags);
				tasks.Add(new RunTask(_toolchain.Gcc, flags));
			}

			//Build Cpp sources
			foreach (string source in _target.Config.CppSources)
			{
				//Get absolute paths
				string input = _target.ResolveSourcePath(source);
				string output = _sources.GetObjectPath(input);
				objects.Add(output);

				List<string> flags = new List<string>
				{
					"-c",
					input,
					$"-o {output}",
					$"-I {_sources.Config.SourcesDir}"
				};
				flags.AddRange(_toolchain.Config.Flags);
				tasks.Add(new RunTask(_toolchain.Gpp, flags));
			}

			//Link
			{
				List<string> flags = new List<string>
				{
					string.Join(" ", objects),
					$"-o {_target.BinFile}",
				};
				flags.AddRange(_toolchain.Config.Flags);
				tasks.Add(new RunTask(_toolchain.Gpp, flags));
			}

			//Create output directories
			foreach (string obj in objects)
			{
				string dir = Path.GetDirectoryName(obj);
				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);
			}

			//Display and execute
			foreach (ITask task in tasks)
			{
				if (!task.Run())
					return false;
			}

			Console.WriteLine("\tSuccessfully built {0}", _target.BinFile);

			return true;
		}
	}
}
