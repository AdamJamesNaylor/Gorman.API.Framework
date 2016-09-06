
namespace Gorman.API.Framework.Services
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Domain;
    using RestSharp;

    //options
    //1) Add all URLs to root. Means long names and could be messy
    //2) Naeivly hardcode the URLs into the framework. Brittle but easiest.
    //3) Navigate everytime
    //4) Add URLs to each framework object and pass those around

    public class ActivityService
        : BaseService {

        public ActivityService(IRestClient restClient, IResponseValidator responseValidator)
            : base(restClient, responseValidator) {

        }

        public ActivityService(Uri domain)
            : base(domain) {

        }

        public async Task<Collection<Activity>> List(long mapId) {
            await Initialise();

            var request = CreateRequest(Method.GET);
            request.AddParameter("mapId", id, ParameterType.UrlSegment);

            var restResponse = await _restClient.ExecuteTaskAsync<Response>(request);
            var map = _responseValidator.Validate(restResponse.Data);
            return _mapConvertor.Convert(map);
        }

        private RestRequest CreateRequest(Method method)
        {
            return new RestRequest(_mapsEndpoint, method)
            {
                RequestFormat = DataFormat.Json
            };
        }

        protected override async Task Initialise()
        {
            if (IsInitialised)
                return;

            await base.Initialise();
            _activitesEndpoint = new Uri(_endpoints["map_url"]);
        }


    }
}
