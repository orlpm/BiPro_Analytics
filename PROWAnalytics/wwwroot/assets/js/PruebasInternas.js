var varIdEmpresa = document.getElementById("HdnIdEmpresa").value

if ($('#ConteoResultadoPruebasInternas').length) {

    var ctx = document.getElementById("ConteoResultadoPruebasInternas");
    var data = {
        datasets: [{
            data: [],
            backgroundColor: [
                "#455C73",
                "#9B59B6",
                "#BDC3C7",
                "#26B99A",
                "#3498DB"
            ],
            label: 'My dataset' // for legend
        }],
        labels: []
    };

    var ResultadoPruebasInternas = new Chart(ctx, {
        data: data,
        type: 'pie',
        otpions: {
            legend: false
        }
    });

    $.ajax({
        type: 'GET', //post method
        url: '/PruebasInternas/GetConteoResultadoPruebasInternasPie', //ajaxformexample url,
        data: { idEmpresa: varIdEmpresa },
        dataType: "json",
        success: function (result, textStatus, jqXHR) {
            ResultadoPruebasInternas.data.labels = result.labels;
            ResultadoPruebasInternas.data.datasets[0].data = result.counts;
            ResultadoPruebasInternas.update();
        }
    });
}


if ($('#Resultado_NPD_PruebasInternas').length) {

    var ctx = document.getElementById("Resultado_NPD_PruebasInternas");
    var ResultadosPositivosNegaticosPorTipoDePrueba = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["PCR", "Rápida de sangre", "Rápida con Pinchazo", "Rápida Nasal", "Tomografía", "Diagnostico Médico"],
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

    $.ajax({
        type: 'GET', //post method
        url: '/PruebasInternas/ResultadoPruebasInternas', //ajaxformexample url
        data: { idEmpresa: varIdEmpresa },
        dataType: "json",
        success: function (result, textStatus, jqXHR) {
            //CondicionesRiesgoBar.data.labels = result.labels;
            ResultadosPositivosNegaticosPorTipoDePrueba.data.datasets[0].data = result.countsPositivos;
            ResultadosPositivosNegaticosPorTipoDePrueba.data.datasets[1].data = result.countsNegativos;
            /*ResultadosPositivosNegaticosPorTipoDePrueba.data.datasets[2].data = result.countsSospechosos;*/
            ResultadosPositivosNegaticosPorTipoDePrueba.update();
        }
    });
}



