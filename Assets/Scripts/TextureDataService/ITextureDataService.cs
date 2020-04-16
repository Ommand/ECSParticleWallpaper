using System;
using UnityEngine;

namespace TextureDataService
{
    public interface ITextureDataService
    {
        void Add(string id, string path);
        void LoadTexture(string id, Action<Texture2D> onComplete);
    }
}