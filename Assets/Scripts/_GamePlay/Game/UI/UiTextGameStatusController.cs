using System;
using System.Collections;
using System.Collections.Generic;
using Ingame.Player;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Zenject;

namespace Ingame.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class UiTextGameStatusController : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 3.2f;

        [Inject] private ElympicsProvider _elympicsProvider;
        private TMP_Text _text;
        private Coroutine _coroutine;

        public bool IsRunning => _coroutine != null;
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _elympicsProvider.blockInput.ValueChanged += OnBlockInputChanged;
        }

        private void OnDestroy()
        {
            _elympicsProvider.blockInput.ValueChanged -= OnBlockInputChanged;
        }

        private void OnBlockInputChanged(bool oldValue, bool newValue)
        {
            if (newValue)
                return;
                
            SetText("Fight");
        }
        
        private IEnumerator ChangeTextRoutine(string text)
        {
            _text.text = text;
            
            yield return new WaitForSeconds(lifeTime);
            
            _text.text = "";
            _coroutine = null;
        }
        
        public void SetText(string text)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
           
            _coroutine = StartCoroutine(ChangeTextRoutine(text));
        }
    }
}