using Secs;

namespace Ingame.Player
{
    public sealed class CollideBetweenPlayerAndGround : IEcsRunSystem
    {
        [EcsInject] private readonly EcsWorld _ecsWorld;
        [EcsInject(typeof(OnCollisionEnterReq))]
        private readonly EcsFilter _onCollisionEnterReqFilter;
        
        [EcsInject]
        private readonly EcsPool<OnCollisionEnterReq> _onCollisionEnterReqPool;
        public void OnRun()
        {
            foreach (var collisionEntity in _onCollisionEnterReqFilter)
            {
                ref var collisionReq = ref _onCollisionEnterReqPool.GetComponent(collisionEntity);
                
                if(!collisionReq.collider.CompareTag("Ground"))
                    return;
                
                if(!collisionReq.senderObject.root.TryGetComponent<EcsEntityReference>(out var ecsEntityReference))
                    return;
                
                if(ecsEntityReference.World.HasCmp<IsJumpBlockedTag>(ecsEntityReference.EntityId))
                    ecsEntityReference.World.DelCmp<IsJumpBlockedTag>(ecsEntityReference.EntityId);
            }
        }
    }
}