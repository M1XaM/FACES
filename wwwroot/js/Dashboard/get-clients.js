document.addEventListener("DOMContentLoaded", function() {
    // Function to fetch client data from the API
    async function fetchClients() {
        try {
            const url = window.location.href;
            const regex = /\/user\/(\d+)\/project\/(\d+)/;
            const match = url.match(regex);

            if (match) {
                const userId = match[1];
                const projectId = match[2];

                // Sample clients for demonstration
                clients = [
                    { name: 'Olivia Martin', email: 'olivia.martin@email.com' },
                    { name: 'Jackson Lee', email: 'jackson.lee@email.com' },
                    { name: 'Isabella Nguyen Lee', email: 'isabella.nguyen@email.com' },
                    { name: 'William Kim', email: 'will@email.com' },
                    { name: 'Sofia Davis', email: 'sofia.davis@email.com' },
                ];
                displayClients(clients);
                setupAddClientButton(clients.length, userId, projectId); // Set up button based on client count
            } else {
                console.error("Invalid URL");
            }
        } catch (error) {
            console.error('Error fetching clients:', error);
        }
    }

    // Function to display the client data in the dashboard
    function displayClients(clients) {
        const totalAudienceElement = document.querySelector('.custom-value'); // Update Total Audience element
        totalAudienceElement.textContent = clients.length; // Update the total audience count
        const audienceList = document.querySelector('.custom-audience-list');
        audienceList.innerHTML = ''; // Clear existing items

        clients.forEach(client => {
            const listItem = document.createElement('li');
            listItem.innerHTML = `
                <img src="${client.profilePicture || 'https://placehold.co/40x40'}" alt="Profile picture of ${client.name}">
                <div>
                  <div class="custom-name">${client.name}</div>
                  <div class="custom-email">${client.email}</div>
                </div>
            `;
            audienceList.appendChild(listItem);
        });

        // Create and append the "Add Client" button
        // const addClientButton = document.createElement('button');
        // addClientButton.id = 'createAudienceBtn';
        // addClientButton.className = 'custom4-project add-new';
        // addClientButton.textContent = 'Add Client';
        // addClientButton.addEventListener('click', addClient); // Attach click event
        // audienceList.appendChild(addClientButton); // Append button to the list
    }

    // Function to set up the add client button visibility
    function setupAddClientButton(clientCount, userId, projectId) {
        const addClientSection = document.getElementById("add-client-section");
        if (clientCount < 5) { // Change this number as needed
            document.getElementById("add-client-btn").style.display = "inline-block"; // Show button
            document.getElementById("view-all-clients").style.display = "none"; // Hide link
            addClientSection.style.display = "block"; // Show the add client section
        } else {
            document.getElementById("add-client-btn").style.display = "none"; // Hide button
            document.getElementById("view-all-clients").style.display = "inline"; // Show link
        }
    }

    fetchClients();
});
