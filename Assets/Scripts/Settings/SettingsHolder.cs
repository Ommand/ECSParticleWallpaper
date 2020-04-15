using System;
using Newtonsoft.Json;
using UnityEngine;
using Utils;

namespace Settings
{
    public class SettingsHolder
    {
        public event Action<SettingsModel> SettingsApplied;
        public SettingsModel SettingsModel { get; private set; }

        public void UpdateSettings(SettingsModel settingsModel)
        {
            SettingsModel = settingsModel;
            SettingsApplied?.Invoke(SettingsModel);

            var serializeObject = JsonConvert.SerializeObject(settingsModel, Formatting.None);
            PlayerPrefs.SetString(SettingsModel.PlayerPrefsKey, serializeObject);
            PlayerPrefs.Save();
        }

        public SettingsHolder()
        {
            var settingsStr = PlayerPrefs.GetString(SettingsModel.PlayerPrefsKey, null);
            if (!string.IsNullOrEmpty(settingsStr))
                SettingsModel = (SettingsModel) JsonConvert.DeserializeObject(settingsStr, typeof(SettingsModel));
            else
                SettingsModel = DefaultSettingsModel;
        }

        public static SettingsModel DefaultSettingsModel => new SettingsModel
        {
            MSAA = QualitySettings.antiAliasing,
            Attraction = 5,
            Repulsion = 1,
            Damping = 2,
            ParticlesScale = 0.04f,
            TargetFramerate = 60,
            BackgroundColor = Camera.main.backgroundColor,
            TextureId = DefaultTextures.DefaultMap,
            RepulsionMode = SettingsModel.RepulsionModes.Segment
        };
    }
}