namespace Gorman.API.Framework {
    using Domain;

    public interface IMapValidator {
        bool IsValidForAdd(Map request);
    }
}