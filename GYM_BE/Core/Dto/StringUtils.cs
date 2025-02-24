using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace GYM_BE.Core.Dto
{
    public static class StringUtils
    {
        public static string SnakeToCamelCase(this string name)
        {
            try
            {
                string[] array = name.Split('_');
                if (array.Length == 1)
                {
                    return JsonNamingPolicy.CamelCase.ConvertName(name);
                }

                if (array.Length > 1)
                {
                    array[0] = array[0].ToLower();
                    for (int i = 1; i < array.Length; i++)
                    {
                        string text = array[i].ToLower();
                        int num = i;
                        string text2 = text.Substring(0, 1).ToUpper();
                        string text3 = text;
                        array[num] = text2 + text3.Substring(1, text3.Length - 1);
                    }
                }

                return array.Aggregate("", (string acc, string x) => acc + x);
            }
            catch (Exception)
            {
                return name;
            }
        }

        public static string CamelOrPascalToSnackCase(this string name)
        {
            return new SnakeCaseNamingStrategy().GetPropertyName(name, hasSpecifiedName: false).ToUpper();
        }

        public static string CamelToPascalCase(this string name)
        {
            string text = name.Substring(0, 1).ToUpper();
            string text2 = name.Substring(1, name.Length - 1);
            return text + text2;
        }
    }
}
