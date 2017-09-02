(function ($) {
    $.fn.extend({
        Scroll: function (opt, callback) {
            if (!opt) var opt = {};
            var _this = this.eq(0).find("ul:first");
            var lineH = _this.find("li:first").height(),
    line = opt.line ? parseInt(opt.line, 10) : parseInt(this.height() / lineH, 10),
    speed = opt.speed ? parseInt(opt.speed, 10) : 7000,  
    timer = opt.timer ? parseInt(opt.timer, 10) : 7000; 
            if (line == 0) line = 1;
            var upHeight = 0 - line * lineH;
            scrollUp = function () {
                _this.animate({
                    marginTop: upHeight
                }, speed, function () {
                    for (i = 1; i <= line; i++) {
                        _this.find("li:first").appendTo(_this);
                    }
                    _this.css({ marginTop: 0 });
                });
            }
            _this.hover(function () {
                clearInterval(timerID);
            }, function () {
                timerID = setInterval("scrollUp()", timer);
            }).mouseout();
        }
    })
})(jQuery);