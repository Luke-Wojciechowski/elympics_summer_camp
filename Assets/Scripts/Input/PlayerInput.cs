using UnityEngine;

namespace Inagme
{
	public struct PlayerInput
	{
		public UI ui;
		public Camera camera;
		public Cursor cursor;
		public Movement movement;
			
		public struct UI
		{
			public bool goBackClicked;
			public bool acceptClicked;
			public bool toggleConstructionUi;
		}

		public struct Camera
		{
			public Vector2 movementInput;
			public float rotationInput;
		}

		public struct Cursor
		{
			public Vector2 cursorPosition;
			public bool leftButtonClicked;
			public bool rightButtonClicked;
			public bool middleButtonClicked;
			public bool leftButtonReleased;
			public bool rightButtonReleased;
			public bool middleButtonReleased;
		}
		
		public struct Movement
		{
			public bool left;
			public bool right;
			public bool forward;
			public bool backward;
			public bool jump;
			public bool dash;
			public int idIndex;
		}
	}
}