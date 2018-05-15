using MySql.Data.MySqlClient;
using Storm.Data;
using Storm.Domain.Entity.WFManage;
using Storm.Domain.IRepository.WFManage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Repository.WFManage
{
    public class WFItemDetailRepository : RepositoryBase<WFItemDetailEntity>, IWFItemDetailRepository
    {
        public List<WFItemDetailEntity> GetItemDetailList(string enCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    WF_ItemDetails d
                                    INNER  JOIN WF_Items i ON i.Id = d.ItemId
                            WHERE   1 = 1
                                    AND i.EnCode = @enCode
                                    AND d.EnabledMark = 1
                            ORDER BY d.SortCode ASC");
            DbParameter[] parameter = 
            {
                 new MySqlParameter("@enCode",enCode)
            };
            List < WFItemDetailEntity > models= this.FindList(strSql.ToString(), parameter);
            if (models != null)
            {
                models = models.Where(m => m.DeleteMark != true).ToList();
            }
            return models;
        }
        public List<WFItemDetailEntity> GetItemDetailByItemIdList(string itemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    WF_ItemDetails d
                                    INNER  JOIN WF_Items i ON i.Id = d.ItemId
                            WHERE   1 = 1
                                    AND i.Id = @Id
                                    AND d.EnabledMark = 1
                            ORDER BY d.SortCode ASC");
            DbParameter[] parameter = 
            {
                 new MySqlParameter("@Id",itemId)
            };
            List<WFItemDetailEntity> models = this.FindList(strSql.ToString(), parameter);
            if (models != null)
            {
                models = models.Where(m => m.DeleteMark != true).ToList();
            }
            return models;
        }
    }
}
