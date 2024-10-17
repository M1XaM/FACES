// Function to create project blocks dynamically
async function generateProjectBlocks() {
    const container = document.querySelector('.custom4-container');
    // Clear existing content (if any)
    container.innerHTML = '';

    try {
        const token = localStorage.getItem('token');
        console.log(token);
        const response = await fetch(`/api/v1/get-user-projects`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const projectsData = await response.json();
        console.log("Project data:", projectsData);

        // Loop through the project data and create project blocks
        projectsData.projects.forEach(project => {
            const projectBlock = document.createElement('a');
            projectBlock.classList.add('custom4-project');
            projectBlock.href = `/project/${project.name}`;
            
            const projectHeader = document.createElement('div');
            projectHeader.classList.add('custom4-project-header');
            projectHeader.textContent = project.name;
            projectBlock.appendChild(projectHeader);
            
            const divider = document.createElement('div');
            divider.classList.add('custom4-divider');
            projectBlock.appendChild(divider);
            
            const projectText = document.createElement('div');
            projectText.classList.add('custom4-project-text');
            projectText.textContent = project.description;
            projectBlock.appendChild(projectText);
            
            container.appendChild(projectBlock);
        });
    } catch (error) {
        console.error('Error fetching project data:', error);
        // Optionally, display an error message to the user
        const errorMessage = document.createElement('div');
        errorMessage.textContent = 'Error loading projects. Please try again later.';
        container.appendChild(errorMessage);
    }
    
    // Add 'Add New Project' block at the end
    const addNewProjectBlock = document.createElement('div');
    addNewProjectBlock.classList.add('custom4-project', 'add-new');
    addNewProjectBlock.id = 'addNewProject';
    
    const plusIcon = document.createElement('div');
    plusIcon.classList.add('plus-icon');
    plusIcon.textContent = '+';
    addNewProjectBlock.appendChild(plusIcon);
    
    const addNewText = document.createElement('div');
    addNewText.classList.add('custom4-project-text');
    addNewText.textContent = 'Add New Project';
    addNewProjectBlock.appendChild(addNewText);
    
    container.appendChild(addNewProjectBlock);
    
    // Re-attach event listener for "Add New Project" block after it's created
    setupAddNewProjectListener();
}

async function setupAddNewProjectListener() {
    const addNewProjectBlock = document.getElementById('addNewProject');
    if (addNewProjectBlock) {
        addNewProjectBlock.addEventListener('click', async function() {
            let projectName = prompt("Enter the new project name:");
            let projectDescription = prompt("Enter the project description:");

            if (projectName && projectDescription) {
                try {
                    const token = localStorage.getItem('token')
                    const response = await fetch(`/api/v1/create-project`, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': `Bearer ${token}`

                        },
                        body: JSON.stringify({ Name: projectName, Description: projectDescription })
                    });
                    
                    console.log(response);
                    if (!response.ok) {
                        throw new Error('Failed to create project: ' + response.statusText);
                    }

                    const data = await response.json(); // Get the newly created project data (including ID)
                    const newProjectName = data.projectName; // Assuming the server responds with the new project ID

                    // Create the new project block with a link to the project details page
                    let newProject = document.createElement('a');
                    newProject.classList.add('custom4-project');
                    newProject.href = `/project/${projectName}`; // Set the URL for the new project
                    newProject.innerHTML = `
                        <div class="custom4-project-header">${projectName}</div>
                        <div class="custom4-divider"></div>
                        <div class="custom4-project-text">${projectDescription}</div>
                    `;

                    // Insert the new project before the 'Add New Project' block
                    document.querySelector('.custom4-container').insertBefore(newProject, addNewProjectBlock);
                } catch (error) {
                    alert("Error creating project: " + error.message);
                    console.error('Error creating project:', error);
                }
            }
        });
    }
}


// Menu Button Logic
const menuButtons = document.querySelectorAll('.menu-btn');

// Function to remove active class from all buttons
function deactivateAllButtons() {
    menuButtons.forEach(button => {
        button.classList.remove('active');
    });
}

// Function to initialize the state
function initialize() {
    // Deactivate all buttons initially
    deactivateAllButtons();
    const dashboardButton = menuButtons[0]; // The first button, e.g., "Dashboard"
    dashboardButton.classList.add('active');
    showPanel('DashboardPanel'); // Show the Dashboard panel by default
}

// Event listeners for menu buttons
menuButtons.forEach((button, index) => {
    button.addEventListener('click', () => {
        // Deactivate all buttons
        deactivateAllButtons();
        // Activate the clicked button
        button.classList.add('active');
    });
});

// Call the function to generate project blocks and initialize the menu on page load
window.onload = function() {
    generateProjectBlocks();  // Generate project blocks from API
    // initialize();             // Initialize the menu buttons
};

// Helper function to show the active panel (if necessary)
function showPanel(panelId) {
    // Logic to show the appropriate panel (e.g., by changing the visibility of different sections)
    console.log("Showing panel:", panelId);
}
