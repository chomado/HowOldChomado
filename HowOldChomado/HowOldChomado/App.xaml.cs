using Autofac;
using Prism.Autofac;
using Prism.Autofac.Forms;
using HowOldChomado.Views;
using Xamarin.Forms;
using HowOldChomado.Services;

namespace HowOldChomado
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override async void OnInitialized()
        {
            this.InitializeComponent();

            await this.NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes()
        {
            // 写真を取る処理に必要
            var builder = new ContainerBuilder();
            builder.RegisterType<CameraService>()
                .As<ICameraService>();
            builder.Update(this.Container);

            this.Container.RegisterTypeForNavigation<NavigationPage>();
            this.Container.RegisterTypeForNavigation<MainPage>();
            this.Container.RegisterTypeForNavigation<RegisterPage>();
        }
    }
}
