
namespace Gorman.API.Framework.Services {
    using System;
    using System.Threading.Tasks;
    using Convertors;
    using RestSharp;
    using Response = API.Domain.Response<API.Domain.Map>;
    using Map = Domain.Map;

    public interface IMapService
        : IBaseService {
        Task<Map> Get(long id);
    }

    public class MapService
        : BaseService, IMapService {

        public MapService(Uri domain)
            : base(domain) {
            _addMapValidator = new MapValidator();
        }

        public MapService(IRestClient restClient, IMapValidator mapValidator, IResponseValidator responseValidator,
            IMapConvertor mapConvertor)
            : base(restClient, responseValidator) {
            _addMapValidator = mapValidator;
            _mapConvertor = mapConvertor;
        }

        //todo in the future let people get by GET /user/activity_name
        public async Task<Map> Get(long id) {
            await Initialise();

            var request = CreateRequest(Method.GET);
            request.AddParameter("mapId", id, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<Response>(request);
            var map = _responseValidator.Validate(restResponse.Data);
            return _mapConvertor.Convert(map);
        }

        //public async Task<Map> Add(Map map) {
        //    await Initialise();

        //    if (!_addMapValidator.IsValidForAdd(map))
        //        throw new Exception();

        //    var request = CreateRequest(Method.POST);
        //    request.AddBody(map);

        //    var restResponse = await _restClient.ExecuteTaskAsync<Response<Map>>(request);
        //    var response = _responseValidator.Validate(restResponse.Data);
        //    return response;
        //}

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
        private readonly IMapConvertor _mapConvertor;
        private Uri _mapsEndpoint;
    }
}