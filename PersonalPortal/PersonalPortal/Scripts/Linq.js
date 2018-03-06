//返回第一项匹配的内容，如果没有匹配成功，返回默认值
Array.prototype.first = function (func, defaultValue) {
    for (var i = 0; i < this.length; i++) {
        if (func(this[i])) {
            return this[i] || defaultValue;
        }
    }
    return defaultValue;
}

//返回第一项匹配的内容的序号，如果没有匹配成功，返回-1
Array.prototype.firstIndex = function (func) {
    for (var i = 0; i < this.length; i++) {
        if (func(this[i])) {
            return i;
        }
    }
    return -1;
}

//按照指定条件对数组进行排序（由小到大）
Array.prototype.orderBy = function (func) {
    var array = this.concat();
    return array.sort(function (before, next) {
        return func(before) - func(next);
    });
}

//返回数组是否包含指定内容
Array.prototype.contains = function (text) {
    return this.any(item => item == text);
}

//确定数组是否所有元素都满足条件
Array.prototype.all = function (func) {
    for (var i = 0; i < this.length; i++) {
        if (!func(this[i])) {
            return false;
        }
    }
    return true;
}

//确定数组是否有任意元素满足条件
Array.prototype.any = function (func) {
    for (var i = 0; i < this.length; i++) {
        if (func(this[i])) {
            return true;
        }
    }
    return false;
}

//数据去重
Array.prototype.distinct = function (func) {
    var array = [];
    func = func || (item => item);
    for (var i = 0; i < this.length; i++) {
        if (!array.any(item => func(item) == func(this[i]))) {
            array.push(this[i]);
        }
    }
    return array;
}

//返回数组内元素最大值
Array.prototype.max = function (func) {
    var max = null;
    func = func || (item => item);
    for (var i = 0; i < this.length; i++) {
        if (max == null) {
            max = func(this[i]);
        } else {
            max = Math.max(func(this[i]), max);
        }
    }
    return max;
}

//返回数组内元素最小值
Array.prototype.min = function (func) {
    var min = null;
    func = func || (item => item);
    for (var i = 0; i < this.length; i++) {
        if (min == null) {
            min = func(this[i]);
        } else {
            min = Math.min(func(this[i]), min);
        }
    }
    return min;
}

//返回数组内元素值之和
Array.prototype.sum = function (func) {
    var sum = 0;
    func = func || (item => item);
    for (var i = 0; i < this.length; i++) {
        sum += func(this[i]);
    }
    return sum;
}

//将数组中每个元素投影到新表中
Array.prototype.select = function (func) {
    var array = [];
    for (var i = 0; i < this.length; i++) {
        array.push(func(this[i], i));
    }
    return array;
}

//返回数组中所有符合条件的项
Array.prototype.where = function (func) {
    var array = [];
    for (var i = 0; i < this.length; i++) {
        if (func(this[i], i)) {
            array.push(this[i]);
        }
    }
    return array;
}

//删除数组指定位置元素
Array.prototype.removeAt = function (index) {
    return this.splice(index, 1);
}

//将数组转换成对象
Array.prototype.toObject = function (funcKey, funcValue) {
    var object = {};
    for (var i = 0; i < this.length; i++) {
        object[funcKey(this[i])] = funcValue(this[i]);
    }
    return object;
}

//验证数字是否在指定区间内
Number.prototype.between = function (min, max) {
    return this >= min && this <= max;
}

//验证时间是否在指定区间内
Date.prototype.between = function (min, max) {
    return this >= min && this <= max;
}

//日期格式化
Date.prototype.format = function (fmt) { //author: meizz 
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

//获取日期所在周次
Date.prototype.getWeek = function () {
    var dayArray = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];
    return dayArray[this.getDay()];
}

//验证是否包含内容
String.prototype.contains = function (text) {
    return this.indexOf(text) > -1;
}
