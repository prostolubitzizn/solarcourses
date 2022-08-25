// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const host = "https://localhost:5002";
refresh();

function refresh() {
    deleteAllRows();
    $.get(host + "/Birthday/allBirthdays", function (data) {
        for (let i = 0; i < data.length; i++) {
            addRow(data[i].id, data[i].fullName, data[i].birthDateString, data[i].photoUrl);
        }
    });
}


function addBirthday(){
    var table = document.getElementById("myTableData");
    var id = document.getElementById("id");
    var myName = document.getElementById("name");
    var birthDate = document.getElementById("birthDate");
    var photo = document.getElementById("photo");
    
    const data =  {
        fullName: myName.value,
        birthDateString: birthDate.value,
        photoUrl: photo.value,
    }
    
    if(id.value){
        
        data.id = id.value;
        $.ajax(
            {
                url: host + "/Birthday/updateBirthday",
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data)
            })
            .then(data => {
                refresh();
            });
    }
    else{
        $.ajax(
            {
                url: host + "/Birthday/insertBirthday",
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data)
            })
            .then(data => {
                refresh();
            });
    }
    
   
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

function getSoonBirthdays(){
    deleteAllRows();
    $.get(host + "/Birthday/soonBirthday", function (data) {
        for (let i = 0; i < data.length; i++) {
            addRow(data[i].id, data[i].fullName, data[i].birthDateString, data[i].photoUrl);
        }
    });
}

function getTodayBirthdays(){
    deleteAllRows();
    $.get(host + "/Birthday/todayBirthday", function (data) {
        for (let i = 0; i < data.length; i++) {
            addRow(data[i].id, data[i].fullName, data[i].birthDateString, data[i].photoUrl);
        }
    });
}

function getOutDatedBirthdays(){
    deleteAllRows();
    $.get(host + "/Birthday/outdatedBirthday", function (data) {
        for (let i = 0; i < data.length; i++) {
            addRow(data[i].id, data[i].fullName, data[i].birthDateString, data[i].photoUrl);
        }
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
    row.insertCell(4).innerHTML= '<img src=' + photo + ' style="width:300px;height:300px;"> + </img>';
    row.insertCell(5).innerHTML=  '<input id="fileupload-'+ id + '" type="file" name="fileupload" accept="image/png, image/jpeg"/><button id="upload-button" onclick="uploadFile(' + id + ')"> Загрузить </button>'
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


function uploadFile(id) {
    let formData = new FormData();
    const fileUploaderId = "#fileupload-" +id;
    
    const myFiles = $(fileUploaderId).prop('files')

    var form_data = new FormData();
    const newFileName = "id-"+ id + '-' + myFiles[0].name;
    
    form_data.append('file', myFiles[0], newFileName);
    
    $.ajax({
        url: host + "/Birthday/uploadImage",
        type: "POST",
        data: form_data,
        async: false,
        success: function (msg) {
            console.log(msg);
        },
        error: function(msg) {
            console.log('Ошибка!');
        },
        dataType: 'text',
        cache: false,
        contentType: false,
        processData: false,
    })
        .then(() => refresh());
}

