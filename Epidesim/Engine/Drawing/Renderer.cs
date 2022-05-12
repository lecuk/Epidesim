using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Epidesim.Engine.Drawing
{
	abstract class Renderer : IDisposable
	{
		public Matrix4 TransformMatrix { get; set; }

		public Renderer()
		{
			TransformMatrix = Matrix4.Identity;
		}

		public virtual void Dispose()
		{
		}
	}
}
