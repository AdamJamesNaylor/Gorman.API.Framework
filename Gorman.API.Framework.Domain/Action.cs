
namespace Gorman.API.Framework.Domain
{
    public class Action
    {
        public long Id { get; set; }
        public long ActorId { get; set; }
        public long ActivityId { get; set; }
        public ActionType Type { get; set; }
    }

    public enum ActionType {
        Add,
        Update,
        Remove,
    }
}
