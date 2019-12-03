// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// loading screen
var $loading = $('#processing-modal').modal('hide');

$(function () {
    
})
    .ajaxStart(function () {
        $loading.modal();

    })
    .ajaxStop(function () {
        $loading.modal('hide');
    });

/**
 * Shows success alert with text
 * @param {any} text - alert text
 */
function successAlert(text) {
    $('#success-content').text(text);
    $('#alert-success').toggleClass('in out');

    setTimeout(function () {
        $('#alert-success').toggleClass('in out');
    }, 3000);
}

/**
 * Shows error alert with text
 * @param {any} text - alert text
 */
function errorAlert(text) {
    $('#danger-content').text(text);
    $('#alert-danger').toggleClass('in out');
}

/**
 * Returns query string param value by key
 * @param {any} name - param name
 */
function getQueryParam(name) {
    var url_string = window.location.href;
    var url = new URL(url_string);
    var c = url.searchParams.get(name);

    return c;
} 

/**
 * Shows a confirmation modal with text and returns user's choice
 * @param {any} text - the text that should be shown in the confirmation modal
 * @param {any} callback - callback function that will handle user's choice
 */
function confirmDelete(text, callback) {
    // apply the text
    $("#confirmation-text").text(text);

    // show the modal
    $("#confirm-delete-modal").modal('show');

    // remove all handlers
    $("#confirm-delete-btn-yes").off();

    // add a new one to run callback
    $("#confirm-delete-btn-yes").on("click", function () {
        $("#confirm-delete-modal").modal('hide');
        callback();
    });
}