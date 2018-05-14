using Storm.Data;
using Storm.Domain.Entity.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.IRepository.WFManage
{
    public interface IWFItemDetailRepository : IRepositoryBase<WFItemDetailEntity>
    {
        List<WFItemDetailEntity> GetItemDetailList(string enCode);
        List<WFItemDetailEntity> GetItemDetailByItemIdList(string itemId);
    }
}
