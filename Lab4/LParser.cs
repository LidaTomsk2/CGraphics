using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Lab4
{
    public class LParser
    {
        private static readonly char[] Ignore = {'+', '-', '[', ']'};

        /// <summary>
        /// Получить результирующую строку
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="deepValue">глубина рекурсии</param>
        /// <returns></returns>
        public static string GetCombinedParsedString(List<LSystemRule> rules, int deepValue)
        {
            var dict = rules.ToDictionary(x => x.Name, x => x.Value);

            var resultString = dict['S'];
            for (int i = 0; i < deepValue; i++)
            {
                var curStrBuilder = new StringBuilder();
                foreach (var curSymb in resultString)
                {
                    // пропускаемые символы
                    if (Ignore.Contains(curSymb))
                    {
                        curStrBuilder.Append(curSymb);
                    }
                    else if (dict.ContainsKey(curSymb)) //  заменить
                    {
                        curStrBuilder.Append(dict[curSymb]);
                    }
                    else // просто дописываем
                    {
                        curStrBuilder.Append(curSymb);
                    }
                }
                resultString = curStrBuilder.ToString();
            }

            return resultString;
        }

        public static IEnumerable<LSystemModel> ParseFiles(string path)
        {
            var serializer = new XmlSerializer(typeof(LSystemModel));
            foreach (var enumerateFile in Directory.EnumerateFiles(path))
            {
                using (var reader = new StreamReader(enumerateFile))
                {
                    yield return (LSystemModel) serializer.Deserialize(reader);
                }
            }
        }
    }
}
