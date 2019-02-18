"use strict";
var settings = {
    showSettings: function () {
        var e = document.getElementById("fixed_panel"),
            s = document.getElementById("rw_settings_show");
        return s.addEventListener("change",
            function (e) {
                sessionStorage.setItem("showSettings", e.target.checked)
            }),
            sessionStorage.getItem("showSettings") && "false" !== sessionStorage.getItem("showSettings") ? s.checked = !0 : setTimeout(function () {
                e.classList.contains("show") || document.querySelector("[data-action='fixedpanel-toggle']").click()
            },
                2e3),
            !1
    },
    vars: {
        body: document.body,
        page: document.getElementsByClassName("page")[0],
        page_header: document.getElementsByClassName("page__header")[0],
        page_container: document.getElementsByClassName("page__container")[0],
        page_sidepanel: document.getElementById("page-sidepanel"),
        page_content: document.getElementById("page-content"),
        content: document.getElementById("content"),
        page_aside: document.getElementById("page-aside"),
        page_aside_navigation: document.getElementById("navigation-default"),
        s_layout: document.getElementById("rw_settings_layout"),
        s_layout_gb: document.getElementById("rw_settings_layout_boxed_group"),
        s_layout_gb_vspace: document.getElementById("rw_settings_layout_boxed_vspace"),
        s_layout_gb_rounded: document.getElementById("rw_settings_layout_boxed_rounded"),
        s_layout_gb_shadowed: document.getElementById("rw_settings_layout_boxed_shadowed"),
        s_layout_gi: document.getElementById("rw_settings_layout_indent_group"),
        s_layout_gi_header_group: document.getElementById("rw_settings_layout_indent_header_group"),
        s_layout_gi_header: document.getElementById("rw_settings_layout_indent_header"),
        s_layout_gi_header_relative: document.getElementById("rw_settings_layout_indent_header_relative"),
        s_layout_gi_container: document.getElementById("rw_settings_layout_indent_container"),
        s_layout_gi_container_single: document.getElementById("rw_settings_layout_indent_container_single"),
        s_layout_gi_rounded: document.getElementById("rw_settings_layout_indent_rounded"),
        s_layout_gi_shadowed: document.getElementById("rw_settings_layout_indent_shadowed"),
        s_layout_gi_move_heading: document.getElementById("rw_settings_layout_indent_move_heading"),
        s_layout_gbg: document.getElementById("rw_settings_layout_bgs_group"),
        s_header_opt_group: document.getElementById("rw_settings_header_opt_group"),
        s_header: document.getElementById("rw_settings_header_fixed"),
        s_header_invert: document.getElementById("rw_settings_header_invert"),
        s_container_opt_group: document.getElementById("rw_settings_container_opt_group"),
        s_container_invert: document.getElementById("rw_settings_container_invert"),
        s_sidepanel_opt_group: document.getElementById("rw_settings_sidepanel_opt_group"),
        s_sidepanel: document.getElementById("rw_settings_sidepanel_hidden"),
        s_sidepanel_invert: document.getElementById("rw_settings_sidepanel_invert"),
        s_content: document.getElementById("rw_settings_content_fluid"),
        s_content_invert: document.getElementById("rw_settings_content_invert"),
        s_nav_opt_group: document.getElementById("rw_settings_navigation_opt_group"),
        s_nav_min: document.getElementById("rw_settings_nav_minimized"),
        s_nav_hid: document.getElementById("rw_settings_nav_hidden"),
        s_nav_fix: document.getElementById("rw_settings_nav_fixed"),
        s_nav_invert: document.getElementById("rw_settings_nav_invert"),
        s_nav_vmiddle: document.getElementById("rw_settings_nav_vmiddle"),
        s_nav_condensed: document.getElementById("rw_settings_nav_condensed"),
        s_nav_cpanel: document.getElementById("rw_settings_nav_cpanel"),
        bgs: ["bg-gradient-1", "bg-gradient-2", "bg-gradient-3", "bg-gradient-4", "bg-gradient-5", "bg-gradient-6", "bg-gradient-7", "bg-gradient-8", "bg-gradient-9", "bg-gradient-10"]
    },
    init: function () {
        settings.showSettings(),
            settings.detect_states(),
            settings.add_events()
    },
    add_events: function () {
        settings.vars.s_layout.addEventListener("change",
            function (e) {
                var s;
                "default" === e.target.value && (settings.vars.s_layout_gb.classList.remove("d-block"), settings.vars.s_layout_gi.classList.remove("d-block"), settings.vars.s_layout_gb.classList.add("d-none"), settings.vars.s_layout_gi.classList.add("d-none"), settings.vars.s_layout_gbg.classList.add("d-none"), settings.vars.body.classList.remove("indent"), settings.vars.body.classList.remove("indent--single-header"), settings.vars.body.classList.remove("indent--single-container"), settings.vars.body.classList.remove("indent--relative-header"), settings.vars.body.classList.remove("indent--rounded"), settings.vars.body.classList.remove("indent--shadowed"), settings.vars.body.classList.remove("boxed"), settings.vars.body.classList.remove("boxed--vspace"), settings.vars.body.classList.remove("boxed--rounded", "boxed--shadowed"), settings.vars.body.classList.remove("boxed--shadowed"), settings.vars.s_layout_gb_vspace.checked = !1, settings.vars.s_layout_gb_rounded.checked = !1, settings.vars.s_layout_gb_shadowed.checked = !1, settings.vars.s_layout_gi_header.checked = !1, settings.vars.s_layout_gi_header_relative.checked = !1, settings.vars.s_layout_gi_rounded.checked = !1, settings.vars.s_layout_gi_shadowed.checked = !1, settings.vars.s_nav_vmiddle.disabled = !0, (s = settings.vars.page.querySelectorAll(".page-heading")[0]) && (s.querySelectorAll(".breadcrumb")[0].classList.remove("d-none"), settings.vars.content.insertBefore(s, settings.vars.content.firstChild)), settings.vars.s_layout_gi_move_heading.checked = !1);
                "boxed" === e.target.value && (settings.vars.s_layout_gb.classList.remove("d-none"), settings.vars.s_layout_gb.classList.add("d-block"), settings.vars.s_layout_gi.classList.remove("d-block"), settings.vars.s_layout_gi.classList.add("d-none"), settings.vars.s_layout_gbg.classList.remove("d-none"), settings.vars.body.classList.remove("indent"), settings.vars.body.classList.remove("indent--single-header"), settings.vars.body.classList.remove("indent--single-container"), settings.vars.body.classList.remove("indent--relative-header"), settings.vars.body.classList.remove("indent--rounded"), settings.vars.body.classList.remove("indent--shadowed"), settings.vars.s_layout_gi_header.checked = !1, settings.vars.s_layout_gi_header_relative.checked = !1, settings.vars.s_layout_gi_rounded.checked = !1, settings.vars.s_layout_gi_shadowed.checked = !1, settings.vars.s_layout_gi_move_heading.disabled = !0, settings.vars.s_nav_vmiddle.disabled = !0, settings.vars.page_aside && settings.vars.page_aside.classList.remove("navigation--vertical-middle"), (s = settings.vars.page.querySelectorAll(".page-heading")[0]) && (s.querySelectorAll(".breadcrumb")[0].classList.remove("d-none"), settings.vars.content.insertBefore(s, settings.vars.content.firstChild)), settings.vars.s_layout_gi_move_heading.checked = !1, settings.vars.body.classList.add("boxed"), app._crt());
                "indent" === e.target.value && (settings.vars.s_layout_gi.classList.remove("d-none"), settings.vars.s_layout_gi.classList.add("d-block"), settings.vars.s_layout_gbg.classList.remove("d-none"), settings.vars.page_header && settings.vars.s_layout_gi_header_group.classList.remove("d-none"), settings.vars.s_layout_gi_move_heading.disabled = !0, settings.vars.s_nav_vmiddle.disabled = !0, settings.vars.page_aside && settings.vars.page_aside.classList.remove("navigation--vertical-middle"), settings.vars.s_layout_gb.classList.remove("d-block"), settings.vars.s_layout_gb.classList.add("d-none"), settings.vars.body.classList.remove("boxed"), settings.vars.body.classList.remove("boxed--vspace"), settings.vars.body.classList.remove("boxed--rounded", "boxed--shadowed"), settings.vars.body.classList.remove("boxed--shadowed"), settings.vars.s_layout_gb_vspace.checked = !1, settings.vars.s_layout_gb_rounded.checked = !1, settings.vars.s_layout_gb_shadowed.checked = !1, settings.vars.page.getElementsByClassName("page__container")[0] && settings.vars.s_layout_gi_container.classList.remove("d-none"), settings.vars.body.classList.add("indent"), app._crt()),
                    app._fireResize(),
                    settings.detect_states()
            }),
            settings.vars.s_layout_gb_vspace.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.body.classList.add("boxed--vspace") : settings.vars.body.classList.remove("boxed--vspace"),
                        app._crt()
                }),
            settings.vars.s_layout_gb_rounded.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.body.classList.add("boxed--rounded") : settings.vars.body.classList.remove("boxed--rounded")
                }),
            settings.vars.s_layout_gb_shadowed.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.body.classList.add("boxed--shadowed") : settings.vars.body.classList.remove("boxed--shadowed")
                }),
            settings.vars.s_layout_gi_header.addEventListener("change",
                function (e) {
                    e.target.checked ? (settings.vars.body.classList.add("indent--single-header"), settings.vars.s_layout_gi_move_heading.disabled = !0, settings.vars.s_layout_gi_header_relative.disabled = !0) : (settings.vars.body.classList.remove("indent--single-header"), settings.vars.s_layout_gi_header_relative.disabled = !1),
                        app._crt()
                }),
            settings.vars.s_layout_gi_header_relative.addEventListener("change",
                function (e) {
                    if (e.target.checked) settings.vars.body.classList.add("indent--relative-header"),
                        settings.vars.s_layout_gi_move_heading.disabled = !1,
                        settings.vars.s_layout_gi_header.disabled = !0;
                    else {
                        settings.vars.body.classList.remove("indent--relative-header"),
                            settings.vars.s_layout_gi_move_heading.disabled = !0,
                            settings.vars.s_layout_gi_header.disabled = !1;
                        var s = settings.vars.page.querySelectorAll(".page-heading")[0],
                            t = s.querySelectorAll(".breadcrumb")[0];
                        t && t.classList.remove("d-none"),
                            settings.vars.content.insertBefore(s, settings.vars.content.firstChild)
                    }
                    app._crt()
                }),
            settings.vars.s_layout_gi_container_single.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.body.classList.add("indent--single-container") : settings.vars.body.classList.remove("indent--single-container"),
                        app._crt()
                }),
            settings.vars.s_layout_gi_rounded.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.body.classList.add("indent--rounded") : settings.vars.body.classList.remove("indent--rounded")
                }),
            settings.vars.s_layout_gi_shadowed.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.body.classList.add("indent--shadowed") : settings.vars.body.classList.remove("indent--shadowed")
                }),
            settings.vars.s_layout_gi_move_heading.addEventListener("change",
                function (e) {
                    var s;
                    e.target.checked ? ((s = settings.vars.content.querySelectorAll(".page-heading")[0]).querySelectorAll(".breadcrumb")[0].classList.add("d-none"), settings.vars.page_header.parentNode.insertBefore(s, settings.vars.page_header.nextSibling)) : ((s = settings.vars.page.querySelectorAll(".page-heading")[0]).querySelectorAll(".breadcrumb")[0].classList.remove("d-none"), settings.vars.content.insertBefore(s, settings.vars.content.firstChild));
                    app._crt()
                });
        var e = document.getElementById("rw_settings_layout_bgs").querySelectorAll("div");
        $(e).each(function (s, t) {
            t.addEventListener("click",
                function (s) {
                    if (s.target.classList.contains("active")) return s.target.classList.remove("active"),
                        settings.vars.body.classList.remove(s.target.getAttribute("class")),
                        !1;
                    settings.vars.bgs.forEach(function (e) {
                        document.body.classList.contains(e) && document.body.classList.remove(e)
                    }),
                        $(e).each(function (e, s) {
                            s.classList.remove("active")
                        }),
                        settings.vars.body.classList.add(s.target.getAttribute("class")),
                        s.target.classList.add("active")
                })
        }),
            settings.vars.s_header.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.page.classList.add("page--w-fixed-header") : settings.vars.page.classList.remove("page--w-fixed-header")
                }),
            settings.vars.s_header_invert.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.page_header.classList.add("invert") : settings.vars.page_header.classList.remove("invert")
                }),
            settings.vars.s_container_invert.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.page_container.classList.add("invert") : settings.vars.page_container.classList.remove("invert")
                }),
            settings.vars.s_nav_min.addEventListener("change",
                function (e) {
                    e.target.checked ? (settings.vars.page_content.classList.add("page-aside--minimized"), settings.vars.page_aside.classList.add("page-aside--minimized")) : (settings.vars.page_content.classList.remove("page-aside--minimized"), settings.vars.page_aside.classList.remove("page-aside--minimized"))
                }),
            settings.vars.s_nav_hid.addEventListener("change",
                function (e) {
                    e.target.checked ? (settings.vars.page_content.classList.add("page-aside--hidden"), settings.vars.page_aside.classList.add("page-aside--hidden")) : (settings.vars.page_content.classList.remove("page-aside--hidden"), settings.vars.page_aside.classList.remove("page-aside--hidden"))
                }),
            settings.vars.s_nav_fix.addEventListener("change",
                function (e) {
                    e.target.checked ? (settings.vars.page_content.classList.add("page__content--w-aside-fixed"), app.layout.aside_fixed(), settings.vars.s_nav_vmiddle.disabled = !1) : (settings.vars.page_content.classList.remove("page__content--w-aside-fixed"), settings.vars.s_nav_vmiddle.disabled = !0, settings.vars.s_nav_vmiddle.checked = !1, settings.vars.page_aside.classList.remove("navigation--vertical-middle"))
                }),
            settings.vars.s_nav_invert.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.page_aside.classList.add("invert") : settings.vars.page_aside.classList.remove("invert")
                }),
            settings.vars.s_nav_vmiddle.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.page_aside.classList.add("navigation--vertical-middle") : settings.vars.page_aside.classList.remove("navigation--vertical-middle")
                }),
            settings.vars.s_nav_condensed.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.page_aside_navigation.classList.add("navigation--condensed") : settings.vars.page_aside_navigation.classList.remove("navigation--condensed")
                }),
            settings.vars.s_nav_cpanel.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.page_aside.classList.remove("page-aside--w-controls") : settings.vars.page_aside.classList.add("page-aside--w-controls")
                }),
            settings.vars.s_sidepanel.addEventListener("change",
                function (e) {
                    e.target.checked ? (settings.vars.page_content.classList.add("page-sidepanel--hidden"), settings.vars.page_sidepanel.classList.add("page-sidepanel--hidden")) : (settings.vars.page_content.classList.remove("page-sidepanel--hidden"), settings.vars.page_sidepanel.classList.remove("page-sidepanel--hidden"))
                }),
            settings.vars.s_sidepanel_invert.addEventListener("change",
                function (e) {
                    e.target.checked ? settings.vars.page_sidepanel.classList.add("invert") : settings.vars.page_sidepanel.classList.remove("invert")
                }),
            settings.vars.s_content.addEventListener("change",
                function (e) {
                    var s = settings.vars.content.querySelectorAll("[class^='container']")[0];
                    e.target.checked ? (s.classList.remove("container"), s.classList.add("container-fluid")) : (s.classList.remove("container-fluid"), s.classList.add("container"))
                }),
            settings.vars.s_content_invert.addEventListener("change",
                function (e) {
                    var s = settings.vars.content.querySelectorAll(".card"),
                        t = settings.vars.content.querySelectorAll(".page-heading")[0];
                    e.target.checked ? (settings.vars.page_content.classList.add("page__content-invert"), t && t.classList.add("invert"), settings.vars.content.classList.add("invert"), $(s).each(function (e, s) {
                        s.classList.add("invert")
                    })) : (settings.vars.page_content.classList.remove("page__content-invert"), t && t.classList.remove("invert"), settings.vars.content.classList.remove("invert"), $(s).each(function (e, s) {
                        s.classList.remove("invert")
                    }))
                })
    },
    detect_states: function () {
        settings.vars.body.classList.contains("boxed") ? (settings.vars.s_layout.value = "boxed", settings.vars.s_layout_gb.classList.remove("d-none"), settings.vars.s_layout_gb.classList.add("d-block"), settings.vars.page.classList.remove("page--w-fixed-header"), settings.vars.s_header.disabled = !0, settings.vars.page_content.classList.remove("page__content--w-aside-fixed"), settings.vars.s_nav_fix.disabled = !0, settings.vars.s_layout_gbg.classList.remove("d-none"), settings.vars.s_layout_gb_vspace.checked = settings.vars.body.classList.contains("boxed--vspace"), settings.vars.s_layout_gb_rounded.checked = settings.vars.body.classList.contains("boxed--rounded"), settings.vars.s_layout_gb_shadowed.checked = settings.vars.body.classList.contains("boxed--shadowed")) : settings.vars.body.classList.contains("indent") ? (settings.vars.s_layout.value = "indent", settings.vars.s_layout_gi.classList.remove("d-none"), settings.vars.s_layout_gi.classList.add("d-block"), settings.vars.page.classList.remove("page--w-fixed-header"), settings.vars.s_header.disabled = !0, settings.vars.page_content.classList.remove("page__content--w-aside-fixed"), settings.vars.s_nav_fix.disabled = !0, settings.vars.page_header && settings.vars.s_layout_gi_header_group.classList.remove("d-none"), settings.vars.page.getElementsByClassName("page__container")[0] && settings.vars.s_layout_gi_container.classList.remove("d-none"), settings.vars.s_layout_gbg.classList.remove("d-none"), settings.vars.body.classList.contains("indent--single-header") && (settings.vars.s_layout_gi_header.checked = !0, settings.vars.s_layout_gi_move_heading.disabled = !0, settings.vars.s_layout_gi_header_relative.disabled = !0), settings.vars.body.classList.contains("indent--relative-header") && (settings.vars.s_layout_gi_header_relative.checked = !0, settings.vars.s_layout_gi_move_heading.disabled = !1, settings.vars.s_layout_gi_header.disabled = !0), settings.vars.body.classList.contains("indent--single-container") && (settings.vars.s_layout_gi_container_single.checked = !0), settings.vars.body.classList.contains("indent--rounded") && (settings.vars.s_layout_gi_rounded.checked = !0), settings.vars.body.classList.contains("indent--shadowed") && (settings.vars.s_layout_gi_shadowed.checked = !0)) : (settings.vars.s_layout.value = "default", settings.vars.s_nav_fix.checked && (settings.vars.s_nav_vmiddle.disabled = !1), settings.vars.s_header.disabled = !1, settings.vars.s_nav_fix.disabled = !1),
            settings.vars.bgs.forEach(function (e) {
                settings.vars.body.classList.contains(e) && document.getElementById("rw_settings_layout_bgs").querySelectorAll("." + e)[0].classList.add("active")
            }),
            settings.vars.s_header.checked = !!settings.vars.page.classList.contains("page--w-fixed-header"),
            settings.vars.page_header && (settings.vars.s_header_invert.checked = !!settings.vars.page_header.classList.contains("invert"), settings.vars.s_header_opt_group.classList.remove("d-none")),
            settings.vars.page_container && (settings.vars.s_container_invert.checked = !!settings.vars.page_container.classList.contains("invert"), settings.vars.s_container_opt_group.classList.remove("d-none")),
            settings.vars.page_aside && (settings.vars.s_nav_opt_group.classList.remove("d-none"), settings.vars.s_nav_min.checked = !!settings.vars.page_content.classList.contains("page-aside--minimized"), settings.vars.s_nav_hid.checked = !!settings.vars.page_content.classList.contains("page-aside--hidden"), settings.vars.s_nav_fix.checked = !!settings.vars.page_content.classList.contains("page__content--w-aside-fixed"), settings.vars.s_nav_invert.checked = !!settings.vars.page_aside.classList.contains("invert"), settings.vars.s_nav_vmiddle.checked = !!settings.vars.page_aside.classList.contains("navigation--vertical-middle"), settings.vars.s_nav_condensed.checked = !!settings.vars.page_aside_navigation.classList.contains("navigation--condensed"), settings.vars.s_nav_cpanel.checked = !settings.vars.page_aside.classList.contains("page-aside--w-controls")),
            settings.vars.page_sidepanel && (settings.vars.s_sidepanel.checked = !!settings.vars.page_content.classList.contains("page-sidepanel--hidden"), settings.vars.s_sidepanel_invert.checked = !!settings.vars.page_sidepanel.classList.contains("invert"), settings.vars.s_sidepanel_opt_group.classList.remove("d-none")),
            settings.vars.content.querySelectorAll("[class^='container'")[0] && (settings.vars.s_content.checked = !!settings.vars.content.querySelectorAll("[class^='container'")[0].classList.contains("container-fluid")),
            settings.vars.s_content_invert.checked = !!settings.vars.page_content.classList.contains("page__content-invert")
    }
};
document.addEventListener("DOMContentLoaded",
    function () {
        settings.init()
    });