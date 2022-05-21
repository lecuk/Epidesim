using System.Collections.Generic;

namespace Epidesim.Engine.Drawing.Types
{
	public class TextureFont
	{
		public struct Glyph
		{
			public Texture2D Texture;

			public char Character;

			public float Left;
			public float Top;
			public float Width;
			public float Height;
			
			public float Advance;
		}
		
		private readonly Dictionary<char, Glyph> alphabet;

		public IReadOnlyDictionary<char, Glyph> Alphabet => alphabet;
		public IEnumerable<char> SupportedChars => alphabet.Keys;

		public readonly float PixelSize;

		public TextureFont(Dictionary<char, Glyph> alphabet, float pixelSize)
		{
			this.alphabet = alphabet;
			this.PixelSize = pixelSize;
		}
	}
}
