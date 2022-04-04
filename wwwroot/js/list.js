function $list(items, onclick) {
    const ul = $element('ul', {}, { 'class': 'list-group' }, { 'click': onclick });

    for (let i = 0; i < items.length; i++) {
        const li = $element('li', {}, { id: 'p_' + i, 'class': 'list-group-item' }, {});
        li.innerHTML = items[i];
        ul.appendChild(li);
    }
    return ul;

}
