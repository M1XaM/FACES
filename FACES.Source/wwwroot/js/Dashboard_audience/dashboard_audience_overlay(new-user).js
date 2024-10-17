document.getElementById('newUserBtn').addEventListener('click', function() {
    document.getElementById('newUserPanel').style.display = 'block';
});

// Логика для скрытия панели при нажатии на Cancel
document.getElementById('cancelUserBtn').addEventListener('click', function() {
    document.getElementById('newUserPanel').style.display = 'none';
});

