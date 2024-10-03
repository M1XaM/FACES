$(document).ready(function() {
  // Обработка ввода и фокуса для полей формы
  $('.form').find('input, textarea').on('keyup blur focus', function (e) {
      var $this = $(this),
          label = $this.prev('label');

      if (e.type === 'keyup') {
          if ($this.val() === '') {
              label.removeClass('active highlight');
          } else {
              label.addClass('active highlight');
          }
      } else if (e.type === 'blur') {
          if ($this.val() === '') {
              label.removeClass('active highlight');
          } else {
              label.removeClass('highlight');
          }
      } else if (e.type === 'focus') {
          if ($this.val() === '') {
              label.removeClass('highlight');
          } else {
              label.addClass('highlight');
          }
      }
  });

  // Обработка переключения вкладок
  $('.tab a').on('click', function (e) {
      e.preventDefault();

      var target = $(this).attr('href');

      $(this).parent().addClass('active');
      $(this).parent().siblings().removeClass('active');

      $('.tab-content > div').hide();
      $(target).fadeIn(600);
  });

  // Обработка перехода к форме "Forgot Password"
  $('.forgot a').on('click', function (e) {
      e.preventDefault();

      $('.tab-content > div').hide(); // Скрыть все формы
      $('#forgot-password').fadeIn(600); // Показать форму "Forgot Password"
  });

  // Обработка возврата к форме "Log In"
  $('#back-to-login').on('click', function (e) {
      e.preventDefault();

      $('.tab-content > div').hide(); // Скрыть все формы
      $('#login').fadeIn(600); // Показать форму "Log In"
  });
});
