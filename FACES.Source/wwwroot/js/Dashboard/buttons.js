// Step 1: Handle "Create Workflow" button click
document.getElementById('createWorkflowBtn').addEventListener('click', function() {
    const section2 = document.getElementById('section2');
    const workflowBuilder = document.getElementById('workflowBuilder');
    section2.classList.remove('show');
    workflowBuilder.classList.add('show');
});

// Step 2: Handle form submission (Proceed to the next step)
document.querySelector('.custom2-button').addEventListener('click', function (event) {
    event.preventDefault(); // Prevent the default form submission behavior

    const workflowBuilder = document.getElementById('workflowBuilder');
    const custom3Body = document.getElementById('custom3-body');

    // Hide the workflowBuilder section
    workflowBuilder.classList.remove('show');
    setTimeout(() => {
        workflowBuilder.style.display = 'none';
    }, 300);

    // Show the custom3Body section
    custom3Body.style.display = 'block';
    setTimeout(() => {
        custom3Body.classList.add('show');
    }, 10);
});
document.querySelector('.custom2-button').addEventListener('click', function (event) {
    event.preventDefault(); // Prevent the default form submission behavior

    const workflowBuilder = document.getElementById('workflowBuilder');
    const custom3Body = document.getElementById('custom3-body');

    // Sync audience data from custom-new-audience to custom3-select-audience
    const newAudienceList = document.querySelectorAll('.custom-audience-list li');
    const custom3SelectAudience = document.querySelector('.custom3-select-audience');
    custom3SelectAudience.innerHTML = '<h3>Select Audience</h3><hr />'; // Clear previous data and reset header

    newAudienceList.forEach((audienceItem, index) => {
        // Skip the "plus button" entry if it exists
        if (audienceItem.querySelector('button')) return;
    
        const avatar = audienceItem.querySelector('img').src;
        const name = audienceItem.querySelector('.custom-name').textContent;
        const email = audienceItem.querySelector('.custom-email').textContent;
    
        // Create audience entry for custom3-select-audience
        const audienceDiv = document.createElement('div');
        audienceDiv.innerHTML = `
            <input id="audience-${index}" name="audience-${index}" type="checkbox" />
            <label for="audience-${index}">
                <img alt="Profile picture of ${name}" src="${avatar}" width="30" height="30" />
                ${name} (${email})
            </label>
        `;
        custom3SelectAudience.appendChild(audienceDiv);
    });

    // Hide the workflowBuilder section
    workflowBuilder.classList.remove('show');
    setTimeout(() => {
        workflowBuilder.style.display = 'none';
    }, 300);

    // Show the custom3Body section
    custom3Body.style.display = 'block';
    setTimeout(() => {
        custom3Body.classList.add('show');
    }, 10);
});
document.getElementById("saveAndSendBtn").addEventListener("click", function () {
    alert("Save & Send functionality triggered!");
    // Add your save and send logic here
});
// Get modal elements
const modal = document.getElementById("emailModal");
const sendEmailButton = document.querySelector(".WriteEmail");
const closeButtons = document.querySelectorAll(".close");
const emailForm = document.getElementById("emailForm");

// Open modal when "Send Email" is clicked
sendEmailButton.addEventListener("click", () => {
  modal.style.display = "block";
});

// Close modal and clear the form when "Cancel" is clicked
closeButtons.forEach(button => {
  button.addEventListener("click", () => {
    modal.style.display = "none";
    emailForm.reset(); // Clear all input fields
  });
});



// Close modal when clicking outside the modal content
window.addEventListener("click", (event) => {
  if (event.target === modal) {
    modal.style.display = "none";
    emailForm.reset(); // Clear all input fields
  }
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

// Get the container for audience options and the add button
const audienceOptions = document.getElementById("audience-options");
const addAudienceBtn = document.getElementById("add-audience-btn");

// Function to add a new audience option
function addAudienceOption() {
  // Prompt the user for audience details
  const audienceName = prompt("Enter audience name:");
  const profilePicture = prompt("Enter URL of profile picture:");

  // Validate input
  if (!audienceName || !profilePicture) {
    alert("Both name and profile picture URL are required!");
    return;
  }

  // Create a new div for the audience option
  const newOption = document.createElement("div");

  // Create the radio input
  const radioInput = document.createElement("input");
  radioInput.type = "radio";
  radioInput.name = "audience";
  radioInput.id = audienceName.toLowerCase().replace(/\s+/g, "-");

  // Create the label
  const label = document.createElement("label");
  label.htmlFor = radioInput.id;

  // Create the profile picture
  const img = document.createElement("img");
  img.src = profilePicture;
  img.alt = `Profile picture of ${audienceName}`;
  img.width = 30;
  img.height = 30;

  // Add text to the label
  label.appendChild(img);
  label.appendChild(document.createTextNode(` ${audienceName}`));

  // Append the radio input and label to the new div
  newOption.appendChild(radioInput);
  newOption.appendChild(label);

  // Add the new option to the container
  audienceOptions.appendChild(newOption);
}

