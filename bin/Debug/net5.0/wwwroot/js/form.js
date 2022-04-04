
function $button(title, onclick) {
    const button = $element('button', {}, { class: 'btn btn-primary' }, { click: onclick });
    button.innerHTML = title;
    button.todo = function () {
        onclick();
    }
    button.addEventListener('click', function () {
        button.todo();
    });
    return button;
}

function $content(title, onclick) {
    const button = $element('button', {}, { class: 'btn btn-primary', style: 'width: 100%;' }, { click: onclick });
    button.innerHTML = title;
    return button;
}



//<!-- заголовок -->
function $title(text) {
    const pnode = document.createElement('span');
    pnode.innerHTML = text;
    pnode.style.fontSize = '22px';
    return pnode;
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
function $inputSearch(label, name, value, options, oninput, onsearch) {
    const pnode = document.createElement('div');
    pnode.addEventListener('keypress', function (evt) {

        if (evt.keyCode == 13) {
            console.log(onsearch);
            try {
                eval(onsearch);
            } catch (e) {
                alert('$inputSearch(...) При обработки события keypress Ошибка ' + e);
            }
        }
    }, true);
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
        $content(label, name, value, options, oninput, onsearch) {
            let optionsContent = '';
            for (let i = 0; i < options.length; i++) {
                optionsContent += '<option>' + options[i] + '</option>';
            }
            return `
                <div class="input-group mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text">`+ label + `</span>
                    </div>
                    <input list="`+ name + `Options" type="text" oninput="` + oninput + `" value="` + value + `" id="` + name + `" name="` + name + `" class="form-control"/>
                    <datalist id="`+ name + `Options">` + optionsContent + `</datalist>
                </div>`;
        },
        $update(label, name, value, options, oninput, onsearch) {
            const content = ctrl.$content.apply(this, arguments);
            pnode.innerHTML = content;
        },
        $updateSearchOptions(list) {
            try {
                const datalist = document.getElementById(name + 'Options');
                $clearNode(datalist);
                let optionsContent = '';
                for (let i = 0; i < list.length; i++) {
                    optionsContent += '<option>' + list[i] + '</option>';
                }
                datalist.innerHTML = optionsContent;
            } catch (e) {
                alert('$updateOptions(...) ошибка ' + e);
            }
        },
        $layout(pcontainer) {
            if (!pcontainer)
                throw new Error('В аргумент pcontainer передано не действительное значение');
            pcontainer.appendChild(pnode);
        }
    };
    ctrl.$update(label, name, value, options, oninput, onsearch);
    ctrl.pnode = pnode;
    return ctrl;
}

