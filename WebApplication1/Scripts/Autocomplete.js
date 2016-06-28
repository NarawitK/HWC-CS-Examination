$(document).ready(function () {
    $("#InputBossID").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/HR_Employee/GetBossName",
                type: "GET",
                datatype: "json",
                data: { term: request.term },
                success: function (data) {
                    var res = data;
                    var delay = 2750;
                    console.log("Success Function: " + res);
                    $("#InputBossID").prop("placeholder", "");
                    if (res.length <= 0) {
                        setTimeout(function () {
                            $("#InputBossID").val("");
                            $("#InputBossID").prop("placeholder", "ไม่พบข้อมูลหัวหน้างาน")
                            $("#BossID").val(null);
                        }, delay)
                        
                    }
                    else {
                        response(res);
                    }
                },
                error: function (result) {
                    $("#InputBossID").val("");
                    $("#InputBossID").prop("placeholder", "ไม่พบข้อมูลหัวหน้างาน");
                    $("#BossID").val(null);
                },
            });
        },
        select(event, ui) {
            $("#InputBossID").val(ui.item.label);
            $("#BossID").val(ui.item.value);
            return false;
        },
        autoFocus:true,
        delay:2.75
    });
});
