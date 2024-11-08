document.addEventListener("DOMContentLoaded", function () {
    const maxVisibleUsers = 4;

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

    document.getElementById("plusButton").addEventListener("click", function () {
        document.getElementById("addOptionsPanel").style.display = "block";
        document.getElementById("overlay").style.display = "block";
    });

    document.getElementById("manualEntryBtn").addEventListener("click", function () {
        document.getElementById("addOptionsPanel").style.display = "none";
        document.getElementById("manualDataEntryPanel").style.display = "block";
    });

    document.getElementById("saveUserBtn").addEventListener("click", function () {
        const avatar = document.getElementById("avatar").value;
        const firstName = document.getElementById("firstName").value;
        const lastName = document.getElementById("lastName").value;
        const email = document.getElementById("email").value;

        if (!firstName || !lastName || !email) {
            alert("Please fill in all fields.");
            return;
        }

        const audienceList = document.querySelector('.custom-audience-list');
        const newUser = document.createElement('li');
        newUser.innerHTML = `
            <img src="${avatar || 'https://placehold.co/40x40'}" alt="Profile picture of ${firstName} ${lastName}">
            <div>
                <span class="custom-name">${firstName} ${lastName}</span><br>
                <span class="custom-email">${email}</span>
            </div>
        `;

        const plusButton = document.getElementById("plusButton");
        audienceList.insertBefore(newUser, plusButton.parentNode);

        document.getElementById("avatar").value = '';
        document.getElementById("firstName").value = '';
        document.getElementById("lastName").value = '';
        document.getElementById("email").value = '';
        document.getElementById("manualDataEntryPanel").style.display = "none";
        document.getElementById("overlay").style.display = "none";

        updateAudienceUI();
    });

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

                    updateAudienceUI();
                }
            }
        });
    });

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
        });
    }

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

    updateAudienceUI();
});
