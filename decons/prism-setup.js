var els = document.querySelectorAll('div.org-src-container pre');

els.forEach(el => {
    el.className += " language-csharp";
    Prism.highlightElement(el);
});


