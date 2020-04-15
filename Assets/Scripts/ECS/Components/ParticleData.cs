using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct ParticleData : IComponentData {
        public Color color;
        public Matrix4x4 matrix;
    }
}