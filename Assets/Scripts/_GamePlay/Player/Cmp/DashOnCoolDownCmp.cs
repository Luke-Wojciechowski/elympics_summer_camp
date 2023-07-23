using Secs;

namespace Ingame.Player
{
    public struct DashOnCoolDownCmp : IEcsComponent
    {
        public float timeLeft;
    }
}