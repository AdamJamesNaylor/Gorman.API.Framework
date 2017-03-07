
namespace Gorman.API.Framework.Domain
{
    using System.Collections.Generic;

    public class Action
    {
        public long Id { get; set; }
        public long ActorId { get; set; }
        public long ActivityId { get; set; }
        public Dictionary<string, string> Parameters{ get; set; }
    }
}
