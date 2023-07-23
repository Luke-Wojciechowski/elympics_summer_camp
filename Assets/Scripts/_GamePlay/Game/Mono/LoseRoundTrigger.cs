using System;
using Elympics;
using Ingame.DiInstallers;
using Ingame.Player;
using UnityEngine;
using Zenject;

namespace Ingame
{
    public sealed class LoseRoundTrigger : ElympicsMonoBehaviour
    {
        [SerializeField] private float cooldown;
        
        private ElympicsProvider _elympicsProvider;
        private GameplaySceneInstaller.EcsWorldProvider _ecsWorldProvider;
        private float _timer;
        
        [Inject]
        private void Construct(ElympicsProvider elympicsProvider,  GameplaySceneInstaller.EcsWorldProvider ecsWorldProvider)
        {
            _elympicsProvider = elympicsProvider;
            _ecsWorldProvider = ecsWorldProvider;
        }
        
        private void Update()
        {
            _timer -= Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!Elympics.IsServer)
                return;

            var body = other.transform.root;
            
            if(!body.TryGetComponent<PlayerBaker>(out var player))
                return;
            
            if(_timer > 0 )
                return;

            _timer = cooldown;

            switch (player.IndexId)
            {
                case 0:
                    _elympicsProvider.playerBWins.Value += 1;
                    break;
                case 1:
                    _elympicsProvider.playerAWins.Value += 1;
                    break;
            }

            _ecsWorldProvider.ecsWorld.NewEntityWithCmp<FinishRoundEvt>(out _);
        }
    }
}