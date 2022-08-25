﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

refresh();

function refresh() {
    deleteAllRows();
    $.get("https://localhost:5002/Birthday/allBirthdays", function (data) {
        console.log('data', data);
        for (let i = 0; i < data.length; i++) {
            addRow(data[i].id, data[i].fullName, data[i].birthDateString, data[i].photoUrl);
        }
    });
}


function deleteBirthday(id){
    $.ajax(
        {
            url: "https://localhost:5002/Birthday/deleteBirthday?id=" + id,
            type: 'DELETE',
        })
        .then(data => {
            refresh();
        });
}

function deleteAllRows(){
    let table = document.getElementById("myTableData");
    let rowCount = table.rows.length;
    var tableHeaderRowCount = 1;
    for (var i = tableHeaderRowCount; i < rowCount; i++) {
        table.deleteRow(tableHeaderRowCount);
    }
}


function addRow(id, name, birthDate, photo) {
    var table = document.getElementById("myTableData");

    var rowCount = table.rows.length;
    var row = table.insertRow(rowCount);

    row.insertCell(0).innerHTML= '<input type="button" id=' + id  + ' value = "Delete" onClick="Javacsript:deleteRow(this)">';
    row.insertCell(1).innerHTML= '<div id =' + id + '>' + id + '</div>';
    row.insertCell(2).innerHTML= name;
    row.insertCell(3).innerHTML= birthDate;
    row.insertCell(4).innerHTML= photo;
}

function deleteRow(obj) {
    let id = $(obj).prop('id');
    deleteBirthday(id);
}

function addTable() {

    var myTableDiv = document.getElementById("myDynamicTable");

    var table = document.createElement('TABLE');
    table.border='1';

    var tableBody = document.createElement('TBODY');
    table.appendChild(tableBody);

    for (var i=0; i<3; i++){
        var tr = document.createElement('TR');
        tableBody.appendChild(tr);

        for (var j=0; j<4; j++){
            var td = document.createElement('TD');
            td.width='75';
            td.appendChild(document.createTextNode("Cell " + i + "," + j));
            tr.appendChild(td);
        }
    }
    myTableDiv.appendChild(table);

}

function load() {

    console.log("Page load finished");

}