var varIdEmpresa = document.getElementById("HdnIdEmpresa").value

if ($('#PositivosNegativosPorSemana').length) {

    var ctx = document.getElementById("PositivosNegativosPorSemana");
    var PositivosNegaticosPorSemana = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: 'Positivos',
                backgroundColor: "#FF3300",
                data: []
            }, {
                label: 'Negativos',
                backgroundColor: "#03586A",
                data: []
            }]
        },

        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}

$.ajax({
    type: 'GET', //post method
    url: '/ReportesContagios/PositivosNegativosPerWeek', //ajaxformexample url
    data: { idEmpresa: varIdEmpresa },
    dataType: "json",
    success: function (result, textStatus, jqXHR) {
        PositivosNegaticosPorSemana.data.labels = result.labels;
        PositivosNegaticosPorSemana.data.datasets[0].data = result.countsPositivos;
        PositivosNegaticosPorSemana.data.datasets[1].data = result.countsNegativos;
        /*ResultadosPositivosNegaticosPorTipoDePrueba.data.datasets[2].data = result.countsSospechosos;*/
        PositivosNegaticosPorSemana.update();
    }
});