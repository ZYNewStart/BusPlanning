var mousestate = 0;
var StartPoint, EndPoint, PathPoints, DragPoints, StartName, EndName;
/*
 * 	var myCity = new BMap.LocalCity();
 *	myCity.get(myFun);
 */
function GetCityName(result){
	var cityName = result.name;
	map.centerAndZoom(cityName);
}

function SearchDrivingRoute(startPoint,endPoint){
	var transit = new BMap.DrivingRoute(map, {
		renderOptions: {
		    map: map,
			panel: "r-result",
			autoViewport: true,
			enableDragging : true //起终点可进行拖拽
		},policy: 0 //时间最少 
	});
	transit.setSearchCompleteCallback(function (result) {
	    PathPoints = "";
	    DragPoints = "";
	    if (result == null) {
	        return;
	    }
	    map.clearOverlays();
	    var dragpoints = result.getPlan(0).getDragPois();
	    for (var i = 0; i < dragpoints.length; i++) {
	        DragPoints += dragpoints[i].point.lng + "," + dragpoints[i].point.lat + ";";
	    }
	    for (var j = 0; j < result.getPlan(0).getNumRoutes() ; j++) {
	        var pathpoints = result.getPlan(0).getRoute(j).getPath();
	        for (var i = 0; i < pathpoints.length; i++) {
	            PathPoints += pathpoints[i].lng + "," + pathpoints[i].lat + ";";
	        }
	    }
	});
	transit.search(startPoint, endPoint);
}

	
function ajaxSubmitInfo() {

    var ischeck = CheckInput();
    if (ischeck == false) {
        alert("联系方式输入有误！格式如13xxxxxxxxx，0755-8394xxxx等");
        return;
    }
    DisablePage(1);
           $.ajax({
           url: "/Service1.svc/RegisterUserInfo",
           contentType:"application/json",
               type: "POST",
               data: '{"Contact":"' + $("#telephone").val() + '","StartName":"'+ StartName +'","EndName":"'+ EndName +'","DragPoints":"' + DragPoints + '","RouteInfo":"' + PathPoints + '"}',
               dataType: "json",
               success: function (returnValue) {
                   if (returnValue == "1") {
                       alert("提交路线成功！我们将尽快和您取得联系，敬请关注！");
                   }
                   else {
                       alert("提交失败！");
                   }
                   DisablePage(0);
               },
	            error: function(XMLHttpRequest, textStatus, errorThrown) {
	                //alert(XMLHttpRequest.status);
	                //alert(XMLHttpRequest.readyState);
	                //alert(textStatus);
	                alert("提交失败！");
	                DisablePage(0);
	            }

           });
}

// 定义一个控件类,即function
function StartEndControl(){
	  // 默认停靠位置和偏移量
	  this.defaultAnchor = BMAP_ANCHOR_TOP_LEFT;
	  this.defaultOffset = new BMap.Size(10, 10);
}

function AddStartEndControl() {

    // 通过JavaScript的prototype属性继承于BMap.Control
    StartEndControl.prototype = new BMap.Control();

    // 自定义控件必须实现自己的initialize方法,并且将控件的DOM元素返回
    // 在本方法中创建个div元素作为控件的容器,并将其添加到地图容器中
    StartEndControl.prototype.initialize = function (map) {
        mousestate = 0;
        //设置地图默认的鼠标指针样式
        map.setDefaultCursor("url('/img/startpoint32.ico'),default");
        // 创建一个DOM元素
        var div = document.createElement("div");
        var startbtn = document.createElement("button");
        startbtn.innerHTML = "选取起终点";
        startbtn.className = "btn";
        startbtn.id = "btncontrol";
        div.appendChild(startbtn);

        // 设置样式
        div.style.cursor = "pointer";
        div.style.border = "1px solid gray";
        div.style.backgroundColor = "white";
        div.style.display = "none";
        // 绑定事件,点击一次放大两级
        startbtn.onclick = function (e) {
            //单击获取点击的经纬度
            map.addEventListener("click", function (e) {
                if (mousestate == 0) {
                    mousestate = 1;
                    map.clearOverlays();
                    var myIcon1 = new BMap.Icon("/img/startpoint32.ico", new BMap.Size(32, 32), { anchor: new BMap.Size(16, 32) });
                    var marker1 = new BMap.Marker(e.point, { icon: myIcon1 });  // 创建标注
                    map.addOverlay(marker1);
                    StartPoint = e.point;
                    SetPointDetails(e.point, "startpoint");
                    map.setDefaultCursor("url('/img/endpoint32.ico'),default");

                }
                else {
                    mousestate = 0;
                    var myIcon2 = new BMap.Icon("/img/endpoint32.ico", new BMap.Size(32, 32), { anchor: new BMap.Size(16, 32) });
                    var marker2 = new BMap.Marker(e.point, { icon: myIcon2 });  // 创建标注
                    map.addOverlay(marker2);              // 将标注添加到地图中 
                    EndPoint = e.point;
                    SetPointDetails(e.point, "endpoint");
                    map.clearOverlays();
                    SearchDrivingRoute(StartPoint, EndPoint);
                    //mousestate = 0;
                    map.setDefaultCursor("url('/img/startpoint32.ico'),default");
                }
            });
        }
        // 添加DOM元素到地图中
        map.getContainer().appendChild(div);
        // 将DOM元素返回
        return div;
    }
    // 创建控件
    var myCtrl = new StartEndControl();
    // 添加到地图当中
    map.addControl(myCtrl);
}

