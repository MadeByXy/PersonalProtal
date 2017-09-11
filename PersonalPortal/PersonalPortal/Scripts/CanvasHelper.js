function Canvas(canvasId) {
    this.cancasId = canvasId;
    this.object = document.getElementById(canvasId);
    this.context = this.object.getContext('2d');

    this.size = {
        width: 100,
        height: 100,
        spacing: 2
    }

    this.AutoSize = function (size_x, size_y) {
        this.size.width = (this.object.scrollWidth - this.size.spacing) / size_x - this.size.spacing;
        this.size.height = (this.object.scrollHeight - this.size.spacing) / size_y - this.size.spacing;
    }

    //绘制矩形
    this.Cube = function (index_x, index_y) {
        this.context.fillRect(
            index_x * (this.size.width + this.size.spacing) + this.size.spacing,
            index_y * (this.size.height + this.size.spacing) + this.size.spacing,
            this.size.width,
            this.size.height);
    }

    //绘制文字
    this.Text = function (text, index_x, index_y) {
        this.context.fillStyle = 'black';
        this.context.textAlign = 'center';
        this.context.textBaseline = 'middle';

        //重计算文字位置使文字位于背景中心
        this.context.fillText(
            text,
            index_x * (this.size.width + this.size.spacing) + this.size.spacing + this.size.width / 2,
            index_y * (this.size.height + this.size.spacing) + this.size.spacing + this.size.height / 2);
    }

    //绘制线段
    this.Line = function (move_x, move_y, line_x, line_y) {
        this.context.moveTo(move_x, move_y);
        this.context.lineTo(line_x, line_y);
        this.context.stroke();
    }

    //清理画布
    this.Clear = function () {
        this.context.clearRect(0, 0, this.object.scrollWidth, this.object.scrollHeight);
    }
}