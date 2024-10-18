// Used acros multiple views, but not all
document.addEventListener("DOMContentLoaded", function() {
    function validateToken(token) {
        return fetch('/api/v1/verify-token', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        })
        .then(response => response.json())
        .then(data => data.valid)
        .catch(error => {
            console.error('Error verifying token:', error);
            return false;
        });
    }

    const token = localStorage.getItem('token');

    if (token) {
        validateToken(token).then(isValid => {
            if (!isValid) {
                window.location.href = '/login';
            }
        });
    } else {
        window.location.href = '/login';
    }
});
