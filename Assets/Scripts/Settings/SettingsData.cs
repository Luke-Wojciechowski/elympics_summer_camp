using System;

namespace Ingame
{
	[Serializable]
	public struct SettingsData
	{
		public Camera camera;
		
		[Serializable]
		public struct Camera
		{
			public float rotationSpeed;
			public float movementSpeed;
			public float dragSpeed;
		}
	}
}