﻿
using RestSharp;

namespace Gorman.API.Framework.Tests.Unit {
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using API.Domain;
    using Convertors;
    using Moq;
    using Newtonsoft.Json;
    using RestSharp;
    using Services;
    using Validators;
    using Xunit;
    using Map = Domain.Map;

    public class MapServiceIntegrationTests {

        [Fact]
        public async Task AddMap() {
            Assert.True(false, "Requires refactored.");
            var restClient = new Mock<IRestClient>();

            restClient.Setup(r => r.BaseUrl).Returns(new Uri("http://www.blah.com"));

            restClient.Setup(r => r.ExecuteTaskAsync<EndpointList>(It.IsAny<IRestRequest>()))
                .Returns(() => CreateMockResponse<EndpointList>(response => response.Data = new EndpointList { MapsUrl = "/blah"}).ToTask());

            restClient.Setup(r => r.ExecuteTaskAsync<API.Domain.Map>(It.IsAny<IRestRequest>()))
                .Returns((IRestRequest r) => CreateMockResponse<API.Domain.Map>(response => response.Data = new API.Domain.Map {Id = (r.GetBody<Map>()).Id}).ToTask());

            var responseValidator = new ResponseValidator();

            var endpoints = await Endpoints.Get(restClient.Object, responseValidator);

            var mapValidator = new MapValidator();
            var mapConvertor = new MapConvertor();

            var mapService = new MapService(new RequestBuilder(endpoints), restClient.Object, mapValidator, responseValidator, mapConvertor);

            var map = new Map {
                Id = 123
            };

            var result = await mapService.Add(map);

            Assert.Equal(map.Id, result.Id);
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

    public static class IRestRequestExtensions {
        public static T GetBody<T>(this IRestRequest r) {
            var parameter = r.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);
            if (parameter == null)
                throw new InvalidOperationException("No body could be found in provided request object.");
            return JsonConvert.DeserializeObject<T>(parameter.Value.ToString());
        }

        public static T FindParameter<T>(this IRestRequest r, string name) {
            var parameter = r.Parameters.Find(p => p.Name == name);
            if (parameter == null)
                throw new InvalidOperationException($"No parameter could be found in provided request object with name {name}.");
            return JsonConvert.DeserializeObject<T>(parameter.Value.ToString());
        }


    }

    public static class IRestResponseExtensions {
        public static Task<IRestResponse<T>> ToTask<T>(this IRestResponse<T> result) {
            return Task.FromResult(result);
        }
    }
}