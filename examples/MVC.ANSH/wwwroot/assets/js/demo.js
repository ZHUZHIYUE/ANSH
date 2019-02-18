"use strict";
var demo = {
    init: function () {
        $("[data-demo-action='update']").each(function () {
            var t = $(this).parents(".card");
            $(this).click(function (e) {
                return e.preventDefault(),
                    app._loading.show(t, {
                        spinner: !0
                    }),
                    setTimeout(function () {
                        app._loading.hide(t)
                    },
                        2e3),
                    !1
            })
        }),
            $("[data-demo-action='expand']").each(function () {
                var t = $(this).parents(".card");
                $(this).click(function (e) {
                    return e.preventDefault(),
                        app._loading.show(t, {
                            spinner: !0
                        }),
                        $(this).toggleClass("active"),
                        t.toggleClass("card--expanded"),
                        app._crt(),
                        setTimeout(function () {
                            app._loading.hide(t)
                        },
                            1e3),
                        !1
                })
            }),
            $("[data-demo-action='invert']").each(function () {
                var t = $(this).parents(".card");
                t.hasClass("invert") && $(this).addClass("active"),
                    $(this).click(function (e) {
                        return e.preventDefault(),
                            $(this).toggleClass("active"),
                            t.toggleClass("invert"),
                            !1
                    })
            }),
            $("[data-demo-action='remove']").each(function () {
                var t = $(this).parents(".card");
                $(this).click(function (e) {
                    return e.preventDefault(),
                        app.card.remove(t),
                        !1
                })
            })
    }
};
document.addEventListener("DOMContentLoaded",
    function () {
        demo.init()
    });