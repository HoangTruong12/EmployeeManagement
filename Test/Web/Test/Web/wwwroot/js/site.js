﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
        }
    });
};

ajaxEdit = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    window.location.href = '/Employee/Index';
                    //$("#view-all").html(res.html);
                    $("#form-modal .modal-body").html('');
                    $("#form-modal .modal-title").html('');

                    $("#form-modal").modal('hide');
                }
                else {
                    $("#form-modal .modal-body").html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
    }
    catch (e) {
        console.log(e);
    }
    return false;
};

//ajaxCreate = form => {
//    try {
//        $.ajax({
//            type: 'POST',
//            url: form.action,
//            data: new FormData(form),
//            contentType: false,
//            processData: false,
//            success: function (res) {
//                if (res.isValid) {
//                    window.location.href = '/Employee/Index';
//                    //$("#view-all").html(res.html);
//                    $("#form-modal .modal-body").html('');
//                    $("#form-modal .modal-title").html('');

//                    $("#form-modal").modal('hide');
//                }
//                else {
//                    window.location.href = '/Employee/Create';
//                    $("#form-modal .modal-body").html(res.html);
//                }
//            },
//            error: function (err) {
//                console.log(err)
//            }
//        })
//    }
//    catch (e) {
//        console.log(e);
//    }
//    return false;
//}

ajaxSendNotificationToAll = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.type == "Success") {
                    window.location.href = '/Employee/Index';
                    //$("#view-all").html(res.html);
                    $("#form-modal .modal-body").html('');
                    $("#form-modal .modal-title").html('');

                    $("#form-modal").modal('hide');
                }
                else {
                    window.location.href = '/Notification/Create';
                    $("#form-modal .modal-body").html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
    }
    catch (e) {
        console.log(e);
    }
}