
namespace Gorman.API.Framework.Domain
{
    using System.Collections.Generic;

    public class Action
    {
        public long Id { get; set; }
        public Actor Actor { get; set; }
        public long ActivityId { get; set; }
        public List<ActionParameter> Parameters{ get; set; }

        public Action() {
            Parameters = new List<ActionParameter>();
        }
    }
}
