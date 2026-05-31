namespace Utils.ColorEffects
{
    public class TintControllerUpdatable : TintController
    {
        private void Update()
        {
            if (!_tintColor.Equals(_lastTintColor))
            {
                ApplyTint();
                _lastTintColor = _tintColor;
            }
        }
    }
}