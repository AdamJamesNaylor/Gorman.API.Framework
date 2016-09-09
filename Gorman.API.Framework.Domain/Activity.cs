
namespace Gorman.API.Framework.Domain {
    using System.Collections.ObjectModel;

    public class Activity {
        public long Id { get; set; }
        public long MapId { get; set; }
        public Collection<Action> Actions { get; set; }
    }
}