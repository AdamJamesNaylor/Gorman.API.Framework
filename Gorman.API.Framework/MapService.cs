
namespace Gorman.API.Framework {
    using System;
    using System.Threading.Tasks;
    using Domain;
    using RestSharp;

    public class MapService
        : BaseService {

        public MapService(Uri domain) : base(domain) {
            _addMapValidator = new MapValidator();
        }

        public MapService(IRestClient restClient, IMapValidator mapValidator) : base(restClient) {
            _addMapValidator = mapValidator;
        }

        //todo in the future let people get by GET /user/activity_name
        public async Task<Map> Get(int id) {
            if (!IsInitialised)
                Initialise();

            var mapsEndpoint = new Uri(_endpoints["map_url"].Replace("{/map_id}", id.ToString()));
            var request = new RestRequest(mapsEndpoint, Method.POST);
            var result = await _restClient.ExecuteTaskAsync<Map>(request);
            return result.Data;
        }

        public async Task<Map> Add(Map map) {
            if (!_addMapValidator.IsValidForAdd(map))
                throw new Exception(); //have to throw because of return type

            var request = new RestRequest("/maps", Method.POST);
            request.AddJsonBody(map);
            var result = await _restClient.ExecuteTaskAsync<Map>(request);
            return result.Data;
        }

        private readonly IMapValidator _addMapValidator;
    }
}