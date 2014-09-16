﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Helpers
{
  public static class EngSymbolsAutoChanger
  {
    private static IDictionary<string, string> EnRusMap = 
      new Dictionary<string, string>
      {
        {"q", "й"}, {"Q", "Й"},
        {"w", "ц"}, {"W", "Ц"},
        {"e", "у"}, {"E", "У"},
        {"r", "к"}, {"R", "К"},
        {"t", "е"}, {"T", "Е"},
        {"y", "н"}, {"Y", "Н"},
        {"u", "г"}, {"U", "Г"},
        {"i", "ш"}, {"I", "Ш"},
        {"o", "щ"}, {"O", "Щ"},
        {"p", "з"}, {"P", "З"},
        {"[", "х"}, {"{", "Х"},
        {"]", "ъ"}, {"}", "Ъ"},
        {"a", "ф"}, {"A", "Ф"},
        {"s", "ы"}, {"S", "Ы"},
        {"d", "в"}, {"D", "В"},
        {"f", "а"}, {"F", "А"},
        {"g", "п"}, {"G", "П"},
        {"h", "р"}, {"H", "Р"},
        {"j", "о"}, {"J", "О"},
        {"k", "л"}, {"K", "Л"},
        {"l", "д"}, {"L", "Д"},
        {";", "ж"}, {":", "Ж"},
        {"'", "э"}, {"\"", "Э"},
        {"z", "я"}, {"Z", "Я"},
        {"x", "ч"}, {"X", "Ч"},
        {"c", "с"}, {"C", "С"},
        {"v", "м"}, {"V", "М"},
        {"b", "и"}, {"B", "И"},
        {"n", "т"}, {"N", "Т"},
        {"m", "ь"}, {"M", "Ь"},
        {",", "б"}, {"<", "Б"},
        {".", "ю"}, {">", "Ю"},
        {"`", "ё"}, {"~", "Ё"}
      };

    public static string GetRusSymbol(string symbol)
    {
      if (EnRusMap.ContainsKey(symbol))
      {
        return EnRusMap[symbol];
      }
      return symbol;
    }
  }
}
