// Updated get-clients.js
document.addEventListener("DOMContentLoaded", function() {
    async function fetchClients() {
        try {
            const url = window.location.href;
            const regex = /\/user\/(\d+)\/project\/(\d+)/;
            const match = url.match(regex);

            if (match) {
                const userId = match[1];
                const projectId = match[2];

                // Empty the hardcoded list
                const clients = []; // No predefined data

                displayClients(clients);
                syncAudienceLists(clients);
                setupAddClientButton(clients.length, userId, projectId);
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
    }

    function syncAudienceLists(clients) {
        const custom3AudienceList = document.querySelector('.custom3-select-audience');
        custom3AudienceList.innerHTML = '<h3>Select Audience</h3><hr />';

        clients.forEach((client, index) => {
            const audienceDiv = document.createElement('div');
            audienceDiv.innerHTML = `
                <input id="audience-${index}" name="audience" type="radio" />
                <label for="audience-${index}">
                    <img alt="Profile picture of ${client.name}" src="${client.profilePicture || 'https://placehold.co/40x40'}" width="30" height="30" />
                    ${client.name} (${client.email})
                </label>
            `;
            custom3AudienceList.appendChild(audienceDiv);
        });
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