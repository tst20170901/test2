$(document).ready(function () {
    //document.onselectstart = function () { return false; }
    //$(document).bind("contextmenu", function () {
    //	return false;
    //});
    writeDateInfo();
    Loading(true);
    iframeresize();
    AddTabMenu('Imain', '/UsersManger/UsersAdmin/WebInit', '首页主控台', '4963_home.png', 'false');
    resizeLayout();
    readyIndex();
    //    $("#tabs_Imain").hover(function () {
    //        AddTabMenu('Imain', 'HomeIndex.aspx', '首页主控台', '4963_home.png', 'false');
    //    });
    //Shieldkeydown();
    Loading(false);
});
function IndexOut() {
    window.location = "/UsersManger/FrameSet/LoginOut";
}
function Controlpanel() {
    AddTabMenu('Controlpanel', '/UsersManger/UsersAdmin/WebInfo', '控制面板', '5026_settings.png', 'true');
}
function writeDateInfo() {
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth();
    var date = now.getDate();
    var day = now.getDay();
    var hour = now.getHours();
    var minu = now.getMinutes();
    var sec = now.getSeconds();
    var week;
    month = month + 1;
    if (month < 10) month = "0" + month;
    if (date < 10) date = "0" + date;
    if (hour < 10) hour = "0" + hour;
    if (minu < 10) minu = "0" + minu;
    if (sec < 10) sec = "0" + sec;
    var arr_week = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
    week = arr_week[day];
    var time = "";
    time = year + "年" + month + "月" + date + "日" + " " + hour + ":" + minu + ":" + sec;
    $("#datetime").text(time);
    var timer = setTimeout("writeDateInfo()", 1000);
}