document.getElementById('addNewProject').addEventListener('click', function() {
    let projectName = prompt("Enter the new project name:");
    let projectDescription = prompt("Enter the project description:");

    if (projectName && projectDescription) {
        let newProject = document.createElement('div');
        newProject.classList.add('custom4-project');
        
        newProject.innerHTML = `
            <div class="custom4-project-header">
                <span class="project-name">${projectName}</span>
                <button class="three-dots" onclick="toggleMenu(event)">⋮</button>
                <div class="menu-dots" style="display: none;">
                    <button onclick="editTitle(event)">Edit name</button>
                    <button onclick="editDescription(event)">Edit description</button>
                </div>
            </div>
            <div class="custom4-divider"></div>
            <div class="custom4-project-text">${projectDescription}</div>
        `;

        // Добавить новую карточку перед "Add New Project" рамкой
        document.querySelector('.custom4-container').insertBefore(newProject, document.getElementById('addNewProject'));
    }
});

function toggleMenu(event) {
    // Закрываем все открытые меню, чтобы не было нескольких открытых одновременно
    closeAllMenus();

    // Открываем текущее меню
    const menu = event.target.nextElementSibling;
    menu.style.display = 'block';

    // Останавливаем распространение события клика, чтобы клик по кнопке не закрыл меню
    event.stopPropagation();
}

// Закрытие всех меню
function closeAllMenus() {
    const allMenus = document.querySelectorAll('.menu-dots');
    allMenus.forEach(menu => {
        menu.style.display = 'none';
    });
}

// Обработка клика в любое место на странице для закрытия меню
document.addEventListener('click', function() {
    closeAllMenus();
});

// Предотвращаем закрытие меню, если клик произошёл внутри самого меню
document.addEventListener('click', function(event) {
    if (event.target.closest('.menu-dots') || event.target.closest('.three-dots')) {
        event.stopPropagation();
    }
});

function editTitle(event) {
    const newTitle = prompt("Enter a new name of the project:");
    if (newTitle) {
        const header = event.target.closest('.custom4-project-header').querySelector('.project-name');
        header.textContent = newTitle; // Изменяем название
    }
}

function editDescription(event) {
    const newDescription = prompt("Enter a new description of the project:");
    if (newDescription) {
        const projectText = event.target.closest('.custom4-project').querySelector('.custom4-project-text');
        projectText.textContent = newDescription; // Изменяем описание
    }
}
