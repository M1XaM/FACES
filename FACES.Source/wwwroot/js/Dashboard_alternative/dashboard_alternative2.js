function toggleMenu(event) {
    const menu = event.target.nextElementSibling;
    menu.style.display = menu.style.display === 'block' ? 'none' : 'block';
}

function editTitle(event) {
    const newTitle = prompt("Enter a new name of the project:");
    if (newTitle) {
        const header = event.target.closest('.custom4-project-header');
        header.childNodes[0].textContent = newTitle; // Изменяем название
    }
}

function editDescription(event) {
    const newDescription = prompt("Enter a new description of the project:");
    if (newDescription) {
        const projectText = event.target.closest('.custom4-project').querySelector('.custom4-project-text');
        projectText.textContent = newDescription; // Изменяем описание
    }
}
