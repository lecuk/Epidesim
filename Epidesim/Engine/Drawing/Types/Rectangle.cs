using OpenTK;
using System;

namespace Epidesim.Engine.Drawing.Types
{
	struct Rectangle
	{
		public Vector2 A;
		public Vector2 B;

		public float Lft => Math.Min(A.X, B.X);
		public float Top => Math.Max(A.Y, B.Y);
		public float Rgt => Math.Max(A.X, B.X);
		public float Bot => Math.Min(A.Y, B.Y);

		public Vector2 Center => (A + B) * 0.5f;

		public float Width => Rgt - Lft;
		public float Height => Top - Bot;
		public Vector2 Size => new Vector2(Width, Height);

		public Rectangle(Vector2 a, Vector2 b)
		{
			A = a;
			B = b;
		}

		public static Rectangle FromTwoPoints(Vector2 a, Vector2 b)
		{
			return new Rectangle(a, b);
		}

		public static Rectangle FromCenterAndSize(Vector2 center, Vector2 size)
		{
			return new Rectangle(center - size * 0.5f, center + size * 0.5f);
		}

		public float Perimeter => (Width + Height) * 2.0f;
		public float Area => Width * Height;

		public Rectangle Translate(Vector2 offset)
		{
			return Rectangle.FromTwoPoints(A + offset, B + offset);
		}

		public Rectangle Scale(Vector2 scale)
		{
			return Rectangle.FromCenterAndSize(Center, Size * scale);
		}

		public bool ContainsPoint(Vector2 point)
		{
			return Lft < point.X && point.X < Rgt
				&& Bot < point.Y && point.Y < Top;
		}
	}
}
