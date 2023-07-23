using UnityEngine;

namespace Inagme
{
	public interface IInputService
	{
		public bool CameraInputEnabled { get; }
		public bool UiInputEnabled { get; }
		public PlayerInput CurrentPlayerInput { get; }
	}
}