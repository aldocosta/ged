/// <reference path="jquery-2.1.1.min.js" />
/// <reference path="default.js" />
var jq = jQuery.noConflict();
jq(document).ready(function () {
    var alterable = null;

    dataTableIt();
    deptoClick();

    jq('#tblpastas tbody tr').each(function (i, e) {

        //evento de apagar a pasta
        jq(jq(e).find('td')[2]).on('click', function () {
            apagarPasta(this);
        });

        //evento de alterar a pasta
        jq(jq(e).find('td')[1]).on('click', function () {
            editarPasta(this);
        });

        //evento de abrir a pasta
        jq(jq(e).find('td')[0]).on('click', function () {
            abrir(this);
        });

    });

    jq('#addPasta').on('click', function () {
        clearAll();
        jq('#modalNovo').modal({ keyboard: true });
        jq('.editavel').removeClass('editavel');
        jq('#tblpastas tbody tr').each(function (i, e) {

        })

    });

    jq('#btnGravar').on('click', function (e) {
        var data = {
            NmPasta: jq('#NmPasta').val(),
            DsPasta: jq('#DsPasta').val(),
            //iddepto: myApp.commom.getParameterByName('iddepto'),
            idpastaform: jq('#idpasta').val()
        };

        myApp.doAjaxPost(
            {
                action: myApp.retornarAppName + '/PastaDepto/GravarPasta',
                callback: function (data) {
                    myApp.processData(data, function () {
                        if (data.idpastaform != null || data.idpastaform != '' || data.idpastaform != undefined) {
                            jq('#tblpastas').DataTable().destroy();
                            jq('.editavel').remove();

                            var tdabrir = myApp.doomer.createElement(null, 'td', '<a href="#"><span class="glyphicon glyphicon-eye-open"></span> Abrir</a>')
                            var tdeditar = myApp.doomer.createElement(null, 'td', '<a href="#"><span class="glyphicon glyphicon-pencil"></span> Editar </a>')
                            var tddeletar = myApp.doomer.createElement(null, 'td', '<a href="#"><span class="glyphicon glyphicon-minus"></span> Deletar</a>')

                            var tdcodigo = myApp.doomer.createElement(null, 'td', data.Codigo);
                            var tdnome = myApp.doomer.createElement(null, 'td', data.NmPasta);
                            var tdds = myApp.doomer.createElement(null, 'td', data.DsPasta);
                            var tdowner = myApp.doomer.createElement(null, 'td', data.Owner.NmEntidade);

                            var tr = myApp.doomer.createElement(null, 'tr', '');

                            jq(tr).append(tdabrir).append(tdeditar).append(tddeletar).append(tdcodigo).append(tdnome).append(tdds).append(tdowner);

                            jq('#tblpastas tbody').append(tr);

                            myApp.commom.dataTableIni('#tblpastas');
                            jq(tddeletar).on('click', function (e) {
                                apagarPasta(this);
                            });

                            jq(tdeditar).on('click', function () {
                                editarPasta(this);
                            });

                            jq(tdabrir).on('click', function () {
                                abrir(this);
                            });

                            jq('#modalNovo').modal('hide');
                            myApp.alert('Pasta inserida com sucesso!', myApp.commom.time);
                        } else {
                            jq(alterable).find(td).eq(3).val(data.nmpasta);
                            jq(alterable).find(td).eq(4).val(data.dspasta);
                            alterable = null;
                            myApp.alert('Pasta atualizada com sucesso!', myApp.commom.time);
                        }
                    }, function () {
                        alert(data.error);
                    });
                },
                callbackerro: function () { },
                jsondata: data
            });
        e.preventDefault();
    });

    ///evento click
    function deptoClick() {
        jq(".deptos").on('click', function () {
            window.location.href = myApp.retornarAppName + "/PastaDepto/GerenciarProcessos?key=" + jq(this).data("key") + "&name=" + jq(this).text().trim() + "&pai=" + jq(this).data("pai");
        });
    }

    ///limpa form
    function clearAll() {
        jq('#NmPasta').val('');
        jq('#DsPasta').val('');
        jq('#idpasta').val('');
    }

    ///criar um datatable ordenado
    function dataTableIt() {
        jq('#tblpastas').DataTable({
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

    function apagarPasta(val) {
        if (confirm('Deseja realmente apagar esta pasta?')) {

            var id = { id: jq(val).next().text() };

            myApp.doAjaxPost({
                action: myApp.retornarAppName + '/PastaDepto/ApagarPasta',
                callback: function (data) {
                    myApp.processData(data, function () {
                        if (data.msgerro == undefined) {
                            jq('#tblpastas').DataTable().destroy();
                            jq(val).parent().remove();
                            dataTableIt();
                            myApp.alert('Pasta removida com sucesso.', myApp.commom.time);
                        } else {
                            alert(data.msgerro);
                        }
                    }, function () {
                        alert(data.error);
                    });
                },
                callbackerro: null,
                jsondata: id
            });
        }
    }

    function abrir(val) {
        //var id = jq(val).next().next().next().text();
        //window.location.href = "/PastaDepto/GerenciarPastasDaPasta?idpasta="+id;
        myApp.commom.abrirPasta(val);
    }

    function editarPasta(val) {
        clearAll();
        jq('#modalNovo').modal({ keyboard: true });

        alterable = jq(val);
        jq(val).parent().addClass('editavel');
        var pasta = {
            id: jq(val).next().next().text(),
            nmpasta: jq(val).next().next().next().text(),
            dspasta: jq(val).next().next().next().next().text()
        };

        jq('#NmPasta').val(pasta.nmpasta);
        jq('#DsPasta').val(pasta.dspasta);
        jq('#idpasta').val(pasta.id);
    }
});