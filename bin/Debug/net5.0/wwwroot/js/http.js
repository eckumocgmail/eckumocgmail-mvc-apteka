
/**
 * Выполнение Get запроса
 * @param {any} uri адрес
 * @param {any} params параметры
 */
function $get(uri, params) {

    return new Promise((resolve, reject) => {
        const url = location.origin + uri + $params(params);
        const request = new XMLHttpRequest();
        request.open('GET', url, true);
        request.responseType = "json";
        request.addEventListener('error', function () {
            reject(request.status);
        }, true);
        request.addEventListener('load', function () {

            const responseHeaders = request.getAllResponseHeaders().split('\r\n').filter(header => header.length > 0);
            resolve({
                status: request.status,
                response: request.response,
                headers: responseHeaders
            });
        }, true);
        request.send(null);
    });
}

/**
 * выполнение запроса POST
 */
function $post(uri, message) {
}

/**
 * выполнение запроса POST
 */
function $put(uri, message) {
}

/**
 * выполнение запроса POST
 */
function $delete(uri, message) {
}

/**
 * выполнение запроса POST
 */
function $options(uri, message) {
}


/**
 * выполнение запроса POST
 */
function $patch(uri, message) {
}





/**
 * 
 * @param {any} file
 * @param {any} url
 * @param {any} onload 
 * @param {any} onerror
 * @param {any} onprogress
 * @param {any} onstart
 * @param {any} oncomplete
 */
function $upload(file, filename, mimetype, url, onload, onerror, onprogress, onstart, oncomplete) {
    var request = new XMLHttpRequest();
    request.responseType = "json";
    request.addEventListener('error', function (err) {
        if (onerror)
            onerror(err);
    });
    request.addEventListener('load', function () {
        if (!request.response.Success) {
            if (onerror)
                onerror(request.response.Message);         
        } else {
            if (onload)
                onload(request.response.Result);
        }
    });
    request.addEventListener('readystatechange', function ( ) {
        switch (request.readyState) {
            case 1: 
                if (onstart)
                onstart(); break;
            case 4: if (oncomplete)
                oncomplete(); break;
        }
    });
    request.addEventListener('progress', function (e) {
        var done = e.position || e.loaded, total = e.totalSize || e.total;
        const value = (Math.floor(done / total * 1000) / 10);
        if (onprogress)
            onprogress(value);
    }, false);
    request.open('POST', url, true);
    request.setRequestHeader('Resource-Name', filename);
    request.setRequestHeader('Mime-Type', mimetype);
    request.setRequestHeader('Content-Type', 'multipart/form-data');
    request.send(file);
}


/**
 * строка запроса 
 */
function $params(params) {
    let paramsStr = '';
    if (params) {
        const names = Object.getOwnPropertyNames(params);
        for (let i = 0; i < names.length; i++) {
            const key = names[i];
            const value = encodeURI(params[names[i]]);
            if (paramsStr.length == 0) {
                paramsStr += '?' + key + '=' + value;
            } else {
                paramsStr += '&' + key + '=' + value;
            }
        }
    }
    return paramsStr;
}

/**
 * Констроллер
 **/
const $http = {
    $get:       $get,
    $post:      $post,
    $put:       $put,
    $delete:    $delete,
    $patch:     $patch,
    $options:   $options,
    $upload:    $upload
}
