let calendar;
LoadCalendar();
$(document).ready(function() {
    GetScheduleForm();
    RefreshCalendarOnCustomerSelect();
})
function RefreshCalendarOnCustomerSelect()
{
    $(".form-select").on("change", function (){
        GetAvailableBlocks();
    })
    GetAvailableBlocks();
}
function GetScheduleForm()
{
    $.ajax({
        method: "get",
        url: "GetScheduleForm/",
        success: function (data){
            $("#formDiv").html(data)
            $("#scheduleForm").submit(function (event) {
                event.preventDefault();
                if ($(this).valid())
                {
                    SendScheduleRequest();
                }
            })
        }
    })
}
function SendScheduleRequest()
{
    let requestData = {
        id: $(".form-select option:selected").attr("data-id"),
        date: new Date($("#date").val()).toISOString(),
        startTime: $("#startTime").val(),
        endTime: $("#endTime").val()
    }
    $.ajax({
        method: "post",
        url: "Scheduler/",
        contentType: "application/json",
        data: JSON.stringify(requestData),
        success: function (response) {
            if (!response.success){
                ToastMessage(response.error);
                return;
            }
            GetAvailableBlocks();
            ToastMessage("Successfully scheduled!")
        }
    })
}
function GetAvailableBlocks()
{
    $.ajax({
        method: "get",
        url: "GetAvailableBlocks/",
        data: {
            customerId: $(".form-select option:selected").attr("data-id"),
        },
        success: function (data){
            let blocksEvents = data;
            calendar.removeAllEvents();
            calendar.addEventSource(blocksEvents);
        }
    })
}
function ToastMessage(message)
{
    const TOAST_DIV = $(".toast");
    const TOAST = new bootstrap.Toast(TOAST_DIV);
    TOAST.hide();
    $(".toast-body").html(message);
    TOAST.show();
}
function LoadCalendar()
{
    document.addEventListener('DOMContentLoaded', function() {
        var calendarEl = document.getElementById('calendar');
        calendar = new FullCalendar.Calendar(calendarEl, {
            timeZone: 'UTC',
            nowIndicator: true,
            initialView: 'timeGridWeek',
            slotMinTime: '08:00:00',
            slotMaxTime: '18:00:00',
            headerToolbar: {
                left: 'prev,next',
                center: 'title',
                right: 'timeGridWeek,timeGridDay'
            },
            selectable: true,
        });
        calendar.render();
    });
}
