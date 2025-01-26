document.addEventListener("DOMContentLoaded", function () {
    // Add event listener for the custom3PlusButton
    const custom3PlusButton = document.getElementById("custom3PlusButton");
    
    if (custom3PlusButton) {
        custom3PlusButton.addEventListener("click", function () {
            document.getElementById("addOptionsPanel").style.display = "block";
            document.getElementById("overlay").style.display = "block";
        });
    }

    // Other existing code...
});

document.addEventListener("DOMContentLoaded", function () {
    const maxVisibleUsers = 4;

    // Updates the visibility of the Add Button and See More link
    function updateAudienceUI() {
        const audienceList = document.querySelector('.custom-audience-list');
        const audienceItems = audienceList.querySelectorAll('li');
        const plusButton = document.getElementById("plusButton").parentNode;
        const seeMoreLink = document.getElementById("seeMoreLink");

        if (audienceItems.length > maxVisibleUsers) {
            plusButton.style.display = "none";
            seeMoreLink.style.display = "block";
        } else {
            plusButton.style.display = "block";
            seeMoreLink.style.display = "none";
        }
    }

    // Show the Add Options Panel
    function showAddOptionsPanel() {
        document.getElementById("addOptionsPanel").style.display = "block";
        document.getElementById("overlay").style.display = "block";
    }

    // Show the Manual Data Entry Panel
    function showManualDataEntryPanel() {
        document.getElementById("addOptionsPanel").style.display = "none";
        document.getElementById("manualDataEntryPanel").style.display = "block";
    }

    // Save new user data manually
    function saveNewUser() {
        const avatar = document.getElementById("avatar").value || 'https://placehold.co/40x40';
        const firstName = document.getElementById("firstName").value;
        const lastName = document.getElementById("lastName").value;
        const email = document.getElementById("email").value;
    
        if (!firstName || !lastName || !email) {
            alert("Please fill in all fields.");
            return;
        }
    
        // Add to custom-new-audience
        const audienceList = document.querySelector('.custom-audience-list');
        const newUser = document.createElement('li');
        newUser.innerHTML = `
            <img src="${avatar}" alt="Profile picture of ${firstName} ${lastName}">
            <div>
                <span class="custom-name">${firstName} ${lastName}</span><br>
                <span class="custom-email">${email}</span>
            </div>
        `;
        const plusButton = document.getElementById("plusButton");
        audienceList.insertBefore(newUser, plusButton.parentNode);
    
        // Add to custom3-audience immediately
        const custom3AudienceList = document.querySelector('.custom3-select-audience');
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
    
        // Reset the form fields
        document.getElementById("avatar").value = '';
        document.getElementById("firstName").value = '';
        document.getElementById("lastName").value = '';
        document.getElementById("email").value = '';
        document.getElementById("manualDataEntryPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";
    
        updateAudienceUI();
    }
    
    // Process and add users from CSV data
    function addUsersFromCSV(data) {
        const customNewAudienceList = document.querySelector('.custom-audience-list');
        const custom3AudienceList = document.querySelector('.custom3-select-audience');
    
        data.forEach(user => {
            const avatar = user.Avatar || 'https://placehold.co/40x40';
            const firstName = user["First Name"];
            const lastName = user["Last Name"];
            const email = user.Email;
    
            if (!firstName || !lastName || !email) {
                console.warn("Skipping incomplete user data:", user);
                return;
            }
    
            // Add to `custom-new-audience`
            const newAudienceItem = document.createElement('li');
            newAudienceItem.innerHTML = `
                <img src="${avatar}" alt="Profile picture of ${firstName} ${lastName}">
                <div>
                    <span class="custom-name">${firstName} ${lastName}</span><br>
                    <span class="custom-email">${email}</span>
                </div>
            `;
            const plusButton = document.getElementById("plusButton");
            customNewAudienceList.insertBefore(newAudienceItem, plusButton.parentNode);
    
            // Add to `custom3-audience`
            const newCustom3AudienceItem = document.createElement('div');
            newCustom3AudienceItem.innerHTML = `
                <input id="${firstName.toLowerCase()}-${lastName.toLowerCase()}" name="audience" type="radio" />
                <label for="${firstName.toLowerCase()}-${lastName.toLowerCase()}">
                    <img src="${avatar}" alt="Profile picture of ${firstName} ${lastName}" height="30" width="30">
                    ${firstName} ${lastName} (${email})
                </label>
            `;
            custom3AudienceList.appendChild(newCustom3AudienceItem);
        });
    
        // Update both UIs
        updateAudienceUI();
        updateAudienceUIForCustom3();
    }

    // Process CSV file upload
    function handleCSVUpload() {
        const csvFileInput = document.getElementById("csvFileInput").files[0];
        if (!csvFileInput) {
            alert("Please select a CSV file.");
            return;
        }

        Papa.parse(csvFileInput, {
            header: true,
            skipEmptyLines: true,
            complete: function (results) {
                if (results.errors.length) {
                    console.error("Errors in CSV file:", results.errors);
                    alert("There was an error processing the CSV file. Please check the console for details.");
                } else {
                    addUsersFromCSV(results.data);
                    document.getElementById("uploadCSVPanel").style.display = "none";
                    document.getElementById("overlay").style.display = "none";
                    updateAudienceUI();
                }
            }
        });
    }

    // Attach event listeners
    document.getElementById("plusButton").addEventListener("click", showAddOptionsPanel);
    document.getElementById("manualEntryBtn").addEventListener("click", showManualDataEntryPanel);
    document.getElementById("saveUserBtn").addEventListener("click", saveNewUser);
    document.getElementById("uploadCSVSubmitBtn").addEventListener("click", handleCSVUpload);

    // Cancel buttons
    document.getElementById("cancelUploadCSVBtn").addEventListener("click", () => {
        document.getElementById("uploadCSVPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";
    });
    document.getElementById("cancelAddOptionsBtn").addEventListener("click", () => {
        document.getElementById("addOptionsPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";
    });
    document.getElementById("cancelManualEntryBtn").addEventListener("click", () => {
        document.getElementById("manualDataEntryPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";
    });

    // Initial UI update
    updateAudienceUI();
});
