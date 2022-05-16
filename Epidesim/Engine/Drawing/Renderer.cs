using OpenTK;
using System;

namespace Epidesim.Engine.Drawing
{
	abstract class Renderer : IDisposable
	{
		public virtual Matrix4 TransformMatrix { get; set; }

		public Renderer()
		{
		}

		public virtual void Reset()
		{
		}

		public virtual void Dispose()
		{
		}
	}
}
