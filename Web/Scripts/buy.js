function alipay() {
    var t = $("input[name='typeid']:checked").val();
    if (!t) {
        alert("请选择套餐");
        return;
    }
    $.post("/Chargelist/AddChargeOrder", { "typeID": t }, function (data) {
        if (data.state == "success") {
            window.location.href = data.msg;
        }
        else {
            alert(a.msg);
        }
    })
}
function wxpay() {
    var t = $("input[name='typeid']:checked").val();
    var p = $("#price").val();
    if (!t) {
        showToast("请选择套餐");
        return;
    }
    $.post("/Chargelist/AddChargeOrderWX", { "typeID": t, "price": p }, function (data) {
        if (data.state == "success") {
            window.location.href = data.msg;
        }
        else {
            showToast(data.msg);
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
    } else if (plate == "") {
        showToast("请输入车牌号");
        return;
    }
    $.post("/Chargelist/ChongZhi", { "cardno": cardno, "cardpwd": cardpwd, "plate": plate }, function (data) {
        if (data.state == "success") {
            showToast(data.msg);
        }
        else {
            showToast(data.msg);
        }
    })
}