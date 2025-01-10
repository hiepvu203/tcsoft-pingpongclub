var ctx = document.getElementById('incomeExpenseChart').getContext('2d');
var chart = new Chart(ctx, {
    type: 'pie', // Loại biểu đồ
    data: {
        labels: ['Thu', 'Chi'],
        datasets: [{
            label: 'Thu chi',
            data: [@totalIncome, @totalExpense], // Tổng thu và chi
            backgroundColor: ['#36A2EB', '#FF6384'],
            borderColor: ['#36A2EB', '#FF6384'],
            borderWidth: 1
        }]
    }
});