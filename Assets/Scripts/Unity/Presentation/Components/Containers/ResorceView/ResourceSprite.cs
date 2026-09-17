using UnityEngine;

namespace Unity.Presentation.HUD
{
    public class ResourceSprite : ResourceIconBase<SpriteRenderer>
    {
        protected override void UpdateSprite(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public override RectTransform GetRect()
        {
            return null;
        }
    }
}