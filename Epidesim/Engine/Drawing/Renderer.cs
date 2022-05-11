using Epidesim.Engine.Drawing.Types;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Epidesim.Engine.Drawing
{
	abstract class Renderer : IDisposable
	{
		public virtual void Dispose()
		{
		}
	}
}
