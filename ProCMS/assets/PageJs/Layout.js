function GetContactUsForm(Type) {
	$.ajax({
		url: "/Home/ContactUs/" + Type,
		type: "Get",
		async: true,
		success: function (data) {
			$("#ContactusForm").empty();
			$("#ContactusForm").html(data);

		},
		error: function (er) {
			console.log(er);
		}
	});
}

function BindContactTarget(obj) {
	if (obj.Status) {
		SuccessToaster(obj.SuccessMessage);
		$("#_ContactUsForm").trigger('reset');

	}
	else {

		FailToaster(obj.SuccessMessage);
	}
}