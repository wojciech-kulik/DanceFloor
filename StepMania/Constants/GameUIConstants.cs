using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMania.Constants
{
    public class GameUIConstants
    {
        public const int PixelsPerSecond = 200;
        public const int ArrowWidthHeight = 65;
        public const int MarginBetweenArrows = 50;

        public const int LeftArrowX = 0;
        public const int DownArrowX = ArrowWidthHeight + MarginBetweenArrows;
        public const int UpArrowX = DownArrowX + ArrowWidthHeight + MarginBetweenArrows;
        public const int RightArrowX = UpArrowX + ArrowWidthHeight + MarginBetweenArrows;

        public const string ResourceImagesPath = "pack://application:,,,/StepMania;component/Images/";
        public const string P1ArrowImage = ResourceImagesPath + "active_arrow_purple.png";
        public const string P2ArrowImage = ResourceImagesPath + "active_arrow_blue.png";
        public const string DefaultGameBackground = ResourceImagesPath + "game_background.jpg";

        public const int SongItemWidth = 250;
    }
}
