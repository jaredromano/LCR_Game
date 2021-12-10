using System.Windows;
using LCR_Game.Modules.GameModule;
using LCR_Game.Services;
using LCR_Game.Services.Interfaces;
using LCR_Game.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace LCR_Game
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILcrGame, LcrGame>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<GameModule>();
        }
    }
}
