using UnityEngine;

namespace UI
{
    public class InstantColorApplier : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private FlexibleColorPicker _colorPicker;

        private bool _isInit = false;
        private void Update()
        {
            if (!_isInit)
            {
                _isInit = true;
                _colorPicker.color = _camera.backgroundColor;
            }
            if (_camera.backgroundColor != _colorPicker.color)
                _camera.backgroundColor = _colorPicker.color;
        }
    }
}