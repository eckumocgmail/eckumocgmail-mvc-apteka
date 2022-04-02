function $element(tag, options, attributes, events) {
    const pnode = document.createElement(tag);
    if (options) {
        Object.assign(pnode, options);
    }
    if (attributes) {
        const names = Object.getOwnPropertyNames(attributes);
        for (let i = 0; i < names.length; i++) {
            pnode.setAttribute(names[i], attributes[names[i]]);
        }
    }
    if (events) {
        const names = Object.getOwnPropertyNames(events);
        for (let i = 0; i < names.length; i++) {
            const type = names[i].startsWith('on') ? names[i].substring(2) : names[i];
            const handler = events[names[i]];
            const action = function () {
                try {
                    return handler.apply(this, arguments);
                } catch (e) {
                    alert('Ошибка при обработки события '+type+ ' '+e);
                }
            }
            pnode.addEventListener(type, action, true);
        }
    }
    document.body.appendChild(pnode);
    return pnode;
}

function $global() {
    if (typeof (window['ctrl']) == 'undefined') {
        return window['ctrl'] = {
        };
    } else {
        return window['ctrl'];
    }
};

function $clearNode(searchStatusSlot) {
    try {
        for (let i = (searchStatusSlot.childNodes.length - 1); i >= 0; i--) {
            searchStatusSlot.removeChild(searchStatusSlot.childNodes[i]);
        }
    } catch (e) {
        alert($clearNode + ' error ' + e);
    }
}

function $button(title, onclick) {
    const button = $element('button', {}, { class: 'btn btn-primary' }, { click: onclick });
    button.innerHTML = title;
    return button;
}

function $content(title, onclick) {
    const button = $element('button', {}, { class: 'btn btn-primary', style: 'width: 100%;' }, { click: onclick });
    button.innerHTML = title;
    return button;
}


