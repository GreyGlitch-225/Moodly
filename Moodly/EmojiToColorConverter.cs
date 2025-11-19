using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Moodly
{
    public class EmojiToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var emoji = value?.ToString();
            if (emoji == "😊") return Colors.LightGreen;
            if (emoji == "😐") return Colors.LightYellow;
            if (emoji == "😔") return Colors.LightBlue;
            if (emoji == "😡") return Colors.Red;
            if (emoji == "😂") return Colors.Green;
            if (emoji == "😍") return Colors.Pink;
            if (emoji == "😵‍💫") return Colors.Orange;
            if (emoji == "🤖") return Colors.Gray;
            return Colors.LightGray;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
