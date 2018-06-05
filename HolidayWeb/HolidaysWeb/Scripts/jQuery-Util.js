/**
 * 名称：jQuery 工具箱
 * 创建日期：2018-01-20
 * 更新日期：2018-01-20
 * 开发者：helang.love@qq.com
 * 联系QQ:1846492969
 */

$.extend({
    Util:{
        /* 浏览器 */
        browser:{
            IE: !!document.all,
            IE6: !!document.all && !window.XMLHttpRequest,
            IE7: !!document.all && /msie 7.0/gi.test(window.navigator.appVersion),
            IE8: !!document.all && /msie 8.0/gi.test(window.navigator.appVersion),
            Firefox: /firefox/gi.test(window.navigator.userAgent),
            Opera: /opera/gi.test(window.navigator.userAgent),
            Chrome: /Chrom/gi.test(window.navigator.userAgent)
        },
        /* 终端 */
        terminal:function (key) {
            var regs={
                /* 安卓 */
                "android":/android/i,
                /* 平果平板电脑 */
                "iPad":/ipad/i,
                /* 苹果手机 */
                "iphone":/iphone/i,
                /* 苹果操作系统 */
                "mac":/macintosh/i,
                /* 微软操作系统 */
                "windows":/windows/i,
                /* 移动端设备 */
                "mobileEnd":/(nokia|iphone|android|ipad|motorola|^mot\-|softbank|foma|docomo|kddi|up\.browser|up\.link|htc|dopod|blazer|netfront|helio|hosin|huawei|novarra|CoolPad|webos|techfaith|palmsource|blackberry|alcatel|amoi|ktouch|nexian|samsung|^sam\-|s[cg]h|^lge|ericsson|philips|sagem|wellcom|bunjalloo|maui|symbian|smartphone|midp|wap|phone|windows ce|iemobile|^spice|^bird|^zte\-|longcos|pantech|gionee|^sie\-|portalmmm|jig\s browser|hiptop|^ucweb|^benq|haier|^lct|opera\s*mobi|opera\*mini|320x320|240x320|176x220)/i
            };
            return regs[key].test(window.navigator.userAgent);
        },
        /*获取地址栏参数
        * key:要查询的字段，缺省返回地址栏所有数据
        * */
        getQueryString: function (key) {
            var search=window.location.search;
            if(search.length<1){
                return false;
            }
            if(key && search.indexOf(key)<0){
                return false;
            }
            search=search.substring(1);
            search=decodeURI(search);
            var searchArr=search.split("&");
            var searchJSON={};
            for(var i=0;i<searchArr.length;i++){
                var item=searchArr[i].split("=");
                searchJSON[item[0]]=item[1];
            }
            if(key){
                return searchJSON[key];
            }else {
                return searchJSON;
            }
        },
        /*获取随机数*/
        getRandom:function (min,max) {
            return Math.floor(Math.random() * (max-min))+min+1;
        },
        /*字符串截取*/
        substring:function (str,length) {
            if(str.length>length){
                return str.substring(0,length)+"...";
            }else {
                return str;
            }
        },
        /* 操作cookie*/
        cookie: {
            /* 设置 */
            set:function (key,val,len,unit) {
                if(!key || typeof val=='undefined'){
                    return false;
                }
                var cookieVal=val;
                if(typeof cookieVal=='object'){
                    var valArr=[];
                    $.each(cookieVal,function (val_k,val_v) {
                        valArr.push(val_k+"="+val_v);
                    });
                    cookieVal=valArr.join("&");
                }
                var len=len || 1;
                var unit=unit || "d";
                var units={
                    "d":len*24*60*60*1000,
                    "h":len*60*60*1000,
                    "m":len*60*1000,
                    "s":len*1000
                };
                var dt = new Date();
                dt.setTime(dt.getTime() + (units[unit]));
                var expires = ";expires=" + dt.toGMTString();
                var path=';path=/', domain='', secure='';
                document.cookie=[key, '=', cookieVal, expires, path, domain, secure].join('');
            },
            /* 获取
            * key:cookie的名称，必传参数
            * name:cookie中的指定值，缺省默认返回cookie下所有值
            * */
            get:function (key,name) {
                var cookieStr=document.cookie;
                var start=cookieStr.indexOf(key+"=");
                if(!key || start<0){
                    return false;
                }
                cookieStr=cookieStr.substring(start);
                var end=cookieStr.indexOf(";");
                if(end>=0){
                    cookieStr=cookieStr.substring(0,end);
                }
                cookieStr=cookieStr.substring(key.length+1);
                cookieStr=decodeURIComponent(cookieStr);
                var keyArr=cookieStr.split("&");
                if(keyArr.length<2){
                    return keyArr[0];
                }
                var keyJSON={};
                for(var i=0;i<keyArr.length;i++){
                    var item=keyArr[i].split("=");
                    keyJSON[item[0]]=item[1];
                }
                if(!name){
                    return keyJSON;
                }else {
                    if(keyJSON.hasOwnProperty(name)){
                        return keyJSON[name];
                    }else {
                        return false;
                    }
                }
            },
            /* 删除 */
            del:function (key) {
                this.set(key,"",-365);
            }
        },
        /* 序列化表单数据为JSON格式 */
        serializeJSON: function (obj) {
            var formArr = obj.serializeArray();
            var formObj = new Object();
            $.each(formArr, function () {
                if (formObj[this.name]) {
                    if (!formObj[this.name].push) {
                        formObj[this.name] = [formObj[this.name]];
                    }
                    formObj[this.name].push(this.value || '');
                } else {
                    formObj[this.name] = this.value || '';
                }
            });
            return formObj;
        },
        /*window对象*/
        win: {
            /*窗口跳转地址*/
            href: function (url, time) {
                var time = time || 0;
                setTimeout(function () {
                    window.location.href = url;
                }, time);
            },
            /*打开新窗口*/
            open: function (url, time) {
                var time = time || 0;
                setTimeout(function () {
                    window.open(url);
                }, time);
            },
            /*回退历史记录*/
            back: function () {
                window.history.back();
            },
            /*刷新当前页面*/
            read: function (time) {
                var time = time || 0;
                setTimeout(function () {
                    window.location.reload();
                }, time);
            }
        },
        /*校验*/
        verify:function (key,val) {
            var regs={
                /* 邮箱 */
                "email":/^[0-9a-zA-Z_]+@[0-9a-zA-Z_]+[\.]{1}[0-9a-zA-Z]+[\.]?[0-9a-zA-Z]+$/,
                /* 手机 */
                "mobile":/^(0|86|17951)?(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$/,
                /* 超链接 */
                "url":/^https?:\/\/(([a-zA-Z0-9_-])+(\.)?)*(:\d+)?(\/((\.)?(\?)?=?&?[a-zA-Z0-9_-](\?)?)*)*$/i,
                /* 身份证 */
                "idCard":/^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$/,
                /* 邮政编码 */
                "postal":/^[1-9]\d{5}(?!\d)$/,
                /* YY-MM-dd 格式日期 */
                "date":/^[1-2][0-9][0-9][0-9]-[0-1]{0,1}[0-9]-[0-3]{0,1}[0-9]$/,
                /* QQ */
                "qq":/^[1-9][0-9]{4,9}$/,
                /* 全部为数字 */
                "numAll":/"^\d+$/,
                /* 适合的帐号 英文/数字 */
                "befitName":/^[a-z0-9]+$/i,
                /* 适合的密码 英文/数字/下划线 */
                "befitPwd":/^\w+$/
            };
            return regs[key].test(val);
        }
    }
});

