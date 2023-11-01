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
        },
        {
            targets: -1, // -1 refers to the last column
            orderable: false // Disable sorting for the last column
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
        },
        {
            targets: -1, // -1 refers to the last column
            orderable: false // Disable sorting for the last column
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
    let subsidiary = document.getElementById("subsidiary_ID");



    let subsidiary_ID = subsidiary.value;

    if (subsidiary_ID == "") {
        return;
    }


    getAddressPromise(subsidiary_ID)
        .then(response => {
            document.getElementById("subsidiary_Address").value = response;
            console.log(response);
        })
        .catch(error => {
            console.error(error);
        });

    getEmployeesBySubsidiaryPromise(subsidiary_ID)
        .then(res => {
            //document.getElementById("subsidiary_Address").value = res.subsidiary_Name;

            // Obtén una referencia al DataTable existente
            //let EmployessAvalaible = $('#EmployessAvalaible').DataTable();


            // Loop through the response data and populate the DataTable
            res.forEach(function (item) {
                let button = '<input name="id02" type="button" id="btnAddEmployee" class="btn btn-primary btn-xs" value="Agregar &#9658" data-employee-id="' + item.employee_ID + '">';

                let row = [
                    item.employee_ID, // ID (hidden)
                    item.employee_Name, // Nombre
                    item.employee_Direction, // Direccion
                    item.employeeSubsidiary_DistanceKM, // Direccion
                    button // Empty column
                ];

                //tableEmployessAvalaible.row.add(row);

                // Add the row to the DataTable
                let addedRow = tableEmployessAvalaible.row.add(row).draw(false).node();

                // Set the data-id attribute on the <tr> element
                $(addedRow).attr('data-id', item.employee_ID);
            });

            // Draw the DataTable to display the data
            tableEmployessAvalaible.draw();

            console.log(res);
        })
        .catch(error => {
            console.error(error);
        });

});


function getAddressPromise(id) {
    return new Promise((resolve, reject) => {
        fetch("/TravelHistory/GetAddress", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify({ subsidiary_ID: id }),
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error("Request failed with status: " + response.status);
                }
            })
            .then(data => {
                resolve(data);
            })
            .catch(error => {
                reject(error);
            });
    });
}

function getAddressPromiseOld(id) {
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

function getEmployeesBySubsidiaryPromise(id) {
    return new Promise((resolve, reject) => {
        fetch("/TravelHistory/GetEmployeesBySubsidiary", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify({ subsidiary_ID: id }),
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error("Request failed with status: " + response.status);
                }
            })
            .then(data => {
                resolve(data);
            })
            .catch(error => {
                reject(error);
            });
    });
}


$("#EmployessAvalaible tbody").on("click", "input#btnAddEmployee", function () {

    let data = tableEmployessAvalaible.row($(this).parents("tr")).data();
    let Employee_ID = data[0];



    AddEmployeeToTravelPromise(Employee_ID)
        .then(response => {

            console.log(`Add ${Employee_ID}`);


            //#DelSubsidiary
            data[4] = '<input name="id02" type="button" id="btnDelEmployee" value="&#9668; Quitar &nbsp;&nbsp;" class="btn btn-primary btn-xs">'


            // Mueve la fila a tabla2
            tableEmployessAdded.row.add(data).draw();

            // Elimina la fila de tabla1
            tableEmployessAvalaible.row($(this).parents("tr")).remove().draw();

        })
        .catch(error => {
            console.error(error); // Request timed out
        });


})


function AddEmployeeToTravelPromise(id) {
    return new Promise((resolve, reject) => {
        let Employee = {
            employee_ID: id,
            subsidiary_ID: 0,
            employeeSubsidiary_DistanceKM: 0
        };


        fetch("/Travel/AddEmployeestoTravel", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify({ tbEmployeesSubsidiary: Employee }),
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error("Request failed with status: " + response.status);
                }
            })
            .then(data => {
                resolve(data);
            })
            .catch(error => {
                reject(error);
            });
    });
}






$("#EmployessAdded tbody").on("click", "input#btnDelEmployee", function () {
    let data = tableEmployessAdded.row($(this).parents("tr")).data();
    let Employee_ID = data[0];

    console.log(`Remove ${Employee_ID}`);


    RemoveEmployeeToTravelPromise(Employee_ID)
        .then(result => {
            console.log(result); // Data fetched from https://example.com/api/data

            //Cambiar id de #btnDelEmployee a #btnAddEmployee
            data[4] = '<input name="id02" type="button" id="btnAddEmployee" value="&nbsp;Agregar &#9658" class="btn btn-primary btn-xs">'

            // Mueve la fila a tabla1
            tableEmployessAvalaible.row.add(data).draw();

            // Elimina la fila de tabla2
            tableEmployessAdded.row($(this).parents("tr")).remove().draw();

            tableEmployessAvalaible.order([0, 'asc']).draw();

        })
        .catch(error => {
            console.error(error); // Request timed out
        });
});





function RemoveEmployeeToTravelPromise(id) {

    return new Promise((resolve, reject) => {


        let Employee = {
            employee_ID: id,
            subsidiary_ID: 0,
            employeeSubsidiary_DistanceKM: 0
        };


        fetch("/Travel/RemoveEmployeestoTravel", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify({ tbEmployeesSubsidiary: Employee }),
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error("Request failed with status: " + response.status);
                }
            })
            .then(data => {
                resolve(data);
            })
            .catch(error => {
                reject(error);
            });
    });
}

//$("#EmployessAvalaible tbody").on("click", "input#addToTravel", function () {

//    let data = tableNoAdded.row($(this).parents("tr")).data();
//    console.log(data);
//})