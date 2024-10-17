document.querySelectorAll('.dots-button').forEach(button => {
    button.addEventListener('click', function() {
        // Переключаем класс active для изменения стилей кнопки при клике
        this.classList.toggle('active');
        
        const parentAction = this.closest('.action');
        const editContainer = parentAction.querySelector('.edit-container');

        if (!editContainer.classList.contains('show-edit')) {
            editContainer.classList.add('show-edit');
        } else {
            editContainer.classList.remove('show-edit');
        }
    });
});
