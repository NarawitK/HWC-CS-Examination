$(document).ready(function () {
    $("#BossID").autocomplete({
        source: function (request, response) {
           $.ajax({
                url: '/AutoComplete/GetBossName',
                type: 'POST',
                dataType: "json",
                data: { keyword: $("#BossID").val() },
                success: function (result) {
                    response(result.d);
                    console.log(result);
                    return result; 
                },
                error: function (result) {
                    return ("ไม่พบข้อมูล");
                }
            });
        }
    });
    $("#BossID").change(function () {
        $.ajax({
            url: '/AutoComplete/GetBossName',
            type: 'POST',
            dataType: "json",
            data: { keyword: $("#BossID").val() },
            success: function (result) {
                console.log(result);
            }
        });
    });
});