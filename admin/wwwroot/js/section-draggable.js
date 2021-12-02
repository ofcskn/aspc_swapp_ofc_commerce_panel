$(document).ready(function () {
    $("#sectionHtml").sortable({
        handle: ".section-bar-top",
        connectWith: ".section-bar",
        items: ".section-bar",
        opacity: 0.8,
        coneHelperSize: true,
        placeholder: 'portlet-sortable-placeholder',
        forcePlaceholderSize: true,
        tolerance: "pointer",
        helper: "clone",
        tolerance: "pointer",
        forcePlaceholderSize: !0,
        helper: "clone",
        cancel: ".portlet-sortable-empty, .portlet-fullscreen", // cancel dragging if portlet is in fullscreen mode
        revert: 250, // animation in milliseconds
        update: function (b, c) {
            if (c.item.prev().hasClass("portlet-sortable-empty")) {
                c.item.prev().before(c.item);
            }

            orderSection();
        }
    });
    $("#sliderHtml").sortable({
        handle: ".section-bar-top",
        connectWith: ".section-bar",
        items: ".section-bar",
        opacity: 0.8,
        coneHelperSize: true,
        placeholder: 'portlet-sortable-placeholder',
        forcePlaceholderSize: true,
        tolerance: "pointer",
        helper: "clone",
        tolerance: "pointer",
        forcePlaceholderSize: !0,
        helper: "clone",
        cancel: ".portlet-sortable-empty, .portlet-fullscreen", // cancel dragging if portlet is in fullscreen mode
        revert: 250, // animation in milliseconds
        update: function (b, c) {
            if (c.item.prev().hasClass("portlet-sortable-empty")) {
                c.item.prev().before(c.item);
            }

            orderSlider();
        }
    });
});