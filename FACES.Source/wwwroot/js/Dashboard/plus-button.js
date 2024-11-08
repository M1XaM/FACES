document.addEventListener("DOMContentLoaded", function () {
    const maxVisibleUsers = 4;

    // Function to update the visibility of Add Button and See More link
    function updateAudienceUI() {
        const audienceList = document.querySelector('.custom-audience-list');
        const audienceItems = audienceList.querySelectorAll('li');
        const plusButton = document.getElementById("plusButton").parentNode;
        const seeMoreLink = document.getElementById("seeMoreLink");


        // If the audience has more than maxVisibleUsers, show "See More" and hide "Add Client" button
        if (audienceItems.length > maxVisibleUsers) {
            plusButton.style.display = "none";
            seeMoreLink.style.display = "block";
        } else {
            plusButton.style.display = "block";
            seeMoreLink.style.display = "none";
        }
    }


    // Add Event Listener for plus button to show options panel
    document.getElementById("plusButton").addEventListener("click", function () {
        document.getElementById("addOptionsPanel").style.display = "block";
        document.getElementById("overlay").style.display = "block";
    });


    // Event listener for entering data manually
    document.getElementById("manualEntryBtn").addEventListener("click", function () {
        document.getElementById("addOptionsPanel").style.display = "none";
        document.getElementById("manualDataEntryPanel").style.display = "block";
    });


    // Event listener for saving new user data manually
    document.getElementById("saveUserBtn").addEventListener("click", function () {
        const avatar = document.getElementById("avatar").value;
        const firstName = document.getElementById("firstName").value;
        const lastName = document.getElementById("lastName").value;
        const email = document.getElementById("email").value;

        if (!firstName || !lastName || !email) {
            alert("Please fill in all fields.");
            return;
        }


        // Create a new list item for the added user
        const audienceList = document.querySelector('.custom-audience-list');
        const newUser = document.createElement('li');
        newUser.innerHTML = `
            <img src="${avatar || 'https://placehold.co/40x40'}" alt="Profile picture of ${firstName} ${lastName}">
            <div>
                <span class="custom-name">${firstName} ${lastName}</span><br>
                <span class="custom-email">${email}</span>
            </div>
        `;

        // Insert the new user before the "Add Client" button (plusButton)
        const plusButton = document.getElementById("plusButton");
        audienceList.insertBefore(newUser, plusButton.parentNode);

        // Reset the form fields and hide the manual data entry panel
        document.getElementById("avatar").value = '';
        document.getElementById("firstName").value = '';
        document.getElementById("lastName").value = '';
        document.getElementById("email").value = '';
        document.getElementById("manualDataEntryPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";


        // Update UI to check whether to show "See More" or "Add Client" button
        updateAudienceUI();
    });

    // Event listener for CSV Upload Submission
    document.getElementById("uploadCSVSubmitBtn").addEventListener("click", function () {
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


                    // Update UI to check whether to show "See More" or "Add Client" button
                    updateAudienceUI();
                }
            }
        });
    });


    // Function to add users from CSV data
    function addUsersFromCSV(data) {
        const audienceList = document.querySelector('.custom-audience-list');
        data.forEach(user => {
            const avatar = user.Avatar || 'https://placehold.co/40x40';
            const firstName = user["First Name"];
            const lastName = user["Last Name"];
            const email = user.Email;

            if (!firstName || !lastName || !email) {
                console.warn("Skipping incomplete user data:", user);
                return;
            }


            // Create a new list item for the added user
            const newUser = document.createElement('li');
            newUser.innerHTML = `
                <img src="${avatar}" alt="Profile picture of ${firstName} ${lastName}">
                <div>
                    <span class="custom-name">${firstName} ${lastName}</span><br>
                    <span class="custom-email">${email}</span>
                </div>
            `;


            // Insert the new user before the "Add Client" button (plusButton)
            const plusButton = document.getElementById("plusButton");
            audienceList.insertBefore(newUser, plusButton.parentNode);
        });
    }


    // Cancel button listeners for CSV Upload and Manual Data Panels
    document.getElementById("cancelUploadCSVBtn").addEventListener("click", function () {
        document.getElementById("uploadCSVPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";
    });

    document.getElementById("cancelAddOptionsBtn").addEventListener("click", function () {
        document.getElementById("addOptionsPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";
    });

    document.getElementById("cancelManualEntryBtn").addEventListener("click", function () {
        document.getElementById("manualDataEntryPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";
    });


    // Initial UI Update to check if "See More" or "Add Client" button should be shown
    updateAudienceUI();
});
