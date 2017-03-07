namespace Gorman.API.Framework.Services {
    using RestSharp;
    using Validators;

    public interface IBaseService {
    }

    public abstract class BaseService
        : IBaseService {

        protected BaseService(Endpoints endpoints, IRestClient restClient, IResponseValidator responseValidator) {
            _endpoints = endpoints;
            _restClient = restClient;
            _responseValidator = responseValidator;
        }

        protected BaseService(Endpoints endpoints) :
            this(endpoints, new RestClient(endpoints.BaseUrl), new ResponseValidator()) {
        }

        protected readonly Endpoints _endpoints;

        protected readonly IRestClient _restClient;
        protected readonly IResponseValidator _responseValidator;
    }
}