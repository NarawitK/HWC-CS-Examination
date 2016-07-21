/*First Ajax Call - CheckBossJunction*/
function callAJAX() {
    var inputBossID = $("#InputBossID").val();
    var editor = $("#EmployeeID").val();
    $.ajax({
        url: "/HR_Employee/CheckEmpCrossBoss",
        datatype: "JSON",
        data: { EmpEditor: editor, BossIDInput: inputBossID },
        type: "GET",
        success: function (result) {
            if (result.length > 0) {
                $("#InputBossID").val(null);
                $("#BossID").val(null);
                alert("คุณเป็นหัวหน้างานของพนักงานคนนี้")
                //$("#InputBossID").prop("placeholder", "คุณเป็นหัวหน้าของพนักงานคนนี้")
                //setTimeout(function () { $("#InputBossID").prop("placeholder", "กรอกรหัสหัวหน้างาน") }, 1000);
            }
            else if (result.length <= 0) {
                $.ajax({
                    url: "/HR_Employee/GetBossName",
                    datatype: "JSON",
                    data: { term: inputBossID },
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
                                if (bID == $("#EmployeeID").val()) {
                                    $("#InputBossID").val(null);
                                    $("#InputBossID").prop("placeholder", "เลือกตัวเองไม่ได้")
                                    alert("ไม่สามารถเลือกตัวเองเป็นหัวหน้างานของตัวเองได้");
                                    setTimeout(function () { $("#InputBossID").prop("placeholder", "กรอกรหัสหัวหน้างาน") }, 1000);
                                    //$("#InputBossID").prop("placeholder", "กรอกรหัสหัวหน้างาน");
                                }
                                    
                 

                        } //End Success if
                            //Case Not Found
                        else {
                            $("#InputBossID").val(null);
                            $("#InputBossID").prop("placeholder", "ไม่พบข้อมูลหัวหน้างาน")
                            $("#BossID").val(null);
                            setTimeout(function () { $("#InputBossID").prop("placeholder", "กรอกรหัสหัวหน้างาน") }, 1000);
                        }
                    },//End Case AJAX Success
                    error: function () {
                        console.log("Error Function: " + result);
                        $("#InputBossID").val(null);
                        alert("มีข้อผิดพลาดเกิดขึ้นในระบบ");
                        $("#InputBossID").prop("placeholder", "มีข้อผิดพลาดเกิดขึ้นในระบบ")
                        $("#BossID").val(null);
                        return false;
                    }//End Case AJAX Error
                });
            }
        },
        error: function (result) {
            $("#InputBossID").prop("placeholder", "เกิดข้อผิดพลาดขึ้น");
        }
    });
}


$(document).ready(function () {
    $("#InputBossID").keypress(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                callAJAX();
            }
    });
    //Click to search
    $("#SearchBtn").click(function () {
        callAJAX();
    });
});