function jsonDateFormat(jsonDt, format) {

    var date, timestamp, dtObj;

    timestamp = jsonDt.replace(/\/Date\((\d+)\)\//, "$1");

    date = new Date(Number(timestamp));

    dtObj = {
        "M+": date.getMonth() + 1,   //月
        "d+": date.getDate(),        //日
        "h+": date.getHours(),       //时
        "m+": date.getMinutes(),     //分
        "s+": date.getSeconds(),     //秒
    };

    //因为年份是4位数，所以单独拿出来处理
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    //遍历dtObj
    for (var k in dtObj) {

        //dtObj的属性名作为正则进行匹配
        if (new RegExp("(" + k + ")").test(format)) {

            //月，日，时，分，秒 小于10时前面补 0
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? dtObj[k] : ("00" + dtObj[k]).substr(("" + dtObj[k]).length));
        }
    }

    return format;
}

// 固定格式化日期时间
function jsonDatetimeFormat(jsonDt) {
    return jsonDateFormat(jsonDt, "yyyy-MM-dd hh:mm:ss");
}

// 百度上传图片组件封装
var uploader;
var uploadBaseUrl = "/Images/Upload/";
var loadIndex;

function fnUploadImage(btnId, callback) {

    // 初始化Web Uploader
    uploader = WebUploader.create({

        // 自动上传。
        auto: true,

        // swf文件路径
        swf: '/Framework/webuploader-0.1.5/Uploader.swf',

        // 文件接收服务端。
        server: '/Upload/UploadImage',

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#' + btnId,

        // 只允许选择文件，可选。
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/gif,image/jpg,image/jpeg,image/png,image/bmp'
        }
    });

    // 当文件被加入队列以后触发
    uploader.on("fileQueued", function (file) {
        loadIndex = layer.load();
    });

    // 文件上传成功
    uploader.on('uploadSuccess', function (file, response) {
        if (response.state == "0") {
            if (callback) {
                callback(uploadBaseUrl + response.url);
            } else {
                layer.alert("上传成功！", { icon: 1 });
            }
        } else {
            layer.alert(response.state, { icon: 2 });
        }

        layer.close(loadIndex);
    });

    // 文件上传失败，现实上传出错。
    uploader.on('uploadError', function (file) {
        layer.close(loadIndex);
        layer.alert("上传失败，请重新再试！", {icon : 2 });
    });
}

// 移动端上传图片
function fnWeUploadImage(btnId, callback) {
    // 初始化Web Uploader
    uploader = WebUploader.create({

        // 自动上传。
        auto: true,

        // swf文件路径
        swf: '/Framework/webuploader-0.1.5/Uploader.swf',

        // 文件接收服务端。
        server: '/Upload/UploadImage',

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#' + btnId,

        // 只允许选择文件，可选。
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/gif,image/jpg,image/jpeg,image/png,image/bmp'
        }
    });

    // 当文件被加入队列以后触发
    uploader.on("fileQueued", function (file) {
        loadIndex = layerload();
    });

    // 文件上传成功
    uploader.on('uploadSuccess', function (file, response) {
        if (response.state == "0") {
            if (callback) {
                callback(uploadBaseUrl + response.url);
            } else {
                layermsg("上传成功！");
            }
        } else {
            layermsg(response.state);
        }

        layer.close(loadIndex);
    });

    // 文件上传失败，现实上传出错。
    uploader.on('uploadError', function (file) {
        layer.close(loadIndex);
        layermsg("上传失败，请重新再试！");
    });
}

// 销毁百度上传图片组件
function fnDestroyUploader() {
    uploader.destroy();
}

// 校验手机号
function fnCheckPhone(phoneNo) {
    if (phoneNo && phoneNo.length > 0) {
        var phoneRex = /^0?1[3|4|5|7|8][0-9]\d{8}$/;
        return phoneRex.test(phoneNo);
    }

    return false;
}

// 校验身份证号
function fnCheckIdCard(idcard) {
    if (idcard && idcard.length > 0) {
        var idcardRex = /^(\d{18})|(\d{17}[X|x])$/;
        return idcardRex.test(idcard);
    }
    
    return false;
}

// 校验邮箱地址
function fnCheckEmail(email) {
    if (email && email.length > 0) {
        var emailRex = /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/;
        return emailRex.test(email);
    }
    
    return false;
}

function layermsg(msg, callback) {
    if (callback) {
        layer.open({
            content: msg,
            btn: '确定',
            yes: callback
        });
    } else {
        layer.open({
            content: msg,
            btn: '确定'
        });
    }
}

function layerload() {
    return layer.open({ type: 2, shadeClose: false });
}