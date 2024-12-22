document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/logs')
        .then(response => response.json())
        .then(data => {
            console.log('Logs:', data);
        });

    fetch('/api/reports')
        .then(response => response.json())
        .then(data => {
            console.log('Reports:', data);
        });
});
