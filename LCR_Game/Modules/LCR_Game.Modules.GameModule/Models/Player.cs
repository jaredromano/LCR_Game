using Prism.Mvvm;

namespace LCR_Game.Modules.GameModule.Models
{
    public class Player : BindableBase
    {
        private bool _isWinner;
        private int _numberOfWins;

        public Player(int id)
        {
            Id = id;
        }

        public int Id { get; }

        /// <summary>
        /// Flag for indicating if the player is the winner
        /// </summary>
        public bool IsWinner
        {
            get => _isWinner;
            set => SetProperty(ref _isWinner, value);
        }

        /// <summary>
        /// Indicates the number of game wins for the player
        /// </summary>
        public int NumberOfWins
        {
            get => _numberOfWins;
            set => SetProperty(ref _numberOfWins, value);
        }
    }
}