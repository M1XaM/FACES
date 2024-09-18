// Step 1: Handle "Create Workflow" button click
document.getElementById('createWorkflowBtn').addEventListener('click', function() {
    const section2 = document.getElementById('section2');
    const workflowBuilder = document.getElementById('workflowBuilder');

    // Debugging: Log elements
    console.log('Create Workflow button clicked');
    console.log('Section 2:', section2);
    console.log('Workflow Builder:', workflowBuilder);

    // Hide section2
    section2.classList.remove('show');
    setTimeout(() => {
        section2.style.display = 'none';
    }, 300);

    // Show workflowBuilder
    workflowBuilder.style.display = 'block';
    setTimeout(() => {
        workflowBuilder.classList.add('show');
    }, 10);
});

// Step 2: Handle form submission (Proceed to the next step)
document.querySelector('.custom2-button').addEventListener('click', function(event) {
    event.preventDefault(); // Prevent default form submission behavior

    const workflowBuilder = document.getElementById('workflowBuilder');
    const customWorkflowSteps = document.getElementById('customWorkflowSteps');

    // Debugging: Log elements
    console.log('Form submitted');
    console.log('Workflow Builder:', workflowBuilder);
    console.log('Custom Workflow Steps:', customWorkflowSteps);

    // Hide the workflowBuilder form
    workflowBuilder.classList.remove('show');
    setTimeout(() => {
        workflowBuilder.style.display = 'none';
    }, 300);

    // Show the customWorkflowSteps section
    customWorkflowSteps.style.display = 'block';
    setTimeout(() => {
        customWorkflowSteps.classList.add('show');
    }, 10);
});
