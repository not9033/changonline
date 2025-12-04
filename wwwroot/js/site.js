// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// loginModal
// ส่วนที่ 1: ปุ่มเปิด Modal
var loginBtn = document.getElementById("login-modal");
if (loginBtn) { // เช็คก่อนว่ามีปุ่มไหม
    loginBtn.addEventListener("click", function () {
        var modal = document.getElementById("login-modal-close");
        if (modal) {
            modal.classList.toggle("login-modal-open");
            console.log("click open");
        }
    });
}

// ส่วนที่ 2: ปุ่มปิด Modal
var closeBtn = document.getElementById("modal-close");
if (closeBtn) { // เช็คก่อนว่ามีปุ่มไหม
    closeBtn.addEventListener("click", function () {
        var modal = document.getElementById("login-modal-close");
        if (modal) {
            modal.classList.toggle("login-modal-open");
            console.log("click close");
        }
    });
}



