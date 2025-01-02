using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UBuild.Models
{
	public class Toolchain
	{
		internal const string ALL = "ALL";

		public string Name { get; set; }
		public string Bin { get; set; }
		public string Prefix { get; set; }
		public List<string> Flags { get; set; } = new List<string>();
		public List<string> LinkFlags { get; set; } = new List<string>();
		
		internal string Gcc => Path.Combine(Bin, Prefix + "gcc");
 		internal string Gpp => Path.Combine(Bin, Prefix + "g++");
		internal string As => Path.Combine(Bin, Prefix + "gcc"); /*  -x assembler-with-cpp */
		internal string AsFlags => "-x assembler-with-cpp";
		internal string ObjCopy => Path.Combine(Bin, Prefix + "objcopy");
		internal string ObjDump => Path.Combine(Bin, Prefix + "objdump");
		internal string Size => Path.Combine(Bin, Prefix + "size");
		internal string Stat => "stat";
		internal string Hexdump => "hexdump";
		internal string Bash => "bash";
		internal string Ext => ".elf";
	}
}