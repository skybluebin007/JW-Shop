var editor;
KindEditor.ready(function (K) {
    editor = K.create('.textarea', {
        uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
        fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
        allowFileManager: true,

    });
});