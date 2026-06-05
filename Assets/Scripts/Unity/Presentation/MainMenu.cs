using System.Linq;
using Core.Application.DataStorage;
using Core.Application.Interfaces;
using Core.Application.Interfaces.ApplicationSession;
using Core.Domain.Services;
using Core.Domain.Services.ApplicationSession;
using TMPro;
using Unity.Game;
using UnityEngine;
using Zenject;

namespace Unity.Presentation
{
	public class MainMenu : MonoBehaviour
	{
		[Inject] private IApplicationSession _applicationSession;
		[Inject] private ILocalization _localization;
		[Inject] private EpochManager _epochManager;
		[Inject] private IDataStorage _dataStorage;
		
		[SerializeField]
		private TMP_Dropdown _languageSelector;
		[SerializeField]
		private TMP_Dropdown _epochSelector;
		[SerializeField]
		private TMP_InputField _foodProductionInput;
		
		private void Start()
		{
			UpdateLanguageSelector();
			UpdateEpochSelector();
			UpdateFoodProductionInput();
		}

		private void UpdateFoodProductionInput()
		{
			_foodProductionInput.text = _dataStorage.FoodProductionPerSecond.ToString();
		}

		private void UpdateEpochSelector()
		{
			_epochSelector.options = _epochManager.GetEpochs().
				Select(epochData => new TMP_Dropdown.OptionData { text = epochData.EpochName } ).ToList();
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

		public void StartGame()
		{
			_applicationSession.CurrentState.StartGame();
		}
		
		public void Save()
		{
			_epochManager.SetEpoch(_epochSelector.value);
			_dataStorage.SetFoodProductionPerSecond(float.Parse(_foodProductionInput.text));
		}
	}
}