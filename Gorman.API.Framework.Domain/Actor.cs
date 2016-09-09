
namespace Gorman.API.Framework.Domain {
    using System;
    using System.Drawing;

    public class Actor {
        public long Id { get; set; }
        public long MapId { get; set; }
        public long PositionX { get; set; }
        public long PositionY { get; set; }
        public string ImageUrl { get; set; }
    }
}