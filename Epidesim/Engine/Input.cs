using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Engine
{
	public class Input
	{
		private static MainWindow Window { get; set; }

		private static MouseState PreviousMouseState { get; set; }
		private static MouseState MouseState { get; set; }

		private static KeyboardState PreviousKeyboardState { get; set; }
		private static KeyboardState KeyboardState { get; set; }

		static Input()
		{
			Window = MainWindow.Get();
		}

		public static void Refresh()
		{
			PreviousMouseState = MouseState;
			PreviousKeyboardState = KeyboardState;

			MouseState = Mouse.GetCursorState();
			KeyboardState = Keyboard.GetState();
		}

		public static bool IsMouseButtonDown(MouseButton button)
		{
			return MouseState.IsButtonDown(button);
		}

		public static bool IsMouseButtonUp(MouseButton button)
		{
			return MouseState.IsButtonUp(button);
		}

		public static bool WasMouseButtonJustPressed(MouseButton button)
		{
			return MouseState.IsButtonDown(button) && PreviousMouseState.IsButtonUp(button);
		}

		public static bool WasMouseButtonJustReleased(MouseButton button)
		{
			return MouseState.IsButtonUp(button) && PreviousMouseState.IsButtonDown(button);
		}

		public static bool WasKeyJustPressed(Key key)
		{
			return KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
		}

		public static bool WasKeyJustReleased(Key key)
		{
			return KeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key);
		}

		public static bool IsKeyDown(Key key)
		{
			return KeyboardState.IsKeyDown(key);
		}

		public static bool IsKeyUp(Key key)
		{
			return KeyboardState.IsKeyUp(key);
		}

		public static Vector2 GetMouseDelta()
		{
			return new Vector2(MouseState.X - PreviousMouseState.X, -(MouseState.Y - PreviousMouseState.Y));
		}

		public static float GetMouseWheelDelta()
		{
			return MouseState.Wheel - PreviousMouseState.Wheel;
		}

		public static Vector2 GetMouseAbsolutePosition()
		{
			return new Vector2(MouseState.X, MouseState.Y);
		}

		public static Vector2 GetMouseLocalPosition()
		{
			var point = Window.PointToClient(new System.Drawing.Point(MouseState.X, MouseState.Y));
			return new Vector2(point.X, Window.Height - point.Y);
		}
	}
}
