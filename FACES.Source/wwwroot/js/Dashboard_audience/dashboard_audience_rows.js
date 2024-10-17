document.addEventListener('DOMContentLoaded', () => {
    const saveUserBtn = document.getElementById('saveUserBtn');
    const newUserPanel = document.getElementById('newUserPanel');
    const userTableContent = document.querySelector('.user-menu');

    saveUserBtn.addEventListener('click', () => {
        const firstName = document.getElementById('FirstName').value;
        const lastName = document.getElementById('LastName').value;
        const email = document.getElementById('email').value;

        if (!firstName || !lastName || !email) {
            alert('Please fill in all fields');
            return;
        }

        const userRow = document.createElement('div');
        userRow.classList.add('row');

        // Динамически добавляем HTML-код для нового пользователя
        userRow.innerHTML = `
            <div class="user-avatar">
                <img src="~/images/Avatar Image.png" alt="User Avatar">
            </div>
            <div class="full-name">${firstName} ${lastName}</div>
            <div class="email">${email}</div>
            <div class="header action">
                <button class="dots-button">•••</button>
                <div class="edit-container" style="display: none;">
                    <button class="edit-button">Edit</button>
                </div>
            </div>
        `;

        // Добавляем новую строку в таблицу пользователей
        userTableContent.appendChild(userRow);

        // Сброс полей формы
        document.getElementById('FirstName').value = '';
        document.getElementById('LastName').value = '';
        document.getElementById('email').value = '';
        newUserPanel.style.display = 'none';
    });

    // Делегирование событий для динамически добавленных элементов
    userTableContent.addEventListener('click', (event) => {
        // Проверка на клик по кнопке с тремя точками
        if (event.target.classList.contains('dots-button')) {
            const editContainer = event.target.nextElementSibling;
            // Переключаем видимость меню редактирования
            editContainer.style.display = editContainer.style.display === 'none' ? 'block' : 'none';
        }

        // Проверка на клик по кнопке "Edit"
        if (event.target.classList.contains('edit-button')) {
            const userRow = event.target.closest('.row');
            const fullName = userRow.querySelector('.full-name').textContent;
            alert(`Editing user: ${fullName}`);
        }
    });
});
