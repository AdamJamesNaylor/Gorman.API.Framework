
namespace Gorman.API.Framework.Convertors {
    using ApiMap = API.Domain.Map;
    using Domain;

    public interface IMapConvertor {
        Map Convert(ApiMap map);
    }

    public class MapConvertor
        : IMapConvertor {
        public Map Convert(ApiMap map) {
            return new Map {

            };
        }
    }
}