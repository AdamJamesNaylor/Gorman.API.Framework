
namespace Gorman.API.Framework.Domain {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    public class Activity {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("parent_id")]
        public long ParentId { get; set; }

        [JsonProperty("activities")]
        public Collection<Activity> Activities { get; set; }

        [JsonProperty("map_id")]
        public long MapId { get; set; }

        [JsonProperty("actions")]
        public Collection<Action> Actions { get; set; }

        [JsonProperty("actors")]
        public Collection<Actor> Actors { get; set; }

        public Activity() {
            Activities = new Collection<Activity>();
            Actions = new Collection<Action>();
            Actors = new Collection<Actor>();
        }

        public Action AddAction(Actor actor, IDictionary<string, string> parameters) {
            var action = new Action {
                Actor = actor,
                ActivityId = this.Id,
            };
            foreach (var param in parameters) {
                action.Parameters.Add(new ActionParameter(param.Key, param.Value));
            }
            this.Actions.Add(action);
            return action;
        }
    }
}