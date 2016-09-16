﻿
namespace Gorman.API.Framework.Domain {
    using System.Collections.Generic;

    public class Map {
        public long Id { get; set; }
        public string TileUrl { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Actor> Actors { get; set; }
    }
}