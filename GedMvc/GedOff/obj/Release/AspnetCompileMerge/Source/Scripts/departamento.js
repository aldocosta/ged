/// <reference path="jquery-1.9.0.min.js" />
/// <reference path="bootstrapPager.min.js" />
/// <reference path="default.js" />

var jq = jQuery.noConflict();
jq(document).ready(function () {
    var time = { 'in': 100, 'out': 4000 }
    criarLista();

    jq('#btnNovo').on('click', function () {
        jq('#modalNovo').modal({ keyboard: true });
        jq('#NmDepto').val('');
    });

    jq('#btngravar').on('click', function () {
        var obj = JSON.stringify({
            NmDepto: jq('#NmDepto').val(),
            Id: jq('#spnidregistro').html().split(':')[1]
        });

        jq.ajax({
            type: 'POST',
            url: myApp.retornarAppName + '/Departamento/NovoDepto',
            dataType: 'json',
            data: obj,
            contentType: 'application/json; charset=utf-8',
            error: function (data) {
                alert('Erro, contate o administrador');
            }
        }).done(function (data) {
            myApp.processData(data, function () {
                criarLista();
                jq('#modalNovo').modal('hide');
                clearAll();
                myApp.alert('Registro inserido/alterado com sucesso!', time);
            }, function () {
                alert("Atenção, ocorreu um erro: " + data.error);
            });
        });
        jq('#btndismiss').click();
    });

    function criarLista() {
        jq.ajax({
            type: 'POST',
            url: myApp.retornarAppName + '/Departamento/CriarGrid',
            dataType: 'json',
            data: '{}',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                myApp.processData(data, function () {
                    createGrid(jq, data);
                }, function () {
                    alert(data.error);
                });
            },
            error: function (data) {
                alert('Erro, contate o administrador');
            }
        });
    }

    function createGrid(jq, data) {
        //jq('#gridlista').DataTable().destroy();
        jq('#gridlista tbody').html('');
        for (var i = 0; i < data.length; i++) {
            var tr = document.createElement('tr');
            var tdalt = document.createElement('td');
            var tddel = document.createElement('td');
            var td = document.createElement('td');
            var td2 = document.createElement('td');
            var td3 = document.createElement('td');
            var td4 = document.createElement('td');

            jq(tdalt).on('click', function () {
                jq('#modalNovo').modal({ keyboard: true });
                jq('#spnidregistro').html('Id: ' + jq(this).next().next().html());
                jq('#NmDepto').val(jq(this).next().next().next().html());
                jq(this).next().next().next().html();
            });

            jq(tddel).on('click', function () {
                var data = JSON.stringify({ id: jq(this).next().html() });
                if (confirm('Deseja deletar esse registro?')) {
                    jq.ajax({
                        type: 'POST',
                        url: myApp.retornarAppName + '/Departamento/DeletarRegistro',
                        dataType: 'json',
                        data: data,
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            myApp.processData(data, function () {
                                criarLista();
                                myApp.alert('Registro excluído com sucesso!', time);
                            }, function () {
                                alert("Atenção, ocorreu um erro: " + data.error);
                            });                            
                        },
                        error: function (data) {
                            alert('Erro, contate o administrador');
                        }
                    });
                }
            });

            jq(tdalt).append('<a href="#">Alterar</a>');
            jq(tddel).append('<a href="#">Deletar</a>');

            jq(td).append(data[i].Id);
            jq(td2).append(data[i].NomeDepto);
            jq(td3).append(data[i].DataCriacao);
            jq(td4).append(data[i].Owner);
            jq(tr).append(tdalt).append(tddel).append(td).append(td2).append(td3).append(td4);
            jq('#gridlista tbody').append(tr);
        }



        jq('#gridlista').DataTable({
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

    jq('#btnfecharnovo').on('click', function () {
        jq('#modalNovo').modal('hide');
    });

    function clearAll() {
        jq('#spnidregistro').html('');
        jq('#NmDepto').val('');
    }
});