using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Engine.Drawing.Types
{
	class DefaultVertexBufferObject : VertexBufferObject
	{
		public DefaultVertexBufferObject (int bufferSize, int componentCount)
			: base(bufferSize, 
				  OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, 
				  sizeof(float), componentCount, 
				  false,
				  OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw, 
				  OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, 
				  OpenTK.Graphics.OpenGL.GetPName.ArrayBufferBinding)
		{
		}
	}
}
