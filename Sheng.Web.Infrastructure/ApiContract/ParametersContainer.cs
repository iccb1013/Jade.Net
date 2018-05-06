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


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    public class ParametersContainer : Dictionary<string, object>
    {
        public bool IsNullOrEmpty(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("没有指定 key");

            if (this.ContainsKey(key) == false)
                return true;

            object value = this[key];

            return value == null || value.ToString() ==String.Empty;
        }

        public T GetValue<T>(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("没有指定 key");

            if (this.ContainsKey(key) == false)
                throw new ArgumentOutOfRangeException("没有指定 key 的数据");

            object value = this[key];

            if (value == null)
                return default(T);

            return (T)value;
        }

        public string GetValue(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("没有指定 key");

            if (this.ContainsKey(key) == false)
                throw new ArgumentOutOfRangeException("没有指定 key 的数据");

            object value = this[key];

            if (value == null)
                return null;

            return value.ToString();
        }

        public Guid GetGuidValue(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("没有指定 key");

            if (this.ContainsKey(key) == false)
                throw new ArgumentOutOfRangeException("没有指定 key 的数据");

            object value = this[key];

            if (value == null && value.ToString() == "")
                return Guid.Empty;

            return Guid.Parse(value.ToString());
        }

        public int GetIntValue(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("没有指定 key");

            if (this.ContainsKey(key) == false)
                throw new ArgumentOutOfRangeException("没有指定 key 的数据");

            object value = this[key];

            if (value == null && value.ToString() == "")
                return 0;

            return int.Parse(value.ToString());
        }

        public bool GetBoolValue(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("没有指定 key");

            if (this.ContainsKey(key) == false)
                throw new ArgumentOutOfRangeException("没有指定 key 的数据");

            object value = this[key];

            if (value == null && value.ToString() == "")
                return false;

            return bool.Parse(value.ToString());
        }

        /// <summary>
        /// 从JArray转换为指定类型的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Guid [] GetGuidArray(string key)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("没有指定 key");

            if (this.ContainsKey(key) == false)
                throw new ArgumentOutOfRangeException("没有指定 key 的数据");

            object value = this[key];

            if (value == null)
                return new Guid [0];

            JArray jArray = value as JArray;

            Guid[] result = new Guid[jArray.Count];

            for(int i = 0; i < jArray.Count; i++)
            {
                Guid obj = Guid.Parse(jArray[i].ToString());
                result[i] = obj;
            }

            return result;
        }
    }
}
