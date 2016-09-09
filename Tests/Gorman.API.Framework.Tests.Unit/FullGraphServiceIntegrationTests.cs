namespace Gorman.API.Framework.Tests.Unit {
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using API.Domain;
    using Convertors;
    using Moq;
    using RestSharp;
    using Services;
    using Xunit;
    using ApiMap = API.Domain.Map;
    using ApiActivity = API.Domain.Activity;

    public class FullGraphServiceIntegrationTests {

        [Fact]
        public async Task GetFullGraph() {
            var restClient = new Mock<IRestClient>();

            restClient.Setup(r => r.BaseUrl).Returns(new Uri("http://www.blah.com"));

            restClient.Setup(r => r.ExecuteTaskAsync<EndpointListResponse>(It.IsAny<IRestRequest>()))
                .Returns(() => new RestResponse<EndpointListResponse> {Data = new EndpointListResponse {MapsUrl = "/blah"}}.ToTask());

            restClient.Setup(r => r.ExecuteTaskAsync<Response<ApiMap>>(It.IsAny<IRestRequest>()))
                .Returns((IRestRequest r) => CreateMockResponse<Response<ApiMap>>(response => response.Data.Data = new ApiMap { Id = r.FindParameter<int>("mapId") }).ToTask());

            restClient.Setup(r => r.ExecuteTaskAsync<Response<List<ApiActivity>>>(It.IsAny<IRestRequest>()))
                .Returns((IRestRequest r) => CreateMockResponse<Response<List<ApiActivity>>>(response => response.Data.Data = new List<ApiActivity> {new ApiActivity(), new ApiActivity()}).ToTask());

            restClient.Setup(r => r.ExecuteTaskAsync<EndpointListResponse>(It.IsAny<IRestRequest>()))
                .Returns((IRestRequest r) => CreateMockResponse<EndpointListResponse>().ToTask());

            var responseValidator = new ResponseValidator();

            var endpoints = await Endpoints.Get(restClient.Object, responseValidator);

            var mapValidator = new MapValidator();
            var mapConvertor = new MapConvertor();
            var mapService = new MapService(endpoints, restClient.Object, mapValidator, responseValidator, mapConvertor);

            var activityConvertor = new ActivityConvertor();
            var activityService = new ActivityService(endpoints, restClient.Object, responseValidator, activityConvertor);

            var graphService = new FullGraphService(endpoints, restClient.Object, responseValidator, mapService,
                activityService);

            const long mapId = 123;
            var map = await graphService.Get(mapId);

            Assert.Equal(mapId, map.Id);
            Assert.Equal(2, map.Activities.Count);
        }


        private static IRestResponse<T> CreateMockResponse<T>(Action<IRestResponse<T>> callback = null)
            where T : new() {

            var result = new RestResponse<T> {
                Data = new T()
            };
            callback?.Invoke(result);
            return result;
        }
    }
}