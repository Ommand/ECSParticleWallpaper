using ECS.Components;
using Unity.Entities;

namespace ECS.Systems
{
    [UpdateBefore(typeof(MoveSystem))]
    public class VelocityUpdateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var time = Time.DeltaTime;
            Entities.ForEach((ref VelocityData velocity, in AccelerationData acceleration) =>
                {
                    velocity.Value += time * acceleration.Value;
                })
                .ScheduleParallel();
        }
    }
}