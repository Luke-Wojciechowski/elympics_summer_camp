using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Ingame
{
	public interface ISceneService
	{
		public event Action OnLoadingStarted;
		public event Action<float> OnLoadingProgressChanged;
		public event Action OnLoadingFinished;

		public void LoadLevel(int level);
	}
}