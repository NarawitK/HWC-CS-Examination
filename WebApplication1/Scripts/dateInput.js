$(document).ready(function () {
    $(".dateInput").datepicker({
        isBE:true,
        autoConversionField:false,
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        maxDate: '+0d',
        yearRange: '-100:+0',
        hideIfNoPrevNext:true
    });
});