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

function successAlert(text) {
    $('#success-content').text(text);
    $('#alert-success').toggleClass('in out');

    setTimeout(function () {
        $('#alert-success').toggleClass('in out');
    }, 3000);
}

function errorAlert(text) {
    $('#danger-content').text(text);
    $('#alert-danger').toggleClass('in out');
}

function getQueryParam(name) {
    var url_string = window.location.href;
    var url = new URL(url_string);
    var c = url.searchParams.get(name);

    return c;
} 