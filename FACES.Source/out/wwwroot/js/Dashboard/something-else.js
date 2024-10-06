// Get all the buttons
// Get the current URL path (e.g., 'dashboard.html')
const currentPath = window.location.pathname.split('/').pop();

// Loop through the buttons and add 'active' class if it matches the current page
buttons.forEach(button => {
    if (button.getAttribute('href') === currentPath) {
        button.classList.add('active');
    }
});


document.getElementById('createWorkflowBtn').addEventListener('click', function() {
  const section2 = document.getElementById('section2');
  const workflowBuilder = document.getElementById('workflowBuilder');

  section2.classList.remove('show');
  setTimeout(() => {
    section2.style.display = 'none'; 
  }, 300); 

  workflowBuilder.style.display = 'block'; 
  setTimeout(() => {
    workflowBuilder.classList.add('show'); 
  }, 10); 
});

document.getElementById('on-date-btn').addEventListener('click', function() {
            setActiveButton('on-date-btn');
        });
        document.getElementById('before-date-btn').addEventListener('click', function() {
            setActiveButton('before-date-btn');
        });
        document.getElementById('after-date-btn').addEventListener('click', function() {
            setActiveButton('after-date-btn');
        });

        function setActiveButton(buttonId) {
            document.getElementById('on-date-btn').classList.remove('active');
            document.getElementById('before-date-btn').classList.remove('active');
            document.getElementById('after-date-btn').classList.remove('active');
            document.getElementById(buttonId).classList.add('active');
        }

        flatpickr("#date-picker", {
            enableTime: false,
            dateFormat: "Y-m-d",
        });