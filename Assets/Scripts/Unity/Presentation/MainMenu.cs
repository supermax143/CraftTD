using System.Linq;
using Core.Application.DataStorage;
using Core.Application.Interfaces;
using Core.Application.Interfaces.ApplicationSession;
using Core.Application.Interfaces.Windows;
using Core.Application.Models;
using Core.Domain.Services;
using Core.Domain.Services.ApplicationSession;
using TMPro;
using Unity.Game;
using Unity.Presentation.Windows;
using UnityEngine;
using Zenject;

namespace Unity.Presentation
{
	public class MainMenu : MonoBehaviour
	{
		[Inject] private IApplicationSession _applicationSession;
		[Inject] private ILocalization _localization;
		[Inject] private IDataStorage _dataStorage;
		[Inject] private IMainModel _mainModel;
        
		private EpochModel Epoch => _mainModel.Epoch;
        
		
		[SerializeField]
		private TMP_Dropdown _languageSelector;
		[SerializeField]
		private TMP_Dropdown _epochSelector;
		[SerializeField]
		private TMP_InputField _foodProductionInput;
		[SerializeField]
		private TMP_InputField _moneyInput;

		[Inject] private IWindowsController _windowsController;

		private void Start()
		{
			UpdateLanguageSelector();
			UpdateMoneyInput();
		}

		private void UpdateMoneyInput()
		{
			_moneyInput.text = Epoch.Money.ToString();
		}


		private void UpdateLanguageSelector()
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

		public void ShowUpgradeWindow()
		{
			_windowsController.ShowWindow<UpgradeWindow>(window =>
			{
				window.Initialize();
				window.Show();
			});
		}
		
		public void StartGame()
		{
			_applicationSession.CurrentState.StartGame();
		}
		
		public void Save()
		{
			Epoch.Money = uint.Parse(_moneyInput.text);
		}
		
		public void Reset()
		{
			_mainModel.Reset();
			UpdateMoneyInput();
		}
	}
}