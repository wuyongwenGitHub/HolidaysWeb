using Holidays.Model.Entites;
using Holidays.Model.FormatModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;

namespace Holidays.DALMSSQL
{
    /// <summary>
    /// mssql数据库 数据层 父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDAL<T> : IDAL.IBaseDAL<T> where T : class,new()
    {
        /// <summary>
        /// EF上下文对象
        /// </summary>
        protected DbContext db = new DBContextFactory().GetDbContext();// = new MODEL.OuOAEntities();

        #region 1.0 新增 实体 +int Add(T model)
        /// <summary>
        /// 新增 实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(T model)
        {
            var db = new DBContextFactory().WrapGetDbContext();
            db.Set<T>().Add(model);
            return db.SaveChanges();//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数
        }
        #endregion

        #region 2.0 根据 id 删除 +int Del(T model)
        /// <summary>
        /// 根据 id 删除
        /// </summary>
        /// <param name="model">包含要删除id的对象</param>
        /// <returns></returns>
        public int Del(T model)
        {
            db.Set<T>().Attach(model);
            db.Set<T>().Remove(model);
            return db.SaveChanges();
        }
        #endregion

        #region 3.0 根据条件删除 +int DelBy(Expression<Func<T, bool>> delWhere)
        /// <summary>
        /// 3.0 根据条件删除
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns></returns>
        public int DelBy(Expression<Func<T, bool>> delWhere)
        {
            //3.1查询要删除的数据
            List<T> listDeleting = db.Set<T>().Where(delWhere).ToList();
            //3.2将要删除的数据 用删除方法添加到 EF 容器中
            listDeleting.ForEach(u =>
            {
                db.Set<T>().Attach(u);//先附加到 EF容器
                db.Set<T>().Remove(u);//标识为 删除 状态
            });
            //3.3一次性 生成sql语句到数据库执行删除
            return db.SaveChanges();
        }
        #endregion

        #region 4.0 修改 +int Modify(T model, params string[] proNames)
        /// <summary>
        /// 4.0 修改，如：
        /// T u = new T() { uId = 1, uLoginName = "asdfasdf" };
        /// this.Modify(u, "uLoginName");
        /// </summary>
        /// <param name="model">要修改的实体对象</param>
        /// <param name="proNames">要修改的 属性 名称</param>
        /// <returns></returns>
        public int Modify(T model, params string[] proNames)
        {
             db = new DBContextFactory().WrapGetDbContext();
            //4.1将 对象 添加到 EF中
            DbEntityEntry entry = db.Entry<T>(model);
            //4.1跟踪对象状态
            db.Set<T>().Attach(model);

            //4.3先设置 对象的包装 状态
            if (proNames == null || proNames.Length <= 0)
            {
                entry.State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                entry.State = System.Data.Entity.EntityState.Unchanged;
            }

            //4.4循环 被修改的属性名 数组
            foreach (string proName in proNames)
            {
                try
                {
                    //4.5将每个 被修改的属性的状态 设置为已修改状态;后面生成update语句时，就只为已修改的属性 更新
                    entry.Property(proName).IsModified = true;
                }
                catch
                {
                    entry.Collection(proName).IsLoaded = true;
                }
            }

            //4.6一次性 生成sql语句到数据库执行
            return db.SaveChanges();
        }
        #endregion

        #region 4.0 批量修改 +int Modify(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedProNames)
        /// <summary>
        /// 4.0 批量修改
        /// </summary>
        /// <param name="model">要修改的实体对象</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="proNames">要修改的 属性 名称</param>
        /// <returns></returns>
        public int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedProNames)
        {
            //4.1查询要修改的数据
            List<T> listModifing = db.Set<T>().Where(whereLambda).ToList();

            //获取 实体类 类型对象
            Type t = typeof(T); // model.GetType();
            //获取 实体类 所有的 公有属性
            List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //创建 实体属性 字典集合
            Dictionary<string, PropertyInfo> dictPros = new Dictionary<string, PropertyInfo>();
            //将 实体属性 中要修改的属性名 添加到 字典集合中 键：属性名  值：属性对象
            proInfos.ForEach(p =>
            {
                if (modifiedProNames.Contains(p.Name))
                {
                    dictPros.Add(p.Name, p);
                }
            });

            //4.3循环 要修改的属性名
            foreach (string proName in modifiedProNames)
            {
                //判断 要修改的属性名是否在 实体类的属性集合中存在
                if (dictPros.ContainsKey(proName))
                {
                    //如果存在，则取出要修改的 属性对象
                    PropertyInfo proInfo = dictPros[proName];
                    //取出 要修改的值
                    object newValue = proInfo.GetValue(model, null); //object newValue = model.uName;

                    //4.4批量设置 要修改 对象的 属性
                    foreach (T usrO in listModifing)
                    {
                        //为 要修改的对象 的 要修改的属性 设置新的值
                        proInfo.SetValue(usrO, newValue, null); //usrO.uName = newValue;
                    }
                }
            }
            //4.4一次性 生成sql语句到数据库执行
            return db.SaveChanges();
        }
        #endregion

