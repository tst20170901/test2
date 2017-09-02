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
        needResult: 1, // Ĭ��Ϊ0��ɨ������΢�Ŵ���1��ֱ�ӷ���ɨ������
        scanType: ["qrCode", "barCode"], // ����ָ��ɨ��ά�뻹��һά�룬Ĭ�϶��߶���
        success: function (res) {
            var result = res.resultStr; // ��needResult Ϊ 1 ʱ��ɨ�뷵�صĽ��
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
            //alert("���гɹ�");
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
            //alert("���гɹ�");
            callback();
        },
        cancel: function () {
        }
    });
}