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

namespace HowOldChomado
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override async void OnInitialized()
        {
            this.InitializeComponent();

            await this.NavigationService.NavigateAsync("NavigationPage/MainPage");
            await this.InializeDatabaseAsync();
        }

        // データベースにクラスの型にあわせたテーブルを作ってru
        private async Task InializeDatabaseAsync()
        {
            var fileService = this.Container.Resolve<IFileService>();
            var connection = new SQLiteAsyncConnection(databasePath: fileService.GetLocalFilePath(fileName: Consts.DatabaseFileName));
            await connection.CreateTableAsync<Player>(CreateFlags.ImplicitIndex);
            await connection.CreateTableAsync<ScoreHistory>(CreateFlags.ImplicitIndex);
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

            builder.Update(this.Container);

            this.Container.RegisterTypeForNavigation<NavigationPage>();
            this.Container.RegisterTypeForNavigation<MainPage>();
            this.Container.RegisterTypeForNavigation<RegisterPage>();
            this.Container.RegisterTypeForNavigation<GamePage>();
        }
    }
}
