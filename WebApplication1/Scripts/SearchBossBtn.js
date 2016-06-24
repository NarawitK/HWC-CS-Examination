$(document).ready(function () {
    $("#SearchBtn").click(function () {
        $.ajax({
            url:"/HR_Employee/GetBossName",
            datatype: "JSON",
            data:{term:$("#InputBossID").val()},
            type:"GET",
            success: function (result) {
                if (result.length > 0) {
                    var fullName = result[0].label;
                    var bID = result[0].value;
                    if (bID !== $("#EmployeeID").val()) {
                        $("#InputBossID").val(fullName);
                        $("#BossID").val(bID);
                    }
                    else {
                        $("#InputBossID").val("");
                        $("#InputBossID").prop("placeholder","ไม่สามารถเลือกหัวหน้างานเป็นตัวเองได้")
                    }
                }
                else {
                    $("#InputBossID").val("");
                    $("#InputBossID").prop("placeholder", "ไม่พบข้อมูลหัวหน้างาน")
                    $("#BossID").val(null);
                }
            },//success fn
            error: function () {
                console.log("Error Function: " + result);
                $("#InputBossID").val("");
                $("#InputBossID").prop("placeholder","มีข้อผิดพลาดเกิดขึ้นในระบบ")
                $("#BossID").val(null);
                return false;
            }//err fn
        });
    });
});