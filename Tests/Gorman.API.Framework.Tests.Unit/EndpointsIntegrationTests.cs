
namespace Gorman.API.Framework.Tests.Unit {
    using System.Threading.Tasks;
    using API.Domain;
    using Moq;
    using RestSharp;
    using Validators;
    using Xunit;

    public class EndpointsIntegrationTests {
        [Fact]
        public async Task Get_WithIncorrectResponseModel_ThrowsResponseValidationException() {
            var restClient = new Mock<IRestClient>();
            var responseWithFailedDeserialisation = new RestResponse<Response<EndpointList>> { Data = new Response<EndpointList> { Data = null, Error = null }, ResponseStatus = ResponseStatus.Completed };
            restClient.Setup(r => r.ExecuteTaskAsync<Response<EndpointList>>(It.IsAny<IRestRequest>()))
                .Returns(responseWithFailedDeserialisation.ToTask());

            await Assert.ThrowsAsync<ResponseValidationException>(async () => await Endpoints.Get(restClient.Object, new ResponseValidator()));
        }
    }
}