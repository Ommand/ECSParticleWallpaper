using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    public struct AttractorPosData : IComponentData
    {
        public float2 Value;
    }
}