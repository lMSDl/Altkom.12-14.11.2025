using Newtonsoft.Json;

namespace MyWebApi
{
    public partial class contractClient
    {
        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
        {
            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        }
    }
}
