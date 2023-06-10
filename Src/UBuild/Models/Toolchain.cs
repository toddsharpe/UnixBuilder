using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;

namespace UBuild.Models
{
	internal class Toolchain
	{
		public string Gcc => Path.Combine(Config.Bin, Config.Prefix + "gcc");
		public string Gpp => Path.Combine(Config.Bin, Config.Prefix + "g++");
		
		public ToolchainFile Config { get; }
		internal Toolchain(ToolchainFile config)
		{
			Config = config;
		}
	}
}