(function($) {

    if (typeof (window.ValidatorUpdateDisplay) === "undefined" || window.ValidatorUpdateDisplay == null) {
        console.warn("The ValidatorUpdateDisplay() method isn't defined. Can't inject custom validation.");
    } else {

        var aspUpdateDisplay = window.ValidatorUpdateDisplay;

        window.ValidatorUpdateDisplay = function (n) {
            var $controlToValidate = $(document.getElementById(n.controltovalidate));
            if (n.isvalid) {
                $controlToValidate.removeClass("invalid");
            } else {
                $controlToValidate.addClass("invalid");
            }
            // Call the original ValidatorUpdateDisplay()-method.
            aspUpdateDisplay(n);
        };
    }

})(jQuery);