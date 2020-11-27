var popup;

function ShowPopup(url) {
    var formDiv = $('<div/>');
    $.get(url)
        .done(function (response) {
            formDiv.html(response);
            popup = formDiv.dialog({
                autoOpen: true,
                resizeable: false,
                width: 600,
                height: 400,
                title: 'Добавить сайт',
                close: function () {
                    popup.dialog('destroy').remove();
                }
            });
        });
}

function SubmitAdd(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var data = $(form).serializeJSON();
        data = JSON.stringify(data);
        console.log(data);

        $.ajax({
            type: 'POST',
            url: '/api/crud',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.success) {
                    popup.dialog('close');
                    ShowMessage(data.message);
                    console.log("row1:" + row(data.site));
                    $("#ListPartial > div > div > table tr:first").after(row(data.site));
                }
            }
        });

    }
    return false;
}

function Delete(id) {

    $.ajax({
        type: 'DELETE',
        url: '/api/crud/' + id,
        success: function (data) {
            if (data.success) {
                ShowMessage(data.message);
                $('#tr-' + id).fadeOut("slow");
            }
        }
    });
};
function ShowMessage(msg) {
    toastr.success(msg);
}

// создание строки для таблицы
var row = function (site) {


    return "<tr id=\"tr-" + site.id + "\">" +
        "<td>" + site.id + "</td>" +
        "<td>" +
        "<a href =\"" + site.url + "\" target=\"_blank\">" + site.url + "</a>" +
        "</td>" +
        "<td></td>" +
        "<td></td>" +
        "<td></td>" +
        "</tr>";

};