// Add event listeners for all the toggle buttons (Dashboard, Workflow, etc.)
const buttons = document.querySelectorAll('.toggle-btn');

// Function to hide all sections
function hideAllSections() {
  document.querySelectorAll('.content-section').forEach(section => {
    section.classList.remove('show');
    section.style.display = 'none'; // Hide all sections
  });
}

// Loop through all buttons to add the click event
buttons.forEach(button => {
  button.addEventListener('click', function() {
    // Hide all sections first
    hideAllSections();

    // Remove active class from all buttons
    buttons.forEach(btn => btn.classList.remove('active'));

    // Add active class to the clicked button
    this.classList.add('active');

    // Show the relevant section based on button ID
    if (this.id === 'btn1') {
      // Dashboard button clicked
      const dashboardSection = document.getElementById('section1');
      dashboardSection.style.display = 'block';
      setTimeout(() => {
        dashboardSection.classList.add('show');
      }, 10);
    } else if (this.id === 'btn2') {
      // Workflow button clicked
      const workflowSection = document.getElementById('section2');
      workflowSection.style.display = 'block';
      setTimeout(() => {
        workflowSection.classList.add('show');
      }, 10);
    }
  });
});

