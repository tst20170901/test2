function initWX() {
    var thisUrl = location.href.split('#')[0];
    $.ajax({
        url: "/bearmall/weixin",
        type: "POST",
        dataType: "json",
        data: { url: thisUrl },
        success: function (json) {
            if (json.code == "1") {
                wx.config({
                    debug: false,
                    appId: json.data.appId,
                    timestamp: json.data.timestamp,
                    nonceStr: json.data.nonceStr,
                    signature: json.data.signature,
                    jsApiList: ['onMenuShareAppMessage', 'onMenuShareTimeline', 'scanQRCode']
                });
            }
        }
    });
}
function QRCode() {
    wx.scanQRCode({
        needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
        scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
        success: function (res) {
            var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
            location.href = "/Business/Index?bid=" + result;
        }
    });
}
function shareTimeline(stitle, slink, simgurl, callback) {
    //alert(stitle);
    wx.onMenuShareTimeline({
        title: stitle,
        link: slink,
        imgUrl: simgurl,
        success: function () {
            //alert("发行成功");
            callback();
        },
        cancel: function () {
        }
    });
}
function shareAppMessage(stitle, sdesc, slink, simgurl, callback) {
    //alert(stitle);
    wx.onMenuShareAppMessage({
        title: stitle,
        desc: sdesc,
        link: slink,
        imgUrl: simgurl,
        type: '',
        dataUrl: '',
        success: function () {
            //alert("发行成功");
            callback();
        },
        cancel: function () {
        }
    });
}