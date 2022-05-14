using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;

namespace Epidesim.Engine
{
	static class ResourceManager
	{
		private static readonly ResourceStorage<Shader> ShaderStorage = new ResourceStorage<Shader>();
		private static readonly ResourceStorage<ShaderProgram> ShaderProgramStorage = new ResourceStorage<ShaderProgram>();
		private static readonly ResourceStorage<Texture2D> TextureStorage = new ResourceStorage<Texture2D>();
		private static readonly ResourceStorage<TextureFont> FontStorage = new ResourceStorage<TextureFont>();

		public static void AddShader(string name, Shader shader)
		{
			ShaderStorage.AddResource(name, shader);
		}

		public static Shader GetShader(string name)
		{
			return ShaderStorage.GetResource(name);
		}

		public static void AddProgram(string name, ShaderProgram program)
		{
			ShaderProgramStorage.AddResource(name, program);
		}

		public static ShaderProgram GetProgram(string name)
		{
			return ShaderProgramStorage.GetResource(name);
		}

		public static void AddTexture(string name, Texture2D texture)
		{
			TextureStorage.AddResource(name, texture);
		}

		public static Texture2D GetTexture(string name)
		{
			return TextureStorage.GetResource(name);
		}

		public static void AddTextureFont(string name, TextureFont font)
		{
			FontStorage.AddResource(name, font);
		}

		public static TextureFont GetTextureFont(string name)
		{
			return FontStorage.GetResource(name);
		}
	}
}
