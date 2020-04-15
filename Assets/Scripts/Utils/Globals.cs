using Settings;
using TextureDataService;
using UnityComponents;
using UnityEngine;

namespace Utils
{
    public static class Globals
    {
        public const float MinRepulsionDist = 0.06f;
        
        public static Rect WorldScreenViewport { get; set; }
        public static Mesh Quad { get; set; }
        public static Material ParticleMaterial { get; set; }
        public static MonoBehaviour CoroutinProcessor { get; set; }
        
        /*
         * Should be replaced with DI in real projects
         */
        public static SettingsHolder SettingsHolder { get; set; } = new SettingsHolder();
        public static ITextureDataService TextureDataService { get; set; } = new DefaultTextureDataService();
        public static IRepulsor Repulsor { get; set; } = new Repulsor(Camera.main);
    }
}