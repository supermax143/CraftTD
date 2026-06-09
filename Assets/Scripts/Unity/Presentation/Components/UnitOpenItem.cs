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

    public UnitTier Tier => _tier;

    public void SetUnit(UnitEntityInfo unit, bool opened)
    {
        _unitNameTF.text = unit.Tier.ToString();
        _priceButton.SetPrice(unit.Cost);
        _priceButton.gameObject.SetActive(!opened);
    }
    }
}