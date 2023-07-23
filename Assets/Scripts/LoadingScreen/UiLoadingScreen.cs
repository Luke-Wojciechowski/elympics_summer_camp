using System;
using System.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame
{
	public sealed class UiLoadingScreen : MonoBehaviour
	{
		[BoxGroup("References")]
		[Required, SerializeField] private CanvasGroup parentCanvasGroup;
		
		[BoxGroup("Animation")]
		[SerializeField] [Min(0f)] private float fadeDuration = 1f;
		
		private void Awake()
		{
			gameObject.SetActive(false);
		}

		public void Show()
		{
			gameObject.SetActive(true);
			parentCanvasGroup.alpha = 0f;
			parentCanvasGroup.DOFade(1, fadeDuration);
		}

		public void Hide()
		{
			 parentCanvasGroup
				.DOFade(0f, fadeDuration)
				.OnComplete(()=>gameObject.SetActive(false));
		}
	}
}