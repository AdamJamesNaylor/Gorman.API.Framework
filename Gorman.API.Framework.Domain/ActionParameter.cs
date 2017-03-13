namespace Gorman.API.Framework.Domain {
    public class ActionParameter {
        public string Key { get; set; }
        public string Value { get; set; }

        public ActionParameter(string key, string value) {
            Key = key;
            Value = value;
        }

        public static string PositionX = "position_x";
        public static string PositionY = "position_y";
        public static string Display = "display";
    }
}