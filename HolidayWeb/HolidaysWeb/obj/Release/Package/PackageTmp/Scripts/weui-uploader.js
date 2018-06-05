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
        $.showLoading('上传中...');
    });

    // 文件上传成功
    uploader.on('uploadSuccess', function (file, response) {
        if (response.state == "0") {
            if (callback) {
                callback(uploadBaseUrl + response.url);
            } else {
                $.toast("上传成功！");
            }
        } else {
            $.toast(response.state,"forbidden");
        }
        $.hideLoading();
    });

    // 文件上传失败，现实上传出错。
    uploader.on('uploadError', function (file) {
        $.hideLoading();
        $.toast("上传失败，请重新再试！", "text");
    });
}
// 销毁百度上传图片组件
function fnDestroyUploader() {
    uploader.destroy();
}