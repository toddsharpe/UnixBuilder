using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBuild.Configs;

namespace UBuild.Models
{
	internal class Project
	{
		public ProjectFile Config { get; }
		internal Project(ProjectFile config)
		{
			Config = config;
		}
	}
}