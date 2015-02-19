$(document).ready(function () {
    if ($(".icon-ok").length) {
        $(".icon-ok").prepend("<div class='close-button'>X</div>");
        $(".icon-ok .close-button").click(function () {
            $(this).parent("div").fadeOut(300);
        });
    }
});