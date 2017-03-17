/// <reference path="jquery-1.9.0.js" />
/// <reference path="default.js" />
var jq = jQuery.noConflict();
jq(document).ready(function () {
    var doomer = myApp.doomer;
    var time = myApp.commom.time;
    listarDeptos();


    jq('#btngravar').on('click', function () {
        gravarRelacionamento();
    });

    jq('#selusuario').on('change', function () {
        atualizarGridDeptoUsuarios(this);
    });

    function atualizarGridDeptoUsuarios(key) {
        var val = { identidade: jq(key).val() };
        if (jq(key).val() != '') {
            myApp.doAjaxPost({
                action: myApp.retornarAppName + '/DeptoUsuario/RetornarDeptosEntidade',
                jsondata: val,
                callback: function (d) {
                    criarGridRelacionamento(jq, d);
                },
                callbackerro: function (d) {
                    alert(d.message);
                }
            })
        }
    }

    function createGrid(jq, data) {
        jq('#tbldepto').DataTable().destroy();
        jq('#tbldepto tbody').html('');
        for (var i = 0; i < data.length; i++) {
            var td = doomer.createElement('', 'td', data[i].Id);
            var td1 = doomer.createElement('', 'td', data[i].NomeDepto);
            var td2 = doomer.createElement('', 'td', '<input type="checkbox" />');
            var linha = doomer.createElement(null, 'tr', '');
            jq(linha).append(td).append(td1).append(td2);

            jq('#tbldepto tbody').append(linha);
        }

        jq('#tbldepto').DataTable({
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

    function gravarRelacionamento() {
        function UserDepto(userId) {
            this.userId = userId;
            this.userDeptos = [];
        }

        var obj = new UserDepto(jq('#selusuario').val());

        jq('#tbldepto tbody tr').each(function (i, v) {
            var check = jq(v).find('input').eq(0);
            if (jq(check).is(':checked')) {
                obj.userDeptos.push(jq(v).find('td').eq(0).html());
            }
        });
        var dados = { identidade: obj.userId, iddepto: obj.userDeptos.join() };
        myApp.doAjaxPost({
            action: myApp.retornarAppName + '/DeptoUsuario/GravarRelacao',
            jsondata: dados,
            callback: function (data) {
                atualizarGridDeptoUsuarios(jq('#selusuario'));
                listarDeptos();
                myApp.alert('Registro atrelado com sucesso!', time);
            },
            callbackerro: function (data) { }
        });

    }

    function listarDeptos() {
        myApp.doAjaxPost({
            action: myApp.retornarAppName + '/DeptoUsuario/RetornarListaDeptos',
            jsondata: '{}',
            callback: function (data) {
                createGrid(jq, data);
            },
            callbackerro: function (data) { }
        });
    }

    function criarGridRelacionamento(jq, data) {
        //RetornarDeptosEntidade
        jq('#tbldeptoentidade tbody').html('');
        for (var i = 0; i < data.length; i++) {
            //<td onclick="alert("ok")";><a href="#">Deletar</a></td>' + '<td>' + data[i].NmDepto + '</td>' + '<td>' + data[i].Id + '</td>
            var tr = doomer.createElement('', 'tr', '');
            var td = doomer.createElement('tddel', 'td', '<a href="#">Deletar</a>');
            var td1 = doomer.createElement('tddel', 'td', data[i].cdDepto);
            var td2 = doomer.createElement('', 'td', data[i].NmDepto);

            jq(td).on('click', function () {
                var data = {
                    id: jq('#selusuario').val(),
                    iddepto: jq(this).next().html()
                };
                if (confirm('Deseja remover este departamento do usuário?')) {
                    myApp.doAjaxPost({
                        action: myApp.retornarAppName + '/DeptoUsuario/DeletarRelacao',
                        jsondata: data,
                        callback: function (d) {
                            atualizarGridDeptoUsuarios(jq('#selusuario'));
                            myApp.alert('Registro desatrelado com sucesso!', time);
                        },
                        callbackerro: function (d) {
                            alert(d.message);
                        }
                    });
                }
            });

            jq(tr).append(td).append(td1).append(td2);


            jq('#tbldeptoentidade tbody').append(tr);
        }
    }

});