using Elympics;
using Ingame.Common;
using NaughtyAttributes;
using Secs;
using UnityEngine;

namespace Ingame.Player
{
    public sealed class PlayerBaker : EcsMonoBaker
    {
        [SerializeReference, Required] private Rigidbody rigidbodyOnHips;
        [SerializeField] private int indexId;

        public int IndexId => indexId;
        protected override void Bake(EcsWorld world, int entityId)
        {
            IgnoreCollisionWithItself();
            
            world.AddCmp<RigidbodyCmp>(entityId).rigidbody = rigidbodyOnHips;
            
            ref var transformCmp = ref world.AddCmp<TransformCmp>(entityId);
            transformCmp.transform = transform;
            transformCmp.initPosition = transform.position;
            
            world.AddCmp<PlayerCmp>(entityId).elympicsPlayerProvider = GetComponent<ElympicsPlayerProvider>();
            world.AddCmp<ElympicsBehaviourMdl>(entityId).elympicsBehaviour = GetComponent<ElympicsBehaviour>();
        }

        private void IgnoreCollisionWithItself()
        {
            var collisions = GetComponentsInChildren<Collider>();
            var cashedSize = collisions.Length;
            
            for (int i = 0; i < cashedSize; i++)
            {
                var currenCollision = collisions[i];
                for (int j = i; j < cashedSize; j++)
                    Physics.IgnoreCollision(currenCollision, collisions[j]);
            }
        }
    }
}