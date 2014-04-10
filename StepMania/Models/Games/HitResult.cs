using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMania.Models.Games
{
    public struct HitResult
    {
        public HitResult(int points, int life, bool bomb = false)
        {
            Points = points;
            Life = life;
            Bomb = bomb;
        }

        public int Points;

        public int Life;

        public bool Bomb;
    }
}
