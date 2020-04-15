using UnityEngine;

namespace ColorSampler
{
    public interface IColorSampler
    {
        bool Sample(Vector2 worldPos, out Color color);
    }
}