document.addEventListener("DOMContentLoaded", function () {
    const custom3PlusButton = document.getElementById("custom3PlusButton");

    if (custom3PlusButton) {
        custom3PlusButton.addEventListener("click", function () {
            const addOptionsPanel = document.getElementById("addOptionsPanel");
            const overlay = document.getElementById("overlay");

            if (addOptionsPanel && overlay) {
                // Show the Add Options Panel and overlay
                addOptionsPanel.style.display = "block";
                overlay.style.display = "block";
                console.log("Custom3 Plus Button clicked. Showing Add Options Panel.");
            } else {
                console.error("Add Options Panel or Overlay not found.");
            }
        });
    } else {
        console.error("Element with ID 'custom3PlusButton' not found.");
    }
});
document.addEventListener("DOMContentLoaded", () => {
    const addOptionsPanel = document.getElementById("addOptionsPanel");
    if (addOptionsPanel) {
        // Create the cancel button dynamically if it doesn't exist
        if (!document.getElementById("cancelAddOptionsBtn")) {
            const cancelBtn = document.createElement("button");
            cancelBtn.id = "cancelAddOptionsBtn";
            cancelBtn.className = "cancel-button";
            cancelBtn.textContent = "Cancel";
            addOptionsPanel.appendChild(cancelBtn);
        }

        // Add event listener to the cancel button
        const cancelButton = document.getElementById("cancelAddOptionsBtn");
        cancelButton.addEventListener("click", () => {
            addOptionsPanel.style.display = "none";
            document.getElementById("overlay").style.display = "none";
            console.log("Cancel button clicked.");
        });
    }
});
