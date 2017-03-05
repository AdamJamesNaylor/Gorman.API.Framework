
namespace Gorman.API.Framework.Services {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using API.Domain;
    using Convertors;
    using RestSharp;
    using Validators;
    using Action = Domain.Action;
    using ApiAction = API.Domain.Action;

    public interface IActionService
        : IBaseService {
        Task<Action> Add(Action action);
        Task<Collection<Action>> List(long activityId);
    }

    public class ActionService
        : BaseService, IActionService {

        public ActionService(Endpoints endpoints)
            : base(endpoints) {
        }

        public ActionService(Endpoints endpoints, IRestClient restClient, IResponseValidator responseValidator,
            IActionConvertor actionConvertor, IAddActionValidator addActionValidator)
            : base(endpoints, restClient, responseValidator) {
            _actionConvertor = actionConvertor;
            _addActionValidator = addActionValidator;
        }

        public async Task<Action> Add(Action action) {

            if (!_addActionValidator.IsValidForAdd(action))
                throw new Exception();

            var request = CreateRequest(Method.POST);
            request.AddParameter("activityId", action.ActivityId);
            request.AddBody(action);

            var restResponse = await _restClient.ExecuteTaskAsync<ApiAction>(request);
            var response = _responseValidator.Validate(restResponse);
            return _actionConvertor.Convert(response);
        }

        public async Task<Collection<Action>> List(long mapId) {

            var request = new RestRequest(_endpoints.MapActorsUrl, Method.GET) {
                RequestFormat = DataFormat.Json
            };

            request.AddParameter("mapId", mapId, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<List<ApiAction>>(request);
            var actions = _responseValidator.Validate(restResponse);
            return _actionConvertor.Convert(actions);
        }

        private RestRequest CreateRequest(Method method) {
            return new RestRequest(_endpoints.ActivityActionsUrl, method) {
                RequestFormat = DataFormat.Json
            };
        }

        private readonly IActionConvertor _actionConvertor;
        private readonly IAddActionValidator _addActionValidator;
    }
}