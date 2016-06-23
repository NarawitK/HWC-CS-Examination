$(document).ready(function () {
    $("#SearchBtn").click(function () {
        $.ajax({
            url:"/HR_Employee/GetBossName",
            datatype: "JSON",
            data:{term:$("#InputBossID").val()},
            type:"GET",
            success: function (result) {
                console.log("Success Function: " + result);
                if (result.length > 0) {
                    var fullName = result[0].label;
                    var bID = result[0].value;
                    $("#InputBossID").val(fullName);
                    $("#BossID").val(bID);
                }
                else {
                    Console.log("Success Else");
                    $("#InputBossID").val("");
                    $("#InputBossID").prop("placeholder", "ไม่พบข้อมูลหัวหน้างาน")
                    $("#BossID").val(null);
                }
            },//success fn
            error: function () {
                console.log("Error Function: " + result);
                $("#InputBossID").val("");
                $("#InputBossID").prop("placeholder","ไม่พบข้อมูลหัวหน้างาน")
                $("#BossID").val(null);
            }//err fn
        });
    });
});