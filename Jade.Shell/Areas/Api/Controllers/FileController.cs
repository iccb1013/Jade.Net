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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jade.Shell.Areas.Api.Controllers
{
    public class FileController : ApiBaseController
    {
       // private static ExceptionHandlingService _exceptionHandling = ExceptionHandlingService.Instance;
        private static string _serverRootDir = AppSettings.RootPath;

        /// <summary>
        /// 上传到本地文件服务器
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            NormalResult<string> result;

            result = SaveFile();

            if(result.Successful)
            {
                return DataResult(result.Data);
            }
            else
            {
                return FailedResult(result.Message);
            }
        }

        /// <summary>
        /// 返回 OSS 访问地址
        /// </summary>
        /// <returns></returns>
        private NormalResult<string> SaveFile()
        {
            HttpRequestBase request = this.HttpContext.Request;

            if (request.Files.Count == 0)
            {
                return new NormalResult<string>("没有要上传的文件。");
            }

            if (request.Files.Count > 1)
            {
                return new NormalResult<string>("一次只能上传一个文件。");
            }

            HttpPostedFileBase file = request.Files[0];

            if (file.ContentLength / 1024 > 4096)
            {
                return new NormalResult<string>(String.Format("文件大小请勿超过：{0} KB", 4096));
            }

            FileInfo fileInfo = new FileInfo(file.FileName);

            //流长度在关闭释放流之后就取不到了
            int length = (int)(file.InputStream.Length / 1024);

            FileStream fsWrite = null;
            Stream stream = null;
            string storeFileName;

            #region 存储文件

            string targetDir;

            try
            {
                targetDir = Path.Combine(_serverRootDir, "FileStore");
                if (Directory.Exists(targetDir) == false)
                    Directory.CreateDirectory(targetDir);

                storeFileName = Guid.NewGuid().ToString() + fileInfo.Extension;

                string outputFileName = Path.Combine(targetDir, storeFileName);

                ////Path.Combine 是 \， HTTP 地址用的是 /
                ////但是序列化时，会对 / 转义成 \/ 
                //result.RelativeUri = "FileStore" + "/" + _site.SiteInfo.Code + "/" + storeFileName;

                byte[] buffer = new byte[4096];

                fsWrite = new FileStream(outputFileName, FileMode.Create);
                stream = file.InputStream;
                stream.Position = 0;

                int read = 0;
                do
                {
                    read = stream.Read(buffer, 0, buffer.Length);
                    fsWrite.Write(buffer, 0, read);

                } while (read > 0);
            }
            catch (Exception ex)
            {
              //  _exceptionHandling.HandleException(ex);
                return new NormalResult<string>(ex.Message);
            }
            finally
            {
                if (fsWrite != null)
                {
                    fsWrite.Flush();
                    fsWrite.Close();
                    fsWrite.Dispose();
                }
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            #endregion

            string storeFilePath = Path.Combine(targetDir, storeFileName);

            string url = AliOSSHelper.Instance.OSS.PutObject(storeFileName, storeFilePath);

            System.IO.File.Delete(storeFilePath);

            NormalResult<string> result = new NormalResult<string>();
            result.Data = url;
            return result;
        }
    }
}