function $http() {
    const ctrl = {

        $get(uri, params) {

            return new Promise((resolve, reject) => {

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


                const request = new XMLHttpRequest();

                const url = location.origin + uri + paramsStr;
                console.log(url);
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

    };
    return ctrl;
}



//<!-- Макет страницы поиска продукции -->
function $contentLayout(container) {
    if (!container) {
        console.warn('$contentLayout(...) container аргумент вызова ссылается на недействительное значение');
        container = $element('div', { id: 'layout' }, {}, );
    }
    const ctrl = {
        $clear: function () {
            if (container.id == '')
                throw new Error('Не установлен атрибут id у контейнера: ' + container.outerHTML);
            $clearNode(container);
            container.innerHTML = `
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12" id="`+ container.id + `TopSlot">

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-4" id="`+ container.id + `LeftSlot"></div>
                        <div class="col-8" id="`+ container.id + `ViewSlot"></div>
                    </div>
                    <div class="row">
                        <div class="col-12" id="`+ container.id + `BottomSlot"></div>
                    </div>
                </div>
            `;
        },
        $top: function (top) {
            const topId = container.id + "TopSlot";
            try {
                const topNode = document.getElementById(topId);
                $clearNode(topNode);
                topNode.appendChild(top);
            } catch (e) {
                alert('$contentLayout.$top(..) ошибка ' + e);
            }
        },
        $bottom: function (bottom) {
            const bottomId = container.id + "BottomSlot";
            try {
                const bottomNode = document.getElementById(bottomId);
                $clearNode(bottomNode);
                bottomNode.appendChild(bottom);
            } catch (e) {
                alert('$contentLayout.$bottom(..) ошибка ' + e);
            }
        },
        $left: function (left) {
            const leftId = container.id + "LeftSlot";
            try {
                const leftNode = document.getElementById(leftId);
                $clearNode(leftNode);
                leftNode.appendChild(left);
            } catch (e) {
                alert('$contentLayout.$left(..) ошибка ' + e);
            }
        },
        $view: function (view) {
            const viewId = container.id + "ViewSlot";
            try {
                const viewNode = document.getElementById(viewId);
                $clearNode(viewNode);
                viewNode.appendChild(view);
            } catch (e) {
                alert('$contentLayout.$view(..) ошибка ' + e);
            }
        },
    }
    try {
        ctrl.$clear();
    } catch (e) {
        alert('$contentLayout(..) ошибка ' + e);
    }
    return ctrl;
}

//<!-- поле ввода -->
function $input() {
    const pnode = document.createElement('div');
    const ctrl = {
        $content(label, type, name, value, placeholder, oninput) {
            return `
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">`+ label + `</span>
                    </div>
                    <input oninput="`+ oninput + `" name="` + name + `" type="` + type + `" class="form-control" placeholder="` + placeholder + `" />
                </div>`;
        },
        $update(label, type, name, value, placeholder, oninput) {
            const content = ctrl.$content.apply(this, arguments);
            pnode.innerHTML = content;
        },
        $layout(pcontainer) {
            pcontainer.appendChild(pnode);
        }
    };

    return ctrl;
}


//<!-- поле выбора выпадающим списком -->
function $inputSelect(label, name, value, options, oninput) {
    const pnode = document.createElement('div');
    const ctrl = {
        $content(label, name, value, options, oninput) {
            let optionsContent = '';
            for (let i = 0; i < options.length; i++) {
                optionsContent += '<option>' + options[i] + '</option>';
            }
            return `
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">`+ label + `</span>
                    </div>
                    <select oninput="`+ oninput + `" value="` + value + `" name="` + name + `" class="form-control">
                    `+ optionsContent + `
                    </select>
                </div>`;
        },
        $update(label, name, value, options, oninput) {
            const content = ctrl.$content.apply(this, arguments);
            pnode.innerHTML = content;
        },
        $layout(pcontainer) {
            pcontainer.appendChild(pnode);
        }
    };
    return ctrl;
}

//<!-- ввод диапазона --> 
function $inputRange(label, type, name, minValue, maxValue, placeholder, oninput) {
    const pnode = document.createElement('div');
    const ctrl = {
        $getMinValue() {
            try {
                const pinput = document.getElementById(name + 'Min');
                if (!pinput)
                    throw new Error('Не найден элемент с id=' + name + 'Min');
                return pinput.value;
            } catch (e) {
                alert('$inputRange(..).$getMinValue(..) Ошибка ' + e);
            }
        },
        $getMaxValue() {
            try {
                const pinput = document.getElementById(name + 'Max');
                if (!pinput)
                    throw new Error('Не найден элемент с id=' + name + 'Max');
                return pinput.value;
            } catch (e) {
                alert('$inputRange(..).$getMinValue(..) Ошибка ' + e);
            }
        },
        $getRange() {
            return '[' + ctrl.$getMinValue() + ',' + ctrl.$getMaxValue() + ']';
        },
        $content(label, type, name, minValue, maxValue, placeholder, oninput) {
            return `
                <div class="input-group d-flex flex-row flex-nowrap" style="width: 100%;">
                    <div class="input-group-prepend">
                        <span class="input-group-text" style="width: 80px;">`+ label + `</span>
                    </div>
                    <input id="`+ name + `Min" name="` + name + `Min" type="` + type + `" value="` + minValue + `" class="form-control" style="width: 100%;" />
                    <input id="`+ name + `Max" name="` + name + `Max" type="` + type + `" value="` + maxValue + `" class="form-control" style="width: 100%;" />
                </div>`;
        },
        $update(label, type, name, value, placeholder, oninput) {
            const content = ctrl.$content.apply(this, arguments);
            pnode.innerHTML = content;
        },
        $layout(pcontainer) {
            pcontainer.appendChild(pnode);
        }
    }
    ctrl.$update(label, type, name, minValue, maxValue, placeholder, oninput);
    return ctrl;
}


//<!-- заголовок -->
function $title(text) {
    const pnode = document.createElement('span');
    pnode.innerHTML = text;
    pnode.style.fontSize = '22px';
    return pnode;
}



//<!-- карточка -->
function $card(title, subtitle, text, ahref, atext) {
    const cardNode = document.createElement('div');
    cardNode.innerHTML = `
        <div class="card">
            <h5 class="card-header">`+ title + `</h5>
            <div class="card-body">
                <h5 class="card-title">`+ subtitle + `</h5>
                <p class="card-text">`+ text + `</p>
                <a href="`+ ahref + `" class="btn btn-primary">` + atext + `</a>
            </div>
        </div>
    `;
    return cardNode;
}



//<!-- результат поиска -->
function $searchResults() {
    const pnode = document.createElement('div');
    const ctrl = {
        $clear: function () {
            $clearNode(pnode);
        },
        $update: function (searchResults) {
            try {
                if (!(searchResults instanceof Array))
                    throw new Error('Функция обновления результатов поиска должна принимать аргумент типа массив');
                ctrl.$clear();
                if (searchResults.length == 0) {
                    pnode.innerHTML = `<div class="alert alert-danger">Нет результатов</div>`;
                } else {

                    for (let i = 0; i < searchResults.length; i++) {
                        const result = searchResults[i];
                        const resultPane = $card(result.title, result.subtitle, result.text, result.ahref, result.atext);
                        pnode.appendChild(resultPane);
                    }
                }


            } catch (e) {
                alert('$searchResults.$update( ... ) ошибка ' + e);
            }
        },
        $layout(pcontainer) {
            pcontainer.appendChild(pnode);
        }
    };
    ctrl.pnode = pnode;
    ctrl.$update([]);
    return ctrl;
}

//<!--поле выбора выпадающим списком -->
function $inputSearch(label, name, value, options, oninput) {
    const pnode = document.createElement('div');
    const ctrl = {
        $options: function () {
            try {
                const pnode = document.getElementById(name + "Options");
                $clearNode(pnode);
                let optionsContent = '';
                for (let i = 0; i < options.length; i++) {
                    optionsContent += '<option>' + options[i] + '</option>';
                }
                pnode.innerHTML = optionsContent;
            } catch (e) {
                alert('$inputSearch().$options(...) Ошибка ' + e);
            }
        },
        $content(label, name, value, options, oninput) {
            let optionsContent = '';
            for (let i = 0; i < options.length; i++) {
                optionsContent += '<option>' + options[i] + '</option>';
            }
            return `
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">`+ label + `</span>
                    </div>
                    <input list="`+ name + `Options" type="text" oninput="` + oninput + `" value="` + value + `" name="` + name + `" class="form-control"/>
                    <datalist id="`+ name + `Options">` + optionsContent + `</datalist>
                </div>`;
        },
        $update(label, name, value, options, oninput) {
            const content = ctrl.$content.apply(this, arguments);
            pnode.innerHTML = content;
        },
        $updateSearchOptions( list ) {
            try {
                const datalist = document.getElementById(name + 'Options');
                $clearNode(datalist);
                let optionsContent = '';
                for (let i = 0; i < list.length; i++) {
                    optionsContent += '<option>' + list[i] + '</option>';
                }
                datalist.innerHTML = optionsContent;
            } catch (e) {
                alert('$updateOptions(...) ошибка '+e);
            }
        },
        $layout(pcontainer) {
            if (!pcontainer)
                throw new Error('В аргумент pcontainer передано не действительное значение');
            pcontainer.appendChild(pnode);
        }
    };
    ctrl.$update(label, name, value, options, oninput);
    ctrl.pnode = pnode;
    return ctrl;
}

