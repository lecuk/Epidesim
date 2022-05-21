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
	public class Texture2D
	{
		private readonly int handle;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public Texture2D(int width, int height, IntPtr dataPtr,
			PixelInternalFormat pixelInternalFormat,
			PixelFormat pixelFormat,
			TextureMinFilter textureMinFilter, 
			TextureMagFilter textureMagFilter,
			TextureWrapMode wrapMode,
			bool generateMipmap)
		{
			this.handle = GL.GenTexture();

			Width = width;
			Height = height;

			GL.BindTexture(TextureTarget.Texture2D, this.handle);
			GL.TexImage2D(TextureTarget.Texture2D, 0, pixelInternalFormat, width, height, 0, pixelFormat, PixelType.UnsignedByte, dataPtr);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)textureMinFilter);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)textureMagFilter);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);

			if (generateMipmap)
			{
				GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			}
		}

		public static Texture2D FromBitmap(
			Bitmap bitmap,
			System.Drawing.Imaging.PixelFormat imagingPixelFormat,
			PixelInternalFormat pixelInternalFormat,
			PixelFormat pixelFormat,
			TextureMinFilter textureMinFilter,
			TextureMagFilter textureMagFilter,
			TextureWrapMode wrapMode,
			bool generateMipmap)
		{
			var bitmapData = bitmap.LockBits(
				new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				imagingPixelFormat);

			var texture = new Texture2D(bitmap.Width, bitmap.Height, bitmapData.Scan0,
				pixelInternalFormat, pixelFormat, textureMinFilter, textureMagFilter, wrapMode, generateMipmap);

			bitmap.UnlockBits(bitmapData);

			return texture;
		}

		public void Use(TextureUnit unit)
		{
			GL.BindTexture(TextureTarget.Texture2D, this.handle);
			GL.ActiveTexture(unit);
		}
	}
}
