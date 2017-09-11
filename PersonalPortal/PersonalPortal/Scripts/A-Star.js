function AStar(Map) {
    this.Map = Map;

    //获取是否结束循环
    this.IsOver = function (length) {
        return Map.length <= length && Map[0].length <= length;
    }

    //获取节点是否允许行走
    this.IsAllow = function (x, y) {
        return Map.length > x && Map[x].length > y;
    }
}