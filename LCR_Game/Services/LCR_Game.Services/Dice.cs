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
        public readonly IList<LcrDiceSide> Sides = new List<LcrDiceSide>()
        {
            LcrDiceSide.L,
            LcrDiceSide.C,
            LcrDiceSide.R,
            LcrDiceSide.Dot,
            LcrDiceSide.Dot,
            LcrDiceSide.Dot
        };

        /// <summary>
        /// Rolls the dice
        /// </summary>
        /// <returns>Randomly generated side of the dice</returns>
        public LcrDiceSide Roll()
        {
            var sidesCount = Sides.Count();
            var diceIndex = _random.Next(0, sidesCount);
            return Sides[diceIndex];
        }
    }
}
