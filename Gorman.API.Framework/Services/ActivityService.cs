
namespace Gorman.API.Framework.Services {
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Convertors;
    using Domain;
    using RestSharp;
    using Response = API.Domain.Response<System.Collections.Generic.List<API.Domain.Activity>>; //todo should this be a list?

    public interface IActivityService {
        Task<Collection<Activity>> List(Map map);
        Task<Collection<Activity>> List(long mapId);
    }

    public class ActivityService
        : BaseService, IActivityService {

        public ActivityService(Endpoints endpoints, IRestClient restClient, IResponseValidator responseValidator,
            IActivityConvertor activityConvertor)
            : base(endpoints, restClient, responseValidator) {
            _activityConvertor = activityConvertor;
        }

        public ActivityService(Endpoints endpoints)
            : base(endpoints) {

        }

        public async Task<Collection<Activity>> List(Map map) {
            return await List(map.Id);
        }

        public async Task<Collection<Activity>> List(long mapId) {
            var request = CreateRequest(Method.GET);
            request.AddParameter("mapId", mapId, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<Response>(request);
            var result = _responseValidator.Validate(restResponse.Data);
            return _activityConvertor.Convert(result);
        }

        private RestRequest CreateRequest(Method method) {
            return new RestRequest(_endpoints.MapActivitiesUrl, method) {
                RequestFormat = DataFormat.Json
            };
        }

        private readonly IActivityConvertor _activityConvertor;
    }
}