using System;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public sealed class MusicInitializer : MonoBehaviour
    {
        private AudioService _audioService;

        [Inject]
        private void Construct(AudioServiceProvider audioServiceProvider)
        {
            _audioService = audioServiceProvider.globalAudio;
        }
        
        private void Awake()
        {
            _audioService.PlaySound("MUSIC");
        }
    }
}