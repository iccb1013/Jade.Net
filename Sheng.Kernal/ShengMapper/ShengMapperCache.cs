/*
********************************************************************
*
*    曹旭升（sheng.c）
*    E-mail: cao.silhouette@msn.com
*    QQ: 279060597
*    https://github.com/iccb1013
*    http://shengxunwei.com
*
*    © Copyright 2017
*
********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Sheng.Kernal
{
    static class ShengMapperCache
    {
        private static Dictionary<Type, ShengMapperTypeDescription> _cacheList = new Dictionary<Type, ShengMapperTypeDescription>();

        static ShengMapperCache()
        {

        }

        public static ShengMapperTypeDescription Get(Type type)
        {
            if (type == null)
                return null;

            if (_cacheList.Keys.Contains(type))
                return _cacheList[type];
            else
            {
                ShengMapperTypeDescription cacheCodon = new ShengMapperTypeDescription(type);

                Monitor.Enter(_cacheList);

                if (_cacheList.Keys.Contains(type) == false)
                    _cacheList.Add(type, cacheCodon);

                Monitor.Exit(_cacheList);

                return cacheCodon;

            }
        }

    }
}
