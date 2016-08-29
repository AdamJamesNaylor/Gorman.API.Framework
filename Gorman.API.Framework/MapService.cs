
namespace Gorman.API.Framework {
    using System;
    using System.Threading.Tasks;
    using Domain;
    using RestSharp;

    public class MapService
        : BaseService {

        public MapService(Uri domain)
            : base(domain) {
            _addMapValidator = new MapValidator();
        }

        public MapService(IRestClient restClient, IMapValidator mapValidator, IResponseValidator responseValidator)
            : base(restClient, responseValidator) {
            _addMapValidator = mapValidator;
        }

        //todo in the future let people get by GET /user/activity_name
        public async Task<Map> Get(int id) {
            await Initialise();

            var request = CreateRequest(Method.GET);
            request.AddParameter("mapId", id, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<Response<Map>>(request);
            var map = _responseValidator.Validate(restResponse.Data);
            return map;
        }

        public async Task<Map> Add(Map map) {
            await Initialise();

            if (!_addMapValidator.IsValidForAdd(map))
                throw new Exception();

            var request = CreateRequest(Method.POST);
            request.AddBody(map);

            var restResponse = await _restClient.ExecuteTaskAsync<Response<Map>>(request);
            var response = _responseValidator.Validate(restResponse.Data);
            return response;
        }

        private RestRequest CreateRequest(Method method) {
            return new RestRequest(_mapsEndpoint, method) {
                RequestFormat = DataFormat.Json
            };
        }

        protected override async Task Initialise() {
            if (IsInitialised)
                return;

            await base.Initialise();
            _mapsEndpoint = new Uri(_endpoints["map_url"]);
        }

        private readonly IMapValidator _addMapValidator;
        private Uri _mapsEndpoint;
    }

}