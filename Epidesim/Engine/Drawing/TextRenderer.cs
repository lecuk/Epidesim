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
	public class TextRenderer : Renderer
	{
		private class CharacterInfo
		{
			public char Character;
			public TextureFont.Glyph Glyph;
			public QuadTextureRenderer Renderer;
			public PrimitiveRenderer PRenderer;
		}

		private float pixelSize;
		private readonly Dictionary<char, CharacterInfo> infoDictionary;
		private IEnumerable<CharacterInfo> Infos => infoDictionary.Values;
		private int maxOccurencesPerCharacter;

		public TextRenderer(int maxOccurencesPerCharacter)
		{
			this.maxOccurencesPerCharacter = maxOccurencesPerCharacter;
			this.infoDictionary = new Dictionary<char, CharacterInfo>();
			this.pixelSize = 1;
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
				int quads = maxOccurencesPerCharacter;

				var info = new CharacterInfo()
				{
					Character = glyph.Character,
					Glyph = glyph,
					Renderer = new QuadTextureRenderer(quads, ResourceManager.GetProgram("textureText")),
					PRenderer = new PrimitiveRenderer(quads * 4, quads * 2, quads * 5) { WireframeMode = true }
				};

				infoDictionary.Add(glyph.Character, info);
			}

			this.pixelSize = font.PixelSize;
		}

		public void AddString(string text, float fontSize, Vector2 position, Color4 color)
		{
			float newLineX = position.X;
			for (int i = 0; i < text.Length; ++i)
			{
				AddChar(text[i], fontSize, ref position, newLineX, color);
			}
		}

		private void AddChar(char character, float fontSize, ref Vector2 position, float newLineX, Color4 color)
		{
			if (character == ' ')
			{
				AddSpace(ref position, fontSize);
				return;
			}

			if (character == '\n')
			{
				AddNewLine(ref position, newLineX, fontSize);
				return;
			}

			if (!infoDictionary.ContainsKey(character))
			{
				character = '?';
			}

			var info = infoDictionary[character];
			var glyph = info.Glyph;
			var size = new Vector2(glyph.Width, -glyph.Height) * fontSize / pixelSize;
			var offset = new Vector2(glyph.Left, glyph.Top) * fontSize / pixelSize;
			var advance = glyph.Advance * fontSize / pixelSize;

			Vector2 topLeft = position + offset;
			Rectangle rect = Rectangle.FromTwoPoints(topLeft, topLeft + size);

			info.Renderer.AddQuad(rect, color);
			info.PRenderer.AddRectangle(rect, color);

			position += new Vector2(advance, 0);
		}

		private void AddSpace(ref Vector2 position, float fontSize)
		{
			var info = infoDictionary['a'];
			var glyph = info.Glyph;
			var advance = glyph.Advance * fontSize / pixelSize;
			position += new Vector2(advance, 0);
		}

		private void AddNewLine(ref Vector2 position, float newLineX, float fontSize)
		{
			var a = infoDictionary['a'].Glyph;
			var j = infoDictionary['j'].Glyph;
			float diff = j.Height - a.Height;
			float height = (a.Height + diff) * fontSize / pixelSize;

			position = new Vector2(newLineX, position.Y - height);
		}

		public override void Reset()
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
