
var _empresaIdentificador = 0;

function GetEmpresaFirstOrDefault() {
    if (typeof $("#graficos .form-actions select")[0] !== 'undefined' && $("#graficos .form-actions select")[0].length > 0) {
        var selectEmpresas = $("#graficos .form-actions select")[0];
        _empresaIdentificador = selectEmpresas.options[1].value;
    }
}

$(document).ready(function () {
    GetEmpresaFirstOrDefault();
    if (_empresaIdentificador != null && _empresaIdentificador != 0) {
        CrearGrG6PositivosSospechososSem();
        CrearGrG7PositivosSospechososSemXArea();
        CrearGrR11PositivosSospechososMen();
        CrearGrR12PositivosSospechososTotal();
        CrearGrR13PositivosXArea();
        CrearGrR14PositivosSospechososSemXArea();
        CrearGrR15PositivosSospechososTotalXArea();
    }
});

//G6. Positivos y sospechosos semanales
function CrearGrG6PositivosSospechososSem() {
    if ($('#GrG6PositivosSospechososSem').length) {
        var ctx = document.getElementById('GrG6PositivosSospechososSem');
        var GrG6PositivosSospechososSem = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ["Antigeno", "LG", "Neumonia No Confimada", "PCR", "TAC", "Sospechosos"],
                datasets: [{
                    label: 'Cantidad',
                    backgroundColor: "#26B99A",
                    data: [] //[2, 3, 2, 0, 3]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Semanal de Positivos y Sospechosos'
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
            url: '/ReportesContagios/ObtenerReporteContagioSemanal',
            data: { IdEmpresa: _empresaIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {

                if (typeof result !== 'undefined' && result.length > 0) {
                    var dataArray = [];
                    var rcPosSemanalesAntigeno = result.find(obj => {
                        return obj.tipoId == "rcPosSemanalesAntigeno";
                    });
                    dataArray.push(rcPosSemanalesAntigeno.cantidad);

                    var rcPosSemanalesLG = result.find(obj => {
                        return obj.tipoId == "rcPosSemanalesLG";
                    });
                    dataArray.push(rcPosSemanalesLG.cantidad);

                    var rcPosSemNeumoniaNoConfCOVID = result.find(obj => {
                        return obj.tipoId == "rcPosSemNeumoniaNoConfCOVID";
                    });
                    dataArray.push(rcPosSemNeumoniaNoConfCOVID.cantidad);

                    var rcPositivosSemPCR = result.find(obj => {
                        return obj.tipoId == "rcPositivosSemPCR";
                    });
                    dataArray.push(rcPositivosSemPCR.cantidad);

                    var rcPositivosSemTAC = result.find(obj => {
                        return obj.tipoId == "rcPositivosSemTAC";
                    });
                    dataArray.push(rcPositivosSemTAC.cantidad);

                    var rcPositivosSospechososNeuNoConfCOVID = result.find(obj => {
                        return obj.tipoId == "rcPositivosSospechososNeuNoConfCOVID";
                    });
                    dataArray.push(rcPositivosSospechososNeuNoConfCOVID.cantidad);

                    GrG6PositivosSospechososSem.data.datasets[0].data = dataArray;
                    GrG6PositivosSospechososSem.update();

                }
            }
        });
    }
}

function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
}

