/**
* Template Name: Medilab - v2.0.0
* Template URL: https://bootstrapmade.com/medilab-free-medical-bootstrap-theme/
* Author: BootstrapMade.com
* License: https://bootstrapmade.com/license/
*/
!(function($) {
  "use strict";

  // Preloader
  $(window).on('load', function() {
    if ($('#preloader').length) {
      $('#preloader').delay(100).fadeOut('slow', function() {
        $(this).remove();
      });
    }
  });

  // Smooth scroll for the navigation menu and links with .scrollto classes
  var scrolltoOffset = $('#header').outerHeight() - 1;
  $(document).on('click', '.nav-menu a, .mobile-nav a, .scrollto', function(e) {
    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
      var target = $(this.hash);
      if (target.length) {
        e.preventDefault();

        var scrollto = target.offset().top - scrolltoOffset;

        if ($(this).attr("href") == '#header') {
          scrollto = 0;
        }

        $('html, body').animate({
          scrollTop: scrollto
        }, 1500, 'easeInOutExpo');

        if ($(this).parents('.nav-menu, .mobile-nav').length) {
          $('.nav-menu .active, .mobile-nav .active').removeClass('active');
          $(this).closest('li').addClass('active');
        }

        if ($('body').hasClass('mobile-nav-active')) {
          $('body').removeClass('mobile-nav-active');
          $('.mobile-nav-toggle i').toggleClass('icofont-navigation-menu icofont-close');
          $('.mobile-nav-overly').fadeOut();
        }
        return false;
      }
    }
  });

  // Activate smooth scroll on page load with hash links in the url
  $(document).ready(function() {
    if (window.location.hash) {
      var initial_nav = window.location.hash;
      if ($(initial_nav).length) {
        var scrollto = $(initial_nav).offset().top - scrolltoOffset;
        $('html, body').animate({
          scrollTop: scrollto
        }, 1500, 'easeInOutExpo');
      }
    }
  });

  // Navigation active state on scroll
  var nav_sections = $('section');
  var main_nav = $('.nav-menu, .mobile-nav');

  $(window).on('scroll', function() {
    var cur_pos = $(this).scrollTop() + 200;

    nav_sections.each(function() {
      var top = $(this).offset().top,
        bottom = top + $(this).outerHeight();

      if (cur_pos >= top && cur_pos <= bottom) {
        if (cur_pos <= bottom) {
          main_nav.find('li').removeClass('active');
        }
        main_nav.find('a[href="#' + $(this).attr('id') + '"]').parent('li').addClass('active');
      }
      if (cur_pos < 300) {
        $(".nav-menu ul:first li:first, .mobile-nav ul:first li:first").addClass('active');
      }
    });
  });

  // Mobile Navigation
  if ($('.nav-menu').length) {
    var $mobile_nav = $('.nav-menu').clone().prop({
      class: 'mobile-nav d-lg-none'
    });
    $('body').append($mobile_nav);
    $('body').prepend('<button type="button" class="mobile-nav-toggle d-lg-none"><i class="icofont-navigation-menu"></i></button>');
    $('body').append('<div class="mobile-nav-overly"></div>');

    $(document).on('click', '.mobile-nav-toggle', function(e) {
      $('body').toggleClass('mobile-nav-active');
      $('.mobile-nav-toggle i').toggleClass('icofont-navigation-menu icofont-close');
      $('.mobile-nav-overly').toggle();
    });

    $(document).on('click', '.mobile-nav .drop-down > a', function(e) {
      e.preventDefault();
      $(this).next().slideToggle(300);
      $(this).parent().toggleClass('active');
    });

    $(document).click(function(e) {
      var container = $(".mobile-nav, .mobile-nav-toggle");
      if (!container.is(e.target) && container.has(e.target).length === 0) {
        if ($('body').hasClass('mobile-nav-active')) {
          $('body').removeClass('mobile-nav-active');
          $('.mobile-nav-toggle i').toggleClass('icofont-navigation-menu icofont-close');
          $('.mobile-nav-overly').fadeOut();
        }
      }
    });
  } else if ($(".mobile-nav, .mobile-nav-toggle").length) {
    $(".mobile-nav, .mobile-nav-toggle").hide();
  }
  // Toggle .header-scrolled class to #header when page is scrolled
  $(window).scroll(function() {
    if ($(this).scrollTop() > 100) {
      $('#header').addClass('header-scrolled');
      $('#topbar').addClass('topbar-scrolled');
    } else {
      $('#header').removeClass('header-scrolled');
      $('#topbar').removeClass('topbar-scrolled');
    }
  });

  if ($(window).scrollTop() > 100) {
    $('#header').addClass('header-scrolled');
    $('#topbar').addClass('topbar-scrolled');
  }
  // Back to top button
  $(window).scroll(function() {
    if ($(this).scrollTop() > 100) {
      $('.back-to-top').fadeIn('slow');
    } else {
      $('.back-to-top').fadeOut('slow');
    }
  });

  $('.back-to-top').click(function() {
    $('html, body').animate({
      scrollTop: 0
    }, 1500, 'easeInOutExpo');
    return false;
  });

  // jQuery counterUp
  $('[data-toggle="counter-up"]').counterUp({
    delay: 10,
    time: 1000
  });

  // Testimonials carousel (uses the Owl Carousel library)
  $(".testimonials-carousel").owlCarousel({
    autoplay: true,
    dots: true,
    loop: true,
    responsive: {
      0: {
        items: 1
      },
      768: {
        items: 1
      },
      900: {
        items: 2
      }
    }
  });

  // Initiate the venobox plugin
  $(document).ready(function() {
    $('.venobox').venobox();
  });

  // Initiate the datepicker plugin
  $(document).ready(function() {
    $('.datepicker').datepicker({
      autoclose: true
    });
  });

})(jQuery);


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
                text: 'Distrubución de Edades'
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
}

