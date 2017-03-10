
namespace Gorman.API.Framework.Services {
    using RestSharp;
    using Domain;

    public interface IRequestBuilder {
        Endpoints Endpoints { get; }
        JsonRestRequest BuildAddActionRequest(long activityId, Action action);
        JsonRestRequest BuildGetActionRequest(long activityId, long actionId);
        JsonRestRequest BuildAddActivityRequest(Activity activity);
        JsonRestRequest BuildGetActivityRequest(long activityId);
        JsonRestRequest BuildAddActorRequest(Actor actor);
        JsonRestRequest BuildGetActorRequest(long activityId, long actorId);
        JsonRestRequest BuildGetMapRequest(long mapId);
        JsonRestRequest BuildAddMapRequest(Map map);
    }

    public class RequestBuilder : IRequestBuilder {

        public RequestBuilder(Endpoints endpoints) {
            Endpoints = endpoints;
        }

        public JsonRestRequest BuildAddActionRequest(long activityId, Action action) {
            var request = new JsonRestRequest(Endpoints.ActionsUrl, Method.POST);
            request.AddUrlSegment("activityId", action.ActivityId.ToString());
            request.AddBody(action);
            return request;
        }

        public JsonRestRequest BuildGetActionRequest(long activityId, long actionId) {
            var request = new JsonRestRequest(Endpoints.ActionsUrl, Method.GET);
            request.AddUrlSegment("activityId", activityId.ToString());
            request.AddUrlSegment("actionId", actionId.ToString());
            return request;
        }

        public JsonRestRequest BuildAddActivityRequest(Activity activity) {
            var request = new JsonRestRequest(Endpoints.ActivitiesUrl, Method.POST);
            request.RemoveUrlSegment("activityId");
            request.AddBody(activity);
            return request;
        }

        public JsonRestRequest BuildGetActivityRequest(long activityId) {
            var request = new JsonRestRequest(Endpoints.ActivitiesUrl, Method.GET);
            request.AddUrlSegment("activityId", activityId.ToString());
            return request;
        }

        public JsonRestRequest BuildAddActorRequest(Actor actor) {
            var request = new JsonRestRequest(Endpoints.ActorsUrl, Method.POST);
            request.AddUrlSegment("activityId", actor.ActivityId.ToString());
            request.RemoveUrlSegment("actorId");
            request.AddBody(actor);
            return request;
        }

        public JsonRestRequest BuildGetActorRequest(long activityId, long actorId) {
            var request = new JsonRestRequest(Endpoints.ActorsUrl, Method.GET);
            request.AddUrlSegment("activityId", activityId.ToString());
            request.AddUrlSegment("actorId", actorId.ToString());
            return request;
        }

        public JsonRestRequest BuildGetMapRequest(long mapId) {
            var request = new JsonRestRequest(Endpoints.MapsUrl, Method.GET);
            request.AddUrlSegment("mapId", mapId.ToString());
            return request;
        }

        public JsonRestRequest BuildAddMapRequest(Map map) {
            var request = new JsonRestRequest(Endpoints.MapsUrl, Method.POST);
            request.RemoveUrlSegment("mapId");
            request.AddBody(map);
            return request;
        }

        public Endpoints Endpoints { get; }
    }
}