using System;
using System.Linq;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "ColorConfig", menuName = "Config/ColorConfig")]
    public class ColorConfig : ScriptableObject
    {
        [Serializable]
        private class Item
        {
            public string name;
            public Color color;
        }

        [SerializeField] private Item[] _items;

        public Color? GetColor(string name)
        {
            return _items?.FirstOrDefault(item => name.StartsWith(item.name))?.color;
        }
    }
}