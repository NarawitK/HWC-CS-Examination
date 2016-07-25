var openStatus = false;
function switcher() {
    if (openStatus == false) {
        openMenu();
        openStatus = true;
    }
    else {
        closeMenu();
        openStatus = false;
    }
}
function openMenu() {
    $("#navigator").width(250);
    $("#navigator > ul").fadeIn("fast");
}
function closeMenu() {
    $("#navigator").width(0);
    $("#navigator > ul").fadeOut("fast");
}