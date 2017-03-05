
namespace Gorman.API.Framework.Services {
    using System;
    using System.Threading.Tasks;
    using Convertors;
    using Newtonsoft.Json;
    using RestSharp;
    using Validators;
    using Map = Domain.Map;

    public interface IMapService
        : IBaseService {
        Task<Map> Add(Map map);
        Task<Map> Get(long id);
    }

    public class MapService
        : BaseService, IMapService {

        public MapService(Endpoints endpoints)
            : base(endpoints) {
            _addMapValidator = new MapValidator();
            _mapConvertor = new MapConvertor();
        }

        public MapService(Endpoints endpoints, IRestClient restClient, IMapValidator mapValidator, IResponseValidator responseValidator, IMapConvertor mapConvertor)
            : base(endpoints, restClient, responseValidator) {
            _addMapValidator = mapValidator;
            _mapConvertor = mapConvertor;
        }

        //todo in the future let people get by GET /user/activity_name
        public async Task<Map> Get(long id) {

            var request = CreateRequest(Method.GET);
            request.AddUrlSegment("mapId", id.ToString());

            var restResponse = await _restClient.ExecuteTaskAsync<API.Domain.Map>(request);
            var map = _responseValidator.Validate(restResponse);
            return _mapConvertor.Convert(map);
        }

        public async Task<Map> Add(Map map) {

            if (!_addMapValidator.IsValidForAdd(map))
                throw new Exception();

            var request = new RestRequest("/maps", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            var serialisedMap = JsonConvert.SerializeObject(_mapConvertor.Convert(map));
            //request.AddJsonBody(_mapConvertor.Convert(map));
            request.AddParameter("application/json", serialisedMap, ParameterType.RequestBody);
            //request.AddUrlSegment("mapId", "");

            var restResponse = await _restClient.ExecuteTaskAsync<API.Domain.Map>(request);
            var response = _responseValidator.Validate(restResponse);
            return _mapConvertor.Convert(response);
        }

        private RestRequest CreateRequest(Method method) {
            return new RestRequest(_endpoints.MapsUrl, method) {
                RequestFormat = DataFormat.Json
            };
        }

        private readonly IMapValidator _addMapValidator;
        private readonly IMapConvertor _mapConvertor;

    }
}