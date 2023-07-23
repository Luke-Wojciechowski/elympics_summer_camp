using Inagme;
using Ingame.Gameplay;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.DiInstallers
{
	public sealed class ProjectContextInstaller : MonoInstaller
	{
		[BoxGroup("Configs")]
		[Required, SerializeField] private SettingsConfig settingsConfig;
		[BoxGroup("Configs")]
		[Required, SerializeField] private SecsConfig secsConfig;
		[BoxGroup("Configs")]
		[Required, SerializeField] private AudioConfig audioConfig;
		
		[Space]
		[BoxGroup("Services")]
		[Required, SerializeField] private SceneService sceneService;
		[BoxGroup("Services")]
		[Required, SerializeReference] private AudioService audioService;
		[BoxGroup("Services")]
		[Required, SerializeReference] private UiLoadingScreen uiLoadingScreen;
		public override void InstallBindings()
		{
			InstallConfigs();
			InstallSettings();
			InstallInput();
			
			InstallSceneService();
			
			InstallLoadingScreen();
			InstallGlobalAudio();

			sceneService.OnLoadingStarted += uiLoadingScreen.Show;
			sceneService.OnLoadingFinished += uiLoadingScreen.Hide;
		}

		private void InstallConfigs()
		{
			
			Container
				.BindInstance(settingsConfig)
				.AsSingle();

			Container
				.BindInstance(secsConfig)
				.AsSingle();

			Container
				.BindInstance(audioConfig)
				.AsSingle();
			
		}
		
		private void InstallSettings()
		{
			Container
				.BindInterfacesAndSelfTo<SettingsService>()
				.AsSingle();
		}

		private void InstallInput()
		{
			Container
				.BindInterfacesAndSelfTo<InputService>()
				.AsSingle();
		}

		private void InstallSceneService()
		{
			Container
				.Bind<ISceneService>()
				.FromInstance(sceneService)
				.AsSingle()
				.NonLazy();
		}

		private void InstallLoadingScreen()
		{
			Container
				.Bind<UiLoadingScreen>()
				.FromInstance(uiLoadingScreen)
				.AsSingle()
				.NonLazy();
			
			Container
				.BindInterfacesAndSelfTo<LoadingScreen>()
				.AsSingle();
		}
		
		private void InstallGlobalAudio()
		{
			var audioServiceProvider = new AudioServiceProvider();
			audioServiceProvider.globalAudio = audioService;
			
			Container
				.Bind<AudioServiceProvider>()
				.FromInstance(audioServiceProvider)
				.AsSingle()
				.NonLazy();
			
		}
	}
}