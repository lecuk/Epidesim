using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Engine.Drawing
{
	class TextRenderer : Renderer
	{
		private class CharacterInfo
		{
			public char Character;
			public TextureFont.Glyph Glyph;
			public QuadTextureRenderer Renderer;
			public PrimitiveRenderer PRenderer;
		}

		private readonly Dictionary<char, CharacterInfo> infoDictionary;
		private IEnumerable<CharacterInfo> Infos => infoDictionary.Values;

		public TextRenderer()
		{
			infoDictionary = new Dictionary<char, CharacterInfo>();
		}

		public override Matrix4 TransformMatrix
		{
			get => base.TransformMatrix;
			set
			{
				foreach (var info in Infos)
				{
					info.Renderer.TransformMatrix = value;
					info.PRenderer.TransformMatrix = value;
				}
			}
		}

		public void LoadFont(TextureFont font)
		{
			infoDictionary.Clear();

			foreach (var glyph in font.Alphabet.Values)
			{
				var info = new CharacterInfo()
				{
					Character = glyph.Character,
					Glyph = glyph,
					Renderer = new QuadTextureRenderer(1000, ResourceManager.GetProgram("textureText")),
					PRenderer = new PrimitiveRenderer(2000, 2000, 2000) { WireframeMode = true }
				};

				infoDictionary.Add(glyph.Character, info);
			}
		}

		public void AddString(string text, ref Vector2 position, Color4 color)
		{
			float newLineX = position.X;
			for (int i = 0; i < text.Length; ++i)
			{
				AddChar(text[i], ref position, newLineX, color);
			}
		}

		private void AddChar(char character, ref Vector2 position, float newLineX, Color4 color)
		{
			if (character == ' ')
			{
				AddSpace(ref position);
				return;
			}

			if (character == '\n')
			{
				AddNewLine(ref position, newLineX);
				return;
			}

			var info = infoDictionary[character];
			var glyph = info.Glyph;
			var size = new Vector2(glyph.Width, -glyph.Height);
			
			Vector2 topLeft = position + new Vector2(glyph.Left, glyph.Top);
			Rectangle rect = Rectangle.FromTwoPoints(topLeft, topLeft + size);

			info.Renderer.AddQuad(rect, color);
			info.PRenderer.AddRectangle(rect, color);

			position += new Vector2(glyph.Advance, 0);
		}

		private void AddSpace(ref Vector2 position)
		{
			var info = infoDictionary['a'];
			var glyph = info.Glyph;
			position += new Vector2(glyph.Advance, 0);
		}

		private void AddNewLine(ref Vector2 position, float newLineX)
		{
			var a = infoDictionary['a'].Glyph;
			var j = infoDictionary['j'].Glyph;
			float diff = j.Height - a.Height;
			float height = a.Height + diff;

			position = new Vector2(newLineX, position.Y - height);
		}

		public void Reset()
		{
			foreach (var info in Infos)
			{
				info.Renderer.Reset();
				info.PRenderer.Reset();
			}
		}

		public void DrawAll()
		{
			foreach (var info in Infos)
			{
				info.Renderer.DrawTexture(info.Glyph.Texture);
			}
		}

		public void DrawWireframe()
		{
			foreach (var info in Infos)
			{
				info.PRenderer.DrawHollowElements();
			}
		}
	}
}
