
var _empresaSeguimientoIdentificador = 0;

function GetEmpresaSeguimientoFirstOrDefault() {
    if (typeof $("#graficos .form-actions select")[0] !== 'undefined' && $("#graficos .form-actions select")[0].length > 0) {
        var selectEmpresas = $("#graficos .form-actions select")[0];
        _empresaSeguimientoIdentificador = selectEmpresas.options[1].value;
    }
}

$(document).ready(function () {
    GetEmpresaSeguimientoFirstOrDefault();
    if (_empresaSeguimientoIdentificador != null && _empresaSeguimientoIdentificador != 0) {
        CrearGrG4PacientesPruebas();
        CrearGrG5TrabajadoresSeguimientoGral();
        CrearGrR8NoLLenaronF4();
        CrearGrGrR9NoLLenaronF4Mens();
    }
});

//G4. A quienes se deben hacer pruebas de seguimiento.
function CrearGrG4PacientesPruebas() {
    if ($('#GrG4PacientesPruebas').length) {
        var ctx = document.getElementById('GrG4PacientesPruebas');
        var GrG4PacientesPruebas = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ["Pacientes"], //["Recursos Humanos", "Finanzas"],
                datasets: [{
                    label: 'Cantidad',
                    backgroundColor: [], //["#668EF9", "#A266F9"],
                    data: []//[2, 3]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Pacientes que necesitan realizar pruebas'
                },
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
            type: 'GET',
            url: '/SeguimientosCovid/ObtenerTrabajadoresARealizarPruebas',
            data: { idEmpresa: _empresaSeguimientoIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {
                if (typeof result !== 'undefined' && result.length > 0) {
                    var randomColor = "#000000".replace(/0/g, function () { return (~~(Math.random() * 16)).toString(16); });
                    GrG4PacientesPruebas.data.datasets[0].backgroundColor.push(randomColor);
                    GrG4PacientesPruebas.data.datasets[0].data.push(result.length);
                    GrG4PacientesPruebas.update();
                }
            }
        });
    }
}

//G5. Seguimiento general de contagiados.
function CrearGrG5TrabajadoresSeguimientoGral() {
    if ($('#GrG5TrabajadoresSeguimientoGral').length) {
        var ctx = document.getElementById('GrG5TrabajadoresSeguimientoGral');
        var GrG5TrabajadoresSeguimientoGral = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ["Género"],
                datasets: [{
                    label: 'Masculino',
                    backgroundColor: "#F9CC66",
                    data: [] //[1, 3, 2, 1]
                },
                {
                    label: 'Femenino',
                    backgroundColor: "#F9B066",
                    data: [] //[1, 3, 2, 1]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Seguimiento general de contagiados'
                },
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
            type: 'GET',
            url: '/SeguimientosCovid/ObtenerTrabajadoresSeguimientoGral',
            data: { idEmpresa: _empresaSeguimientoIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {

                if (typeof result !== 'undefined' && result.length > 0) {
                    var dataSet0Array = [];

                    var masculinos = $.grep(result, function (obj) {
                        return obj.genero == "Masculino";
                    });
                        //.map(function (prop) { return prop.genero; });
                    var femeninos = $.grep(result, function (obj) {
                        return obj.genero == "Femenino";
                    });
                        //.map(function (prop) { return prop.genero; });

                    GrG5TrabajadoresSeguimientoGral.data.datasets[0].data.push(masculinos.length)
                    GrG5TrabajadoresSeguimientoGral.data.datasets[1].data.push(femeninos.length);
                    GrG5TrabajadoresSeguimientoGral.update();
                }

            }
        });
    }
}

