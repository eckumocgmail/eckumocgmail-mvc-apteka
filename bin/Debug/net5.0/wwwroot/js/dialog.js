/**
 * Модальное диалоговое окно
 * @param {any} handleElements функция принимающая элементы управления
 */
function $dialog(handleElements) {
    if (document.getElementById('exampleModalLong') == null) {
        const node = document.createElement('div');
        node.innerHTML = `
            <!-- Button trigger modal -->
            <button type="button" class="btn btn-primary"
                    id="toggleModalBtn"
                    style="display: none;"
                    data-toggle="modal" data-target="#exampleModalLong">
              Launch demo modal
            </button>

            <!-- Modal -->
            <div class="modal fade" id="exampleModalLong" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
              <div class="modal-dialog" role="document">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="modalHeader">Modal title</h5>
                  </div>
                  <div class="modal-body" id="modalBody">
                  
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="submitModalBtn" data-dismiss="modal">
                        подтвердить </button>
                    <button type="button" class="btn btn-secondary"
                        data-dismiss="modal" id="cancelModalBtn">отменить</button>
                  </div>
                </div>
              </div>
            </div>`;
        document.body.appendChild(node);
    }
    document.getElementById('toggleModalBtn').click();
    const ctrl = {

        modalPane: document.getElementById('exampleModalLong'),
        cancelModalBtn: document.getElementById('cancelModalBtn'),
        submitModalBtn: document.getElementById('submitModalBtn'),
        modalHeader: document.getElementById('modalHeader'),
        modalBody: document.getElementById('modalBody')
    };
    handleElements(ctrl);
}


/**
 * Запрос подтверждения
 */
function $submitDialog(title, text, onsubmit, oncancel) {
    $dialog((ctrl) => {
        ctrl.modalHeader.innerHTML = title;
        ctrl.modalBody.innerHTML = text;
        const listener1 = function () {
            onsubmit();
            ctrl.submitModalBtn.removeEventListener('click', listener1, true);
            ctrl.cancelModalBtn.removeEventListener('click', listener2, true);
        };
        ctrl.submitModalBtn.addEventListener('click', listener1, true);

        const listener2 = function () {
            oncancel();
            ctrl.cancelModalBtn.removeEventListener('click', listener2, true);
            ctrl.submitModalBtn.removeEventListener('click', listener1, true);
        };
        ctrl.cancelModalBtn.addEventListener('click', listener2, true);
    });
}


/**
 * Запрос подтверждения
 */
function $nodeDialog(header, body, onstart, oncomplete ) {
    $dialog((ctrl) => {
        $clearNode(ctrl.modalHeader);
        ctrl.modalHeader.appendChild(header);
        $clearNode(ctrl.modalBody);
        ctrl.modalBody.appendChild(body);
 
        
        const listener1 = function () {
            if (oncomplete)oncomplete(true);
            ctrl.submitModalBtn.removeEventListener('click', listener1, true);
            ctrl.cancelModalBtn.removeEventListener('click', listener2, true);
        };
        ctrl.submitModalBtn.addEventListener('click', listener1, true);

        const listener2 = function () {
            if (oncomplete)oncomplete(false);
            ctrl.cancelModalBtn.removeEventListener('click', listener2, true);
            ctrl.submitModalBtn.removeEventListener('click', listener1, true);
        };
        ctrl.cancelModalBtn.addEventListener('click', listener2, true);
        if (onstart ) onstart();
    });
}


/**
 * $progressbar().$set( value )
 **/
function $progressbar() {
    const container = document.createElement('div');
    document.body.appendChild(container);
    const ctrl = {
        value: 0,
        min: 0,
        max: 100
    }    
    function $update() {
        container.innerHTML =
            `<div class="progress">
                <div id="p1" class="progress-bar" role="progressbar" style="width: `+ ctrl.value +`%" aria-valuenow="`+ ctrl.value +`" aria-valuemin="`+ ctrl.min +`" aria-valuemax="`+ ctrl.max +`"></div>
            </div>`;
    }
    ctrl.$set = function (val) {
        ctrl.value = val;
        $update();
    }
    $update();
    ctrl.pnode = container;
    return ctrl;
}


/**
 * Запрос подтверждения
 */
function $progressDialog() {
    
    const title = $title('Пожалуйста подождите');
    const progress = $progressbar();

    
    const html = {
        begin: function () { progress.$set(0); },
        progress: function (value) { progress.$set(value); },
        complete: function () { progress.$set(0); }
    };
    $nodeDialog(title, progress.pnode, html.begin, html.complete);
    let i = 0;
    setInterval(function () { progress.$set(i++); },100);
  
}