
namespace Gorman.API.Framework {
    using System;
    using System.Threading.Tasks;
    using API.Domain;
    using RestSharp;
    using Validators;

    public class Endpoints {

        public Uri BaseUrl { get; private set; }
        public string MapsUrl { get; set; }
        public string ActivitiesUrl { get; set; }
        public string ActorsUrl { get; set; }
        public string ActionsUrl { get; set; }
        public string MapActivitiesUrl => "/maps/{mapId}/activities";
        public string MapActorsUrl => "/maps/{mapId}/actors";
        public string ActivityActionsUrl => "activities/{activityId}/actions";

        public static async Task<Endpoints> Get(Uri baseUrl) {
            return await Get(new RestClient(baseUrl), new ResponseValidator());
        }

        public static async Task<Endpoints> Get(IRestClient restClient, IResponseValidator responseValidator) {
            var request = new RestRequest();

            var response = await restClient.ExecuteTaskAsync<Response<EndpointList>>(request);
            var endpointList = responseValidator.Validate(response);
            var endpoints = Convert(endpointList);
            endpoints.BaseUrl = restClient.BaseUrl;
            return endpoints;
        }

        private Endpoints() { }

        private static Endpoints Convert(EndpointList response) {
            return new Endpoints {
                MapsUrl = response.MapsUrl,
                ActivitiesUrl = response.ActivitiesUrl,
                ActorsUrl = response.ActorsUrl,
                ActionsUrl = response.ActionsUrl
            };
        }
    }
}