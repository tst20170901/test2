function Loading(bool) { if (bool) { top.$("#loading").show(); } else { setInterval(loadinghide, 900); } }
function loadinghide() { if (top.document.getElementById("loading") != null) { top.$("#loading").hide(); } }
function getAjax(url, parm, callBack) { $.ajax({ type: 'post', dataType: "text", url: url, data: parm, cache: false, async: false, success: function (msg) { callBack(msg); } }); }
var MainContent_subtract = 122;
var Sidebar_subtract = 148;
function iframeresize() {
    resizeU();
    $(window).resize(resizeU);
    function resizeU() {
        var divkuangH = $(window).height();
        $("#MainContent").height(divkuangH - MainContent_subtract - 1);
    }
}
function Shieldkeydown() { $("*").keydown(function (e) { e = window.event || e || e.which; if (e.keyCode == 112 || e.keyCode == 113 || e.keyCode == 114 || e.keyCode == 115 || e.keyCode == 116 || e.keyCode == 117 || e.keyCode == 118 || e.keyCode == 119 || e.keyCode == 120 || e.keyCode == 121 || e.keyCode == 122 || e.keyCode == 123) { e.keyCode = 0; return false; } }) }
function AddTabMenu(tabid, url, name, img, Isclose, IsReplace) {
    if (url == "" || url == "#") { url = "/ErrorPage/404.html" }
    var tabs_container = top.$("#tabs_container");
    var ContentPannel = top.$("#ContentPannel");
    if (IsReplace == 'true') { top.RemoveDiv(tabid); }
    //if (Isclose != 'false') { top.$(".navigation").hide(); } else { top.$(".navigation").show(); }
    if (top.document.getElementById("tabs_" + tabid) == null) { //如果当前tabid存在直接显示已经打开的tab
        Loading(true);
        tabs_container.find('li').removeClass('selected');
        ContentPannel.find('iframe').hide();
        if (Isclose != 'false') { //判断是否带关闭tab
            tabs_container.append("<li id=\"tabs_" + tabid + "\" class='selected' win_close='true'><span title='" + name + "' onclick=\"javascript:AddTabMenu('" + tabid + "','" + url + "','" + name + "','" + img + "','true')\"><a href=\"javascript:;\"><img src=\"/Content/images/32/" + img + "\" width=\"20\" height=\"20\">" + name + "</a></span><a class=\"win_close\" title=\"关闭当前窗口\" onclick=\"RemoveDiv('" + tabid + "')\"></a></li>");
        } else {
            tabs_container.append("<li id=\"tabs_" + tabid + "\" class=\"selected\" onclick=\"javascript:AddTabMenu('" + tabid + "','" + url + "','" + name + "','" + img + "','false')\"><a><img src=\"/Content/images/32/" + img + "\" width=\"20\" height=\"20\">" + name + "</a></li>");
        }
        ContentPannel.append("<iframe id=\"tabs_iframe_" + tabid + "\" name=\"tabs_iframe_" + tabid + "\" height=\"100%\" width=\"100%\" src=\"" + url + "\" frameBorder=\"0\"></iframe>");
    }
    else {
        tabs_container.find('li').removeClass('selected');
        ContentPannel.find('iframe').hide();
        tabs_container.find('#tabs_' + tabid).addClass('selected');
        top.document.getElementById("tabs_iframe_" + tabid).style.display = 'block';
    }
}
//样式
function readyIndex() {
    $("#toolbar img").hover(function () {
        $(this).addClass("pageBase_Div");
    }, function () {
        $(this).removeClass("pageBase_Div");
    });
    $("#topnav li").click(function () {
        $("#topnav li").find('a').removeClass("onnav")
        $(this).find('a').addClass("onnav");
    });
    $(".sub-menu div").click(function () {
        $('.sub-menu div').removeClass("selected")
        $(this).addClass("selected");
    }).hover(function () {
        $(this).addClass("navHover");
    }, function () {
        $(this).removeClass("navHover");
    });
    $(".sub-menu").hover(function () {
        $(this).css("overflow", "auto");
    }, function () {
        $(this).css("overflow", "hidden");
    });
}
function resizeLayout() {
    resizeU();
    $(window).resize(resizeU);
    function resizeU() {
        var accordion_head = $('.accordion > li > a'), accordion_body = $('.accordion li > .sub-menu');
        $(".sub-menu").css('height', $(".navigation").height() - 19 - accordion_head.length * accordion_head.height() - accordion_head.length + 'px');
        //accordion_head.first().addClass('active').next().slideDown('normal');
        accordion_head.on('click', function (event) {
            event.preventDefault();
            if ($(this).attr('class') != 'active') {
                accordion_body.slideUp('normal');
                $(this).next().stop(true, true).slideToggle('normal');
                accordion_head.removeClass('active');
                $(this).addClass('active');
            }
        });
    }
}
function RemoveDiv(obj) {
    var tabs_container = top.$("#tabs_container");
    var ContentPannel = top.$("#ContentPannel");
    tabs_container.find("#tabs_" + obj).remove();
    ContentPannel.find("#tabs_iframe_" + obj).remove();
    var tablist = tabs_container.find('li');
    var pannellist = ContentPannel.find('iframe');
    if (tablist.length > 0) {
        tablist[tablist.length - 1].className = 'selected';
        pannellist[tablist.length - 1].style.display = 'block';
    }
    if (tablist.length == '1') {
        top.$(".navigation").show();
    }
}
var Contentheight = "";
var Contentwidth = "";
var FixedTableHeight = "";
//最大化
function Maximize() {
    $(".top").hide();
    $("#full_screen").attr('src', 'images/16/05.gif').attr('title', '还原').attr('onclick', 'Fullrestore()');
    MainContent_subtract = MainContent_subtract - 70;
    Sidebar_subtract = Sidebar_subtract - 70;
    iframeresize();
}
//还原
function Fullrestore() {
    $(".top").show();
    $("#full_screen").attr('src', 'images/16/04.gif').attr('title', '最大化').attr('onclick', 'Maximize()');
    MainContent_subtract = MainContent_subtract + 70;
    Sidebar_subtract = Sidebar_subtract + 70;
    iframeresize();
}
function divresize(element, height) {
    resizeU();
    $(window).resize(resizeU);
    function resizeU() {
        $(element).css("height", $(window).height() - height);
    }
}
function treeAttrCss() {
    $(".black").treeview({ control: "#treecontrol", persist: "cookie", cookieId: "treeview-gray" }); treeCss();
}
function treeCss() {
    $(".strTree li div").css("cursor", "pointer"); $(".strTree li div").click(function () { $(".strTree li div").removeClass("collapsableselected"); $(this).addClass("collapsableselected"); }).hover(function () { $(this).addClass("collapsablehover"); }, function () { $(".strTree li div").removeClass("collapsablehover"); });
}
function Replace() { Loading(true); window.location.href = window.location.href; return false; }
function Add(url) { Loading(true); window.location.href = url; return false; }
function ThisCloseTab() { var tabs_container = top.$("#tabs_container"); top.RemoveDiv(tabs_container.find('.selected').attr('id').substr(5)); }