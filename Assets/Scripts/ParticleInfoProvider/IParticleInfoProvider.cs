using System.Collections.Generic;
using UnityEngine;

namespace ParticleInfoProvider
{
    public interface IParticleInfoProvider
    {
        (IReadOnlyList<Color> colors, IReadOnlyList<Vector2> positions) GetData();
    }
}
