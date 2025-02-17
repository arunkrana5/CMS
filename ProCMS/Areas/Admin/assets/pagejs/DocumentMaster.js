$(".btnShowGallery").bind("click", function () {
    DocumentList(0);
});

$("#ddDoctype").bind("change", function (e) {
    var ID = $(this).find("option:selected").val();
    DocumentList(ID);
});

function DocumentList(DocTypeID) {
    $.ajax({
        url: "/Admin/Index/_DocumentList",
        type: "Post",
        data: { DocTypeID: DocTypeID },
        success: function (data) {
            $("#TagetDiv_Doc").empty();
            $("#TagetDiv_Doc").html(data);
            $("#ViewModal_Doc").modal({
                backdrop: 'static',
                keyboard: false
            });
            $(".DeleteDocument").on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                DeleteDocument(this);
            });
            $(".rdosingleselection").on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                FillDoc(this);
            });
            $(".DocURLCopy").on('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                DocURLCopy(this);
            });
        },
        error: function (er) {
            console.log(er);
            CloseLoadingDialog();

        }
    });
}

$(".btnAddDocument").bind("click", function () {
    AddDocument();
});

function AddDocument() {
    $.ajax({
        url: "/Admin/Index/_AddDocument",
        type: "get",
        success: function (data) {
            $("#TagetDiv_AddDoc").empty();
            $("#TagetDiv_AddDoc").html(data);
            $("#AddModal_Doc").modal({
                backdrop: 'static',
                keyboard: false
            });
            $(".applyselect").select2();

            var form = $("#_AddDocumentForm")
                .removeData("validator")
                .removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
           

        },
        error: function (er) {
            console.log(er);
            CloseLoadingDialog();

        }
    });
}

function OnAddDocSuccess(obj) {
    CloseLoadingDialog();
    $("#AddModal_Doc").modal('hide');
    if (obj.Status) {
        SuccessToaster(obj.SuccessMessage);

        $("#ddDoctype").val(obj.OtherID).trigger('change');
        
    }
    else {
        FailToaster(obj.SuccessMessage);
    }

}

function SearchDocument() {
    // Declare variables
    var input, filter, ul, li, a, i, txtValue;
    input = document.getElementById('MySearchForDocument');
    filter = input.value.toUpperCase();
    ul = document.getElementById("documentul");
    li = ul.getElementsByTagName('li');

    // Loop through all list items, and hide those who don't match the search query
    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("a")[0];

        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}


function DeleteDocument(obj) {
    var ID = $(obj).closest("li").find("input:hidden[name=hdnDocID]").val();
    var DocumentName = $(obj).closest("li").find("input:hidden[name=hdnDocName]").val();
    var DocTypeID = $(obj).closest("li").find("input:hidden[name=hdnDocTypeID]").val();

    ConfirmMsgBox('Do you really Want to Delete ' + DocumentName + '?', '', function () {
        $.ajax({
            url: "/Admin/CommonAjax/DeleteDocumentJSON",
            type: "Post",
            async: true,
            data: { DocID: ID },
            success: function (data) {
                if (data) {
                    DocumentList(DocTypeID);
                }
                else {
                    FailToaster('Can not delete right Now');
                }
            },
            error: function (er) {
                alert(er);
            }
        });
    });
}



function DocURLCopy(obj) {
   
    var copyText = $(obj).closest("li").find("input:text[name=txtURLCopy]");
    copyText.select();
    //copyText.setSelectionRange(0, 99999); 
    document.execCommand("copy");
    SuccessToaster('Copied Successfully');
}

function FillDoc(obj) {
    debugger;
    var count = Number($("#MultipleCount").val());
    var hdnDocID = $(obj).closest("li").find("input:hidden[name=hdnDocID]").val();
    var hdnURL = $(obj).closest("li").find("input:text[name=txtURLCopy]").val();
    $(".hdnselectedDoc_" + count).val(hdnDocID);
    $(".ImgselectedURL_" + count).attr('src', hdnURL);
    $('.modal').modal('hide');
}