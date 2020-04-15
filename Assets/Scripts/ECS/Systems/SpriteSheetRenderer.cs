using System.Collections.Generic;
using ECS.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityComponents;
using UnityEngine;
using UnityEngine.Rendering;
using Utils;

namespace ECS.Systems
{
    [UpdateAfter(typeof(TrsMatrixCalculationSystem))]
    public class SpriteSheetRenderer : SystemBase
    {
        private const int BatchCount = 1023;
        private readonly List<Matrix4x4> _matrixList = new List<Matrix4x4>(BatchCount);
        private readonly List<Vector4> _colorList = new List<Vector4>(BatchCount);
        protected override void OnUpdate()
        {
            var materialPropertyBlock = new MaterialPropertyBlock();
            var quadMesh = Globals.Quad;
            var material = Globals.ParticleMaterial;
            var shaderPropertyId = Shader.PropertyToID("_Color");

            var entityQuery = GetEntityQuery(typeof(ParticleData));
            var animationData = entityQuery.ToComponentDataArray<ParticleData>(Allocator.TempJob);
            var layer = LayerMask.NameToLayer("Particles");
        
            for (int meshCount = 0; meshCount < animationData.Length; meshCount+= BatchCount)
            {
                var batchSize = math.min(BatchCount, animationData.Length - meshCount);
                _matrixList.Clear();
                _colorList.Clear();

                for (var i = meshCount; i < meshCount + batchSize; i++)
                {
                    _matrixList.Add(animationData[i].matrix);
                    _colorList.Add(animationData[i].color);
                }
            
                materialPropertyBlock.SetVectorArray(shaderPropertyId, _colorList);
                Graphics.DrawMeshInstanced(
                    quadMesh,
                    0,
                    material,
                    _matrixList,
                    materialPropertyBlock,
                    ShadowCastingMode.Off, false, layer);
            }
        

            animationData.Dispose();

            // Entities.ForEach((ref Translation translation, ref SpriteSheetAnimation_Data spriteSheetAnimationData) =>
            //     {
            //
            //         materialPropertyBlock.SetColor(shaderPropertyId, spriteSheetAnimationData.color);
            //
            //         Graphics.DrawMesh(
            //             quadMesh,
            //             spriteSheetAnimationData.matrix,
            //             material,
            //             0, // Layer
            //             camera,
            //             0, // Submesh index
            //             materialPropertyBlock
            //         );
            //     })
            //     .WithoutBurst()
            //     .Run();
        }

    }
}