//G7.Número semanal de positivos y sospechosos por área.
function CrearGrG7PositivosSospechososSemXArea() {
    if ($('#GrG7PositivosSospechososSemXArea').length) {
        var ctx = document.getElementById('GrG7PositivosSospechososSemXArea');
        var GrG7PositivosSospechososSemXArea = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: [], //["Recursos Humanos", "Finanzas"],
                datasets: [{
                    label: 'Positivos',
                    backgroundColor: "#BF351A",
                    data: [] //[2, 3, 2, 1]
                },
                {
                    label: 'Sospechosos',
                    backgroundColor: "#0C4DBF",
                    data: [] //[2, 3, 2, 1]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Semanal De Positivos Y Sospechosos Por \u00C1rea'
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
            url: '/ReportesContagios/ObtenerReporteContagioSemanalXArea',
            data: { IdEmpresa: _empresaIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {

                if (typeof result !== 'undefined' && result.length > 0) {

                    var areas = result.map(a => a.nombreArea);
                    var uniqueAreas = areas.filter(onlyUnique);
                    var dataSet0Array = [];
                    var dataSet1Array = [];

                    uniqueAreas.forEach(function (area, indice, array) {
                        var positivos = $.grep(result, function (obj)
                        {
                            return obj.nombreArea == area &&
                                (obj.tipoId == "rcPosSemanalesAntigeno" ||
                                    obj.tipoId == "rcPosSemanalesLG" ||
                                    obj.tipoId == "rcPosSemNeumoniaNoConfCOVID" ||
                                    obj.tipoId == "rcPositivosSemPCR" ||
                                    obj.tipoId == "rcPositivosSemTAC"
                                );
                        })
                            .map(function (prop) { return prop.cantidad; });

                        var sospechosos = $.grep(result, function (obj) {
                            return obj.nombreArea == area && obj.tipoId == "rcPositivosSospechososNeuNoConfCOVID";
                        })
                            .map(function (prop) { return prop.cantidad; });

                        var posSum = positivos.reduce((a, b) => a + b, 0);
                        var sosSum = sospechosos.reduce((a, b) => a + b, 0);
                        dataSet0Array.push(posSum);
                        dataSet1Array.push(sosSum);
                        GrG7PositivosSospechososSemXArea.data.labels.push(area);
                    });

                    GrG7PositivosSospechososSemXArea.data.datasets[0].data = dataSet0Array;
                    GrG7PositivosSospechososSemXArea.data.datasets[1].data = dataSet1Array;
                    GrG7PositivosSospechososSemXArea.update();
                }

            }
        });
    }
}

//R11. AM. Positivos y sospechosos
function CrearGrR11PositivosSospechososMen() {
    if ($('#GrR11PositivosSospechososMen').length) {
        var ctx = document.getElementById('GrR11PositivosSospechososMen');
        var GrR11PositivosSospechososMen = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ["Antigeno", "LG", "Neumonia No Confimada", "PCR", "TAC", "Sospechosos"],
                datasets: [{
                    label: 'Cantidad',
                    backgroundColor: "#26B99A",
                    data: [] //[2, 3, 2, 0, 3]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Semanal de Positivos y Sospechosos'
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
            url: '/ReportesContagios/ObtenerReporteContagioMensuales',
            data: { IdEmpresa: _empresaIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {

                if (typeof result !== 'undefined' && result.length > 0) {
                    var dataArray = [];
                    var rcPosSemanalesAntigeno = result.find(obj => {
                        return obj.tipoId == "rcPosSemanalesAntigeno";
                    });
                    dataArray.push(rcPosSemanalesAntigeno.cantidad);

                    var rcPosSemanalesLG = result.find(obj => {
                        return obj.tipoId == "rcPosSemanalesLG";
                    });
                    dataArray.push(rcPosSemanalesLG.cantidad);

                    var rcPosSemNeumoniaNoConfCOVID = result.find(obj => {
                        return obj.tipoId == "rcPosSemNeumoniaNoConfCOVID";
                    });
                    dataArray.push(rcPosSemNeumoniaNoConfCOVID.cantidad);

                    var rcPositivosSemPCR = result.find(obj => {
                        return obj.tipoId == "rcPositivosSemPCR";
                    });
                    dataArray.push(rcPositivosSemPCR.cantidad);

                    var rcPositivosSemTAC = result.find(obj => {
                        return obj.tipoId == "rcPositivosSemTAC";
                    });
                    dataArray.push(rcPositivosSemTAC.cantidad);

                    var rcPositivosSospechososNeuNoConfCOVID = result.find(obj => {
                        return obj.tipoId == "rcPositivosSospechososNeuNoConfCOVID";
                    });
                    dataArray.push(rcPositivosSospechososNeuNoConfCOVID.cantidad);

                    GrR11PositivosSospechososMen.data.datasets[0].data = dataArray;
                    GrR11PositivosSospechososMen.update();
                }
            }
        });
    }
}

//R12. AT. Positivos, sospechosos y descartados.
function CrearGrR12PositivosSospechososTotal() {
    if ($('#GrR12PositivosSospechososTotal').length) {
        var ctx = document.getElementById('GrR12PositivosSospechososTotal');
        var GrR12PositivosSospechososTot = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ["Antigeno", "LG", "Neumonia No Confimada", "PCR", "TAC", "Sospechosos", "Descartados"],
                datasets: [{
                    label: 'Cantidad',
                    backgroundColor: "#F56AE0",
                    data: [] //[1, 3, 2, 0, 3]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Mensual de Positivos, Sospechosos Y Descartados'
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
            url: '/ReportesContagios/ObtenerReporteContagioEmpresa',
            data: { IdEmpresa: _empresaIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {

                if (typeof result !== 'undefined' && result.length > 0) {
                    var dataArray = [];
                    var rcPosSemanalesAntigeno = result.find(obj => {
                        return obj.tipoId == "rcPosSemanalesAntigeno";
                    });
                    dataArray.push(rcPosSemanalesAntigeno.cantidad);

                    var rcPosSemanalesLG = result.find(obj => {
                        return obj.tipoId == "rcPosSemanalesLG";
                    });
                    dataArray.push(rcPosSemanalesLG.cantidad);

                    var rcPosSemNeumoniaNoConfCOVID = result.find(obj => {
                        return obj.tipoId == "rcPosSemNeumoniaNoConfCOVID";
                    });
                    dataArray.push(rcPosSemNeumoniaNoConfCOVID.cantidad);

                    var rcPositivosSemPCR = result.find(obj => {
                        return obj.tipoId == "rcPositivosSemPCR";
                    });
                    dataArray.push(rcPositivosSemPCR.cantidad);

                    var rcPositivosSemTAC = result.find(obj => {
                        return obj.tipoId == "rcPositivosSemTAC";
                    });
                    dataArray.push(rcPositivosSemTAC.cantidad);

                    var rcPositivosSospechososNeuNoConfCOVID = result.find(obj => {
                        return obj.tipoId == "rcPositivosSospechososNeuNoConfCOVID";
                    });
                    dataArray.push(rcPositivosSospechososNeuNoConfCOVID.cantidad);

                    var rcSospechososDescartados = result.find(obj => {
                        return obj.tipoId == "rcSospechososDescartados";
                    });
                    dataArray.push(rcSospechososDescartados.cantidad);

                    GrR12PositivosSospechososTot.data.datasets[0].data = dataArray;
                    GrR12PositivosSospechososTot.update();
                }
            }
        });
    }
}

//R13. Distribución de positivos por área.
function CrearGrR13PositivosXArea() {
    if ($('#GrR13PositivosXArea').length) {
        var ctx = document.getElementById('GrR13PositivosXArea');
        var GrR13PositivosXArea = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: [], //["Recursos Humanos", "Finanzas"],
                datasets: [{
                    label: 'Positivos',
                    backgroundColor: [], //["#668EF9", "#A266F9"],
                    data: []//[2, 3]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Positivos Por \u00C1rea'
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
            url: '/ReportesContagios/ObtenerPositivosSemanalXArea',
            data: { IdEmpresa: _empresaIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {

                if (typeof result !== 'undefined' && result.length > 0) {

                    var areas = result.map(a => a.nombreArea);
                    var uniqueAreas = areas.filter(onlyUnique);
                    var dataSet0Array = [];

                    uniqueAreas.forEach(function (area, indice, array) {
                        var positivos = $.grep(result, function (obj) {
                            return obj.nombreArea == area &&
                                (obj.tipoId == "rcPosSemanalesAntigeno" ||
                                    obj.tipoId == "rcPosSemanalesLG" ||
                                    obj.tipoId == "rcPosSemNeumoniaNoConfCOVID" ||
                                    obj.tipoId == "rcPositivosSemPCR" ||
                                    obj.tipoId == "rcPositivosSemTAC"
                                );
                        })
                            .map(function (prop) { return prop.cantidad; });

                        var posSum = positivos.reduce((a, b) => a + b, 0);
                        dataSet0Array.push(posSum);
                        GrR13PositivosXArea.data.labels.push(area);
                        var randomColor = "#000000".replace(/0/g, function () { return (~~(Math.random() * 16)).toString(16); });
                        GrR13PositivosXArea.data.datasets[0].backgroundColor.push(randomColor);
                    });

                    GrR13PositivosXArea.data.datasets[0].data = dataSet0Array;
                    GrR13PositivosXArea.update();
                }

            }
        });
    }
}

//R14. AM Distribución por área.
function CrearGrR14PositivosSospechososSemXArea() {
    if ($('#GrR14PositivosMenXArea').length) {
        var ctx = document.getElementById('GrR14PositivosMenXArea');
        var GrR14PositivosMenXArea = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: [], //["Recursos Humanos", "Finanzas", "Tecnologías", "Producción"],
                datasets: [{
                    label: 'Positivos',
                    backgroundColor: "#F9CC66",
                    data: [] //[1, 3, 2, 1]
                }, {
                    label: 'Sospechosos',
                    backgroundColor: "#F9B066",
                    data: [] //[2, 1, 1, 4]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Mensual De Positivos Y Sospechosos Por \u00C1rea'
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
            url: '/ReportesContagios/ObtenerPositivosMensualXArea',
            data: { IdEmpresa: _empresaIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {

                if (typeof result !== 'undefined' && result.length > 0) {

                    var areas = result.map(a => a.nombreArea);
                    var uniqueAreas = areas.filter(onlyUnique);
                    var dataSet0Array = [];
                    var dataSet1Array = [];

                    uniqueAreas.forEach(function (area, indice, array) {
                        var positivos = $.grep(result, function (obj) {
                            return obj.nombreArea == area &&
                                (obj.tipoId == "rcPosSemanalesAntigeno" ||
                                    obj.tipoId == "rcPosSemanalesLG" ||
                                    obj.tipoId == "rcPosSemNeumoniaNoConfCOVID" ||
                                    obj.tipoId == "rcPositivosSemPCR" ||
                                    obj.tipoId == "rcPositivosSemTAC"
                                );
                        })
                            .map(function (prop) { return prop.cantidad; });

                        var sospechosos = $.grep(result, function (obj) {
                            return obj.nombreArea == area && obj.tipoId == "rcPositivosSospechososNeuNoConfCOVID";
                        })
                            .map(function (prop) { return prop.cantidad; });

                        var posSum = positivos.reduce((a, b) => a + b, 0);
                        var sosSum = sospechosos.reduce((a, b) => a + b, 0);
                        dataSet0Array.push(posSum);
                        dataSet1Array.push(sosSum);
                        GrR14PositivosMenXArea.data.labels.push(area);
                    });

                    GrR14PositivosMenXArea.data.datasets[0].data = dataSet0Array;
                    GrR14PositivosMenXArea.data.datasets[1].data = dataSet1Array;
                    GrR14PositivosMenXArea.update();
                }

            }
        });
    }
}

//R15. AT Distribución por área
function CrearGrR15PositivosSospechososTotalXArea() {
    if ($('#GrR15PositivosTotalXArea').length) {
        var ctx = document.getElementById('GrR15PositivosTotalXArea');
        var GrR15PositivosTotalXArea = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: [], //["Recursos Humanos", "Finanzas", "Tecnologías", "Producción"],
                datasets: [{
                    label: 'Positivos',
                    backgroundColor: "#F966D1",
                    data: [] //[1, 3, 2, 1]
                }, {
                    label: 'Sospechosos',
                    backgroundColor: "#D366F9",
                    data: [] //[2, 1, 1, 4]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: 'Acumulado Total De Positivos Y Sospechosos Por \u00C1rea'
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
            url: '/ReportesContagios/ObtenerPositivosTotalXArea',
            data: { IdEmpresa: _empresaIdentificador },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {
                console.log(result);
                if (typeof result !== 'undefined' && result.length > 0) {

                    var areas = result.map(a => a.nombreArea);
                    var uniqueAreas = areas.filter(onlyUnique);
                    var dataSet0Array = [];
                    var dataSet1Array = [];

                    uniqueAreas.forEach(function (area, indice, array) {
                        var positivos = $.grep(result, function (obj) {
                            return obj.nombreArea == area &&
                                (obj.tipoId == "rcPosSemanalesAntigeno" ||
                                    obj.tipoId == "rcPosSemanalesLG" ||
                                    obj.tipoId == "rcPosSemNeumoniaNoConfCOVID" ||
                                    obj.tipoId == "rcPositivosSemPCR" ||
                                    obj.tipoId == "rcPositivosSemTAC"
                                );
                        })
                            .map(function (prop) { return prop.cantidad; });

                        var sospechosos = $.grep(result, function (obj) {
                            return obj.nombreArea == area && obj.tipoId == "rcPositivosSospechososNeuNoConfCOVID";
                        })
                            .map(function (prop) { return prop.cantidad; });

                        var posSum = positivos.reduce((a, b) => a + b, 0);
                        var sosSum = sospechosos.reduce((a, b) => a + b, 0);
                        dataSet0Array.push(posSum);
                        dataSet1Array.push(sosSum);
                        GrR15PositivosTotalXArea.data.labels.push(area);
                    });

                    GrR15PositivosTotalXArea.data.datasets[0].data = dataSet0Array;
                    GrR15PositivosTotalXArea.data.datasets[1].data = dataSet1Array;
                    GrR15PositivosTotalXArea.update();
                }

            }
        });
    }
}