var varIdEmpresa = document.getElementById("HdnIdEmpresa").value

if ($('#TiposDePruebas').length) {

    var ctx = document.getElementById("TiposDePruebas");
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

    var TiposDePruebasPie = new Chart(ctx, {
        data: data,
        type: 'doughnut',
        otpions: {
            legend: false
        }
    });

    $.ajax({
        type: 'GET', //post method
        url: '/Pruebas/GetConteTipoPruebasPie', //ajaxformexample url,
        data: { idEmpresa: varIdEmpresa },
        dataType: "json",
        success: function (result, textStatus, jqXHR) {
            TiposDePruebasPie.data.labels = result.labels;
            TiposDePruebasPie.data.datasets[0].data = result.counts;
            TiposDePruebasPie.update();
        }
    });
}

if ($('#ResultadoPruebas').length) {

    var ctx = document.getElementById("ResultadoPruebas");
    var CondicionesRiesgoBar = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["En la casa en asilamiento", "En otra casa aislado", "Hospitalizado en piso NO- COVID", "Hospitalizado en piso COVID", "En terapia intensiva","Falleció"],

            datasets: [{
                label: 'Positivos',
                backgroundColor: "#ff8000",
                data: []
            //}, {
            //    label: 'Negativos',
            //    backgroundColor: "#03586A",
            //    data: []
            //}, {
            //    label: 'Sospechosos',
            //    backgroundColor: "#FFFF00	",
            //    data: []
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
        url: '/Pruebas/ResultadoPruebas', //ajaxformexample url
        data: { idEmpresa: varIdEmpresa },
        dataType: "json",
        success: function (result, textStatus, jqXHR) {
            //CondicionesRiesgoBar.data.labels = result.labels;
            CondicionesRiesgoBar.data.datasets[0].data = result.countsPositivos;
            //CondicionesRiesgoBar.data.datasets[1].data = result.countsNegativos;
            //CondicionesRiesgoBar.data.datasets[2].data = result.countsSospechosos;
            CondicionesRiesgoBar.update();
        }
    });

}
