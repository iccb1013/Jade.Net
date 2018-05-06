
function __requestLoadDataApi(apiUrl, callback) {
    var loadLayerIndex = layer.load(0, {
        shade: [0.2, '#fff']
    });

    $.ajax({
        url: apiUrl,
        type: "POST",
        dataType: "json",
        success: function (data, status, jqXHR) {
            layer.close(loadLayerIndex);
            if (data.Successful) {
                if (callback != undefined && callback != null) {
                    callback(data);
                }
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

function __requestSaveDataApi(apiUrl, data, mode, callback) {
    var loadLayerIndex = layer.load(0, {
        shade: [0.2, '#fff']
    });

    $.ajax({
        url: apiUrl,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (data, status, jqXHR) {
            layer.close(loadLayerIndex);
            if (data.Successful) {

                if (callback != undefined && callback != null) {
                    callback(data);
                }

                if (mode != undefined && mode != null && mode != "") {
                    var index = parent.layer.getFrameIndex(window.name);
                    if (mode == "create") {
                        parent.__loadDataAndCloseLayer(index);
                    } else {
                        parent.__loadDataOnPageAndCloseLayer(index, _data.Name);
                    }
                }

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

function __requestRemoveDataApi(apiUrl, callback) {

    var msg = "是否确认删除？"

    var confirmLayerIndex = layer.confirm(msg, {
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
            url: apiUrl,
            type: "POST",
            dataType: "json",
            success: function (data, status, jqXHR) {
                if (data.Successful) {
                    if (callback != undefined && callback != null) {
                        callback();
                    }
                    else {
                        var index = parent.layer.getFrameIndex(window.name);
                        parent.__loadDataOnPageAndCloseLayer(index);
                    }

                } else {
                    layer.closeAll();
                    handleApiResult(data);
                }
            },
            error: function (xmlHttpRequest) {
                layer.closeAll();
                alert("Error: " + xmlHttpRequest.status);
            }
        });
    });
}

//关闭Frame弹窗，如编辑窗口
function __closePopupFrameLayer() {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}
