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

            if (player.Difficulty == difficulty)
                return new SolidColorBrush(new Color() { A = 0xDE, R = 0xF1, G = 0xD2, B = 0x69 });
            else
                return new SolidColorBrush(new Color() { A = 0xDE, R = 0x2B, G = 0xAC, B = 0x47 });
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
