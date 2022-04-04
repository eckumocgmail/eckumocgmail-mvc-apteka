function $table(properties) {
    let html = '<table>';
    html += '<tr><td>Свойство</td><td>Значение</td></tr>';
    const names = Object.getOwnPropertyNames(properties);
    for (let i = 0; i < names.length; i++) {
        html += '<tr><td>' + names[i] + '</td><td>' + properties[names[i]] + '</td></tr>';
    }
    html += '</table>';
    return html;
}