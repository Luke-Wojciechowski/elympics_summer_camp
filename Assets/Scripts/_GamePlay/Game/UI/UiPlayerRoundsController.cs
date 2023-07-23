using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
    public sealed class UiPlayerRoundsController : MonoBehaviour
    {
        [SerializeReference] private List<Image> _roundsImages;
        [SerializeField] private Color color = Color.white;

        private int _lastImageIndex;

        private void Awake()
        {
            _lastImageIndex = 0;
        }

        public void AddPoint(int value)
        {
            if (value - 1 != _lastImageIndex)
            {
                value = value >= _roundsImages.Count ? _roundsImages.Count-1 : value;
                ResetPoints();
                for (int i = 0; i < value; i++)
                    _roundsImages[i].color = new Color(color.r, color.g, color.b);
                
                _lastImageIndex = value - 1;
                return;
            }
            
            if(_lastImageIndex >= _roundsImages.Count)
                return;

            _roundsImages[_lastImageIndex++].color = new Color(color.r, color.g, color.b);
        }

        public void ResetPoints()
        {
            foreach (var round in _roundsImages)
                round.color = Color.white;
            
            _lastImageIndex = 0;
        }
    }
}