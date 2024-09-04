function toggleDropdown(dropdownId, iconSelector) {
    const dropdown = document.getElementById(dropdownId);
    const icon = document.querySelector(iconSelector);

    dropdown.classList.toggle("show");

    if (dropdown.classList.contains("show")) {
        icon.classList.add("rotate");
    } else {
        icon.classList.remove("rotate");
    }
}

function MyFunction() {
    toggleDropdown("MyDropdown", ".dropdown .fa-arrow-up");
}

function MyFunctiontwo() {
    toggleDropdown("MyDropdown2", ".dropdown2 .fa-arrow-up");

}

window.onclick = function(event) {
    if (!event.target.matches('.dropbtn') && !event.target.matches('.fa-arrow-up')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var icons = document.querySelectorAll(".dropdown .fa-arrow-up");

        for (var i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
                icons[i].classList.remove("rotate");
            }
        }

        var dropdowns2 = document.getElementsByClassName("dropdown-content2");
        for (var j = 0; j < dropdowns2.length; j++) {
            var openDropdown2 = dropdowns2[j];
            if (openDropdown2.classList.contains('show')) {
                openDropdown2.classList.remove('show');
            }
        }
    }
}
