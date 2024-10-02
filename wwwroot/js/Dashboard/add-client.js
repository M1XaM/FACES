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

    const token = localStorage.getItem('token')
    const projectId = match[2];
    try {
        const response = await fetch(`/api/v1//project/${projectId}/add-client`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ name: clientName })
        });
        if (!response.ok) {
            throw new Error('Failed to add client');
        }
        const newClient = await response.json(); 
        await fetchClients();
        document.getElementById("client-name").value = "";
    } catch (error) {
        console.error('Error adding client:', error);
    }
}
