// Получаем ссылки на кнопки меню и панели контента
const menuButtons = document.querySelectorAll('.menu-btn');
const buttonContainer = document.querySelector('.button-container'); // контейнер кнопок


// Функция для удаления активного класса со всех кнопок
function deactivateAllButtons() {
    menuButtons.forEach(button => {
        button.classList.remove('active');
    });
}

// Устанавливаем начальное состояние
function initialize() {
    // Устанавливаем активную панель и кнопку для "Dashboard"
    deactivateAllButtons();
    const dashboardButton = menuButtons[0]; // Кнопка "Dashboard"
    dashboardButton.classList.add('active');
    showPanel('DashboardPanel'); // Изначально показываем панель "Dashboard"
}

// Добавляем обработчики событий для кнопок
menuButtons.forEach((button, index) => {
    button.addEventListener('click', () => {
        // Удаляем активный класс со всех кнопок
        deactivateAllButtons();
        // Добавляем активный класс к нажатой кнопке
        button.classList.add('active');

        
    });
});

// Инициализируем страницу
initialize();
