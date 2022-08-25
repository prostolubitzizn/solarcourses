// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

console.log("Hello world!");

$.get( "https://localhost:5002/Birthday/allBirthdays", function( data ) {
    console.log("data", data);
    alert( "Load was performed." );
});