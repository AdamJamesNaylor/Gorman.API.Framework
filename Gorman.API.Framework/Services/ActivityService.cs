
namespace Gorman.API.Framework.Services {
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Convertors;
    using RestSharp;
    using Validators;
    using Activity = Domain.Activity;
    using ApiActivity = API.Domain.Activity;
    using ApiAction = API.Domain.Action;

    public interface IActivityService {
        Task<Activity> Add(Activity activity);
        Task<Activity> Get(long activityId, bool fullGraph = false);
    }

        public class ActivityService
        : BaseService, IActivityService {

        public ActivityService(Endpoints endpoints)
            : base(new RequestBuilder(endpoints)) {
            _activityConvertor = new ActivityConvertor();
            _addActivityValidator = new AddActivityValidator();
            _actorService = new ActorService(endpoints);
            _actionService = new ActionService(endpoints);
            _actionConvertor = new ActionConvertor();
        }

        public ActivityService(IRequestBuilder requestBuilder, IRestClient restClient,
            IResponseValidator responseValidator,
            IActivityConvertor activityConvertor, IActionService actionService, IActionConvertor actionConvertor,
            IAddActivityValidator addActivityValidator, IActorService actorService)
            : base(requestBuilder, restClient, responseValidator) {
            _activityConvertor = activityConvertor;
            _actionService = actionService;
            _actionConvertor = actionConvertor;
            _addActivityValidator = addActivityValidator;
            _actorService = actorService;
        }

        public async Task<Activity> Add(Activity activity) {

            if (!_addActivityValidator.IsValidForAdd(activity))
                throw new Exception();

            var request = _requestBuilder.BuildAddActivityRequest(activity);
            var response = await _restClient.ExecuteTaskAsync<ApiActivity>(request);
            _responseValidator.Validate(response);
            activity.Id = response.Data.Id;

            await AddNestedActivities(activity);
            await AddActors(activity);
            await AddActions(activity);

            return _activityConvertor.Convert(response.Data);
        }

        public async Task<Activity> Get(long activityId, bool fullGraph = false) {
            var request = _requestBuilder.BuildGetActivityRequest(activityId);
            var response = await _restClient.ExecuteTaskAsync<ApiActivity>(request);
            _responseValidator.Validate(response);
            var apiActivity = response.Data;
            var activity = _activityConvertor.Convert(apiActivity);

            if (!fullGraph)
                return activity;

            foreach (var nestedActivity in apiActivity.Activities) {
                activity.Activities.Add(await Get(nestedActivity.Id, fullGraph));
            }

            foreach (var apiActorSummary in apiActivity.Actors) {
                activity.Actors.Add(await _actorService.Get(activity.Id, apiActorSummary.Id, fullGraph));
            }

            foreach (var apiActionSummary in apiActivity.Actions) {
                var apiAction = await GetAction(activityId, apiActionSummary.Id);
                var action = _actionConvertor.Convert(apiAction);
                action.Actor = activity.Actors.FirstOrDefault(a => a.Id == apiAction.ActorId);
                activity.Actions.Add(action);
            }

            return activity;
        }

        private async Task<ApiAction> GetAction(long activityId, long actionId) {
            var getActionRequest = _requestBuilder.BuildGetActionRequest(activityId, actionId);
            var getActionResponse = await _restClient.ExecuteTaskAsync<ApiAction>(getActionRequest);
            _responseValidator.Validate(getActionResponse);
            var apiAction = getActionResponse.Data;
            return apiAction;
        }

        private async Task AddActors(Activity activity) {
            if (activity.Actors == null || !activity.Actors.Any())
                return;

            foreach (var actor in activity.Actors) {
                actor.ActivityId = activity.Id;
                var persistedActor = await _actorService.Add(actor);
                actor.Id = persistedActor.Id;
            }
        }

        private async Task AddNestedActivities(Activity activity) {
            if (activity.Activities == null || !activity.Activities.Any())
                return;

            foreach (var child in activity.Activities) {
                child.ParentId = activity.Id;
                var persistedChild = await Add(child);
                child.Id = persistedChild.Id;
            }
        }

        private async Task AddActions(Activity activity) {
            if (activity.Actions == null || !activity.Actions.Any())
                return;

            foreach (var action in activity.Actions) {
                action.ActivityId = activity.Id;
                var persistedAction = await _actionService.Add(action);
                action.Id = persistedAction.Id;
            }
        }

        private readonly IActivityConvertor _activityConvertor;
        private readonly IActionService _actionService;
        private readonly IAddActivityValidator _addActivityValidator;
        private readonly IActorService _actorService;
        private readonly IActionConvertor _actionConvertor;
    }
}