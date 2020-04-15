using ECS.Components;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.Systems
{
    public class MoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var time = Time.DeltaTime;
            Entities.ForEach((ref Translation t, in VelocityData velocity) => { t.Value.xy += time * velocity.Value; })
                .ScheduleParallel();
        }
    }
}