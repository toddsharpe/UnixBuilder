using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBuild.Actions
{
	internal interface IAction
	{
		void Init();
		void Run();
	}
}
