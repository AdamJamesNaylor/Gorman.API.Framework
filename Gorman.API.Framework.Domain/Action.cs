
namespace Gorman.API.Framework.Domain
{
    using System.Collections.Generic;

    public class Action
    {
        public long Id { get; set; }
        public Actor Actor { get; set; }
        public long ActivityId { get; set; }
        public Dictionary<string, string> Parameters{ get; set; }

        public Action() {
            Parameters = new Dictionary<string, string>();
        }
    }

    public static class ActionParameter {
        public static string PositionX = "position_x";
        public static string PositionY = "position_y";
    }
}
