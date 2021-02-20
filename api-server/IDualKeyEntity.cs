using System;

namespace Sheep.IHeartFiction.ApiServer
{
    public interface IDualKeyEntity
    {
        Guid UUID { get; set; }
        int Id { get; set; }
    }
}
