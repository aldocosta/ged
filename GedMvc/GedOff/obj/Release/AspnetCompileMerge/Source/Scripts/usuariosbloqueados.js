/// <reference path="default.js" />
/// <reference path="jquery-2.1.1.min.js" />
var jq = jQuery.noConflict();
jq(document).ready(function () {
    myApp.commom.dataTableIni('#tblusuarios');
    jq('#tblusuarios a').on('click', function () {
        var dados = {
            id: jq(this).parent().next().text().trim(),
            usuario: jq(this).parent().next().next().html(),
            login: jq(this).parent().next().next().next().html(),
            email: jq(this).parent().next().next().next().next().html()
        }

        jq('#dadosid').val(dados.id);
        jq('#dadosnome').text(dados.usuario);

        jq('#pnlRecuperar').modal();
    });
});