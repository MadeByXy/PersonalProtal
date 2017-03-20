﻿//写cookies
function setCookie(name, value, json) {
    var exp = new Date();
    exp.setTime(exp.getTime() + 60 * 60 * 1000);
    document.cookie = name + "=" + escape(json ? JSON.stringify(value) : value) + ";expires=" + exp.toGMTString();
}

//读取cookies
function getCookie(name, json) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg)) {
        var result = unescape(arr[2]);
        return json ? JSON.parse(result) : result;
    }
    else { return null; }
}

//删除cookies
function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}