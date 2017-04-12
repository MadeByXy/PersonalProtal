document.write("<link rel='stylesheet' href='/Scripts/CodeMirror/lib/codemirror.css' />");
document.write("<link rel='stylesheet' href='/Scripts/CodeMirror/fold/foldgutter.css' />");
document.write("<link rel='stylesheet' href='/Scripts/CodeMirror/theme/mdn-like.css' />");
document.write("<link rel='stylesheet' href='/Scripts/CodeMirror/lint/lint.css' />");

document.write("<script type='text/javascript' src='/Scripts/CodeMirror/lib/codemirror.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/lib/matchbrackets.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/lib/active-line.js'></script>");

document.write("<script type='text/javascript' src='/Scripts/CodeMirror/fold/foldcode.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/fold/foldgutter.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/fold/brace-fold.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/fold/xml-fold.js'></script>");

document.write("<script type='text/javascript' src='/Scripts/CodeMirror/lint/lint.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/lint/json-lint.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/lint/css-lint.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/lint/html-lint.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/lint/javascript-lint.js'></script>");

document.write("<script type='text/javascript' src='/Scripts/CodeMirror/language/clike.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/language/css.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/language/htmlmixed.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/language/javascript.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/language/lua.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/language/python.js'></script>");
document.write("<script type='text/javascript' src='/Scripts/CodeMirror/language/xml.js'></script>");

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
