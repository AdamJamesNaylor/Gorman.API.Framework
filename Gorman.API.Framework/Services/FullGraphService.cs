
namespace Gorman.API.Framework.Services {
    using System;
    using System.Threading.Tasks;
    using Convertors;
    using Domain;
    using RestSharp;

    public class FullGraphService
        : BaseService {

        public FullGraphService(IRestClient restClient, IResponseValidator responseValidator, IMapService mapService)
            : base(restClient, responseValidator) {
            _mapService = mapService;
        }

        public FullGraphService(Uri domain)
            : base(domain) {
            _mapService = new MapService(_restClient, new MapValidator(), _responseValidator, new MapConvertor());
        }

        public async Task<Map> Get(long mapId) {
            await Initialise();

            var map = await _mapService.Get(mapId);
            return map;
        }

        protected override async Task Initialise() {
            await base.Initialise();

            _mapService.Initialise(this);
        }

        private readonly IMapService _mapService;
    }
}