using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using UBuild.Actions;

public class Program
{
	public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

	public enum ActionType
	{
		None,
		Build,
		List
	}

	[Argument(0, Description = "Action")]
	public ActionType Action { get; }

	[Option("-t|--target <path>", Description = "Target to build")]
	public string Target { get;}

	[Option("-p|--project <path>", Description = "Project to build")]
	public string Project { get;}

	[Option("-c|--compiler <path>", Description = "Toolchain to use")]
	public string Toolchain { get;} = "";

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
		}

		return ValidationResult.Success;
	}

	private void OnExecute()
	{
		Console.WriteLine("Action: {0}", Action);

		IAction action;
		switch (Action)
		{
			case ActionType.Build:
				string sources = Directory.GetCurrentDirectory();
				if (!String.IsNullOrEmpty(Project))
					action = new BuildProjectAction(sources, Project, Toolchain);
				else
					action = new BuildAction(sources, Target, Toolchain);
				break;

			default:
				throw new NotImplementedException();
		}

		action.Init();
		action.Run();
	}
}