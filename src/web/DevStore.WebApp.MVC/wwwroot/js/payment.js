
function CreditCard() {

    if ($("#card_number").length > 0) {
        $("#card_number").validateCreditCard(function (e) {

            $(this).removeClass();
            $(this).addClass("form-control");

            if (e.card_type !== null)
                $(this).addClass(e.card_type.name);

            if (e.valid)
                $(this).addClass("valid");
            else
                $(this).removeClass("valid");

            if (e.length_valid && !e.valid) {
                $(this).addClass("invalid");
            } else {
                $(this).removeClass("invalid");
            }
        });
    }
}


$(document).ready(function () {
    CreditCard();
    $('#card_number').mask('0000 0000 0000 0000');
    $('#card_expiration').mask('00/00');
    $('#card_cvv').mask('0000');
});
