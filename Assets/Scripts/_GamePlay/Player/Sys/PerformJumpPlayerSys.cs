using Inagme;
using Ingame.Common;
using Secs;
using UnityEngine;
using Zenject;

namespace Ingame.Player
{
    public sealed class PerformJumpPlayerSys : IEcsRunSystem
    {
        [EcsInject(typeof(RigidbodyCmp), typeof(PlayerCmp))]
        [AndExclude(typeof(IsJumpBlockedTag))]
        private readonly EcsFilter _playerFilter;
        
        [EcsInject] private readonly EcsPool<RigidbodyCmp> _rigidbodyPool;
        [EcsInject] private readonly EcsPool<IsJumpBlockedTag> _dashPool;
        [EcsInject] private readonly EcsPool<ElympicsBehaviourMdl> _elympicsBehaviourMdlPool;
        [EcsInject] private readonly EcsPool<PlayerCmp> _playerCmpPool;
        
        private PlayerMovementConfig _playerMovementConfig;
        private float _forcePower;
        private ElympicsProvider _elympicsProvider;
        
        [Inject]
        private void Construct(PlayerMovementConfig playerMovementConfig, ElympicsProvider elympicsProvider)
        {
            _playerMovementConfig = playerMovementConfig;
            _elympicsProvider = elympicsProvider;
            _forcePower = playerMovementConfig.JumpForce;
        }
        
        public void OnRun()
        {
            if(_elympicsProvider.blockInput.Value)
                return;
            
            foreach (var playerEntity in _playerFilter)
            {
                ref var elympicsBehaviourMdl = ref _elympicsBehaviourMdlPool.GetComponent(playerEntity);
                ref var playerCmp = ref _playerCmpPool.GetComponent(playerEntity);
                ref var rb = ref _rigidbodyPool.GetComponent(playerEntity).rigidbody;
                
                if(!playerCmp.elympicsPlayerProvider.Movement.jump)
                    continue;
                
                _dashPool.AddComponent(playerEntity);
                rb.AddForce(Vector3.up*_forcePower, ForceMode.Impulse);
            }
        }
    }
}