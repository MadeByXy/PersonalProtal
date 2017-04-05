//写入cookies
function setCookie(name, value, isJson) {
    name = encodeURIComponent(encodeURIComponent(name));
    var exp = new Date();
    exp.setTime(exp.getTime() + 60 * 60 * 1000);
    var temp;
    try { temp = JSON.stringify(value); value = temp; } catch (e) { value = encodeURIComponent(encodeURIComponent(value)); }
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

//读取cookies
function getCookie(name, isJson) {
    name = encodeURIComponent(encodeURIComponent(name));
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg)) {
        var result = unescape(arr[2]);
        try {
            return JSON.parse(result);
        } catch (e) {
            return decodeURIComponent(decodeURIComponent(result));
        }
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