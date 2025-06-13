// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    var menuBtn = document.getElementById('menuToggleBtn');
    var mainContent = document.getElementById('mainContent');
    var sidebarMenu = document.getElementById('sidebarMenu');

    if (menuBtn && mainContent && sidebarMenu) {
        menuBtn.addEventListener('click', function () {
            // Wait for the sidebar to finish toggling (Bootstrap collapse is animated)
            setTimeout(function () {
                var isCollapsed = !sidebarMenu.classList.contains('show');
                if (!isCollapsed) {
                    mainContent.style.display = 'none';
                } else {
                    mainContent.style.display = '';
                }
            }, 350); // Bootstrap default collapse transition is 300ms
        });

        // Also handle sidebar collapse events (in case user clicks outside)
        sidebarMenu.addEventListener('shown.bs.collapse', function () {
            mainContent.style.display = 'none';
        });
        sidebarMenu.addEventListener('hidden.bs.collapse', function () {
            mainContent.style.display = '';
        });
    }
});

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
