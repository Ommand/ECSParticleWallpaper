using UnityEngine;

namespace ColorSampler
{
    public class RandomColorSampler : IColorSampler
    {
        public bool Sample(Vector2 worldPos, out Color color)
        {
            color = new Color(Random.value, Random.value, Random.value);
            return true;
        }
    }
}