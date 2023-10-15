(function ($) {
    "use strict"

    $('.datetimepicker-default').datetimepicker(
        {
            format: 'Y/m/d H:i',            
            mask: true,
            todayButton: true,


        }
    );

    $.datetimepicker.setLocale('vi');

})(jQuery); 