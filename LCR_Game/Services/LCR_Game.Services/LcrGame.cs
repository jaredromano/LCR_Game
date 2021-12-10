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
        private Player _currentPlayer;


        public LcrGame(int players)
        {
            Players = new HashSet<Player>();
            InitializePlayers(players);
        }

        public int NumberOfTurns { get; private set; }
        public HashSet<Player> Players { get; }
        public Player Winner { get; set; }

        public Task Play(CancellationToken ct)
        {
            return Task.Run(
                () =>
                {
                    var numberOfPlayersWithChips = Players.Count(p => p.ChipQuantity > 0);
                    while (numberOfPlayersWithChips != 1 || ct.IsCancellationRequested)
                    {
                        TakeTurn(_currentPlayer);
                        _currentPlayer = _currentPlayer.PlayerLeft;
                        NumberOfTurns++;
                        numberOfPlayersWithChips = Players.Count(p => p.ChipQuantity > 0);
                    }

                    Winner = Players.FirstOrDefault(p => p.ChipQuantity > 0);
                }, ct);
        }

        private void InitializePlayers(int count)
        {
            for (var i = 0; i < count; i++) InsertPlayer(i, ChipQuantity);
        }

        private void InsertPlayer(int playerNumber, int chipQuantity)
        {
            Player newNode;

            // If the list is empty, create a single node
            if (_currentPlayer == null)
            {
                newNode = new Player(playerNumber, chipQuantity);
                newNode.PlayerRight = newNode.PlayerLeft = newNode;
                _currentPlayer = newNode;
                Players.Add(newNode);
                return;
            }

            // If list is not empty
            /* Find last node */
            Player last = _currentPlayer.PlayerLeft;

            // Create Node dynamically
            newNode = new Player(playerNumber, chipQuantity)
            {
                // Start is going to be next of new_node
                PlayerRight = _currentPlayer,
                PlayerLeft = last
            };
            Players.Add(newNode);

            // Make new node previous of start
            _currentPlayer.PlayerLeft = newNode;

            // Make new node next of old last
            last.PlayerRight = newNode;
        }

        private void ProcessDice(Player currentPlayer, DiceSide diceSide)
        {
            switch (diceSide)
            {
                case DiceSide.L:
                    currentPlayer.RemoveChip();
                    currentPlayer.PlayerLeft.AddChip();
                    return;
                case DiceSide.C:
                    currentPlayer.RemoveChip();
                    return;
                case DiceSide.R:
                    currentPlayer.RemoveChip();
                    currentPlayer.PlayerRight.AddChip();
                    return;
                case DiceSide.Dot:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(diceSide), diceSide, null);
            }
        }

        private void TakeTurn(Player currentPlayer)
        {
            if (currentPlayer.ChipQuantity == 0) return;

            for (var i = 0; i < currentPlayer.ChipQuantity; i++)
            {
                Dice dice = new Dice();
                DiceSide diceSide = dice.Roll();
                ProcessDice(currentPlayer, diceSide);
            }
        }
    }
}