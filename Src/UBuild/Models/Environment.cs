using System.Text.Json;
using System.Text.Json.Serialization;

namespace UBuild.Models
{
	public class Environment
	{
		internal const string FileName = "Env_build.json";

		public string Output { get; set; }
		public string Sources { get; set; }
		public string Exes { get; set; }
		public string Configs { get; set; }
		public List<Project> Projects { get; set;} = new List<Project>();
		public List<Toolchain> Toolchains { get; set; } = new List<Toolchain>();

		internal string OutputObj => Output + "_obj";
		internal string OutputExe => Output + "_exe";

		internal string SourcesDirectory => Path.Combine(Directory, Sources);
		internal string OutputObjectDirectory => Path.Combine(Directory, OutputObj);
		internal string OutputExeDirectory => Path.Combine(Directory, OutputExe);
		internal string ExeDirectory => Path.Combine(Directory, Exes);
		internal string ConfigsDirectory => Path.Combine(Directory, Configs);
		internal string Directory { get; set; }

		internal static Environment Load(string filename)
		{
			string json = File.ReadAllText(filename);
			Environment env = JsonSerializer.Deserialize<Environment>(json);
			env.Directory = Path.GetDirectoryName(filename);

			return env;
		}

		internal Executable GetExe(string path)
		{
			string file = Path.Combine(ExeDirectory, (path ?? "") + "_exe.json");
			if (!File.Exists(file))
				return null;

			string json = File.ReadAllText(file);
			Executable exe = JsonSerializer.Deserialize<Executable>(json);
			exe.OutDir = Path.Combine(OutputExeDirectory, path);

			return exe;
		}

		internal string GetObjectPath(string source)
		{
			string objectFile = source.Replace(".cc", ".o").Replace(".cpp", ".o").Replace(".c", ".o").Replace(".s", ".o");
			return Path.Combine(OutputObjectDirectory, objectFile);
		}

		internal string GetSourcePath(string source)
		{
			return Path.Combine(SourcesDirectory, source);
		}
	}
}
