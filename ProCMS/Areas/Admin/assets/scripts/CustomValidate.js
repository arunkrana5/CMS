


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



function EncryptQueryStringJSON(value) {

    var data = $.parseJSON($.ajax({
        url: '/Admin/CommonAjax/EncryptQueryStringJSON',
        dataType: "json",
        data: { Value: value },
        async: false
    }).responseText);
    return data;

}
function DecryptQueryStringJSON(value) {

    var data = $.parseJSON($.ajax({
        url: '/Admin/CommonAjax/DecryptQueryStringJSON',
        dataType: "json",
        data: { Value: value },
        async: false
    }).responseText);
    return data;

}
