using Newtonsoft.Json.Serialization;

namespace GYM_BE.Core.Dto
{
    public class SnackToCamelCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.SnakeToCamelCase();
        }
    }
}
