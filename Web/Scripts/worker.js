var Toast = function (config) { this.context = config.context == null ? $('body') : config.context; this.message = config.message; this.time = config.time == null ? 5000 : config.time; this.left = config.left; this.top = config.top; this.init(); }; var msgEntity; Toast.prototype = { init: function () { $("#toastMessage").remove(); var msgDIV = new Array(); msgDIV.push('<div id="toastMessage">'); msgDIV.push('<span>' + this.message + '</span>'); msgDIV.push('</div>'); msgEntity = $(msgDIV.join('')).appendTo(this.context); var left = this.left == null ? this.context.width() / 2 - (msgEntity.find('span').width() + 120) / 2 : this.left; var top = this.top == null ? $(document).scrollTop() + 200 + 'px' : this.top; msgEntity.css({ position: 'absolute', top: top, 'z-index': '99999', left: left, 'background': 'rgba(40,40,40,.75)', color: '#FFFFFF', 'border-radius': '5px', 'font-size': '18px', 'text-align': 'center', padding: '10px 30px', margin: '10px' }); msgEntity.hide(); }, show: function () { msgEntity.show(); setTimeout(function () { msgEntity.hide(); }, this.time); } }
function showToast(msg) { new Toast({ context: $('body'), message: msg }).show(); }
var map;
var location_callback = null;
function set_location(lat, lng) {
    $("#lat").val(lat);
    $("#lng").val(lng);
    get_rgeocode(lat, lng);
}
function select_location(callback) {
    lat = $("#lat").val();
    lng = $("#lng").val();
    init_map(lat, lng);
    if (lat > 0 && lng > 0) {
        map.setCenter(AMap.LngLat(lng, lat));
    }
    location_callback = callback;
}
var lock = true;
function showmap() {
    if (lock) {
        select_location(set_location);
    }
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
                var marker = new AMap.Marker({
                    offset: new AMap.Pixel(-15, -15),
                    icon: '/Content/images/car.gif',
                    position: e.position,
                    draggable: false,
                    cursor: 'move',
                    raiseOnDrag: true
                });
                marker.setMap(map);
            });
            AMap.event.addListener(map, 'dragend', function (e) { check_location(); });
            AMap.event.addListener(map, 'zoomend', function (e) { check_location(); });
        });
    }
}
function select_ok() {
    dismiss_map();
}
function check_location() {
    var center = map.getCenter();
    if (location_callback) {
        location_callback(center.getLat(), center.getLng());
    }
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
var pagenum = 2;
function getOrdersList1(page) {
    $("#loadbtn").hide();
    $("#loadorder").show();
    $.ajax({
        type: "post",
        traditional: true,
        dataType: "json",
        url: "/WebWorker/Orders/AjaxOrderList1/",
        data: { page: pagenum, sd: $("txtSDate").val(), ed: $("txtEDate").val() },
        success: function (data) {
            if (data.code == "0") {
                $("#ordermsg").text(data.msg);
                $("#loadorder").hide();
            } else {
                if (data.data.length > 0) {
                    $("#orderListBlock").append(data.data);
                    $("#loadbtn").show();
                    $("#loadorder").hide();
                    pagenum += 1;
                }
                else {
                    $("#ordermsg").text("没有了");
                    $("#loadbtn").hide();
                    $("#loadorder").hide();
                }
            }
        },
        error: function () {
            showToast("网络错误，请重试");
        }
    });
}
function getOrdersList2(page) {
    $("#loadbtn").hide();
    $("#loadorder").show();
    $.ajax({
        type: "post",
        traditional: true,
        dataType: "json",
        url: "/WebWorker/Orders/AjaxOrderList2/",
        data: { page: pagenum, sd: $("txtSDate").val(), ed: $("txtEDate").val() },
        success: function (data) {
            if (data.code == "0") {
                $("#ordermsg").text(data.msg);
                $("#loadorder").hide();
            } else {
                if (data.data.length > 0) {
                    $("#orderListBlock").append(data.data);
                    $("#loadbtn").show();
                    $("#loadorder").hide();
                    pagenum += 1;
                }
                else {
                    $("#ordermsg").text("没有了");
                    $("#loadbtn").hide();
                    $("#loadorder").hide();
                }
            }
        },
        error: function () {
            showToast("网络错误，请重试");
        }
    });
}