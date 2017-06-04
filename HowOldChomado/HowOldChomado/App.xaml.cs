using System.Threading.Tasks;
using Autofac;
using Prism.Autofac;
using Prism.Autofac.Forms;
using Xamarin.Forms;

using HowOldChomado.Views;
using HowOldChomado.Services;
using HowOldChomado.Repositories;
using HowOldChomado.BusinessObjects;

using SQLite;
using System;

namespace HowOldChomado
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) 
        {
            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
			{
				var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
				Resx.AppResources.Culture = ci; // set the RESX for resource localization
				DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
			}
        }

        protected override async void OnInitialized()
        {
            this.InitializeComponent();

            await this.NavigationService.NavigateAsync("SplashPage");
            await this.InializeDatabaseAsync();
            await this.NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        // データベースにクラスの型にあわせたテーブルを作ってru
        private async Task InializeDatabaseAsync()
        {
            var fileService = this.Container.Resolve<IFileService>();
            var connection = new SQLiteAsyncConnection(databasePath: fileService.GetLocalFilePath(fileName: Consts.DatabaseFileName));
            await connection.CreateTableAsync<Player>(CreateFlags.ImplicitIndex);
            await connection.CreateTableAsync<ScoreHistory>(CreateFlags.ImplicitIndex);
            await connection.CreateTableAsync<PersonListId>(CreateFlags.ImplicitIndex);
            if (await connection.Table<PersonListId>().FirstOrDefaultAsync() == null)
            {
                await connection.InsertAsync(new PersonListId { Id = Guid.NewGuid().ToString() });
            }
        }

        protected override void RegisterTypes()
        {
            var builder = new ContainerBuilder();

            // 写真を取る処理に必要
            builder.RegisterType<CameraService>()
                .As<ICameraService>();

            builder.RegisterType<FaceService>()
                .As<IFaceService>();

            builder.RegisterType<PlayerRepository>()
                .As<IPlayerRepository>();

            builder.RegisterType<ScoreHistoryRepository>()
                .As<IScoreHistoryRepository>();

            builder.RegisterType<PersonListIdRepository>()
                .As<IPersonListIdRepository>();

            builder.Update(this.Container);

            this.Container.RegisterTypeForNavigation<NavigationPage>();
            this.Container.RegisterTypeForNavigation<MainPage>();
            this.Container.RegisterTypeForNavigation<RegisterPage>();
            this.Container.RegisterTypeForNavigation<GamePage>();
            this.Container.RegisterTypeForNavigation<SplashPage>();
        }
    }
}
