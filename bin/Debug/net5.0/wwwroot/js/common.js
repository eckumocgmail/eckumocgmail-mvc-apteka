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
                    alert('Ошибка при обработки события ' + type + ' ' + e);
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

function $updateNode(container, component) {
    try {
        $clearNode(container);
        console.log(container.innerHTML, component.outerHTML);
        container.innerHTML=component.outerHTML;
    } catch (e) {
        alert($clearNode + ' error ' + e);
    }
}
