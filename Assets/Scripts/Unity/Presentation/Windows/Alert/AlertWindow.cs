using System;
using Core.Application.Interfaces;
using TMPro;
using Unity.Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows.Alert
{
    [Window(nameof(AlertWindow))]
    public class AlertWindow : WindowBase
    {
        public event Action<AlertResult> OnResultSelected;

        [SerializeField]
        private TextMeshProUGUI _labelText;
        [SerializeField]
        private TextMeshProUGUI _descriptionText;
        [SerializeField]
        private GameObject _okButton;
        [SerializeField]
        private GameObject _yesButton;
        [SerializeField]
        private GameObject _noButton;

        [Inject]
        private ILocalization _localization;

        public void Setup(AlertWindowState state, string labelKey, string descriptionKey)
        {
            _labelText.text = _localization.Get(labelKey);
            _descriptionText.text = _localization.Get(descriptionKey);

            _okButton.SetActive(state == AlertWindowState.Ok);
            _yesButton.SetActive(state == AlertWindowState.YesNo);
            _noButton.SetActive(state == AlertWindowState.YesNo);
        }

        public void OnOkClicked()
        {
            OnResultSelected?.Invoke(AlertResult.Ok);
            Hide();
        }

        public void OnYesClicked()
        {
            OnResultSelected?.Invoke(AlertResult.Yes);
            Hide();
        }

        public void OnNoClicked()
        {
            OnResultSelected?.Invoke(AlertResult.No);
            Hide();
        }
    }
}