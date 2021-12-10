using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using LCR_Game.Core.Mvvm;
using LCR_Game.Modules.GameModule.Models;
using LCR_Game.Services;
using Prism.Commands;
using Prism.Regions;

namespace LCR_Game.Modules.GameModule.ViewModels
{
    public class GameViewModel : RegionViewModelBase
    {
        private DelegateCommand _cancelCommand;
        private CancellationTokenSource _gameCancellationTokenSource;

        private int _gamesQuantity;
        private bool _isGamePlaying;
        private DelegateCommand _playCommandCommand;
        private int _playersQuantity;

        public GameViewModel(IRegionManager regionManager) :
            base(regionManager)
        {
        }

        private ObservableCollection<KeyValuePair<int, int>> _plotPoints;

        public ObservableCollection<KeyValuePair<int, int>> PlotPoints
        {
            get => _plotPoints;
            set => SetProperty(ref _plotPoints, value);
        }

        private Preset _selectedPreset;

        public Preset SelectedPreset
        {
            get => _selectedPreset;
            set
            {
                if (SetProperty(ref _selectedPreset, value))
                {
                    GamesQuantity = _selectedPreset.GamesQuantity;
                    PlayersQuantity = _selectedPreset.PlayersQuantity;
                };
            }
        }


        public IList<Preset> PresetsList { get; } = new List<Preset>()
        {
            new() { PlayersQuantity = 3, GamesQuantity = 100 },
            new() { PlayersQuantity = 3, GamesQuantity = 100 },
            new() { PlayersQuantity = 3, GamesQuantity = 100 },
            new() { PlayersQuantity = 3, GamesQuantity = 100 },
            new() { PlayersQuantity = 3, GamesQuantity = 100 },
            new() { PlayersQuantity = 3, GamesQuantity = 100 },
            new() { PlayersQuantity = 3, GamesQuantity = 100 },
            new() { PlayersQuantity = 3, GamesQuantity = 100 },
        };

        

        public DelegateCommand CancelCommand
            => _cancelCommand ??= new DelegateCommand(ExecuteCancel, CanExecuteCancel)
                .ObservesProperty(() => IsGamePlaying);

        public int GamesQuantity
        {
            get => _gamesQuantity;
            set => SetProperty(ref _gamesQuantity, value);
        }

        public bool IsGamePlaying
        {
            get => _isGamePlaying;
            set => SetProperty(ref _isGamePlaying, value);
        }

        public DelegateCommand PlayCommand
            => _playCommandCommand ??= new DelegateCommand(ExecutePlay, CanExecutePlay)
                .ObservesProperty(() => IsGamePlaying);

        public int PlayersQuantity
        {
            get => _playersQuantity;
            set => SetProperty(ref _playersQuantity, value);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }

        private bool CanExecuteCancel()
            => IsGamePlaying;

        private bool CanExecutePlay()
            => !IsGamePlaying;

        private void ExecuteCancel()
        {
            _gameCancellationTokenSource.Cancel();
        }

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

        private async Task PlayGames(int numberOfPlayers, int numberOfGames, CancellationToken ct)
        {
            for (var i = 0; i < numberOfGames; i++)
            {
                LcrGame game = new(numberOfPlayers);
                await game.Play(ct);
                PlotPoints.Add(new KeyValuePair<int, int>(i, game.NumberOfTurns));
            }
        }
    }
}