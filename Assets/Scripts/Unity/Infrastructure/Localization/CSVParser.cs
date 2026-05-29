using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Unity.Infrastructure.Localization
{
   public class CSVParser
   {
      private const char SEPARATOR = '|';
      
      public static bool TryParse(string csvText, out ParseDto data)
      {
        data = default;

        if (string.IsNullOrWhiteSpace(csvText))
        {
            Debug.LogError("[LocalizationController] Пустой текст CSV. Нечего загружать.");
            return false;
        }

        using var reader = new StringReader(csvText);

        var allLanguages = new Dictionary<string, Dictionary<string, string>>();
        var languageCodes = new List<string>();

        string headerLine = reader.ReadLine();
        if (string.IsNullOrWhiteSpace(headerLine))
        {
            Debug.LogError("[LocalizationController] Первая строка CSV пуста. Ожидается заголовок: key;ru;en;...");
            return false;
        }

        string[] headers = SplitCsvLine(headerLine);
        if (headers.Length < 2 || !string.Equals(headers[0], "key", StringComparison.OrdinalIgnoreCase))
        {
            Debug.LogError("[LocalizationController] Первая колонка должна называться 'key'. Текущая строка: " + headerLine);
            return false;
        }

        // Список языков из заголовка, начиная со второго столбца.
        for (int i = 1; i < headers.Length; i++)
        {
            string langCode = headers[i].Trim();
            if (string.IsNullOrEmpty(langCode))
                continue;

            if (!allLanguages.ContainsKey(langCode))
            {
                allLanguages[langCode] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                languageCodes.Add(langCode);
            }
        }

        if (languageCodes.Count == 0)
        {
            Debug.LogError("[LocalizationController] Не найдено ни одного языка в заголовке.");
            return false;
        }

        // Читаем строки с данными.
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] columns = SplitCsvLine(line);
            if (columns.Length == 0)
                continue;

            string key = columns[0].Trim();
            if (string.IsNullOrEmpty(key))
                continue;

            for (int i = 1; i < headers.Length && i < columns.Length; i++)
            {
                string langCode = headers[i].Trim();
                if (string.IsNullOrEmpty(langCode))
                    continue;

                string value = columns[i].Trim();
                if (!allLanguages.TryGetValue(langCode, out var table))
                    continue;

                // Последнее значение для ключа перезапишет предыдущее — это нормально.
                table[key] = value;
            }
        }

        data = new ParseDto()
        {
            LanguageCodes = languageCodes,
            AllLanguages = allLanguages
        };
        
        return true;
      }
      
      private static string[] SplitCsvLine(string line)
      {
          if (line == null)
              return Array.Empty<string>();

          var result = new List<string>();
          var sb = new StringBuilder();
          bool inQuotes = false;

          for (int i = 0; i < line.Length; i++)
          {
              char c = line[i];

              if (c == '"')
              {
                  // Двойные кавычки внутри поля ("") интерпретируем как один символ ".
                  if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                  {
                      sb.Append('"');
                      i++;
                  }
                  else
                  {
                      inQuotes = !inQuotes;
                  }
              }
              else if (c == SEPARATOR && !inQuotes)
              {
                  result.Add(sb.ToString());
                  sb.Clear();
              }
              else
              {
                  sb.Append(c);
              }
          }

          result.Add(sb.ToString());
          return result.ToArray();
      }
      
      public struct ParseDto
      {
          public Dictionary<string, Dictionary<string, string>> AllLanguages;
          public List<string> LanguageCodes;
      }
   }
}