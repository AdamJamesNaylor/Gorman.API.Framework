
namespace Gorman.API.Framework.Convertors {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Domain;
    using Action = Domain.Action;
    using ApiAction = API.Domain.Action;
    using ActionParameter = Domain.ActionParameter;

    public interface IActionConvertor {
        Collection<Action> Convert(List<ApiAction> actions);
        Action Convert(ApiAction action);
    }

    public class ActionConvertor
        : IActionConvertor {

        public Action Convert(ApiAction action) {
            return new Action {
                Id = action.Id,
                //ActorId = action.ActorId,
                ActivityId = action.ActivityId,
                Parameters = action.Parameters.Select(p => new ActionParameter(p.Key, p.Value)).ToList()
            };
        }

        public Collection<Action> Convert(List<ApiAction> actions) {
            return new Collection<Action>(actions.Select(Convert).ToList());
        }
    }
}