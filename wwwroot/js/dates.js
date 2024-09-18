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
    const customWorkflowSteps = document.getElementById('customWorkflowSteps');
    const custom3Body = document.getElementById('custom3-body');

    // Hide the current workflow steps
    customWorkflowSteps.classList.remove('show');
    setTimeout(() => {
        customWorkflowSteps.style.display = 'none'; // Hide the workflow steps section
    }, 300);

    // Show the next step (custom3Body - Workflow Trigger)
    custom3Body.style.display = 'block'; // Make it visible
    setTimeout(() => {
        custom3Body.classList.add('show'); // Animate opacity to make it fully visible
    }, 10);
});
