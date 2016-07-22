var openStatus = false;
function switcher() {
    if (openStatus == false) {
        openMenu();
        openStatus = true;
        console.log("Menu Opened:" + openStatus);
    }
    else {
        closeMenu();
        openStatus = false;
        console.log("Menu Closed:" + openStatus);
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