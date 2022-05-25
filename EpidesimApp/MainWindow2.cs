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
using Epidesim.Engine;

namespace Epidesim
{
	class MainWindow2 : GameWindow
	{
		#region Fields

		private EpidemicSimulation SimulationToRun;
		private ISimulationRenderer<EpidemicSimulation> SimulationRenderer;
		
		public Color4 BackgroundColor { get; set; }

		private static MainWindow2 _instance;
		public static MainWindow2 Get()
		{
			if (_instance == null)
			{
				_instance = new MainWindow2(800, 450, "hello world");
			}
			return _instance;
		}

		#endregion

		#region Constructors

		private MainWindow2(int width, int height, string title)
			: base(width, height, new GraphicsMode(ColorFormat.Empty, 1, 1, 4), title)
		{
			SimulationRenderer = new EpidemicSimulationRenderer();
			SimulationToRun = new EpidemicSimulation();
			BackgroundColor = Color.MidnightBlue;
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
			GL.Enable(EnableCap.Multisample);

			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			GL.Hint(HintTarget.MultisampleFilterHintNv, HintMode.Fastest);

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
			base.OnResize(e);
			if (Width == 0 && Height == 0)
			{
				// ignore minimize;
				return;
			}

			GL.Viewport(0, 0, Width, Height);
			SimulationToRun.SetScreenSize(Width, Height);
		}

		protected override void OnUnload(EventArgs e)
		{
		}

		#endregion
	}
}
