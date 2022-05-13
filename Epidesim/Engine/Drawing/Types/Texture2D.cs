using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Epidesim.Engine.Drawing.Types
{
	class Texture2D
	{
		private readonly int handle;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public Texture2D(int width, int height, IntPtr dataPtr)
		{
			this.handle = GL.GenTexture();

			Width = width;
			Height = height;

			GL.BindTexture(TextureTarget.Texture2D, this.handle);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, dataPtr);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}

		public static Texture2D Load(string path)
		{
			using (var image = Image.FromFile(path))
			{
				var bitmap = image as Bitmap;
				if (bitmap == null)
				{
					throw new ArgumentException("Not image file");
				}

				var bitmapData = bitmap.LockBits(
					new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				
				var texture = new Texture2D(bitmap.Width, bitmap.Height, bitmapData.Scan0);

				bitmap.UnlockBits(bitmapData);

				return texture;
			}
		}

		public void Use(TextureUnit unit)
		{
			GL.BindTexture(TextureTarget.Texture2D, this.handle);
			GL.ActiveTexture(unit);
		}
	}
}
