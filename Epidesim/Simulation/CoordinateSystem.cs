using Epidesim.Engine.Drawing.Types;
using OpenTK;
using System;

namespace Epidesim.Simulation
{
	class CoordinateSystem
	{
		public Rectangle ViewRectangle { get; set; }
		public float ScreenWidth { get; set; }
		public float ScreenHeight { get; set; }

		public Matrix4 GetTransformationMatrix()
		{
			return Matrix4.Identity
				* Matrix4.CreateTranslation(ViewRectangle.Center.X, ViewRectangle.Center.Y, 0)
				* Matrix4.CreateScale(1.0f / ViewRectangle.Width, 1.0f / ViewRectangle.Height, 1);
		}

		public Vector2 WorldCoordinateToGLCoordinate(Vector2 world)
		{
			return Vector2.Divide(world - ViewRectangle.Center, ViewRectangle.Size) * 2.0f;
		}

		public Vector2 GLCoordinateToWorldCoordinate(Vector2 gl)
		{
			return gl * 0.5f * ViewRectangle.Size + ViewRectangle.Center;
		}

		public Vector2 WorldCoordinateToScreenCoordinate(Vector2 world)
		{
			Vector2 center = ViewRectangle.Center;
			Vector2 screenSize = new Vector2(ScreenWidth, ScreenHeight);
			Vector2 screenPos = Vector2.Divide(center - world, ViewRectangle.Size);
			return (screenPos + Vector2.One) / 2 * screenSize;
		}

		public Vector2 ScreenCoordinateToWorldCoordinate(Vector2 screen)
		{
			Vector2 center = ViewRectangle.Center;
			Vector2 screenSize = new Vector2(ScreenWidth, ScreenHeight);
			Vector2 screenPos = Vector2.Divide(screen, screenSize) * 2 - Vector2.One;
			return screenPos * ViewRectangle.Size - center;
		}
	}
}
