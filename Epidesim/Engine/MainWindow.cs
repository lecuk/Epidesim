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
		
		public Color4 BackgroundColor { get; set; }

		#endregion

		#region Constructors

		public MainWindow(int width, int height, string title)
			: base(width, height, GraphicsMode.Default, title)
		{
			SimulationRenderer = new PolygonSimulationRenderer();
			SimulationToRun = new PolygonSimulation(width, height);
			BackgroundColor = Color.MidnightBlue;
			SimulationRenderer.ScreenWidth = Width;
			SimulationRenderer.ScreenHeight = Height;
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
			GL.DepthFunc(DepthFunction.Less);
			GL.DepthRange(0, 99999);
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
			SimulationToRun.Width = (float)Width;
			SimulationToRun.Height = (float)Height;
			SimulationRenderer.ScreenWidth = Width;
			SimulationRenderer.ScreenHeight = Height;
			base.OnResize(e);
		}

		protected override void OnUnload(EventArgs e)
		{
		}

		#endregion
	}
}
