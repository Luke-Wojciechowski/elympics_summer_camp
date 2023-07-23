using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Ingame
{
	public sealed class LoadingScreen : IDisposable
	{
		private readonly ISceneService _sceneService;
		private readonly UiLoadingScreen _loadingScreenPrefab;
		private readonly DiContainer _diContainer;
		
		private UiLoadingScreen _uiLoadingScreen;

		[Inject]
		public LoadingScreen(ISceneService sceneService, UiLoadingScreen loadingScreen, DiContainer diContainer)
		{
			_sceneService = sceneService;
			_loadingScreenPrefab = loadingScreen;
			_diContainer = diContainer;

			_sceneService.OnLoadingStarted += OnLoadingStarted;
			_sceneService.OnLoadingFinished += OnLoadingFinished;
		}

		public void Dispose()
		{
			_sceneService.OnLoadingStarted -= OnLoadingStarted;
			_sceneService.OnLoadingFinished -= OnLoadingFinished;
		}

		private void OnLoadingStarted()
		{
			_uiLoadingScreen ??= Object.Instantiate(_loadingScreenPrefab, null, true);
			_diContainer.InstantiatePrefabForComponent<UiLoadingScreen>(_uiLoadingScreen);

			Object.DontDestroyOnLoad(_uiLoadingScreen.gameObject);
			
			_uiLoadingScreen.Show();
		}

		private void OnLoadingFinished()
		{
			_uiLoadingScreen.Hide();
			_uiLoadingScreen = null;
		}
	}
}