using System;
using OpenTK.Graphics.OpenGL;

namespace Epidesim.Engine.Drawing.Types
{
	class VertexBufferObject : IDisposable
	{
		private readonly int id;
		private readonly BufferUsageHint hint;
		private readonly BufferTarget target;
		private readonly GetPName targetBinding;
		private byte[] data;

		public readonly VertexAttribPointerType Type;
		public readonly int Size;
		public readonly int ComponentCount;
		public readonly bool IsNormalized;

		public VertexBufferObject(
			int bufferSize,
			VertexAttribPointerType type,
			int size,
			int componentCount,
			bool isNormalized,
			BufferUsageHint hint,
			BufferTarget target,
			GetPName targetBinding)
		{
			this.Type = type;
			this.Size = size;
			this.ComponentCount = componentCount;
			this.IsNormalized = isNormalized;

			this.id = GL.GenBuffer();
			this.target = target;
			this.hint = hint;
			this.targetBinding = targetBinding;
			this.data = new byte[bufferSize];
		}

		public bool TryBind()
		{
			if (GL.GetInteger(targetBinding) != 0) return false;

			Bind();
			return true;
		}

		public void Bind()
		{
			if (GL.GetInteger(targetBinding) != 0)
				throw new InvalidOperationException("Another VBO is already bound");

			GL.BindBuffer(target, this.id);
		}

		public void SetData<T>(T[] data)
		{
			SetData(data, data.Length);
		}

		public void SetData<T>(T[] data, int count)
		{
			int usedByteCount = count * Size;
			System.Buffer.BlockCopy(data, 0, this.data, 0, usedByteCount);
			GL.BufferData(target, usedByteCount, this.data, hint);
		}

		public bool TryUnbind()
		{
			if (GL.GetInteger(targetBinding) != this.id) return false;

			Unbind();
			return true;
		}

		public void Unbind()
		{
			if (GL.GetInteger(targetBinding) != this.id)
				throw new InvalidOperationException("Attempt to unbind wrong VBO");

			GL.BindBuffer(target, 0);
		}

		public void Dispose()
		{
			GL.DeleteBuffer(this.id);
		}
	}
}
