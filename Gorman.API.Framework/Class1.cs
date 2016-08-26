
namespace Gorman.API.Framework {
    using System;
    using System.Threading.Tasks;
    using RestSharp;

    public class MapService {

        public MapService(Uri domain) {
            _restClient = new RestClient(domain);
            _addMapValidator = new MapValidator();
        }

        public MapService(IRestClient restClient, IMapValidator mapValidator) {
            _restClient = restClient;
            _addMapValidator = mapValidator;
        }

        //todo in the future let people get by GET /user/activity_name
        public async void Get(int id) {
            //todo add initialisation code to build list of endpoints
            var mapsEndpoint = new Uri(MapsEndpoint.Replace("{/map_id}", id.ToString()));
            var request = new RestRequest(mapsEndpoint, Method.POST);
            var result = await _restClient.ExecuteTaskAsync<Map>(request);
            return new Response<Map>(result.Data);
        }

        public async Task<Response<Map>> Add(Map map) {
            if (!_addMapValidator.IsValidForAdd(map))
                return new Response<Map>();

            var request = new RestRequest("/maps", Method.POST);
            request.AddJsonBody(map);
            var result = await _restClient.ExecuteTaskAsync<Map>(request);
            return new Response<Map>(result.Data);
        }

        private readonly IRestClient _restClient;
        private readonly IMapValidator _addMapValidator;

        private const string MapsEndpoint = "/maps{/map_id}";
    }

    public class MapValidator
        : IMapValidator {

        public bool IsValidForAdd(Map map) {
            return true;
        }
    }

    public class Map {
        public int Id { get; set; }
        
    }

    public class Response<T> {
        public T Result { get; set; }

        public Response() { }

        public Response(T result) {
            Result = result;
        }
        
    }
    

}