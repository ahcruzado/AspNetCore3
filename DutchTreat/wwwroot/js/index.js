$(document).ready(function() {    
    console.log("Hello Pluralsight");

    //var theForm = document.getElementById("theForm");
    //theForm.hidden = true;
    var theForm = jQuery("#theForm");
    theForm.hide();

    //var button = document.getElementById("buyButton");
    //button.addEventListener("click", function () {
    //    alert("Buying Item");
    //});

    var button = $("#buyButton");
    button.on("click", function () {
        alert("Buying Item");
    });

    var productInfo = $(".product-props li");
    productInfo.on("click", function () {
        console.log("You clicked on " + $(this).text());
    });

    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", function () {
        //$popupForm.toggle();
        //$popupForm.toggle(1000);
        $popupForm.fadeToggle(1000);
    });
});