
document.addEventListener("DOMContentLoaded", function () {



});

document.getElementById("subsidiary_ID").addEventListener("change", function () {

    console.log("Entro Can");
    let subsidiary_ID = document.getElementById("subsidiary_ID").value;

    console.log(subsidiary_ID);
    GetAddressPromise(subsidiary_ID)
        .then(response => {
            document.getElementById("subsidiary_Name").value = response.subsidiary_Direction
            console.log(result); // Data fetched from https://example.com/api/data
        })
        .catch(error => {
            console.error(error); // Request timed out
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