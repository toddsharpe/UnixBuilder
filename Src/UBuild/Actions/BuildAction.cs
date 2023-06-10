using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBuild.Configs;
using UBuild.Tasks;

namespace UBuild.Actions
{
	internal class BuildAction : IAction
	{

		private string _baseDir;
		private string _toolchain;

		private SourcesFile _project;
		private TargetFile _target;

		internal BuildAction(string dir, string target, string toolchain = "")
		{
			_baseDir = dir;

			_project = new SourcesFile(dir);
			_target = new TargetFile(Path.Combine(dir, target));

			_toolchain = toolchain;
		}

		public void Init()
		{
			_project.Load();
			_target.Load();
		}

		public bool Run()
		{
			//Get toolchain
			string key = String.IsNullOrEmpty(_toolchain) ? "Host" : _toolchain;
			if (!String.IsNullOrEmpty(_target.Toolchain))
			{
				if (_target.Toolchain != key)
				{
					Console.WriteLine("Skipping {0}, Target differs {1}, {2}", _target.Name, _target.Toolchain, key);
					return false;
				}
			}

			Console.WriteLine("Building {0} for {1}", _target.Name, key);

			//Get toolchain file
			ToolchainFile toolchain = new ToolchainFile(_baseDir, key);
			toolchain.Load();

			List<ITask> tasks = new List<ITask>();
			List<string> objects = new List<string>();

			//Build C sources
			foreach (string source in _target.CSources)
			{
				//Get absolute paths
				string input = ResolvePath(source);
				string output = GetOutputFile(input);
				objects.Add(output);

				List<string> flags = new List<string>
				{
					"-c",
					input,
					$"-o {output}",
					$"-I {_baseDir}"
				};
				flags.AddRange(toolchain.Flags);
				tasks.Add(new RunTask(toolchain.Gcc, flags));
			}

			//Build Cpp sources
			foreach (string source in _target.CppSources)
			{
				//Get absolute paths
				string input = ResolvePath(source);
				string output = GetOutputFile(input);
				objects.Add(output);

				List<string> flags = new List<string>
				{
					"-c",
					input,
					$"-o {output}",
					$"-I {_baseDir}"
				};
				flags.AddRange(toolchain.Flags);
				tasks.Add(new RunTask(toolchain.Gpp, flags));
			}

			//Link
			{
				List<string> flags = new List<string>
				{
					string.Join(" ", objects),
					$"-o {GetTarget()}",
				};
				flags.AddRange(toolchain.Flags);
				tasks.Add(new RunTask(toolchain.Gpp, flags));
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
				//task.Display();
				if (!task.Run())
					return false;
			}

			return true;
		}

		private string ResolvePath(string path)
		{
			if (path.StartsWith("^"))
			{
				//Path relative to project
				return Path.Combine(_baseDir, path.Substring(1));
			}
			else if (Path.IsPathRooted(path))
			{
				return path;
			}
			else
			{
				//Short paths are from target dir
				return Path.Combine(_target.Dir, path);
			}
		}

		private string GetOutputFile(string path)
		{
			string rel = Path.GetRelativePath(_baseDir, path);
			string mapped = Path.Combine(_baseDir, _project.OutDir, rel);
			string rep = mapped.Replace(".cc", ".o").Replace(".cpp", ".o").Replace(".c", ".o");
			return Path.GetFullPath(rep);
		}

		private string GetTarget()
		{
			string path = Path.Combine(_target.Dir, _target.Name);
			string rel = Path.GetRelativePath(_baseDir, path);
			string mapped = Path.Combine(_baseDir, _project.OutDir, rel);
			return Path.GetFullPath(mapped);
		}
	}
}
