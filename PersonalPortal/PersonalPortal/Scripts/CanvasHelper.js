function Canvas(canvasId) {
    this.cancasId = canvasId;
    this.object = document.getElementById(canvasId);
    this.context = this.object.getContext('2d');

    this.size = {
        width: 100,
        height: 100,
        spacing: 2
    }

    //x节点序号计算横坐标
    this.x_center = function (index_x, isText) {
        return index_x * (this.size.width + this.size.spacing) + this.size.spacing + (isText == true ? this.size.width / 2 : 0);
    }

    //y节点序号计算纵坐标
    this.y_center = function (index_y, isText) {
        return index_y * (this.size.height + this.size.spacing) + this.size.spacing + (isText == true ? this.size.height / 2 : 0);
    }

    //重计算单位长宽
    this.AutoSize = function (size_x, size_y) {
        this.size.width = (this.object.scrollWidth - this.size.spacing) / size_x - this.size.spacing;
        this.size.height = (this.object.scrollHeight - this.size.spacing) / size_y - this.size.spacing;
    }

    //绘制矩形
    this.Cube = function (index_x, index_y) {
        this.context.fillRect(
            this.x_center(index_x),
            this.y_center(index_y),
            this.size.width,
            this.size.height);
    }

    //绘制文字
    this.Text = function (text, index_x, index_y) {
        this.context.fillStyle = 'black';
        this.context.textAlign = 'center';
        this.context.textBaseline = 'middle';

        //添加文字前删除当前位置已有文字
        this.context.clearRect(
            this.x_center(index_x, false),
            this.y_center(index_y, false),
            this.size.width,
            this.size.height);

        this.context.fillText(
            text,
            this.x_center(index_x, true),
            this.y_center(index_y, true));
    }

    //绘制线段
    this.Line = function (move_x, move_y, line_x, line_y) {
        this.context.beginPath();

        this.context.moveTo(
            this.x_center(move_x, true),
            this.y_center(move_y, true));

        this.context.lineTo(
            this.x_cneter(line_x, true),
            this.y_center(line_y, true));
        this.context.stroke();
        this.context.closePath();
    }

    //绘制线段组
    this.LineArray = function (indexArray, style) {
        this.context.beginPath();
        for (var i = 0; i < indexArray.length; i++) {
            this.context.lineTo(this.x_center(indexArray[i].x, true), this.y_center(indexArray[i].y, true))
        }

        this.context.strokeStyle = style || 'black';
        this.context.stroke();
        this.context.closePath();
    }

    //清理画布
    this.Clear = function () {
        this.context.clearRect(0, 0, this.object.scrollWidth, this.object.scrollHeight);
    }
}