using System;
using UnityEngine;

namespace ColorSampler
{
    public class TextureColorSampler : IColorSampler
    {
        private readonly Rect _viewport;
        private readonly float _alphaThreshold;
        private Rect _textureWorldRect;
        private readonly Texture2D _texture2D;

        private void CalculateTextureRect()
        {
            var screenRatio = _viewport.width / _viewport.height;
            var textureRatio = (float) _texture2D.width / _texture2D.height;
            float textureWorldWidth;
            float textureWorldHeight;

            if (textureRatio < screenRatio)
            {
                textureWorldHeight = _viewport.height;
                textureWorldWidth = _viewport.height * textureRatio;
            }
            else
            {
                textureWorldWidth = _viewport.width;
                textureWorldHeight = _viewport.width / textureRatio;
            }
        
            var textureWorldSize = new Vector2(textureWorldWidth, textureWorldHeight);

            _textureWorldRect = new Rect(-textureWorldSize / 2, textureWorldSize);
        }

        public bool Sample(Vector2 worldPos, out Color color)
        {
            if (_textureWorldRect.Contains(worldPos))
            {
                var u = Mathf.InverseLerp(_textureWorldRect.xMin, _textureWorldRect.xMax, worldPos.x);
                var v = Mathf.InverseLerp(_textureWorldRect.yMin, _textureWorldRect.yMax, worldPos.y);
                color = _texture2D.GetPixelBilinear(u, v);
                var alpha = color.a;
                color.a = 1;
                return alpha >= _alphaThreshold;
            }

            color = Color.clear;
            return false;
        }

        public TextureColorSampler(Texture2D texture2D, Rect viewport, float alphaThreshold = 0)
        {
            _viewport = viewport;
            _alphaThreshold = alphaThreshold;
            _texture2D = texture2D ? texture2D : throw new ArgumentNullException(nameof(texture2D));
            CalculateTextureRect();
        }
    }
}