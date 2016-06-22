$(document).ready(function () {
    $(".dateInput").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true,
        maxDate: 'd',
        yearRange:'-100:+0'
    });
});