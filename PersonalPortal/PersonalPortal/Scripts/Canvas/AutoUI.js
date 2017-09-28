AutoUI = {
    Size: {
        Width: 0,
        Height: 0
    },
    Canvas: null,
    StrokeStyle: 'black',
    IsBorder: false,
    //绘制边框
    Border: function () {
        if (this.IsBorder) {
            this.Canvas.context.strokeStyle = this.StrokeStyle;
            this.Canvas.context.strokeRect(
                (this.Canvas.object.width - this.Size.Width) / 2,
                (this.Canvas.object.height - this.Size.Height) / 2,
                this.Size.Width,
                this.Size.Height)
        }
    },
    //绘制矩形
    Square: function () {
        this.Border();

        weight = (1 + 1 / Math.sqrt(2)) / 2;
        this.Canvas.context.strokeStyle = this.StrokeStyle;
        this.Canvas.context.strokeRect(
            (this.Canvas.object.width - this.Size.Width * weight) / 2,
            (this.Canvas.object.height - this.Size.Height * weight) / 2,
            this.Size.Width * weight,
            this.Size.Height * weight)
    },
    //绘制圆形
    Round: function () {
        this.BezierCurve(0);
    },
    //三角形
    Triangle: function () {
        this.Border();

        weight = 1 - (1 + 1 / Math.sqrt(2)) / 2;
        circle = {
            x: (this.Canvas.object.width) / 2,
            y: (this.Canvas.object.height) / 2 + 0.125 * this.Size.Height,
        }

        radius = {
            x: this.Size.Width / 2,
            y: this.Size.Height / 2
        }

        this.Canvas.context.strokeStyle = this.StrokeStyle;
        this.Canvas.context.beginPath();
        this.Canvas.context.lineTo(
            circle.x,
            circle.y - radius.y);
        this.Canvas.context.lineTo(
             circle.x + radius.x,
            circle.y + 0.5 * radius.y);
        this.Canvas.context.lineTo(
            circle.x - radius.x,
            circle.y + 0.5 * radius.y);

        this.Canvas.context.closePath();
        this.Canvas.context.stroke();
    },
    //超椭圆
    SuperEllipse: function () {
        this.BezierCurve(0.08)
    },
    //圆角矩形
    RoundedRectangle: function () {
        this.BezierCurve(1 - Math.sqrt(3))
    },
    //通用曲线
    BezierCurve: function (weight) {
        this.Border();

        //√3为圆形
        weight = Math.sqrt(3) + weight;
        circle = {
            x: (this.Canvas.object.width) / 2,
            y: (this.Canvas.object.height) / 2,
        }
        radius = {
            x: this.Size.Width / 2,
            y: this.Size.Height / 2
        }

        this.Canvas.context.strokeStyle = this.StrokeStyle;
        this.Canvas.context.beginPath();

        this.Canvas.context.moveTo(
            circle.x,
            circle.y - radius.y);
        this.Canvas.context.bezierCurveTo(
            circle.x + radius.x / weight,
            circle.y - radius.y,
            circle.x + radius.x,
            circle.y - radius.y / weight,
            circle.x + radius.x,
            circle.y);
        this.Canvas.context.bezierCurveTo(
            circle.x + radius.x,
            circle.y + radius.y / weight,
            circle.x + radius.x / weight,
            circle.y + radius.y,
            circle.x,
            circle.y + radius.y);
        this.Canvas.context.bezierCurveTo(
            circle.x - radius.x / weight,
            circle.y + radius.y,
            circle.x - radius.x,
            circle.y + radius.y / weight,
            circle.x - radius.x,
            circle.y);
        this.Canvas.context.bezierCurveTo(
            circle.x - radius.x,
            circle.y - radius.y / weight,
            circle.x - radius.x / weight,
            circle.y - radius.y,
            circle.x,
            circle.y - radius.y);

        this.Canvas.context.closePath();
        this.Canvas.context.stroke();
    }
}