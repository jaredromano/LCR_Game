using System;
using System.Collections.Generic;
using System.Text;

namespace LCR_Game.Services
{
    public sealed class Player
    {
        public Player(int playerNumber, int chipQuantity)
        {
            ChipQuantity = chipQuantity;
        }

        public int PlayerNumber { get; set; }

        public Player PlayerLeft { get; internal set; }

        public Player PlayerRight { get; internal set; }

        public int ChipQuantity { get; private set; }

        public void RemoveChip()
        {
            if (ChipQuantity == 0) throw new InvalidOperationException("Cannot remove chip, no chips left");
            ChipQuantity--;
        }

        public void AddChip()
        {
            ChipQuantity++;
        }
    }
}
