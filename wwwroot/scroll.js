window.addScrollListener = (dotNetHelper) => {
    window.onscroll = () => {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 500) {
            dotNetHelper.invokeMethodAsync('LoadMoreBlogs');
        }
    };
};

window.removeScrollListener = () => {
    window.onscroll = null;
};