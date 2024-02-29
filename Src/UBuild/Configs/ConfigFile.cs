using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace UBuild.Configs
{
	internal class ConfigFile
	{
		protected string File { get; }

		internal ConfigFile(string file)
		{
			File = file;
			Load();
		}

		private void Load()
		{
			//Use reflection to get properties
			PropertyInfo[] properties = this.GetType().GetProperties();

			//Parse
			Dictionary<string, object> parsed = Parse();

			foreach (KeyValuePair<string, object> kvp in parsed)
			{
				//Ensure property exists
				PropertyInfo prop = properties.SingleOrDefault(i => i.Name == kvp.Key)
					?? throw new Exception("Property not found: " + kvp.Key + " while parsing " + File);
				prop.SetValue(this, kvp.Value);
			}
		}

		private Dictionary<string, object> Parse()
		{
			Dictionary<string, object> parsed = new Dictionary<string, object>();

			object current = null;
			string[] lines = System.IO.File.ReadAllLines(this.File);
			foreach (string line in lines)
			{
				if (String.IsNullOrEmpty(line))
					continue;

				if (line[0] == '#')
					continue;

				if (line.Contains('['))
				{
					//Start of subsection
					string subsection = line.Replace("[", string.Empty).Replace("]", string.Empty);
					Type type = this.GetType().GetProperty(subsection)?.PropertyType;
					current = Activator.CreateInstance(type);
					parsed.Add(subsection, current);
					continue;
				}

				if (current == null)
				{
					//Simple Key/Value of property
					string[] parts = line.Split('=');
					Debug.Assert(parts.Length == 2);
					parsed.Add(parts[0], parts[1]);
				}
				else if (current.GetType() == typeof(List<string>))
				{
					((List<string>)current).Add(line);
				}
				else if (current.GetType() == typeof(Dictionary<string, string>))
				{
					string[] parts = line.Split('=');
					Debug.Assert(parts.Length == 2);
					((Dictionary<string, string>)current).Add(parts[0], parts[1]);
				}
			}

			return parsed;
		}
	}
}