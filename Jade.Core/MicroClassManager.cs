/*
 
曹旭升（sheng.c）
E-mail: cao.silhouette@msn.com
QQ: 279060597
https://github.com/iccb1013
http://shengxunwei.com 
  
 */

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Jade.Model;
using Sheng.Web.Infrastructure;
using Sheng.Kernal;

namespace Jade.Core
{
    public class MicroClassManager
    {
        private static readonly MicroClassManager _instance = new MicroClassManager();

        public static MicroClassManager Instance
        {
            get { return _instance; }
        }

        #region MicroClassInfo

        public Micro_Class_Info GetMicroClassInfo(int id)
        {
            using (Entities db = new Entities())
            {
                Micro_Class_Info microClassInfo = db.Micro_Class_Info
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return microClassInfo;
            }
        }

        public NormalResult CreateMicroClassInfo(Micro_Class_Info microClassInfo)
        {
            //microClassInfo.Id = Guid.NewGuid();

            microClassInfo.create_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                db.Micro_Class_Info.Add(microClassInfo);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateMicroClassInfo(Micro_Class_Info microClassInfo)
        {

            microClassInfo.update_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                IQueryable<Micro_Class_Info> queryable = db.Micro_Class_Info;

                Micro_Class_Info dbMicro_Class_Info = queryable.FirstOrDefault(e => e.id == microClassInfo.id);
                if (dbMicro_Class_Info == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(microClassInfo, dbMicro_Class_Info);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemoveMicroClassInfo(int id)
        {
            using (Entities db = new Entities())
            {
                Micro_Class_Info microClassInfo = db.Micro_Class_Info.FirstOrDefault(e => e.id == id);
                if (microClassInfo != null)
                {
                    db.Micro_Class_Info.Remove(microClassInfo);
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Micro_Class_Info> GetMicroClassInfoList(GetListDataArgs args)
        {
            GetListDataResult<Micro_Class_Info> result = new GetListDataResult<Micro_Class_Info>();
            using (Entities db = new Entities())
            {
                IQueryable<Micro_Class_Info> queryable = db.Micro_Class_Info
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("TopicInfoTitle") == false)
                {
                    string topicInfoTitle = args.Parameters.GetValue("TopicInfoTitle");
                    queryable = queryable.Where(c => c.topic_info_title.Contains(topicInfoTitle));
                }
                if (args.Parameters.IsNullOrEmpty("GroupId") == false)
                {
                    int groupId = args.Parameters.GetIntValue("GroupId");
                    if (groupId > 0)
                    {
                        queryable = queryable.Where(c => c.group_id == groupId);
                    }
                }

                result.PagingInfo = args.PagingInfo;
                int totalCount = queryable.Count();
                result.PagingInfo.UpdateTotalCount(totalCount);

                result.Data = queryable.OrderBy(args.OrderBy).Include(t => t.Micro_Class_Group)
                    .Skip((result.PagingInfo.CurrentPage - 1) * result.PagingInfo.PageSize)
                    .Take(result.PagingInfo.PageSize).ToList();
            }

            return result;
        }

        #endregion

        #region MicroClassGroup

        public Micro_Class_Group GetMicroClassGroup(int id)
        {
            using (Entities db = new Entities())
            {
                Micro_Class_Group microClassGroup = db.Micro_Class_Group
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return microClassGroup;
            }
        }

        public NormalResult CreateMicroClassGroup(Micro_Class_Group microClassGroup)
        {
            //microClassGroup.Id = Guid.NewGuid();

            microClassGroup.create_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                db.Micro_Class_Group.Add(microClassGroup);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdateMicroClassGroup(Micro_Class_Group microClassGroup)
        {
            microClassGroup.update_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                IQueryable<Micro_Class_Group> queryable = db.Micro_Class_Group;

                Micro_Class_Group dbMicro_Class_Group = queryable.FirstOrDefault(e => e.id == microClassGroup.id);
                if (dbMicro_Class_Group == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(microClassGroup, dbMicro_Class_Group);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemoveMicroClassGroup(int id)
        {
            using (Entities db = new Entities())
            {
                Micro_Class_Group microClassGroup = db.Micro_Class_Group.FirstOrDefault(e => e.id == id);
                if (microClassGroup != null)
                {
                    //先删除微课程课程
                    List<Micro_Class_Info> list = db.Micro_Class_Info.Where(e => e.group_id == id).ToList();
                    foreach (var item in list)
                    {
                        db.Micro_Class_Info.Remove(item);
                    }

                    db.Micro_Class_Group.Remove(microClassGroup);
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Micro_Class_Group> GetMicroClassGroupList(GetListDataArgs args)
        {
            GetListDataResult<Micro_Class_Group> result = new GetListDataResult<Micro_Class_Group>();
            using (Entities db = new Entities())
            {
                IQueryable<Micro_Class_Group> queryable = db.Micro_Class_Group
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("Name") == false)
                {
                    string name = args.Parameters.GetValue<string>("Name");
                    queryable = queryable.Where(c => c.name.Contains(name));
                }
                result.PagingInfo = args.PagingInfo;
                int totalCount = queryable.Count();
                result.PagingInfo.UpdateTotalCount(totalCount);

                result.Data = queryable.OrderBy(args.OrderBy)
                    .Skip((result.PagingInfo.CurrentPage - 1) * result.PagingInfo.PageSize)
                    .Take(result.PagingInfo.PageSize).ToList();
            }

            return result;
        }

        #endregion
    }
}
