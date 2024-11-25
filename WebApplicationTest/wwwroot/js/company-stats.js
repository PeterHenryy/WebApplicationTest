
let ctx = document.getElementById('company-chart').getContext('2d');

$.ajax({
    type: 'GET',
    url: '/Company/GetCompanyRevenuesPerMonth',
    success: function (data) {
        updateChart(data);
    },
    error: function (error) {

        console.error('Error fetching data from the server:', error);
    }
});

function updateChart(revenues) {
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            datasets: [{
                label: 'Company Monthly Revenue $',
                data: revenues,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.6)',
                    'rgba(255, 159, 64, 0.6)',
                    'rgba(255, 205, 86, 0.6)',
                    'rgba(75, 192, 192, 0.6)',
                    'rgba(54, 162, 235, 0.6)',
                    'rgba(153, 102, 255, 0.6)',
                    'rgba(201, 203, 207, 0.6)',
                    'rgba(255, 120, 120, 0.6)',
                    'rgba(120, 201, 120, 0.6)',
                    'rgba(120, 120, 255, 0.6)',
                    'rgba(255, 200, 120, 0.6)',
                    'rgba(200, 120, 255, 0.6)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}