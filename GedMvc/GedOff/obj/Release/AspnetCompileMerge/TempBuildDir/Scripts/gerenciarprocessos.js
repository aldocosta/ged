/// <reference path="jquery-2.1.1.js" />
/// <reference path="bootstrapPager.min.js" />
/// <reference path="default.js" />


var jq = jQuery.noConflict();
jq(document).ready(function () {

    //jq(jq('#menuNavegar ul li')[0]).hide();
    //jq(jq('#menuNavegar ul li')[1]).hide();
    jq(jq('#menuNavegar ul li')[2]).hide();
    jq(jq('#menuNavegar ul li')[3]).hide();

    //exibir mensagens
    var rq = myApp.commom.getParameterByName("erro");
    if (rq != "") {
        alert(rq);
    }

    //iniciando o plugin do datatable
    myApp.commom.dataTableIni('#tblarquivos');

    jq('.descricao').mouseover(function (e) {
        var pos = { x: e.pageX, y: e.pageY };
        myApp.alertLocal(jq(this).next().val(), myApp.commom.time, pos, jq(this));
        //alert(pos.x + '-' + pos.y);
    });

    jq('.delclass a').on('click', function (e) {
        if (!confirm('Deseja realmente remover este arquivo?')) {
            return false;
        }
    });


});