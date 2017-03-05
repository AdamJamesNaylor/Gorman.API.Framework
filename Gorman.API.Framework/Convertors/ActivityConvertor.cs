namespace Gorman.API.Framework.Convertors {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Domain;

    public interface IActivityConvertor {
        Activity Convert(API.Domain.Activity activity);
        Collection<Activity> Convert(IEnumerable<API.Domain.Activity> activities);
    }

    public class ActivityConvertor
        : IActivityConvertor {
        public Activity Convert(API.Domain.Activity activity) {
            return new Activity {
                Id = activity.Id,
                ParentId = activity.ParentId,
                MapId = activity.MapId
            };
        }

        public Collection<Activity> Convert(IEnumerable<API.Domain.Activity> activities) {
            return new Collection<Activity>(activities.Select(Convert).ToList());
        }
    }
}