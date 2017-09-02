function showToast(msg, url) { layer.open({ content: msg, style: "background:#333;color:#FFF;border:none;", time: 2, end: function () { if (typeof (url) != "undefined" && url != "") { window.location.href = url; } } }); }
function logout() { location.href = "/Login/LoginOut"; }
var models = null;
var mapinit = false;//服务项目初始化
var cardinit = false;//优惠券初始化
/*线上*/
var lock = true;
var map;
var location_callback = null;
function showmap() {
    if (lock) {
        select_location(set_location);
        order_preorder();
    }
}
function set_location(lat, lng) {
    $("#lat").val(lat);
    $("#lng").val(lng);
    get_rgeocode(lat, lng);
    order_preorder();
}
function select_location(callback) {
    lat = $("#lat").val();
    lng = $("#lng").val();
    init_map(lat, lng);
    if (lat > 0 && lng > 0) {
        map.setCenter(AMap.LngLat(lng, lat));
    }
    location_callback = callback;
    show_map();
}
function init_map(lat, lng) {
    if (!map) {
        if (lng <= 0 || lat <= 0) {
            lng = 115.670177;
            lat = 37.73892;
        }
        get_rgeocode(lat, lng);
        map = new AMap.Map('mymap', { resizeEnable: true, view: new AMap.View2D({ zoom: 15, center: new AMap.LngLat(lng, lat) }) });
        map.plugin(["AMap.ToolBar"], function () { var tool = new AMap.ToolBar(); map.addControl(tool); });
        map.plugin('AMap.Geolocation', function () {
            geolocation = new AMap.Geolocation({
                enableHighAccuracy: true,
                timeout: 10000,
                maximumAge: 0,
                convert: true,
                showButton: true,
                buttonPosition: 'LB',
                buttonOffset: new AMap.Pixel(10, 20),
                showMarker: true,
                showCircle: true,
                panToLocation: false,
                zoomToAccuracy: false
            });
            map.addControl(geolocation);
            geolocation.getCurrentPosition();
            AMap.event.addListener(geolocation, 'complete', function (e) {
                map.setCenter(e.position);
                check_location();
            });
            AMap.event.addListener(map, 'dragend', function (e) { check_location(); });
            AMap.event.addListener(map, 'zoomend', function (e) { check_location(); });
        });
    }
}
function check_location() {
    var center = map.getCenter();
    if (location_callback) {
        location_callback(center.getLat(), center.getLng());
    }
}
function select_ok() {
    dismiss_map();
}
function dismiss_map() {
    $("#map").hide();
    location_callback = null;
    $("#orderinfo").show();
    return 0;
}
function order() {
    var mobile_show = $("#mobile_show").val();
    var name = $("#name").val();
    var plate = $("#plate").val();
    var brand_model_show = $("#brand_model_show").val();
    var bandid = $("#brand").val();
    var bid = $("#branchid").val();
    var modelid = $("#model").val();
    var color = $("#color").val();
    var time = $("#selecttime").val();
    var timestr = $("#selecttime").find("option:selected").text();
    var userstate = $("#userstate").val();
    var val_pay = $('input[name="zhifu"]:checked').val();
    if (time == "0" || time < 8 || time > 20) {
        showToast("请明日预约下单");
        return;
    }
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
    var itemIds = new Array();
    var pte = true;
    $("input[name='washitem']").each(function () {
        if (this.checked) {
            itemIds.push($(this).attr('id'));
            if ($(this).attr('tys') == "vip") {
                if ($(this).attr('plate') != "" && $(this).attr('plate') != plate) {
                    pte = false;
                }
            }
        }
    });
    if (itemIds.length <= 0) { showToast('请选择服务项目！'); return; }
    var coupIds = new Array();
    $("input[name='couponmodel']").each(function () {
        if (this.checked) {
            coupIds.push($(this).attr('id'));
        }
    });
    if (mobile_show == "") {
        showToast("请输入手机号");
        return;
    } else if (name == "") {
        showToast("请输入称呼");
        return;
    } else if (plate == "") {
        showToast("请输入车牌号");
        return;
    } else if (userstate == "noinit") {
        showToast("请拖动地图选取位置");
        return;
    } else if (userstate != "ok") {
        showToast("请重新选取位置");
        return;
    }
    if (!pte) {
        showToast("该会员卡只能服务于绑定车牌");
        return;
    }
    $("#ordersubmit").removeAttr("onclick");
    $.ajax({
        type: 'post',
        traditional: true,
        dataType: "json",
        async: false,
        url: "/BearClient/SubmitOrder",
        data: { bid: bid, coupid: coupIds, 'itemIds': itemIds, 'paymode': val_pay, a: $("#lng").val(), b: $("#lat").val(), mobile: mobile_show, name: name, plate: plate, brandshow: brand_model_show, brand: bandid, model: modelid, color: color, timespan: time, timestr: timestr, comment: $("#comment").val(), address: $("#address").val() },
        success: function (data) {
            if (data.code == 0) {
                showToast(data.msg);
                $("#ordersubmit").attr("onclick", "order()");
                return;
            } else if (data.code == 1) {
                if (data.msg == "zfb") {
                    window.location.href = "/OrderAlipay/Pay?oid=" + data.data;
                } else if (data.msg == "wx") {
                    window.location.href = "/OrderWeiXinPay/Pay?oid=" + data.data;
                } else if (data.msg == "qb") {
                    window.location.href = "/BearClient/PayResult?oid=" + data.data;
                }
                return;
            } else if (data.code == "2") {
                $("#userstate").val("fail");
                branchclose(data.msg);
            }
        }
    });
}
function branchclose(msg) {
    $("#map").hide();
    $("#orderinfo").show();
    var tipBg = $("#confimbg");
    var tipBox = $("#confimbox");
    var m = $("#closemsg");
    tipBg.removeClass("hide");
    tipBox.removeClass("hide");
    m.html(msg);
    tipBox.css({ marginLeft: -(tipBox.width() / 2), marginTop: -(tipBox.height() / 2) });
}
function order_preorder() {
    mapinit = false;
    cardinit = false;
    $("#service").val("");
    $("#msg").html("服务时间预计中……");
    $("#mapmsg").html("服务时间预计中……");
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "/BearClient/PrepareOrder/",
        data: { a: $("#lng").val(), b: $("#lat").val() },
        success: function (data) {
            if (data.code == "0") {
                $("#msg").html(data.msg);
                $("#mapmsg").html(data.msg);
                $("#userstate").val("fail");
                $("#branchid").val("0");
            } else if (data.code == "1") {
                $("#msg").html(data.msg);
                $("#mapmsg").html(data.msg);
                $("#userstate").val("ok");
                $("#branchid").val(data.data);
            } else if (data.code == "2") {
                $("#userstate").val("fail");
                branchclose(data.msg);
            }
        },
        error: function (error) {
            showToast("网络错误，请重试");
        }
    });
}
/*线上*/
/*线下*/
var lock1 = true;
var mapshop;
var location_callback1 = null;
function showshop() {
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "/BearClient/PrepareShopOrder/",
        data: { a: $("#lng").val(), b: $("#lat").val() },
        success: function (data) {
            if (data.code == "0") {
                $("#shopmsg").html(data.msg);
            } else {
                var html = "";
                for (var i = 0; i < data.data.length; i++) {
                    html += "<div class=\"shopitem\">" +
                                "<div class=\"shoptit\">" + data.data[i].Title + "(距离：" + data.data[i].Distan + "米)<a href=\"javascript:void(0)\" class=\"edo_btn1 fn-right margin10\" onclick=\"select_shop('" + data.data[i].Title + "', " + data.data[i].ID + "," + data.data[i].BranchID + ")\">我要洗车</a></div>" +
                                "<div class=\"shopbd\">" +
                                    "<img src=\"" + data.data[i].Img + "\" width=\"120\" height=\"100\" />" +
                                    "<div class=\"shopbd_txt\"><div class=\"address\">" + data.data[i].District + "</div>" +
                                    "<div class=\"cishu\"><a href=\"tel:" + data.data[i].Phone + "\" style=\"color: #FE9223\">" + data.data[i].Phone + "</a></div>" +
                                    "<div class=\"w\">预计等待<span class=\"wr\">" + data.data[i].OrderTime + "</span>分钟(有<span class=\"wr\">" + data.data[i].OrderCount + "</span>辆车排队)</div>" +
                                    "<div class=\"date\">营业时间：" + data.data[i].ST + " - " + data.data[i].ET + "</div></div>" +
                                    "<div class=\"fn-clear\"></div>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"h10p g\"></div>";
                }
                $("#shoplistbd").html(html);
                $("#shopmsg").text("");
                $("#load").hide();
            }
        },
        error: function () {
            showToast("网络错误，请重试");
        }
    });
    $("#shoplist").show();
    $("#orderinfo").hide();
    $("#msg").hide();
}
function select_shop(t, m, b) {
    $("#shopid").val(m);
    $("#branchid").val(b);
    $("#shopname").val(t);
    $("#orderinfo").show();
    $("#msg").show();
    $("#shoplist").hide();
}
function set_location1(lat, lng) {
    $("#lat").val(lat);
    $("#lng").val(lng);
    get_rgeocode(lat, lng);
    xcforder_preorder();
}
function showmapxcf() {
    if (lock1) {
        select_location1(set_location1);
        xcforder_preorder();
    }
}
function select_location1(callback) {
    lat = $("#lat").val();
    lng = $("#lng").val();
    init_map1(lat, lng);
    if (lat > 0 && lng > 0) {
        mapshop.setCenter(AMap.LngLat(lng, lat));
    }
    location_callback1 = callback;
    show_map();
}
function init_map1(lat, lng) {
    if (!mapshop) {
        if (lng <= 0 || lat <= 0) {
            lng = 115.670177;
            lat = 37.73892;
        }
        get_rgeocode(lat, lng);
        mapshop = new AMap.Map('mymap', { resizeEnable: true, view: new AMap.View2D({ zoom: 15, center: new AMap.LngLat(lng, lat) }) });
        mapshop.plugin(["AMap.ToolBar"], function () { var tool = new AMap.ToolBar(); mapshop.addControl(tool); });
        mapshop.plugin('AMap.Geolocation', function () {
            geolocation = new AMap.Geolocation({
                enableHighAccuracy: true,
                timeout: 10000,
                maximumAge: 0,
                convert: true,
                showButton: true,
                buttonPosition: 'LB',
                buttonOffset: new AMap.Pixel(10, 20),
                showMarker: true,
                showCircle: true,
                panToLocation: false,
                zoomToAccuracy: false
            });
            mapshop.addControl(geolocation);
            geolocation.getCurrentPosition();
            AMap.event.addListener(geolocation, 'complete', function (e) {
                mapshop.setCenter(e.position);
                check_location1();
            });
            AMap.event.addListener(mapshop, 'dragend', function (e) { check_location1(); });
            AMap.event.addListener(mapshop, 'zoomend', function (e) { check_location1(); });
        });
    }
}
function check_location1() {
    var center = mapshop.getCenter();
    if (location_callback1) {
        location_callback1(center.getLat(), center.getLng());
    }
}
function xcforder_preorder() {
    mapinit = false;
    cardinit = false;
    $("#service").val("");
    $("#msg").html("附近洗车房查找中……");
    $("#mapmsg").html("附近洗车房查找中……");
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "/BearClient/PrepareOrder/",
        data: { a: $("#lng").val(), b: $("#lat").val() },
        success: function (data) {
            if (data.code == "0") {
                $("#msg").html(data.msg);
                $("#mapmsg").html(data.msg);
                $("#userstate").val("fail");
                $("#branchid").val("0");
            } else if (data.code == "1") {
                $("#msg").html(data.msg);
                $("#mapmsg").html(data.msg);
                $("#userstate").val("ok");
                $("#branchid").val(data.data);
            } else if (data.code == "2") {
                $("#userstate").val("ok");
                $("#branchid").val(data.data);
            }
        },
        error: function (error) {
            showToast("网络错误，请重试");
        }
    });
}
function shoporder() {
    var mobile_show = $("#mobile_show").val();
    var name = $("#name").val();
    var plate = $("#plate").val();
    var brand_model_show = $("#brand_model_show").val();
    var bandid = $("#brand").val();
    var modelid = $("#model").val();
    var color = $("#color").val();
    var userstate = $("#userstate").val();
    var workid = $("#shopid").val();
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
    var itemIds = new Array();
    var pte = true;
    $("input[name='washitem']").each(function () {
        if (this.checked) {
            itemIds.push($(this).attr('id'));
            if ($(this).attr('tys') == "vip") {
                if ($(this).attr('plate') != "" && $(this).attr('plate') != plate) {
                    pte = false;
                }
            }
        }
    });
    if (itemIds.length <= 0) { showToast('请选择服务项目！'); return; }
    var coupIds = new Array();
    $("input[name='couponmodel']").each(function () {
        if (this.checked) {
            coupIds.push($(this).attr('id'));
        }
    });
    if (mobile_show == "") {
        showToast("请输入手机号");
        return;
    } else if (name == "") {
        showToast("请输入称呼");
        return;
    } else if (brand_model_show == "" || plate == "" || color == "") {
        showToast("请选择车型");
        return;
    } else if (userstate == "noinit") {
        showToast("请拖动地图选取位置");
        return;
    } else if (userstate != "ok") {
        showToast("请重新选取位置");
        return;
    } else if (workid == "0") {
        showToast("请选择洗车店");
        return;
    }
    if (!pte) {
        showToast("该会员卡只能服务于绑定车牌");
        return;
    }
    $("#ordersubmit").removeAttr("onclick");
    $.ajax({
        type: 'post',
        traditional: true,
        dataType: "json",
        async: false,
        url: "/BearClient/ShopSubmitOrder",
        data: { workid: workid, coupid: coupIds, 'itemIds': itemIds, 'paymode': val_pay, a: $("#lng").val(), b: $("#lat").val(), mobile: mobile_show, name: name, plate: plate, brandshow: brand_model_show, brand: bandid, model: modelid, color: color, comment: $("#comment").val(), address: $("#address").val() },
        success: function (data) {
            if (data.code == 0) {
                showToast(data.msg);
                $("#ordersubmit").attr("onclick", "shoporder()");
                return;
            } else if (data.code == 1) {
                if (data.msg == "zfb") {
                    window.location.href = "/OrderAlipay/Pay?oid=" + data.data;
                } else if (data.msg == "wx") {
                    window.location.href = "/OrderWeiXinPay/Pay?oid=" + data.data;
                } else if (data.msg == "qb") {
                    window.location.href = "/BearClient/PayResult?oid=" + data.data;
                }
                return;
            }
        }
    });
}
function select_ok1() {
    $("#map").hide();
    location_callback1 = null;
    showshop();
}
/*线下*/
/*通用*/
function show_map() {
    $("#map").show();
    $("#orderinfo").hide();
}
function get_rgeocode(lat, lng) {
    lnglatXY = new AMap.LngLat(lng, lat);
    AMap.service(["AMap.Geocoder"], function () {
        MGeocoder = new AMap.Geocoder({
            radius: 1000,
            extensions: "all"
        });
        MGeocoder.getAddress(lnglatXY, function (status, result) {
            if (status === 'complete' && result.info === 'OK') {
                var addr = result.regeocode.formattedAddress;
                addr = addr.replace(result.regeocode.addressComponent.province, "");
                $("#address").val(addr);
                $("#mapaddress").html(addr);
            }
        });
    });
}
function showcard() {
    if ($("#lng").val() == "0" || $("#lat").val() == "0" || $("#branchid").val() == "0") {
        showToast("请重新选取停车位置");
        cardinit = false;
        return;
    }
    if (!cardinit) {
        $.ajax({
            type: "post",
            dataType: "json",
            url: "/BearClient/CouponsCardList/",
            data: { bid: $("#branchid").val() },
            success: function (data) {
                if (data.code == "1") {
                    $("#couponlist").html(data.data.ListCoupons);
                    $("#coupmsg").text("");
                    $("#loadcoup").hide();
                    $("#loadvipcard").hide();
                    cardinit = true;
                } else {
                    $("#coupmsg").text("没有优惠券");
                }
            },
            error: function () {
                showToast("网络错误，请重试");
            }
        });
    }
    $("#mycoupons").show();
    $("#orderinfo").hide();
    $("#msg").hide();
}
function f(d) {
    var cd = $("#" + d).attr('cd');
    var id = $("#" + d).attr('id');
    $("input[name='couponmodel'][cd='" + cd + "']").each(function () {
        if (this.checked) {
            $("input[name='couponmodel'][cd='" + cd + "'][id!='" + id + "']").removeAttr('checked');
        }
    });
    sum();
}
function hidecoupons() {
    $("#orderinfo").show();
    $("#msg").show();
    $("#mycoupons").hide();
}
function showitem() {
    if ($("#lng").val() == "0" || $("#lat").val() == "0" || $("#branchid").val() == "0") {
        showToast("请重新选取停车位置");
        mapinit = false;
        return;
    }
    $("#loaditem").show();
    if (!mapinit) {
        $.ajax({
            type: "post",
            dataType: "json",
            url: "/BearClient/ItemList/",
            data: { bid: $("#branchid").val() },
            success: function (data) {
                if (data.code == "0") {
                    $("#itemmsg").text(data.msg);
                } else {
                    var html = "";
                    for (var i = 0; i < data.data.WashItem.length; i++) {
                        html += "<label class=\"edo_check_label1\" for=\"item" + data.data.WashItem[i].ID + "\" onclick=\"i('item" + data.data.WashItem[i].ID + "')\">" +
                                "   <input type=\"checkbox\" class=\"edo_check1\" name=\"washitem\" tys=\"item\" id=\"item" + data.data.WashItem[i].ID + "\" value=\"" + data.data.WashItem[i].Price + "\" title=\"" + data.data.WashItem[i].Title + "\" cd=\"" + data.data.WashItem[i].IsMust + "\" />" +
                                "   <i class=\"edo_icon_checked\"></i>" +
                                "   <span>" + data.data.WashItem[i].Title + "(" + data.data.WashItem[i].Price + ")</span>" +
                                "</label>";
                    }
                    $("#washitemlist .washitemlist").html(html);
                    $("#vipcardlist").html(data.data.ListVipCard);
                    $("#itemmsg").text("");
                    mapinit = true;
                }
                $("#loaditem").hide();
            },
            error: function () {
                showToast("网络错误，请重试");
            }
        });
    } else {
        $("#loaditem").hide();
    }
    $("#itemlist").show();
    $("#orderinfo").hide();
    $("#msg").hide();
}
function i(s) {
    var cd = $("#" + s).attr('cd');
    var id = $("#" + s).attr('id');
    $("input[name='washitem'][cd='" + cd + "']").each(function () {
        if (this.checked) {
            $("input[name='washitem'][cd='" + cd + "'][id!='" + id + "']").removeAttr('checked');
        }
    });
    sum();
}
function sum() {
    var money = 0;
    var coupons = 0;
    var sssss = "";
    var s1 = "";
    var bo = false;
    $("input[name='washitem']").each(function () {
        if (this.checked) {
            money += parseFloat($(this).attr('value'));
            if ($(this).attr('tys') == "item") {
                sssss += $(this).attr('title') + "(" + $(this).attr('value') + ")" + " ";
            } else {
                sssss += $(this).attr('title') + " ";
            }
        }
    });
    $("input[name='couponmodel']").each(function () {
        if (this.checked) {
            coupons += parseFloat($(this).attr('value'));
            s1 += $(this).attr('title') + "(" + $(this).attr('value') + ")" + " ";
        }
    });
    $("#coupons").val(s1);
    $("#service").val(sssss);
    var all = money - coupons;
    if (all <= 0) {
        all = 0.00;
    }
    $("#showallmoney").html(all);
    $("#allmoney").val(all);
}
function showitemshop() {
    if ($("#lng").val() == "0" || $("#lat").val() == "0" || $("#branchid").val() == "0") {
        showToast("请重新选取实体店");
        mapinit = false;
        return;
    }
    if (!mapinit) {
        $.ajax({
            type: "post",
            dataType: "json",
            url: "/BearClient/ShopItemList/",
            data: { bid: $("#branchid").val() },
            success: function (data) {
                if (data.code == "0") {
                    $("#itemmsg").text(data.msg);
                } else {
                    var html = "";
                    for (var i = 0; i < data.data.WashItem.length; i++) {
                        html += "<label class=\"edo_check_label1\" for=\"item" + data.data.WashItem[i].ID + "\" onclick=\"i('item" + data.data.WashItem[i].ID + "')\">" +
                                "   <input type=\"checkbox\" class=\"edo_check1\" name=\"washitem\" tys=\"item\" id=\"item" + data.data.WashItem[i].ID + "\" value=\"" + data.data.WashItem[i].Price + "\" title=\"" + data.data.WashItem[i].Title + "\" cd=\"" + data.data.WashItem[i].IsMust + "\" />" +
                                "   <i class=\"edo_icon_checked\"></i>" +
                                "   <span>" + data.data.WashItem[i].Title + "(" + data.data.WashItem[i].Price + ")</span>" +
                                "</label>";
                    }
                    $("#washitemlist .washitemlist").html(html);
                    $("#vipcardlist").html(data.data.ListVipCard);
                    $("#itemmsg").text("");
                    mapinit = true;
                }
            },
            error: function () {
                showToast("网络错误，请重试");
            }
        });
    }
    $("#itemlist").show();
    $("#orderinfo").hide();
    $("#msg").hide();
}
function hideitem() {
    $("#itemlist").hide();
    $("#orderinfo").show();
    $("#msg").show();
}
/*通用*/
/*在线买卡*/
var lock2 = true;
var mapcard;
var location_callback2 = null;
function showmapcard() {
    if (lock2) {
        select_location2(set_location2);
        cardorder_preorder();
    }
}
function select_location2(callback) {
    lat = $("#lat").val();
    lng = $("#lng").val();
    init_map2(lat, lng);
    if (lat > 0 && lng > 0) {
        mapcard.setCenter(AMap.LngLat(lng, lat));
    }
    location_callback2 = callback;
    show_map();
}
function set_location2(lat, lng) {
    $("#lat").val(lat);
    $("#lng").val(lng);
    get_rgeocode(lat, lng);
    cardorder_preorder();
}
function init_map2(lat, lng) {
    if (!mapcard) {
        if (lng <= 0 || lat <= 0) {
            lng = 115.670177;
            lat = 37.73892;
        }
        get_rgeocode(lat, lng);
        mapcard = new AMap.Map('mymap', { resizeEnable: true, view: new AMap.View2D({ zoom: 15, center: new AMap.LngLat(lng, lat) }) });
        mapcard.plugin(["AMap.ToolBar"], function () { var tool = new AMap.ToolBar(); mapcard.addControl(tool); });
        mapcard.plugin('AMap.Geolocation', function () {
            geolocation = new AMap.Geolocation({
                enableHighAccuracy: true,
                timeout: 10000,
                maximumAge: 0,
                convert: true,
                showButton: true,
                buttonPosition: 'LB',
                buttonOffset: new AMap.Pixel(10, 20),
                showMarker: true,
                showCircle: true,
                panToLocation: false,
                zoomToAccuracy: false
            });
            mapcard.addControl(geolocation);
            geolocation.getCurrentPosition();
            AMap.event.addListener(geolocation, 'complete', function (e) {
                mapcard.setCenter(e.position);
                check_location2();
            });
            AMap.event.addListener(mapcard, 'dragend', function (e) { check_location2(); });
            AMap.event.addListener(mapcard, 'zoomend', function (e) { check_location2(); });
        });
    }
}
function check_location2() {
    var center = mapcard.getCenter();
    if (location_callback2) {
        location_callback2(center.getLat(), center.getLng());
    }
}
function cardorder_preorder() {
    mapinit = false;
    cardinit = false;
    $("#service").val("");
    $("#locmsg").html("小熊洗车分公司查找中……");
    $("#mapmsg").html("小熊洗车分公司查找中……");
    $.ajax({
        type: "GET",
        dataType: "json",
        url: "/VipCardList/GetBranch/",
        data: { a: $("#lng").val(), b: $("#lat").val() },
        success: function (data) {
            if (data.code == "0") {
                $("#locmsg").html(data.msg);
                $("#mapmsg").html(data.msg);
                $("#userstate").val("fail");
                $("#branchid").val("0");
            } else if (data.code == "1") {
                $("#locmsg").html(data.msg);
                $("#mapmsg").html(data.msg);
                $("#userstate").val("ok");
                $("#branchid").val(data.data);
                showcardlist();
            } else if (data.code == "2") {
                $("#userstate").val("ok");
                $("#branchid").val(data.data);
            }
        },
        error: function (error) {
            showToast("网络错误，请重试");
        }
    });
}
function showcardlist() {
    $("#load").show();
    $("#cardlist").html("");
    $("#xianshicard").html("");
    $("#xianshi").hide();
    $.ajax({
        type: "post",
        dataType: "json",
        url: "/VipCardList/ItemList/",
        data: { bid: $("#branchid").val() },
        success: function (data) {
            if (data.code == "0") {
                showToast(data.msg);
            } else {
                var html = "";
                if (data.data.VipTypeItemOnline) {
                    for (var i = 0; i < data.data.VipTypeItemOnline.length; i++) {
                        html += "<label class=\"edo_check_label1\" for=\"vip" + data.data.VipTypeItemOnline[i].ID + "\">";
                        if (i == 0) {
                            html += "<input type=\"radio\" class=\"edo_check1\" name=\"typeid\" checked=\"checked\" id=\"vip" + data.data.VipTypeItemOnline[i].ID + "\" value=\"" + data.data.VipTypeItemOnline[i].ID + "\" />";
                        }
                        else {
                            html += "<input type=\"radio\" class=\"edo_check1\" name=\"typeid\" id=\"vip" + data.data.VipTypeItemOnline[i].ID + "\" value=\"" + data.data.VipTypeItemOnline[i].ID + "\" />";
                        }
                        html += "<i class=\"edo_icon_checked\"></i>";
                        html += "<span>" + data.data.VipTypeItemOnline[i].Title + " &nbsp;" + data.data.VipTypeItemOnline[i].Price + "元</span>";
                        //html += "<span class='jscount'>仅剩" + data.data.VipTypeItem[i].CardCount + "张</span>";
                        html += "</label>";
                    }
                    $("#cardlist").html(html);
                }
                var html1 = "";
                if (data.data.VipTypeItemHot) {
                    for (var i = 0; i < data.data.VipTypeItemHot.length; i++) {
                        html1 += "<label class=\"edo_check_label1\" for=\"vip" + data.data.VipTypeItemHot[i].ID + "\">";
                        if (i == 0) {
                            html1 += "<input type=\"radio\" class=\"edo_check1\" name=\"typeid\" checked=\"checked\" id=\"vip" + data.data.VipTypeItemHot[i].ID + "\" value=\"" + data.data.VipTypeItemHot[i].ID + "\" />";
                        }
                        else {
                            html1 += "<input type=\"radio\" class=\"edo_check1\" name=\"typeid\" id=\"vip" + data.data.VipTypeItemHot[i].ID + "\" value=\"" + data.data.VipTypeItemHot[i].ID + "\" />";
                        }
                        html1 += "<i class=\"edo_icon_checked\"></i>";
                        html1 += "<span style=\"color:#F00\">" + data.data.VipTypeItemHot[i].Title + " &nbsp;" + data.data.VipTypeItemHot[i].Price + "元</span>";
                        html1 += "<span class='jscount'>仅剩" + data.data.VipTypeItemHot[i].CardCount + "张</span>";
                        html1 += "</label>";
                    }
                    $("#xianshicard").html(html1);
                    $("#xianshi").show();
                }

            }
            $("#load").hide();
        },
        error: function () {
            showToast("网络错误，请重试");
        }
    });
}
function cardorder() {
    var t = $("input[name='typeid']:checked").val();
    var p = $("#price").val();
    var bid = $("#branchid").val();
    var userstate = $("#userstate").val();
    var workno = $("#workno").val();
    if (!t) {
        showToast("请选择套餐");
        return;
    } else if (userstate == "noinit") {
        showToast("请手动定位或等待自动定位完成");
        return;
    } else if (userstate != "ok") {
        showToast("请重新定位");
        return;
    }
    $("#wxpay").removeAttr("onclick");
    $.post("/VipCardList/OrderCardSubmit", { "typeID": t, "price": p, "bid": bid, "workno": workno }, function (data) {
        var v = JSON.parse(data);
        if (v.code == "1") {
            window.location.href = v.msg;
        }
        else {
            showToast(v.msg);
            $("#wxpay").attr("onclick", "cardorder()");
        }
    })
}
function vipex() {
    var cardno = $("#cardno").val();
    var cardpwd = $("#cardpwd").val();
    var plate = $("#plate").val();
    if (cardno == "") {
        showToast("请输入卡号");
        return;
    } else if (cardpwd == "") {
        showToast("请输入密码");
        return;
    }
    $.post("/VipCardList/ChongZhi", { "cardno": cardno, "cardpwd": cardpwd, "plate": plate }, function (data) {
        var v = JSON.parse(data);
        showToast(v.msg);
    })
}
function select_ok2() {
    $("#map").hide();
    location_callback2 = null;
    $("#orderinfo").show();
    return 0;
}
/*在线买卡*/