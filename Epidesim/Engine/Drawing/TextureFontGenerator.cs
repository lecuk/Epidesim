using Epidesim.Engine.Drawing.Types;
using OpenTK.Graphics.OpenGL;
using SharpFont;
using System;
using System.Collections.Generic;

namespace Epidesim.Engine.Drawing
{
	static class TextureFontGenerator
	{
		public static TextureFont Generate(string fontPath, IList<char> characters)
		{
			var lib = new Library();
			var face = new Face(lib, fontPath);
			face.SetPixelSizes(0, 256);

			// set 1 byte pixel alignment 
			GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

			var alphabet = new Dictionary<char, TextureFont.Glyph>();

			foreach (char character in characters)
			{
				try
				{
					face.LoadChar(character, LoadFlags.Render, LoadTarget.Normal);
					GlyphSlot glyph = face.Glyph;
					FTBitmap bitmap = glyph.Bitmap;

					var texture = new Texture2D(bitmap.Width, bitmap.Rows, bitmap.Buffer,
						PixelInternalFormat.R8,
						PixelFormat.Red,
						TextureMinFilter.Linear,
						TextureMagFilter.Linear,
						TextureWrapMode.ClampToEdge,
						generateMipmap: true);
					
					var glyphData = new TextureFont.Glyph();
					glyphData.Texture = texture;
					glyphData.Character = character;
					glyphData.Left = glyph.BitmapLeft;
					glyphData.Top = glyph.BitmapTop;
					glyphData.Width = bitmap.Width;
					glyphData.Height = bitmap.Rows;
					glyphData.Advance = glyph.Advance.X / 64;

					alphabet.Add(character, glyphData);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex);
				}
			}
			
			return new TextureFont(alphabet);
		}
	}
}
