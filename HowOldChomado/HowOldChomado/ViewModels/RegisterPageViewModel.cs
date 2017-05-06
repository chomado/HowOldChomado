using HowOldChomado.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowOldChomado.ViewModels
{
    public class RegisterPageViewModel : BindableBase
    {
        private ICameraService CameraService { get; }
        public DelegateCommand TakePhotoCommand { get; }

        private byte[] image;

        public byte[] Image
        {
            get { return this.image; }
            set { this.SetProperty(storage: ref this.image, value: value); }
        }

        public RegisterPageViewModel(ICameraService cameraService)
        {
            this.CameraService = cameraService;
            this.TakePhotoCommand = new DelegateCommand(async () => await this.TakePhotoAsync());
        }

        private async Task TakePhotoAsync()
        {
            this.Image = await this.CameraService.TakePhotosAsync();
        }
    }
}
