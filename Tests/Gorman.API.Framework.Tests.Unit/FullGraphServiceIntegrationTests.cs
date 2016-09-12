namespace Gorman.API.Framework.Tests.Unit {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using API.Domain;
    using Convertors;
    using Moq;
    using RestSharp;
    using Services;
    using Validators;
    using Xunit;
    using ApiMap = API.Domain.Map;
    using ApiActivity = API.Domain.Activity;
    using ApiActor = API.Domain.Actor;
    using ApiAction = API.Domain.Action;

    public class FullGraphServiceIntegrationTests {

        [Fact]
        public async Task GetFullGraph() {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(r => r.BaseUrl).Returns(new Uri("http://www.blah.com"));

            var responseValidator = new ResponseValidator();

            restClient.Setup(r => r.ExecuteTaskAsync<Response<EndpointList>>(It.IsAny<IRestRequest>()))
                .Returns(() => CreateMockResponse<Response<EndpointList>>(response => response.Data.Data = new EndpointList { MapsUrl = "/blah" }).ToTask());

            var endpoints = await Endpoints.Get(restClient.Object, responseValidator);

            var mapValidator = new MapValidator();
            var mapConvertor = new MapConvertor();
            var mapService = new MapService(endpoints, restClient.Object, mapValidator, responseValidator, mapConvertor);

            restClient.Setup(r => r.ExecuteTaskAsync<Response<ApiMap>>(It.IsAny<IRestRequest>()))
                .Returns((IRestRequest r) => CreateMockResponse<Response<ApiMap>>(response => response.Data.Data = new ApiMap { Id = r.FindParameter<int>("mapId") }).ToTask());

            var addActivityValidator = new AddActivityValidator();
            var activityConvertor = new ActivityConvertor();
            var activityService = new ActivityService(endpoints, restClient.Object, responseValidator, activityConvertor, addActivityValidator);

            restClient.Setup(r => r.ExecuteTaskAsync<Response<List<ApiActivity>>>(It.IsAny<IRestRequest>()))
                .Returns((IRestRequest r) => CreateMockResponse<Response<List<ApiActivity>>>(response => response.Data.Data = new List<ApiActivity> {new ApiActivity { Id = 456 }, new ApiActivity()}).ToTask());

            var addActorValidator = new AddActorValidator();
            var actorConvertor = new ActorConvertor();
            var actorService = new ActorService(endpoints, restClient.Object, responseValidator, actorConvertor, addActorValidator);

            restClient.Setup(r => r.ExecuteTaskAsync<Response<List<ApiActor>>>(It.IsAny<IRestRequest>()))
                .Returns((IRestRequest r) => CreateMockResponse<Response<List<ApiActor>>>(response => response.Data.Data = new List<ApiActor> { new ApiActor(), new ApiActor(), new ApiActor() }).ToTask());

            var addActionValidator = new AddActionValidator();
            var actionConvertor = new ActionConvertor();
            var actionService = new ActionService(endpoints, restClient.Object, responseValidator, actionConvertor, addActionValidator);

            restClient.Setup(r => r.ExecuteTaskAsync<Response<List<ApiAction>>>(It.IsAny<IRestRequest>()))
                .Returns((IRestRequest r) => CreateMockResponse<Response<List<ApiAction>>>(response => response.Data.Data = new List<ApiAction>()).ToTask());

            restClient.Setup(r => r.ExecuteTaskAsync<Response<List<ApiAction>>>(It.Is<IRestRequest>(request => request.Parameters.Exists(p => (long)p.Value == 456))))
                .Returns((IRestRequest r) => CreateMockResponse<Response<List<ApiAction>>>(response => response.Data.Data = new List<ApiAction> { new ApiAction(), new ApiAction(), new ApiAction(), new ApiAction() }).ToTask());

            var graphService = new FullGraphService(endpoints, restClient.Object, responseValidator, mapService,
                activityService, actorService, actionService);

            const long mapId = 123;
            var map = await graphService.Get(mapId);

            Assert.Equal(mapId, map.Id);
            Assert.Equal(2, map.Activities.Count);
            Assert.Equal(3, map.Actors.Count);
            Assert.True(map.Activities.First(a => a.Id == 456).Actions.Count == 4);
            var allActivitiesHaveNoActionsExcept456 = map.Activities.Where(a => a.Id != 456).All(a => !a.Actions.Any());
            Assert.True(allActivitiesHaveNoActionsExcept456);
        }


        private static IRestResponse<T> CreateMockResponse<T>(Action<IRestResponse<T>> callback = null)
            where T : new() {

            var result = new RestResponse<T> {
                ResponseStatus = ResponseStatus.Completed,
                Data = new T()
            };
            callback?.Invoke(result);
            return result;
        }
    }
}