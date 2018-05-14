
function htmlEncode(str) {
    var ele = document.createElement('span');
    ele.appendChild(document.createTextNode(str));
    return ele.innerHTML;
}
function htmlDecode(str) {
    var ele = document.createElement('span');
    ele.innerHTML = str;
    return ele.textContent;
}

function actionIndex() {
    parent.window.location.href = "/Home/Index";
}
//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

//复制文本
function copyToClipboard(txt) {
    if (window.clipboardData) {
        window.clipboardData.clearData();
        clipboardData.setData("Text", txt);
        alert("复制成功！");

    } else if (navigator.userAgent.indexOf("Opera") != -1) {
        window.location = txt;
    } else if (window.netscape) {
        try {
            netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
        } catch (e) {
            alert("被浏览器拒绝！\n请在浏览器地址栏输入'about:config'并回车\n然后将 'signed.applets.codebase_principal_support'设置为'true'");
        }
        var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
        if (!clip)
            return;
        var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
        if (!trans)
            return;
        trans.addDataFlavor("text/unicode");
        var str = new Object();
        var len = new Object();
        var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
        var copytext = txt;
        str.data = copytext;
        trans.setTransferData("text/unicode", str, copytext.length * 2);
        var clipid = Components.interfaces.nsIClipboard;
        if (!clip)
            return false;
        clip.setData(trans, null, clipid.kGlobalClipboard);
        alert("复制成功！");
    } else {
        alert("复制失败，浏览器不提供此功能！");
    }
}

//上传图片
function uploadSysImg(fileId) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    formData.append("fileicon", $("#" + fileId + "")[0].files[0]);
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/UploadSysImg",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

//上传多个图片
function uploadSysImgs(fileId, imgfiles) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    var fileInput = $("#" + fileId + "")[0].files;
    for (var i = 0; i < fileInput.length; i++) {
        var names = fileInput[i].name;
        var index = $.inArray(names, imgfiles);
        if (index >= 0) {
            formData.append("fileImg_" + i, $("#" + fileId + "")[0].files[i]);
        }
    }
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/UploadSysImgs",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

//上传文件
function uploadSysFile(fileId) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    formData.append("fileicon", $("#" + fileId + "")[0].files[0]);
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/UploadSysFile",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

//上传多个文件
function uploadSysFiles(fileId) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    var upfiles = $("#" + fileId + "")[0].files;
    if (upfiles != null) {
        for (var i = 0; i < upfiles.length; i++) {
            formData.append("files" + i, upfiles[i]);
        }
    }
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/UploadSysFiles",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

//上传图片
function uploadImg(fileId) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    formData.append("fileicon", $("#" + fileId + "")[0].files[0]);
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/UploadImg",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

//上传多个图片
function uploadImgs(fileId, imgfiles) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    var fileInput = $("#" + fileId + "")[0].files;
    for (var i = 0; i < fileInput.length; i++) {
        var names = fileInput[i].name;
        var index = $.inArray(names, imgfiles);
        if (index >= 0) {
            formData.append("fileImg_" + i, $("#" + fileId + "")[0].files[i]);
        }
    }
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/UploadImgs",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

//上传文件
function uploadFile(fileId) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    formData.append("fileicon", $("#" + fileId + "")[0].files[0]);
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/UploadFile",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

//上传多个文件
function uploadFiles(fileId) {
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    var upfiles = $("#" + fileId + "")[0].files;
    if (upfiles != null) {
        for (var i = 0; i < upfiles.length; i++) {
            formData.append("files" + i, upfiles[i]);
        }
    }
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/UploadFiles",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
        }
    });
    return dataret;
}

//上传多个资源文件
function uploadResourceFiles(fileId, resourceId) {
    $.loading(true, "正在上传文件……");
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    formData.append("resourceId", resourceId);
    var upfiles = $("#" + fileId + "")[0].files;
    if (upfiles != null) {
        for (var i = 0; i < upfiles.length; i++) {
            formData.append("files" + i, upfiles[i]);
        }
    }
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/uploadResourceFiles",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
            $.loading(false);
        }
    });
    return dataret;
}

//上传多个系统模板资源文件
function uploadSysResourceFiles(fileId, parentId, resourceId) {
    $.loading(true, "正在上传文件……");
    var dataret = { success: true, data: "" };
    var formData = new FormData();
    formData.append("resourceId", resourceId);
    formData.append("parentId", parentId);
    var upfiles = $("#" + fileId + "")[0].files;
    if (upfiles != null) {
        for (var i = 0; i < upfiles.length; i++) {
            formData.append("files" + i, upfiles[i]);
        }
    }
    $.ajax({
        type: "POST",
        url: "/SystemManage/UpFile/UploadSysResourceFiles",
        dataType: "json",
        data: formData,
        async: false,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        success: function (data) {
            var jsonData = eval(data);
            if (jsonData.message == "true") {
                if (jsonData.data != null && jsonData.data != undefined) {
                    //filePath = jsonData.data; 
                    dataret.success = true;
                    dataret.data = jsonData.data;
                }
            } else {
                dataret.success = false;
                dataret.data = jsonData.data;
                alert(jsonData.data);
                return;
            }
            $.loading(false);
        }
    });
    return dataret;
}

//获取当前日期
function getDateNow(formatStr) {
    var nowDate = "";
    var myDate = new Date();
    nowDate = myDate.Format(formatStr);
    return nowDate;
}
//格式化日期
function formatDateToStr(formatStr,str) {
    var nowDate = "";
    var myDate = new Date(str);
    nowDate = myDate.Format(formatStr);
    return nowDate;
}
Date.prototype.Format = function (formatStr) {
    var str = formatStr;
    var Week = ['日', '一', '二', '三', '四', '五', '六'];

    str = str.replace(/yyyy|YYYY/, this.getFullYear());
    str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : '0' + (this.getYear() % 100));

    str = str.replace(/MM/, this.getMonth() > 9 ? this.getMonth().toString() : '0' + this.getMonth());
    str = str.replace(/M/g, this.getMonth());

    str = str.replace(/w|W/g, Week[this.getDay()]);

    str = str.replace(/dd|DD/, this.getDate() > 9 ? this.getDate().toString() : '0' + this.getDate());
    str = str.replace(/d|D/g, this.getDate());

    str = str.replace(/hh|HH/, this.getHours() > 9 ? this.getHours().toString() : '0' + this.getHours());
    str = str.replace(/h|H/g, this.getHours());
    str = str.replace(/mm/, this.getMinutes() > 9 ? this.getMinutes().toString() : '0' + this.getMinutes());
    str = str.replace(/m/g, this.getMinutes());

    str = str.replace(/ss|SS/, this.getSeconds() > 9 ? this.getSeconds().toString() : '0' + this.getSeconds());
    str = str.replace(/s|S/g, this.getSeconds());

    return str;
}

var oldHTML = $.fn.html;
$.fn.formhtml = function () {
    if (arguments.length) return oldHTML.apply(this, arguments);
    $("input,textarea,button", this).each(function () {
        this.setAttribute('value', this.value);
    });
    $("textarea", this).each(function () {
        this.innerText = this.value
    });
    $(":radio,:checkbox", this).each(function () {
        // im not really even sure you need to do this for "checked"  
        // but what the heck, better safe than sorry  
        if (this.checked) this.setAttribute('checked', 'checked');
        else this.removeAttribute('checked');
    });
    $("option", this).each(function () {
        // also not sure, but, better safe...  
        if (this.selected) this.setAttribute('selected', 'selected');
        else this.removeAttribute('selected');
    });
    return oldHTML.apply(this);
};
