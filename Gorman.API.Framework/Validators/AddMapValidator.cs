
namespace Gorman.API.Framework.Validators {
    using Domain;

    public interface IAddActorValidator {
        bool IsValidForAdd(Actor actor);
    }

    public class AddActorValidator
        : IAddActorValidator {
        public bool IsValidForAdd(Actor actor) {
            return true;
        }
    }
}