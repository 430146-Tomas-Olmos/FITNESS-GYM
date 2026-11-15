/*
 * FitLife Network - Control de acceso por rol (frontend)
 *
 * Este archivo es la UNICA fuente de verdad de "quien ve que" en la interfaz.
 * Se carga en el <head> de cada pagina, antes que el resto de los scripts, para
 * poder bloquear el acceso (redireccionar) antes de que se dibuje la pagina.
 *
 * IMPORTANTE: esto controla la experiencia de usuario. La seguridad real la
 * aplica el backend con [Authorize(Roles = ...)] en cada controlador.
 *
 * Para cambiar permisos editar la tabla RULES (paginas) mas abajo.
 */
(function () {
    'use strict';

    var TOKEN_KEY = 'fitlife_token';
    var USER_KEY = 'fitlife_user';
    var DENIED_KEY = 'fitlife_denied';
    var HOME = '/inicio.html';
    var LOGIN = '/index.html';
    var ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

    // En Inicio, los recuadros de modulos sin permiso se muestran:
    //   'lock' -> visibles pero bloqueados (candado, no navegan)
    //   'hide' -> ocultos por completo
    // Las "pestañas" (Dashboard, Reportes, Administracion) siempre se ocultan.
    var LOCKED_CARD_MODE = 'lock';

    var R = {
        GENERAL: 'ROLE_GERENTE_GENERAL',
        MIEMBROS: 'ROLE_GERENTE_MIEMBROS',
        VENTAS: 'ROLE_GERENTE_VENTAS',
        EQUIPAMIENTO: 'ROLE_GERENTE_EQUIPAMIENTO',
        VENDEDOR: 'ROLE_VENDEDOR',
        TECNICO: 'ROLE_TECNICO'
    };
    var TODOS = [R.GENERAL, R.MIEMBROS, R.VENTAS, R.EQUIPAMIENTO, R.VENDEDOR, R.TECNICO];
    var GERENTES = [R.GENERAL, R.MIEMBROS, R.VENTAS, R.EQUIPAMIENTO];

    // Reglas por pagina. Gana la primera que coincida. Lo que no coincide con
    // ninguna regla queda solo para el Gerente General (denegar por defecto).
    // tab:true => es una "pestaña" (se oculta, no se bloquea con candado).
    var RULES = [
        { re: /^(index\.html)?$/, publica: true },
        { re: /^inicio\.html$/, roles: TODOS },

        { re: /^dashboard\.html$/, roles: [R.GENERAL], tab: true },
        { re: /^administracion\.html$|^modules\/administracion\//, roles: [R.GENERAL], tab: true },
        { re: /^reportes-tpi\.html$/, roles: GERENTES, tab: true },

        // Miembros: el vendedor solo opera socios y cuotas (lo que permite el backend)
        { re: /^miembros\.html$|^modules\/miembros\/(socios|cuotas)\.html$/, roles: [R.GENERAL, R.MIEMBROS, R.VENDEDOR] },
        { re: /^modules\/miembros\//, roles: [R.GENERAL, R.MIEMBROS] },

        // Tienda: compras y transferencias son solo de gerencia
        { re: /^tienda\.html$|^modules\/tienda\/(productos|ventas|stock|cajas|devoluciones)\.html$/, roles: [R.GENERAL, R.VENTAS, R.VENDEDOR] },
        { re: /^modules\/tienda\//, roles: [R.GENERAL, R.VENTAS] },

        // Equipamiento / mantenimiento
        { re: /^equipamiento\.html$|^modules\/equipamiento\//, roles: [R.GENERAL, R.EQUIPAMIENTO, R.TECNICO] }
    ];
    var REGLA_POR_DEFECTO = { roles: [R.GENERAL] };

    // ---------------------------------------------------------------- sesion

    function decodePayload(token) {
        try {
            var b64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
            while (b64.length % 4) b64 += '=';
            var json = decodeURIComponent(atob(b64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));
            return JSON.parse(json);
        } catch (e) {
            return null;
        }
    }

    function clearSession() {
        try {
            localStorage.removeItem(TOKEN_KEY);
            localStorage.removeItem(USER_KEY);
        } catch (e) { /* sin storage */ }
    }

    function readRoles() {
        var token = null;
        try { token = localStorage.getItem(TOKEN_KEY); } catch (e) { /* sin storage */ }
        if (!token) return null;

        var payload = decodePayload(token);
        if (!payload || (payload.exp && payload.exp * 1000 <= Date.now())) {
            clearSession(); // token invalido o vencido
            return null;
        }
        var raw = payload[ROLE_CLAIM] || payload.role || payload.roles || [];
        return [].concat(raw).map(function (r) { return String(r).toUpperCase(); });
    }

    // ---------------------------------------------------------------- reglas

    function normalize(pathname) {
        var p;
        try { p = decodeURIComponent(pathname).toLowerCase(); } catch (e) { p = String(pathname).toLowerCase(); }
        var i = p.indexOf('/modules/');
        if (i >= 0) return p.slice(i + 1);
        return p.split('/').pop();
    }

    function ruleFor(path) {
        for (var i = 0; i < RULES.length; i++) {
            if (RULES[i].re.test(path)) return RULES[i];
        }
        return REGLA_POR_DEFECTO;
    }

    var roles = readRoles();

    function hasAny(list) {
        if (!roles) return false;
        return list.some(function (r) { return roles.indexOf(r) !== -1; });
    }

    function canPath(path) {
        var rule = ruleFor(path);
        return rule.publica === true || hasAny(rule.roles);
    }

    function pathOfHref(href) {
        if (!href || href.charAt(0) === '#' || /^(javascript|mailto|tel):/i.test(href)) return null;
        try {
            var u = new URL(href, location.href);
            return u.origin === location.origin ? normalize(u.pathname) : null;
        } catch (e) {
            return null;
        }
    }

    function canHref(href) {
        var p = pathOfHref(href);
        return p === null ? true : canPath(p);
    }

    // --------------------------------------------------------- API publica

    window.FitLifeAccess = {
        roles: roles || [],
        roleNames: R,
        can: function (page) { return canPath(normalize(page)); },
        canHref: canHref,
        isGeneral: function () { return hasAny([R.GENERAL]); }
    };

    // ------------------------------------------------------- bloqueo de URL

    var here = normalize(location.pathname);
    var rule = ruleFor(here);

    function redirect(url) {
        document.documentElement.style.visibility = 'hidden';
        location.replace(url);
    }

    if (!rule.publica) {
        if (!roles || roles.length === 0) {
            clearSession();
            redirect(LOGIN);
            return;
        }
        if (!canPath(here)) {
            try { sessionStorage.setItem(DENIED_KEY, '1'); } catch (e) { /* sin storage */ }
            redirect(HOME);
            return;
        }
    }
    if (rule.publica) return; // login: nada mas que hacer

    // ------------------------------------------------------- interfaz (DOM)

    function notify(message, type) {
        var main = document.querySelector('main') || document.body;
        var box = document.getElementById('flAccessNotice');
        if (!box) {
            box = document.createElement('div');
            box.id = 'flAccessNotice';
            main.insertBefore(box, main.firstChild);
        }
        box.className = 'alert alert-' + (type || 'warning') + ' d-flex align-items-center gap-2';
        box.setAttribute('role', 'alert');
        box.innerHTML = '<i class="bi bi-shield-lock"></i><span></span>';
        box.lastChild.textContent = message;
        clearTimeout(notify._t);
        notify._t = setTimeout(function () { if (box.parentNode) box.parentNode.removeChild(box); }, 5000);
    }

    function filterSidebar() {
        document.querySelectorAll('aside a[href], #sidebarNav a[href]').forEach(function (link) {
            if (!canHref(link.getAttribute('href'))) link.remove();
        });
    }

    function hasLinkTo(nav, page) {
        return Array.prototype.some.call(nav.querySelectorAll('a[href]'), function (a) {
            return pathOfHref(a.getAttribute('href')) === page;
        });
    }

    function makeLink(href, icon, label) {
        var a = document.createElement('a');
        a.href = href;
        a.className = 'btn btn-dark text-start px-3 py-2 rounded-3 text-white-50';
        a.innerHTML = '<i class="bi ' + icon + ' me-2"></i>' + label;
        return a;
    }

    // En las paginas de modulo garantiza que siempre haya forma de volver a
    // Inicio y de llegar a Reportes (si el rol los tiene).
    function ensureNavLinks() {
        if (here === 'inicio.html') return;
        var nav = document.querySelector('aside nav, #sidebarNav');
        if (!nav) return;

        if (!hasLinkTo(nav, 'inicio.html')) {
            nav.insertBefore(makeLink('/inicio.html', 'bi-house-door', 'Inicio'), nav.firstChild);
        }
        if (canPath('reportes-tpi.html') && !hasLinkTo(nav, 'reportes-tpi.html')) {
            var link = makeLink('/reportes-tpi.html', 'bi-bar-chart-line', 'Reportes');
            var admin = Array.prototype.find.call(nav.querySelectorAll('a[href]'), function (a) {
                return pathOfHref(a.getAttribute('href')) === 'administracion.html';
            });
            nav.insertBefore(link, admin || null);
        }
    }

    function lockCard(link, label) {
        var card = link.querySelector('.card');
        link.removeAttribute('href');
        link.setAttribute('aria-disabled', 'true');
        link.setAttribute('tabindex', '-1');
        link.style.cursor = 'not-allowed';
        if (card) {
            card.classList.add('opacity-50');
            card.style.filter = 'grayscale(0.7)';
            card.style.position = 'relative';
            var badge = document.createElement('span');
            badge.className = 'position-absolute top-0 end-0 m-3 badge rounded-pill text-bg-secondary';
            badge.innerHTML = '<i class="bi bi-lock-fill me-1"></i>Sin acceso';
            card.appendChild(badge);
        }
        link.addEventListener('click', function (e) {
            e.preventDefault();
            notify('Tu rol no tiene acceso a ' + label + '.', 'warning');
        });
    }

    function applyHomeCards() {
        if (here !== 'inicio.html') return;
        document.querySelectorAll('main a[href]').forEach(function (link) {
            var page = pathOfHref(link.getAttribute('href'));
            if (page === null || canPath(page)) return;

            var column = link.closest('[class*="col-"]') || link;
            if (ruleFor(page).tab || LOCKED_CARD_MODE === 'hide') {
                column.remove();
            } else {
                var title = link.querySelector('h2, h3, h4, h5');
                lockCard(link, title ? title.textContent.trim() : 'esa sección');
            }
        });

        try {
            if (sessionStorage.getItem(DENIED_KEY)) {
                sessionStorage.removeItem(DENIED_KEY);
                notify('Tu rol no tiene acceso a la sección que intentaste abrir.', 'warning');
            }
        } catch (e) { /* sin storage */ }
    }

    // Compatibilidad: <el data-role="ROLE_X,ROLE_Y"> solo se ve con alguno de esos roles
    function applyDataRole() {
        document.querySelectorAll('[data-role]').forEach(function (el) {
            var needed = el.getAttribute('data-role').toUpperCase().split(',').map(function (s) { return s.trim(); });
            if (!hasAny(needed)) el.style.display = 'none';
        });
    }

    function applyUi() {
        filterSidebar();
        ensureNavLinks();
        applyHomeCards();
        applyDataRole();
    }

    // app.js arma parte del menu en su propio DOMContentLoaded: se aplica despues
    // (setTimeout 0) y otra vez en load. Es idempotente.
    document.addEventListener('DOMContentLoaded', function () { setTimeout(applyUi, 0); });
    window.addEventListener('load', function () { filterSidebar(); ensureNavLinks(); });
})();
