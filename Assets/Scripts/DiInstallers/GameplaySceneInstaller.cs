using Ingame.Gameplay;
using Ingame.Player;
using NaughtyAttributes;
using Secs;
using UnityEngine;
using Zenject;

namespace Ingame.DiInstallers
{
	public sealed class GameplaySceneInstaller : MonoInstaller
	{
		[Required, SerializeReference] private AudioService audioService;
		[Required, SerializeReference] private PlayerMovementConfig playerMovementConfig;
		[Required, SerializeReference] private ElympicsProvider elympicsProvider;

		[Inject]
		private void Construct(AudioServiceProvider audioServiceProvider)
		{
			audioServiceProvider.localAudio = audioService;
		}
		
		public override void InstallBindings()
		{
			InstallEcs();
			InstallConfigs();
		}

		private void InstallEcs()
		{
			Container
				.Bind<ElympicsProvider>()
				.FromInstance(elympicsProvider)
				.AsSingle()
				.NonLazy();
			
			Container
				.BindInstance(
					new EcsWorldProvider
					{
						ecsWorld = new EcsWorld("gameplay")
					})
				.AsSingle();
			
			Container
				.BindInterfacesAndSelfTo<EcsRuntime>()
				.AsSingle();
		}

		private void InstallConfigs()
		{
			Container
				.BindInstance(playerMovementConfig)
				.AsSingle();
		}
		
		public sealed class EcsWorldProvider
		{
			public EcsWorld ecsWorld;
		}
	}
}