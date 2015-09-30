<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WcfServiceDemoOne.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    //参数为整数的方法
       function fn1()
       {
           $.ajax({
           url: "http://localhost:12079/Service1.svc/GetData",
               type: "POST",
               contentType: "text/json",
               data: '{"value":2}',
               dataType: "json",
               success: function(returnValue) {
                   alert(returnValue);
               },
               error: function() {
                   alert('error');
               }
           });

       }

//参数为实体类的方法
       function fn2() {
           $.ajax({
           url: "http://localhost:12079/Service1.svc/GetDataUsingDataContract",
               type: "POST",
               contentType: "application/json",
               data: '{"CityID":1,"CityName":"北京","Seq":"3"}',
               dataType: "json",
               success: function(returnValue) {
               alert(returnValue.CityID + '  ' + returnValue.CityName + "--" + returnValue.Seq);
               },
               error: function() {
                   alert('error');
               }
           });
       }

//返回值为类集合的方法
       function fn3() {
           $.ajax({
               url: "http://localhost:12079/Service1.svc/GetList",
               type: "POST",
               contentType: "application/json",
               dataType: "json",
               success: function(returnValue) {
               for (var i = 0; i < returnValue.length; i++) {
                   alert(returnValue[i].CityID + '  ' + returnValue[i].CityName+'---'+returnValue[i].Seq);
                   }
               },
               error: function() {
                   alert('error');
               }
           });

       }

       function fn4() {
           $.ajax({
           url: "http://localhost:12079/Service1.svc/GetListData",
               type: "POST",
               contentType: "application/json",
               data: '[{"CityID":1,"CityName":"北京","Seq":"3"},{"CityID":3,"CityName":"上海","Seq":"3"}]',
               dataType: "json",
               success: function(returnValue) {
               for (var i = 0; i < returnValue.length; i++) {
                   alert(returnValue[i].CityID + '  ' + returnValue[i].CityName + '---' + returnValue[i].Seq);
               }
               },
               error: function() {
                   alert('error');
               }
           });
       }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <input id="Button1" type="button" value="调用1" onclick="fn1();" /></div>
        <input id="Button2" type="button" value="调用2" onclick="fn2();" />
        <br />
    <input id="Button3" type="button" value="调用3" onclick="fn3();" /></form>
    <br />
    <input id="Button4" type="button" value="调用4"  onclick="fn4();"/>
    
</body>
</html>
