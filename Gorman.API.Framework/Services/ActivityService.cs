
namespace Gorman.API.Framework.Services {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using API.Domain;
    using Convertors;
    using RestSharp;
    using Validators;
    using Activity = Domain.Activity;
    using ApiActivity = API.Domain.Activity;
    using Map = Domain.Map;

    public interface IActivityService {
        Task<Activity> Add(Activity activity);
        Task<Collection<Activity>> List(Map map);
        Task<Collection<Activity>> List(long mapId);
    }

    public class ActivityService
        : BaseService, IActivityService {

        public ActivityService(Endpoints endpoints, IRestClient restClient, IResponseValidator responseValidator,
            IActivityConvertor activityConvertor, IAddActivityValidator addActivityValidator)
            : base(endpoints, restClient, responseValidator) {
            _activityConvertor = activityConvertor;
            _addActivityValidator = addActivityValidator;
        }

        public ActivityService(Endpoints endpoints)
            : base(endpoints) {
            _activityConvertor = new ActivityConvertor();
            _addActivityValidator = new AddActivityValidator();
        }

        public async Task<Activity> Add(Activity activity) {

            if (!_addActivityValidator.IsValidForAdd(activity))
                throw new Exception();

            var request = CreateRequest(Method.POST, _endpoints.ActivitiesUrl);
            request.AddParameter("activityId", "", ParameterType.UrlSegment);
            request.AddBody(activity);

            var restResponse = await _restClient.ExecuteTaskAsync<ApiActivity>(request);
            var response = _responseValidator.Validate(restResponse);
            return _activityConvertor.Convert(response);
        }

        public async Task<Activity> Get(long activityId, bool fullGraph = false) {
            var request = CreateRequest(Method.GET, _endpoints.ActivitiesUrl);
            request.AddParameter("activityId", activityId, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<ApiActivity>(request);
            var result = _responseValidator.Validate(restResponse);
            return _activityConvertor.Convert(result);
        }

        public async Task<Collection<Activity>> List(Map map) {
            return await List(map.Id);
        }

        public async Task<Collection<Activity>> List(long mapId) {
            var request = CreateRequest(Method.GET, _endpoints.MapActivitiesUrl);
            request.AddParameter("mapId", mapId, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<List<ApiActivity>>(request);
            var result = _responseValidator.Validate(restResponse);
            return _activityConvertor.Convert(result);
        }

        private RestRequest CreateRequest(Method method, string resource) {
            return new RestRequest(resource, method) {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new JsonSerializer()
        };
        }

        private readonly IActivityConvertor _activityConvertor;
        private readonly IAddActivityValidator _addActivityValidator;
    }
}