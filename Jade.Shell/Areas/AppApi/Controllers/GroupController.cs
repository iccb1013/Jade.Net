/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com r
*

*
********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto.AppDto;
using Sheng.Web.Infrastructure;

namespace Jade.Shell.Areas.AppApi.Controllers
{
    
    public class GroupController : AppApiBaseController
    {
        private static readonly MicroClassManager _microClassManager = new MicroClassManager();
        // GET: AppApi/MicroClassGroup
        public ActionResult QueryMicroGroup()
        {
            AppGetMicroClassGroupListArgs args = RequestArgs<AppGetMicroClassGroupListArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            if (string.IsNullOrEmpty(args.OrderBy))
            {
                args.OrderBy = "Create_Time DESC";
            }
            GetListDataArgs contractArgs = TransferAppGetListJsonArgsToGetListArgs.ToGetListDataArgs(args);

            GetListDataResult<Micro_Class_Group> microClassGroupList = _microClassManager.GetMicroClassGroupList(contractArgs);
            

            AppGetListDataResult<MicroClassGroupAppDto> result = new AppGetListDataResult<MicroClassGroupAppDto>();

            result.pageNo = microClassGroupList.PagingInfo.CurrentPage;
            result.pageCount = microClassGroupList.PagingInfo.PageSize;
            result.totalPage = microClassGroupList.PagingInfo.TotalPage;
            result.totalRecord = microClassGroupList.PagingInfo.TotalCount;

            result.objectList = Mapper.Map<List<Micro_Class_Group>, List<MicroClassGroupAppDto>>(microClassGroupList.Data);

            return DataResult(result);
        }
    }
}