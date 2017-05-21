using HowOldChomado.BusinessObjects;
using HowOldChomado.Repositories;
using HowOldChomado.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HowOldChomado.ViewModels
{
    public class GamePageViewModel : BindableBase, INavigationAware
    {
        private INavigationService NavigationService { get; }
        private ICameraService CameraService { get; }
        private IFaceService FaceService { get; }
        private IPlayerRepository PlayerRepository { get; }
        private IScoreHistoryRepository ScoreHistoryRepository { get; }

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
            this.ScoreHistoryRepository = scoreHistoryRepository;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            var picture = await this.CameraService.TakePhotosAsync();
            if (picture == null)
            {
                await this.NavigationService.GoBackAsync();
                return;
            }

            this.Picture = picture;
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
                    await this.ScoreHistoryRepository.AddAsync(new ScoreHistory
                    {
                        PlayerId = vm.Player.Id,
                        Age = vm.FaceDetectionResult.Age,
                        Date = DateTime.Now,
                    });
                }

                this.FaceDetectionResults = l;
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
