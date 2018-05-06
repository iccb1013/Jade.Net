//Layer弹出组件的动画方式
var _layerShift = 5;

function goUrl(url)
{
    window.location.href = url;
}

function getQueryString(name)
{
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function subStringAndEtc(string, length) {
    if (string.length > length) {
        return string.substr(0, length) + "...";
    } else {
        return string;
    }
}

function getTopWindow() {
    var win = window.parent;
    while (win != null && win != win.parent) {
        win = win.parent;
    }
    return win;
}

function isExitsFunction(funcName) {
    try {
        if (typeof (eval(funcName)) == "function") {
            return true;
        }
    } catch (e) { }
    return false;
}

//Cookie 操作
function setCookie(name, value, expiredays)
{

    var exdate = new Date()
    exdate.setDate(exdate.getDate() + expiredays)

    var str = name + "=" + escape(value);
    if (expiredays != null)
    {
        str += ";expires=" + exdate.toGMTString() + "; path=/";
    }

    document.cookie = str;
}

function getCookie(name)
{
    if (document.cookie.length > 0)
    {
        c_start = document.cookie.indexOf(name + "=")
        if (c_start != -1)
        {
            c_start = c_start + name.length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return unescape(document.cookie.substring(c_start, c_end))
        }
    }
    return ""
}

function removeCookie(name)
{
    var exdate = new Date()
    exdate.setDate(exdate.getDate() - 10);
    document.cookie = name + "=v; expires=" + exdate.toGMTString() + "; path=/";
}

////////

//Layer

function layerInputAlertMsg()
{
    layerMsg("请核对您的输入。");
}

function layerMsg(message,time)
{
    if (time != undefined && time != null)
    {
        layer.msg(message, {
            time: time
        });
    }
    else
    {
        layer.msg(message, {
            time: 1500
        });
    }
}

function layerAlert(message,callback)
{
    var alertLayerIndex = layer.alert(message, {
        title: false, closeBtn: false, shift: _layerShift,
        success: function (layero, index)
        {
            $(layero).focus();
            $(layero).keypress(function (e)
            {
                if (e.keyCode == 13)
                {
                    layer.close(alertLayerIndex);
                    if (callback != undefined && callback != null)
                    {
                        callback();
                    }
                } else if (e.keyCode == 27)
                {
                    layer.close(alertLayerIndex);
                    if (callback != undefined && callback != null)
                    {
                        callback();
                    }
                }
            });
            ////alert($(layero).find("a").length);
            //alert($($(layero).find("a")[0]).html());
            //$($(layero).find("a")[0]).focus();
        },
        yes: function (index)
        {
            layer.close(alertLayerIndex);
            if (callback != undefined && callback != null)
            {
                callback();
            }
        }
    });
}

function layerConfirm(message,callback) {
    var confirmLayerIndex = layer.confirm(message, {
        btn: ['确认', '取消'], //按钮
        shade: [0.4, '#393D49'],
        title: false,
        closeBtn: false,
        shift: _layerShift
    }, function () {
        layer.close(confirmLayerIndex);
        callback();
    });
}

///////////

//jquery validation

function showValidationErrors(errorMap, errorList)
{
    $.each(this.successList, function (index, value)
    {
        $(value).css("background-color", "#FFF");
    });

    if (errorList.length > 0)
    {
        var message = "";
        $.each(errorList, function (index, value)
        {
            $(value.element).css("background-color", "#FFD7D7");
            message += value.message + "</br>";
        });
        if (message != "")
        {
            layerAlert(message.substr(0, message.length - 5));
        }
    }

    // this.defaultShowErrors();
}

function hightlightValidationErrors(errorMap, errorList) {
    $.each(this.successList, function (index, value) {
        $(value).css("background-color", "#FFF");
    });

    if (errorList.length > 0) {
        $.each(errorList, function (index, value) {
            $(value.element).css("background-color", "#FFD7D7");
        });
    }

    // this.defaultShowErrors();
}

///////////

//jsTree
//获取选中节点
//以及选中节点的父节点Id
function getJsTreeSelectedNodesIdArray(treeviewId, args) {
    if (args == undefined) {
        args = new Object();
    }

    //默认查找上级节点，除非手工设置为false
    if (args.parents == undefined || args.parents == null) {
        args.parents = true;
    }

    var selectedNodeList = $('#' + treeviewId).jstree().get_checked(true);
    //alert(selectedNodeList.length);
    var result = new Array();
    var flag;
    for (var i = 0; i < selectedNodeList.length; i++) {

        var id = $(selectedNodeList[i]).attr("id");
      //  alert(selectedNodeList[i].li_attr.type);

        var filterResult = true;
        if (args.filter != undefined && args.filter != null)
        {
            filterResult = args.filter(selectedNodeList[i]);
        }

        if (filterResult) {
            var exist = false;
            for (var k = 0; k < result.length; k++) {
                //  console.log(selectedNodeList[i]);
                if (id === result[k]) {
                    exist = true;
                    break;
                }
            }
            if (exist === false) {
                result.push(id);
            }
        }
       
        if (args.parents == true) {

            var parents = selectedNodeList[i].parents;

            for (var j = 0; j < parents.length - 1; j++) {
                id = parents[j];

                exist = false;
                for (var k = 0; k < result.length; k++) {
                    if (id === result[k]) {
                        exist = true;
                        break;
                    }
                }
                if (exist === false) {
                    result.push(id);
                }

            }

        }
    }

    return result;
}

function getJsTreeSelectedNodesArray(treeviewId, args) {
    if (args == undefined) {
        args = new Object();
    }

    //默认查找上级节点，除非手工设置为false
    if (args.parents == undefined || args.parents == null) {
        args.parents = true;
    }

    var selectedNodeList = $('#' + treeviewId).jstree().get_checked(true);
    //alert(selectedNodeList.length);
    var result = new Array();
    var flag;
    for (var i = 0; i < selectedNodeList.length; i++) {

        var id = $(selectedNodeList[i]).attr("id");
        var text = $(selectedNodeList[i]).attr("text");
        //  alert(selectedNodeList[i].li_attr.type);

        var filterResult = true;
        if (args.filter != undefined && args.filter != null) {
            filterResult = args.filter(selectedNodeList[i]);
        }

        if (filterResult) {
            var exist = false;
            for (var k = 0; k < result.length; k++) {
                //  console.log(selectedNodeList[i]);
                if (id === result[k].id) {
                    exist = true;
                    break;
                }
            }
            if (exist === false) {
                var newNode = new Object();
                newNode.id = id;
                newNode.text = text;

                result.push(newNode);
            }
        }

        if (args.parents == true) {

            var parents = selectedNodeList[i].parents;

            for (var j = 0; j < parents.length - 1; j++) {
                id = parents[j];

                exist = false;
                for (var k = 0; k < result.length; k++) {
                    if (id === result[k].id) {
                        exist = true;
                        break;
                    }
                }
                if (exist === false) {

                    var newNode = new Object();
                    newNode.id = id;
                    newNode.text = text;

                    result.push(newNode);
                }

            }

        }
    }

    return result;
}


function setJsTreeSelectedNodes(treeviewId,selectedIdArray) {
    if (selectedIdArray == undefined)
        return;

    for (var i = 0; i < selectedIdArray.length; i++) {
        var isParent = $("#" + treeviewId).jstree().is_parent(selectedIdArray[i]);
        if (isParent == false) {
            $("#" + treeviewId).jstree('select_node', selectedIdArray[i]);
        }
    }

}


///////////////

//ApiResult
//Success = false
function handleApiResult(data) {
    if (data.Success)
        return;

    switch (data.Reason) {
        case 7001:
            layerAlert(data.Message, function () {
                getTopWindow().location.href = "/Home/Login";
            });
            break;
        default:
            layerAlert(data.Message);
            break;
    }
}

// 对Date的扩展，将 Date 转化为指定格式的String 
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
// var submitTime = new Date(d[i].SubmitTime).format("yyyy-MM-dd hh:mm:ss");
// var time = new Date(d[i].LimitedTime.replace(/-/g,"/")).format("yyyy-MM-dd ")
Date.prototype.format = function (fmt)
{ //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

String.format = function () {
    if (arguments.length == 0)
        return null;

    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}

//维持session
function heartbeat()
{
    setInterval(function ()
    {
        $.get("/Api/UserContext/Heartbeat");
    },60000);    
}

//DTO操作相关

function __getDto(containerId, dto) {
    $("#" + containerId + " [dtoproperty]").each(function (n, element) {
        var dtoproperty = $(element).attr("dtoproperty");
        var dtopropertytype = $(element).attr("dtopropertytype");
        var elementVal = $(element).val();
        //console.log(dtoproperty);
        if (dtopropertytype == undefined || dtopropertytype == null || dtopropertytype=="")
        {
            dto[dtoproperty] = elementVal;
        }else{
            switch (dtopropertytype) {
                case "date":  //截取日期部分
                    if (elementVal != null && elementVal != "") {
                        dto[dtoproperty] = elementVal.substr(0, 10);
                    }
                    break;
                default:
                    alert("dtopropertytype 不支持：" + dtopropertytype);
                    break;
            }
        }
    });
}

function __setDto(containerId, dto, skipProperties) {
    $("#" + containerId + " [dtoproperty]").each(function (n, element) {

        var dtoproperty = $(element).attr("dtoproperty");

        if (dtoproperty == undefined || dtoproperty == null || dtoproperty == "") {
            return;
        }

        if (skipProperties != undefined && skipProperties != null && skipProperties.indexOf(dtoproperty) >= 0) {
            return;
        }

        var dtopropertytype = $(element).attr("dtopropertytype");
        var dtopropertytrigger = $(element).attr("dtopropertytrigger");
        var dtoPropertyValue = dto[dtoproperty];

        if (dtopropertytype == undefined || dtopropertytype == null || dtopropertytype == "") {
            if (dtoPropertyValue != null) {
                //.toString()  如果是数组，就不能加toString，把数组设置到select2控件时，会把数组变成一个字符串而使之无效
                // 但对于一般下拉框控件，如果后台返回的是 bool 类型的值，不 toString 的话，又不能使之正确设置到下拉框控件
                //  alert(dto[dtoproperty].toString());
                //http://www.cnblogs.com/mofish/p/3388427.html js数据类型判断和数组判断
                var value = dto[dtoproperty];
                //console.log(typeof (value));
                if (typeof (value) == "boolean") {
                    value = value.toString();
                }
                $(element).val(value);

            } else {
                $(element).val("");
            }
        } else {
            switch (dtopropertytype) {
                case "date":  //截取日期部分
                    if (dtoPropertyValue != null) {
                        $(element).val(dtoPropertyValue.substr(0, 10));
                    }
                    break;
                default:
                    alert("dtopropertytype 不支持：" + dtopropertytype);
                    break;
            }
        }

        if (dtopropertytrigger != undefined && dtopropertytrigger != null && dtopropertytrigger != "") {
            $(element).trigger(dtopropertytrigger);
        }
    });
}

//加载字典
function __getDictionaryItems(args,callback) {
    var loadLayerIndex = layer.load(0, {
        shade: [0.2, '#fff']
    });


    $.ajax({
        url: "/Api/Dictionary/GetMultipleDictionaryItemWholeList/",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(args),
        success: function (data, status, jqXHR) {
            layer.close(loadLayerIndex);
            if (data.Successful) {

                if (callback != undefined && callback != null)
                    callback(data.Data);

            } else {
                handleApiResult(data);
            }
        },
        error: function (xmlHttpRequest) {
            layer.close(loadLayerIndex);
            alert("Error: " + xmlHttpRequest.status);
        }
    });
}

function __loadDictionaryItemsToSelectControl(args, callback) {
    var loadLayerIndex = layer.load(0, {
        shade: [0.2, '#fff']
    });


    $.ajax({
        url: "/Api/Dictionary/GetMultipleDictionaryItemWholeList/",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(args),
        success: function (data, status, jqXHR) {
            layer.close(loadLayerIndex);
            if (data.Successful) {

                for (var targetName in data.Data) {
                    $("#" + targetName).html("");
                    var items = data.Data[targetName];

                    //是否给每个下拉框前面加上空白选项
                    if (args.Parameter_AllowedEmptyOption != undefined && args.Parameter_AllowedEmptyOption != null && args.Parameter_AllowedEmptyOption == true) {
                        if (args.Parameter_AllowedEmptyOptionText != undefined && args.Parameter_AllowedEmptyOptionText != null && args.Parameter_AllowedEmptyOptionText != "") {
                            $("#" + targetName).append("<option value='' dictionaryitemvalue=''>" + args.Parameter_AllowedEmptyOptionText + "</option>");
                        } else {
                            $("#" + targetName).append("<option value='' dictionaryitemvalue=''></option>");
                        }
                    }
                    else {
                        //判断这个特定的下拉框是否允许空白选项
                        if ($("#" + targetName).attr("allowedEmptyOption") == "true") {
                            var text = $("#" + targetName).attr("allowedEmptyOptionText");
                            if (text!=undefined && text!=null && text != "") {
                                $("#" + targetName).append("<option value='' dictionaryitemvalue=''>" + text  + "</option>");
                            }
                            else {
                                $("#" + targetName).append("<option value='' dictionaryitemvalue=''></option>");
                            }
                        }
                    }

                    for (var i = 0 ; i < items.length; i++) {
                        $("#" + targetName).append("<option value='" + items[i].Id + "' dictionaryitemvalue='" + items[i].Value + "'>" + items[i].Text + "</option>");
                    }

                    $("#" + targetName).trigger("change");
                }
                
                if (callback != undefined && callback != null)
                    callback();

            } else {
                handleApiResult(data);
            }
        },
        error: function (xmlHttpRequest) {
            layer.close(loadLayerIndex);
            alert("Error: " + xmlHttpRequest.status);
        }
    });
}

//联动两个下拉框，并自动从字典加载数据
function __selectCascading(select1Id, select2Id) {
    $("#" + select1Id).change(function () {
        var selectedValue = $(this).val();
        if (selectedValue == null || selectedValue == "") {
            $("#" + select2Id).html("");
            $("#" + select2Id).trigger("change");
            return;
        }

        var args = new Object();
        args[select2Id] = selectedValue;
        __loadDictionaryItemsToSelectControl(args);
    });
}

//公共方法

var __clone = (function () {
    return function (obj) { Clone.prototype = obj; return new Clone() };
    function Clone() { }
}());

function __createSerialNumber(module,callback) {

    var loadLayerIndex = layer.load(0, {
        shade: [0.2, '#fff']
    });

    //$.get("/Api/System/CreateSerialNumber?module=" + module, function (data) {
    //    layer.closeAll();
    //    alert(data.Data);
    //    return data.Data;
    //},"json");


    $.ajax({
        url: "/Api/System/CreateSerialNumber?module=" + module,
        type: "POST",
        dataType: "json",
        async: false,
        success: function (data, status, jqXHR) {
            layer.closeAll();

            if (data.Successful) {
               
                callback(data.Data);

            } else {
                handleApiResult(data);
            }
        },
        error: function (xmlHttpRequest) {
            layer.closeAll();
            alert("Error: " + xmlHttpRequest.status);
        }
    });
}

function __scrollTableHeader() {
    // alert(divTableBodyContainer.scrollLeft);
    var ml = 0 - divTableBodyContainer.scrollLeft;
    document.getElementById("tableHeader").style.cssText = "margin-left:" + ml + "px;";
}

function __calculateTbodyWidth(thead, tbody) {
    $(tbody).find("table").find("tr:eq(0)").each(function () {
        $(this).find("td").each(function (index) {
            var a = $(thead).find("table").find("tr:eq(0)").find("th:eq(" + index + ")").outerWidth();
            var b = $(this).outerWidth();
            console.log(a, b, a > b);
            if (b > a) {
                $(thead).find("table").find("tr:eq(0)").find("th:eq(" + index + ")").css({
                    "width": b + "px",
                    "min-width": b + "px"
                });
            } else if (a > b) {
                $(this).css({
                    "width": a + "px",
                    "min-width": a + "px"
                });
                $(thead).find("table").find("tr:eq(0)").find("th:eq(" + index + ")").css({
                    "width": a + "px",
                    "min-width": a + "px"
                });
            }
        });
    });
    var a = $(".divDataTable_thead").find("table").outerWidth();  /*占时用不到，不知道以后会不会*/
    var b = $(".divDataTable_tbody").find("table").outerWidth();  /*占时用不到，不知道以后会不会*/
}


//args
//  msg
//  apiUrl
//  data
//  callback
function __requestApi(args) {

    if (args.msg == undefined || args.msg == null || args.msg == "") {

        var loadLayerIndex = layer.load(0, {
            shade: [0.2, '#fff']
        });

        $.ajax({
            url: args.apiUrl,
            type: "POST",
            dataType: "json",
            data: args.data == undefined || args.data == null ? null : JSON.stringify(args.data),
            success: function (data, status, jqXHR) {
                layer.closeAll();

                if (data.Successful) {
                    if (args.callback != undefined && args.callback != null) {
                        args.callback(data);
                    }
                    //else {
                    //    var index = parent.layer.getFrameIndex(window.name);
                    //    parent.__loadDataOnPageAndCloseLayer(index);
                    //}

                } else {
                   
                    handleApiResult(data);
                }
            },
            error: function (xmlHttpRequest) {
                layer.closeAll();
                //alert("Error: " + xmlHttpRequest.status);  防止页面AJAX请求没完成时，点了连接跳转，弹出 Error:0
            }
        });

    } else {
        var confirmLayerIndex = layer.confirm(args.msg, {
            btn: ['确认', '取消'], //按钮
            shade: [0.4, '#393D49'],
            title: false,
            closeBtn: false,
            shift: _layerShift
        }, function () {
            layer.close(confirmLayerIndex);

            var loadLayerIndex = layer.load(0, {
                shade: [0.2, '#fff']
            });

            $.ajax({
                url: args.apiUrl,
                type: "POST",
                dataType: "json",
                data: args.data == undefined || args.data == null ? null : JSON.stringify(args.data),
                success: function (data, status, jqXHR) {
                    layer.closeAll();
                    if (data.Successful) {
                   //     console.log(JSON.stringify(data));

                        if (args.callback != undefined && args.callback != null) {
                            args.callback(data);
                        }
                        //else {
                        //    var index = parent.layer.getFrameIndex(window.name);
                        //    parent.__loadDataOnPageAndCloseLayer(index);
                        //}

                    } else {
                        handleApiResult(data);
                    }
                },
                error: function (xmlHttpRequest) {
                    layer.closeAll();
                  //  alert("Error: " + xmlHttpRequest.status);
                }
            });
        });
    }
}


function __setReadOnly(elementName) {

    if (elementName == undefined || elementName == null)
        elementName = "";

    if (elementName != "") {
        elementName = "#" + elementName + " ";
    }

    $(elementName + "input").each(function (index, element) {

        //不能直接把按钮也disabled，有很多页面是直接把整个容器__setReadOnly的，会使基本操作按钮 ，如打印、取消也灰掉
        //这里通过增加一个属性来控制

        var elementType = $(element).attr("type");

        if (elementType == "text") {
            $(element).attr("class", "input_readonly");
            $(element).attr("readonly", "readonly");
        }
        else if (elementType == "button") {
            if ($(element).attr("setreadonly") == "disabled") {
                $(element).attr("disabled", "disabled");
            }
        }
    });

    $(elementName + "select").each(function (index, element) {
        $(element).attr("class", "input_readonly");
        $(element).attr("disabled", "disabled");
    });

    $(elementName + "div[divselector]").each(function (index, element) {
        $(element).removeClass("divBorder_gray");
    });

    $(elementName + "[operationalarea]").each(function (index, element) {
        $(element).hide();
    });
}

function __setNotReadOnly(elementName) {

    if (elementName == undefined || elementName == null)
        elementName = "";

    if (elementName != "") {
        elementName = "#" + elementName + " ";
    }

    $(elementName + "input").each(function (index, element) {

        var elementType = $(element).attr("type");

        if (elementType == "text") {
            $(element).attr("class", "input_16");
            $(element).removeAttr("readonly");
        }
        else if (elementType == "button") {
            if ($(element).attr("setreadonly") == "disabled") {
                $(element).removeAttr("disabled");
            }
        }

    });

    $(elementName + "select").each(function (index, element) {
        $(element).attr("class", "input_16");
        $(element).removeAttr("disabled");
    });

    $(elementName + "div[divselector]").each(function (index, element) {
        $(element).addClass("divBorder_gray");
    });

    $(elementName + "[operationalarea]").each(function (index, element) {
        $(element).show();
    });
}



//处理省市区三级联动

var __areaListRefreshing = false;

//处理省市区下拉框
//args.selectProvinceId
//args.selectCityId
//args.selectAreaId
//args.provinceId
//args.cityId
//args.areaId
//args.callback
function __loadAreaList(args) {

    $("#" + args.selectProvinceId).select2();
    $("#" + args.selectCityId).select2();
    $("#" + args.selectAreaId).select2();

    var getAreaRelatedWrapperArgs = new Object();
    getAreaRelatedWrapperArgs.ProvinceId = args.provinceId;
    getAreaRelatedWrapperArgs.CityId = args.cityId;

    var requestApiArgs = new Object();
    requestApiArgs.apiUrl = "/Api/Area/GetAreaRelatedWrapper/";
    requestApiArgs.data = getAreaRelatedWrapperArgs;
    requestApiArgs.callback = function (data) {

        if (args.AllowedEmptyOption != undefined && args.AllowedEmptyOption != null && args.AllowedEmptyOption == true) {
            if (args.AllowedEmptyOptionText != undefined && args.AllowedEmptyOptionText != null && args.AllowedEmptyOptionText != "") {
                $("#" + args.selectProvinceId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                $("#" + args.selectCityId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                $("#" + args.selectAreaId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
            } else {
                $("#" + args.selectProvinceId).append("<option value='' dictionaryitemvalue=''></option>");
                $("#" + args.selectCityId).append("<option value='' dictionaryitemvalue=''></option>");
                $("#" + args.selectAreaId).append("<option value='' dictionaryitemvalue=''></option>");
            }
        }

        //加载省
        __loadAreaDataToSelect2(data.Data.ProvinceList, args.selectProvinceId, args.provinceId);
        
        //加载市
        __loadAreaDataToSelect2(data.Data.CityList, args.selectCityId, args.cityId);

        //加载区
        __loadAreaDataToSelect2(data.Data.AreaList, args.selectAreaId, args.areaId);
       

        //绑定省回调
        $("#" + args.selectProvinceId).change(function () {

            if (__areaListRefreshing)
                return;

            $("#" + args.selectCityId).html("");

            var provinceId = $("#" + args.selectProvinceId).val();

            if (provinceId == null || provinceId == "") {
                $("#" + args.selectCityId).trigger("change");
                return;
            }

            var requestApiArgs = new Object();
            requestApiArgs.apiUrl = "/Api/Area/GetAreaSimpleList/" + provinceId;
            requestApiArgs.callback = function (data) {

                if (args.AllowedEmptyOption != undefined && args.AllowedEmptyOption != null && args.AllowedEmptyOption == true) {
                    if (args.AllowedEmptyOptionText != undefined && args.AllowedEmptyOptionText != null && args.AllowedEmptyOptionText != "") {
                        $("#" + args.selectCityId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                    } else {
                        $("#" + args.selectCityId).append("<option value='' dictionaryitemvalue=''></option>");
                    }
                }

                __loadAreaDataToSelect2(data.Data, args.selectCityId, null);              

            }

            __requestApi(requestApiArgs);
        });

        //绑定市回调
        $("#" + args.selectCityId).change(function () {

            if (__areaListRefreshing)
                return;

            $("#" + args.selectAreaId).html("");

            var cityId = $("#" + args.selectCityId).val();

            if (cityId == null || cityId == "") {
                $("#" + args.selectAreaId).trigger("change");
                return;
            }

            var requestApiArgs = new Object();
            requestApiArgs.apiUrl = "/Api/Area/GetAreaSimpleList/" + cityId;
            requestApiArgs.callback = function (data) {


                if (args.AllowedEmptyOption != undefined && args.AllowedEmptyOption != null && args.AllowedEmptyOption == true) {
                    if (args.AllowedEmptyOptionText != undefined && args.AllowedEmptyOptionText != null && args.AllowedEmptyOptionText != "") {
                        $("#" + args.selectAreaId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                    } else {
                        $("#" + args.selectAreaId).append("<option value='' dictionaryitemvalue=''></option>");
                    }
                }

                __loadAreaDataToSelect2(data.Data, args.selectAreaId, null);

              

            }

            __requestApi(requestApiArgs);
        });

        if (args.callback != undefined && args.callback != null)
            args.callback();
    }

    __requestApi(requestApiArgs);
}

//加载区域数据 
function __loadAreaDataToSelect2(data,select2Id,selectedValue) {
    var itemList = new Array();
    if (data != undefined && data != null) {
        for (var i = 0; i < data.length; i++) {
            var item = new Object();
            item.id = data[i].Id;
            item.text = data[i].Name;
            item.pinyin = data[i].Pinyin;
            item.jianpin = data[i].Jianpin;
            itemList[itemList.length] = item;
        }
    }
    $("#" + select2Id).select2({
        data: itemList,
        matcher: __select2matchAreaCustom
    });

    if (selectedValue != undefined && selectedValue != null && selectedValue != "") {
        $("#" + select2Id).val(selectedValue);
       
    }

    $("#" + select2Id).trigger("change");
}

//https://select2.org/searching
//http://blog.csdn.net/u012906135/article/details/70568932
//省市区下拉框select2控件的拼音选择
function __select2matchAreaCustom(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') {
        return data;
    }

    // Do not display the item if there is no 'text' property
    if (typeof data.text === 'undefined') {
        return null;
    }

    //console.log(JSON.stringify(data));

    // `params.term` should be the term that is used for searching
    // `data.text` is the text that is displayed for the data object
    if (data.text.indexOf(params.term) > -1
        || (data.pinyin != undefined && data.pinyin != null && data.pinyin.toUpperCase().indexOf(params.term.toUpperCase()) > -1)
        || (data.jianpin != undefined && data.jianpin != null && data.jianpin.toUpperCase().indexOf(params.term.toUpperCase()) > -1)) {
        var modifiedData = $.extend({}, data, true);
        //modifiedData.text += ' (matched)';

        // You can return modified objects from here
        // This includes matching the `children` how you want in nested data sets
        return modifiedData;
    }

    // Return `null` if the term should not be displayed
    return null;
}

//刷新省市区三级联动的选择
//args.selectProvinceId
//args.selectCityId
//args.selectAreaId
//args.provinceId
//args.cityId
//args.areaId
function __refreshAreaList(args) {
    var getAreaRelatedWrapperArgs = new Object();
    getAreaRelatedWrapperArgs.ProvinceId = args.provinceId;
    getAreaRelatedWrapperArgs.CityId = args.cityId;

    var requestApiArgs = new Object();
    requestApiArgs.apiUrl = "/Api/Area/GetAreaRelatedWrapper/";
    requestApiArgs.data = getAreaRelatedWrapperArgs;
    requestApiArgs.callback = function (data) {

        __areaListRefreshing = true;

        if (args.AllowedEmptyOption != undefined && args.AllowedEmptyOption != null && args.AllowedEmptyOption == true) {
            if (args.AllowedEmptyOptionText != undefined && args.AllowedEmptyOptionText != null && args.AllowedEmptyOptionText != "") {
                $("#" + args.selectProvinceId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                $("#" + args.selectCityId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                $("#" + args.selectAreaId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
            } else {
                $("#" + args.selectProvinceId).append("<option value='' dictionaryitemvalue=''></option>");
                $("#" + args.selectCityId).append("<option value='' dictionaryitemvalue=''></option>");
                $("#" + args.selectAreaId).append("<option value='' dictionaryitemvalue=''></option>");
            }
        }

        //加载省
        __loadAreaDataToSelect2(data.Data.ProvinceList, args.selectProvinceId, args.provinceId);

        //加载市
        __loadAreaDataToSelect2(data.Data.CityList, args.selectCityId, args.cityId);

        //加载区
        __loadAreaDataToSelect2(data.Data.AreaList, args.selectAreaId, args.areaId);

        __areaListRefreshing = false;

        if (args.callback != undefined && args.callback != null)
            args.callback();
    }

    __requestApi(requestApiArgs);

}


//处理商品品类三级联动

//加载品类类型品种下拉框
//允许不指定selectProductCatalogId控件，根据固定的productCatalogId做类型和品种的二级联动
function __loadProductCatalogList(args) {

    if (args.selectProductCatalogId != undefined && args.selectProductCatalogId != null && args.selectProductCatalogId != "")
    {
        $("#" + args.selectProductCatalogId).select2();
    }

    $("#" + args.selectProductTypeId).select2();
    $("#" + args.selectProductKindId).select2();

    var getProductCatalogRelatedWrapperArgs = new Object();
    getProductCatalogRelatedWrapperArgs.ProductCatalogId = args.productCatalogId;
    getProductCatalogRelatedWrapperArgs.ProductTypeId = args.productTypeId;
    getProductCatalogRelatedWrapperArgs.AllowedEmptyOption = args.AllowedEmptyOption;

    if (args.ForSale != undefined && args.ForSale != null && args.ForSale != "") {
        getProductCatalogRelatedWrapperArgs.ForSale = args.ForSale;
    }

    var requestApiArgs = new Object();
    requestApiArgs.apiUrl = "/Api/Product/GetProductCatalogRelatedWrapper/";
    requestApiArgs.data = getProductCatalogRelatedWrapperArgs;
    requestApiArgs.callback = function (data) {

        if (args.AllowedEmptyOption != undefined && args.AllowedEmptyOption != null && args.AllowedEmptyOption == true) {
            if (args.AllowedEmptyOptionText != undefined && args.AllowedEmptyOptionText != null && args.AllowedEmptyOptionText != "") {
                $("#" + args.selectProductCatalogId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                $("#" + args.selectProductTypeId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                $("#" + args.selectProductKindId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
            } else {
                $("#" + args.selectProductCatalogId).append("<option value='' dictionaryitemvalue=''>　</option>");
                $("#" + args.selectProductTypeId).append("<option value='' dictionaryitemvalue=''>　</option>");
                $("#" + args.selectProductKindId).append("<option value='' dictionaryitemvalue=''>　</option>");
            }
        }

        //加载品类
        if (args.selectProductCatalogId != undefined && args.selectProductCatalogId != null && args.selectProductCatalogId != "") {
            __loadProductCatalogDataToSelect2(data.Data.ProductCatalogList, args.selectProductCatalogId, args.productCatalogId);
        }

        //加载类型
        __loadProductCatalogDataToSelect2(data.Data.ProductTypeList, args.selectProductTypeId, args.productTypeId);

        //加载品种
        __loadProductCatalogDataToSelect2(data.Data.ProductKindList, args.selectProductKindId, args.productKindId);


        //绑定品类回调
        if (args.selectProductCatalogId != undefined && args.selectProductCatalogId != null && args.selectProductCatalogId != "") {
            $("#" + args.selectProductCatalogId).change(function () {

                $("#" + args.selectProductTypeId).html("");

                var productCatalogId = $("#" + args.selectProductCatalogId).val();

                if (productCatalogId == null || productCatalogId == "") {
                    $("#" + args.selectProductTypeId).trigger("change");
                    return;
                }

                var getProductCatalogListArgs = new Object();
                getProductCatalogListArgs.ParentItemId = productCatalogId;

                var requestApiArgs = new Object();
                requestApiArgs.apiUrl = "/Api/Product/GetProductCatalogList/";
                requestApiArgs.data = getProductCatalogListArgs;
                requestApiArgs.callback = function (data) {

                    if (args.AllowedEmptyOption != undefined && args.AllowedEmptyOption != null && args.AllowedEmptyOption == true) {
                        if (args.AllowedEmptyOptionText != undefined && args.AllowedEmptyOptionText != null && args.AllowedEmptyOptionText != "") {
                            $("#" + args.selectProductTypeId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                        } else {
                            $("#" + args.selectProductTypeId).append("<option value='' dictionaryitemvalue=''></option>");
                        }
                    }

                    __loadProductCatalogDataToSelect2(data.Data, args.selectProductTypeId, null);

                }

                __requestApi(requestApiArgs);
            });
        }
        //绑定类型回调
        $("#" + args.selectProductTypeId).change(function () {

            $("#" + args.selectProductKindId).html("");

            var productTypeId = $("#" + args.selectProductTypeId).val();

            if (productTypeId == null || productTypeId == "") {
                $("#" + args.selectProductKindId).trigger("change");
                return;
            }

            var getProductCatalogListArgs = new Object();
            getProductCatalogListArgs.ParentItemId = productTypeId;

            var requestApiArgs = new Object();
            requestApiArgs.apiUrl = "/Api/Product/GetProductCatalogList/";
            requestApiArgs.data = getProductCatalogListArgs;
            requestApiArgs.callback = function (data) {


                if (args.AllowedEmptyOption != undefined && args.AllowedEmptyOption != null && args.AllowedEmptyOption == true) {
                    if (args.AllowedEmptyOptionText != undefined && args.AllowedEmptyOptionText != null && args.AllowedEmptyOptionText != "") {
                        $("#" + args.selectProductKindId).append("<option value='' dictionaryitemvalue=''>" + args.AllowedEmptyOptionText + "</option>");
                    } else {
                        $("#" + args.selectProductKindId).append("<option value='' dictionaryitemvalue=''></option>");
                    }
                }

                __loadProductCatalogDataToSelect2(data.Data, args.selectProductKindId, null);



            }

            __requestApi(requestApiArgs);
        });

        if (args.callback != undefined && args.callback != null)
            args.callback();
    }
  

    __requestApi(requestApiArgs);
}


//加载商品品类数据 
function __loadProductCatalogDataToSelect2(data, select2Id, selectedValue) {

    if (data == null)
        return;

    var itemList = new Array();
    for (var i = 0; i < data.length; i++) {
        var item = new Object();
        item.id = data[i].Id;
        item.text = data[i].Name;
        item.code = data[i].Code;
        itemList[itemList.length] = item;
    }
    $("#" + select2Id).select2({
        data: itemList,
        matcher: __select2matchProductCatalogCustom
    });

    if (selectedValue != undefined && selectedValue != null && selectedValue != "") {
        $("#" + select2Id).val(selectedValue);

    }

    $("#" + select2Id).trigger("change");
}

//https://select2.org/searching
//http://blog.csdn.net/u012906135/article/details/70568932
//商品品类下拉框select2控件的Code选择
function __select2matchProductCatalogCustom(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') {
        return data;
    }

    // Do not display the item if there is no 'text' property
    if (typeof data.text === 'undefined') {
        return null;
    }

    //console.log(JSON.stringify(data));

    // `params.term` should be the term that is used for searching
    // `data.text` is the text that is displayed for the data object
    if (data.text.indexOf(params.term) > -1
        || (data.code != undefined && data.code != null && data.code.toUpperCase().indexOf(params.term.toUpperCase()) > -1)) {
        var modifiedData = $.extend({}, data, true);
        //modifiedData.text += ' (matched)';

        // You can return modified objects from here
        // This includes matching the `children` how you want in nested data sets
        return modifiedData;
    }

    // Return `null` if the term should not be displayed
    return null;
}
