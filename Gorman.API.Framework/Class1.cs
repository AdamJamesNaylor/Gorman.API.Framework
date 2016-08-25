
namespace Gorman.API.Framework {
    using RestSharp;

    public class MapService {

        public MapService(IRestClient restClient, IMapValidator mapValidator) {
            _restClient = restClient;
            _addMapValidator = mapValidator;
        }

        public async Response Add(Map map) {
            if (!_addMapValidator.IsValidForAdd(map))
                return;

            var request = new RestRequest("/maps", Method.POST);
            request.AddJsonBody(map);
            var result = await _restClient.ExecuteTaskAsync<Map>(request);
        }

        private readonly IRestClient _restClient;
        private readonly IMapValidator _addMapValidator;
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

    public class Response {
        
    }
    

}