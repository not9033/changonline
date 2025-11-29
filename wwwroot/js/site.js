// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// loginModal
document.getElementById("login-modal").addEventListener("click", function () {
        document.getElementById("login-modal-close").classList.toggle("login-modal-open");
        console.log("click");
    });

document.getElementById("modal-close").addEventListener("click", function () {
    document.getElementById("login-modal-close").classList.toggle("login-modal-open");
});




