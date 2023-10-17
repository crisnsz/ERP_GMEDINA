var tableNoAdded;

var tableAdded;


document.addEventListener("DOMContentLoaded", function () {

    // Inicializar las tablas con DataTables
    tableNoAdded = $('#NoAdded').DataTable({
        "searching": false,
        "lengthChange": false,

        "order": [[0, 'asc']],
        "columnDefs": [{
            "targets": [0],
            "visible": false
        }],
        "oLanguage": {
            "oPaginate": {
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
            "sEmptyTable": "No hay registros",
            "sInfoEmpty": "Mostrando 0 de 0 Entradas",
            "sSearch": "Buscar",
            "sInfo": "Mostrando _START_ a _END_ Entradas",

        }
    });

    tableAdded = $('#Added').DataTable({
        "searching": false,
        "lengthChange": false,

        "order": [[0, 'asc']],
        "columnDefs": [{
            "targets": [0],
            "visible": false
        }],
        "oLanguage": {
            "oPaginate": {
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
            "sEmptyTable": "No hay registros",
            "sInfoEmpty": "Mostrando 0 de 0 Entradas",
            "sSearch": "Buscar",
            "sInfo": "Mostrando _START_ a _END_ Entradas",

        }
    });
});

$("#NoAdded tbody").on("click", "input#AddSubsidiary", function () {
    var data = tableNoAdded.row($(this).parents("tr")).data();
    var subsidiary_ID = data[0];
    var subsidiary_Name = data[1];

    

    SyncToControllerAddPromise(subsidiary_ID, subsidiary_Name)
        .then(result => {
            console.log(result); // Data fetched from https://example.com/api/data

            //Cambiar id de #AddSubsidiary a #DelSubsidiary
            data[3] = '<input name="id02" type="button" id="DelSubsidiary" value="&#9668; Quitar &nbsp;&nbsp;" class="btn btn-primary btn-xs">'

            // Mueve la fila a tabla2
            tableAdded.row.add(data).draw();

            // Elimina la fila de tabla1
            tableNoAdded.row($(this).parents("tr")).remove().draw();
        })
        .catch(error => {
            console.error(error); // Request timed out
        });
});
function SyncToControllerAddPromise(id, km) {
    return new Promise((resolve, reject) => {
        let Subsidiary = {
            subsidiary_ID: id,
            employeeSubsidiary_DistanceKM: km,
        };

        $.ajax({
            url: "/Employee/AddSubsidiary",
            method: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ tbEmployeesSubsidiary: Subsidiary }),
        })
            .done(function (data) {
                resolve(data)
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                // Error handling
                reject(new Error("Request timed out"));
            });
    });
}


$("#Added tbody").on("click", "input#DelSubsidiary", function () {
    var data = tableAdded.row($(this).parents("tr")).data();
    var subsidiary_ID = data[0];
    var subsidiary_Name = data[1];


    SyncToControllerRemovePromise(subsidiary_ID, subsidiary_Name)
        .then(result => {
            console.log(result); // Data fetched from https://example.com/api/data

            //Cambiar id de #DelSubsidiary a #AddSubsidiary
            data[3] = '<input name="id02" type="button" id="AddSubsidiary" value="Agregar &#9658" class="btn btn-primary btn-xs">'

            // Mueve la fila a tabla2
            tableNoAdded.row.add(data).draw();

            // Elimina la fila de tabla1
            tableAdded.row($(this).parents("tr")).remove().draw();
        })
        .catch(error => {
            console.error(error); // Request timed out
        });
    tableNoAdded.order([0, 'asc']).draw();
});

function SyncToControllerRemovePromise(id, name) {
    return new Promise((resolve, reject) => {
        let Subsidiary = {
            subsidiary_ID: id,
            subsidiary_Name: name,
        };

        $.ajax({
            url: "/Employee/RemoveSubsidiary",
            method: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ tbSubsidiary: Subsidiary }),
        })
            .done(function (data) {
                resolve(data)
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                // Error handling
                reject(new Error("Request timed out"));
            });
    });

}
