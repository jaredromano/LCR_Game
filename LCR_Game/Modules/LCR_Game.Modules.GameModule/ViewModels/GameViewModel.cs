using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LCR_Game.Core.Mvvm;
using LCR_Game.Modules.GameModule.Models;
using LCR_Game.Services;
using Prism.Commands;
using Prism.Regions;

namespace LCR_Game.Modules.GameModule.ViewModels
{
    public class GameViewModel : RegionViewModelBase
    {
        private ObservableCollection<KeyValuePair<int, int>> _averagePlotPoints;
        private DelegateCommand _cancelCommand;
        private CancellationTokenSource _gameCancellationTokenSource;
        private int _gamesQuantity;
        private bool _isGamePlaying;
        private ObservableCollection<KeyValuePair<int, int>> _maxPlotPoints;
        private ObservableCollection<KeyValuePair<int, int>> _minPlotPoints;
        private DelegateCommand _playCommandCommand;
        private ObservableCollection<Player> _players;
        private int _playersQuantity;
        private ObservableCollection<KeyValuePair<int, int>> _plotPoints;
        private Preset _selectedPreset;

        public GameViewModel(IRegionManager regionManager) :
            base(regionManager)
        {
            SelectedPreset = PresetsList.FirstOrDefault();
        }

        /// <summary>
        ///     A collection of key value pairs containing the average number of turns
        /// </summary>
        public ObservableCollection<KeyValuePair<int, int>> AveragePlotPoints
        {
            get => _averagePlotPoints;
            set => SetProperty(ref _averagePlotPoints, value);
        }

        /// <summary>
        ///     Cancels the current game
        /// </summary>
        public DelegateCommand CancelCommand
            => _cancelCommand ??= new DelegateCommand(ExecuteCancel, CanExecuteCancel)
                .ObservesProperty(() => IsGamePlaying);

        /// <summary>
        ///     The number of games configured for the current run
        /// </summary>
        public int GamesQuantity
        {
            get => _gamesQuantity;
            set => SetProperty(ref _gamesQuantity, value);
        }

        /// <summary>
        ///     Flag to indicate the a game is running, controls the play and cancel buttons
        /// </summary>
        public bool IsGamePlaying
        {
            get => _isGamePlaying;
            set => SetProperty(ref _isGamePlaying, value);
        }


        /// <summary>
        ///     A collection of key value pairs of the games with the maximum number of terns in the set vs the game#
        /// </summary>
        public ObservableCollection<KeyValuePair<int, int>> MaxPlotPoints
        {
            get => _maxPlotPoints;
            set => SetProperty(ref _maxPlotPoints, value);
        }

        /// <summary>
        ///     A collection of key value pairs of the games with the minimum number of terns in the set vs the game#
        /// </summary>
        public ObservableCollection<KeyValuePair<int, int>> MinPlotPoints
        {
            get => _minPlotPoints;
            set => SetProperty(ref _minPlotPoints, value);
        }

        /// <summary>
        ///     A Command to start playing the series of games
        /// </summary>
        public DelegateCommand PlayCommand
            => _playCommandCommand ??= new DelegateCommand(ExecutePlay, CanExecutePlay)
                .ObservesProperty(() => IsGamePlaying);

        /// <summary>
        ///     Players of the current game
        /// </summary>
        public ObservableCollection<Player> Players
        {
            get => _players;
            set => SetProperty(ref _players, value);
        }

        /// <summary>
        ///     Number of players for the current game configured by the user
        /// </summary>
        public int PlayersQuantity
        {
            get => _playersQuantity;
            set => SetProperty(ref _playersQuantity, value);
        }

        /// <summary>
        ///     A Collection of key value pairs that contain the number of turns per game
        ///     <para>Key is the game number</para>
        ///     <para>Value is the number of turns</para>
        /// </summary>
        public ObservableCollection<KeyValuePair<int, int>> PlotPoints
        {
            get => _plotPoints;
            set => SetProperty(ref _plotPoints, value);
        }

        /// <summary>
        ///     A list of presets a user can select
        /// </summary>
        public IList<Preset> PresetsList { get; } = new List<Preset>
        {
            new() { PlayersQuantity = 3, GamesQuantity = 100 },
            new() { PlayersQuantity = 4, GamesQuantity = 100 },
            new() { PlayersQuantity = 5, GamesQuantity = 100 },
            new() { PlayersQuantity = 5, GamesQuantity = 1000 },
            new() { PlayersQuantity = 5, GamesQuantity = 10000 },
            new() { PlayersQuantity = 5, GamesQuantity = 100000 },
            new() { PlayersQuantity = 6, GamesQuantity = 100 },
            new() { PlayersQuantity = 7, GamesQuantity = 100 }
        };

