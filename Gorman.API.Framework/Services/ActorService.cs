
namespace Gorman.API.Framework.Services {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Convertors;
    using RestSharp;
    using Validators;
    using Actor = Domain.Actor;
    using ApiActor = API.Domain.Actor;

    public interface IActorService
        : IBaseService {
        Task<Actor> Add(Actor actor);
        Task<Actor> Get(long activityId, long actorId, bool fullGraph = false);
        Task<Collection<Actor>> List(long mapId);
    }

    public class ActorService
        : BaseService, IActorService {

        public ActorService(Endpoints endpoints)
            : base(new RequestBuilder(endpoints)) {
            _actorConvertor = new ActorConvertor();
            _addActorValidator = new AddActorValidator();
        }

        public ActorService(IRequestBuilder requestBuilder, IRestClient restClient, IResponseValidator responseValidator,
            IActorConvertor actorConvertor, IAddActorValidator addActorValidator)
            : base(requestBuilder, restClient, responseValidator) {
            _actorConvertor = actorConvertor;
            _addActorValidator = addActorValidator;
        }

        public async Task<Actor> Add(Actor actor) {

            if (!_addActorValidator.IsValidForAdd(actor))
                throw new Exception();

            var request = _requestBuilder.BuildAddActorRequest(actor);
            var response = await _restClient.ExecuteTaskAsync<ApiActor>(request);
            _responseValidator.Validate(response);
            return _actorConvertor.Convert(response.Data);
        }

        public async Task<Actor> Get(long activityId, long actorId, bool fullGraph = false) {
            var request = _requestBuilder.BuildGetActorRequest(activityId, actorId);
            var response = await _restClient.ExecuteTaskAsync<ApiActor>(request);
            _responseValidator.Validate(response);
            var apiActor = response.Data;
            var activity = _actorConvertor.Convert(apiActor);

            if (!fullGraph)
                return activity;

            return activity;
        }


        public async Task<Collection<Actor>> List(long mapId) {
            throw new NotImplementedException();

            var request = new RestRequest(_requestBuilder.Endpoints.ActorsUrl, Method.GET) {
                RequestFormat = DataFormat.Json
            };

            request.AddParameter("mapId", mapId, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<List<ApiActor>>(request);
            _responseValidator.Validate(restResponse);
            return _actorConvertor.Convert(restResponse.Data);
        }


        private readonly IActorConvertor _actorConvertor;
        private readonly IAddActorValidator _addActorValidator;
    }
}