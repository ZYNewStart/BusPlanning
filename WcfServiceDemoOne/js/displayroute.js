function DrawSegment(segmentInfo, indexSegment) {
    var Station = new Array();
    for (var i = 0; i < segmentInfo.length; i++) {
        Station.push(new BMap.Point(segmentInfo[i].LNG, segmentInfo[i].LAT));
        //AddMark(new BMap.Point(segmentInfo[i].StopLongitude, segmentInfo[i].StopLatitude), segmentInfo[i].StopName, segmentInfo[i].IsVirtual); //在线路的站点上标注
    }
    //switch (indexSegment) {
    //    case 0:
    //        AddSpecialMark(new BMap.Point(segmentInfo[0].StopLongitude, segmentInfo[0].StopLatitude), segmentInfo[0].StopName, 1); //标注起点
    //        AddSpecialMark(new BMap.Point(segmentInfo[segmentInfo.length - 1].StopLongitude, segmentInfo[segmentInfo.length - 1].StopLatitude), segmentInfo[segmentInfo.length - 1].StopName, 2); //标注终点
    //        break;
    //    case 1:
    //        AddSpecialMark(new BMap.Point(segmentInfo[0].StopLongitude, segmentInfo[0].StopLatitude), segmentInfo[0].StopName, 1); //标注起点
    //        AddSpecialMark(new BMap.Point(segmentInfo[segmentInfo.length - 1].StopLongitude, segmentInfo[segmentInfo.length - 1].StopLatitude), segmentInfo[segmentInfo.length - 1].StopName, 3); //标注换乘
    //        break;
    //    case 2:
    //        AddSpecialMark(new BMap.Point(segmentInfo[segmentInfo.length - 1].StopLongitude, segmentInfo[segmentInfo.length - 1].StopLatitude), segmentInfo[segmentInfo.length - 1].StopName, 3); //标注换乘
    //        break;
    //    case 3:
    //        AddSpecialMark(new BMap.Point(segmentInfo[segmentInfo.length - 1].StopLongitude, segmentInfo[segmentInfo.length - 1].StopLatitude), segmentInfo[segmentInfo.length - 1].StopName, 2); //标注终点
    //        break;
    //    default:
    //}
    map.centerAndZoom(new BMap.Point(segmentInfo[0].LNG, segmentInfo[0].LAT), 15);
    var polyline = new BMap.Polyline(Station, { strokeColor: "red", strokeWeight: 6, strokeOpacity: 0.5 }); //在地图上画线
    map.addOverlay(polyline); //画线
    AddSpecialMark(new BMap.Point(segmentInfo[0].LNG, segmentInfo[0].LAT), "起点站", 1); //标注起点
    AddSpecialMark(new BMap.Point(segmentInfo[segmentInfo.length - 1].LNG, segmentInfo[segmentInfo.length - 1].LAT), "终点站", 2); //标注终点
}

//功能：在地图中添加标注
//参数：MarkPoint：坐标，StationInfo：信息框中信息，PointStyle：站点类型
//PointStyle:站点类型 1：起点 2：终点 3：换乘
function AddSpecialMark(MarkPoint, StationInfo, PointStyle) {
    switch (PointStyle) {
        case 1:
            var myIcon = new BMap.Icon("/img/startpoint32.ico", new BMap.Size(32, 32), { anchor: new BMap.Size(16, 32) });
            StationInfo = "起点站:" + StationInfo;
            break;
        case 2:
            var myIcon = new BMap.Icon("/img/endpoint32.ico", new BMap.Size(32, 32), { anchor: new BMap.Size(16, 32) });
            StationInfo = "终点站:" + StationInfo;
            break;
        case 3:
            var myIcon = new BMap.Icon("/img/transferpoint32.ico", new BMap.Size(32, 32), { anchor: new BMap.Size(16, 32) });
            StationInfo = "换乘站:" + StationInfo;
            break;
        default:
            var myIcon = new BMap.Icon("/img/TransitStop.ico", new BMap.Size(32, 32), { anchor: new BMap.Size(16, 32) });
    }
    var marker2 = new BMap.Marker(MarkPoint, { icon: myIcon });  // 创建标注
    map.addOverlay(marker2);              // 将标注添加到地图中 
    var infoWindow2 = new BMap.InfoWindow("<p style='font-size:18px;'>" + StationInfo + "</p>");
    marker2.addEventListener("click", function () { this.openInfoWindow(infoWindow2); });
}

