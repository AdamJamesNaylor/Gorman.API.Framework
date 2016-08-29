namespace Gorman.API.Framework {
    using System;
    using System.Collections.Specialized;
    using Domain;
    using RestSharp;

    public abstract class BaseService {
        public bool IsInitialised { get; internal set; }

        protected BaseService(IRestClient restClient) {
            _restClient = restClient;
        }

        protected BaseService(Uri domain) {
            _restClient = new RestClient(domain);
            IsInitialised = false;
        }

        public async void Initialise() {
            var request = new RestRequest(Method.GET);
            var result = await _restClient.ExecuteTaskAsync<EndpointListResponse>(request);

            _endpoints.Add("maps_url", result.Data.MapUrl);

            IsInitialised = true;
        }

        protected readonly StringDictionary _endpoints = new StringDictionary();

        protected readonly IRestClient _restClient;
    }
}