using System;

namespace Ingame
{
	public interface ISettingsService
	{
		public ref SettingsData CurrentSettings { get; }
		public event Action<SettingsData> OnSettingsUpdated;

		public void ResetToDefault();
		public void ApplySettings(in SettingsData settingsData);
	}
}