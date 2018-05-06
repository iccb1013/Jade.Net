var __currentPage = 1;
var __loadListDataResultObj;

var __table;

function __loadDataAndCloseLayer(layerIndex) {
    layer.close(layerIndex);
    loadData();
}

function __loadDataOnPageAndCloseLayer(layerIndex) {
    layer.close(layerIndex);
    loadData(__currentPage);
}

//args.ApiUrl
//args.Args
//
function __loadListData(args) {

    var loadLayerIndex = layer.load(0, {
        shade: [0.2, '#fff']
    });

    $.ajax({
        url: args.ApiUrl,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(args.Args),
        success: function (data, status, jqXHR) {
            // alert(JSON.stringify(data));

            layer.close(loadLayerIndex);
            if (data.Successful) {
                var resultObj = data.Data;
                __loadListDataResultObj = resultObj;
                //alert(JSON.stringify(resultObj));

                //允许反回的结果不包含分页信息

                var rowNo = 1;

                if (resultObj.PagingInfo != undefined && resultObj.PagingInfo != null) {
                    __currentPage = resultObj.PagingInfo.CurrentPage;
                    rowNo = (__currentPage - 1) * resultObj.PagingInfo.PageSize + 1;
                } else {
                    __currentPage = 1;
                }

                for (var i = 0; i < resultObj.Data.length; i++) {
                    resultObj.Data[i].NO = rowNo;
                    rowNo++;
                }

                //判断是否提供了加工数据的特殊方法
                var data;
                if (isExitsFunction("processLoadedListData")) {
                    data = processLoadedListData(resultObj.Data);
                }
                else {
                    data = resultObj.Data;
                }

                __table.reload({
                    data: data
                });

                if (resultObj.PagingInfo != undefined && resultObj.PagingInfo != null) {
                    laypage({
                        skin: 'yahei',
                        cont: document.getElementById('divPagingContainer'),
                        pages: resultObj.PagingInfo.TotalPage, //总页数
                        curr: resultObj.PagingInfo.CurrentPage, //当前页
                        groups: 7, //连续显示分页数
                        jump: function (obj, first) {
                            if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                                loadData(obj.curr);
                            }
                        }
                    });
                }

            } else {
                //handleApiResult(data);
                handleApiResult(data);
            }
        },
        error: function (xmlHttpRequest) {
            layer.close(loadLayerIndex);
            alert("Error: " + xmlHttpRequest.status);
        }
    });
}

