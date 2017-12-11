//Cookie过期时间(毫秒)
var cookie_ExpirationTime = 12 * 60 * 60 * 1000;

//写入cookies
function setCookie(name, value) {
    name = encodeURIComponent(encodeURIComponent(name));
    delCookie(name);
    try {
        value = JSON.stringify(value);
    } catch (e) {
        value = encodeURIComponent(encodeURIComponent(value));
    }
    localStorage.setItem(name, JSON.stringify({
        date: Date.parse(new Date()),
        value: value
    }));
}

//读取cookies
function getCookie(name) {
    name = encodeURIComponent(encodeURIComponent(name));
    var result = localStorage.getItem(name);
    try {
        result = JSON.parse(result);
        if (result.date == undefined ||
            Date.parse(new Date(result.date)) + cookie_ExpirationTime < Date.parse(new Date())) {
            //过期
            delCookie(name);
            return null;
        } else {
            try {
                return JSON.parse(result.value);
            } catch (ex) {
                return decodeURIComponent(decodeURIComponent(result.value));
            }
        }
    } catch (e) {
        delCookie(name);
        return null;
    }
}

//删除cookies
function delCookie(name) {
    localStorage.removeItem(name);
}