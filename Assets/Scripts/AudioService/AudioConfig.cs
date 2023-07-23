using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Ingame
{
    [CreateAssetMenu(menuName = "Configs/Audio", fileName = "AudioConfig")]
    public sealed class AudioConfig : ScriptableObject
    {
        [SerializeField] private List<AudioWrapper> audios;

        public IReadOnlyCollection<AudioWrapper> Audios => audios;
    }

    [Serializable]
    public sealed class AudioWrapper
    {
        [SerializeField] private string audioName;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private AudioWrapperSettings audioSettings;
        
        public AudioClip AudioClip => audioClip;
        public string AudioName => audioName;

        public AudioWrapperSettings AudioSettings => audioSettings;
    }
    
    [Serializable]
    public sealed class AudioWrapperSettings
    {
        [Header("Basic Settings")]
        [SerializeField]
        [Range(0, 256)]
        private int priority;
        
        [SerializeField]
        [Range(0, 1)]
        private float volume;
        
        [SerializeField]
        [Range(0, 3)]
        private float pitch;
        
        [SerializeField]
        [Range(0, 1)]
        private float spatialBlend;
        
        [SerializeField]
        private bool loop;

        [Space(5)]
        [Header("Advanced Settings")]
        [SerializeField]
        [Range(0, 5)]
        private float dopplerLevel = 0;
        
        [SerializeField]
        [Range(0, 360)]
        private float spread = 0;
        
        [SerializeField]
        [Min(0)]
        private float minDistance = 0;
        
        [SerializeField]
        [Min(0.01f)]
        private float maxDistance = 0.01f;
        
        public float Volume => volume;
        public int Priority => priority;
        public float SpatialBlend => spatialBlend;
        public float Pitch => pitch;
        public bool Loop => loop;
        public float DopplerLevel => dopplerLevel;
        public float Spread => spread;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
    }
}