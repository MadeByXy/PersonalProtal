function Canvas(canvasId) {
    this.cancasId = canvasId;
    this.object = document.getElementById(canvasId);
    this.context = this.object.getContext('2d');

    this.size = {
        width: 100,
        height: 100,
        spacing: 2
    }
    size = this.size;

    this.Coordinates = {
        From: {
            //x节点序号计算横坐标
            X: function (index_x, isText) {
                return index_x * (size.width + size.spacing) + size.spacing + (isText == true ? size.width / 2 : 0);
            },
            //y节点序号计算纵坐标
            Y: function (index_y, isText) {
                return index_y * (size.height + size.spacing) + size.spacing + (isText == true ? size.height / 2 : 0);
            }
        },
        To: {
            //横坐标计算x节点
            X: function (position_x) {
                return parseInt((position_x - size.spacing) / (size.width + size.spacing));
            },
            //纵坐标计算y节点
            Y: function (position_y) {
                return parseInt((position_y - size.spacing) / (size.height + size.spacing));
            }
        }
    }

    //重计算单位长宽
    this.AutoSize = function (size_x, size_y) {
        this.size.width = (this.object.scrollWidth - this.size.spacing) / size_x - this.size.spacing;
        this.size.height = (this.object.scrollHeight - this.size.spacing) / size_y - this.size.spacing;
    }

    //绘制矩形
    this.Cube = function (index_x, index_y) {
        //添加文字前删除当前位置已有矩形
        this.ClearItem(index_x, index_y);

        this.context.fillRect(
            this.Coordinates.From.X(index_x),
            this.Coordinates.From.Y(index_y),
            this.size.width,
            this.size.height);
    }

    //绘制文字
    this.Text = function (text, index_x, index_y) {
        this.context.fillStyle = 'black';
        this.context.textAlign = 'center';
        this.context.textBaseline = 'middle';

        //添加文字前删除当前位置已有文字
        this.ClearItem(index_x, index_y);

        this.context.fillText(
            text,
            this.Coordinates.From.X(index_x, true),
            this.Coordinates.From.Y(index_y, true));
    }



    //绘制线段
    this.Line = function (move_x, move_y, line_x, line_y) {
        this.context.beginPath();

        this.context.moveTo(
            this.Coordinates.From.X(move_x, true),
            this.Coordinates.From.Y(move_y, true));

        this.context.lineTo(
            this.Coordinates.From.X(line_x, true),
            this.Coordinates.From.Y(line_y, true));
        this.context.stroke();
        this.context.closePath();
    }

    //绘制线段组
    this.LineArray = function (indexArray, style) {
        this.context.beginPath();
        for (var i = 0; i < indexArray.length; i++) {
            this.context.lineTo(this.Coordinates.From.X(indexArray[i].x, true), this.Coordinates.From.Y(indexArray[i].y, true))
        }

        this.context.strokeStyle = style || 'black';
        this.context.stroke();
        this.context.closePath();
    }

    //绘制实心多边形
    this.LinePolygon = function (indexArray) {
        this.context.beginPath();
        for (var i = 0; i < indexArray.length; i++) {
            this.context.lineTo(this.Coordinates.From.X(indexArray[i].x, true), this.Coordinates.From.Y(indexArray[i].y, true))
        }

        this.context.closePath();
        this.context.fill();
    }

    //清理一个单元格
    this.ClearItem = function (index_x, index_y) {
        this.context.clearRect(
            this.Coordinates.From.X(index_x, false) - this.size.spacing,
            this.Coordinates.From.Y(index_y, false) - this.size.spacing,
            this.size.width + 2 * this.size.spacing,
            this.size.height + 2 * this.size.spacing);
    }

    //清理画布
    this.Clear = function () {
        this.context.clearRect(0, 0, this.object.scrollWidth, this.object.scrollHeight);
    }
}