using Secs;
using UnityEngine;

namespace Ingame.Player
{
    public sealed class CooldownDashAction : IEcsRunSystem
    {
        [EcsInject] private EcsWorld _ecsWorld;
        [EcsInject(typeof(DashOnCoolDownCmp))]
        private readonly EcsFilter _dashOnCoolDownCmpFilter;
        
        [EcsInject] private readonly EcsPool<DashOnCoolDownCmp> _dashOnCoolDownCmpPool;
        public void OnRun()
        {
            foreach (var dashEntity in _dashOnCoolDownCmpFilter)
            {
                ref var dashOnCoolDownCmp = ref _dashOnCoolDownCmpPool.GetComponent(dashEntity);

                dashOnCoolDownCmp.timeLeft -= Time.deltaTime;
                
                if(dashOnCoolDownCmp.timeLeft <=0)
                    _ecsWorld.DelCmp<DashOnCoolDownCmp>(dashEntity);
            }
        }
    }
}