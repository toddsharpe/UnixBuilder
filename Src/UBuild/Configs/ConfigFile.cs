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
				PropertyInfo prop = properties.Single(i => i.Name == kvp.Key);
				prop.SetValue(this, kvp.Value);
			}
		}

		private Dictionary<string, object> Parse()
		{
			Dictionary<string, object> parsed = new Dictionary<string, object>();

			string subsection = null;
			string[] lines = System.IO.File.ReadAllLines(this.File);
			foreach (string line in lines)
			{
				if (String.IsNullOrEmpty(line))
					continue;

				if (line[0] == '#')
					continue;

				if (line.Contains('='))
				{
					//Simple KVP
					string[] parts = line.Split('=');
					Debug.Assert(parts.Length == 2);

					if (subsection != null)
					{
						if (!parsed.ContainsKey(subsection))
							parsed.Add(subsection, new Dictionary<string, string>());
						else
						{
							Type type = parsed[subsection].GetType();
							if (type == typeof(List<string>))
							{
								((List<string>)parsed[subsection]).Add(line);
							}
							else
							{
								((Dictionary<string, string>)parsed[subsection]).Add(parts[0], parts[1]);
							}
						}
					}
					else
					{
						parsed.Add(parts[0], parts[1]);
					}
				}
				else if (line.Contains('['))
				{
					//Start of subsection
					subsection = line.Replace("[", string.Empty).Replace("]", string.Empty);
				}
				else
				{
					//Subsection is just a list of values
					if (!parsed.ContainsKey(subsection))
						parsed.Add(subsection, new List<string>());
					((List<string>)parsed[subsection]).Add(line);
				}
			}

			return parsed;
		}
	}
}