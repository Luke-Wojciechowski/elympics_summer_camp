using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Ingame
{
    public sealed class SceneService : MonoBehaviour,ISceneService
    {
        private const float DELAY_BEFORE_LOADING = 1.2f;
        private const float DELAY_BEFORE_FINISHING= 1.2f;

        public event Action OnLoadingStarted;
        public event Action<float> OnLoadingProgressChanged;
        public event Action OnLoadingFinished;

        private Coroutine _levelCoroutine;

        private IEnumerator LoadSceneCoroutine(int level)
        {
            OnLoadingStarted?.Invoke();

            yield return new WaitForSeconds(DELAY_BEFORE_LOADING);

            var loading = SceneManager.LoadSceneAsync(level);

            while (!loading.isDone)
            {
                OnLoadingProgressChanged?.Invoke(loading.progress);
                yield return null;
            }

            yield return new WaitForSeconds(DELAY_BEFORE_FINISHING);

            _levelCoroutine = null;
            OnLoadingFinished?.Invoke();
        }

        /// <summary>
        /// Loads level
        /// </summary>
        /// <param name="level">Level index that will be loaded</param>
        public void LoadLevel(int level)
        {
            if (level < 0 || level >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogWarning("Level can not be smaller than zero or bigger than number of scenes");
                return;
            }

            if(_levelCoroutine != null)
                return;

            _levelCoroutine = StartCoroutine(LoadSceneCoroutine(level));
        }
    }
}