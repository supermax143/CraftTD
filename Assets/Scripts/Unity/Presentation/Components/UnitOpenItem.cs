using Core.Application.Models;
using TMPro;
using Unity.Game;
using UnityEngine;

namespace Unity.Presentation.Components
{
    public class UnitOpenItem : MonoBehaviour

    {
    [SerializeField] private TextMeshProUGUI _unitNameTF;
    [SerializeField] private PriceButton _priceButton;
    [SerializeField] private UnitTier _tier;
    private UnitModel _unitModel;

    public UnitTier Tier => _tier;

    public void SetUnit(UnitModel unitModel)
    {
        _unitModel = unitModel;
        _unitNameTF.text = _unitModel.Tier.ToString();
        _priceButton.SetPrice(_unitModel.UnlockCost);
        _priceButton.gameObject.SetActive(!_unitModel.IsUnitOpened);
    }
    }
}