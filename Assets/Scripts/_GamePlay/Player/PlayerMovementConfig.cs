using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Player
{
    [CreateAssetMenu(menuName = "Configs/MovementConfig", fileName = "PlayerMovementConfig")]
    public sealed class PlayerMovementConfig : ScriptableObject
    {
        [SerializeField] [Min(0)] private float speed;
        [SerializeField] [Min(0)] private float dashForce;
        [SerializeField] [Min(0)] private float jumpForce;
        [SerializeField] [Min(0)] private float dashCooldown;
        [SerializeField] [Min(0)] private float yDirectionOnDash;

        public float Speed => speed;
        public float DashForce => dashForce; 
        public float JumpForce => jumpForce;
        
        public float DashCooldown => dashCooldown;
        
        public float YDirectionOnDash => yDirectionOnDash;
    }
}