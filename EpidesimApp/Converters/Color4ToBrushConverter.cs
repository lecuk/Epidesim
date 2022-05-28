using OpenTK.Graphics;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EpidesimApp.Converters
{
	class Color4ToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Color4 color4)
			{
				return new SolidColorBrush(Color.FromArgb(
					(byte)(color4.A * 255), 
					(byte)(color4.R * 255), 
					(byte)(color4.G * 255), 
					(byte)(color4.B * 255)));
			}

			return new SolidColorBrush();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
