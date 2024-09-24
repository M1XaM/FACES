document.getElementById('loginButton').addEventListener('click', function(event) {
    event.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const data = {
        Email: email,
        Password: password
    };
    console.log("Data being sent:", JSON.stringify(data));

    fetch('/login-post', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        body: JSON.stringify(data)
    })
    .then(response => {
        console.log("Response Status:", response.status);  // Logs the status (e.g., 200, 400, 500)
        console.log("Response OK:", response.ok);  // Logs whether the response was successful (response.ok)
        console.log("Full Response:", response);  // Logs the entire response object

        if (!response.ok) {
            // Print the full response text (e.g., for a server error or bad request)
            return response.text().then(text => {
                console.error('Response Error Text:', text);
                throw new Error('Network response was not ok: ' + text);
            });
        }

        return response.text();  // Use text() instead of json() initially
    })
    .then(text => {
        // Try to parse JSON only if the text isn't empty
        if (text) {
            return JSON.parse(text);
        } else {
            throw new Error('Empty response');
        }
    })
    .then(result => {
        if (result.success) {
            window.location.href = result.redirectUrl;
        } else {
            alert(result.message);
        }
    })
    .catch(error => {
        console.error('Error:', error);  // Print the error message in the console
        alert('An error occurred while processing the request.');
    });
});
