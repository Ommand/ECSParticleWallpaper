using System;
using UnityEngine;

namespace UnityComponents
{
    public class Repulsor : IRepulsor
    {
        private readonly Camera _camera;
        public Vector2 Position => _camera.ScreenToWorldPoint(Input.mousePosition);

        public Repulsor(Camera camera)
        {
            _camera = camera ? camera : throw new ArgumentNullException(nameof(camera));
        }
    }
}