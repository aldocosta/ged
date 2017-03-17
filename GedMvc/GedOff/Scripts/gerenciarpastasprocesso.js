/// <reference path="jquery-2.1.1.min.js" />
/// <reference path="default.js" />
var jq = jQuery.noConflict();
jq(document).ready(function () {
    jq('#addPasta').on('click', function () {
        clearAll();
        jq('#modalNovaPasta').modal('show');
    });

    jq('#addProcesso').on('click', function () {
        clearAll();
        jq('#modalNovoProcesso').modal('show');
    });

    myApp.commom.dataTableIni("#tblpastas");

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

            var tipo = jq(this).next().next().next().next().text().trim().replace('#', '');
            if (tipo.trim() == "Pasta") {
                myApp.commom.abrirPastaMapeando(this);
            } else if (tipo.trim() == "Processo") {
                myApp.commom.abrirProcesso(this);
            }
        });

    });

    jq('#btnGravarProcesso').on('click', function () {
        var processo = {
            NmProcesso: jq('#NmProcesso').val(),
            DsProcesso: jq('#DsProcesso').val(),
            IdPasta: myApp.commom.getParameterByName("idpasta"),
            IdProcesso: jq('#idpasta').val()
        }

        myApp.doAjaxPost({
            action: myApp.retornarAppName + "/PastaDepto/GravarProcesso",
            callback: function (data) {
                myApp.processData(data, function () {
                    jq('#tblpastas').DataTable().destroy();
                    if (jq('#idpasta').val() != '') {
                        jq('.editavel').remove();
                    }

                    var tdabrir = myApp.doomer.createElement(null, 'td', '<a href="#"><span class="glyphicon glyphicon-eye-open"></span> Abrir</a>')
                    var tdeditar = myApp.doomer.createElement(null, 'td', '<a href="#"><span class="glyphicon glyphicon-pencil"></span> Editar </a>')
                    var tddeletar = myApp.doomer.createElement(null, 'td', '<a href="#"><span class="glyphicon glyphicon-minus"></span> Deletar</a>')

                    var tdcodigo = myApp.doomer.createElement(null, 'td', data.Id);
                    var tdpasta = myApp.doomer.createElement(null, 'td', '<span class="glyphicon glyphicon-paperclip"></span> Processo');

                    var tdnome = myApp.doomer.createElement(null, 'td', data.NomeProcesso);
                    var tdds = myApp.doomer.createElement(null, 'td', data.Descricao);
                    var tdowner = myApp.doomer.createElement(null, 'td', data.Owner.NmEntidade);

                    var tr = myApp.doomer.createElement(null, 'tr', '');

                    jq(tr).append(tdabrir).append(tdeditar).append(tddeletar).append(tdcodigo).append(tdpasta).append(tdnome).append(tdds).append(tdowner);
                    jq('#tblpastas tbody').append(tr);

                    jq(tddeletar).on('click', function (e) {
                        apagarPasta(this);
                    });

                    jq(tdeditar).on('click', function () {
                        editarPasta(this);
                    });

                    jq(tdabrir).on('click', function () {
                        myApp.commom.abrirProcesso(this);
                    });

                    jq('#modalNovaPasta').modal('hide');

                    myApp.commom.dataTableIni('#tblpastas');
                    if (jq('#idpasta').val() != '') {
                        myApp.alert('Pasta atualizada com sucesso!', myApp.commom.time);
                    } else {
                        myApp.alert('Pasta inserida com sucesso!', myApp.commom.time);
                    }
                }, function () {
                    alert(data.message);
                });
            },
            callbackerro: function () { },
            jsondata: processo
        });
    });

    jq('#btnGravarNovaPasta').on('click', function () {
        var action = myApp.retornarAppName + '/PastaDepto/GravarPastaFilha';

        var pasta = {
            NmPasta: jq('#NmPasta').val(),
            DsPasta: jq('#DsPasta').val()
        }

        //update
        if (jq('#idpasta').val() != '') {
            action = myApp.retornarAppName + '/PastaDepto/GravarPasta';
            pasta.idpastaform = jq('#idpasta').val();
        } else {
            //gravar filha
            pasta.idpastapai = myApp.commom.getParameterByName('idpasta');
        }

        myApp.doAjaxPost({
            action: action,
            callback: function (data) {
                myApp.processData(data, function () {
                    jq('#tblpastas').DataTable().destroy();
                    if (jq('#idpasta').val() != '') {
                        jq('.editavel').remove();
                    }

                    var tdabrir = myApp.doomer.createElement(null, 'td', '<a href="#"><span class="glyphicon glyphicon-eye-open"></span> Abrir</a>')
                    var tdeditar = myApp.doomer.createElement(null, 'td', '<a href="#"><span class="glyphicon glyphicon-pencil"></span> Editar </a>')
                    var tddeletar = myApp.doomer.createElement(null, 'td', '<a href="#"><span class="glyphicon glyphicon-minus"></span> Deletar</a>')

                    var tdcodigo = myApp.doomer.createElement(null, 'td', data.Codigo);
                    var tdpasta = myApp.doomer.createElement(null, 'td', '<span class="glyphicon glyphicon-folder-open"></span> Pasta');

                    var tdnome = myApp.doomer.createElement(null, 'td', data.NmPasta);
                    var tdds = myApp.doomer.createElement(null, 'td', data.DsPasta);
                    var tdowner = myApp.doomer.createElement(null, 'td', data.Owner.NmEntidade);

                    var tr = myApp.doomer.createElement(null, 'tr', '');

                    jq(tr).append(tdabrir).append(tdeditar).append(tddeletar).append(tdcodigo).append(tdpasta).append(tdnome).append(tdds).append(tdowner);
                    jq('#tblpastas tbody').append(tr);

                    jq(tddeletar).on('click', function (e) {
                        apagarPasta(this);
                    });

                    jq(tdeditar).on('click', function () {
                        editarPasta(this);
                    });

                    jq(tdabrir).on('click', function () {
                        myApp.commom.abrirPasta(this);
                    });

                    jq('#modalNovaPasta').modal('hide');

                    myApp.commom.dataTableIni('#tblpastas');
                    if (jq('#idpasta').val() != '') {
                        myApp.alert('Pasta atualizada com sucesso!', myApp.commom.time);
                    } else {
                        myApp.alert('Pasta inserida com sucesso!', myApp.commom.time);
                    }
                }, function () {
                    alert(data.message);
                });
            },
            callbackerro: function () { },
            jsondata: pasta
        });
    });

    function editarPasta(val) {
        clearAll();

        alterable = jq(val);
        jq(val).parent().addClass('editavel');
        var pasta = {
            id: jq(val).next().next().text().trim(),
            tipo: jq(val).next().next().next().text().trim(),
            nmpasta: jq(val).next().next().next().next().text().trim(),
            dspasta: jq(val).next().next().next().next().next().text().trim()
        };

        if (pasta.tipo == 'Pasta') {
            jq('#NmPasta').val(pasta.nmpasta);
            jq('#DsPasta').val(pasta.dspasta);
            jq('#modalNovaPasta').modal({ keyboard: true });
        } else {
            jq('#NmProcesso').val(pasta.nmpasta);
            jq('#DsProcesso').val(pasta.dspasta);
            jq('#modalNovoProcesso').modal({ keyboard: true });
        }
        jq('#idpasta').val(pasta.id);
    }

    function clearAll() {
        jq('#NmPasta').val('');
        jq('#DsPasta').val('');
        jq('#idpasta').val('');
        jq('#NmProcesso').val('');
        jq('#DsProcesso').val('');
    }

    function apagarPasta(val) {
        if (confirm('Deseja realmente apagar esta pasta?')) {

            var id = { id: jq(val).next().text() };
            var tipo = jq(val).next().next().text().trim();
            var action = "";

            //apago tanto pasta quanto processo
            if (tipo == "Pasta") {
                action = myApp.retornarAppName + '/PastaDepto/ApagarPasta';
            } else {
                action = myApp.retornarAppName + '/PastaDepto/ApagarProcesso';
            }
            myApp.doAjaxPost({
                action: action,
                callback: function (data) {
                    myApp.processData(data, function () {
                        if (data.msgerro == null) {
                            jq('#tblpastas').DataTable().destroy();
                            jq(val).parent().remove();
                            myApp.commom.dataTableIni('#tblpastas');
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

});