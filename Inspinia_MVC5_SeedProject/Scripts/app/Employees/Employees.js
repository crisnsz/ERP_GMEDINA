var tableNoAssign;
var rowsData = [];
var tableAssign;
$(document).ready(function () {
    // Inicializa las tablas con DataTables
    var table1 = $('#NoAdded').DataTable({
        "order": [],
        "columnDefs": [{
            "targets": [0],
            "visible": false
        }]
    });

    var table2 = $('#Added').DataTable({
        "order": [[0, 'asc']],
        "columnDefs": [{
            "targets": [0],
            "visible": false
        }]
    });

    $('#AddSubsidiary').on('click', function () {
        table1.rows().nodes().to$().each(function () {
            var checkbox = $(this).find('td:eq(2) input[type="checkbox"]');
            if (checkbox.prop('checked')) {
                var data = table1.row($(this)).data();
                table2.row.add(data).draw();
                table1.row($(this)).remove().draw();
            }
        });

        var datos = [];
        $('#Added tbody tr').each(function () {
            let subsidiary_ID = $(this).find('td:eq(0)').text();
            let subsidiary_Name = $(this).find('td:eq(1)').text();
            datos.push({ ID: subsidiary_ID, Nombre: subsidiary_Name });
        });

        // Convierte los datos a una cadena JSON
        var datosJSON = JSON.stringify(datos);

        table2.order([0, 'asc']).draw();
        $.ajax({
            url: "/Employee/AddSubsidiary",
            method: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ tbEmployeesSubsidiary: datosJSON }),
        })
    });

    $('#DelSubsidiary').on('click', function () {
        table2.rows().nodes().to$().each(function () {
            var checkbox = $(this).find('td:eq(2) input[type="checkbox"]');
            if (checkbox.prop('checked')) {
                var data = table2.row($(this)).data();
                table1.row.add(data).draw();
                table2.row($(this)).remove().draw();
            }
        });
        table1.order([0, 'asc']).draw();
    });


    $('#enviarDatos').click(function () {
        var datos = [];
        $('#Added tbody tr').each(function () {
            var id = $(this).find('td:eq(0)').text();
            var nombre = $(this).find('td:eq(1)').text();
            datos.push({ ID: id, Nombre: nombre });
        });

        // Convierte los datos a una cadena JSON
        var datosJSON = JSON.stringify(datos);

        // Crea un campo de formulario oculto para enviar los datos
        $('<input>').attr({
            type: 'hidden',
            name: 'datos',
            value: datosJSON
        }).appendTo('form');

        // Envía el formulario
        $('form').submit();
    });
});


//$('#AddSubsidiary').click(function () {
//    // Inicializa DataTables en la tabla
//    var tabla1 = document.getElementById("NoAdded");

//    var tabla2 = document.getElementById("Added");

//    // Agregar un evento de clic a las casillas de verificación en la tercera columna
//    var filasTabla1 = tabla1.getElementsByTagName("tr");
//    for (var i = 1; i < filasTabla1.length; i++) { // Comenzamos en 1 para omitir la fila de encabezado
//        var checkbox = filasTabla1[i].querySelector("input[type='checkbox']");
//        checkbox.addEventListener("change", function () {
//            // Verificar el estado de la casilla de verificación
//            if (this.checked) {
//                // Clonar la fila seleccionada y agregarla a la tabla2
//                var filaSeleccionada = filasTabla1[i].cloneNode(true);
//                tabla2.appendChild(filaSeleccionada);

//                // Eliminar la fila seleccionada de la tabla1
//                filasTabla1[i].parentNode.removeChild(filasTabla1[i]);
//            }
//        });
//    }

//})



//$(document).ready(function () {




//    $('#AgregarRol').prop('disabled', true);
//    $('#QuitarRol').prop('disabled', true);
//    $('#btnGuardarUsuario').prop('disabled', true);

//    $('#NoAdded').DataTable({

//        "searching": true,
//        "scrollY": "300px",
//        "scrollCollapse": true,
//        "paging": false,
//        "info": false,
//        "oLanguage": {
//            "oPaginate": {
//                "sNext": "Siguiente",
//                "sPrevious": "Anterior",
//            },
//            "sSearch": "Buscar",
//            "sLengthMenu": "Mostrar _MENU_ Registros Por Página",
//            "sInfo": "Mostrando _START_ a _END_ Entradas"

//        },

//    });

//    $('#Added').DataTable({

//        "searching": true,
//        "scrollY": "300px",
//        "scrollCollapse": true,
//        "paging": false,
//        "info": false,
//        "oLanguage": {
//            "oPaginate": {
//                "sNext": "Siguiente",
//                "sPrevious": "Anterior",
//            },
//            "sSearch": "Buscar",
//            "sLengthMenu": "Mostrar _MENU_ Registros Por Página",
//            "sInfo": "Mostrando _START_ a _END_ Entradas"

