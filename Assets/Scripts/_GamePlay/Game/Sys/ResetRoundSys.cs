using System;
using Ingame.Common;
using Ingame.Player;
using Secs;
using UnityEngine;

namespace Ingame
{
    public sealed class ResetRoundSys : IEcsReactiveSystem
    {
        [EcsInject] private readonly EcsWorld _ecsWorld;

        [EcsInject(typeof(TransformCmp),typeof(BoneTag))] private readonly EcsFilter _boneFilter;
        [EcsInject] private readonly EcsPool<TransformCmp> _transformPool;
 
        public EcsFilter ObserveFilter(in EcsWorld ecsWorld)
        {
            return null;
        }

        public Type ObserveOnType()
        {
            return typeof(FinishRoundEvt);
        }

        public void OnExecute(in int entityId)
        {
            foreach (var boneEntity in _boneFilter)
            {
                ref var transformCmp = ref _transformPool.GetComponent(boneEntity);
                transformCmp.transform.position = transformCmp.initPosition;
            }
            
            _ecsWorld.DelEntity(entityId);
        }

        public IEcsReactiveSystem.ComponentReactiveState ObserveOnState()
        {
            return IEcsReactiveSystem.ComponentReactiveState.ComponentAdded;
        }
    }
}