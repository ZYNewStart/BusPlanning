//function getPageSize() {
//    var body = document.documentElement;
//    var bodyOffsetWidth = 0;
//    var bodyOffsetHeight = 0;
//    var bodyScrollWidth = 0;
//    var bodyScrollHeight = 0;
//    var pageDimensions = [0, 0];
//    pageDimensions[0] = body.clientHeight;
//    pageDimensions[1] = body.clientWidth;
//    bodyOffsetWidth = body.offsetWidth;
//    bodyOffsetHeight = body.offsetHeight;
//    bodyScrollWidth = body.scrollWidth;
//    bodyScrollHeight = body.scrollHeight;
//    if (bodyOffsetHeight > pageDimensions[0]) {
//        pageDimensions[0] = bodyOffsetHeight;
//    }
//    if (bodyOffsetWidth > pageDimensions[1]) {
//        pageDimensions[1] = bodyOffsetWidth;
//    }
//    if (bodyScrollHeight > pageDimensions[0]) {
//        pageDimensions[0] = bodyScrollHeight;
//    }
//    if (bodyScrollWidth > pageDimensions[1]) {
//        pageDimensions[1] = bodyScrollWidth;
//    }
//    return pageDimensions;
//}

//function DisablePage(sandglassSpan) {
//    var ctrlSandglass = document.getElementById("divSandglass");
//    var divImg = document.getElementById("divLockImg");
//    if (sandglassSpan == 1) {
//        document.body.style.cursor = 'wait';
//        var pageDimensions = getPageSize();
//        ctrlSandglass.style.top = 0;
//        ctrlSandglass.style.left = 0;
//        ctrlSandglass.style.height = pageDimensions[0];
//        ctrlSandglass.style.width = pageDimensions[1];
//        ctrlSandglass.style.display = "block";
//        divImg.style.display = "block";
//    }
//    else {
//        ctrlSandglass.style.display = "none";
//        document.body.style.cursor = 'auto';
//        divImg.style.display = "none";
//    }
//}

function DisablePage(display) {
    if (display == 1) {
        document.body.style.cursor = 'wait';
        $("#BgDiv1").css({
            display: "block",
            height: $(document).height()
        });
        var yscroll = document.documentElement.scrollTop;
        var screenx = $(window).width();
        var screeny = $(window).height();
        $(".DialogDiv").css("display", "block");
        $(".DialogDiv").css("top", yscroll + "px");
        var DialogDiv_width = $(".DialogDiv").width();
        var DialogDiv_height = $(".DialogDiv").height();
        $(".DialogDiv").css("left", (screenx / 2 - DialogDiv_width / 2) + "px")
        $(".DialogDiv").css("top", (screeny / 2 - DialogDiv_height / 2) + "px")
        $("body").css("overflow", "hidden");
    }
    else {
        $("#BgDiv1").css("display", "none");
        $(".DialogDiv").css("display", "none");
        document.body.style.cursor = 'auto';
    }
}