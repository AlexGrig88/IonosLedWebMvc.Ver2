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

function givMeLoader() {
    const loader = document.querySelector(".loader");
    const calc_result_view = document.querySelectorAll(".calc-result-view");
    calc_result_view.forEach(item => {
        item.style.display = 'none';
    });
    loader.style.display = "block";
}
