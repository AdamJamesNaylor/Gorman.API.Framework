
namespace Gorman.API.Framework.Services {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using API.Domain;
    using Convertors;
    using RestSharp;
    using Validators;
    using Actor = Domain.Actor;
    using ApiActor = API.Domain.Actor;

    public interface IActorService
        : IBaseService {
        Task<Actor> Add(Actor actor);
        Task<Collection<Actor>> List(long mapId);
    }

    public class ActorService
        : BaseService, IActorService {

        public ActorService(Endpoints endpoints)
            : base(endpoints) {
            _actorConvertor = new ActorConvertor();
            _addActorValidator = new AddActorValidator();
        }

        public ActorService(Endpoints endpoints, IRestClient restClient, IResponseValidator responseValidator,
            IActorConvertor actorConvertor, IAddActorValidator addActorValidator)
            : base(endpoints, restClient, responseValidator) {
            _actorConvertor = actorConvertor;
            _addActorValidator = addActorValidator;
        }

        public async Task<Actor> Add(Actor actor) {

            if (!_addActorValidator.IsValidForAdd(actor))
                throw new Exception();

            var request = new JsonRestRequest(_endpoints.ActorsUrl, Method.POST)
                .AddUrlSegment("activityId", actor.ActivityId.ToString())
                .RemoveUrlSegment("actorId")
                .AddBody(actor);

            var restResponse = await _restClient.ExecuteTaskAsync<ApiActor>(request);
            _responseValidator.Validate(restResponse);
            return _actorConvertor.Convert(restResponse.Data);
        }

        public async Task<Collection<Actor>> List(long mapId) {
            throw new NotImplementedException();

            var request = new RestRequest(_endpoints.ActorsUrl, Method.GET) {
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