// Add event listeners for all the toggle buttons (Dashboard, Workflow, etc.)
const buttons = document.querySelectorAll('.toggle-btn');

function hideAllSections() {
  document.querySelectorAll('.content-section').forEach(section => {
    if (!section.classList.contains('content')) {
      section.classList.remove('show');
      section.style.display = 'none';
    }
  });

  // Hide customWorkflowSteps if it's visible
  const customWorkflowSteps = document.getElementById('customWorkflowSteps');
  if (customWorkflowSteps.classList.contains('show')) {
    customWorkflowSteps.classList.remove('show');
    setTimeout(() => {
      customWorkflowSteps.style.display = 'none';
    }, 300);
  }

  // Hide custom3-container if it's visible
  const custom3Container = document.getElementById('custom3-body');
  if (custom3Container.classList.contains('show')) {
    custom3Container.classList.remove('show');
    setTimeout(() => {
      custom3Container.style.display = 'none';
    }, 300);
  }
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

const parentObserver = new MutationObserver(() => {
  const headerCustom2 = document.querySelector('.header-custom2');
  if (headerCustom2) {
      parentObserver.disconnect(); // Stop observing once the element is found

      const observer = new MutationObserver((mutations) => {
          mutations.forEach((mutation) => {
              if (window.getComputedStyle(headerCustom2).display === 'none') {
                  headerCustom2.style.display = 'flex';
                  console.log('Header was hidden, restored it to flex.');
              }
          });
      });

      observer.observe(headerCustom2, { attributes: true, childList: true, subtree: true });
      console.log("MutationObserver initialized for .header-custom2.");
  }
});

parentObserver.observe(document.body, { childList: true, subtree: true });
