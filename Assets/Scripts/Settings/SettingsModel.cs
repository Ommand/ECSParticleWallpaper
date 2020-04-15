using System;
using Newtonsoft.Json;
using UnityEngine;
using Utils;

namespace Settings
{
    public class SettingsModel
    {
        public const string PlayerPrefsKey = "Settings_Key";
    
        public enum ParticleTypes : byte
        {
            Quad = 0,
            Circle = 1
        };

        public enum RepulsionModes : byte
        {
            Point,
            Segment
        }

        public int MSAA { get; set; }
        public int TargetFramerate { get; set; }
        public ParticleTypes ParticleType { get; set; }
        public float Attraction { get; set; }
        public float Repulsion { get; set; }
        public RepulsionModes RepulsionMode { get; set; }
        public float Damping { get; set; }
        [JsonIgnore] public float ParticlesSpacing => ParticlesScale * 1.3f;
        public float ParticlesScale { get; set; }
        public string TextureId { get; set; } = DefaultTextures.DefaultMap;

        [JsonConverter(typeof(ColorConverter))]
        public Color BackgroundColor { get; set; }
        public float Brightness { get; set; }
        public float Contrast { get; set; }
        public float Saturation { get; set; }
    }

    public class ColorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var color = (Color) value;
            writer.WriteValue($"#{ColorUtility.ToHtmlStringRGBA(color)}");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ColorUtility.TryParseHtmlString((string) reader.Value, out var color) ? color : Color.magenta;
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }
    }
}