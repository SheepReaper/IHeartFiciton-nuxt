using System;

namespace Sheep.IHeartFiction.ApiServer
{
    public class Entity : IDualKeyEntity
    {
        public int Id { get; set; }
        public Guid UUID { get; set; }
    }
}
