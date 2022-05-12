using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Engine
{
	class Input
	{
		private static MouseState PreviousMouseState { get; set; }
		public static MouseState MouseState { get; private set; }

		private static KeyboardState PreviousKeyboardState { get; set; }
		public static KeyboardState KeyboardState { get; private set; }

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
			return new Vector2(MouseState.X - PreviousMouseState.X, MouseState.Y - PreviousMouseState.Y);
		}

		public static float GetMouseWheelDelta()
		{
			return MouseState.Wheel - PreviousMouseState.Wheel;
		}
	}
}
