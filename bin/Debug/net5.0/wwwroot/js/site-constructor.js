const pageCtrl = $contentLayout();


pageCtrl.$view($content('view', () => { alert(1); }));
pageCtrl.$left($content('left', () => { alert(1); }));
pageCtrl.$top($content('top', () => { alert(1); }));
pageCtrl.$bottom($content('bottom', () => { alert(1); }));