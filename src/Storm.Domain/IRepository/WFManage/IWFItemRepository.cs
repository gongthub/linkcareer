using Storm.Data;
using Storm.Domain.Entity.WFManage;
using System.Collections.Generic;

namespace Storm.Domain.IRepository.WFManage
{
    public interface IWFItemRepository : IRepositoryBase<WFItemEntity>
    {
        List<WFItemEntity> GetEnableList();
    }
}
