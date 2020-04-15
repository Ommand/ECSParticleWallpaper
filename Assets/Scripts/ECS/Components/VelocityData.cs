using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    public struct VelocityData : IComponentData
    {
        public float2 Value;
    }
}