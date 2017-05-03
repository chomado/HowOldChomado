using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HowOldChomado.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {

        private INavigationService NavigationService { get; }
        public DelegateCommand<string> NavigateCommand { get; }

        public MainPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.NavigateCommand = new DelegateCommand<string>(async x => await this.NavigationService.NavigateAsync(x));
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }
    }
}
