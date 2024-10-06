
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

  document.getElementById('saveAndNextBtn').addEventListener('click', function() {
    const workflowBuilder = document.getElementById('workflowBuilder');
    const customWorkflowSteps = document.getElementById('customWorkflowSteps');
    const custom3Body = document.getElementById('custom3-body');
    const headerCustom2 = document.querySelector('.header-custom2');

    // Log header-custom2 visibility status
    console.log('Before change - Header visibility:', window.getComputedStyle(headerCustom2).display);

    // Ensure the header stays visible
    headerCustom2.style.display = 'flex';
    headerCustom2.classList.add('show');

    // Log after applying display and class
    console.log('After change - Header visibility:', window.getComputedStyle(headerCustom2).display);

    // Hide the workflowBuilder form
    workflowBuilder.classList.remove('show');
    setTimeout(() => {
        workflowBuilder.style.display = 'none';
    }, 300);

    // Hide the customWorkflowSteps section
    customWorkflowSteps.style.display = 'none';

    // Show the next section (custom3Body)
    custom3Body.style.display = 'block';
    setTimeout(() => {
      custom3Body.classList.add('show'); // Ensure smooth transition
    }, 10);
});
