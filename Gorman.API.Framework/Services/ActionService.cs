
namespace Gorman.API.Framework.Services {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Convertors;
    using RestSharp;
    using Validators;
    using Action = Domain.Action;
    using ApiAction = API.Domain.Action;

    public interface IActionService
        : IBaseService {
        Task<Action> Add(Action action);
        Task<Action> Get(long activityId, long actionId, bool fullGraph = false);
        Task<Collection<Action>> List(long activityId);
    }

    public class ActionService
        : BaseService, IActionService {

        public ActionService(Endpoints endpoints)
            : base(new RequestBuilder(endpoints)) {
            _actionConvertor = new ActionConvertor();
            _addActionValidator = new AddActionValidator();
        }

        public ActionService(IRequestBuilder requestBuilder, IRestClient restClient, IResponseValidator responseValidator,
            IActionConvertor actionConvertor, IAddActionValidator addActionValidator)
            : base(requestBuilder, restClient, responseValidator) {
            _actionConvertor = actionConvertor;
            _addActionValidator = addActionValidator;
        }

        public async Task<Action> Add(Action action) {

            if (!_addActionValidator.IsValidForAdd(action))
                throw new Exception();

            var request = _requestBuilder.BuildAddActionRequest(action.ActivityId, action);
            var response = await _restClient.ExecuteTaskAsync<ApiAction>(request);
            _responseValidator.Validate(response);
            return _actionConvertor.Convert(response.Data);
        }

        public async Task<Action> Get(long activityId, long actionId, bool fullGraph = false) {
            var request = _requestBuilder.BuildGetActionRequest(activityId, actionId);
            var response = await _restClient.ExecuteTaskAsync<ApiAction>(request);
            _responseValidator.Validate(response);
            var apiAction = response.Data;
            var action = _actionConvertor.Convert(apiAction);

            if (!fullGraph)
                return action;

            return action;
        }

        public async Task<Collection<Action>> List(long mapId) {
            throw new NotImplementedException();
            var request = new JsonRestRequest(_requestBuilder.Endpoints.ActionsUrl, Method.GET)
                .AddUrlSegment("mapId", mapId.ToString());

            var restResponse = await _restClient.ExecuteTaskAsync<List<ApiAction>>(request);
            _responseValidator.Validate(restResponse);
            return _actionConvertor.Convert(restResponse.Data);
        }

        private readonly IActionConvertor _actionConvertor;
        private readonly IAddActionValidator _addActionValidator;
    }
}