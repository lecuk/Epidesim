using Epidesim.Engine.Drawing.Types;
using OpenTK;
using System;

namespace Epidesim.Engine
{
	public class CoordinateSystem
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
			var leftBottom = new Vector2(ViewRectangle.Lft, ViewRectangle.Bot);
			var screenSize = new Vector2(ScreenWidth, ScreenHeight);
			var screenPos = Vector2.Divide(world - leftBottom, ViewRectangle.Size);
			return screenPos * screenSize;
		}

		public Vector2 ScreenCoordinateToWorldCoordinate(Vector2 screen)
		{
			var leftBottom = new Vector2(ViewRectangle.Lft, ViewRectangle.Bot);
			var screenSize = new Vector2(ScreenWidth, ScreenHeight);
			var screenPos = Vector2.Divide(screen, screenSize);
			return screenPos * ViewRectangle.Size + leftBottom;
		}

		public Vector2 WorldDeltaScreenDelta(Vector2 world)
		{
			var screenSize = new Vector2(ScreenWidth, ScreenHeight);
			var screenPos = Vector2.Divide(world, ViewRectangle.Size);
			return screenPos * screenSize;
		}

		public Vector2 ScreenDeltaToWorldDelta(Vector2 screen)
		{
			var screenSize = new Vector2(ScreenWidth, ScreenHeight);
			var screenPos = Vector2.Divide(screen, screenSize);
			return screenPos * ViewRectangle.Size;
		}
	}
}
