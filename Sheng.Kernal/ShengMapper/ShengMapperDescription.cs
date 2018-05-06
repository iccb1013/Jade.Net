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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sheng.Kernal
{
    class ShengMapperTypeDescription
    {
        public Type Type
        {
            get;
            private set;
        }

        private List<ShengMapperPropertyDescription> _propertyList = new List<ShengMapperPropertyDescription>();
        public List<ShengMapperPropertyDescription> PropertyList
        {
            get { return _propertyList; }
            set { _propertyList = value; }
        }

        private Hashtable _propertyNames = new Hashtable();

        public ShengMapperTypeDescription(Type type)
        {
            Type = type;

            PropertyInfo[] propertyList = Type.GetProperties();
            foreach (PropertyInfo property in propertyList)
            {
                ShengMapperPropertyDescription propertyMappingDescription = new ShengMapperPropertyDescription(property);

                _propertyList.Add(propertyMappingDescription);
                _propertyNames.Add(property.Name, propertyMappingDescription);
            }
        }

        public bool ContainsProperty(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("TypeMappingDescription.ContainsProperty 必须指定属性名。");

            return _propertyNames.ContainsKey(propertyName);
        }       

        public bool IsVirtual(string propertyName)
        {
            ShengMapperPropertyDescription propertyMappingDescription = (ShengMapperPropertyDescription)_propertyNames[propertyName];
            return propertyMappingDescription.PropertyInfo.SetMethod.IsVirtual;
        }

        public object GetValue(object obj, string propertyName)
        {
            if (obj == null)
                throw new ArgumentNullException("指定的对象为空。");

            if (obj.GetType() != this.Type)
                throw new ArgumentException("指定的对象类型与缓存的对象类型不一致。");

            if (_propertyNames.ContainsKey(propertyName) == false)
                throw new ArgumentOutOfRangeException("指定的属性名不存在。");

            ShengMapperPropertyDescription propertyMappingDescription = (ShengMapperPropertyDescription)_propertyNames[propertyName];
            if (propertyMappingDescription.CanRead == false)
                throw new InvalidOperationException("属性 " + propertyName + "不可读。");

            return propertyMappingDescription.GetValue(obj);
        }

        public void SetValue(object obj, string propertyName, object value)
        {
            if (obj == null)
                throw new ArgumentNullException("指定的对象为空。");

            if (obj.GetType() != this.Type)
                throw new ArgumentException("指定的对象类型与缓存的对象类型不一致。");

            if (_propertyNames.ContainsKey(propertyName) == false)
                throw new ArgumentOutOfRangeException("指定的属性名不存在。");

            ShengMapperPropertyDescription propertyMappingDescription = (ShengMapperPropertyDescription)_propertyNames[propertyName];
            if (propertyMappingDescription.CanWrite == false)
                throw new InvalidOperationException("属性 " + propertyName + "只读。");

            Type propertyType = propertyMappingDescription.PropertyInfo.PropertyType;
            if (propertyType.IsValueType == false && value != null)
            {
                Type valueType = value.GetType();
                if(propertyType != valueType && valueType.IsSubclassOf(propertyType) == false)
                {
                    throw new ArgumentException("目标对象的 " + propertyName + "与 value 的类型既不一致，也不是目标类型的派生类。");
                }
            }

            propertyMappingDescription.SetValue(obj, value);
        }

    }

    //如果要给属性实现转换器
    //在此实现
    //可参考 sheng.ADO.NET.Plus
    //https://github.com/iccb1013/sheng.ADO.NET.Plus
    class ShengMapperPropertyDescription
    {
        public string Name
        {
            get
            {
                if (_propertyInfo == null)
                    return String.Empty;

                return _propertyInfo.Name;
            }
        }
      
        public bool CanRead
        {
            get
            {
                return _propertyInfo.CanRead;
            }
        }

        public bool CanWrite
        {
            get
            {
                return _propertyInfo.CanWrite;
            }
        }

        private PropertyInfo _propertyInfo;
        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
            set { _propertyInfo = value; }
        }

        public ShengMapperPropertyDescription(PropertyInfo property)
        {
            PropertyInfo = property;
        }

        public object GetValue(object obj)
        {
            return _propertyInfo.GetValue(obj, null);
        }

        public void SetValue(object obj,object value)
        {
            _propertyInfo.SetValue(obj, value, null);
        }
    }
}
