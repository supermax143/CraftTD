using Core.Application.Interfaces;
using Core.Domain.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation
{
    public class InitGamePanel : MonoBehaviour
    {
    
        [SerializeField]
        private TMP_Text _textField;
        [SerializeField]
        private Scrollbar _scrollbar;
    
        [Inject] IBootstrapProgress _bootstrapProgress;
        [Inject] ILocalization _localization;
    
        void Start()
        {
            _bootstrapProgress.OnStepStarted += UpdateView;
            _bootstrapProgress.OnInitializationComplete += UpdateView;
            UpdateView();
        }

        private void UpdateView()
        {
            _scrollbar.size = _bootstrapProgress.Progress;
            if (_localization.Initialized)
            {
                _textField.text = _localization.Get(_bootstrapProgress.CurStepIdent);
            }
        }


        private void OnDestroy()
        {
            _bootstrapProgress.OnStepStarted -= UpdateView;
            _bootstrapProgress.OnInitializationComplete -= UpdateView;
        }
    }
}
