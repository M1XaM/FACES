document.getElementById('registrationButton').addEventListener('click', function(event) {
    event.preventDefault();

    const firstName = document.getElementById('firstNameReg').value;
    const lastName = document.getElementById('lastNameReg').value;
    const email = document.getElementById('emailReg').value;
    const password = document.getElementById('passwordReg').value;

    const data = {
        FirstName: firstName,
        LastName: lastName,
        Email: email,
        Password: password
    };

    fetch('/user/registration-post', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        body: JSON.stringify(data)
    })
    .then(response => response.json())
    .then(result => {
        if (result.success) {
            window.location.href = result.redirectUrl;
        } else {
            alert(result.message);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('An error occurred while processing the registration.');
    });
});
