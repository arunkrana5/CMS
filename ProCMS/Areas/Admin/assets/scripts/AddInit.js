
$(document).ready(function () {
    $(".applyselect").select2();
});


function CheckDuplicateJson(Type, ID, Obj) {
    var Value = $(Obj).val();
    if (Value) {
        $.ajax({
            url: "/CommonAjax/fnCheckDuplicate",
            type: "Get",
            async: true,
            data: { sType: Type, lId: ID, sFieldValue: Value },
            success: function (data) {

                if (data.Status) {

                    $(Obj).css('border', '2px solid red');
                    $(Obj).val('');
                    FailToaster(data.SuccessMessage);
                }
                else {

                    $(Obj).css('border', '2px solid Green');
                }
            },
            error: function (er) {
                console.log(er);
            }
        });
    }
}



