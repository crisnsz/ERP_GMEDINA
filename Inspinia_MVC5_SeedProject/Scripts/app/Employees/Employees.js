var tableNoAssign;
var rowsData = [];
var tableAssign;
$(document).ready(function () {
    // Inicializa DataTables en la tabla
    tableNoAssign = $('#NoAdded').DataTable();


    // Obtiene los datos de las columnas 1 y 2 y las almacena en un arreglo
    //var rowsData = table.rows().data().map(function (row) {
    //    return [row[0], row[1], row[2]]; // Obtiene datos de la Columna 1 y Columna 2 de cada fila
    //});
    let maxIndex = tableNoAssign.rows().count();

    for (let i = 0; i < maxIndex && i < tableNoAssign.rows().count(); i++) {
        let row = tableNoAssign.row(i).data();
        rowsData.push([row[1], row[2], row[3]]);
    }

    // Convierte el arreglo en una cadena JSON
    jsonData = JSON.stringify(rowsData[0]);


    // Imprime la cadena JSON (puedes hacer lo que necesites con los datos)
    console.log(jsonData);

})


$('#AddSubsidiary').click(function () {
    // Inicializa DataTables en la tabla
    //var table = $('#NoAdded').DataTable();


    // Get the checkbox values
    var checkboxValues = [];

    var checkboxValuestoAdd = [];
    tableNoAssign.column(3).nodes().to$().find('#check').each(function (row) {
        checkboxValues.push($(this).prop('checked'));
    });




    // Use map to get the indices where the value is true
    const trueIndices = checkboxValues.map((value, index) => (value === true ? index : -1)).filter(index => index !== -1);


    const valoresObtenidos = trueIndices.map(index => rowsData[index]);
       
    var data = rowsData[0];

    const valoresNoEnArray2 = rowsD*******ata.filter((value, index) => !trueIndices.includes(index));

    console.log(valoresNoEnArray2);



    $('#Added').DataTable({
        data: valoresObtenidos
    });

})



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


