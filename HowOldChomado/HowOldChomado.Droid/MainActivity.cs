using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Autofac;
using Prism.Autofac.Forms;
using Plugin.Permissions;
using HowOldChomado.Droid.Services;
using HowOldChomado.Services;
using ImageCircle.Forms.Plugin.Droid;

namespace HowOldChomado.Droid
{
    [Activity(Label = "HowOldChomado", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            ImageCircleRenderer.Init();
            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }


    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainer container)
        {
            // 各OSごとに作ったクラス(FileService)は、各OSごとにDIコンテナに登録しないといけなく.
            var builder = new ContainerBuilder();
            builder.RegisterType<FileService>()
                .As<IFileService>();
            builder.Update(container);
        }
    }
}