function SetPointDetails(point, element) {
    var gc = new BMap.Geocoder();
    gc.getLocation(point, function (rs) {
        var addComp = rs.addressComponents;
        var details = addComp.district + addComp.street + addComp.streetNumber;//addComp.province +  addComp.city + 
        if (mousestate == 1) {
            StartName = addComp.city;//StartName = addComp.city + addComp.district;
        }
        else if (mousestate == 0) {
            EndName = addComp.city;//EndName = addComp.city + addComp.district;
        }
        document.getElementById(element).value = details;
    });
}

function StartEndExchange(startelement, endelement) {
    var startstr = document.getElementById(startelement).value;
    var endstr = document.getElementById(endelement).value;
    document.getElementById(startelement).value = endstr;
    document.getElementById(endelement).value = startstr;
    var startpoi = StartPoint;
    StartPoint = EndPoint;
    EndPoint = startpoi;
    if (StartPoint != '' && EndPoint != '') {
        SearchDrivingRoute(StartPoint, EndPoint);
        document.getElementById("telephone").focus();
    }
}

function CheckInput() {
    var contact = document.getElementById("telephone").value;
    var result = contact.match(/^((\d{11})|(\d{7,8})|(\d{4}|\d{3})-(\d{7,8}))$/);
    if (result == null) { 
        //document.getElementById("telephonealter").style.display = "normal";
        return false;
    }
    return true;
}

function AddSEPointAutoComplete() {

    var sp = new BMap.Autocomplete(    //建立一个自动完成的对象
		{
		    "input": "startpoint"
		, "location": map
		});
    sp.addEventListener("onhighlight", function (e) {  //鼠标放在下拉列表上的事件
        var str = "";
        var _value = e.fromitem.value;
        var value = "";
        if (e.fromitem.index > -1) {
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
        }
        str = "FromItem<br />index = " + e.fromitem.index + "<br />value = " + value;

        value = "";
        if (e.toitem.index > -1) {
            _value = e.toitem.value;
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
            StartName = _value.city;//StartName = _value.city + _value.district;
        }
        str += "<br />ToItem<br />index = " + e.toitem.index + "<br />value = " + value;
        G("searchResultPanel").innerHTML = str;
    });

    var myValue;
    sp.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
        var _value = e.item.value;
        myValue = _value.province + _value.city + _value.district + _value.street + _value.business;
        G("searchResultPanel").innerHTML = "onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;
        setPlace(0, myValue);
        StartName = _value.city;//StartName = _value.city + _value.district;
    });


    var ep = new BMap.Autocomplete(    //建立一个自动完成的对象
    {
        "input": "endpoint",
        "location": map
    });

    ep.addEventListener("onhighlight", function (e) {  //鼠标放在下拉列表上的事件
        var str = "";
        var _value = e.fromitem.value;
        var value = "";
        if (e.fromitem.index > -1) {
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
        }
        str = "FromItem<br />index = " + e.fromitem.index + "<br />value = " + value;

        value = "";
        if (e.toitem.index > -1) {
            _value = e.toitem.value;
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
            EndName = _value.city;//EndName = _value.city + _value.district;
        }
        str += "<br />ToItem<br />index = " + e.toitem.index + "<br />value = " + value;
        G("searchResultPanel").innerHTML = str;
    });

    var myValue1;
    ep.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
        var _value = e.item.value;
        myValue1 = _value.province + _value.city + _value.district + _value.street + _value.business;
        G("searchResultPanel").innerHTML = "onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;
        EndName = _value.city;//EndName = _value.city + _value.district;
        setPlace(1,myValue1);
    });
}

// 百度地图API功能
function G(id) {
    return document.getElementById(id);
}

function setPlace(isend, myValue) {
    map.clearOverlays();
    function myFun() {
        var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果
        if (isend == 0) {
            mousestate = 1;
            map.clearOverlays();
            var myIcon1 = new BMap.Icon("/img/startpoint32.ico", new BMap.Size(32, 32), { anchor: new BMap.Size(16, 32) });
            var marker1 = new BMap.Marker(pp, { icon: myIcon1 });  // 创建标注
            map.addOverlay(marker1);
            StartPoint = pp;
            //SetPointDetails(pp, "startpoint");
        }
        else {
            mousestate = 2;
            var myIcon2 = new BMap.Icon("/img/endpoint32.ico", new BMap.Size(32, 32), { anchor: new BMap.Size(16, 32) });
            var marker2 = new BMap.Marker(pp, { icon: myIcon2 });  // 创建标注
            map.addOverlay(marker2);              // 将标注添加到地图中 
            EndPoint = pp;
            //SetPointDetails(pp, "endpoint");
            map.clearOverlays();
            SearchDrivingRoute(StartPoint, EndPoint);
            mousestate = 0;
        }
    }
    var local = new BMap.LocalSearch(map, { //智能搜索
        onSearchComplete: myFun
    });
    local.search(myValue);
}

function ManualSearchDrivingRoute() {
    var startname = $("#startpoint").val();
    var endname = $("#endpoint").val();
    SearchDrivingRoute(startname,endname);
}