var varIdEmpresa = document.getElementById("HdnIdEmpresa").value

if ($('#PiramidePoblacional').length) {

    var ctxPiramide = document.getElementById('PiramidePoblacional').getContext('2d');
    var piramidePoblacional = new Chart(ctxPiramide, {
        type: 'horizontalBar',
        data: {
            labels: ["18-25", "25-35", "35-45", "45-55", "55-65", "65-75", "75-85", ">85"],
            datasets: [
                {
                    data: [],
                    label: "Hombres",
                    backgroundColor: "blue"
                },
                {
                    data: [],
                    label: "Mujeres",
                    backgroundColor: "red",
                }

            ]
        },
        options: {
            title: {
                display: true,
                text: 'Distrubuci\u00F3n de Edades'
            },
            scales: {
                xAxes: [{
                    stacked: true,
                }],
                yAxes: [{
                    stacked: true
                }]
            }
        }
    });

    $.ajax({
        type: 'GET', //post method
        url: '/FactoresRiesgos/PiramidePoblacional', //ajaxformexample url
        data: { idEmpresa: varIdEmpresa },
        dataType: "json",
        success: function (result, textStatus, jqXHR) {
            //piramidePoblacional.data.labels = result.labels;
            piramidePoblacional.data.datasets[0].data = result.countsHombres;
            piramidePoblacional.data.datasets[1].data = result.countsMujeres;
            result.countsMujeres;
            piramidePoblacional.update();
        }
    });
}


if ($('#CondicionesRiesgoBar').length) {

    var ctx = document.getElementById("CondicionesRiesgoBar");
    var CondicionesRiesgoBar = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["Diabetes", "Hipertensión", "Asma", "SobrePeso", "Obesidad", "Enf. Autoinmune", "Enf. Corazón", "Cancer", "Tabaquismo", "Consumo Alcohol", "Farmacos o Drogas", "Embarazo"],
            datasets: [{
                label: 'Mujeres',
                backgroundColor: "#26B99A",
                data: []
            }, {
                label: 'Hombres',
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
        url: '/Tablero/CondicionesRiesgo', //ajaxformexample url
        data: { idEmpresa: varIdEmpresa },
        dataType: "json",
        success: function (result, textStatus, jqXHR) {
            //CondicionesRiesgoBar.data.labels = result.labels;
            CondicionesRiesgoBar.data.datasets[0].data = result.countsMujeres;
            CondicionesRiesgoBar.data.datasets[1].data = result.countsHombres;
            CondicionesRiesgoBar.update();
        }
    });
}

if ($('#RiesgosExpocisionCasaTransporte').length) {

    var ctx = document.getElementById('RiesgosExpocisionCasaTransporte').getContext('2d');
    var RiesgosExpocisionCasaTransporte = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ['> 3 personas vivienda', 'Múltiples Familias', 'Transporte Público'],
            datasets: [{
                data: [12, 19, 3],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
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
        url: '/Tablero/RiesgosExpocisionCasaTransporte', //ajaxformexample url
        data: { idEmpresa: varIdEmpresa },
        dataType: "json",
        success: function (result, textStatus, jqXHR) {
            //piramidePoblacional.data.labels = result.labels;
            RiesgosExpocisionCasaTransporte.data.datasets[0].data = result.counts;

            RiesgosExpocisionCasaTransporte.update();
        }
    });
}

if ($('#RiesgosEspacioLaboral').length) {
    var ctx = document.getElementById('RiesgosEspacioLaboral').getContext('2d');
    var RiesgosEspacioLaboral = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ['Espacio de trabajo cerrado', 'Sin ventilaci\u00F3n'],
            datasets: [{
                data: [1, 1],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
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
        url: '/Tablero/RiesgosEspacioLaboral', //ajaxformexample url
        data: { idEmpresa: varIdEmpresa },
        dataType: "json",
        success: function (result, textStatus, jqXHR) {
            //piramidePoblacional.data.labels = result.labels;
            RiesgosEspacioLaboral.data.datasets[0].data = result.counts;

            RiesgosEspacioLaboral.update();
        }
    });
}

//M1. Empleados que llenaron su encuesta de identificacion y factores de riesgo.
if ($('#LlenadoEmpleadosFactoresRiesgos').length) {

    var ctx = document.getElementById("LlenadoEmpleadosFactoresRiesgos");
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

    var LlenadoEmpleadosFactoresRiesgos_Chart = new Chart(ctx, {
        data: data,
        type: 'pie',
        otpions: {
            legend: false
        }
    });

    $.ajax({
        type: 'GET', //post method
        url: '/Tablero/EmpleadosLLenaronFactoresRiesgos', //ajaxformexample url,
        data: { idEmpresa: varIdEmpresa },
        dataType: "json",
        success: function (result, textStatus, jqXHR) {
            LlenadoEmpleadosFactoresRiesgos_Chart.data.labels = result.labels;
            LlenadoEmpleadosFactoresRiesgos_Chart.data.datasets[0].data = result.counts;
            LlenadoEmpleadosFactoresRiesgos_Chart.update();
        }
    });
}