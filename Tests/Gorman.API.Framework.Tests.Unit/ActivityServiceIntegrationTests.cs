namespace Gorman.API.Framework.Tests.Unit {
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Convertors;
    using Domain;
    using Moq;
    using RestSharp;
    using Services;
    using Validators;
    using Xunit;

    public class ActivityServiceIntegrationTests {

        [Fact]
        public async Task AddActivity_WithChildActions_AssignsActorIdToAction() {
            var requestBuilder = new Mock<IRequestBuilder>();
            var restClient = new Mock<IRestClient>();
            var responseValidator = new Mock<IResponseValidator>();
            var activityConvertor = new Mock<IActivityConvertor>();
            var actionService = new Mock<IActionService>();
            var actionConvertor = new Mock<IActionConvertor>();
            var addActivityValidator = new Mock<IAddActivityValidator>();
            var actorService = new Mock<IActorService>();

            addActivityValidator.Setup(a => a.IsValidForAdd(It.IsAny<Activity>())).Returns(true);
            requestBuilder.Setup(r => r.BuildAddActivityRequest(It.IsAny<Activity>())).Returns(new JsonRestRequest("", Method.POST));
            restClient.Setup(r => r.ExecuteTaskAsync<API.Domain.Activity>(It.IsAny<JsonRestRequest>())).ReturnsAsync(new RestResponse<API.Domain.Activity> { Data = new API.Domain.Activity()});
            actorService.Setup(a => a.Add(It.IsAny<Actor>())).ReturnsAsync(new Actor { Id = 123 });
            //activityConvertor.Setup(a => a.Convert(It.IsAny<API.Domain.Activity>())).Returns((API.Domain.Activity a) => new Activity { Actions = new Collection<Action> { new Action { Actor = a.Actions.First(). } } });
            var someActor = new Actor();

            var someActivity = new Activity
            {
                Actors = new Collection<Actor> { someActor }
            };

            var sut = new ActivityService(requestBuilder.Object, restClient.Object, responseValidator.Object, activityConvertor.Object, actionService.Object, actionConvertor.Object, addActivityValidator.Object, actorService.Object);
            var result = await sut.Add(someActivity);

            Assert.Equal(123, result.Activities.First().Actions.First().Actor.Id);
        }

    }
}