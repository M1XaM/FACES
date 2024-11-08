// Step 1: Handle "Create Workflow" button click
document.getElementById('createWorkflowBtn').addEventListener('click', function() {
    const section2 = document.getElementById('section2');
    const workflowBuilder = document.getElementById('workflowBuilder');
    section2.classList.remove('show');
    workflowBuilder.classList.add('show');
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

// Get all project links
const projectLinks = document.querySelectorAll('.project-link');

// Add click event listener to each project link
projectLinks.forEach(link => {
    link.addEventListener('click', function(event) {
        event.preventDefault(); // Prevent the default link behavior

        // Hide the project section (item4)
        const item4 = document.querySelector('.item4');
        item4.classList.remove('show');
        setTimeout(() => {
            item4.style.display = 'none';
        }, 300);

        // Show the dashboard section (section1)
        const dashboardSection = document.getElementById('section1');
        dashboardSection.style.display = 'block';
        setTimeout(() => {
            dashboardSection.classList.add('show');
        }, 10);
    });
});