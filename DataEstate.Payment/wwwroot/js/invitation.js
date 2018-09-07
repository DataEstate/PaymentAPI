"use strict";

(function($) {
    var pubkey = $("#pubkey").val();
    var stripe = Stripe(pubkey);
    var elements = stripe.elements();
    var style = {
        base: {
            fontFamily: "Helvetica Neue"
        }
    };

    //Form Validation
    var validator = $("#frm-subscription").validate();
    function validateForm() {
        return validator.form();
    }

    var card = elements.create('card', {style:style});
    card.mount("#stripe-card");
    card.on("change", function(ev) {
        
        if (ev.error) {
            $("#stripe-error").text(ev.error.message);
            $("#stripe-error").removeClass("hide");
        }
        else {
            $("#stripe-error").addClass("hide");
        }
    });
    var cardToken;

    $("#payment-submit").click(function(ev) {
        if (validateForm()) {
            var cardData;
            if ($("#card-holder").val() != null) {
                cardData = {
                    name: $("#card-holder").val()
                };
                if ($("#card-country").val() != null) {
                    cardData.address_country= $("#card-country").val();
                }
            }
            stripe.createToken(card, cardData).then(function(result) {
                if (result.error !== undefined) {
                    console.log(result.error);
                    $("#stripe-error").text(result.error.message);
                    $("#stripe-error").removeClass("hide");
                }
                else if (result.token !== undefined && result.token.card !== undefined) {
                    $("#stripe-error").addClass("hide");
                    cardToken = result.token;
                    var cc = cardToken.card;
                    $("#customer-name").text($("#businessName").val());
                    $("#customer-email").text($("#email").val());
                    $("#customer-card-holder").text(cc.name);
                    $("#customer-card-number").text("**** **** **** "+cc.last4);
                    $("#customer-card-brand").text(cc.brand);
                    $("#customer-card-expiry").text(cc.exp_month + "/"+cc.exp_year);
                    if (cc.address_country != null) {
                        $("#customer-card-country").text(cc.address_country);
                    }
                    $(".modal-body").addClass("show");
                    $("#message-alert").modal("show");
                }
            });
        }

    });
    $("#pay-now").click(function(ev) {
        //Make payment
        ev.preventDefault();
        var form = $("#frm-subscription");
        form.append('<input type="hidden" value="'+cardToken.id+'" name="token">');
        console.log(form);
        form.submit();
    });
})(jQuery);
