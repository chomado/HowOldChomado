using HowOldChomado.BusinessObjects;
using HowOldChomado.Repositories;
using HowOldChomado.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowOldChomado.ViewModels
{
    public class GamePageViewModel : BindableBase, INavigationAware
    {
        private INavigationService NavigationService { get; }
        private ICameraService CameraService { get; }
        private IFaceService FaceService { get; }
        private IPlayerRepository PlayerRepository { get; }
        private IScoreHistoryRepository ScoreHisotryRepository { get; }

        public DelegateCommand StartGameCommand { get; }

        private byte[] picture;

        public byte[] Picture
        {
            get { return this.picture; }
            set { this.SetProperty(ref this.picture, value); }
        }

        private IEnumerable<FaceDetectionResultViewModel> faceDetectionResults;

        public IEnumerable<FaceDetectionResultViewModel> FaceDetectionResults
        {
            get { return this.faceDetectionResults; }
            set { this.SetProperty(ref this.faceDetectionResults, value); }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.SetProperty(ref this.isBusy, value); }
        }

        public GamePageViewModel(INavigationService navigationService,
            ICameraService cameraService,
            IFaceService faceService,
            IPlayerRepository playerRepository,
            IScoreHistoryRepository scoreHistoryRepository)
        {
            this.NavigationService = navigationService;
            this.CameraService = cameraService;
            this.FaceService = faceService;
            this.PlayerRepository = playerRepository;
            this.ScoreHisotryRepository = scoreHistoryRepository;

            this.StartGameCommand = new DelegateCommand(async () => await this.StartGameAsync());
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            await this.StartGameAsync();
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        private async Task StartGameAsync()
        {
            this.IsBusy = true;
            try
            {
                this.Picture = await this.CameraService.TakePhotosAsync();
                if (this.Picture == null)
                {
                    await this.NavigationService.GoBackAsync();
                    return;
                }

                var detectResults = await this.DetectPictureAsync();

                if (detectResults.Any())
                {
                    this.DetectWinner(detectResults);
                }

                this.FaceDetectionResults = detectResults;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private void DetectWinner(List<FaceDetectionResultViewModel> detectResults)
        {
            var winnerDiff = detectResults.Min(x => x.Diff);
            foreach (var player in detectResults.Where(x => x.Diff == winnerDiff))
            {
                player.IsWinner = true;
            }
        }

        private async Task<List<FaceDetectionResultViewModel>> DetectPictureAsync()
        {
            var results = await this.FaceService.DetectFacesAsync(new ImageRequest { Image = this.Picture });
            var l = new List<FaceDetectionResultViewModel>();
            foreach (var r in results)
            {
                var vm = new FaceDetectionResultViewModel
                {
                    FaceDetectionResult = r,
                    Player = await this.PlayerRepository.FindByFaceIdAsync(r.FaceId),
                };
                l.Add(vm);

                if (vm.Player != null)
                {
                    await this.ScoreHisotryRepository.AddAsync(new ScoreHistory
                    {
                        PlayerId = vm.Player.Id,
                        Age = vm.FaceDetectionResult.Age,
                        Date = DateTime.Now,
                    });
                }
            }

            return l;
        }
    }
}
