
namespace Gorman.API.Framework.Services {
    using System.Linq;
    using System.Threading.Tasks;
    using Convertors;
    using Domain;
    using RestSharp;
    using Validators;

    //http://tacnet.io/tacsketch/#
    //http://www.stratuga.com/editor/
    //http://www.csgoboard.com/board
    //https://katanaapp.com

    public class FullGraphService
        : BaseService {

        public FullGraphService(Endpoints endpoints, IRestClient restClient, IResponseValidator responseValidator, IMapService mapService, IActivityService activityService, IActorService actorService, IActionService actionService)
            : base(endpoints, restClient, responseValidator) {
            _mapService = mapService;
            _activityService = activityService;
            _actorService = actorService;
            _actionService = actionService;
        }

        public FullGraphService(Endpoints endpoints)
            : base(endpoints) {
            _mapService = new MapService(endpoints, _restClient, new MapValidator(), _responseValidator, new MapConvertor());
        }

        public async Task<Map> Get(long mapId) {
            var mapTask = _mapService.Get(mapId);
            var activitesTask = _activityService.List(mapId);
            var actorsTask = _actorService.List(mapId);

            var map = await mapTask;
            map.Activities = (await activitesTask).ToList();
            foreach (var activity in map.Activities) {
                activity.Actions = await _actionService.List(activity.Id);
            }
            map.Actors = (await actorsTask).ToList();

            return map;
        }

        private readonly IMapService _mapService;
        private readonly IActivityService _activityService;
        private readonly IActorService _actorService;
        private readonly IActionService _actionService;
    }
}