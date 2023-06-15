using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;
using UBuild.Packagers;
using UBuild.Models;

namespace UBuild.Actions
{
	public enum PackageType
	{
		Zip
	}
	
	public class PackageAction : IAction
	{
		private readonly Sources _sources;
		private readonly Project _project;
		private readonly PackageType _type;

		internal PackageAction(Sources sources, Project project, PackageType type)
		{
			_sources = sources;
			_project = project;
			_type = type;
		}

		public ActionResult Run()
		{
			Console.WriteLine("Packaging {0} for {1}", _project.Config.Name, _type);

			string packageDir = Path.Combine(_sources.OutPath, "packages");
			if (!Directory.Exists(packageDir))
				Directory.CreateDirectory(packageDir);

			string destFile = Path.Combine(packageDir, _project.Config.Name);

			using (IPackager packager = GetPackager(destFile))
			{
				//Delete package if it exists
				if (File.Exists(packager.DestFile))
					File.Delete(packager.DestFile);

				//Initialize packager
				packager.Init();

				//Add all the target binaries
				IEnumerable<string> binaries = _project.Config.Targets.Select(i =>
				{
					Target target = _sources.GetTarget(i);
					return target.BinFile;
				});
				packager.AddEntries("bin", binaries);

				//Add all the configs
				IEnumerable<string> configs = _project.Config.Configs.Select(i => _sources.ResolvePath(i));
				packager.AddEntries("dat", configs);

				Console.WriteLine("\tSuccessfully built {0}", packager.DestFile);
			}

			return ActionResult.Success;
		}

		private IPackager GetPackager(string destFile)
		{
			switch (_type)
			{
				case PackageType.Zip:
					return new ZipPackager(destFile);

				default:
					throw new Exception("Unknown packager");
			}
		}
	}
}