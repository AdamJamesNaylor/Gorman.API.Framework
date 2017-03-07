
namespace Gorman.API.Framework {
    public class ResourceUrlCache {
        public ResourceUrlCache(Endpoints endpoints) {
            _endpoints = endpoints;
        }

        private readonly Endpoints _endpoints;
    }
}
