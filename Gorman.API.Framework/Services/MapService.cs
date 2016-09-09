
namespace Gorman.API.Framework.Services {
    using System;
    using System.Threading.Tasks;
    using Convertors;
    using RestSharp;
    using Response = API.Domain.Response<API.Domain.Map>;
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
        }

        public MapService(Endpoints endpoints, IRestClient restClient, IMapValidator mapValidator, IResponseValidator responseValidator, IMapConvertor mapConvertor)
            : base(endpoints, restClient, responseValidator) {
            _addMapValidator = mapValidator;
            _mapConvertor = mapConvertor;
        }

        //todo in the future let people get by GET /user/activity_name
        public async Task<Map> Get(long id) {

            var request = CreateRequest(Method.GET);
            request.AddParameter("mapId", id, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<Response>(request);
            var map = _responseValidator.Validate(restResponse.Data);
            return _mapConvertor.Convert(map);
        }

        public async Task<Map> Add(Map map) {

            if (!_addMapValidator.IsValidForAdd(map))
                throw new Exception();

            var request = CreateRequest(Method.POST);
            request.AddBody(map);

            var restResponse = await _restClient.ExecuteTaskAsync<Response>(request);
            var response = _responseValidator.Validate(restResponse.Data);
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