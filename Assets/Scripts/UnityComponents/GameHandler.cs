using ColorSampler;
using Config;
using ECS.Components;
using ParticleInfoProvider;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Utils;

namespace UnityComponents
{
    public class GameHandler : MonoBehaviour
    {
        [SerializeField] private Material _instancedMaterial;
        [SerializeField] private Mesh _quadMesh;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private ParticleTypeConfig _particleConfig;
        [SerializeField] private PostProcessProfile _postProcessProfile;

        private Camera _camera;
        private Texture2D _samplerTexture;
        private float _lastScale;
        private string _lastSamplerName;

        private void Awake()
        {
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
            _camera = Camera.main;
            Globals.Quad = _quadMesh;
            Globals.ParticleMaterial = _instancedMaterial;
            Globals.CoroutinProcessor = this;

            CalculateViewport();

            ApplySettings(Globals.SettingsHolder.SettingsModel);
            Globals.SettingsHolder.SettingsApplied += SettingsHolderOnSettingsApplied;
        }

        private void OnDestroy()
        {
            Globals.SettingsHolder.SettingsApplied -= SettingsHolderOnSettingsApplied;
        }

        private void CalculateViewport()
        {
            var extent =
                new Vector2(_camera.orthographicSize * Screen.width / Screen.height, _camera.orthographicSize) * 0.95f;
            var viewport = new Rect((Vector2) _camera.transform.position - extent, extent * 2);
            Globals.WorldScreenViewport = viewport;
        }

        private void SettingsHolderOnSettingsApplied(Settings.SettingsModel settingsModel)
        {
            ApplySettings(settingsModel);
        }

        private void ApplySettings(Settings.SettingsModel settingsModel, bool allowedToUpdateTexture = true)
        {
            /*
             * Reload texture if ID was changed
             */
            var settingsTextureId = settingsModel.TextureId ?? DefaultTextures.DefaultMap;
            if (allowedToUpdateTexture && (!_samplerTexture || settingsTextureId != _samplerTexture?.name))
            {
                if (_samplerTexture && !_samplerTexture.name.StartsWith(DefaultTextures.DefaultPrefix))
                    Destroy(_samplerTexture);

                Globals.TextureDataService.LoadTexture(settingsTextureId, texture2D =>
                {
                    _samplerTexture = texture2D ? texture2D : Texture2D.whiteTexture;
                    ApplySettings(settingsModel, false);
                });
                return;
            }

            if (!_samplerTexture) return;

            _instancedMaterial.mainTexture = _particleConfig.GetTexture(settingsModel.ParticleType);
            QualitySettings.antiAliasing = settingsModel.MSAA;
            Application.targetFrameRate = settingsModel.TargetFramerate;
            _camera.backgroundColor = settingsModel.BackgroundColor;

            var colorGrading = _postProcessProfile.GetSetting<ColorGrading>();
            colorGrading.brightness.value = settingsModel.Brightness;
            colorGrading.saturation.value = settingsModel.Saturation;
            colorGrading.contrast.value = settingsModel.Contrast;

            var isScaleOrTextureChanged = settingsModel.ParticlesScale != _lastScale || _lastSamplerName != _samplerTexture.name;
            if (isScaleOrTextureChanged)
            {
                _lastSamplerName = _samplerTexture.name;
                _lastScale = settingsModel.ParticlesScale;
                
                var colorSampler = new TextureColorSampler(_samplerTexture, Globals.WorldScreenViewport, 10 / 255.0f);
                var provider = new DefaultParticleInfoProvider(colorSampler, Globals.WorldScreenViewport, Globals.SettingsHolder.SettingsModel.ParticlesSpacing);
                
                PrepareEntities(provider);
            }
        }

        private void PrepareEntities(IParticleInfoProvider provider)
        {
            var (colors, positions) = provider.GetData();

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entityArchetype = entityManager.CreateArchetype(
                typeof(Translation),
                typeof(ParticleData),
                typeof(VelocityData),
                typeof(AccelerationData),
                typeof(AttractorPosData)
            );

            entityManager.DestroyEntity(entityManager.CreateEntityQuery(typeof(ParticleData)));

            var entityArray = new NativeArray<Entity>(colors.Count, Allocator.Temp);
            entityManager.CreateEntity(entityArchetype, entityArray);

            for (var index = 0; index < entityArray.Length; index++)
            {
                var entity = entityArray[index];
                var pos = new float3(positions[index].x, positions[index].y, 0);
                entityManager.SetComponentData(entity, new Translation {Value = pos});
                entityManager.SetComponentData(entity, new AttractorPosData {Value = pos.xy});
                entityManager.SetComponentData(entity, new ParticleData {color = colors[index]});
            }

            _label.text = entityArray.Length.ToString();
            entityArray.Dispose();

        }
    }
}