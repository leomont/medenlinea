//Funcion para visualizar mensajes enviados por el cliente o el servidor
function messageSuccess(title, message) {
    var mensaje = $('<h4>' + title + '</h4>' + '<p>' + message + '</p>');
    $("#msj-success").html(mensaje);
    if (!$('#msj-success').is('.in')) {
        $('#msj-success').addClass('in');
        setTimeout(function () {
            $('#msj-success').removeClass('in');
        }, 5000);
    }
}
//Funcion para visualizar mensajes de error enviados por el cliente o el servidor
function messageError(title, error) {
    var mensaje = $('<h4>' + title + '</h4>' + '<p>' + error + '</p>');
    $("#msj-error").html(mensaje);
    if (!$('#msj-error').is('.in')) {
        $('#msj-error').addClass('in');
        setTimeout(function () {
            $('#msj-error').removeClass('in');
        }, 5000);
    }
}