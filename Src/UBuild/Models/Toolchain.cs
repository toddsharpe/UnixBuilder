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
		public string As => Path.Combine(Config.Bin, Config.Prefix + "gcc"); /*  -x assembler-with-cpp */
		public string AsFlags => "-x assembler-with-cpp";
		public string ObjCopy => Path.Combine(Config.Bin, Config.Prefix + "objcopy");
		public string ObjDump => Path.Combine(Config.Bin, Config.Prefix + "objdump");
		public string Size => Path.Combine(Config.Bin, Config.Prefix + "size");
		public string Stat => "stat";
		public string Hexdump => "hexdump";
		public string Bash => "bash";
		public string Ext => ".elf";
		public ToolchainFile Config { get; }
		internal Toolchain(ToolchainFile config)
		{
			Config = config;
		}
	}
}