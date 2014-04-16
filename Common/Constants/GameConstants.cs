using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GameConstants
    {
        //Hit Points
        public static readonly int BestHitPoints = 20;
        public static readonly int MediumHitPoints = 10;
        public static readonly int WorstHitPoints = 5;

        //Hit Times
        public static readonly double BestHitTime = 0.05;
        public static readonly double MediumHitTime = 0.13;
        public static readonly double WorstHitTime = 0.2;

        //Life Points
        public static readonly int BombLifePoints = 20;
        public static readonly int MissLifePoints = 8;
        public static readonly int WrongMomentOrActionLifePoints = 4;

        public static readonly int FullLife = 100;
    }
}
