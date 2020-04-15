using ECS.Components;
using Unity.Entities;
using Utils;

namespace ECS.Systems
{
    [UpdateBefore(typeof(MoveSystem))]
    public class VelocityUpdateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var damping = Globals.SettingsHolder.SettingsModel.Damping;
            var time = Time.DeltaTime;
            Entities.ForEach((ref VelocityData velocity, in AccelerationData acceleration) =>
                {
                    var dampingForce = -velocity.Value * damping;
                    velocity.Value += time * (acceleration.Value + dampingForce);
                })
                .ScheduleParallel();
        }
    }
}