        /// <summary>
        ///     The currently selected preset
        /// </summary>
        public Preset SelectedPreset
        {
            get => _selectedPreset;
            set
            {
                if (!SetProperty(ref _selectedPreset, value)) return;
                GamesQuantity = _selectedPreset.GamesQuantity;
                PlayersQuantity = _selectedPreset.PlayersQuantity;
            }
        }


        /// <summary>
        ///     Fires when the view is the navigation target
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //not implemented
        }

        private ObservableCollection<KeyValuePair<int, int>> CalculateAveragePlotPoints()
        {
            var average = (int)PlotPoints.Select(p => p.Value).Average();
            // var averagePlotPoints = Enumerable.Range(0, numberOfGames)
            //     .Select(x => new KeyValuePair<int, int>(x, average));

            var averagePlotPoints = new List<KeyValuePair<int, int>>
            {
                new(0, average),
                new(PlotPoints.Count() - 1, average)
            };

            return new ObservableCollection<KeyValuePair<int, int>>(averagePlotPoints);
        }

        private ObservableCollection<KeyValuePair<int, int>> CalculateMaxPlotPoints()
        {
            var max = PlotPoints.Select(p => p.Value).Max();
            var maxPlotPoints = PlotPoints.Where(p => p.Value == max);
            return new ObservableCollection<KeyValuePair<int, int>>(maxPlotPoints);
        }

        private ObservableCollection<KeyValuePair<int, int>> CalculateMinPlotPoints()
        {
            var min = PlotPoints.Select(p => p.Value).Min();
            var minPlotPoints = PlotPoints.Where(p => p.Value == min);
            return new ObservableCollection<KeyValuePair<int, int>>(minPlotPoints);
        }

        /// <summary>
        ///     Indicates if the Cancel button can be pressed
        /// </summary>
        private bool CanExecuteCancel()
            => IsGamePlaying;

        /// <summary>
        ///     Indicates if the Play button can be pressed
        /// </summary>
        private bool CanExecutePlay()
            => !IsGamePlaying;

        /// <summary>
        ///     Action for the <see cref="CancelCommand" />
        /// </summary>
        private void ExecuteCancel()
        {
            _gameCancellationTokenSource.Cancel();
        }

        /// <summary>
        ///     Action for the <see cref="PlayCommand" />
        /// </summary>
        private async void ExecutePlay()
        {
            _gameCancellationTokenSource = new CancellationTokenSource();
            PlotPoints = new ObservableCollection<KeyValuePair<int, int>>();
            IsGamePlaying = true;
            try
            {
                await PlayGames(PlayersQuantity, GamesQuantity, _gameCancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                IsGamePlaying = false;
            }
        }

        /// <summary>
        ///     Task for playing all games. populates <see cref="PlotPoints" />, <see cref="AveragePlotPoints" />,
        ///     <see cref="MaxPlotPoints" />, <see cref="MinPlotPoints" />,
        /// </summary>
        private async Task PlayGames(int numberOfPlayers, int numberOfGames, CancellationToken ct)
        {
            // Create players
            var players = Enumerable.Range(1, numberOfPlayers).Select(i => new Player(i));
            Players = new ObservableCollection<Player>(players);

            // Clear out previous game data
            AveragePlotPoints = new ObservableCollection<KeyValuePair<int, int>>();
            MaxPlotPoints = new ObservableCollection<KeyValuePair<int, int>>();
            MinPlotPoints = new ObservableCollection<KeyValuePair<int, int>>();

            // Play through games
            for (var i = 0; i < numberOfGames; i++)
            {
                LcrGame game = new(numberOfPlayers);
                await game.Play(ct);
                PlotPoints.Add(new KeyValuePair<int, int>(i, game.NumberOfTurns));
                AveragePlotPoints = CalculateAveragePlotPoints();
                Player winner = Players.First(p => p.Id == game.Winner.Id);
                winner.NumberOfWins++;
            }

            MaxPlotPoints = CalculateMaxPlotPoints();
            MinPlotPoints = CalculateMinPlotPoints();
            SetWinner();
        }

        private void SetWinner()
        {
            var winningScore = Players.Max(p => p.NumberOfWins);
            Player overallWinner = Players.First(p => p.NumberOfWins == winningScore);
            overallWinner.IsWinner = true;
        }
    }
}