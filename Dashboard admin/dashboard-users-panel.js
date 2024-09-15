// Получаем ссылки на кнопки меню и панели контента
const menuButtons = document.querySelectorAll('.menu-btn');
const contentPanels = document.querySelectorAll('.content-panel');

// Функция для отображения соответствующей панели
function showPanel(panelId) {
    contentPanels.forEach(panel => {
        if (panel.id === panelId) {
            panel.classList.add('active'); // Добавляем класс для отображения панели
        } else {
            panel.classList.remove('active'); // Убираем класс с остальных панелей
        }
    });
}

// Функция для удаления активного класса со всех кнопок
function deactivateAllButtons() {
    menuButtons.forEach(button => {
        button.classList.remove('active');
    });
}

// Устанавливаем начальное состояние
function initialize() {
    // Устанавливаем активную панель и кнопку для "Users"
    deactivateAllButtons();
    const userButton = menuButtons[0]; // Кнопка "Users"
    userButton.classList.add('active');
    showPanel('userPanel'); // Изначально показываем панель "Users"
}

// Добавляем обработчики событий для кнопок
menuButtons.forEach((button, index) => {
    button.addEventListener('click', () => {
        // Удаляем активный класс со всех кнопок
        deactivateAllButtons();
        // Добавляем активный класс к нажатой кнопке
        button.classList.add('active');

        // Отображаем соответствующую панель
        switch (index) {
            case 0:
                showPanel('userPanel'); // Users
                break;
            case 1:
                showPanel('emailTemplatesPanel'); // Email Templates
                break;
            case 2:
                showPanel('cardsTemplatePanel'); // Cards Template
                break;
            case 3:
                showPanel('productsPanel'); // Products
                break;
            case 4:
                showPanel('settingsPanel'); // Products
                break;
            case 5:
                showPanel('helpPanel'); // Products
                break;
        }
    });
});

// Инициализируем страницу
initialize();
