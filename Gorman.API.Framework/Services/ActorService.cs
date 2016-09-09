
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

            var request = CreateRequest(Method.POST);
            request.AddParameter("mapId", actor.MapId);
            request.AddBody(actor);

            var restResponse = await _restClient.ExecuteTaskAsync<Response<ApiActor>>(request);
            var response = _responseValidator.Validate(restResponse.Data);
            return _actorConvertor.Convert(response);
        }

        public async Task<Collection<Actor>> List(long mapId) {

            var request = new RestRequest(_endpoints.MapActorsUrl, Method.GET) {
                RequestFormat = DataFormat.Json
            };

            request.AddParameter("mapId", mapId, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<Response<List<ApiActor>>>(request);
            var actors = _responseValidator.Validate(restResponse.Data);
            return _actorConvertor.Convert(actors);
        }

        private RestRequest CreateRequest(Method method) {
            return new RestRequest(_endpoints.MapActorsUrl, method) {
                RequestFormat = DataFormat.Json
            };
        }

        private readonly IActorConvertor _actorConvertor;
        private readonly IAddActorValidator _addActorValidator;
    }
}