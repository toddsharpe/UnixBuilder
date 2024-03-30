using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBuild.Configs;
using UBuild.Tasks;
using UBuild.Models;
using System.Diagnostics;
using System.Reflection;

namespace UBuild.Actions
{
	internal class BuildAction : IAction
	{
		public bool Verbose { private get; set; }
		private readonly Sources _sources;
		private readonly Target _target;
		private readonly Toolchain _toolchain;
		public string OutputFile => _target.BinFile + _toolchain.Ext;

		internal BuildAction(Sources sources, Target target, Toolchain toolchain)
		{
			_sources = sources;
			_target = target;
			_toolchain = toolchain;
		}

		public ActionResult Run()
		{
			//Get toolchain
			if (!_target.CompatibleWith(_toolchain))
			{
				Console.WriteLine("Skipping {0}, Toolchain differs {1}, {2}", _target.Config.Name, _target.Config.Toolchain, _toolchain.Config.Name);
				return ActionResult.Skipped;
			}

			Console.WriteLine("Building {0} for {1}", _target.Config.Name, _toolchain.Config.Name);

			List<ITask> tasks = new List<ITask>();
			List<string> objects = new List<string>();

			//Build list of includes
			List<string> includes = new List<string>();
			foreach (string includeDir in _target.Config.IncludeDirs)
			{
				//Get absolute path
				string path = _target.ResolveSourcePath(includeDir);
				includes.Add(path);
			}

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

				//Add include dirs
				foreach (string include in includes)
					flags.Add($"-I {include}");

				//Add defines
				foreach (string define in _target.Config.Defines)
					flags.Add($"-D {define}");

				//Add flags
				flags.AddRange(_target.Config.Flags);
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

				//Add include dirs
				foreach (string include in includes)
					flags.Add($"-I {include}");

				//Add defines
				foreach (string define in _target.Config.Defines)
					flags.Add($"-D {define}");

				//Add flags
				flags.AddRange(_target.Config.Flags);
				flags.AddRange(_target.Config.CppFlags);
				flags.AddRange(_toolchain.Config.Flags);

				tasks.Add(new RunTask(_toolchain.Gpp, flags));
			}

			//Asm sources
			foreach (string source in _target.Config.AsmSources)
			{
				//Get absolute paths
				string input = _target.ResolveSourcePath(source);
				string output = _sources.GetObjectPath(input);
				objects.Add(output);

				List<string> flags = new List<string>
				{
					_toolchain.AsFlags,
					"-c",
					input,
					$"-o {output}",
				};

				//Add defines
				foreach (string define in _target.Config.Defines)
					flags.Add($"-D {define}");

				//Add flags
				flags.AddRange(_target.Config.Flags);
				flags.AddRange(_toolchain.Config.Flags);

				tasks.Add(new RunTask(_toolchain.As, flags));
			}

			//Link
			{
				List<string> flags = new List<string>
				{
					string.Join(" ", objects),
					$"-o {OutputFile}",
				};

				//Add flags
				foreach (string flag in _target.Config.LinkFlags)
					flags.Add(_target.Eval(flag));

				flags.AddRange(_toolchain.Config.LinkFlags);
				tasks.Add(new RunTask(_toolchain.Gpp, flags));
			}

			//Post build env
			Dictionary<string, string> env = new Dictionary<string, string>
			{
				{ "BinFile", _target.BinFile },
				{ "OutDir", _target.OutDir },
			};
			foreach (PropertyInfo property in typeof(Toolchain).GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
			{
				if (property.PropertyType != typeof(string))
					continue;
				
				string value = property.GetValue(_toolchain) as string;
				env.Add(property.Name, value);
			}

			//Post build
			foreach (string postbuild in _target.Config.PostBuild)
			{
				string[] parts = postbuild.Split(':');
				Debug.Assert(parts.Length == 2);
				Debug.Assert(parts[0][0] == '$');
				string bin = typeof(Toolchain).GetProperty(parts[0].Substring(1)).GetValue(_toolchain) as string;
				List<string> args = parts[1].Split(' ').Select(_target.Eval).Select(_sources.GetAbsoluteSrcPath).ToList();
				tasks.Add(new RunTask(bin, args, env));
			}

			//Create output directories
			foreach (string obj in objects)
			{
				string dir = Path.GetDirectoryName(obj);
				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);
			}

			//View created file
			tasks.Add(new RunTask("stat", new List<string> { OutputFile }));

			//Display and execute
			foreach (ITask task in tasks)
			{
				if (Verbose)
					task.Display();
				if (!task.Run())
					return ActionResult.Failed;
			}

			Console.WriteLine("\tSuccessfully built {0}", OutputFile);

			return ActionResult.Success;
		}
	}
}
