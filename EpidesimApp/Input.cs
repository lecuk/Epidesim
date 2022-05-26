using OpenTK;
using OpenTK.Input;
using System.Windows;

namespace Epidesim
{
	public class Input
	{
		public static FrameworkElement Origin { get; set; }

		private static MouseState PreviousMouseState { get; set; }
		private static MouseState MouseState { get; set; }
		private static KeyboardState PreviousKeyboardState { get; set; }
		private static KeyboardState KeyboardState { get; set; }

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
			var point = Origin.PointFromScreen(new Point(MouseState.X, MouseState.Y));
			return new Vector2((float)point.X, (float)(Origin.ActualHeight - point.Y));
		}
	}
}
