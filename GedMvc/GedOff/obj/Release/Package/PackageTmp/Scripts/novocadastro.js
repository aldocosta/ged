/// <reference path="jquery-1.9.0.js" />
/// <reference path="default.js" />
/// <reference path="bootstrapPager.min.js" />

var jq = jQuery.noConflict();
jq(document).ready(function () {

    carregarGrid();

    jq('#btnBloquear').on('click', function () {
        if (confirm('Deseja realmente marcar este registro como bloqueado? O usuário não acessará o sistema depois deste procedimento')) {
            if (true){
                markAsDeleted(jq('#spnidregistro').text().split(':')[1].trim(), jq);
            } else {
                alert('Usuário admin não pode ser marcado como deletado');
            }
        }        
    });

    jq('#btnlista').on('click', function () {
        if (!jq('#pnllista').is(":visible")) {
            jq('#pnllista').slideDown();
        }
        else {
            jq('#pnllista').slideUp();
        }
    });

    jq('#btngravar').on('click', function () {
        novoRegistro();
    });

    jq('#btnReset').on('click', function () {
        var data = { id: jq('#spnidregistro').text().split(':')[1] };
        myApp.doAjaxPost({
            action: myApp.retornarAppName + '/cadastro/ResetSenha',
            callback: function (data) {
                myApp.processData(data, function () {
                    if (data) {
                        jq('.close').click();
                        myApp.alert("Reset efetuado para 123456", myApp.commom.time);
                    }
                }, function () {
                });
            },
            callbackerro: function () {

            }, jsondata: data
        });
    });


    jq('#btnNovo').on('click', function () {
        jq('#btnBloquear').hide();
        jq('#modalNovo').modal({ keyboard: true });
        clearControls();
        jq('#myModalLabel').html('').html('Novo Registro');
    });

    function carregarGrid() {
        var action = myApp.retornarAppName + '/Cadastro/CriarGrid';
        myApp.doAjaxPost({
            action: action,
            callback: function (data) {
                myApp.processData(data, function () {
                    createGrid(jq, data);
                }, function () {
                    alert(data.error);
                });
            },
            callbackerro: function (data) {
                alert(data.error);
            }
        });
    }

    function createGrid(jq, data) {
        jq('#tablecon').DataTable().destroy();
        jq('#tablecon tbody').html('');
        for (var i = 0; i < data.length; i++) {
            var tr = document.createElement('tr');

            var tdalt = document.createElement('td');
            jq(tdalt).append('<a href="#">Alterar</a>');

            var tddel = document.createElement('td');
            jq(tddel).append('<a href="#">Deletar</a>');

            var td2 = document.createElement('td');
            jq(td2).append(data[i].Codigo);

            var td3 = document.createElement('td');
            jq(td3).append(data[i].NmEntidade);

            var td4 = document.createElement('td');
            jq(td4).append(data[i].NmUser);

            var td5 = document.createElement('td');
            jq(td5).append(data[i].NmEmail);

            var td6 = document.createElement('td');
            jq(td6).append(data[i].Tipo.Nome);

            jq(tdalt).on('click', function () {
                jq('#myModalLabel').html('').html('Alteração de Registro');
                jq('#NmEntidade').val(jq(jq(this)[0]).next().next().next().text());
                jq('#NmUser').val(jq(jq(this)[0]).next().next().next().next().text());
                jq('#NmEmail').val(jq(jq(this)[0]).next().next().next().next().next().text());
                jq('#spnidregistro').html('ID: ' + jq(jq(this)[0]).next().next().text() + '');

                var sel = jq(jq(this)[0]).next().next().next().next().next().next().text();
                jq('#btnBloquear').show();
                switch (sel) {
                    case "Administrador":
                        jq('#selTipo option[value="1"]').attr('selected', 'selected');
                        break;
                    case "Usuário":
                        jq('#selTipo option[value="2"]').attr('selected', 'selected');
                        break;
                }
                jq('#modalNovo').modal({ keyboard: true });
            });

            jq(tddel).on('click', function () {
                if (confirm('Deseja realmente deletar este registro')) {
                    if (jq(jq(this)[0]).next().next().text() != "admin") {
                        deletar(jq(jq(this)[0]).next().text(), jq);
                    } else {
                        alert('Usuário admin não pode ser deletado');
                    }
                }
            });

            jq(tr).append(tdalt).append(tddel).append(td2).append(td3).append(td4).append(td5).append(td6);
            jq('#tablecon tbody').append(tr);
        }
        jq('#tablecon').DataTable({
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

    function novoRegistro() {

        var ent = {
            Codigo: jq('#spnidregistro').html(),
            NmEntidade: jq('#NmEntidade').val(),
            NmUser: jq('#NmUser').val(),
            NmEmail: jq('#NmEmail').val(),
            selTipo: jq('#selTipo').val()
        };

        ent.Codigo = ent.Codigo.replace("ID: ", '');

        myApp.doAjaxPost({
            action: myApp.retornarAppName + '/Cadastro/NovoRegistro',
            jsondata: ent,
            callback: function (data) {
                myApp.processData(data, function () {
                    carregarGrid();
                    jq('#btnfecharnovo').click();
                    myApp.alert('Registro inserido/alterado com sucesso!', myApp.commom.time);
                }, function () {
                    jq('#btnfecharnovo').click();
                    alert('Atenção, ocorreu um erro: ' + data.error);
                });
            },
            callbackerro: function (data) {
                alert(data.responseText);
            }
        });

    }

    function deletar(id, jq) {
        var data = { id: id };
        myApp.doAjaxPost({
            action: myApp.retornarAppName + '/Cadastro/DeletarRegistro',
            callback: function (data) {
                myApp.processData(data, function () {
                    carregarGrid();
                    jq('#btnfecharnovo').click();
                    myApp.alert('Registro excluído com sucesso!', myApp.commom.time);
                }, function () {
                    alert('Atenção, ocorreu um erro: ' + data.error);
                });
            },
            callbackerro: function () { },
            jsondata: data
        });
    }

    function markAsDeleted(id, jq) {
        var data = { id: id };
        myApp.doAjaxPost({
            action: myApp.retornarAppName + '/Cadastro/BloquearRegistro',
            callback: function (data) {
                myApp.processData(data, function () {
                    carregarGrid();
                    jq('#btnfecharnovo').click();
                    myApp.alert('Registro marcado como excluído com sucesso!', myApp.commom.time);
                }, function () {
                    alert('Atenção, ocorreu um erro: ' + data.error);
                });
            },
            callbackerro: function () { },
            jsondata: data
        });
    }

    function clearControls() {
        jq('#NmEntidade').val('');
        jq('#NmUser').val('');
        jq('#NmEmail').val('');
        jq('#selTipo option[value="1"]').attr('selected', 'selected');
        jq('#spnidregistro').html('');
    }
});