/// <reference path="jquery-2.1.1.min.js" />
/// <reference path="default.js" />

var jq = jQuery.noConflict();
jq(document).ready(function () {
    jq('.deptos').on('click', function () {
        window.self.location.href = myApp.retornarAppName+ '/pastadepto/GerenciarPastas?iddepto=' + jq(this).data("iddepto");
    });
});