//R8. Pacientes que no llenaron su F4
function CrearGrR8NoLLenaronF4() {
    if ($('#GrGrR8NoLLenaronF4').length) {
        var ctx = document.getElementById('GrGrR8NoLLenaronF4');
        var GrGrR8NoLLenaronF4 = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ["SI Llenaron", "NO Llenaron"],
                datasets: [{
                    label: 'SI',
                    backgroundColor: ["#9497EF", "#90EFB2"],
                    data: []
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Pacientes Que Si/No Llenaron F4.'
                },
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
            type: 'GET',
            url: '/SeguimientosCovid/ObtenerPacientesNoLlenaronRC',
            data: { idEmpresa: _empresaSeguimientoIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {

                if (typeof result !== 'undefined' && result.length > 0) {

                    var siRealizoPrueba = $.grep(result, function (obj) {
                        return obj.yaRealizoPrueba == "Si";
                    });

                    var noRealizoPrueba = $.grep(result, function (obj) {
                        return obj.yaRealizoPrueba == "No";
                    });

                    GrGrR8NoLLenaronF4.data.datasets[0].data.push(siRealizoPrueba.length)
                    GrGrR8NoLLenaronF4.data.datasets[0].data.push(noRealizoPrueba.length);
                    GrGrR8NoLLenaronF4.update();
                }

            }
        });
    }
}

//R9.AM.Quienes no llenaron su F4 en todo el mes.
function CrearGrGrR9NoLLenaronF4Mens() {
    if ($('#GrGrR9NoLLenaronF4Mens').length) {
        var ctx = document.getElementById('GrGrR9NoLLenaronF4Mens');
        var GrGrR9NoLLenaronF4Mens = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ["SI Llenaron", "NO Llenaron"],
                datasets: [{
                    label: 'SI',
                    backgroundColor: ["#F791F7", "#9510F8"],
                    data: [] //[12, 6]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Pacientes Que Si/No Llenaron F4 En Todo El Mes'
                },
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
            type: 'GET',
            url: '/SeguimientosCovid/ObtenerAMNoLlenaronSeguimientoCovidMen',
            data: { idEmpresa: _empresaSeguimientoIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {

                if (typeof result !== 'undefined' && result.length > 0) {

                    var siRealizoSeg = $.grep(result, function (obj) {
                        return obj.yaRealizoSeguimientoCovid == "Si";
                    });

                    var noRealizoSeg = $.grep(result, function (obj) {
                        return obj.yaRealizoSeguimientoCovid == "No";
                    });

                    GrGrR9NoLLenaronF4Mens.data.datasets[0].data.push(siRealizoSeg.length)
                    GrGrR9NoLLenaronF4Mens.data.datasets[0].data.push(noRealizoSeg.length);
                    GrGrR9NoLLenaronF4Mens.update();
                }

            }
        });
    }
}

if ($('#pieSeguimientoSintomas').length) {

    var ctx = document.getElementById("pieSeguimientoSintomas");
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

    var pieSeguimientoSintomas = new Chart(ctx, {
        data: data,
        type: 'pie',
        otpions: {
            legend: false
        }
    });
}

$.ajax({
    type: 'GET', //post method
    url: '/SeguimientosCovid/GetSintomasPie', //ajaxformexample url,
    data: { idEmpresa: varIdEmpresa },
    dataType: "json",
    success: function (result, textStatus, jqXHR) {
        pieSeguimientoSintomas.data.labels = result.labels;
        pieSeguimientoSintomas.data.datasets[0].data = result.counts;
        pieSeguimientoSintomas.update();
    }
});

if ($('#SeguimientoUbicacionesPie').length) {

    var ctx = document.getElementById("SeguimientoUbicacionesPie");
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

    var SeguimientoUbicacionesPie = new Chart(ctx, {
        data: data,
        type: 'pie',
        otpions: {
            legend: false
        }
    });
}

$.ajax({
    type: 'GET', //post method
    url: '/SeguimientosCovid/GetUbicacionesPie', //ajaxformexample url,
    data: { idEmpresa: varIdEmpresa },
    dataType: "json",
    success: function (result, textStatus, jqXHR) {
        SeguimientoUbicacionesPie.data.labels = result.labels;
        SeguimientoUbicacionesPie.data.datasets[0].data = result.counts;
        SeguimientoUbicacionesPie.update();
    }
});