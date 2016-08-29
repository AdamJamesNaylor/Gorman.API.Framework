namespace Gorman.API.Framework {
    using System;
    using System.Collections.Specialized;
    using System.Threading.Tasks;
    using Domain;
    using RestSharp;

    public abstract class BaseService {
        public bool IsInitialised { get; internal set; }

        protected BaseService(IRestClient restClient, IResponseValidator responseValidator) {
            _restClient = restClient;
            _responseValidator = responseValidator;
        }

        protected BaseService(Uri domain) {
            _restClient = new RestClient(domain);
            _responseValidator = new ResponseValidator();
            IsInitialised = false;
        }

        protected async virtual Task Initialise() {
            if (IsInitialised)
                return;

            var request = new RestRequest(Method.GET);
            var result = await _restClient.ExecuteTaskAsync<EndpointListResponse>(request);

            _endpoints.Add("maps_url", result.Data.MapUrl);

            IsInitialised = true;
        }

        protected readonly StringDictionary _endpoints = new StringDictionary();

        protected readonly IRestClient _restClient;
        protected readonly IResponseValidator _responseValidator;
    }
}