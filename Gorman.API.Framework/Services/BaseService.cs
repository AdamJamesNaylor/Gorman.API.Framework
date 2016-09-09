namespace Gorman.API.Framework.Services {
    using System;
    using System.Collections.Specialized;
    using System.Threading.Tasks;
    using API.Domain;
    using RestSharp;

    public interface IBaseService {
    }

    public abstract class BaseService
        : IBaseService {

        protected BaseService(Endpoints endpoints, IRestClient restClient, IResponseValidator responseValidator) {
            _endpoints = endpoints;
            _restClient = restClient;
            _responseValidator = responseValidator;
        }

        protected BaseService(Endpoints endpoints) {
            _endpoints = endpoints;
            _restClient = new RestClient(endpoints.BaseUrl);
            _responseValidator = new ResponseValidator();
        }

        protected readonly Endpoints _endpoints;

        protected readonly IRestClient _restClient;
        protected readonly IResponseValidator _responseValidator;
    }
}