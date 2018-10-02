
initMCEall();
function initMCEall() {
    
    tinymce.init({
        mode: "textareas",
        theme: "modern",
        height: 400,
        width: 500,
        plugins: 'textcolor colorpicker image code searchreplace emoticons table wordcount advlist lists contextmenu paste  ',
        toolbar: "imageupload  forecolor   backcolor undo redo  link image code searchreplace emoticons table advlist styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | ",
        advlist_bullet_styles: "square",
        menubar: true ,
        image_dimensions: false,
        image_class_list: [
            { title: 'Responsive', value: 'img-responsive' }
        ],
       



    });

}


