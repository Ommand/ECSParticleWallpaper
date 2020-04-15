using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    public struct AccelerationData : IComponentData
    {
        public float2 Value;
    }
}