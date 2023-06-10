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

	[Option("-p|--toolchain <path>", Description = "Toolchain to use")]
	public string Toolchain { get;} = "";

	private ValidationResult OnValidate()
	{
		if (Action == ActionType.None)
			return new ValidationResult("Muist specify an action");

		switch (Action)
		{
		case ActionType.Build:
			if (String.IsNullOrWhiteSpace(Target))
				return new ValidationResult("Must specify a target");
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
				string project = Directory.GetCurrentDirectory();
				action = new BuildAction(project, Target, Toolchain);
				break;

			default:
				throw new NotImplementedException();
		}

		action.Init();
		action.Run();
	}
}