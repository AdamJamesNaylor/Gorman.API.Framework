
namespace Gorman.API.Framework.Services {
    using System;
    using System.Threading.Tasks;
    using Convertors;
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
            : base(new RequestBuilder(endpoints)) {
            _addMapValidator = new MapValidator();
            _mapConvertor = new MapConvertor();
        }

        public MapService(IRequestBuilder requestBuilder, IRestClient restClient, IMapValidator mapValidator, IResponseValidator responseValidator, IMapConvertor mapConvertor)
            : base(requestBuilder, restClient, responseValidator) {
            _addMapValidator = mapValidator;
            _mapConvertor = mapConvertor;
        }

        //todo in the future let people get by GET /user/activity_name
        public async Task<Map> Get(long id) {

            var request = _requestBuilder.BuildGetMapRequest(id);
            var response = await _restClient.ExecuteTaskAsync<API.Domain.Map>(request);
            _responseValidator.Validate(response);
            return _mapConvertor.Convert(response.Data);
        }

        public async Task<Map> Add(Map map) {

            if (!_addMapValidator.IsValidForAdd(map))
                throw new Exception();

            var request = _requestBuilder.BuildAddMapRequest(map);
            var response = await _restClient.ExecuteTaskAsync<API.Domain.Map>(request);
            _responseValidator.Validate(response);
            return _mapConvertor.Convert(response.Data);
        }

        private readonly IMapValidator _addMapValidator;
        private readonly IMapConvertor _mapConvertor;

    }
}