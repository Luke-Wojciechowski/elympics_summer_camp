using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Zenject;

namespace Ingame
{
    public sealed class AudioService : MonoBehaviour
    {
        private AudioConfig _audioConfig;
        private ObjectPool<AudioSource> _audioPool;
        private Dictionary<string, AudioWrapper> _stringToAudioWrapperDictionary;

        [Inject]
        private void Construct(AudioConfig audioConfig)
        {
            _audioConfig = audioConfig;
        }
        
        private void Awake()
        {
            _stringToAudioWrapperDictionary = _audioConfig.Audios.ToDictionary(e => e.AudioName, e => e);
            
            _audioPool = new ObjectPool<AudioSource>(
                OnAudioClipCreate,
                OnAudioClipGet,
                OnAudioClipRelease,
                OnAudioClipDestroy
                );
        }
        
        private AudioSource OnAudioClipCreate()
        {
            var audioGameObject = new GameObject("audio");
            audioGameObject.transform.SetParent(transform);
            
            return audioGameObject.AddComponent<AudioSource>();
        }
        
        private void OnAudioClipGet(AudioSource audioSource)
        {
            audioSource.gameObject.SetActive(true);
        }

        private void OnAudioClipRelease(AudioSource audioSource)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.gameObject.SetActive(false);
        }
        
        private void OnAudioClipDestroy(AudioSource audioSource)
        {
           
        }
        
        private IEnumerator PlaySoundThenReturnToPoolRoutine(AudioSource audioSource)
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);

            StopSound(audioSource);
        }

        private void SetupAudioSource(AudioSource audioSource, AudioWrapper audioWrapper)
        {
            var audioSettings = audioWrapper.AudioSettings;
            var audioClip = audioWrapper.AudioClip;
    
            audioSource.clip = audioClip;
            audioSource.priority = audioSettings.Priority;
            audioSource.volume = audioSettings.Volume;
            audioSource.pitch = audioSettings.Pitch;
            audioSource.spatialBlend = audioSettings.SpatialBlend;
            audioSource.loop = audioSettings.Loop;
            audioSource.dopplerLevel = audioSettings.DopplerLevel;
            audioSource.spread = audioSettings.Spread;
            audioSource.minDistance = audioSettings.MinDistance;
            audioSource.maxDistance = audioSettings.MaxDistance;
        }
        
        public AudioSource PlaySound(string clipName)
        {
            var audioSource = _audioPool.Get();
         
            SetupAudioSource(audioSource, _stringToAudioWrapperDictionary[clipName]);
            audioSource.Play();
            
            StartCoroutine(PlaySoundThenReturnToPoolRoutine(audioSource));
            
            return audioSource;
        }
        
        public void StopSound(AudioSource audioSource)
        {
            audioSource.Stop();
            _audioPool.Release(audioSource);
        }
        
    }
}