function MyFunction() {
    document.getElementById("MyDropdown").classList.toggle("show");
}

function MyFunctiontwo() {
    document.getElementById("MyDropdown2").classList.toggle("show");
}

window.onclick = function(event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var dropdowns2 = document.getElementsByClassName("dropdown-content2");
        
        for (var i = 0; i < dropdowns.length; i++) {    
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
        
        for (var j = 0; j < dropdowns2.length; j++) {
            var openDropdown2 = dropdowns2[j];
            if (openDropdown2.classList.contains('show')) {
                openDropdown2.classList.remove('show');
            }
        }
    }
}

document.getElementById("nav__links").addEventListener("click", function(e) {
    const tgt = e.target;
    if (tgt.classList.contains('fa-arrow-up')) { 
      tgt.classList.toggle('open');
    }
  });

