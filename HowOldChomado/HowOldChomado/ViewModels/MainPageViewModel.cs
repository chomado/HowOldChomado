using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using HowOldChomado.BusinessObjects;
using HowOldChomado.Repositories;

namespace HowOldChomado.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService NavigationService { get; }
        private IPlayerRepository PlayerRepository { get; }
        public DelegateCommand<string> NavigateCommand { get; }

        private IEnumerable<Player> players;

        public IEnumerable<Player> Players
        {
            get { return this.players; }
            set { this.SetProperty(storage: ref this.players, value: value); }
        }

        public MainPageViewModel(INavigationService navigationService, IPlayerRepository playerRepository)
        {
            this.NavigationService = navigationService;
            this.PlayerRepository = playerRepository;
            this.NavigateCommand = new DelegateCommand<string>(async x => await this.NavigationService.NavigateAsync(x));
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            this.Players = await this.PlayerRepository.FindAllAsync();
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }
    }
}
