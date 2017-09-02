$(document).ready(function () {
    var area_id = 0;
    if (typeof (nc_a[area_id]) == 'object' && nc_a[area_id].length > 0) {
        var area_select = $("#region > select");
        areaInit(area_select, area_id);
    }
    $("#region > select").change(regionChange); // select的onchange事件           
})
function regionChange() {
    $(this).nextAll("select").remove();
    // 计算当前选中到id和拼起来的name
    var selects = $(this).siblings("select").andSelf();
    var names = new Array();
    for (i = 0; i < selects.length; i++) {
        sel = selects[i];
        if (sel.value > 0) {
            name = sel.options[sel.selectedIndex].text;
            names.push(name);
        }
    }
    $("#pcregion").val(names);
    $("#ProvinceName").val(name);
    $("#CityName").val(names);
    if (this.value > 0) {//下级地区
        var area_id = this.value;
        if (typeof (nc_a[area_id]) == 'object' && nc_a[area_id].length > 0) {//数组存在
            $("<select></select>").change(regionChange).insertAfter(this);
            areaInit($(this).next("select"), area_id); //初始化地区
        }
    }
}
function regChange(area_select) {
    $(area_select).nextAll("select").remove();
    // 计算当前选中到id和拼起来的name
    var selects = $(area_select).siblings("select").andSelf();
    var names = new Array();
    for (i = 0; i < selects.length; i++) {
        sel = selects[i];
        if (sel.value > 0) {
            name = sel.options[sel.selectedIndex].text;
            names.push(name);
        }
    }
    if ($(area_select).val() > 0) {//下级地区
        var area_id = $(area_select).val();
        if (typeof (nc_a[area_id]) == 'object' && nc_a[area_id].length > 0) {//数组存在
            $("<select></select>").change(regionChange).insertAfter(area_select);
            areaInit($(area_select).next("select"), area_id); //初始化地区
        }
    }
}
function areaInit(area_select, area_id) {
    if (typeof (area_select) == 'object' && nc_a[area_id].length > 0) {
        var areas = new Array();
        areas = nc_a[area_id];
        $(area_select).append("<option>-请选择-</option>");
        var hfregion = $("#pcregion").val();
        for (i = 0; i < areas.length; i++) {
            if (hfregion.length > 0 && hfregion.indexOf(areas[i][1]) >= 0) {
                $(area_select).append("<option value='" + areas[i][0] + "' selected='selected'>" + areas[i][1] + "</option>");
            } else {
                $(area_select).append("<option value='" + areas[i][0] + "'>" + areas[i][1] + "</option>");
            }
        }
        if (hfregion.length > 0) {
            regChange(area_select);
        }
    }
}