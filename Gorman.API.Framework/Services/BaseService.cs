namespace Gorman.API.Framework.Services {
    using RestSharp;
    using Validators;

    public interface IBaseService {
    }

    public abstract class BaseService
        : IBaseService {

        protected BaseService(IRequestBuilder requestBuilder, IRestClient restClient, IResponseValidator responseValidator) {
            _requestBuilder = requestBuilder;
            _restClient = restClient;
            _responseValidator = responseValidator;
        }

        protected BaseService(IRequestBuilder requestBuilder) :
            this(requestBuilder, new RestClient(requestBuilder.Endpoints.BaseUrl), new ResponseValidator()) {
        }

        protected readonly IRequestBuilder _requestBuilder;

        protected readonly IRestClient _restClient;
        protected readonly IResponseValidator _responseValidator;
    }
}