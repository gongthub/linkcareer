using Storm.Code;
using Storm.Domain.Entity.WFManage;
using Storm.Domain.IRepository.WFManage;
using Storm.Repository.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Application.WFManage
{
    public class WFItemApp
    {
        private IWFItemRepository service = new WFItemRepository();
        private IWFItemDetailRepository serviceDetail = new WFItemDetailRepository();
        public List<WFItemEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public List<WFItemEntity> GetEnableList()
        {
            return service.IQueryable().ToList();
        }
        public WFItemEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (service.IQueryable().Count(t => t.ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.Id == keyValue);
            }
        }
        public void SubmitForm(WFItemEntity itemsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsEntity.Modify(keyValue);
                service.Update(itemsEntity);
            }
            else
            {
                itemsEntity.Create();
                service.Insert(itemsEntity);
            }
        }
        public List<WFItemDetailEntity> GetDetailList(string itemId = "", string keyword = "")
        {
            var expression = ExtLinq.True<WFItemDetailEntity>();
            if (!string.IsNullOrEmpty(itemId))
            {
                expression = expression.And(t => t.ItemId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.ItemName.Contains(keyword));
                expression = expression.Or(t => t.ItemCode.Contains(keyword));
            }
            return serviceDetail.IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        }
        public List<WFItemDetailEntity> GetItemDetailList(string enCode)
        {
            return serviceDetail.GetItemDetailList(enCode);
        }
        public List<WFItemDetailEntity> GetItemDetailByItemIdList(string itemId)
        {
            return serviceDetail.GetItemDetailByItemIdList(itemId);
        }
        public List<WFItemDetailEntity> GetEnableItemDetailList()
        {
            return serviceDetail.IQueryable(m => m.EnabledMark == true).ToList();
        }
        public WFItemDetailEntity GetDetailForm(string keyValue)
        {
            return serviceDetail.FindEntity(keyValue);
        }
        public void DeleteDetailForm(string keyValue)
        {
            serviceDetail.Delete(t => t.Id == keyValue);
        }
        public void SubmitDetailForm(WFItemDetailEntity itemsDetailEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                itemsDetailEntity.Modify(keyValue);
                serviceDetail.Update(itemsDetailEntity);
            }
            else
            {
                itemsDetailEntity.Create();
                serviceDetail.Insert(itemsDetailEntity);
            }
        }
    }
}
