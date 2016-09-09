
namespace Gorman.API.Framework.Services {
    using System.Linq;
    using System.Threading.Tasks;
    using Convertors;
    using Domain;
    using RestSharp;

    public class FullGraphService
        : BaseService {

        public FullGraphService(Endpoints endpoints, IRestClient restClient, IResponseValidator responseValidator, IMapService mapService, IActivityService activityService)
            : base(endpoints, restClient, responseValidator) {
            _mapService = mapService;
            _activityService = activityService;
        }

        public FullGraphService(Endpoints endpoints)
            : base(endpoints) {
            _mapService = new MapService(endpoints, _restClient, new MapValidator(), _responseValidator, new MapConvertor());
        }

        public async Task<Map> Get(long mapId) {
            var mapTask = _mapService.Get(mapId);
            var activitesTask = _activityService.List(mapId);

            var map = await mapTask;
            map.Activities = (await activitesTask).ToList();

            return map;
        }

        private readonly IMapService _mapService;
        private readonly IActivityService _activityService;
    }
}