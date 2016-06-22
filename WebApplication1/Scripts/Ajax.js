$(document).ready(function () {
    $("#submit-btn").click(function () {
        $("#showBoxVal").val($("#searchbox").val())
        $.ajax({
            url: 'AjaxController/SearchPeople',
            type: 'GET',
            data: { 'keyword': $("#searchbox").val() },
            cache: false,
            success: function (result) {
                $("#searchbox").val(result);
                $("#showQV").html(result);
            },
            complete: function (result) {
                $("#searchbox").val(result);
                $("#showQV").text(result);
            },
            error: function (result) {
                $("#searchbox").val(result);
                $("#showQV").text(result);
            }
        });
    });
});


