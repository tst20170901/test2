function showToast(msg, url) { layer.open({ content: msg, style: "background:#333;color:#FFF;border:none;", time: 2, end: function () { if (typeof (url) != "undefined" && href != "") { window.location.href = url; } } }); }
$(document).ready(function () {
    scrollPage(getGoodsList);
    slideNav();
});
function slideNav() {
    $(window).resize(function () {
        $("#dataClass").css({
            height: $(window).height() - 92 + "px"
        })
    });
    $(window).resize()
}
var page = 1;
var isload = true;
function scrollPage(callBackFun) {
    $(window).scroll(function () {
        if (isload) { //ajax在后台获取数据时，设值其false，防止页面多次加载
            // var more_top =document.getElementById("dataMore").offsetTop; //加载更多层距离document 顶部的距离
            if ($(this).scrollTop() + $(window).height() + 100 >= $(document).height() && $(this).scrollTop() > 100) {
                //更多出现在滚动区域
                page++;
                isload = false;
                window.setTimeout(function () {
                    callBackFun();
                }, 200);
            }
        }
    });
}
function getGoodsList() {
    $("#loadpro").show();
    $.ajax({
        type: "post",
        traditional: true,
        dataType: "json",
        url: "/BearMall/AjaxBusinessProductList/",
        data: { page: page, typeid: typeid,bid:bid },
        success: function (data) {
            isload = true;
            if (data.code == "0") {
                $("#loadpro").hide();
            } else {
                if (data.data.length > 0) {
                    $("#dataList").append(data.data);
                    $("#loadpro").hide();
                    pagenum += 1;
                }
                else {
                    $("#loadpro").hide();
                }
            }
        },
        error: function () {
            showToast("网络错误，请重试");
        }
    });
}

function order() {
    var mobile_show = $("#mobile_show").val();
    var name = $("#name").val();
    var address = $("#address").val();
    var val_pay = $('input[name="zhifu"]:checked').val();
    if (val_pay == "10") {
        var all = $("#allmoney").val();
        var walletmoney = $("#walletmoney").val();
        if (parseFloat(all) > walletmoney) {
            showToast("钱包余额不足，请选择其他支付方式");
            return;
        }
    } else if (val_pay == "20") {
        var all = $("#allmoney").val();
        if (parseFloat(all) == 0) {
            showToast("0元不能选择微信支付，请选择钱包支付方式");
            return;
        }
    }
    if (mobile_show == "") {
        showToast("请输入手机号");
        return;
    } else if (name == "") {
        showToast("请输入称呼");
        return;
    } else if (address == "") {
        showToast("请输入地址");
        return;
    } 
    $("#ordersubmit").removeAttr("onclick");
    $.ajax({
        type: 'post',
        traditional: true,
        dataType: "json",
        async: false,
        url: "/Product/ProductOrderPost",
        data: { 'paymode': val_pay, pid: $("#pid").val(), mobile: mobile_show, name: name, comment: $("#comment").val(), address: $("#address").val() },
        success: function (data) {
            if (data.code == 0) {
                showToast(data.msg);
                $("#ordersubmit").attr("onclick", "order()");
                return;
            } else if (data.code == 1) {
                if (data.msg == "zfb") {
                    window.location.href = "/OrderAlipay/Pay?oid=" + data.data;
                } else if (data.msg == "wx") {
                    window.location.href = "/OrderWeiXinPay/ProPay?oid=" + data.data;
                } else if (data.msg == "qb") {
                    window.location.href = "/Product/PayResult?oid=" + data.data;
                }
                return;
            }
        }
    });
}