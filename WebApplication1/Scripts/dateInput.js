$(document).ready(function () {
    $(".dateInput").datepicker({
        isBE:true,
        autoConversionField:false,
        formatDate: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        maxDate: '0',
        yearRange:'-100:+0'
    });
});