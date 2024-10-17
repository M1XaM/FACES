document.getElementById("plusButton").addEventListener("click", function() {
    document.getElementById("addUserPanel").style.display = "block";  // Показать панель
    document.getElementById("overlay").style.display = "block";  // Показать затемнение
});

document.getElementById("cancelButton").addEventListener("click", function() {
    document.getElementById("addUserPanel").style.display = "none";  // Скрыть панель
    document.getElementById("overlay").style.display = "none";  // Скрыть затемнение
});
