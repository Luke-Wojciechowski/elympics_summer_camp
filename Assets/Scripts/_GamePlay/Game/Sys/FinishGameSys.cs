using System;
using Ingame.DiInstallers;
using Secs;
using Zenject;

namespace Ingame
{
    public sealed class FinishGameSys : IEcsReactiveSystem
    {
        [EcsInject] private readonly EcsWorld _ecsWorld;   
        
        private ISceneService _sceneService;
        private UiLoadingScreen _uiLoadingScreen;
        
        [Inject] 
        private void Construct( ISceneService sceneService,UiLoadingScreen uiLoadingScreen)
        {
            _uiLoadingScreen = uiLoadingScreen;
            _sceneService = sceneService;
        }
        
        public EcsFilter ObserveFilter(in EcsWorld ecsWorld)
        {
            return null;
        }

        public Type ObserveOnType()
        {
            return typeof(FinishGameEvt);
        }

        public void OnExecute(in int entityId)
        {
            _sceneService.LoadLevel(0);
            _ecsWorld.DelEntity(entityId);
        }

        public IEcsReactiveSystem.ComponentReactiveState ObserveOnState()
        {
            return IEcsReactiveSystem.ComponentReactiveState.ComponentAdded;
        }
    }
}