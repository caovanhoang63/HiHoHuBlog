window.ChangeUrl = function(url){
    history.replaceState(null, '', url);
}
