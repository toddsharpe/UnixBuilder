using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBuild.Tasks
{
	internal interface ITask
	{
		void Display();
		bool Run();
	}
}
