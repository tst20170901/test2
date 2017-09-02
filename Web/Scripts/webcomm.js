function rollPicL(id) {
    var speed = 30;
    var st = 1000;
    var w = 0;
    $("#" + id + " td").each(function () { w += $(this).width() });
    if (w > $("#" + id).width()) $("#" + id + " tr").html($("#" + id + " tr").html() + $("#" + id + " tr").html());
    var timeID = false;
    function play() {
        var temp = $("#" + id).scrollLeft() + 1;
        $("#" + id).scrollLeft(temp);
        if (parseInt($("#" + id).scrollLeft()) == parseInt(w)) {
            $("#" + id).scrollLeft(0);
        }
    }
    timeID = setInterval(play, speed);
    $("#" + id).bind("mouseover", function () { clearInterval(timeID); }).bind("mouseout", function () { timeID = setInterval(play, speed); });
}
function SetHome(obj, vrl) {
    try {
        obj.style.behavior = 'url(#default#homepage)'; obj.setHomePage(vrl);
    }
    catch (e) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            }
            catch (e) {
                alert("此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将 [signed.applets.codebase_principal_support]的值设置为'true',双击即可。");
            }
            var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
            prefs.setCharPref('browser.startup.homepage', vrl);
        } else {
            alert("您的浏览器不支持，请按照下面步骤操作：1.打开浏览器设置。2.点击设置网页。3.输入：" + vrl + "点击确定。");
        }
    }
}
function AddFavou(sTitle, sURL) {
    try {
        window.external.addFavorite(sURL, sTitle);
    }
    catch (e) {
        try {
            window.sidebar.addPanel(sTitle, sURL, "");
        }
        catch (e) {
            alert("加入收藏失败，请使用Ctrl+D进行添加");
        }
    }
}