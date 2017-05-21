using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Autofac;
using Prism.Autofac.Forms;
using HowOldChomado.UWP.Services;
using HowOldChomado.Services;
using ImageCircle.Forms.Plugin.UWP;

namespace HowOldChomado.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            ImageCircleRenderer.Init();
            LoadApplication(new HowOldChomado.App(new UwpInitializer()));
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainer container)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FileService>()
                .As<IFileService>();
            builder.Update(container);
        }
    }

}
