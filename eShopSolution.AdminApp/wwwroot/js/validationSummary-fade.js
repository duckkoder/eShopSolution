document.addEventListener('DOMContentLoaded', function () {
    var validationSummary = document.querySelector('div[asp-validation-summary="ModelOnly"]');
    if (validationSummary && validationSummary.innerHTML.trim() !== "") {
        setTimeout(function () {
            validationSummary.style.display = 'none';
        }, 5000); // 5000 milliseconds = 5 seconds
    }
});