        #region 5.0 根据条件查询 +List<T> GetListBy(Expression<Func<T,bool>> whereLambda)
        /// <summary>
        /// 5.0 根据条件查询 +List<T> GetListBy(Expression<Func<T,bool>> whereLambda)
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            Type t = typeof(T);
            if (t.Name.Equals("HouseInfoView"))
            {
                List<HouseInfoView> houseInfoView= db.Set<T>().AsNoTracking().Where(whereLambda).ToList() as List<HouseInfoView>;
                return GetToDayPrice(houseInfoView);
            }
                return db.Set<T>().AsNoTracking().Where(whereLambda).ToList();
        }
        #endregion

        #region 5.1 根据条件 排序 和查询 + List<T> GetListBy<TKey>
        //添加参数IsAsc判断是调用升序还是降序 update by zz  2015年10月13日11:59:42
        /// <summary>
        /// 5.1 根据条件 排序 和查询
        /// </summary>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="whereLambda">查询条件 lambda表达式</param>
        /// <param name="orderLambda">排序条件 lambda表达式</param>
        /// <param name="IsAsc">判断调用升序还是降序</param>
        /// <returns></returns>
        public List<T> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool IsAsc = true)
        {
            //List<int> listIds = new List<int>() { 1, 2, 3 };
            //new MODEL.OuOAEntities().Ou_UserInfo.Where(u => listIds.Contains(u.uId));
            if (IsAsc)
            {
                return db.Set<T>().Where(whereLambda).OrderBy(orderLambda).ToList();
            }
            else
            {
                return db.Set<T>().Where(whereLambda).OrderByDescending(orderLambda).ToList();
            }
        }
        #endregion

        #region 6.0 分页查询 + List<T> GetPagedList<TKey>
        /// <summary>
        /// 6.0 分页查询 + List<T> GetPagedList<TKey>
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件 lambda表达式</param>
        /// <param name="orderBy">排序 lambda表达式</param>
        /// <param name="isAsc">排序方式</param>
        /// <returns></returns>
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if (isAsc)
            {
                return db.Set<T>().Where(whereLambda).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return db.Set<T>().Where(whereLambda).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }

        }
        #endregion

        #region 6.1分页查询 带输出 +List<T> GetPagedList<TKey>
        /// <summary>
        /// 6.1分页查询 带输出
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            rowCount = db.Set<T>().Where(whereLambda).Count();
            //1.查询分页数据
            if (isAsc)
            {
                Type t = typeof(T);
                if (t.Name.Equals("HouseInfoView"))
                {
                    List<HouseInfoView> houseInfoView = db.Set<T>().OrderBy(orderBy).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList() as List<HouseInfoView>;
                    return GetToDayPrice(houseInfoView);

                }
                return db.Set<T>().OrderBy(orderBy).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            //2.查询总行数
            else
            {
                Type t = typeof(T);
                if (t.Name.Equals("HouseInfoView"))
                {
                    List<HouseInfoView> houseInfoView = db.Set<T>().OrderByDescending(orderBy).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList() as List<HouseInfoView>;
                    return GetToDayPrice(houseInfoView);

                }
                return db.Set<T>().OrderByDescending(orderBy).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }
        private List<T> GetToDayPrice(List<HouseInfoView> houseInfoView=null)
        {
            CustomDAL customDAL = new CustomDAL();
            DateTime dtm = Convert.ToDateTime(GetNetDateTime());//获取网络时间
            string date = dtm.ToString("yyyy-MM-dd");
            houseInfoView.ForEach(x =>
            {
                TodayPrice todayPrice = customDAL.GetTodayPrice<TodayPrice>(x.ID, date).FirstOrDefault();
                if (todayPrice != null)
                {
                    x.Price = todayPrice.todayprice;
                }
            });

            return houseInfoView as List<T>;
        }
        public static string GetNetDateTime()
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;
            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse)request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                { if (h == "Date") { datetime = headerCollection[h]; } }
                return datetime;
            }
            catch (Exception) { return datetime; }
            finally
            {
                if (request != null)
                { request.Abort(); }
                if (response != null)
                { response.Close(); }
                if (headerCollection != null)
                { headerCollection.Clear(); }
            }
        }
        #endregion

        #region 7.0 跟据条件查询是否有数据 + bool IsExist(Expression<Func<T, bool>> whereLambda)
        public bool IsExist(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).Count() > 0 ? true : false;
        }
        #endregion

        #region 7.1 统计条数 + int CountRow(Expression<Func<T, bool>> whereLambda)
        public int CountRow(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).Count();
        }
        #endregion

        #region 执行Sql
        public List<T> SqlQuerys(string sql)
        {
            return db.Set<T>().SqlQuery(sql).ToList();
        }
        #endregion

    }
}
