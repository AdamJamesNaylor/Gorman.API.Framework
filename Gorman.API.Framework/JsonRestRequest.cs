
namespace Gorman.API.Framework {
    using Convertors;
    using RestSharp;

    public class JsonRestRequest
        : RestRequest {

        public JsonRestRequest(string resource, Method method)
            : base (resource, method) {
            RequestFormat = DataFormat.Json;
            JsonSerializer = new JsonSerializer();
        }
    }
}