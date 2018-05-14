using Storm.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Storm.Data
{
    public class RepositoryBase : IRepositoryBase, IDisposable
    {
        private StormDbContext dbcontext = new StormDbContext();
        private DbTransaction dbTransaction { get; set; }
        public IRepositoryBase BeginTrans()
        {
            DbConnection dbConnection = ((IObjectContextAdapter)dbcontext).ObjectContext.Connection;
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
            dbTransaction = dbConnection.BeginTransaction();
            return this;
        }
        public int Commit()
        {
            try
            {
                var returnValue = dbcontext.SaveChanges();
                if (dbTransaction != null)
                {
                    dbTransaction.Commit();
                }
                return returnValue;
            }
            catch (Exception)
            {
                if (dbTransaction != null)
                {
                    this.dbTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                this.Dispose();
            }
        }
        public void Dispose()
        {
            if (dbTransaction != null)
            {
                this.dbTransaction.Dispose();
            }
            this.dbcontext.Dispose();
        }
        public int Insert<TEntity>(TEntity entity) where TEntity : class
        {
            RemoveHoldingEntityInContext(entity);
            dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Insert<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            foreach (var entity in entitys)
            {
                RemoveHoldingEntityInContext(entity);
                dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Update<TEntity>(TEntity entity) where TEntity : class
        {
            RemoveHoldingEntityInContext(entity);
            dbcontext.Set<TEntity>().Attach(entity);
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] objAttrs = prop.GetCustomAttributes(typeof(NotMappedAttribute), true);
                if (objAttrs.Length <= 0)
                {
                    if (prop.GetValue(entity, null) != null)
                    {
                        if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                            dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                        dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                    }
                }
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Delete<TEntity>(TEntity entity) where TEntity : class
        {
            RemoveHoldingEntityInContext(entity);
            dbcontext.Set<TEntity>().Attach(entity);
            dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entitys = dbcontext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int DeleteById<TEntity>(TEntity entity) where TEntity : class
        {
            RemoveHoldingEntityInContext(entity);
            dbcontext.Set<TEntity>().Attach(entity);
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name.ToLower() == "DeleteMark".ToLower())
                {
                    dbcontext.Entry(entity).Property(prop.Name).CurrentValue = true;
                    dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                }
                if (prop.Name.ToLower() == "DeleteUserId".ToLower())
                {
                    var LoginInfo = OperatorProvider.Provider.GetCurrent();
                    if (LoginInfo != null)
                    {
                        dbcontext.Entry(entity).Property(prop.Name).CurrentValue = LoginInfo.UserId;
                        dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                    }
                }
                if (prop.Name.ToLower() == "DeleteTime".ToLower())
                {
                    dbcontext.Entry(entity).Property(prop.Name).CurrentValue = DateTime.Now;
                    dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                }
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int DeleteById<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entitys = dbcontext.Set<TEntity>().Where(predicate).ToList();
            for (int i = 0; i < entitys.Count; i++)
            {
                RemoveHoldingEntityInContext(entitys[i]);
                dbcontext.Set<TEntity>().Attach(entitys[i]);
                PropertyInfo[] props = entitys[i].GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() == "DeleteMark".ToLower())
                    {
                        dbcontext.Entry(entitys[i]).Property(prop.Name).CurrentValue = true;
                        dbcontext.Entry(entitys[i]).Property(prop.Name).IsModified = true;
                    }
                    if (prop.Name.ToLower() == "DeleteUserId".ToLower())
                    {
                        var LoginInfo = OperatorProvider.Provider.GetCurrent();
                        if (LoginInfo != null)
                        {
                            dbcontext.Entry(entitys[i]).Property(prop.Name).CurrentValue = LoginInfo.UserId;
                            dbcontext.Entry(entitys[i]).Property(prop.Name).IsModified = true;
                        }
                    }
                    if (prop.Name.ToLower() == "DeleteTime".ToLower())
                    {
                        dbcontext.Entry(entitys[i]).Property(prop.Name).CurrentValue = DateTime.Now;
                        dbcontext.Entry(entitys[i]).Property(prop.Name).IsModified = true;
                    }
                }
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        public TEntity FindEntity<TEntity>(object keyValue) where TEntity : class
        {
            return dbcontext.Set<TEntity>().Find(keyValue);
        }
        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return dbcontext.Set<TEntity>().FirstOrDefault(predicate);
        }
        public TEntity FindEntity<TEntity>(string strSql)
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql).FirstOrDefault<TEntity>();
        }
        public TEntity FindEntity<TEntity>(string strSql, DbParameter[] dbParameter)
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).FirstOrDefault<TEntity>();
        }
        public IQueryable<TEntity> IQueryable<TEntity>() where TEntity : class
        {
            return dbcontext.Set<TEntity>();
        }
        public IQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return dbcontext.Set<TEntity>().Where(predicate);
        }
        public List<TEntity> FindList<TEntity>(string strSql) where TEntity : class
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        public List<TEntity> FindList<TEntity>(string strSql, DbParameter[] dbParameter) where TEntity : class
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
        public List<TEntity> FindList<TEntity>(Pagination pagination) where TEntity : class,new()
        {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = dbcontext.Set<TEntity>().AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
        public List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class,new()
        {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = dbcontext.Set<TEntity>().Where(predicate);
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }

        //用于监测Context中的Entity是否存在，如果存在，将其Detach，防止出现问题。
        private Boolean RemoveHoldingEntityInContext<TEntity>(TEntity entity) where TEntity : class
        {
            var objContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
            var objSet = objContext.CreateObjectSet<TEntity>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        }
    }
}
