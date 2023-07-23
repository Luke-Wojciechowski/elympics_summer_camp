using Secs;
using UnityEngine;

namespace Ingame.Common
{
    public struct TransformCmp : IEcsComponent
    {
        public Transform transform;
        public Vector3 initPosition;
    }
}