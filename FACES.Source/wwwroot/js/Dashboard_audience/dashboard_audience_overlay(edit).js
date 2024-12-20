// Получаем элементы
const editButtons = document.querySelectorAll('.edit-button');
const overlay = document.getElementById('overlay');
const addUserPanel = document.getElementById('addUserPanel');
const closeButton = document.getElementById('closeButton');

// Функция для открытия панели
function openPanel() {
    overlay.style.display = 'block';  // Показать затемнение
    addUserPanel.style.display = 'block'; // Показать панель
}

// Функция для закрытия панели
function closePanel() {
    overlay.style.display = 'none';   // Скрыть затемнение
    addUserPanel.style.display = 'none'; // Скрыть панель
}

// Назначаем обработчики событий для кнопок "Edit"
editButtons.forEach(button => {
    button.addEventListener('click', openPanel);
});

// Назначаем обработчики событий для кнопок "Закрыть"
closeButton.addEventListener('click', closePanel);

// Закрытие при нажатии на затемнённый фон
overlay.addEventListener('click', closePanel);
