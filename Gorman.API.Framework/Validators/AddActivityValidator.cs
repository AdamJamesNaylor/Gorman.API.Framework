
namespace Gorman.API.Framework.Validators {
    using Domain;

    public interface IAddActivityValidator {
        bool IsValidForAdd(Activity activity);
    }

    public class AddActivityValidator
        : IAddActivityValidator {
        public bool IsValidForAdd(Activity activity) {
            return true;
        }
    }
}