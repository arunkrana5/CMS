﻿


function isNumber(evt) {
	evt = (evt) ? evt : window.event;
	var charCode = (evt.which) ? evt.which : evt.keyCode;
	if (charCode > 31 && (charCode < 48 || charCode > 57)) {
		return false;
	}
	return true;
}

$("form").attr('autocomplete', 'off');



function ShowLoadingDialog() {
    document.getElementById("dvMaskFullpage").style.display = '';
}
function CloseLoadingDialog() {

    document.getElementById("dvMaskFullpage").style.display = 'none';
}
function MsgClose() {

    $('#MsgDiv').hide();

}

function DatatableScripts(TableID)
{
    var handleDataTableButtons = function () {
        if ($("#" + TableID+"").length) {
            $("#" + TableID+"").DataTable({
                dom: "Bfrtip",
                lengthMenu: [
                    [10, 25, 50, -1],
                    ['10 rows', '25 rows', '50 rows', 'Show all']
                ],
                buttons: [

                    {
                        extend: "pageLength",
                        className: "btn-sm"
                    }, {
                        extend: "csv",
                        className: "btn-sm"
                    }, {
                        extend: "excel",
                        className: "btn-sm"
                    }, {
                        extend: "pdf",
                        className: "btn-sm"
                    }, {
                        extend: "print",
                        className: "btn-sm"
                    }
                ],
                responsive: true
            });
        }
    };

    TableManageButtons = function () {
        "use strict";
        return {
            init: function () {
                handleDataTableButtons();
            }
        };
    }();


    TableManageButtons.init();
}

function ConfirmMsgBox(Title, Message, yesCallback) {
    swal({
        title: Title,
        text: Message,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) { yesCallback(); }
        else { }
    });
}

function inWords(t) {
    var i = t.split("."),
        r = i[0],
        u = i[1];
    return (r = r.toString()).length > 9 ? "overflow" : (n = ("000000000" + r).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/), !n) ? void 0 : (i = "", i += n[1] != 0 ? (a[Number(n[1])] || b[n[1][0]] + " " + a[n[1][1]]) + "Crore " : "", i += n[2] != 0 ? (a[Number(n[2])] || b[n[2][0]] + " " + a[n[2][1]]) + "Lakh " : "", i += n[3] != 0 ? (a[Number(n[3])] || b[n[3][0]] + " " + a[n[3][1]]) + "Thousand " : "", i += n[4] != 0 ? (a[Number(n[4])] || b[n[4][0]] + " " + a[n[4][1]]) + "Hundred " : "", i += n[5] != 0 ? (i != "" ? u == "00" ? "And " : "" : "") + (a[Number(n[5])] || b[n[5][0]] + " " + a[n[5][1]]) : "", u == "00" ? i + "Only " : (n = ("000000000" + u).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/), !n) ? void 0 : (i += n[1] != 0 ? (a[Number(n[1])] || b[n[1][0]] + " " + a[n[1][1]]) + "Crore " : "", i += n[2] != 0 ? (a[Number(n[2])] || b[n[2][0]] + " " + a[n[2][1]]) + "Lakh " : "", i += n[3] != 0 ? (a[Number(n[3])] || b[n[3][0]] + " " + a[n[3][1]]) + "Thousand " : "", i += n[4] != 0 ? (a[Number(n[4])] || b[n[4][0]] + " " + a[n[4][1]]) + "Hundred " : "", i + (n[5] != 0 ? (i != "" ? "And " : "") + (a[Number(n[5])] || b[n[5][0]] + " " + a[n[5][1]]) + " Paise Only " : "")))
}



function EncryptQueryStringJSON(value) {

    var data = $.parseJSON($.ajax({
        url: '/CommonAjax/EncryptQueryStringJSON',
        dataType: "json",
        data: { Value: value },
        async: false
    }).responseText);
    return data;

}
function DecryptQueryStringJSON(value) {

    var data = $.parseJSON($.ajax({
        url: '/CommonAjax/DecryptQueryStringJSON',
        dataType: "json",
        data: { Value: value },
        async: false
    }).responseText);
    return data;

}
