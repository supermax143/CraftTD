using System;
using System.Collections.Generic;
using Core.Application.Info.Shop;
using Unity.Game.Attributes;

namespace Core.Application.Interfaces
{
   public interface ILocalization
   {
      event Action<string> LanguageChanged;
      string Get(string key);
      bool Initialized { get; }
      string CurrentLanguageCode { get; }
      List<string> LanguageCodes { get; }
      void SetLanguage(string languageCode);
      bool TryGetLanguageCodes(out IEnumerable<string> codes);

      string GetAttributeLocale(GameEntityAttributeKind kind);
      string GetShopItemLabel(ShopItemConfig shopItem);
      string GetShopItemDescription(ShopItemConfig shopItem);
   }
}