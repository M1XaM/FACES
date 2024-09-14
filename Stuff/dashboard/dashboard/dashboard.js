const buttons = document.querySelectorAll('.toggle-btn');

buttons.forEach(button => {
  button.addEventListener('click', function() {
    const sectionToShow = this.id.replace('btn', 'section');

    document.querySelectorAll('.content-section').forEach(section => {
      section.classList.remove('show');
    });

    setTimeout(() => {
      document.getElementById(sectionToShow).classList.add('show');
    }, 300); 
  });
});
