using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LCR_Game.Services.Interfaces;

namespace LCR_Game.Services
{
    public class LcrGame : ILcrGame
    {
        private const int ChipQuantity = 3;
        private LcrGamePlayer _currentLcrGamePlayer;


        public LcrGame(int players)
        {
            Players = new HashSet<LcrGamePlayer>();
            InitializePlayers(players);
        }

        public int NumberOfTurns { get; private set; }
        public HashSet<LcrGamePlayer> Players { get; }
        public LcrGamePlayer Winner { get; set; }

        public Task Play(CancellationToken ct)
        {
            return Task.Run(
                () =>
                {
                    var numberOfPlayersWithChips = Players.Count(p => p.ChipQuantity > 0);
                    while (numberOfPlayersWithChips != 1 || ct.IsCancellationRequested)
                    {
                        TakeTurn(_currentLcrGamePlayer);
                        _currentLcrGamePlayer = (LcrGamePlayer)_currentLcrGamePlayer.GamePlayerLeft;
                        NumberOfTurns++;
                        numberOfPlayersWithChips = Players.Count(p => p.ChipQuantity > 0);
                    }

                    Winner = Players.FirstOrDefault(p => p.ChipQuantity > 0);
                }, ct);
        }

        private void InitializePlayers(int count)
        {
            for (var i = 1; i <= count; i++) InsertPlayer(i, ChipQuantity);
        }

        private void InsertPlayer(int playerId, int chipQuantity)
        {
            LcrGamePlayer newNode;

            // If the list is empty, create a single node
            if (_currentLcrGamePlayer == null)
            {
                newNode = new LcrGamePlayer(playerId, chipQuantity);
                newNode.GamePlayerRight = newNode.GamePlayerLeft = newNode;
                _currentLcrGamePlayer = newNode;
                Players.Add(newNode);
                return;
            }

            // If list is not empty
            /* Find last node */
            LcrGamePlayer last = (LcrGamePlayer)_currentLcrGamePlayer.GamePlayerLeft;

            // Create Node dynamically
            newNode = new LcrGamePlayer(playerId, chipQuantity)
            {
                // Start is going to be next of new_node
                GamePlayerRight = _currentLcrGamePlayer,
                GamePlayerLeft = last
            };
            Players.Add(newNode);

            // Make new node previous of start
            _currentLcrGamePlayer.GamePlayerLeft = newNode;

            // Make new node next of old last
            last.GamePlayerRight = newNode;
        }

        private void ProcessDice(LcrGamePlayer currentLcrGamePlayer, LcrDiceSide lcrDiceSide)
        {
            switch (lcrDiceSide)
            {
                case LcrDiceSide.L:
                    currentLcrGamePlayer.RemoveChip();
                    ((LcrGamePlayer)currentLcrGamePlayer.GamePlayerLeft).AddChip();
                    return;
                case LcrDiceSide.C:
                    currentLcrGamePlayer.RemoveChip();
                    return;
                case LcrDiceSide.R:
                    currentLcrGamePlayer.RemoveChip();
                    ((LcrGamePlayer)currentLcrGamePlayer.GamePlayerLeft).AddChip();
                    return;
                case LcrDiceSide.Dot:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lcrDiceSide), lcrDiceSide, null);
            }
        }

        private void TakeTurn(LcrGamePlayer currentLcrGamePlayer)
        {
            if (currentLcrGamePlayer.ChipQuantity == 0) return;

            for (var i = 0; i < currentLcrGamePlayer.ChipQuantity; i++)
            {
                LcrDice lcrDice = new LcrDice();
                LcrDiceSide lcrDiceSide = lcrDice.Roll();
                ProcessDice(currentLcrGamePlayer, lcrDiceSide);
            }
        }
    }
}