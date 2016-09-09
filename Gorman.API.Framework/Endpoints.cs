
namespace Gorman.API.Framework {
    using System;
    using System.Threading.Tasks;
    using API.Domain;
    using RestSharp;

    public class Endpoints {

        public Uri BaseUrl { get; private set; }
        public string MapsUrl { get; set; }
        public string ActivitiesUrl { get; set; }
        public string ActorsUrl { get; set; }
        public string ActionsUrl { get; set; }
        public string MapActivitiesUrl => "/maps/{mapId}/activities";
        public string MapActorsUrl => "/maps/{mapId}/actors";

        public static async Task<Endpoints> Get(Uri baseUrl) {
            return await Get(new RestClient(baseUrl), new ResponseValidator());
        }

        public static async Task<Endpoints> Get(IRestClient restClient, IResponseValidator responseValidator) {
            var request = new RestRequest();

            var response = await restClient.ExecuteTaskAsync<EndpointListResponse>(request);

            var endpoints = Convert(response.Data);
            endpoints.BaseUrl = restClient.BaseUrl;
            return endpoints;
        }

        private Endpoints() { }

        private static Endpoints Convert(EndpointListResponse response) {
            return new Endpoints {
                MapsUrl = response.MapsUrl,
                ActivitiesUrl = response.ActivitiesUrl,
                ActorsUrl = response.ActorsUrl,
                ActionsUrl = response.ActionsUrl
            };
        }
    }
}