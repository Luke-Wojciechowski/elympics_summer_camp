using Inagme;
using Ingame.Common;
using Secs;
using UnityEngine;
using Zenject;

namespace Ingame.Player
{
    public sealed class DashPlayerSys : IEcsRunSystem
    {
        [EcsInject(typeof(RigidbodyCmp), typeof(PlayerCmp))]
        [AndExclude(typeof(DashOnCoolDownCmp))]
        private readonly EcsFilter _playerFilter;
        
        [EcsInject] private readonly EcsPool<RigidbodyCmp> _rigidbodyPool;
        [EcsInject] private readonly EcsPool<DashOnCoolDownCmp> _dashPool;
        [EcsInject] private readonly EcsPool<ElympicsBehaviourMdl> _elympicsBehaviourMdlPool;
        [EcsInject] private readonly EcsPool<PlayerCmp> _playerCmpPool;
        
        private PlayerMovementConfig _playerMovementConfig;
        private ElympicsProvider _elympicsProvider;
        private float _forcePower;
        
        [Inject]
        private void Construct(PlayerMovementConfig playerMovementConfig, ElympicsProvider elympicsProvider)
        {
            _playerMovementConfig = playerMovementConfig;
            _elympicsProvider = elympicsProvider;
            _forcePower = playerMovementConfig.DashForce;
        }
        
        public void OnRun()
        {
            if(_elympicsProvider.blockInput.Value)
                return;
            
            foreach (var playerEntity in _playerFilter)
            {
                ref var rb = ref _rigidbodyPool.GetComponent(playerEntity).rigidbody;
                ref var player = ref _playerCmpPool.GetComponent(playerEntity);
   
                var movement = player.elympicsPlayerProvider.Movement;
                
                if(!movement.dash)
                    continue;
                
                var forwardDirection =  (movement.backward?-1:0) +(movement.forward?1:0);
                var rightDirection =  (movement.left?-1:0) +(movement.right?1:0); 
                var transform = rb.transform;
                
                _dashPool.AddComponent(playerEntity).timeLeft = _playerMovementConfig.DashCooldown;
                rb.AddForce((transform.forward*forwardDirection +transform.right*rightDirection + Vector3.up*_playerMovementConfig.YDirectionOnDash)*_forcePower, ForceMode.Impulse);
            }
        }
    }
}