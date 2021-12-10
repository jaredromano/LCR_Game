using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCR_Game.Services
{
    public sealed class Dice
    {
        /// <summary>
        /// Random number generator
        /// </summary>
        private Random _random = new Random();

        /// <summary>
        /// The 6 sides of the dice
        /// </summary>
        public readonly IList<DiceSide> Sides = new List<DiceSide>()
        {
            DiceSide.L,
            DiceSide.C,
            DiceSide.R,
            DiceSide.Dot,
            DiceSide.Dot,
            DiceSide.Dot
        };

        /// <summary>
        /// Rolls the dice
        /// </summary>
        /// <returns>Randomly generated side of the dice</returns>
        public DiceSide Roll()
        {
            var sidesCount = Sides.Count();
            var diceIndex = _random.Next(0, sidesCount);
            return Sides[diceIndex];
        }
    }
}