function ajaxGetRegCityName(timelistid) {
    $.ajax({
        url: "/Service1.svc/GetRegCityNameByDate",
        contentType: "application/json",
        data: '{"timelistid":' + timelistid + '}',
        type: "POST",
        dataType: "json",
        success: function (returnValue) {
            GetAllRegCityName(returnValue);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status);
            alert(XMLHttpRequest.readyState);
            alert(textStatus);
            alert("提交失败！");
        }
    });
}

function ajaxGetRegionNewUser() {
    $.ajax({
        url: "/Service1.svc/GetRegionNewUser",
        contentType: "application/json",
        type: "POST",
        dataType: "json",
        success: function (returnValue) {
            GetNewUser(returnValue);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status);
            alert(XMLHttpRequest.readyState);
            alert(textStatus);
            alert("提交失败！");
        }
    });
}

function ajaxSelectedCityName(cityname, timelistid) {
    $.ajax({
        url: "/Service1.svc/GetAllRouteInfoByCity",
        contentType: "application/json",
        type: "POST",
        data: '{"cityname":"' + cityname + '","timelistid":'+ timelistid + '}',
        dataType: "json",
        success: function (returnValue) {
            if (returnValue == null || returnValue == "") {
                alert("数据读取失败！请稍后再试。");
                return;
            }
            for (var i = 0; i < returnValue.length; i++) {
                DrawSegment(returnValue[i].RouteList, 1);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status);
            alert(XMLHttpRequest.readyState);
            alert(textStatus);
            alert("提交失败！");
            //DisablePage(0);
        }
    });
}

function ajaxGetRoute() {
    //DisablePage(1);
    $.ajax({
        url: "/Service1.svc/GetRegRoute",
        contentType: "application/json",
        type: "POST",
        data: '{"userid":100}',
        dataType: "json",
        success: function (returnValue) {
            DrawSegment(returnValue.RouteList, 1);
            //DisablePage(0);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status);
            alert(XMLHttpRequest.readyState);
            alert(textStatus);
            alert("提交失败！");
            //DisablePage(0);
        }
    });
}

function GetCityName(result) {
    var cityName = result.name;
    map.centerAndZoom(cityName);
}

function GetAllRegCityName(citylist) {
    var select = document.getElementById('selectcity');
    for (var i = 0; i < citylist.length; i++) {
        var cityli = document.createElement('option');
        cityli.innerHTML = citylist[i];
        select.appendChild(cityli);
    }
}

function GetNewUser(newuser) {
    var body = document.getElementById('bodynewuser');
    var regionname = [];
    for (var i = 0; i < newuser.length; i++) {
        var tr = document.createElement('tr');
        tr.className = (i%2 == 0 ? "success":"danger");
        var tdregion = document.createElement('td');
        tdregion.innerHTML = newuser[i].RegionName;
        var tduserno = document.createElement('td');
        tduserno.innerHTML = newuser[i].UserNo;
        tr.appendChild(tdregion);
        tr.appendChild(tduserno);
        body.appendChild(tr);
        regionname.push(newuser[i].RegionName);
    }
    GetAllRegCityName(regionname);//更新城市
}

function DateSelectChange(obj) {
    var city = document.getElementById('selectcity');
    city.options.length = 0;
    ajaxGetRegCityName(obj.value);
}

function QueryRoute() {
    map.clearOverlays();
    var city = document.getElementById('selectcity');
    var indexcity = city.selectedIndex;
    var date = document.getElementById('selectdate');
    var indexdate = date.selectedIndex;
    ajaxSelectedCityName(city.options[indexcity].text, date.options[indexdate].value);
}