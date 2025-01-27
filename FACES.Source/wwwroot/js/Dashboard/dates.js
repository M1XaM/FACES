document.addEventListener('DOMContentLoaded', function () {
  const createWorkflowBtn = document.getElementById('createWorkflowBtn');
  if (createWorkflowBtn) {
      createWorkflowBtn.addEventListener('click', function () {
          const section2 = document.getElementById('section2');
          const workflowBuilder = document.getElementById('workflowBuilder');

          section2.classList.remove('show');
          setTimeout(() => {
              section2.style.display = 'none';
          }, 300);

          workflowBuilder.style.display = 'block';
          setTimeout(() => {
              workflowBuilder.classList.add('show');
          }, 10);
      });
  } else {
      console.error("Element with ID 'createWorkflowBtn' not found.");
  }
});
