using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using HowOldChomado.BusinessObjects;
using HowOldChomado.Repositories;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HowOldChomado.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private static readonly PropertyChangedEventArgs ScoreDiffPropertyChangedEventArgs = new PropertyChangedEventArgs(propertyName: nameof(ScoreDiff));
        private INavigationService NavigationService { get; }
        private IPlayerRepository PlayerRepository { get; }
        private IScoreHistoryRepository ScoreHistoryRepository { get; }
        public DelegateCommand<string> NavigateCommand { get; }
        public DelegateCommand DeletePlayerCommand { get; }

        private IEnumerable<Player> players;

        public IEnumerable<Player> Players
        {
            get { return this.players; }
            set { this.SetProperty(storage: ref this.players, value: value); }
        }

        private Player selectedPlayer;

        public Player SelectedPlayer
        {
            get { return this.selectedPlayer; }
            set { this.SetProperty(ref this.selectedPlayer, value); this.UpdateMaxScoreAsync(); }
        }

        private ScoreHistory maxScore;

        public ScoreHistory MaxScore
        {
            get { return this.maxScore; }
            set { this.SetProperty(ref this.maxScore, value); this.OnPropertyChanged(ScoreDiffPropertyChangedEventArgs); }
        }

        public int ScoreDiff
        {
            get
            {
                if (this.SelectedPlayer == null)
                {
                    return 0;
                }

                if (this.MaxScore == null)
                {
                    return 0;
                }

                return this.MaxScore.Age - this.SelectedPlayer.Age;
            }
        }


        public MainPageViewModel(INavigationService navigationService, 
            IPlayerRepository playerRepository,
            IScoreHistoryRepository scoreHistoryRepository)
        {
            this.NavigationService = navigationService;
            this.PlayerRepository = playerRepository;
            this.ScoreHistoryRepository = scoreHistoryRepository;
            this.NavigateCommand = new DelegateCommand<string>(async x => await this.NavigationService.NavigateAsync(x));
            this.DeletePlayerCommand = new DelegateCommand(async () => await this.DeletePlayerExecuteAsync());
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            this.Players = await this.PlayerRepository.FindAllAsync();
            this.SelectedPlayer = this.Players.FirstOrDefault();
        }

        private async Task UpdateMaxScoreAsync()
        {
            if (this.SelectedPlayer == null)
            {
                this.MaxScore = null;
                return;
            }
            //////
            this.MaxScore = await this.ScoreHistoryRepository.FindMaxScoreHistoryByPlayerIdAsync(this.SelectedPlayer.Id);
        }

        private async Task DeletePlayerExecuteAsync()
        {
            if (this.SelectedPlayer == null)
            {
                return;
            }

            await this.PlayerRepository.DeleteAsync(this.SelectedPlayer);
            this.Players = await this.PlayerRepository.FindAllAsync();
            this.SelectedPlayer = this.Players.FirstOrDefault();
        }

    }
}
