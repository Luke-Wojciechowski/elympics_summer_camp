
using Ingame.Common;
using Secs;
using Zenject;

namespace Ingame.Player
{
    public sealed class MovePlayerSys : IEcsRunSystem
    {
        [EcsInject(typeof(RigidbodyCmp),typeof(TransformCmp), typeof(PlayerCmp),typeof(ElympicsBehaviourMdl))] 
        private readonly EcsFilter _playerFilter;
        
        [EcsInject] private readonly EcsPool<RigidbodyCmp> _rigidbodyPool;
        [EcsInject] private readonly EcsPool<TransformCmp> _transformPool;
        [EcsInject] private readonly EcsPool<PlayerCmp> _playerPool;
        [EcsInject] private readonly EcsPool<ElympicsBehaviourMdl> _elympicsBehaviourMdlPool;
        
        private float _speed;
        private ElympicsProvider _elympicsProvider;
        
        [Inject]
        private void Construct(PlayerMovementConfig playerMovementConfig, ElympicsProvider elympicsProvider)
        {
            _speed = playerMovementConfig.Speed;
            _elympicsProvider = elympicsProvider;
        }
        
        public void OnRun()
        {
            if(_elympicsProvider.blockInput.Value)
                return;
            
            foreach (var playerEntity in _playerFilter)
            {
                ref var rb = ref _rigidbodyPool.GetComponent(playerEntity).rigidbody;
                ref var player = ref _playerPool.GetComponent(playerEntity);
                
                if(player.elympicsPlayerProvider.Movement.forward)
                    rb.AddForce(rb.transform.forward*_speed);
                
                if(player.elympicsPlayerProvider.Movement.left)
                    rb.AddForce(-rb.transform.right*_speed);

                if(player.elympicsPlayerProvider.Movement.right)
                    rb.AddForce(rb.transform.right*_speed);

                if(player.elympicsPlayerProvider.Movement.backward)
                    rb.AddForce(-rb.transform.forward*_speed);
            }
        }
    }
}