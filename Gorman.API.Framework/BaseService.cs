namespace Gorman.API.Framework {
    using System;
    using System.Collections.Specialized;
    using System.Threading.Tasks;
    using API.Domain;
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

        protected virtual async Task Initialise() {
            if (IsInitialised)
                return;

            var request = new RestRequest(Method.GET);
            var result = await _restClient.ExecuteTaskAsync<EndpointListResponse>(request);

            _endpoints.Add("maps_url", result.Data.MapsUrl);

            IsInitialised = true;
        }

        protected virtual void Initialise(BaseService initialisedService) {
            if (IsInitialised)
                return;

            _endpoints.Clear();
            foreach (var key in initialisedService._endpoints.Keys)
                _endpoints.Add(key.ToString(), initialisedService._endpoints[key.ToString()]);
            
            IsInitialised = true;
        }

        protected readonly StringDictionary _endpoints = new StringDictionary();

        protected readonly IRestClient _restClient;
        protected readonly IResponseValidator _responseValidator;
    }
}