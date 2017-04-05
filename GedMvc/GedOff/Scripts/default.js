(function () {
    this.myApp = this.myApp || {};
    var ns = this.myApp;

    ns.commom = ns.commom || {};
    ns.commom.time = { 'in': 100, 'out': 5000 }
    ///'Erro desconhecido, contate o administrador!'
    ns.commom.unknowerro = 'Erro desconhecido, contate o administrador!';

    ///Retorna uma querystring
    ns.commom.getParameterByName = function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    ///Inicia o plugin do datatable em portugues
    ns.commom.dataTableIni = function (name) {
        jq(name).DataTable({
            "language": {
                "sSearch": "Pesquisar",
                "sLengthMenu": "Mostrar _MENU_",
                "infoFiltered": " - Filtrando de _MAX_ registros",
                "paginate": {
                    "next": "Próxima",
                    "previous": "Anterior",
                    "emptyTable": "Sem registros",
                    "infoFiltered": " - Filtrando _MAX_ registros",
                },
                "info": "Pagina _PAGE_ de _PAGES_"
            }
        });
    }

    ns.commom.abrirPasta = function (val) {
        var id = jq(val).next().next().next().text().trim().replace('#', '');
        window.location.href = myApp.retornarAppName + "/PastaDepto/GerenciarPastasDaPasta?idpasta=" + id;
    }

    ns.commom.abrirProcesso = function (val) {
        var id = jq(val).next().next().next().text().trim().replace('#', '');
        window.location.href = myApp.retornarAppName + "/PastaDepto/GerenciarProcessos?idprocesso=" + id;
    }

    ///Abri uma pasta e faz um sitemape
    ns.commom.abrirPastaMapeando = function (val) {
        var id = jq(val).next().next().next().text().trim().replace('#', '');
        var pasta = jq('#spnpastanome').text().trim();

        //no caso, é o idpasta da url atual
        var idpastaanterior = myApp.commom.getParameterByName('idpasta');

        //aqui é uma nova url
        window.location.href = myApp.retornarAppName + "/PastaDepto/GerenciarPastasDaPasta?idpasta=" + id + '&pasta=' + pasta + '&idpastaanterior=' + idpastaanterior;
    }

    //ns.retornarAppName = "";
    ns.retornarAppName = "/ged";

    ///alerta personalizado no canto direito da tela
    ns.alert = function (msg, time) {
        var div = document.createElement('div');
        var innerdiv = document.createElement('div');
        innerdiv.innerHTML = msg.toUpperCase();

        div.appendChild(innerdiv);

        jq(div).css('width', '210px').css('height', '70px').css('background-color', 'black').css('color', 'white').css('top', '10%').css('right', '2%').
        css('position', 'absolute').css('text-align', 'center').css('border', '1px solid #FFFF66').animate({
            'right': '1%'
        }, time.in).fadeOut(time.out);
        jq(innerdiv).css('width', '210px').css('height', '70px').css('display', 'table-cell').css('vertical-align', 'middle');
        document.body.appendChild(div);
    };

    ns.alertLocal = function (msg, time, pos, jqobj) {
        var div = document.createElement('div');
        var innerdiv = document.createElement('div');
        innerdiv.innerHTML = msg.toUpperCase();

        div.appendChild(innerdiv);

        jq(div).attr('id', 'divdesc');
        var obj = jq(div);
        if (jq('#divdesc').is(':visible') == false) {

            jq(div).css('background-color', 'black').css('color', 'white').css('top', pos.y).css('right', '2%').
                css('position', 'absolute').css('text-align', 'center').css('border', '1px solid #FFFF66').animate({
                    'right': '1%'
                }, time.in);

            jq(innerdiv).css('width', '210px').css('height', '70px').css('display', 'table-cell').css('vertical-align', 'middle');
            document.body.appendChild(div);
        }

        jq(jqobj).mouseout(function () {
            jq(obj).hide();
            //var interval = setInterval(function () {                
            //    clearInterval(interval);
            //}, myApp.commom.time.out);
        });

    };


    ///obj.action 
    ///obj.callback
    ///obj.callbackerro
    ///obj.jsondata (opcional)
    ns.doAjaxPost = function (obj) {
        jq.ajax({
            type: 'POST',
            url: obj.action,
            dataType: 'json',
            data: JSON.stringify(obj.jsondata),
            contentType: 'application/json; charset=utf-8',
            success: obj.callback,
            error: obj.callbackerro
        });
    }

    ///Processa o retorno de um ajax request
    ns.processData = function (data, success, error) {
        if (data.msg != null) {
            if ((data.msg.toString().indexOf("Login Expirado") > -1)) {
                window.location.href = myApp.retornarAppName + "/home/login?msg=" + data.msg;
                return;
            } else {
                error(data);
                //alert(data.msg);
            }
        } else if (data.error == undefined) {
            success();
        }
        else {
            //alert("Atenção, ocorreu um erro: " + data.error);
            error();
        }
    }

    ///Namespace pra manipular o dom 
    ns.doomer = ns.doomer || {}
    var doomer = ns.doomer;

    ///cria um elemento com html com id(opcional) e conteudo(opcional)
    doomer.createElement = function (id, type, content) {
        var temp = document.createElement(type);
        if (id != '' || id != null || id != undefined)
            temp.id = id;

        if (content != '' || content != null || content != undefined)
            temp.innerHTML = content;

        return temp;
    }

})();