using LCR_Game.Core;
using LCR_Game.Modules.GameModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace LCR_Game.Modules.GameModule
{
    public class GameModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public GameModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, "GameView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<GameView>();
        }
    }
}