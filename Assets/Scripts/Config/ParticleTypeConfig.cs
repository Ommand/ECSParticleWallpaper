using System;
using System.Linq;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "ParticleTypeConfig", menuName = "Config/ParticleTypeConfig", order = 1)]
    public class ParticleTypeConfig : ScriptableObject
    {
        [Serializable]
        private class Item
        {
            public Settings.SettingsModel.ParticleTypes Type;
            public Texture2D Texture;
        }

        [SerializeField] private Item[] _items;

        public Texture2D GetTexture(Settings.SettingsModel.ParticleTypes type)
        {
            return _items?.FirstOrDefault(item => item.Type == type)?.Texture;
        }
    }
}