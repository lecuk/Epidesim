using System;
using System.IO;
using Epidesim.Engine.Drawing;
using Epidesim.Simulation;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
using System.Diagnostics;
using System.Drawing;
using Epidesim.Simulation.Polygon;
using Epidesim.Simulation.Epidemic;

namespace Epidesim.Engine
{
	class MainWindow : GameWindow
	{
		#region Fields

		private EpidemicSimulation SimulationToRun;
		private ISimulationRenderer<EpidemicSimulation> SimulationRenderer;
		
		public Color4 BackgroundColor { get; set; }

		private static MainWindow _instance;
		public static MainWindow Get()
		{
			if (_instance == null)
			{
				_instance = new MainWindow(800, 450, "hello world");
			}
			return _instance;
		}

		#endregion

		#region Constructors

		private MainWindow(int width, int height, string title)
			: base(width, height, GraphicsMode.Default, title)
		{
			InitResources();

			var cityBuilder = new CityBuilder()
			{
				SectorSize = 40f,
				RoadWidth = 5f
			};

			SimulationRenderer = new EpidemicSimulationRenderer();
			SimulationToRun = new EpidemicSimulation(cityBuilder.Build(20, 15), 5000);
			BackgroundColor = Color.MidnightBlue;
		}

		~MainWindow()
		{
		}

		#endregion

		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// depth doesn't seem to work with blending
			//GL.Enable(EnableCap.DepthTest);

			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.Texture2D);

			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

			SimulationToRun.Start();
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			Input.Refresh();

			SimulationToRun.Update(e.Time);

			if (Input.IsKeyDown(Key.Escape))
			{
				base.Exit();
			}

			base.OnUpdateFrame(e);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.ClearColor(BackgroundColor);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			SimulationRenderer.Render(SimulationToRun);

			base.Context.SwapBuffers();
			base.OnRenderFrame(e);
		}

		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(0, 0, Width, Height);
			SimulationToRun.SetScreenSize(Width, Height);
			base.OnResize(e);
		}

		protected override void OnUnload(EventArgs e)
		{
		}

		private void InitResources()
		{
			InitShaders();
			InitShaderPrograms();
			InitTextures();
			InitFonts();
		}

		private void InitShaders()
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

		private void InitShaderPrograms()
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

		private void InitTextures()
		{
			ResourceManager.AddTexture("halo",
				Texture2DLoader.LoadFromFile("Resources/halo.png"));

			ResourceManager.AddTexture("osu",
				Texture2DLoader.LoadFromFile("Resources/osu.png"));

			ResourceManager.AddTexture("prometheus",
				Texture2DLoader.LoadFromFile("Resources/prometheus.jpg"));
		}

		private void InitFonts()
		{
			string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,;/\\()<>{}[]+-=|!?\"\'";

			ResourceManager.AddTextureFont("arial",
				TextureFontGenerator.Generate("Resources/arial.ttf", alphabet.ToCharArray()));

			ResourceManager.AddTextureFont("consolas",
				TextureFontGenerator.Generate("Resources/consolas.ttf", alphabet.ToCharArray()));
		}

		#endregion
	}
}
