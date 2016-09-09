
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
        }

        public async Task<Activity> Add(Activity activity) {

            if (!_addActivityValidator.IsValidForAdd(activity))
                throw new Exception();

            var request = CreateRequest(Method.POST);
            request.AddParameter("mapId", activity.MapId);
            request.AddBody(activity);

            var restResponse = await _restClient.ExecuteTaskAsync<Response<ApiActivity>>(request);
            var response = _responseValidator.Validate(restResponse.Data);
            return _activityConvertor.Convert(response);
        }
        
        public async Task<Collection<Activity>> List(Map map) {
            return await List(map.Id);
        }

        public async Task<Collection<Activity>> List(long mapId) {
            var request = CreateRequest(Method.GET);
            request.AddParameter("mapId", mapId, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<Response<List<ApiActivity>>>(request);
            var result = _responseValidator.Validate(restResponse.Data);
            return _activityConvertor.Convert(result);
        }

        private RestRequest CreateRequest(Method method) {
            return new RestRequest(_endpoints.MapActivitiesUrl, method) {
                RequestFormat = DataFormat.Json
            };
        }

        private readonly IActivityConvertor _activityConvertor;
        private readonly IAddActivityValidator _addActivityValidator;
    }
}