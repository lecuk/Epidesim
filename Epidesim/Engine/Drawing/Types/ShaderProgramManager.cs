using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Engine.Drawing.Types
{
	static class ShaderProgramManager
	{
		private static Dictionary<string, ShaderProgram> programs = new Dictionary<string, ShaderProgram>();

		public static void AddProgram(string name, ShaderProgram program)
		{
			programs.Add(name, program);
		}

		public static ShaderProgram GetProgram(string name)
		{
			return programs[name];
		}
	}
}
