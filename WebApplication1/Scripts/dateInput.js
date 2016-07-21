$(document).ready(function () {
    $(".dateInput").datepicker({
        isBE:true,
        autoConversionField:false,
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        yearRange: '1753:+0',
        maxDate: '+0',
        monthNamesShort: ["ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.",
                   "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.",
                   "ต.ค.", "พ.ย.", "ธ.ค."],
        hideIfNoPrevNext: true
    });
});