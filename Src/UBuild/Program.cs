using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using UBuild.Actions;
using UBuild.Models;

public class Program
{
	public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

	public enum ActionType
	{
		None,
		Build,
		Package,
		Run
	}

	[Argument(0, Description = "Action")]
	public ActionType Action { get; }

	[Option("-t|--target <path>", Description = "Target to build")]
	public string Target { get;}

	[Option("-p|--project <path>", Description = "Project to build")]
	public string Project { get;}

	[Option("-c|--compiler <path>", Description = "Toolchain to use")]
	public string Toolchain { get;} = "Host";

	[Option("-f|--file <path>", Description = "Package file")]
	public PackageType PackageType { get;} = PackageType.Zip;

	[Option("-v|--verbose", Description = "Display commands.")]
	public bool Verbose { get;} = false;

	private ValidationResult OnValidate()
	{
		if (Action == ActionType.None)
			return new ValidationResult("Muist specify an action");

		switch (Action)
		{
		case ActionType.Build:
			if (String.IsNullOrWhiteSpace(Target) && String.IsNullOrWhiteSpace(Project))
				return new ValidationResult("Must specify a target or project");
			break;

		case ActionType.Package:
			if (String.IsNullOrWhiteSpace(Project))
				return new ValidationResult("Must specify a project");
			break;

		case ActionType.Run:
			if (String.IsNullOrWhiteSpace(Target))
				return new ValidationResult("Must specify a target");
			break;
		}

		return ValidationResult.Success;
	}

	private void OnExecute()
	{
		string sourcesDir = Directory.GetCurrentDirectory();
		Console.WriteLine("Action: {0}", Action);

		IAction action;
		switch (Action)
		{
			case ActionType.Build:
			{
				Sources sources = Sources.Open(sourcesDir);
				Toolchain toolchain = sources.GetToolchain(Toolchain);

				if (!String.IsNullOrEmpty(Project))
				{
					Project project = sources.GetProject(Project);
					action = new BuildProjectAction(sources, project, toolchain);
				}
				else
				{
					Target target = sources.GetTarget(Target);
					action = new BuildAction(sources, target, toolchain);
				}
				break;
			}

			case ActionType.Package:
			{
				Sources sources = Sources.Open(sourcesDir);
				Project project = sources.GetProject(Project);
				action = new PackageAction(sources, project, PackageType);
				break;
			}

			case ActionType.Run:
			{
				Sources sources = Sources.Open(sourcesDir);
				Target target = sources.GetTarget(Target);
				Toolchain toolchain = sources.GetToolchain(Toolchain);

				action = new BuildRunAction(sources, target, toolchain);
				break;
			}

			default:
				throw new NotImplementedException();
		}

		action.Verbose = Verbose;
		action.Run();
	}
}