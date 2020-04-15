using System;
using System.Collections.Generic;
using ColorSampler;
using Unity.Mathematics;
using UnityEngine;

namespace ParticleInfoProvider
{
    public class DefaultParticleInfoProvider : IParticleInfoProvider
    {
        private readonly IColorSampler _sampler;
        private readonly Rect _viewport;
        private readonly float _step;

        public (IReadOnlyList<Color> colors, IReadOnlyList<Vector2> positions) GetData()
        {
            var colors = new List<Color>();
            var positions = new List<Vector2>();

            var rows = Mathf.RoundToInt(_viewport.height / _step) + 1;
            var cols = Mathf.RoundToInt(_viewport.width / _step) + 1;

            var rowStep = _viewport.height / (rows - 1);
            var colStep = _viewport.width / (cols - 1);

            for (var row = 0; row < rows; row++)
            for (var col = 0; col < cols; col++)
            {
                var pos = _viewport.min + new Vector2(col * colStep, row * rowStep);
                if (_sampler.Sample(pos, out var color))
                {
                    positions.Add(pos);
                    colors.Add(color);
                }
            }

            return (colors, positions);
        }

        public DefaultParticleInfoProvider(IColorSampler sampler, Rect viewport, float step)
        {
            _sampler = sampler ?? throw new ArgumentNullException(nameof(sampler));
            _viewport = viewport;
            _step = math.clamp(step, 0, 1e2f);
        }
    }
}
