var loader = {
    Css: [{
        baseUrl: "/Scripts/CodeMirror/",
        scripts: ["lib/codemirror", "fold/foldgutter", "theme/mdn-like", "lint/lint"]
    }],
    Js: [{
        baseUrl: "/Scripts/CodeMirror/lib/",
        scripts: ["codemirror", "matchbrackets", "active-line"]
    }, {
        baseUrl: "/Scripts/CodeMirror/fold/",
        scripts: ["foldcode", "foldgutter", "brace-fold", "xml-fold"]
    }, {
        baseUrl: "/Scripts/CodeMirror/lint/",
        scripts: ["lint", "json-lint", "css-lint", "html-lint", "javascript-lint"]
    }, {
        baseUrl: "/Scripts/CodeMirror/language/",
        scripts: ["clike", "css", "htmlmixed", "javascript", "lua", "python", "xml"]
    }]
}
for (var i = 0; i < loader.Css.length; i++) {
    for (var ii = 0; ii < loader.Css[i].scripts.length; ii++) {
        document.write("<link rel='stylesheet' href='" + loader.Css[i].baseUrl + loader.Css[i].scripts[ii] + ".css' />");
    }
}
for (var i = 0; i < loader.Js.length; i++) {
    for (var ii = 0; ii < loader.Js[i].scripts.length; ii++) {
        document.write("<script type='text/javascript' src='" + loader.Js[i].baseUrl + loader.Js[i].scripts[ii] + ".js'></script>");
    }
}

var Mirror = function (id, language) {
    var editor = CodeMirror.fromTextArea(document.getElementById(id), {
        mode: language,
        theme: "mdn-like",
        indentUnit: 4,
        lineNumbers: true,
        lineWrapping: true,
        styleActiveLine: true,
        matchBrackets: true,
        foldGutter: true,
        gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter", "CodeMirror-lint-markers"]
    });
    return editor;
}
