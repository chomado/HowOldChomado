using HowOldChomado.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HowOldChomado.ViewModels
{
    public class RegisterPageViewModel : BindableBase
    {
        private static readonly PropertyChangedEventArgs IsValidInputPropertyChangedEventArgs = new PropertyChangedEventArgs(propertyName: nameof(IsValidInput));
        private ICameraService CameraService { get; }

        public DelegateCommand TakePhotoCommand { get; }
        public DelegateCommand RegisterCommand { get; }

        private byte[] image;

        public byte[] Image
        {
            get { return this.image; }
            set { this.SetProperty(storage: ref this.image, value: value); }
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.SetProperty(storage: ref this.name, value: value);
                this.OnPropertyChanged(IsValidInputPropertyChangedEventArgs);
            }
        }

        private string age;
        public string Age
        {
            get { return this.age; }
            set
            {
                this.SetProperty(storage: ref this.age, value: value);
                this.OnPropertyChanged(IsValidInputPropertyChangedEventArgs);
            }
        }

        public bool IsValidInput
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Name))
                {
                    return false;
                }
            }
        }

        public RegisterPageViewModel(ICameraService cameraService)
        {
            this.CameraService = cameraService;
            this.TakePhotoCommand = new DelegateCommand(async () => await this.TakePhotoAsync());
            this.RegisterCommand = new DelegateCommand(async () => await this.RegisterAsync())
                .ObservesCanExecute(canExecuteExpression: () => this.IsValidInput);
        }

        private async Task RegisterAsync()
        {
        }

        private async Task TakePhotoAsync()
        {
            this.Image = await this.CameraService.TakePhotosAsync();
        }
    }
}
