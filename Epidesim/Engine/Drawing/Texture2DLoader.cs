using Epidesim.Engine.Drawing.Types;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Epidesim.Engine.Drawing
{
	public static class Texture2DLoader
	{
		public static Texture2D LoadFromFile(string path)
		{
			using (Image image = Image.FromFile(path))
			{
				var bitmap = image as Bitmap;

				return Texture2D.FromBitmap(bitmap,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb,
					PixelInternalFormat.Rgba,
					PixelFormat.Bgra,
					TextureMinFilter.Linear,
					TextureMagFilter.Nearest,
					TextureWrapMode.Clamp,
					generateMipmap: true);
			}
		}
	}
}
