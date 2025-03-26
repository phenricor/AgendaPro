let TOAST_DIV = $(".toast");
let TOAST = new bootstrap.Toast(TOAST_DIV);
$(document).ready(function () {
    ReturnBagMessageAsToast();
    GetCustomerTable();
    ShowCreateModal();
});
function ReturnBagMessageAsToast()
{
    if (viewBagMessage != null)
    {
        ToastMessage(`${viewBagMessage}`);// Variable set in index.cshtml
    }
}
function GetCustomerTable()
{
    let teste = 
    $.ajax({
        method: "get",
        url: "Customers/LoadCustomersTable/",
        data: {
            pageSize: 5,
            pageIndex: viewBagPageIndex // Variable set in index.cshtml
        },
        success: function (data) {
            $("#customersTable").html(data);
            ShowEditModal();
        }
    })
}
function ShowCreateModal()
{
    $("#buttonShowCreateModal").on("click", function () {
        $.ajax({
            method: "get",
            url: "Customers/Create/",
            success: function (data) {
                $("#customerCreateModal").html(data);
                var modal = new bootstrap.Modal("#createModal");
                modal.show();
                SendCreateRequest(modal);
            }
        })
    })
}

function ShowEditModal()
{
    $(".edit-button").on("click", function () {
        let id = $(this).attr("data-id");
        $.ajax({
            method: "get",
            url: "Customers/Edit/",
            data: {
                id: id,
            },
            success: function (data) {
                $("#customerCreateModal").html(data);
                let modal = new bootstrap.Modal("#createModal");
                modal.show();
                SendUpdateRequest(modal);
            }
        })
    })
}
function SendCreateRequest(modal)
{
    const FORM = $("#createCustomerForm");
    FORM.submit(function (event) {
        event.preventDefault();
        if ($(this).valid())
        {
            $.ajax({
                method: "post",
                url: "Customers/Create/",
                contentType: "application/json",
                data: JSON.stringify({
                    name: $("#name").val(),
                    email: $("#email").val(),
                    cnpj: $("#cnpj").val(),
                    phone: $("#phone").val()
                }),
                success: function (response) {
                    if (response.success) {
                        modal.hide();
                        FORM[0].reset();
                        GetCustomerTable();
                        ToastMessage("Customer created successfully.")
                    } else {
                        ShowErrorInRespectiveField(response);
                    }
                },
                error: function () {
                    ToastMessage("An error ocurred.");
                }
            })
        }
    })
}
function SendUpdateRequest(modal)
{
    const FORM = $("#editCustomerForm");
    FORM.submit(function (event) {
        event.preventDefault();
        if ($(this).valid())
        {
            $.ajax({
                method: "post",
                url: "Customers/Edit/",
                contentType: "application/json",
                data: JSON.stringify({
                    id: $("#customerId").val(),
                    name: $("#name").val(),
                    email: $("#email").val(),
                    cnpj: $("#cnpj").val(),
                    phone: $("#phone").val()
                }),
                success: function (response) {
                    if (response.success) {
                        modal.hide();
                        FORM[0].reset();
                        GetCustomerTable();
                        ToastMessage("Customer updated successfully.")
                    } else {
                        ShowErrorInRespectiveField(response);
                    }
                },
                error: function () {
                    ToastMessage("An error ocurred.");
                }
            })
        }
    })
}
function ShowErrorInRespectiveField(response)
{
    if (response.field != null)
    {
        let field = response.field + "Validation"
        return $("#"+field).html(response.error);
    }
    return ToastMessage(response.error);
}
function ToastMessage(message)
{
    $(".toast-body").html(message);
    TOAST.hide();
    TOAST.show();
}