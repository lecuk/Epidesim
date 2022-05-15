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
				* Matrix4.CreateTranslation(-ViewRectangle.Center.X, -ViewRectangle.Center.Y, 0)
				* Matrix4.CreateScale(2.0f / ViewRectangle.Width, 2.0f / ViewRectangle.Height, 1);
		}

		public Vector2 WorldCoordinateToScreenCoordinate(Vector2 world)
		{
			Vector2 leftBottom = new Vector2(ViewRectangle.Lft, ViewRectangle.Bot);
			Vector2 screenSize = new Vector2(ScreenWidth, ScreenHeight);
			Vector2 screenPos = Vector2.Divide(world - leftBottom, ViewRectangle.Size);
			return screenPos * screenSize;
		}

		public Vector2 ScreenCoordinateToWorldCoordinate(Vector2 screen)
		{
			Vector2 leftBottom = new Vector2(ViewRectangle.Lft, ViewRectangle.Bot);
			Vector2 screenSize = new Vector2(ScreenWidth, ScreenHeight);
			Vector2 screenPos = Vector2.Divide(screen, screenSize);
			return screenPos * ViewRectangle.Size + leftBottom;
		}
	}
}
