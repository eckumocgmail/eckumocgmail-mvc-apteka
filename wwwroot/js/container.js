

//<!-- Макет страницы поиска продукции -->
function $contentLayout(container) {
    if (!container) {
        console.warn('$contentLayout(...) container аргумент вызова ссылается на недействительное значение');
        container = $element('div', { id: 'layout' }, {},);
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
