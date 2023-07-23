using System;
using Elympics;
using Ingame.DiInstallers;
using Ingame.Player;
using Secs;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace Ingame.Gameplay
{
	public sealed class EcsRuntime : IInitializable, ITickable, IDisposable 
	{
		private readonly DiContainer _diContainer;
		
		private readonly EcsWorld _world;

		private readonly EcsSystems _systems;
		private readonly EcsSystems _elympicSystems;
		private readonly ElympicsProvider _elympicsProvider;
		
		public EcsRuntime(
			DiContainer diContainer, 
			GameplaySceneInstaller.EcsWorldProvider ecsWorldProvider, 
			ElympicsProvider elympicsProvider)
		{
			_world = ecsWorldProvider.ecsWorld;

			_diContainer = diContainer;
			
			_systems = new EcsSystems(_world);
			_elympicSystems = new EcsSystems(_world);
			_elympicsProvider = elympicsProvider;
		}
		
		private void InitializeSystems()
		{
			_elympicsProvider.TickableAction = _elympicSystems.FireRunSystems;
			
			_systems
				.Add(new CollideBetweenPlayerAndGround())
				.Add(new CooldownDashAction())
				.Add(new DisposePhysicsRequestsSys(_world))
				.Add(new ResetRoundSys())
				.Add(new FinishGameSys())
				/*.Add(new WinRoundSystem())
				.Add(new UpdateRoundViewSystem())*/;

			_elympicSystems
				.Add(new MovePlayerSys())
				.Add(new DashPlayerSys())
				.Add(new PerformJumpPlayerSys());
			
#if UNITY_EDITOR
			_systems.AttachProfiler();
			_elympicSystems.AttachProfiler();
#endif
			
			_systems.Inject(_diContainer);
			_elympicSystems.Inject(_diContainer);
			
			_world.BakeAllBakersInScene();
			_world.BindPhysics();
		}
		
		public void Initialize()
		{
			InitializeSystems();
			
			_systems.FireInitSystems();
			_elympicSystems.FireInitSystems();
		}

		public void Tick()
		{
			_systems.FireRunSystems();
		}

		public void Dispose()
		{
#if UNITY_EDITOR
			_systems.ReleaseProfiler();
			_elympicSystems.ReleaseProfiler();
#endif
			
			_systems.FireDisposeSystems();
			_elympicSystems.FireDisposeSystems();
			
			_world.UnbindPhysics();
		}
	}
}