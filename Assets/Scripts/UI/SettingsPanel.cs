using System.IO;
using Config;
using DG.Tweening;
using Settings;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private ColorConfig _defaultTextureColorColfig;
    
        [Header("References")]
        [SerializeField] private TMP_Dropdown _particleTypeDropdown;
        [SerializeField] private TMP_Dropdown _msaaDropdown;
        [SerializeField] private TMP_Dropdown _framerateDropdown;
        [SerializeField] private TMP_Dropdown _repulsionDropdown;
        [SerializeField] private Slider _attractionSlider;
        [SerializeField] private TMP_Text _attractionValueLabel;
        [SerializeField] private Slider _repulsionSlider;
        [SerializeField] private TMP_Text _repulsionValueLabel;
        [SerializeField] private Slider _dampingSlider;
        [SerializeField] private TMP_Text _dampingValueLabel;
        [SerializeField] private Slider _particlesCountSlider;
        [SerializeField] private Image _backgroundColor;
        [SerializeField] private FlexibleColorPicker _colorPicker;
        [SerializeField] private TMP_Dropdown _defaultImageDropdown;
        [SerializeField] private RectTransform _hiddenPos;
        [SerializeField] private Slider _brightnessSlider;
        [SerializeField] private TMP_Text _brightnessValueLabel;
        [SerializeField] private Slider _saturationSlider;
        [SerializeField] private TMP_Text _saturationValueLabel;
        [SerializeField] private Slider _contrastSlider;
        [SerializeField] private TMP_Text _contrastValueLabel;
    
        private SettingsModel _settingsModel;
        private Vector2 _startPos;
        private RectTransform _rect;
        private bool _isHidden;
        private void Awake()
        {
            _settingsModel = Globals.SettingsHolder.SettingsModel;
            _rect = (RectTransform) transform;
            ApplySettingsVisual(_settingsModel);
            Globals.SettingsHolder.SettingsApplied += SettingsHolderOnSettingsApplied;

            BindCallbacks();
        }

        private void Start()
        {
            _startPos = _rect.anchoredPosition;
            _rect.anchoredPosition = _hiddenPos.anchoredPosition;
            _isHidden = true;
            _colorPicker.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Globals.SettingsHolder.SettingsApplied -= SettingsHolderOnSettingsApplied;

            UnbindCallbacks();
        }

        private void BindCallbacks()
        {
            if (_particleTypeDropdown) _particleTypeDropdown.onValueChanged.AddListener(_ => UpdateSettings());
            if (_msaaDropdown) _msaaDropdown.onValueChanged.AddListener(_ => UpdateSettings());
            if (_framerateDropdown) _framerateDropdown.onValueChanged.AddListener(_ => UpdateSettings());
            if (_repulsionDropdown) _repulsionDropdown.onValueChanged.AddListener(_ => UpdateSettings());
            if (_attractionSlider) _attractionSlider.onValueChanged.AddListener(_=> UpdateSettings());
            if (_repulsionSlider) _repulsionSlider.onValueChanged.AddListener(_=> UpdateSettings());
            if (_dampingSlider) _dampingSlider.onValueChanged.AddListener(_=> UpdateSettings());
            if (_brightnessSlider) _brightnessSlider.onValueChanged.AddListener(_=> UpdateSettings());
            if (_saturationSlider) _saturationSlider.onValueChanged.AddListener(_=> UpdateSettings());
            if (_contrastSlider) _contrastSlider.onValueChanged.AddListener(_=> UpdateSettings());
            if (_particlesCountSlider) _particlesCountSlider.onValueChanged.AddListener(_=> UpdateSettings());
            if (_defaultImageDropdown) _defaultImageDropdown.onValueChanged.AddListener(LoadDefaultImage);
        }

        private void UnbindCallbacks()
        {
            if (_particleTypeDropdown) _particleTypeDropdown.onValueChanged.RemoveAllListeners();
            if (_msaaDropdown) _msaaDropdown.onValueChanged.RemoveAllListeners();
            if (_framerateDropdown) _framerateDropdown.onValueChanged.RemoveAllListeners();
            if (_repulsionDropdown) _repulsionDropdown.onValueChanged.RemoveAllListeners();
            if (_attractionSlider) _attractionSlider.onValueChanged.RemoveAllListeners();
            if (_repulsionSlider) _repulsionSlider.onValueChanged.RemoveAllListeners();
            if (_dampingSlider) _dampingSlider.onValueChanged.RemoveAllListeners();
            if (_particlesCountSlider) _particlesCountSlider.onValueChanged.RemoveAllListeners();
            if (_brightnessSlider) _brightnessSlider.onValueChanged.RemoveAllListeners();
            if (_saturationSlider) _saturationSlider.onValueChanged.RemoveAllListeners();
            if (_contrastSlider) _contrastSlider.onValueChanged.RemoveAllListeners();
        }

        private void SettingsHolderOnSettingsApplied(SettingsModel settingsModel)
        {
            ApplySettingsVisual(settingsModel);
        }

        private void UpdateSettings()
        {
            _settingsModel.MSAA = 1 << _msaaDropdown.value;
            _settingsModel.TargetFramerate = 15 << _framerateDropdown.value;
            _settingsModel.ParticleType = (SettingsModel.ParticleTypes) _particleTypeDropdown.value;
            _settingsModel.Attraction = _attractionSlider.value;
            _settingsModel.Repulsion = _repulsionSlider.value;
            _settingsModel.Damping = _dampingSlider.value;
            _settingsModel.ParticlesScale = 1 / _particlesCountSlider.value;
            _settingsModel.RepulsionMode = (SettingsModel.RepulsionModes) _repulsionDropdown.value;
            _settingsModel.Brightness = _brightnessSlider.value;
            _settingsModel.Contrast = _contrastSlider.value;
            _settingsModel.Saturation = _saturationSlider.value;
        
            Globals.SettingsHolder.UpdateSettings(_settingsModel);
        }

        private void ApplySettingsVisual(SettingsModel settingsModel)
        {
            int GetPotBase(int value)
            {
                var counter = 0;
                while (value > 1)
                {
                    value >>= 1;
                    counter++;
                }
                return counter;
            }

            _msaaDropdown.SetValueWithoutNotify(GetPotBase(settingsModel.MSAA));
            _framerateDropdown.SetValueWithoutNotify(GetPotBase(settingsModel.TargetFramerate / 15));
            _particleTypeDropdown.SetValueWithoutNotify((int) settingsModel.ParticleType);
            _attractionSlider.SetValueWithoutNotify(settingsModel.Attraction);
            _attractionValueLabel.text = settingsModel.Attraction.ToString("N1");
            _repulsionSlider.SetValueWithoutNotify(settingsModel.Repulsion);
            _repulsionValueLabel.text = settingsModel.Repulsion.ToString("N2");
            _dampingSlider.SetValueWithoutNotify(settingsModel.Damping);
            _dampingValueLabel.text = settingsModel.Damping.ToString("N1");
            _particlesCountSlider.SetValueWithoutNotify(1 / settingsModel.ParticlesScale);
            _backgroundColor.color = settingsModel.BackgroundColor;
            _repulsionDropdown.SetValueWithoutNotify((int) settingsModel.RepulsionMode);
            _brightnessSlider.SetValueWithoutNotify(settingsModel.Brightness);
            _brightnessValueLabel.text = settingsModel.Brightness.ToString("N0");
            _saturationSlider.SetValueWithoutNotify(settingsModel.Saturation);
            _saturationValueLabel.text = settingsModel.Saturation.ToString("N0");
            _contrastSlider.SetValueWithoutNotify(settingsModel.Contrast);
            _contrastValueLabel.text = settingsModel.Contrast.ToString("N0");
        }

        public void LoadImageButtonClick()
        {
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Image", ".png"));
            FileBrowser.ShowLoadDialog(path =>
                {
                    var id = Path.GetFileName(path);
                    Globals.TextureDataService.Add(id, path);
                    _settingsModel.TextureId = id;
                    Globals.SettingsHolder.UpdateSettings(_settingsModel);
                }, null,
                initialPath: Application.dataPath);
        }

        private void LoadDefaultImage(int index)
        {
            /*
             * Index == 0 is reserved for NONE value
             */
            if (index == 0)
                return;
        
            _settingsModel.TextureId = $"{DefaultTextures.DefaultPrefix}{_defaultImageDropdown.options[index].text.ToUpper()}.png";
            _settingsModel.BackgroundColor = _defaultTextureColorColfig?.GetColor(_settingsModel.TextureId) ?? _settingsModel.BackgroundColor;
            Globals.SettingsHolder.UpdateSettings(_settingsModel);
            _defaultImageDropdown.value = -1;
        }

        public void SwitchState()
        {
            if (gameObject.activeSelf)
                Hide();
            else
                Show();
        }

        public void Show()
        {
            if (gameObject.activeSelf)
                return;
        
            gameObject.SetActive(true);
            _rect.DOAnchorPos(_startPos, 0.4f)
                .SetEase(Ease.OutCubic);
        }

        public void Hide()
        {
            if (_colorPicker.isActiveAndEnabled)
                ApplyColor();
            _rect.DOAnchorPos(_hiddenPos.anchoredPosition, 0.4f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => { gameObject.SetActive(false); });
        }

        public void ApplyColor()
        {
            _settingsModel.BackgroundColor = _colorPicker.color;
            Globals.SettingsHolder.UpdateSettings(_settingsModel);
            _colorPicker.gameObject.SetActive(false);
        }

        public void ResetSettings()
        {
            _settingsModel = SettingsHolder.DefaultSettingsModel;
            LoadDefaultImage(1);
        }
    }
}