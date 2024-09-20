const users = [
  { avatar: 'avatar1.png', fullName: 'John Doe', email: 'john@example.com' },
  { avatar: 'avatar2.png', fullName: 'Adam Zicker', email: 'adam@example.com' },
  { avatar: 'avatar2.png', fullName: 'Boris Bug', email: 'boris1970@example.com'},
  { avatar: 'avatar2.png', fullName: 'wqr qwer', email: 'bowerqwer@example.com'}
];

function addUserRows() {
  const tableContent = document.getElementById('user-table-content');
  
  users.forEach(user => {
      // Avatar
      const avatarDiv = document.createElement('div');
      avatarDiv.className = 'avatar';
      avatarDiv.innerHTML = `<img src="${user.avatar}" alt="User Avatar">`;
      
      // Full Name
      const nameDiv = document.createElement('div');
      nameDiv.className = 'full-name';
      nameDiv.textContent = user.fullName;
      
      // Email
      const emailDiv = document.createElement('div');
      emailDiv.className = 'email';
      emailDiv.textContent = user.email;
      
      // Add to table
      tableContent.appendChild(avatarDiv);
      tableContent.appendChild(nameDiv);
      tableContent.appendChild(emailDiv);
  });
}

addUserRows();


