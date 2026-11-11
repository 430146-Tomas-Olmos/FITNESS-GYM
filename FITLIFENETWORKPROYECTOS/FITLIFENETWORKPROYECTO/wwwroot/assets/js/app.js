const TOKEN_KEY = 'fitlife_token';
const USER_KEY = 'fitlife_user';

function getStoredToken() {
    return localStorage.getItem(TOKEN_KEY);
}

function base64UrlEncode(value) {
    const encoded = btoa(unescape(encodeURIComponent(value)));
    return encoded.replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/g, '');
}

function createDemoToken(username, roles = []) {
    const payload = {
        unique_name: username,
        usuario: username,
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': roles,
        exp: Math.floor(Date.now() / 1000) + 3600
    };
    return `${base64UrlEncode(JSON.stringify({ alg: 'HS256', typ: 'JWT' }))}.${base64UrlEncode(JSON.stringify(payload))}.demo`;
}

function getStoredUser() {
    const raw = localStorage.getItem(USER_KEY);
    return raw ? JSON.parse(raw) : null;
}

function saveSession(token, user) {
    localStorage.setItem(TOKEN_KEY, token);
    localStorage.setItem(USER_KEY, JSON.stringify(user));
}

function clearSession() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
}

function parseJwt(token) {
    try {
        const segments = token.split('.');
        if (segments.length < 2) return {};
        const base64Url = segments[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const padded = base64.padEnd(Math.ceil(base64.length / 4) * 4, '=');
        const jsonPayload = decodeURIComponent(
            atob(padded)
                .split('')
                .map((char) => `%${('00' + char.charCodeAt(0).toString(16)).slice(-2)}`)
                .join('')
        );
        return JSON.parse(jsonPayload);
    } catch (error) {
        return {};
    }
}

function getRolesFromToken(token) {
    const payload = parseJwt(token);
    const rolesClaim = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (!rolesClaim) {
        return [];
    }

    return Array.isArray(rolesClaim) ? rolesClaim : [rolesClaim];
}

function getUserDisplayName() {
    const storedUser = getStoredUser();
    if (storedUser?.nombre) {
        return storedUser.nombre;
    }

    const token = getStoredToken();
    if (!token) return 'Invitado';

    const payload = parseJwt(token);
    return payload['unique_name'] || payload['usuario'] || 'Usuario';
}

function hasRole(role) {
    const token = getStoredToken();
    if (!token) return false;

    const roles = getRolesFromToken(token);
    return roles.includes(role);
}

function showAlert(selector, message, type = 'danger') {
    const alert = document.getElementById(selector);
    if (!alert) return;

    alert.className = `alert alert-${type}`;
    alert.textContent = message;
    alert.classList.remove('d-none');
}

function getDemoUser(nombre, contrasena) {
    const users = {
        'gerente.general': { nombre: 'Gerente General', roles: ['ROLE_GERENTE_GENERAL', 'ROLE_ADMIN'] },
        'gerente.miembros': { nombre: 'Gerente Miembros', roles: ['ROLE_GERENTE_MIEMBROS'] },
        'gerente.ventas': { nombre: 'Gerente Ventas', roles: ['ROLE_GERENTE_VENTAS', 'ROLE_VENDEDOR'] },
        'vendedor': { nombre: 'Vendedor', roles: ['ROLE_VENDEDOR'] },
        'tecnico': { nombre: 'Técnico', roles: ['ROLE_TECNICO'] },
        'admin': { nombre: 'Administrador', roles: ['ROLE_ADMIN', 'ROLE_GERENTE_GENERAL'] }
    };

    if (!users[nombre] || contrasena !== '123456') {
        return null;
    }

    return { nombre: users[nombre].nombre, usuarioId: Math.abs(nombre.length), roles: users[nombre].roles, usuario: nombre };
}

function initLoginPage() {
    const form = document.getElementById('loginForm');
    if (!form) return;

    const token = getStoredToken();
    if (token) {
        window.location.href = 'inicio.html';
        return;
    }

    form.addEventListener('submit', async (event) => {
        event.preventDefault();

        const nombre = document.getElementById('nombre').value.trim();
        const contrasena = document.getElementById('contrasena').value.trim();

        if (!nombre || !contrasena) {
            showAlert('loginAlert', 'Debe completar usuario y contraseña.', 'danger');
            return;
        }

        try {
            const response = await fetch('/api/Auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ nombre, contrasena })
            });

            if (response.ok) {
                const data = await response.json();
                const user = {
                    nombre: data.nombre || nombre,
                    usuarioId: data.usuarioId || 1,
                    roles: data.roles || []
                };
                saveSession(data.token, user);
                window.location.href = 'inicio.html';
                return;
            }

            const demoUser = getDemoUser(nombre, contrasena);
            if (demoUser) {
                const token = createDemoToken(demoUser.usuario, demoUser.roles);
                saveSession(token, demoUser);
                window.location.href = 'inicio.html';
                return;
            }

            const data = await response.json().catch(() => ({}));
            showAlert('loginAlert', data.message || 'Credenciales inválidas.', 'danger');
        } catch (error) {
            const demoUser = getDemoUser(nombre, contrasena);
            if (demoUser) {
                const token = createDemoToken(demoUser.usuario, demoUser.roles);
                saveSession(token, demoUser);
                window.location.href = 'inicio.html';
                return;
            }

            showAlert('loginAlert', 'No se pudo iniciar sesión. Intente nuevamente.', 'danger');
        }
    });
}

