document.querySelectorAll('.toggle-button').forEach(button => {
    button.addEventListener('click', function() {
        if (this.classList.contains('on')) {
            this.classList.remove('on');
            this.classList.add('off');
        } else {
            this.classList.remove('off');
            this.classList.add('on');
        }
    });
});
