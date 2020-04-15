using ECS.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Utils;

namespace ECS.Systems
{
    [UpdateAfter(typeof(MoveSystem))]
    public class TrsMatrixCalculationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var scale = Globals.SettingsHolder.SettingsModel.ParticlesScale;
            Entities.ForEach((ref ParticleData data, in Translation translation) =>
                {
                    data.matrix = Matrix4x4.TRS(translation.Value, Quaternion.identity,
                        new Vector3(scale, scale, scale));
                })
                .ScheduleParallel();
        }
    }
}