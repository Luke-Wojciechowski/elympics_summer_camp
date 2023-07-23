using System;
using Ingame;

namespace Ingame
{
	public sealed class SettingsService : ISettingsService
	{
		private SettingsData _defaultSettings;
		private SettingsData _currentSettings;
		
		public ref SettingsData CurrentSettings => ref _currentSettings;

		public event Action<SettingsData> OnSettingsUpdated;

		public SettingsService(SettingsConfig settingsConfig)
		{
			_defaultSettings = settingsConfig.DefaultSettings;
			_currentSettings = settingsConfig.DefaultSettings;
		}
		
		public void ResetToDefault()
		{
			SLog.Msg($"Resting settings to default: {_defaultSettings}", this);
			
			_currentSettings = _defaultSettings;
			OnSettingsUpdated?.Invoke(_currentSettings);
		}

		public void ApplySettings(in SettingsData settingsData)
		{
			SLog.Msg($"Applying settings: {settingsData}", this);
			
			_currentSettings = settingsData;
			OnSettingsUpdated?.Invoke(_currentSettings);
		}
	}
}