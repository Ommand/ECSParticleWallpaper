using ECS.Components;
using Settings;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityComponents;
using UnityEngine;
using Utils;

namespace ECS.Systems
{
    [UpdateBefore(typeof(VelocityUpdateSystem))]
    public class AccelerationSystem : SystemBase
    {
        private float2 _repulsorPrevPos = new float2(float.NaN);
    
        public static float2 FindNearestPointOnLine(in float2 origin, in float2 end, in float2 point)
        {
            var heading = end - origin;
            var magnitudeSqMax = math.distancesq(origin, end);
            heading = math.normalizesafe(heading);

            //Do projection from the point but clamp it
            var lhs = point - origin;
            var dotP = math.dot(lhs, heading);
            dotP = math.clamp(dotP, 0f, magnitudeSqMax);
            return origin + heading * dotP;
        }
    
        protected override void OnUpdate()
        {
            var repulsionMode = Globals.SettingsHolder.SettingsModel.RepulsionMode;
            float2 repulsorPos = Globals.Repulsor.Position;
            float2 repulsorPrevPos = float.IsNaN(_repulsorPrevPos.x) ? repulsorPos : _repulsorPrevPos;
            _repulsorPrevPos = repulsorPos;
        
            var attractionPower = Globals.SettingsHolder.SettingsModel.Attraction;
            var repulsionPower = Globals.SettingsHolder.SettingsModel.Repulsion;
            var dampingPower = Globals.SettingsHolder.SettingsModel.Damping;
            Entities.ForEach((ref AccelerationData acceleration, in AttractorPosData attractorPosition, in Translation t, in VelocityData velocity) =>
                {
                    var attraction = (attractorPosition.Value - t.Value.xy) * attractionPower;

                    var repulsorPosition = repulsionMode == SettingsModel.RepulsionModes.Point
                        ? repulsorPos
                        : FindNearestPointOnLine(repulsorPos, repulsorPrevPos, t.Value.xy);
                
                    var distSqr = math.clamp(math.distancesq(repulsorPosition, t.Value.xy), Globals.MinRepulsionDist * Globals.MinRepulsionDist, float.MaxValue);
                    var repulsion = -math.normalizesafe(repulsorPosition - t.Value.xy) / distSqr * repulsionPower;
                    
                    var damping  = -velocity.Value * dampingPower;
                    
                    acceleration.Value = attraction + repulsion + damping;
                })
                .ScheduleParallel();
        }
    }
}