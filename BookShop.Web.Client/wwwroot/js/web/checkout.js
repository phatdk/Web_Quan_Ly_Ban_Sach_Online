(function ($) {
    var processCheckout = function () {
        $('#btn-submit-order').on('click', function (e) {
            e.preventDefault();
            var data = getFormDataJson('frmSubmitCheckout');
            var check = validationForm('frmSubmitCheckout');
            data.PaymentMethod = $('select option:selected').val() 

            if (check) {
                coreAjax(
                    check
                    , '/Payment/ProcessCheckout'
                    , JSON.stringify(data)
                    , 'POST'
                    , function (res) {
                        localStorage.setItem('orderInfo', JSON.stringify(data))
                        setTimeout(function () {
                            window.location.href = res;
                        }, 500)
                    }
                    , function () { }
                );
            }
        });
    }
    //Load functions
    $(document).ready(function () {
        processCheckout();
    });
})(jQuery);