"use strict";
var plugins = {
    mCustomScrollBar: function () {
        $(".scroll").mCustomScrollbar({
            axis: "y",
            theme: "minimal-dark",
            autoHideScrollbar: !0,
            scrollInertia: 300,
            advanced: {
                autoScrollOnFocus: !1
            }
        }),
            window.addEventListener("resize",
                function () {
                    $(".scroll").mCustomScrollbar("update")
                },
                !0)
    },
    bs_inits: function () {
        $('[data-toggle="popover"]').popover(),
            $('[data-toggle="tooltip"]').tooltip()
    },
    select2: function () {
        $(".select-simple").length > 0 && $(".select-simple").select2({
            minimumResultsForSearch: -1
        }),
            $(".select-default").length > 0 && $(".select-default").select2(),
            $(".select-clearable").length > 0 && $(".select-clearable").select2({
                placeholder: "Choose option...",
                allowClear: !0
            })
    },
    smartWizard: function () {
        $(".wizard").length > 0 && ($(".wizard > ul").each(function () {
            $(this).addClass("steps_" + $(this).children("li").length)
        }), $(".wizard").smartWizard({
            onLeaveStep: function (t) {
                var e = t.parents(".wizard");
                if (e.hasClass("wizard-validation")) {
                    var a = !0;
                    if ($("input,textarea", $(t.attr("href"))).each(function (t, e) {
                        a = validate.element(e) && a
                    }), !a) return e.find(".stepContainer").removeAttr("style"),
                        validate.focusInvalid(),
                        !1
                }
                return app._crt(),
                    !0
            },
            onShowStep: function (t) {
                t.parents(".wizard").hasClass("show-submit") && (t.attr("rel") == t.parents(".anchor").find("li").length && t.parents(".wizard").find(".actionBar .btn-secondary").css("display", "block"));
                return app._crt(),
                    !0
            }
        }))
    },
    maskedInput: function () {
        $("input[class^='mask_']").length > 0 && ($("input.mask_tin").mask("99-9999999"), $("input.mask_ssn").mask("999-99-9999"), $("input.mask_date").mask("9999-99-99"), $("input.mask_date_rev").mask("99-99-9999"), $("input.mask_product").mask("a*-999-a999"), $("input.mask_phone").mask("99 (999) 999-99-99"), $("input.mask_phone_ext").mask("99 (999) 999-9999? x99999"), $("input.mask_credit").mask("9999-9999-9999-9999"), $("input.mask_percent").mask("99%"))
    },
    extension: function () {
        $.expr[":"].containsi = function (t, e, a) {
            return jQuery(t).text().toUpperCase().indexOf(a[3].toUpperCase()) >= 0
        }
    }
};
document.addEventListener("DOMContentLoaded",
    function () {
        plugins.mCustomScrollBar(),
            plugins.bs_inits(),
            plugins.select2(),
            plugins.smartWizard(),
            plugins.maskedInput(),
            plugins.extension()
    }),
    window.addEventListener("resize",
        function () { },
        !0);