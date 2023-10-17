let tableEmployessAvalaible;

let tableEmployessAdded;

document.addEventListener("DOMContentLoaded", function () {

    // Inicializar las tablas con DataTables
    tableEmployessAvalaible = $('#EmployessAvalaible').DataTable({
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

    // Inicializar las tablas con DataTables
    tableEmployessAdded = $('#EmployessAdded').DataTable({
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





document.getElementById("subsidiary_ID").addEventListener("change", function () {

    let subsidiary_ID = document.getElementById("subsidiary_ID").value;


    GetAddressPromise(subsidiary_ID)
        .then(response => {
            document.getElementById("subsidiary_Address").value = response;
            console.log(response);
        })
        .catch(error => {
            console.error(error);
        });

    GetEmployeesBySubsidiaryPromise(subsidiary_ID)
        .then(response => {
            document.getElementById("subsidiary_Address").value = response;

            //// Obtén una referencia al DataTable existente
            //let tablaExistente = $('#EmployessAvalaible').DataTable();

            //// Limpia los datos existentes en la tabla
            //tablaExistente.clear().draw();

            //// Agrega los nuevos datos a la tabla
            //tablaExistente.rows.add(nuevosDatos).draw();


            console.log(response);
        })
        .catch(error => {
            console.error(error);
        });

});


function GetAddressPromise(id) {
    return new Promise((resolve, reject) => {

        $.ajax({
            url: "/TravelHistory/GetAddress",
            method: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ subsidiary_ID: id }),
        })
            .done(function (res) {
                resolve(res)
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                // Error handling
                reject(new Error("Request timed out"));
            });
    });
}

function GetEmployeesBySubsidiaryPromise(id) {
    return new Promise((resolve, reject) => {

        $.ajax({
            url: "/TravelHistory/GetEmployeesBySubsidiary",
            method: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ subsidiary_ID: id }),
        })
            .done(function (res) {
                resolve(res)
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                // Error handling
                reject(new Error("Request timed out"));
            });
    });
}
//$("#EmployessAvalaible tbody").on("click", "input#addToTravel", function () {

//    let data = tableNoAdded.row($(this).parents("tr")).data();
//    console.log(data);
//})