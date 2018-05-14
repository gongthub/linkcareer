using Storm.Data;
using Storm.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Storm.Domain.IRepository.SystemManage
{
    public interface IItemsDetailRepository : IRepositoryBase<ItemsDetailEntity>
    {
        List<ItemsDetailEntity> GetItemList(string enCode);
    }
}
