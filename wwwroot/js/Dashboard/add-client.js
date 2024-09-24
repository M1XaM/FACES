document.addEventListener("DOMContentLoaded", function () {
    const addClientButton = document.getElementById("createAudienceBtn");
    if (addClientButton) {
        
        addClientButton.addEventListener('click', addClient); // Attach click event
    }
});

async function addClient() {
    const clientName = document.getElementById("client-name").value; // Get the client name from input
    if (!clientName) {
        alert("Client name is required.");
        return;
    }

    const url = window.location.href;
    const regex = /\/user\/(\d+)\/project\/(\d+)/;
    const match = url.match(regex);

    if (match) {
        const userId = match[1];
        const projectId = match[2];

        try {
            const response = await fetch(`/api/v1/user/${userId}/project/${projectId}/add-client`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ name: clientName }) // Send client name to the server
            });

            if (!response.ok) {
                throw new Error('Failed to add client');
            }

            const newClient = await response.json(); // Assuming the response returns the newly created client
            await fetchClients(); // Fetch the updated client list again
            document.getElementById("client-name").value = ""; // Clear input field
        } catch (error) {
            console.error('Error adding client:', error);
        }
    }
}
