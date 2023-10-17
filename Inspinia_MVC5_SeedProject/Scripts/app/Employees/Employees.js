var noAssignTable;

var assignTable;


$(document).ready(function () {

    
    // Inicializa las tablas con DataTables
    noAssignTable = $('#NoAdded').DataTable({
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

    assignTable = $('#Added').DataTable({
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

$("#AddSubsidiary").on("click", function (event) {
    noAssignTable.rows().nodes().to$().each(function () {

        let checkbox = $(this).find('td:eq(2) input[type="checkbox"]');
        if (checkbox.prop('checked')) {
            let data = noAssignTable.row($(this)).data();
            let subsidiary_ID = data[0];
            let subsidiary_Name = data[1];
            SyncToControllerAddPromise(subsidiary_ID, subsidiary_Name)
                .then(result => {
                    console.log(result); // Data fetched from https://example.com/api/data
                    assignTable.row.add(data).draw();
                    noAssignTable.row($(this)).remove().draw();
                })
                .catch(error => {
                    console.error(error); // Request timed out
                });
        }
    });
})



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

$("#DelSubsidiary").on("click", function (event) {

    assignTable.rows().nodes().to$().each(function () {
        let checkbox = $(this).find('td:eq(2) input[type="checkbox"]');
        if (checkbox.prop('checked')) {
            let data = assignTable.row($(this)).data();

            let subsidiary_ID = data[0];
            let subsidiary_Name = data[1];

            SyncToControllerRemovePromise(subsidiary_ID, subsidiary_Name)
                .then(result => {
                    console.log(result); // Data fetched from https://example.com/api/data
                    noAssignTable.row.add(data).draw();
                    assignTable.row($(this)).remove().draw();
                })
                .catch(error => {
                    console.error(error); // Request timed out
                });

        }
    });
    noAssignTable.order([0, 'asc']).draw();
})



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
