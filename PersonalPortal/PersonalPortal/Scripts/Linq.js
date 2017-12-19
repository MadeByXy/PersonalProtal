//返回数组第一项匹配内容, 如果没有匹配, 则返回null
Array.prototype.first = function (func, key) {
    var result = null;

    $.each(this, function (index, item) {
        if (func(item)) {
            result = item;
            return false;
        }
    })

    if (key != null && result != null) {
        result = result[key];
    }

    return result;
}

//返回数组第一项匹配内容的序号, 如果没有匹配, 则返回-1
Array.prototype.firstIndex = function (func) {
    var result = -1;

    $.each(this, function (index, item) {
        if (func(item)) {
            result = index;
            return false;
        }
    })

    return result;
}

//按照条件对数组进行排序
Array.prototype.orderBy = function (func) {
    return this.sort(function (before, next) {
        return func(before) - func(next);
    });
}

//返回数组是否包含指定内容
Array.prototype.contains = function (func) {
    return this.firstIndex(func) != -1;
}

//将序列中的每个元素投影到新表中
Array.prototype.select = function (func) {
    var array = [];
    $.each(this, function (index, item) {
        array.push(func(item, index));
    })
    return array;
}

//基于谓词筛选值序列, 将在谓词函数的逻辑中使用每个元素的索引
Array.prototype.where = function (func) {
    var array = [];
    $.each(this, function (index, item) {
        if (func(item, index)) {
            array.push(item);
        }
    })
    return array;
}

//数组转对象
Array.prototype.toObject = function (funcKey, funcValue) {
    var object = {};
    $.each(this, function (index, item) {
        object[funcKey(item)] = funcValue(item);
    })

    return object;
}
