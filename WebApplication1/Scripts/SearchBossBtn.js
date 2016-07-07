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
                //Case Boss is not self
                if (bID !== $("#EmployeeID").val()) {
                    $("#InputBossID").val(fullName);
                    $("#BossID").val(bID);
                }
                    //Case Boss is self
                else {
                    $("#InputBossID").val("");
                    $("#InputBossID").prop("placeholder", "เลือกตัวเองไม่ได้")
                }
            }
                //Case Not Found
            else {
                $("#InputBossID").val("");
                $("#InputBossID").prop("placeholder", "ไม่พบข้อมูลหัวหน้างาน")
                $("#BossID").val(null);
            }
        },//end Success Fn
        error: function () {
            console.log("Error Function: " + result);
            $("#InputBossID").val("");
            $("#InputBossID").prop("placeholder", "มีข้อผิดพลาดเกิดขึ้นในระบบ")
            $("#BossID").val(null);
            return false;
        }//end Err Fn
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