/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com
*

*
********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    public static class Yuntongxun
    {
        static CCPRestSDK.CCPRestSDK _api = new CCPRestSDK.CCPRestSDK();
        static bool _isInit = false;

        static YuntongxunConfigurationSection _config = ConfigurationService.Instance.YuntongxunConfiguration;

        static Yuntongxun()
        {
            _isInit = _api.init(_config.Config.RestAddress, _config.Config.RestPort);
            _api.setAccount(_config.Config.Account, _config.Config.Token);
            _api.setAppId(_config.Config.AppId);
        }

        /// <summary>
        /// 模板短信
        /// </summary>
        /// <param name="to">短信接收端手机号码集合，用英文逗号分开，每批发送的手机号数量不得超过100个</param>
        /// <param name="templateId">模板Id</param>
        /// <param name="data">可选字段 内容数据，用于替换模板中{序号}</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static string SendTemplateSMS(string to, string templateId, string[] data)
        {
            string ret = null;
            try
            {
                if (_isInit)
                {
                    Dictionary<string, object> retData = _api.SendTemplateSMS(to, templateId, data);
                    ret = getDictionaryData(retData);
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            finally
            {
               
            }

            return ret;
        }

        static string getDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += getDictionaryData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }
    }
}
