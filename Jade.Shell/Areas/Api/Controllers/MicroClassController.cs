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


using AutoMapper;
using Jade.Core;
using Jade.Model;
using Jade.Model.Dto;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Areas.Api.Controllers
{
    public class MicroClassController : ApiBaseController
    {
        private static readonly MicroClassManager _microClassManager = MicroClassManager.Instance;

        #region MicroClassGroup

        public ActionResult GetMicroClassGroup(int id)
        {
            Micro_Class_Group microClassGroup = _microClassManager.GetMicroClassGroup(id);
            if (microClassGroup == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Micro_Class_GroupDto microClassGroupDto = Mapper.Map<Micro_Class_GroupDto>(microClassGroup);
            return DataResult(microClassGroupDto);
        }

        public ActionResult CreateMicroClassGroup()
        {
            Micro_Class_GroupDto args = RequestArgs<Micro_Class_GroupDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Micro_Class_Group microClassGroup = Mapper.Map<Micro_Class_Group>(args);

            NormalResult result = _microClassManager.CreateMicroClassGroup(microClassGroup);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateMicroClassGroup()
        {
            Micro_Class_GroupDto args = RequestArgs<Micro_Class_GroupDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Micro_Class_Group microClassGroup = Mapper.Map<Micro_Class_Group>(args);

            NormalResult result = _microClassManager.UpdateMicroClassGroup(microClassGroup);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveMicroClassGroup(int id)
        {
            _microClassManager.RemoveMicroClassGroup(id);
            return SuccessfulResult();
        }

        public ActionResult GetMicroClassGroupList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Micro_Class_Group> microClassGroupList = _microClassManager.GetMicroClassGroupList(args);

            GetListDataResult<Micro_Class_GroupDto> result = new GetListDataResult<Micro_Class_GroupDto>();
            result.PagingInfo = microClassGroupList.PagingInfo;
            result.Data = Mapper.Map<List<Micro_Class_Group>, List<Micro_Class_GroupDto>>(microClassGroupList.Data);

            return DataResult(result);
        }

        #endregion

        #region MicroClassInfo

        public ActionResult GetMicroClassInfo(int id)
        {
            Micro_Class_Info microClassInfo = _microClassManager.GetMicroClassInfo(id);
            if (microClassInfo == null)
            {
                return FailedResult("指定的数据不存在。");
            }

            Micro_Class_InfoDto microClassInfoDto = Mapper.Map<Micro_Class_InfoDto>(microClassInfo);
            return DataResult(microClassInfoDto);
        }

        public ActionResult CreateMicroClassInfo()
        {
            Micro_Class_InfoDto args = RequestArgs<Micro_Class_InfoDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Micro_Class_Info microClassInfo = Mapper.Map<Micro_Class_Info>(args);

            NormalResult result = _microClassManager.CreateMicroClassInfo(microClassInfo);

            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult UpdateMicroClassInfo()
        {
            Micro_Class_InfoDto args = RequestArgs<Micro_Class_InfoDto>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            Micro_Class_Info microClassInfo = Mapper.Map<Micro_Class_Info>(args);

            NormalResult result = _microClassManager.UpdateMicroClassInfo(microClassInfo);
            return ApiResult(result.Successful, result.Message);
        }

        public ActionResult RemoveMicroClassInfo(int id)
        {
            _microClassManager.RemoveMicroClassInfo(id);
            return SuccessfulResult();
        }

        public ActionResult GetMicroClassInfoList()
        {
            GetListDataArgs args = RequestArgs<GetListDataArgs>();
            if (args == null)
            {
                return FailedResult("参数无效。");
            }

            GetListDataResult<Micro_Class_Info> microClassInfoList = _microClassManager.GetMicroClassInfoList(args);

            GetListDataResult<Micro_Class_InfoDto> result = new GetListDataResult<Micro_Class_InfoDto>();
            result.PagingInfo = microClassInfoList.PagingInfo;
            result.Data = Mapper.Map<List<Micro_Class_Info>, List<Micro_Class_InfoDto>>(microClassInfoList.Data);

            return DataResult(result);
        }

        #endregion

    }
}