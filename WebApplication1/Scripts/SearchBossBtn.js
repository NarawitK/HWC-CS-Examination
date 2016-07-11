function callAJAX() {
    $.ajax({
        url: "/HR_Employee/GetBossName",
        datatype: "JSON",
        data: { term: $("#InputBossID").val() },
        type: "GET",
        success: function (result) {
            if (result.length > 0) {
                var fullName = result[0].label;
                var bID = result[0].value;
                //Case Boss is not themself
                if (bID !== $("#EmployeeID").val()) {
                    $("#InputBossID").val(fullName);
                    $("#BossID").val(bID);
                }
                    //Case Boss is themself
                else {
                    $("#InputBossID").val(null);
                    $("#InputBossID").prop("placeholder", "เลือกตัวเองไม่ได้")
                }
            }
                //Case Not Found
            else {
                $("#InputBossID").val(null);
                $("#InputBossID").prop("placeholder", "ไม่พบข้อมูลหัวหน้างาน")
                $("#BossID").val(null);
            }
        },//End Case AJAX Success
        error: function () {
            console.log("Error Function: " + result);
            $("#InputBossID").val(null);
            $("#InputBossID").prop("placeholder", "มีข้อผิดพลาดเกิดขึ้นในระบบ")
            $("#BossID").val(null);
            return false;
        }//End Case AJAX Error
    });
}
$(document).ready(function () {
    //Press Enter to Search
    $("#InputBossID").focus(function () { 
        $(this).keypress(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                callAJAX();
            }
        });
    });
    //Click to search
    $("#SearchBtn").click(function () {
        callAJAX();
    });

});