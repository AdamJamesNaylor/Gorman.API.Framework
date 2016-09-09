namespace Gorman.API.Framework.Convertors {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Domain;

    public interface IActorConvertor {
        Collection<Actor> Convert(List<API.Domain.Actor> actors);
        Actor Convert(API.Domain.Actor actor);
    }

    public class ActorConvertor
        : IActorConvertor {
        public Collection<Actor> Convert(List<API.Domain.Actor> actors) {
            return new Collection<Actor>(actors.Select(Convert).ToList());
        }

        public Actor Convert(API.Domain.Actor actor) {
            throw new System.NotImplementedException();
        }
    }
}