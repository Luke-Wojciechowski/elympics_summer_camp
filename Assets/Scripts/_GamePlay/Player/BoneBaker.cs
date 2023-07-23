using Ingame.Common;
using Secs;

namespace Ingame.Player
{
    public sealed class BoneBaker : EcsMonoBaker
    {
        protected override void Bake(EcsWorld world, int entityId)
        {
            world.AddCmp<BoneTag>(entityId);
            ref var transformCmp = ref world.AddCmp<TransformCmp>(entityId);
            transformCmp.transform = transform;
            transformCmp.initPosition = transform.position;
        }
    }
}