
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

    // Hide workflowBuilder and customWorkflowSteps with a transition
    workflowBuilder.classList.add('hide');
    customWorkflowSteps.classList.add('hide');
    setTimeout(() => {
        workflowBuilder.style.display = 'none';
        customWorkflowSteps.style.display = 'none';
    }, 300);

    // Ensure header-custom2 stays visible
    headerCustom2.style.display = 'flex';
    
    // Show custom3Body with a transition
    custom3Body.style.display = 'block';
    setTimeout(() => {
        custom3Body.classList.add('show');
    }, 10);
});


