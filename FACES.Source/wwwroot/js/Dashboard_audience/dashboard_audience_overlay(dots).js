document.addEventListener('DOMContentLoaded', () => {
    const popupPanel = document.getElementById('popupPanel');
    const overlay = document.getElementById('overlay');
    const closeButton = document.getElementById('closeButton');

    // Открытие панели при нажатии на dots-button
    document.body.addEventListener('click', (event) => {
        if (event.target.classList.contains('dots-button')) {
            // Показываем панель и overlay
            popupPanel.style.display = 'block';
            overlay.style.display = 'block';
        }
    });

    // Закрытие панели по кнопке "Save preferences"
    closeButton.addEventListener('click', () => {
        popupPanel.style.display = 'none';
        overlay.style.display = 'none';
    });

    // Закрытие панели по клику на overlay
    overlay.addEventListener('click', () => {
        popupPanel.style.display = 'none';
        overlay.style.display = 'none';
    });
});
