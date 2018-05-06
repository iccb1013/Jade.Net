/*
 
曹旭升（sheng.c）
E-mail: cao.silhouette@msn.com
QQ: 279060597
https://github.com/iccb1013
http://shengxunwei.com 
  
 */

using Jade.Model;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Data.Entity;
using Jade.Model.Dto;

namespace Jade.Core
{
    public class PushMessageManager
    {
        private static readonly PushMessageManager _instance = new PushMessageManager();
        public static PushMessageManager Instance
        {
            get { return _instance; }
        }

        private PushMessageManager()
        {

        }

        #region PushMessage

        public Push_Message GetPushMessage(int id)
        {
            using (Entities db = new Entities())
            {
                Push_Message pushMessage = db.Push_Message
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return pushMessage;
            }
        }

        public NormalResult CreatePushMessage(Push_Message pushMessage)
        {
            //pushMessage.Id = Guid.NewGuid();

            pushMessage.create_time = DateTime.Now;

            using (Entities db = new Entities())
            {
                db.Push_Message.Add(pushMessage);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public NormalResult UpdatePushMessage(Push_Message pushMessage)
        {

            using (Entities db = new Entities())
            {
                IQueryable<Push_Message> queryable = db.Push_Message;

                Push_Message dbPush_Message = queryable.FirstOrDefault(e => e.id == pushMessage.id);
                if (dbPush_Message == null)
                    return new NormalResult("指定的数据不存在。");

                ShengMapper.SetValuesSkipVirtual(pushMessage, dbPush_Message);

                db.SaveChanges();
            }

            return new NormalResult();
        }

        public void RemovePushMessage(int id)
        {
            using (Entities db = new Entities())
            {
                Push_Message pushMessage = db.Push_Message.FirstOrDefault(e => e.id == id);
                if (pushMessage != null)
                {
                    db.Push_Message.Remove(pushMessage);
                    db.SaveChanges();
                }
            }
        }

        public GetListDataResult<Push_Message> GetPushMessageList(GetListDataArgs args)
        {
            GetListDataResult<Push_Message> result = new GetListDataResult<Push_Message>();
            using (Entities db = new Entities())
            {
                IQueryable<Push_Message> queryable = db.Push_Message
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("Keyword");
                    queryable = queryable.Where(c => c.title.Contains(keyword) || c.description.Contains(keyword));
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

        #region PushMessageHistory

        public Push_History GetPushHistory(int id)
        {
            using (Entities db = new Entities())
            {
                Push_History pushHistory = db.Push_History
                    .Include(c => c.User)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == id);
                return pushHistory;
            }
        }

        public NormalResult CreatePushHistory(int pushMessageId, int userId, bool androidSendFlag, bool iosSendFlag)
        {
            using (Entities db = new Entities())
            {
                Push_Message dbPush_Message = db.Push_Message
                    .AsNoTracking()
                    .FirstOrDefault(e => e.id == pushMessageId);

                if (dbPush_Message == null)
                    return new NormalResult("指定的数据不存在。");

                Push_History push_History = new Push_History();
                push_History.push_message_id = pushMessageId;
                push_History.title = dbPush_Message.title;
                push_History.type = dbPush_Message.type;
                push_History.description = dbPush_Message.description;
                push_History.richContent = dbPush_Message.richContent;
                push_History.user_id = userId;
                push_History.create_time = DateTime.Now;
                push_History.android_push_state = androidSendFlag;
                push_History.ios_push_state = iosSendFlag;

                db.Push_History.Add(push_History);
                db.SaveChanges();
            }

            return new NormalResult();
        }

        public GetListDataResult<Push_History> GetPushHistoryList(GetListDataArgs args)
        {
            GetListDataResult<Push_History> result = new GetListDataResult<Push_History>();
            using (Entities db = new Entities())
            {
                IQueryable<Push_History> queryable = db.Push_History
                    .Include(c => c.User)
                    .AsNoTracking();

                if (args.Parameters.IsNullOrEmpty("PushMessageId") == false)
                {
                    int pushMessageId = args.Parameters.GetIntValue("PushMessageId");
                    queryable = queryable.Where(c => c.push_message_id == pushMessageId);
                }

                if (args.Parameters.IsNullOrEmpty("Keyword") == false)
                {
                    string keyword = args.Parameters.GetValue<string>("Keyword");
                    queryable = queryable.Where(c => c.title.Contains(keyword) || c.description.Contains(keyword));
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
