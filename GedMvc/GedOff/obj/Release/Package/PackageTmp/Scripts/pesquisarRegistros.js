/// <reference path="jquery-2.1.1.min.js" />
/// <reference path="default.js" />

var jq = jQuery.noConflict();
jq(document).ready(function () {

    jq(jq('#menuNavegar ul li')[2]).hide();
    jq(jq('#menuNavegar ul li')[3]).hide();

    try {
        myApp.commom.dataTableIni('#tblreg');
    } catch (e) {
        alert(e.message);
    }

    //jq('table tbody tr').each(function (i, v) {
       //jq(v).find('a').addClass('btn btn-primary');
       // jq(jq(v).find('td')[2]).css('background-color', 'white').css('color', 'black').find('a').css('color', 'black');
    //});

});

//var jq = jQuery.noConflict();
//jq(document).ready(function(){

//});