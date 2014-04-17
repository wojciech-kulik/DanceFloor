using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMania.Constants
{
    public class GameUIConstants
    {
        public static readonly int PixelsPerSecond = 200;
        public static readonly int ArrowWidthHeight = 65;
        public static readonly int MarginBetweenArrows = 50;

        public static readonly int LeftArrowX = 0;
        public static readonly int DownArrowX = ArrowWidthHeight + MarginBetweenArrows;
        public static readonly int UpArrowX = DownArrowX + ArrowWidthHeight + MarginBetweenArrows;
        public static readonly int RightArrowX = UpArrowX + ArrowWidthHeight + MarginBetweenArrows;

        public static readonly string ResourceImagesPath = "pack://application:,,,/StepMania;component/Images/";
        public static readonly string P1ArrowImage = ResourceImagesPath + "active_arrow_purple.png";
        public static readonly string P2ArrowImage = ResourceImagesPath + "active_arrow_blue.png";
        public static readonly string DefaultGameBackground = ResourceImagesPath + "game_background.jpg";
    }
}
