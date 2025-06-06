// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function passEmployee(emplName, id) {
    const input_asp_delete = document.getElementById("asp-del-js");
    input_asp_delete.setAttribute('value', `${id}`)
    document.getElementById('id01').style.display = 'block';
    const modal_p = document.querySelector(".container-modal p");
    modal_p.innerText = `Вы уверены, что хотите удалить сотрудника ${emplName}?`;
};

function giveMeLoader() {
    const loader = document.querySelector(".loader");
    const hide_for_loader = document.querySelectorAll(".hide-for-loader");
    hide_for_loader.forEach(item => {
        item.style.display = 'none';
    });
    loader.style.display = "block";
}

function openCity(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}
