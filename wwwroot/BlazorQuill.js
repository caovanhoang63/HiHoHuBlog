(function () {
    window.QuillFunctions = {        
        createQuill: function (
            quillElement, toolBar, readOnly,
            placeholder, theme, debugLevel, customFonts) {  

            Quill.register('modules/blotFormatter', QuillBlotFormatter.default);

            var options = {
                debug: debugLevel,
                modules: {
                    toolbar: toolBar,
                    blotFormatter: {}
                },
                placeholder: placeholder,
                readOnly: readOnly,
                theme: theme
            };

            if (customFonts != null) {
                var fontAttributor = Quill.import('formats/font');
                fontAttributor.whitelist = customFonts;
                Quill.register(fontAttributor, true);

            }
            new Quill(quillElement, options);
        },

        bindOnChangeEvent: function (element, dotNetHelper) {
            if (!element || !element.quill) return;

            element.quill.on('text-change', function () {
                var content = element.quill.root.innerHTML;
                dotNetHelper.invokeMethodAsync('OnChangeHandler', content);
            });
        },


        getQuillContent: function(quillElement) {
            return JSON.stringify(quillElement.__quill.getContents());
        },
        getQuillText: function(quillElement) {
            return quillElement.__quill.getText();
        },
        getQuillHTML: function(quillElement) {
            return quillElement.__quill.root.innerHTML;
        },
        loadQuillContent: function(quillElement, quillContent) {
            content = JSON.parse(quillContent);
            return quillElement.__quill.setContents(content, 'api');
        },
        loadQuillHTMLContent: function (quillElement, quillHTMLContent) {
            return quillElement.__quill.root.innerHTML = quillHTMLContent;
        },
        enableQuillEditor: function (quillElement, mode) {
            quillElement.__quill.enable(mode);
        },
        insertQuillImage: function (quillElement, imageURL) {
            var Delta = Quill.import('delta');
            editorIndex = 0;

            if (quillElement.__quill.getSelection() !== null) {
                editorIndex = quillElement.__quill.getSelection().index;
            }

            return quillElement.__quill.updateContents(
                new Delta()
                    .retain(editorIndex)
                    .insert({ image: imageURL },
                        { alt: imageURL }));
        },
    };
})();