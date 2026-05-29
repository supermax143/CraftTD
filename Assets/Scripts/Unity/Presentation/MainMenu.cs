using System.Linq;
using Core.Application.Interfaces;
using Core.Application.Interfaces.ApplicationSession;
using Core.Domain.Services;
using Core.Domain.Services.ApplicationSession;
using TMPro;
using UnityEngine;
using Zenject;

namespace Unity.Presentation
{
	public class MainMenu : MonoBehaviour
	{
		[Inject] private IApplicationSession _applicationSession;
		[Inject] private ILocalization _localization;
		
		[SerializeField]
		private TMP_Dropdown _languageSelector;

		private void Start()
		{
			UpdateLangugeSelector();
		}

		private void UpdateLangugeSelector()
		{
			if (!_localization.TryGetLanguageCodes(out var codes))
			{
				return;
			}
			
			_languageSelector.options = codes.
				Select(code => new TMP_Dropdown.OptionData { text = code } ).ToList();
			
			_languageSelector.onValueChanged.AddListener(OnLangugeChanged);
		}


		private void OnLangugeChanged(int value)
		{
			 _localization.SetLanguage(_languageSelector.options[value].text);
		}

		public void StartGame()
		{
			_applicationSession.CurrentState.StartGame();
		}
	}
}