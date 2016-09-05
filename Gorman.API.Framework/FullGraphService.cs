
namespace Gorman.API.Framework {
    using System;
    using Domain;
    using RestSharp;

    public class FullGraphService
        : BaseService {
        public FullGraphService(IRestClient restClient, IResponseValidator responseValidator, )
            : base(restClient, responseValidator) {
        }

        public FullGraphService(Uri domain)
            : base(domain) {
        }

        public Map Get(long mapId) {
            
        }
    }
}