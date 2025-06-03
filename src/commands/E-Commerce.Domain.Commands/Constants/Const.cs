using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.Domain.Constants
{
    public class Const
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}
