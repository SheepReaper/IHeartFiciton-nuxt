using System;
using System.Collections.Generic;

namespace Sheep.IHeartFiction.ApiServer
{
    public interface IStore<TValue> where TValue : class, IDualKeyEntity {
        IEnumerable<TValue> Entities { get; }

        Func<TValue, bool> GetIdSelector<TKey>(TKey id) {
            if (id is Guid gid)
                return s => s.UUID == gid;

            if (id is int iid)
                return s => s.Id == iid;

            throw new NotSupportedException($"Type: '{typeof(TKey)}' is not supported as a key type for '{typeof(TValue)}' entity store.");
        }

        TValue Create(TValue entity);

        IEnumerable<TValue> Read() => Entities;

        TValue? Read<TKey>(TKey id) => Entities.FirstOrNull(GetIdSelector(id));

        TValue? Update<TKey>(TKey id, TValue newData);

        TValue? Update(TValue entity) => Update(entity.UUID, entity);

        bool Delete<TKey>(TKey id);
        
        bool Delete(TValue entity) => Delete(entity.UUID);
    }
}
