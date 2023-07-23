using System.Runtime.CompilerServices;
using Ingame;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Inagme
{
	public sealed class InputService : IInputService, ITickable
	{
		private readonly ISettingsService _settingsService;
		private readonly UnityInputActions _inputActions = new();
		private PlayerInput _currentPlayerInput;

		public bool CameraInputEnabled
		{
			get => _inputActions.Camera.enabled;
			set
			{
				if(value)
					_inputActions.Camera.Enable();
				else _inputActions.Camera.Disable();
			}
		}

		public bool UiInputEnabled
		{
			get => _inputActions.UI.enabled;
			set
			{
				if(value)
					_inputActions.UI.Enable();
				else
					_inputActions.UI.Disable();
			}
		}

		public bool UiMovementEnabled
		{
			get => _inputActions.Movement.enabled;
			set
			{
				if(value)
					_inputActions.Movement.Enable();
				else
					_inputActions.Movement.Disable();
			}
		}
		
		public PlayerInput CurrentPlayerInput => _currentPlayerInput;

		[Inject]
		public InputService(ISettingsService settingsService)
		{
			_settingsService = settingsService;
			_inputActions.Enable();
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private PlayerInput.UI GetUiInput()
		{
			return new PlayerInput.UI
			{
				goBackClicked = _inputActions.UI.GoBack.WasPerformedThisFrame(),
				acceptClicked = _inputActions.UI.Accept.WasPerformedThisFrame(),
				toggleConstructionUi = _inputActions.UI.ToggleConstructionUi.WasPerformedThisFrame()
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private PlayerInput.Camera GetCameraInput()
		{
			ref var cameraSettings = ref _settingsService.CurrentSettings.camera;

			var movementInput = _inputActions.Camera.Drag.IsPressed()
				? -Mouse.current.delta.ReadValue() * cameraSettings.dragSpeed
				: _inputActions.Camera.Move.ReadValue<Vector2>() * cameraSettings.movementSpeed;

			float rotationInput = _inputActions.Camera.Rotate.ReadValue<float>() * cameraSettings.rotationSpeed; 
			
			return new PlayerInput.Camera
			{
				movementInput = movementInput,
				rotationInput = rotationInput
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private PlayerInput.Cursor GetCursorInput()
		{
			
			return new PlayerInput.Cursor
			{
				cursorPosition = Mouse.current.position.ReadValue(),
				leftButtonClicked = Mouse.current.leftButton.wasPressedThisFrame,
				rightButtonClicked = Mouse.current.rightButton.wasPressedThisFrame,
				middleButtonClicked = Mouse.current.middleButton.wasPressedThisFrame,
				leftButtonReleased = Mouse.current.leftButton.wasReleasedThisFrame,
				rightButtonReleased = Mouse.current.rightButton.wasReleasedThisFrame,
				middleButtonReleased = Mouse.current.middleButton.wasReleasedThisFrame,
			};
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private PlayerInput.Movement GetMovementInput()
		{
			
			return new PlayerInput.Movement
			{
				forward = _inputActions.Movement.Forward.IsPressed(),
				backward = _inputActions.Movement.Backward.IsPressed(),
				left = _inputActions.Movement.Left.IsPressed(),
				right = _inputActions.Movement.Right.IsPressed(),
				jump = _inputActions.Movement.Jump.IsPressed(),
				dash = _inputActions.Movement.Dash.IsPressed(),
			};
		}
		public void Tick()
		{
			_currentPlayerInput = new PlayerInput
			{
				ui = GetUiInput(),
				camera = GetCameraInput(),
				cursor = GetCursorInput(),
				movement = GetMovementInput()
			};
		}
	}
}