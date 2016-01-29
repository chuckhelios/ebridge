// set the jquery sliders

$(function () {
    $(".slider").each(
        function (index) {
            console.log(index);
            console.log($(this).attr("name"));
            $(this).slider({
                range: "max",
                min: 1,
                max: 10,
                value: 2,
                slide: function (event, ui) {
                    $("input[name='"+$(this).attr("name")+"']").val(ui.value);
                }
            });
            //$("#value_"+this.attr("id")).val($("#slider-range-max").slider("value"));
        }
    )
});

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});
