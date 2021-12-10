using System;
using System.Collections.Generic;
using System.Text;

namespace LCR_Game.Services
{
    public sealed class LcrGamePlayer : IGamePlayer
    {
        public LcrGamePlayer(int playerId, int chipQuantity)
        {
            Id = playerId;
            ChipQuantity = chipQuantity;
        }

        public int Id { get; set; }

        public IGamePlayer GamePlayerLeft { get; internal set; }

        public IGamePlayer GamePlayerRight { get; internal set; }

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
