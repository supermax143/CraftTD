namespace Unity.Infrastructure.Tutorial
{
    public interface ITutorialOverlayController
    {
        void ShowOverlay(float alpha = -1);
        void HideOverlay();
    }
}