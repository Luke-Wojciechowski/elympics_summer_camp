using UnityEngine;

namespace Ingame
{
	[CreateAssetMenu(fileName = "SettingsConfig", menuName = "Configs/SettingsConfig")]
	public sealed class SettingsConfig : ScriptableObject
	{
		[SerializeField] private SettingsData defaultSettings;

		public SettingsData DefaultSettings => defaultSettings;
	}
}