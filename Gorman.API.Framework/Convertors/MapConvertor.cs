
namespace Gorman.API.Framework.Convertors {
    using ApiMap = API.Domain.Map;
    using Domain;

    public interface IMapConvertor {
        Map Convert(ApiMap map);
        ApiMap Convert(Map map);
    }

    public class MapConvertor
        : IMapConvertor {
        public Map Convert(ApiMap map) {
            return new Map {
                Id = map.Id,
                TileUrl = map.TileUrl
            };
        }
        public ApiMap Convert(Map map) {
            return new ApiMap {
                Id = map.Id,
                TileUrl = map.TileUrl
            };
        }
    }
}