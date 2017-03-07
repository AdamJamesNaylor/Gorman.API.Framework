
namespace Gorman.API.Framework.Services {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
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

        public ActivityService(Endpoints endpoints)
            : base(endpoints) {
            _activityConvertor = new ActivityConvertor();
            _addActivityValidator = new AddActivityValidator();
            _actionService = new ActionService(endpoints);
            _actorService = new ActorService(endpoints);
        }

        public ActivityService(Endpoints endpoints, IRestClient restClient, IResponseValidator responseValidator,
            IActivityConvertor activityConvertor, IAddActivityValidator addActivityValidator, IActionService actionService, IActorService actorService)
            : base(endpoints, restClient, responseValidator) {
            _activityConvertor = activityConvertor;
            _addActivityValidator = addActivityValidator;
            _actionService = actionService;
            _actorService = actorService;
        }

        public async Task<Activity> Add(Activity activity) {

            if (!_addActivityValidator.IsValidForAdd(activity))
                throw new Exception();

            var request = new JsonRestRequest(_endpoints.ActivitiesUrl, Method.POST)
                .RemoveUrlSegment("activityId")
                .AddBody(activity);

            var restResponse = await _restClient.ExecuteTaskAsync<ApiActivity>(request);
            _responseValidator.Validate(restResponse);
            activity.Id = restResponse.Data.Id;

            AddNestedActivities(activity);
            AddActors(activity);

            return _activityConvertor.Convert(restResponse.Data);
        }

        public async Task<Activity> Get(long activityId, bool fullGraph = false) {
            var request = new JsonRestRequest(_endpoints.ActivitiesUrl, Method.GET)
                .AddUrlSegment("activityId", activityId.ToString());

            var restResponse = await _restClient.ExecuteTaskAsync<ApiActivity>(request);
            _responseValidator.Validate(restResponse);

            return _activityConvertor.Convert(restResponse.Data);
        }

        public async Task<Collection<Activity>> List(Map map) {
            return await List(map.Id);
        }

        public async Task<Collection<Activity>> List(long mapId) {
            throw new NotImplementedException();
            var request = new JsonRestRequest(_endpoints.ActivitiesUrl, Method.GET);
            request.AddParameter("mapId", mapId, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<List<ApiActivity>>(request);
            _responseValidator.Validate(restResponse);
            return _activityConvertor.Convert(restResponse.Data);
        }

        private void AddActors(Activity activity) {
            if (activity.Actors == null || !activity.Actors.Any())
                return;

            foreach (var actor in activity.Actors) {
                actor.ActivityId = activity.Id;
                var persistedActor = _actorService.Add(actor);
                actor.Id = persistedActor.Id;
            }
        }

        private void AddNestedActivities(Activity activity) {
            if (activity.Activities == null || !activity.Activities.Any())
                return;

            foreach (var child in activity.Activities) {
                child.ParentId = activity.Id;
                var persistedChild = Add(child);
                child.Id = persistedChild.Id;
            }
        }

        private readonly IActivityConvertor _activityConvertor;
        private readonly IAddActivityValidator _addActivityValidator;
        private readonly IActionService _actionService;
        private readonly IActorService _actorService;
    }
}