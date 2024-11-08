// Add event listeners for all the toggle buttons (Dashboard, Workflow, etc.)
const buttons = document.querySelectorAll('.toggle-btn');

// Function to hide all sections except the .content section
function hideAllSections() {
  document.querySelectorAll('.content-section').forEach(section => {
    // Check if the section does not have the 'content' class to prevent hiding it
    if (!section.classList.contains('content')) {
      section.classList.remove('show');
      section.style.display = 'none'; // Hide all sections except the .content section
    }
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

const headerCustom2 = document.querySelector('.header-custom2');
const observer = new MutationObserver((mutations) => {
    mutations.forEach((mutation) => {
        if (window.getComputedStyle(headerCustom2).display === 'none') {
            headerCustom2.style.display = 'flex';
            console.log('Header was hidden, restored it to flex.');
        }
    });
});
observer.observe(headerCustom2, { attributes: true, childList: true, subtree: true });