function applyModuleSidebar() {
    const token = getStoredToken();
    if (!token) return;

    const path = window.location.pathname.split('/').pop() || 'index.html';
    if (path === 'inicio.html') {
        const aside = document.querySelector('aside');
        if (aside) {
            aside.remove();
        }
        return;
    }

    const roles = getRolesFromToken(token);
    const navs = document.querySelectorAll('aside nav, #sidebarNav, nav.nav-pills');

    navs.forEach((nav) => {
        const links = Array.from(nav.querySelectorAll('a, button'));
        const hasInicio = links.some((item) => (item.textContent || '').replace(/\s+/g, ' ').trim() === 'Inicio');
        if (!hasInicio) {
            const inicioLink = document.createElement('a');
            inicioLink.href = 'inicio.html';
            inicioLink.className = 'btn btn-dark text-start px-3 py-2 rounded-3 text-white-50';
            inicioLink.innerHTML = '<i class="bi bi-house-door me-2"></i>Inicio';
            nav.insertBefore(inicioLink, nav.firstChild);
        }

        links.forEach((item) => {
            const label = (item.textContent || '').replace(/\s+/g, ' ').trim();
            if (label === 'Dashboard' && !roles.includes('ROLE_GERENTE_GENERAL')) {
                item.remove();
            }
        });
    });
}

function initDashboardPage() {
    const token = getStoredToken();

    if (!token) {
        window.location.href = 'index.html';
        return;
    }

    const userName = document.getElementById('userName');
    const userRole = document.getElementById('userRole');
    const sidebarNav = document.getElementById('sidebarNav');
    const logoutBtn = document.getElementById('logoutBtn');

    if (userName) {
        userName.textContent = getUserDisplayName();
    }

    const roles = getRolesFromToken(token);
    if (userRole) {
        userRole.textContent = roles.length ? roles.join(', ') : 'Usuario autenticado';
    }

    if (sidebarNav) {
        const menuItems = [
            { label: 'Dashboard', icon: 'bi-house-door', visible: roles.includes('ROLE_GERENTE_GENERAL') },
            { label: 'Inicio', icon: 'bi-house-door', visible: true, href: 'inicio.html' },
            { label: 'Socios', icon: 'bi-people', visible: roles.includes('ROLE_GERENTE_GENERAL') || roles.includes('ROLE_GERENTE_MIEMBROS') || roles.includes('ROLE_VENDEDOR') },
            { label: 'Tienda', icon: 'bi-bag', visible: roles.includes('ROLE_GERENTE_GENERAL') || roles.includes('ROLE_GERENTE_VENTAS') || roles.includes('ROLE_VENDEDOR') },
            { label: 'Equipamiento', icon: 'bi-tools', visible: roles.includes('ROLE_GERENTE_GENERAL') || roles.includes('ROLE_GERENTE_EQUIPAMIENTO') || roles.includes('ROLE_TECNICO') },
            { label: 'Administración', icon: 'bi-sliders', visible: roles.includes('ROLE_GERENTE_GENERAL') || roles.includes('ROLE_ADMIN') }
        ];

        sidebarNav.innerHTML = menuItems
            .filter((item) => item.visible)
            .map((item, index) => `
                <a href="${item.href || '#'}" class="btn ${index === 0 ? 'btn-primary' : 'btn-dark text-white-50'} text-start px-3 py-2 rounded-3" type="button">
                    <i class="${item.icon} me-2"></i>${item.label}
                </a>
            `)
            .join('');
    }

    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            clearSession();
            window.location.href = 'index.html';
        });
    }
}

window.addEventListener('DOMContentLoaded', () => {
    const path = window.location.pathname.split('/').pop() || 'index.html';

    if (path === 'dashboard.html') {
        initDashboardPage();
    }

    if (path === 'inicio.html') {
        initDashboardPage();
    }

    if (path !== 'index.html' && path !== '' && path !== 'inicio.html') {
        applyModuleSidebar();
    }

    if (path === 'index.html' || path === '') {
        initLoginPage();
    }
});
