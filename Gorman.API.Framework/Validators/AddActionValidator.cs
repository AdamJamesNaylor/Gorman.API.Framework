
namespace Gorman.API.Framework.Validators {
    using Domain;

    public interface IAddActionValidator {
        bool IsValidForAdd(Action action);
    }

    public class AddActionValidator
        : IAddActionValidator {
        public bool IsValidForAdd(Action action) {
            return true;
        }
    }
}