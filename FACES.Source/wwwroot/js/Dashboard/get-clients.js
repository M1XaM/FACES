document.addEventListener("DOMContentLoaded", function() {
    async function fetchClients() {
        try {
            const url = window.location.href;
            const regex = /\/user\/(\d+)\/project\/(\d+)/;
            const match = url.match(regex);

            if (match) {
                const userId = match[1];
                const projectId = match[2];

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

    function displayClients(clients) {
        const totalAudienceElement = document.querySelector('.custom-value');
        totalAudienceElement.textContent = clients.length;
        const audienceList = document.querySelector('.custom-audience-list');
        audienceList.innerHTML = '';

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

    function setupAddClientButton(clientCount, userId, projectId) {
        const addClientSection = document.getElementById("add-client-section");
        if (clientCount < 5) { 
            document.getElementById("add-client-btn").style.display = "inline-block"; 
            document.getElementById("view-all-clients").style.display = "none"; 
            addClientSection.style.display = "block"; 
        } else {
            document.getElementById("add-client-btn").style.display = "none"; 
            document.getElementById("view-all-clients").style.display = "inline"; 
        }
    }

    fetchClients();
});
