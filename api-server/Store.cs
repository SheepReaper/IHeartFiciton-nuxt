using System.Collections.Generic;
using System.Linq;

namespace Sheep.IHeartFiction.ApiServer
{
    public class Store<TValue> : IStore<TValue> where TValue : class, IDualKeyEntity
    {
        private readonly ICollection<TValue> _entities = new List<TValue>();
        public IEnumerable<TValue> Entities => _entities;

        public TValue Create(TValue entity)
        {
            entity.Id = _entities.GetNextId();
            entity.UUID = _entities.GetNextGuid();

            _entities.Add(entity);

            return entity;
        }


        public TValue? Update<TKey>(TKey id, TValue newData)
        {
            // Does not handle the case of duplicate Id's
            if (_entities.TryGetSome(out var existing, ((IStore<TValue>)this).GetIdSelector(id)))
            {
                newData.AssignTo(existing.First(), new List<string> { "UUID", "Id" });
                return existing.First();
            }

            return null;
        }


        public bool Delete<TKey>(TKey id)
        {
            // Does not handle the case of duplicate Id's
            if (_entities.TryGetSome(out var existing, ((IStore<TValue>)this).GetIdSelector(id)))
            {
                _entities.Remove(existing.First());
                return true;
            }

            return false;
        }
    }
}
