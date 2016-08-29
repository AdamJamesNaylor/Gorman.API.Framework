namespace Gorman.API.Framework {
    using Domain;

    public class MapValidator
        : IMapValidator {

        public bool IsValidForAdd(Map map) {
            return true;
        }
    }
}