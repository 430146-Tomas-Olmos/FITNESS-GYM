// Role Guard script
// Reads JWT token from localStorage, extracts role claims, and hides unauthorized menu items and navigation links.
(function () {
    function decodePayload(token) {
        try {
            const parts = token.split('.');
            if (parts.length < 2) return null;
            const payload = parts[1];
            const decoded = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
            return JSON.parse(decoded);
        } catch (e) {
            console.error('Failed to decode JWT payload', e);
            return null;
        }
    }

    const token = localStorage.getItem('fitlife_token') || localStorage.getItem('token');
    if (!token) return;

    const payload = decodePayload(token);
    let roles = [];

    if (payload) {
        const rawRole = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
            payload['role'] ||
            payload['Role'] ||
            payload['roles'];

        if (Array.isArray(rawRole)) {
            roles = rawRole.map(r => String(r).toUpperCase());
        } else if (typeof rawRole === 'string') {
            roles = [rawRole.toUpperCase()];
        }
    }

    // Also check saved user in localStorage
    try {
        const savedUser = JSON.parse(localStorage.getItem('fitlife_user') || '{}');
        if (Array.isArray(savedUser.roles)) {
            savedUser.roles.forEach(r => {
                const upper = String(r).toUpperCase();
                if (!roles.includes(upper)) roles.push(upper);
            });
        }
    } catch (e) {}

    const isAdmin = roles.some(r => r === 'ROLE_GERENTE_GENERAL' || r === 'ROLE_ADMIN' || r === 'ADMIN');
    const isMiembros = isAdmin || roles.some(r => r === 'ROLE_GERENTE_MIEMBROS');
    const isVentas = isAdmin || roles.some(r => r === 'ROLE_GERENTE_VENTAS' || r === 'ROLE_VENDEDOR');
    const isEquipamiento = isAdmin || roles.some(r => r === 'ROLE_GERENTE_EQUIPAMIENTO' || r === 'ROLE_TECNICO');

    // Hide navigation links by target or label if unauthorized
    document.querySelectorAll('aside nav a, #sidebarNav a, nav.nav-pills a').forEach(link => {
        const href = (link.getAttribute('href') || '').toLowerCase();
        const text = (link.textContent || '').trim().toLowerCase();

        // Dashboard
        if ((href.includes('dashboard') || text === 'dashboard') && !isAdmin) {
            link.style.display = 'none';
        }
        // Administracion
        if ((href.includes('administracion') || href.includes('usuarios') || text.includes('administración') || text.includes('administracion') || text.includes('usuarios')) && !isAdmin) {
            link.style.display = 'none';
        }
        // Tienda
        if ((href.includes('tienda') || href.includes('productos') || href.includes('ventas') || href.includes('compras') || href.includes('cajas') || text.includes('tienda')) && !isVentas) {
            link.style.display = 'none';
        }
        // Equipamiento
        if ((href.includes('equipamiento') || href.includes('equipos') || href.includes('incidencias') || href.includes('mantenimiento') || text.includes('equipamiento')) && !isEquipamiento) {
            link.style.display = 'none';
        }
        // Socios / Miembros
        if ((href.includes('miembros') || href.includes('socios') || href.includes('contratos') || href.includes('cuotas') || href.includes('accesos')) && !isMiembros && !isVentas) {
            if (!href.includes('reservas')) {
                link.style.display = 'none';
            }
        }
    });

    // Also support any element with data-role or li[id]
    document.querySelectorAll('[data-role]').forEach(el => {
        const requiredRole = el.getAttribute('data-role').toUpperCase();
        if (!roles.includes(requiredRole) && !isAdmin) {
            el.style.display = 'none';
        }
    });
})();
