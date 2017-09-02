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
        url: "/BearMall/AjaxBusinessList/",
        data: { page: page, typeid: typeid, bid: bid, branchid: $("#branchid").val() },
        success: function (data) {
            isload = true;
            if (data.code == "0") {
                $("#loadpro").hide();
            } else {
                if (data.data.length > 0) {
                    $("#dataList").append(data.data);
                    $("#loadpro").hide();
                    page += 1;
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