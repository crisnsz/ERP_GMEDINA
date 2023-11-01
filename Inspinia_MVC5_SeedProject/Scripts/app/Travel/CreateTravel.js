let tableEmployessAvalaible;

let tableEmployessAdded;

const dropdown = document.getElementById("transporter_ID");
const dropdownBlocked = document.getElementById("subsidiary_ID");

const distance = document.getElementById("distance_Kilometers");
const totalCostField = document.getElementById("total_travel_Cost");

const add = "add";
const subtract = "subtract";
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

    setDate();


    let subsidiary_ID = document.getElementById("subsidiary_ID").value;
    console.log(subsidiary_ID);

    if (subsidiary_ID != '' && subsidiary_ID != null && subsidiary_ID != undefined) {
        loadSubsidaryInfo(subsidiary_ID);
    }

    distance.value = 0;
    totalCostField.value = 0;



});


// Add an event listener to the dropdown list
dropdown.addEventListener("change", function () {

    const selectedOption = dropdown.options[dropdown.selectedIndex];
    const transporter_ID = selectedOption.value;
    const transporter = document.getElementById("transporter_fee");

    console.log(`Transporter ${transporter_ID}`);


    if (transporter_ID == 'Transporter') {
        transporter.value = '';
    }

    getTransporterInfo(transporter_ID)
        .then(res => {
            transporter.value = res.transporter_Fee

            console.log(res);
        })
        .catch(error => {
            console.error(error);
        });



    // Check if the selected value is not an empty string

    if (dropdown.value !== "") {
        // Enable the input field
        dropdownBlocked.disabled = false;
    } else {
        // Disable the input field
        dropdownBlocked.disabled = true;
    }





});

function getTransporterInfo(id) {
    return new Promise((resolve, reject) => {
        fetch("/Travel/GetTransporterInfo", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify({ Transporter: id }),
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



function EmployeeTravelToday(employee_ID) {
    return new Promise((resolve, reject) => {
        fetch("/Travel/EmployeeTravelToday", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify({ Employee: employee_ID }),
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


function setDate() {
    // Get the current date and time
    let currentDate = new Date();

    // Extract the current year, month, and day
    let year = currentDate.getFullYear();
    let month = (currentDate.getMonth() + 1).toString().padStart(2, '0'); // Zero-pad the month
    let day = currentDate.getDate().toString().padStart(2, '0'); // Zero-pad the day

    // Create a string in the format 'YYYY-MM-DD'
    let todayDate = `${year}-${month}-${day}`;


    // Set the input field to today's date
    document.getElementById("departure_Date_and_Time").value = `${todayDate}T00:00`;

}


document.getElementById("subsidiary_ID").addEventListener("change", function () {

    let subsidiary_ID = document.getElementById("subsidiary_ID").value;

    loadSubsidaryInfo(subsidiary_ID);



});

function loadSubsidaryInfo(subsidiary_ID) {
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
            tableEmployessAvalaible.clear().draw();


            tableEmployessAdded.clear().draw();
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

}
function getAddressPromise(id) {
    return new Promise((resolve, reject) => {
        fetch("/Travel/GetAddress", {
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
            url: "/Travel/GetAddress",
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
    console.log(id);
    if (id == null || id === undefined) {
        // TODO document why this block is empty
        return;
    }

    return new Promise((resolve, reject) => {
        fetch("/Travel/GetEmployeesBySubsidiary", {
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

function sumOrSubtract(operation, firstValue, secondValue) {
    if (operation === add) {
        return firstValue + secondValue;
    } else if (operation === subtract) {
        return firstValue - secondValue;
    } else {
        // Handle unsupported operations or return a default value
        return 0; // You can change this to another default value if needed
    }
}

function updateTotalCost(operation, km) {

    const transporter = document.getElementById("transporter_fee");


    let transporterfee = parseFloat(transporter.value); // Convert to a number if needed

    km = parseFloat(km);


    console.log(`Add ${totalCostField.value}, ${transporterfee}, ${km}`);


    if (isNaN(transporterfee) && isNaN(km)) {

        totalCostField.value = ''; // Clear the field if the inputs are not valid numbers
        return;
    }

    let newTotalCost = transporterfee * km;

    if (totalCostField.value === null || totalCostField.value === undefined || totalCostField.value.trim() === '') {
        totalCostField.value = newTotalCost;
        return;
    }

    //totalCostField.value = parseFloat(totalCostField.value) + newTotalCost;

    //totalCostField.value = sumOrSubtract(add, parseFloat(totalCostField.value), newTotalCost);


    if (operation === add) {
        totalCostField.value = parseFloat(totalCostField.value) + newTotalCost;
    } else if (operation === subtract) {
        totalCostField.value = parseFloat(totalCostField.value) - newTotalCost;
    }
}



function updateDistance(operation, km) {
    km = parseFloat(km);

    let distanceValue = parseFloat(distance.value); // Convert to a number if needed

    console.log(`Distance: ${km}`);
    console.log(`txtDistance: ${distanceValue}`);



    if (distance.value === null || distance.value === undefined || distance.value.trim() === '') {
        console.log(`Is Empty: ${km}`);
        distance.value = km;
        return;
    }

    if (operation === add) {
        distance.value = distanceValue + km;
    } else if (operation === subtract) {
        distance.value = distanceValue - km;
    }
    //distance.value = distanceValue + km;
}


$("#EmployessAvalaible tbody").on("click", "input#btnAddEmployee", function () {

    let data = tableEmployessAvalaible.row($(this).parents("tr")).data();
    let Employee_ID = data[0];
    let Kilometers = (data[3])

    let futureKM = parseFloat(Kilometers) + parseFloat(distance.value)

    if (futureKM > 100) {
        alert("Limite de distancia excedida!");
        return;
    }

    EmployeeTravelToday(Employee_ID)
        .then(res => {
            console.log(res);
        })
        .catch(error => {
            console.error(error);
        });



    AddEmployeeToTravelPromise(Employee_ID)
        .then(response => {

            console.log(response); // Data fetched from https://example.com/api/data

            updateTotalCost(add, Kilometers)

            updateDistance(add, Kilometers)

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
        let TravelDetail = {
            employee_ID: id
        };


        fetch("/Travel/AddEmployeestoTravel", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify({ Employee: id }),
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
    let Kilometers = (data[3])


    console.log(`Remove ${Employee_ID}`);

    RemoveEmployeeToTravelPromise(Employee_ID)
        .then(response => {

            console.log(response); // Data fetched from https://example.com/api/data


            updateTotalCost(subtract, Kilometers)

            updateDistance(subtract, Kilometers)
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


        let TravelDetail = {
            employee_ID: id
        };


        fetch("/Travel/RemoveEmployeestoTravel", {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            body: JSON.stringify({ Employee: id }),
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