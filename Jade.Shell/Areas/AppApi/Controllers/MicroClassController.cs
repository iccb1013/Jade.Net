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
    public class MicroClassController : AppApiBaseController
    {
        private static readonly MicroClassManager _microClassManager = MicroClassManager.Instance;
        // GET: AppApi/Microclass
        public ActionResult QueryMicroInfo()
        {
            AppGetMicroClassInfoListArgs args = RequestArgs<AppGetMicroClassInfoListArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            if (string.IsNullOrEmpty(args.OrderBy))
            {
                args.OrderBy = "Create_Time DESC";
            }
            GetListDataArgs contractArgs = TransferAppGetListJsonArgsToGetListArgs.ToGetListDataArgs(args);
            
            GetListDataResult<Micro_Class_Info> microClassInfoList = _microClassManager.GetMicroClassInfoList(contractArgs);

            AppGetListDataResult<MicroClassInfoAppDto> result = new AppGetListDataResult<MicroClassInfoAppDto>();

            result.pageNo = microClassInfoList.PagingInfo.CurrentPage;
            result.pageCount = microClassInfoList.PagingInfo.PageSize;
            result.totalPage = microClassInfoList.PagingInfo.TotalPage;
            result.totalRecord = microClassInfoList.PagingInfo.TotalCount;

            result.objectList = Mapper.Map<List<Micro_Class_Info>, List<MicroClassInfoAppDto>>(microClassInfoList.Data);

            return DataResult(result);
        }
    }
}