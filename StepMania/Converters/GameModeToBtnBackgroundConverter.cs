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
    public class GameModeToBtnBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var game = (IGame)value;
            string p = (parameter as string);

            if ((p == "single" && !game.IsMultiplayer) || (p == "multi" && game.IsMultiplayer))
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
