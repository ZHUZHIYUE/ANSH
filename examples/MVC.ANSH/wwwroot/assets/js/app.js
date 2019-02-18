"use strict";
var app = {
    settings: {
        animation: 190,
        animationPanel: 500,
        navigation: {
            detectAuto: !0,
            closeOther: !0,
            fixNavAlwaysDropUp: !1
        },
        headerHeight: 60,
        containerHeight: 60,
        boxedPaddings: 100,
        indentPaddings: 60,
        logo: '<div class="logo-text"><strong class="text-primary">#</strong> THE <strong>RIGHT WAY</strong></div>',
        backToTop: !0,
        backToTopHeight: 200,
        responsiveState: !1,
        breakpoints: {
            xs: 0,
            sm: 576,
            md: 768,
            lg: 1024,
            xl: 1200
        }
    },
    layout: {
        responsive: function () {
            var e = $("#page-content"),
                a = $("#page-aside"),
                n = $("#page-sidepanel");
            app.settings.responsiveState ? app.settings.responsiveState = !1 : (window.innerWidth <= app.settings.breakpoints.md && (a && (e.addClass("page-aside--hidden"), a.addClass("page-aside--hidden")), n && (e.addClass("page-sidepanel--hidden"), n.addClass("page-sidepanel--hidden"))), window.innerWidth <= app.settings.breakpoints.xl && n && (e.addClass("page-sidepanel--hidden"), n.addClass("page-sidepanel--hidden")))
        },
        controls: function () {
            var e = $("#page-aside"),
                a = $("#page-sidepanel"),
                n = $("[data-action='aside-minimize']"),
                t = $("[data-action='aside-hide']"),
                i = $("[data-action='sidepanel-hide']"),
                o = $("[data-action='horizontal-show']");
            n.length > 0 && app._controlPanelEvent(n, e, "page-aside-animation-show", "page-aside--minimized", !1, app.settings.breakpoints.md, "group1", $("#content")),
                t.length > 0 && app._controlPanelEvent(t, e, "page-aside-animation-show", "page-aside--hidden", $("#content"), app.settings.breakpoints.md, "group1"),
                i.length > 0 && app._controlPanelEvent(i, a, "page-sidepanel-animation-show", "page-sidepanel--hidden", $("#content"), app.settings.breakpoints.xl, "group2"),
                o && o.click(function (e) {
                    $(this).parent().toggleClass("horizontal-navigation--show")
                })
        },
        aside_fixed: function () {
            var e = $("#page-content");
            e.hasClass("page__content--w-aside-fixed") && $(window).on("scroll", app._debouncer(function () {
                var a = 0;
                $(".page__header").length > 0 && (a += app.settings.headerHeight),
                    $(".page__container").length > 0 && (a += app.settings.containerHeight),
                    window.pageYOffset > a ? e.addClass("page-aside-scrolled") : e.removeClass("page-aside-scrolled"),
                    $(".page-aside > .scroll").length > 0 && setTimeout(function () {
                        $(".page-aside > .scroll").mCustomScrollbar("update")
                    },
                        50)
            },
                100))
        },
        fixed_panel: function () {
            var e = $("#fixed_panel"),
                a = $("[data-action='fixedpanel-toggle']");
            e.length > 0 && a.each(function (a, n) {
                $(n).on("click",
                    function () {
                        e.hasClass("show") ? (app._backdrop.hide(), e.removeClass("show")) : (app._backdrop.show(!0), e.addClass("show"))
                    })
            })
        }
    },
    header_search: function () {
        var e = $("#header_search");
        if (0 === e.length) return !1;
        var a = e.find("input"),
            n = e.find("div");
        a.on("focus",
            function () {
                e.addClass("page-header-search--focus")
            }),
            n.on("mouseup",
                function () {
                    a.value = "",
                        a.focus()
                }),
            a.on("blur",
                function () {
                    e.removeClass("page-header-search--focus")
                })
    },
    navigation_detect_auto: function () {
        if (app.settings.navigation.detectAuto) {
            var e = window.location.pathname.split("/"),
                a = e[e.length - 1];
            $(".navigation a[href='" + a + "']").parent("li").addClass("active").parents(".openable").addClass("open active")
        }
    },
    navigation_quick_build: function (e, a) {
        var n = $("[id^='" + a + "']");
        e = $("#" + e);
        n.length > 0 && (app._loading.show(e.parent(), {
            spinner: !0
        }), n.each(function (a, n) {
            e.append($('<li><a href="#' + n.getAttribute("id") + '">' + n.innerHTML + "</a></li>"))
        }), setTimeout(function () {
            app._loading.hide(e.parent())
        },
            1e3))
    },
    navigation: function () {
        $(".navigation").each(function () {
            var e = $(this);
            e.find("a").each(function () {
                $(this).click(function (a) {
                    if ("#" === $(this).attr("href").charAt(0) && e.hasClass("navigation--quick")) {
                        if (a.preventDefault(), $(this).attr("href").length <= 1) return !1;
                        var n = $($(this).attr("href")),
                            t = n.parents(".card");
                        return t.length > 0 ? (t.removeClass("keepAttentionTo"), t.offsetWidth, $("html, body").animate({
                            scrollTop: t.offset().top - 20
                        },
                            app.settings.animation,
                            function () {
                                t.addClass("keepAttentionTo")
                            })) : window.scroll({
                                top: n.offset().top - 20,
                                left: 0,
                                behavior: "smooth"
                            }),
                            !1
                    }
                    if ($(this).next().is("UL")) {
                        a.preventDefault();
                        var i = $(this).parent();
                        if (i.hasClass("open")) return i.removeClass("open"),
                            !1;
                        if (app.settings.navigation.closeOther) {
                            var o = $(this).parents("li");
                            $(this).parents("ul").find("> li").not(o).removeClass("open")
                        }
                        return i.addClass("open"),
                            app.settings.responsiveState = !0,
                            app._crt(),
                            i.trigger("mouseenter"),
                            !1
                    }
                })
            }),
                app._navigationFix(e)
        })
    },
    file_tree: function () {
        $(".file-tree").each(function () {
            $(this).find("li.folder > a").each(function () {
                $(this).click(function (e) {
                    e.preventDefault();
                    var a = $(this).parent(),
                        n = $(this).find(".icon");
                    return a.hasClass("open") ? (a.removeClass("open"), n.length > 0 && (n.removeClass("fa-folder-open-o"), n.addClass("fa-folder-o"))) : (a.addClass("open"), n && (n.removeClass("fa-folder-o"), n.addClass("fa-folder-open-o"))),
                        app._crt(),
                        !1
                })
            })
        })
    },
    card: {
        remove: function (e, a) {
            return e.addClass("fadeOut", "animated"),
                setTimeout(function () {
                    e.remove()
                },
                    app.settings.animation),
                "function" == typeof a && a(),
                app._crt(),
                !1
        }
    },
    _navigationFix: function (e) {
        e.find("li").each(function () {
            var a = $(this);
            a.mouseenter(function (n) {
                n.preventDefault();
                var t = e.parent();
                if (t.hasClass("page-aside--minimized") || t.hasClass("navigation--minimized")) {
                    var i = $("#page-aside").offsetHeight,
                        o = a.find("UL")[0];
                    if (o) {
                        o.removeClass("height-control"),
                            o.height("auto");
                        var s = i - a.offsetTop,
                            d = a.offsetTop,
                            p = 0,
                            r = 0;
                        s > d ? p = s : (p = d, r = 1),
                            1 === r && (app.settings.navigation.fixNavAlwaysDropUp ? o.addClass("dropup") : s < o.offsetHeight && o.addClass("dropup")),
                            p - o.offsetHeight < 0 && (o.addClass("height-control"), o.height(p + "px"))
                    }
                }
            }),
                a.mouseleave(function (e) {
                    e.preventDefault();
                    var a = $(this).find("UL");
                    a && (a.removeClass("height-control dropup"), a.css({
                        top: "auto",
                        height: "auto"
                    }))
                })
        })
    },
    _controlPanelEvent: function (e, a, n, t, i, o, s, d) {
        var p = $("#page-content");
        e.each(function () {
            $(this).click(function (e) {
                if (e.preventDefault, a.removeClass(n), $(this).toggleClass("active"), a.offsetWidth, a.hasClass(t)) {
                    if (a.removeClass(t), a.addClass(n), p.removeClass(t), !1 !== i) {
                        if (a.hasClass("page-aside--minimized")) return !1;
                        window.innerWidth <= o && (i.addClass("hideContainerContent"), app._loading.show(i, {
                            id: s,
                            spinner: !0,
                            solid: !0
                        }))
                    }
                } else a.addClass(t, n),
                    p.addClass(t),
                    !1 !== i ? setTimeout(function () {
                        i.children(".loading").length <= 1 && i.removeClass("hideContainerContent"),
                            app._loading.hide(i, app.settings.animation, s)
                    },
                        app.settings.animation) : setTimeout(function () {
                            d.children(".loading").length <= 1 && d.removeClass("hideContainerContent"),
                                app._loading.hide(d, app.settings.animation, s)
                        },
                            app.settings.animation);
                app.settings.responsiveState = !0,
                    app._crt()
            })
        })
    },
    _fireResize: function () {
        if (- 1 !== navigator.userAgent.indexOf("MSIE") || navigator.appVersion.indexOf("Trident/") > 0) {
            var e = document.createEvent("UIEvents");
            e.initUIEvent("resize", !0, !1, window, 0),
                window.dispatchEvent(e)
        } else window.dispatchEvent(new Event("resize"))
    },
    _crt: function (e) {
        e = void 0 === e ? app.settings.animationPanel : 0;
        console.log("crt"),
            setTimeout(function () {
                app._fireResize()
            },
                e)
    },
    _backdrop: {
        show: function (e) {
            var a = $("<div>");
            a.addClass("backdrop"),
                void 0 !== e && a.addClass("backdrop--mtransparent"),
                $("body").append(a)
        },
        hide: function () {
            var e = $("body").find(".backdrop");
            e.addClass("fadeOut"),
                setTimeout(function () {
                    e.remove()
                },
                    app.settings.animation)
        }
    },
    _loading: {
        show: function (e, a) {
            var n = ["loading"],
                t = !1,
                i = !1,
                o = !1;
            if ("object" == typeof a && (void 0 !== a.spinner && !0 === a.spinner && (n.push("loading--w-spinner"), o = !0), void 0 !== a.dark && !0 === a.dark && n.push("loading--dark"), void 0 !== a.text && a.text.length > 0 && (n.push("loading--text"), i = a.text), void 0 !== a.solid && !0 === a.solid && n.push("loading--solid"), void 0 !== a.id && (t = a.id)), e) {
                e.addClass("loading-process");
                var s = $("<div>"),
                    d = $("<div>"),
                    p = $("<div>");
                i && d.html(i),
                    t && s.attr("id", "loading_layer_" + t),
                    o && (p.addClass("loading-spinner"), d.append(p)),
                    (o || i) && s.append(d);
                for (var r = 0; r < n.length; r++) s.addClass(n[r]);
                e.hasClass("preloading") && (e.addClass("loaded"), setTimeout(function () {
                    e.removeClass("preloading", "loaded")
                },
                    app.settings.animation)),
                    e.append(s)
            }
        },
        hide: function (e, a, n) {
            e && (void 0 === a && (a = 0), setTimeout(function () {
                var a = e.find(".loading");
                a.length > 0 && a.each(function () {
                    var t = $(this);
                    void 0 !== t.attr("id") && t.attr("id") !== "loading_layer_" + n || (t.addClass("fadeOut"), setTimeout(function () {
                        t.remove(),
                            a.length - 1 == 0 && e.removeClass("loading-process")
                    },
                        app.settings.animation))
                })
            },
                a))
        }
    },
    _page_loading: {
        show: function (e) {
            var a = document.body,
                n = document.createElement("div");
            if (n.classList.add("page-loader"), "object" == typeof e) {
                if (void 0 !== e.logo) {
                    var t = document.createElement("div");
                    t.classList.add("logo-holder", "logo-holder--xl"),
                        "boolean" == typeof e.logo ? t.innerHTML = app.settings.logo : t.innerHTML = e.logo,
                        void 0 !== e.logoAnimate && ("boolean" == typeof e.logoAnimate ? t.classList.add("zoomIn", "animated") : t.classList.add(e.logoAnimate, "animated")),
                        n.appendChild(t)
                }
                if (void 0 !== e.spinner) {
                    var i = document.createElement("div");
                    i.classList.add("page-loader__spinner"),
                        n.appendChild(i)
                }
                void 0 !== e.animation && ("boolean" == typeof e.animation ? n.classList.add("page-loader--animation") : n.classList.add(e.animation))
            }
            a.classList.add("page-loading"),
                a.appendChild(n)
        },
        hide: function () {
            var e = document.body;
            e.querySelector(".page-loader").classList.add("fadeOut"),
                setTimeout(function () {
                    e.classList.remove("page-loading"),
                        $(e).find(".page-loader").remove()
                },
                    app.settings.animation)
        }
    },
    _backToTop: function () {
        if (!app.settings.backToTop) return !1;
        var e = document.createElement("div");
        e.classList.add("back_to_top"),
            e.addEventListener("click",
                function () {
                    window.scroll({
                        top: 0,
                        left: 0,
                        behavior: "smooth"
                    })
                }),
            document.body.appendChild(e),
            window.addEventListener("scroll",
                function () {
                    window.pageYOffset > app.settings.backToTopHeight ? e.classList.add("show") : e.classList.remove("show")
                })
    },
    _rwProgress: function () {
        var e = document.querySelectorAll(".rw-progress");
        $(e).each(function (e, a) {
            var n = a.dataset.value;
            if (n) {
                for (var t = Math.round(n / 10), i = 0; i <= 9; i++) {
                    var o = document.createElement("div");
                    i < t && o.classList.add("active"),
                        a.appendChild(o)
                }
                if (a.classList.contains("rw-progress--animation")) {
                    var s = a.querySelectorAll("div");
                    $(s).each(function (e, a) {
                        setTimeout(function () {
                            a.classList.add("animate")
                        },
                            e * app.settings.animation)
                    })
                }
            }
        })
    },
    _debouncer: function (e, a) {
        var n;
        a = a || 200;
        return function () {
            var t = this,
                i = arguments;
            clearTimeout(n),
                n = setTimeout(function () {
                    e.apply(t, Array.prototype.slice.call(i))
                },
                    a)
        }
    }
};
document.addEventListener("DOMContentLoaded",
    function () {
        app.layout.controls(),
            app.layout.aside_fixed(),
            app.layout.fixed_panel(),
            app.layout.responsive(),
            app.navigation_detect_auto(),
            app.navigation_quick_build("navigation-quick", "rw-"),
            app.navigation(),
            app.file_tree(),
            app.header_search(),
            app._backToTop(),
            app._rwProgress()
    }),
    window.addEventListener("resize",
        function () {
            app.layout.responsive()
        },
        !0);