//        },

//    });
//    $('#Added> tbody > tr').each(function () {
//        $(this).remove();
//    })

//})

////  $('#Table_BuscarEmpleado').DataTable(
////  {
////    "searching": false,
////    "lengthChange": false,

////    "oLanguage": {
////      "oPaginate": {
////        "sNext": "Siguiente",
////        "sPrevious": "Anterior",
////      },
////      "sEmptyTable": "No hay registros",
////      "sInfoEmpty": "Mostrando 0 de 0 Entradas",
////      "sSearch": "Buscar",
////      "sInfo": "Mostrando _START_ a _END_ Entradas",

////    }
////  });

////var $rows = $('#Table_BuscarEmpleado tr');
////$("#search").keyup(function () {
////  var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();

////  $rows.show().filter(function () {
////    var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
////    return !~text.indexOf(val);
////  }).hide();
////});
////});

//$(document).on('keyup', '#usu_Password, #confirmar-pass', function () {
//    var usu_Password = $('#usu_Password').val().trim();
//    var bar = $('#confirmar-pass').val().trim();
//    valido = document.getElementById('Password');
//    if (usu_Password != '' && bar != '') {
//        if (usu_Password.length < 8) {
//            valido.innerText = "Longitud debe ser mínimo de 8 caracteres";
//            $('#AgregarRol').prop('disabled', false);
//            $('#QuitarRol').prop('disabled', false);
//            $('#btnGuardarUsuario').prop('disabled', true);
//        }
//        else {
//            if (!usu_Password || !bar || usu_Password == '' || bar == '') {
//                valido.innerText = "Las contraseñas no coinciden";
//            }
//            else {
//                if (usu_Password !== bar) {
//                    $('#AgregarRol').prop('disabled', true);
//                    $('#QuitarRol').prop('disabled', true);
//                    $('#btnGuardarUsuario').prop('disabled', true);
//                    valido.innerText = "Las contraseñas no coinciden";
//                }

//                else {
//                    $('#AgregarRol').prop('disabled', false);
//                    $('#QuitarRol').prop('disabled', false);
//                    $('#btnGuardarUsuario').prop('disabled', false);
//                    valido.innerText = "";
//                }
//            }
//        }
//    }
//});


//$('#AddSubsidiary').click(function () {
//    $('#NoAdded > tbody > tr').each(function () {
//        idItem = $(this).data('id');
//        isChecked = $('#check' + idItem).is(':checked')
//        if (isChecked) {
//            active = $(this);
//            $('#NoAsignados tbody').append(active)
//            $('#check' + idItem).prop('checked', false);
//            $(this).remove();
//            $('#Added tbody').append(active)

//        }
//    })
//})

//$('#DelSubsidiary').click(function () {
//    $('#Added > tbody > tr').each(function () {
//        idItem = $(this).data('id');
//        if ($('#check' + idItem).is(':checked')) {
//            active = $(this);
//            $('#Added tbody').append(active)
//            $('#check' + idItem).prop('checked', false);
//            $(this).remove();
//            $('#NoAssign tbody').append(active)
//            var RolesUsuario = {
//                rol_Id: idItem,
//            };
//            var NoAsignados = $('#NoAssign').length;
//            $('#Added tbody').append(active)
//            $('#check' + idItem).prop('checked', false);
//            $(this).remove();
//            $('#NoAssign tbody').append(active);
//        }
//    })
//})

//function Seleccionar(emp_Id) {
//    $.ajax({
//        url: "/Usuario/getEmpleado",
//        method: "POST",
//        dataType: 'json',
//        contentType: "application/json; charset=utf-8",
//        data: JSON.stringify({ EmpleadoID: emp_Id }),
//    })
//        .done(function (data) {
//            if (data.length > 0) {
//                $.each(data, function (i, item) {
//                    $('#usu_Nombres').val(item.emp_Nombres);
//                    $('#usu_Apellidos').val(item.emp_Apellidos);
//                    $('#usu_Correo').val(item.emp_Correoelectronico);
//                    $('#emp_Id').val(item.emp_Id);
//                    separador = " ", // un espacio en blanco
//                        cadena = $('#usu_Nombres').val();
//                    cadena2 = $('#usu_Apellidos').val();
//                    arregloDeSubCadenas = Reemplazar(cadena);
//                    arregloDeSubCadenas2 = Reemplazar(cadena2);
//                    arregloDeSubCadenas = arregloDeSubCadenas.split(separador);
//                    arregloDeSubCadenas2 = arregloDeSubCadenas2.split(separador);
//                    $('#usu_NombreUsuario').val(arregloDeSubCadenas + "." + arregloDeSubCadenas2);
//                    $("#ModalAgregarEmpleado").modal('hide');
//                })
//            }
//        })
//}


