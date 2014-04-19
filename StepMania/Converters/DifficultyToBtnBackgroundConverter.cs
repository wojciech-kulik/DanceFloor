using Caliburn.Micro;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace StepMania.Converters
{
    public class DifficultyToBtnBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var player = (IPlayer)value;
            string d = (parameter as string);

            Difficulty difficulty = Difficulty.Easy;
            if (d == "easy")
                difficulty = Difficulty.Easy;
            else if (d == "medium")
                difficulty = Difficulty.Medium;
            else
                difficulty = Difficulty.Hard;

            if (player.Difficulty != difficulty)
                return new LinearGradientBrush(new Color() { A = 0xFF, R = 0xF5, G = 0xBF, B = 0x2F }, new Color() { A = 0xFF, R = 0xAA, G = 0x7F, B = 0x0C }, 90.0);
            else
                return new LinearGradientBrush(Colors.White, new Color() { A = 0xFF, R = 0x85, G = 0x85, B = 0x85 }, 90.0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
