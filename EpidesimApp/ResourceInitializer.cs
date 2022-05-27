using Epidesim.Engine;
using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
using System;

namespace Epidesim
{
	static class ResourceInitializer
	{
		private static bool isInitialized = false;

		public static void Init()
		{
			if (isInitialized)
			{
				throw new Exception("Already initialized.");
			}

			InitShaders();
			InitShaderPrograms();
			InitTextures();
			InitFonts();

			isInitialized = true;
		}

		private static void InitShaders()
		{
			ResourceManager.AddShader("simpleVertex",
				VertexShader.Load("Shaders/Simple/VertexShader.glsl"));
			ResourceManager.AddShader("simpleFragment",
				FragmentShader.Load("Shaders/Simple/FragmentShader.glsl"));

			ResourceManager.AddShader("textureVertex",
				VertexShader.Load("Shaders/Texture/VertexShader.glsl"));
			ResourceManager.AddShader("textureFragment",
				FragmentShader.Load("Shaders/Texture/FragmentShader.glsl"));

			ResourceManager.AddShader("textVertex",
				VertexShader.Load("Shaders/Text/VertexShader.glsl"));
			ResourceManager.AddShader("textFragment",
				FragmentShader.Load("Shaders/Text/FragmentShader.glsl"));
		}

		private static void InitShaderPrograms()
		{
			ResourceManager.AddProgram("primitive", new ShaderProgram(
				ResourceManager.GetShader("simpleVertex") as VertexShader,
				ResourceManager.GetShader("simpleFragment") as FragmentShader));

			ResourceManager.AddProgram("textureDefault", new ShaderProgram(
				ResourceManager.GetShader("textureVertex") as VertexShader,
				ResourceManager.GetShader("textureFragment") as FragmentShader));

			ResourceManager.AddProgram("textureText", new ShaderProgram(
				ResourceManager.GetShader("textVertex") as VertexShader,
				ResourceManager.GetShader("textFragment") as FragmentShader));
		}

		private static void InitTextures()
		{
			ResourceManager.AddTexture("halo",
				Texture2DLoader.LoadFromFile("Resources/halo.png"));

			ResourceManager.AddTexture("osu",
				Texture2DLoader.LoadFromFile("Resources/osu.png"));

			ResourceManager.AddTexture("prometheus",
				Texture2DLoader.LoadFromFile("Resources/prometheus.jpg"));
		}

		private static void InitFonts()
		{
			string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,;:/\\()<>{}[]+-=|!?\"\'%";

			ResourceManager.AddTextureFont("arial",
				TextureFontGenerator.Generate("Resources/arial.ttf", 128, alphabet.ToCharArray()));

			ResourceManager.AddTextureFont("consolas",
				TextureFontGenerator.Generate("Resources/consolas.ttf", 128, alphabet.ToCharArray()));
		}
	}
}
