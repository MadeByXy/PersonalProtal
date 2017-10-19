//写入cookies
function setCookie(name, value) {
    name = encodeURIComponent(encodeURIComponent(name));
    delCookie(name);
    try {
        value = JSON.stringify(value);
    } catch (e) {
        value = encodeURIComponent(encodeURIComponent(value));
    }
    localStorage.setItem(name, value);
}

//读取cookies
function getCookie(name) {
    name = encodeURIComponent(encodeURIComponent(name));
    var result = localStorage.getItem(name);
    try {
        return JSON.parse(result);
    } catch (e) {
        return decodeURIComponent(decodeURIComponent(result));
    }
    return result;
}

//删除cookies
function delCookie(name) {
    localStorage.removeItem(name);
}