$.ajax({
    type: 'GET', //post method
    url: '/Tablero/PiramidePoblacional', //ajaxformexample url
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

if ($('#CondicionesRiesgoBar').length) {

    var ctx = document.getElementById("CondicionesRiesgoBar");
    var CondicionesRiesgoBar = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["Diabetes", "Obesidad", "Embarazo", "Cancer", "Tabaquismo", "Alcoholismo", "Drogas"],
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
}

var varIdEmpresa = document.getElementById("HdnIdEmpresa").value


$.ajax({
    type: 'GET', //post method
    url: '/Tablero/CondicionesRiesgo', //ajaxformexample url
    data: { idEmpresa: varIdEmpresa},
    dataType: "json",
    success: function (result, textStatus, jqXHR) {
        //CondicionesRiesgoBar.data.labels = result.labels;
        CondicionesRiesgoBar.data.datasets[0].data = result.countsMujeres;
            CondicionesRiesgoBar.data.datasets[1].data =  result.countsHombres;
        CondicionesRiesgoBar.update();
    }
});



if ($('#pieChart').length) {

    var ctx = document.getElementById("pieChart");
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

    var pieChart = new Chart(ctx, {
        data: data,
        type: 'pie',
        otpions: {
            legend: false
        }
    });
}

$.ajax({
    type: 'GET', //post method
    url: '/Tablero/EmpleadosContactoCovid', //ajaxformexample url,
    data: { idEmpresa: varIdEmpresa },
    dataType: "json",
    success: function (result, textStatus, jqXHR) {
        pieChart.data.labels = result.labels;
        pieChart.data.datasets[0].data = result.counts;
        pieChart.update();
    }
});



if ($('#RiesgosExpocisionCasaTransporte').length) {
    var ctx = document.getElementById('RiesgosExpocisionCasaTransporte').getContext('2d');
    var RiesgosExpocisionCasaTransporte = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ['> 3 personas vivienda', 'Multiples Familias', 'Transporte Publico'],
            datasets: [{
                label: 'Riesgo casa y transporte',
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
}

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

if ($('#RiesgosEspacioLaboral').length) {
    var ctx = document.getElementById('RiesgosEspacioLaboral').getContext('2d');
    var RiesgosEspacioLaboral = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ['Espacio de trabajo cerrado', 'Sin ventilación'],
            datasets: [{
                label: 'Riesgo Espacion Laboral',
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
}

$.ajax({
    type: 'GET', //post method
    url: '/Tablero/RiesgosEspacioLaboral', //ajaxformexample url
    data: { idEmpresa: varIdEmpresa },
    dataType: "json",
    success: function (result, textStatus, jqXHR) {
        //piramidePoblacional.data.labels = result.labels;
        RiesgosEspacioLaboral.data.datasets[0].data = result.counts;

        RiesgosExpocisionCasaTransporte.update();
    }
});


if ($('#SintomasCOVID').length) {
    var ctx = document.getElementById('SintomasCOVID').getContext('2d');
    var SintomasCOVID = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ['Síntomas COVID', 'Otros'],
            datasets: [{
                label: 'Sítomas Covid',
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
}

$.ajax({
    type: 'GET', //post method
    url: '/Tablero/SintomasCOVID', //ajaxformexample url
    data: { idEmpresa: varIdEmpresa },
    dataType: "json",
    success: function (result, textStatus, jqXHR) {
        //piramidePoblacional.data.labels = result.labels;
        RiesgosEspacioLaboral.data.datasets[0].data = result.counts;

        RiesgosExpocisionCasaTransporte.update();
    }
});


if ($('#EmpleadosAnosmiaHiposmia').length) {

    var ctx = document.getElementById("EmpleadosAnosmiaHiposmia");
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

    var pieChartAnosmiaHiposmia = new Chart(ctx, {
        data: data,
        type: 'pie',
        otpions: {
            legend: false
        }
    });
}

$.ajax({
    type: 'GET', //post method
    url: '/Tablero/EmpleadosAnosmiaHiposmia', //ajaxformexample url,
    data: { idEmpresa: varIdEmpresa },
    dataType: "json",
    success: function (result, textStatus, jqXHR) {
        pieChartAnosmiaHiposmia.data.labels = result.labels;
        pieChartAnosmiaHiposmia.data.datasets[0].data = result.counts;
        pieChartAnosmiaHiposmia.update();
    }
});



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
}

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


//M2. Empleados con condiciones constantes de riesgo de contagio. -T4, T5-
if ($('#EmleadosCondicionesConstantes').length) {

    var ctx = document.getElementById("EmleadosCondicionesConstantes");
    var emleadosCondicionesConstantes = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: '',
                backgroundColor: "#26B99A",
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

var varIdEmpresa = document.getElementById("HdnIdEmpresa").value


$.ajax({
    type: 'GET', //post method
    url: '/Tablero/EmpleadosCondicionesConstantes', //ajaxformexample url
    data: { idEmpresa: varIdEmpresa },
    dataType: "json",
    success: function (result, textStatus, jqXHR) {
        emleadosCondicionesConstantes.data.labels = result.labels;
        emleadosCondicionesConstantes.data.datasets[0].data = result.counts;
        emleadosCondicionesConstantes.update();
    }
});

if ($('#EmpleadosRiesgoComplicaciones').length) {

    var ctx = document.getElementById("EmpleadosRiesgoComplicaciones");
    var empleadosRiesgoComplicaciones = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: '',
                backgroundColor: "#26B99A",
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

var varIdEmpresa = document.getElementById("HdnIdEmpresa").value

doAjax();

async function doAjax(args) {
    let result;

    try {
        result = await $.ajax({
            type: 'GET', //post method
            url: '/Tablero/EmpleadosRiesgoComplicacionContagio', //ajaxformexample url
            data: { idEmpresa: varIdEmpresa },
            dataType: "json",
            success: function (result, textStatus, jqXHR) {
                empleadosRiesgoComplicaciones.data.labels = result.labels;
                empleadosRiesgoComplicaciones.data.datasets[0].data = result.counts;
                empleadosRiesgoComplicaciones.update();
            }
        });

        return result;
    }
    catch (error) {
        console.error(error);
    }
}