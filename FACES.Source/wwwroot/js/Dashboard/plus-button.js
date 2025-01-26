document.addEventListener("DOMContentLoaded", function () {
    // Function to show the Add Options Panel
    function showAddOptionsPanel() {
        const addOptionsPanel = document.getElementById("addOptionsPanel");
        const overlay = document.getElementById("overlay");
        if (addOptionsPanel && overlay) {
            addOptionsPanel.style.display = "block";
            overlay.style.display = "block";
        } else {
            console.error("Add Options Panel or Overlay not found.");
        }
    }

    // Function to show the Manual Data Entry Panel
    function showManualDataEntryPanel() {
        const manualDataEntryPanel = document.getElementById("manualDataEntryPanel");
        const addOptionsPanel = document.getElementById("addOptionsPanel");
        if (manualDataEntryPanel && addOptionsPanel) {
            addOptionsPanel.style.display = "none";
            manualDataEntryPanel.style.display = "block";
        } else {
            console.error("Manual Data Entry Panel or Add Options Panel not found.");
        }
    }

    // Function to update the audience UI
    function updateAudienceUI() {
        const audienceList = document.querySelector('.custom-audience-list');
        const plusButton = document.getElementById("plusButton");
        const seeMoreLink = document.getElementById("seeMoreLink");
        if (audienceList && plusButton && seeMoreLink) {
            const audienceItems = audienceList.querySelectorAll('li');
            const maxVisibleUsers = 4;

            if (audienceItems.length > maxVisibleUsers) {
                plusButton.style.display = "none";
                seeMoreLink.style.display = "block";
            } else {
                plusButton.style.display = "block";
                seeMoreLink.style.display = "none";
            }
        } else {
            console.error("Audience List, Plus Button, or See More Link not found.");
        }
    }

    // Function to save new user data manually
    function saveNewUser() {
        const avatar = document.getElementById("avatar")?.value || 'https://placehold.co/40x40';
        const firstName = document.getElementById("firstName")?.value;
        const lastName = document.getElementById("lastName")?.value;
        const email = document.getElementById("email")?.value;

        if (!firstName || !lastName || !email) {
            alert("Please fill in all fields.");
            return;
        }

        const audienceList = document.querySelector('.custom-audience-list');
        const plusButton = document.getElementById("plusButton");
        if (audienceList && plusButton) {
            // Add to custom-new-audience
            const newUser = document.createElement('li');
            newUser.innerHTML = `
                <img src="${avatar}" alt="Profile picture of ${firstName} ${lastName}">
                <div>
                    <span class="custom-name">${firstName} ${lastName}</span><br>
                    <span class="custom-email">${email}</span>
                </div>
            `;
            audienceList.insertBefore(newUser, plusButton.parentNode);
        }

        // Add to custom3-audience
        const custom3AudienceList = document.querySelector('.custom3-select-audience');
        if (custom3AudienceList) {
            const custom3User = document.createElement('div');
            custom3User.innerHTML = `
                <input id="${firstName.toLowerCase()}-${lastName.toLowerCase()}" name="audience-${firstName.toLowerCase()}" type="checkbox" />
                <label for="${firstName.toLowerCase()}-${lastName.toLowerCase()}">
                    <img src="${avatar}" alt="Profile picture of ${firstName} ${lastName}" height="30" width="30">
                    <span>${firstName} ${lastName}</span>
                    <span class="custom-email">(${email})</span>
                </label>
            `;
            custom3AudienceList.appendChild(custom3User);
        }

        // Reset the form fields
        document.getElementById("avatar").value = '';
        document.getElementById("firstName").value = '';
        document.getElementById("lastName").value = '';
        document.getElementById("email").value = '';
        document.getElementById("manualDataEntryPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";

        updateAudienceUI();
    }

    // Event Listeners
    const plusButton = document.getElementById("plusButton");
    if (plusButton) {
        plusButton.addEventListener("click", showAddOptionsPanel);
    } else {
        console.error("Element with ID 'plusButton' not found.");
    }

    const manualEntryBtn = document.getElementById("manualEntryBtn");
    if (manualEntryBtn) {
        manualEntryBtn.addEventListener("click", showManualDataEntryPanel);
    }

    const saveUserBtn = document.getElementById("saveUserBtn");
    if (saveUserBtn) {
        saveUserBtn.addEventListener("click", saveNewUser);
    }

    const cancelManualEntryBtn = document.getElementById("cancelManualEntryBtn");
    if (cancelManualEntryBtn) {
        cancelManualEntryBtn.addEventListener("click", () => {
            document.getElementById("manualDataEntryPanel").style.display = "none";
            document.getElementById("overlay").style.display = "none";
        });
    }

    // Initial UI update
    updateAudienceUI();
});
