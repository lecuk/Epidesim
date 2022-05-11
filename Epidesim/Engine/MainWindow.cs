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

namespace Epidesim.Engine
{
	class MainWindow : GameWindow
	{
		#region Fields

		private PolygonSimulation SimulationToRun;
		private ISimulationRenderer<PolygonSimulation> SimulationRenderer;

		private VertexShader vertexShader;
		private FragmentShader fragmentShader;
		private ShaderProgram shaderProgram;
		
		public Color4 BackgroundColor { get; set; }

		#endregion

		#region Constructors

		public MainWindow(int width, int height, string title)
			: base(width, height, GraphicsMode.Default, title)
		{
			SimulationRenderer = new PolygonSimulationRenderer(shaderProgram);
			SimulationToRun = new PolygonSimulation(width, height);
			BackgroundColor = Color.MidnightBlue;
		}

		public MainWindow(int width, int height)
			: this(width, height, "no title") {  }

		public MainWindow()
			: this(800, 450) {  }

		~MainWindow()
		{
		}

		#endregion

		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GL.Enable(EnableCap.DepthTest);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			KeyboardState inputState = Keyboard.GetState();

			SimulationToRun.Update(e.Time);

			if (inputState.IsKeyDown(Key.Escape))
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
			SimulationToRun.Width = (float)Width;
			SimulationToRun.Height = (float)Height;
			base.OnResize(e);
		}

		protected override void OnUnload(EventArgs e)
		{
		}

		#endregion
	}
}
