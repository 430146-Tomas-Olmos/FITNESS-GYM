// Administracion module logic with real backend API integration

function getAuthHeader() {
    const token = localStorage.getItem('fitlife_token');
    return token ? { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' } : { 'Content-Type': 'application/json' };
}

let currentUsers = [];
let editingUserId = null;
let refreshUsersView = () => renderUsers(currentUsers);

function escHtml(v) {
    return String(v ?? '').replace(/[&<>"']/g, ch => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[ch]));
}

async function readError(res, fallback) {
    try {
        const j = await res.json();
        return j.error || j.message || j.title || fallback;
    } catch (e) {
        return fallback;
    }
}

const auditSeed = [
    { fecha: '2026-11-10 09:42', usuario: 'mlopez', accion: 'Aprobó renovación', modulo: 'Miembros', resultado: 'OK' },
    { fecha: '2026-11-10 09:18', usuario: 'scosta', accion: 'Cerró caja', modulo: 'Tienda', resultado: 'OK' },
    { fecha: '2026-11-10 09:05', usuario: 'dromero', accion: 'Actualizó mantenimiento', modulo: 'Equipamiento', resultado: 'OK' },
    { fecha: '2026-11-10 08:40', usuario: 'lperez', accion: 'Dio de baja socio', modulo: 'Miembros', resultado: 'OK' },
    { fecha: '2026-11-10 08:12', usuario: 'sistema', accion: 'Error de sincronización', modulo: 'Reportes', resultado: 'Error' }
];

const reportsSeed = [
    { nombre: 'Ventas', periodo: 'Mes actual', valor: '$1.240.500', cambio: '+12%', estado: 'Activo' },
    { nombre: 'Cuotas cobradas', periodo: 'Mes actual', valor: '$430.800', cambio: '+8%', estado: 'Activo' },
    { nombre: 'Socios activos', periodo: 'Últimos 30 días', valor: '1.284', cambio: '+4%', estado: 'Activo' },
    { nombre: 'Stock crítico', periodo: 'Actual', valor: '14 productos', cambio: '-2%', estado: 'Atención' },
    { nombre: 'Mantenimientos', periodo: 'Semana', valor: '23 tareas', cambio: '+5%', estado: 'Activo' }
];

const configSeed = [
    { nombre: 'Casa Central', valor: 'Sede principal', ubicacion: 'CABA', estado: 'Activa' },
    { nombre: 'Turno mañana', valor: '08:00 - 16:00', ubicacion: 'Todas', estado: 'Activo' },
    { nombre: 'Turno tarde', valor: '16:00 - 00:00', ubicacion: 'Todas', estado: 'Activo' },
    { nombre: 'Forma de pago', valor: 'Efectivo, tarjeta, transferencia', ubicacion: 'Global', estado: 'Activo' },
    { nombre: 'Parámetro global', valor: 'Días de mora: 15', ubicacion: 'Global', estado: 'Activo' }
];

function renderUsers(rows) {
    const tbody = document.getElementById('usuariosTableBody');
    if (!tbody) return;

    tbody.innerHTML = rows.map((user) => `
        <tr>
            <td>
                <div class="d-flex align-items-center gap-3">
                    <div class="rounded-circle bg-primary-subtle text-primary d-flex align-items-center justify-content-center" style="width: 36px; height: 36px;">
                        <i class="bi bi-person"></i>
                    </div>
                    <div>
                        <div class="fw-semibold">${escHtml(user.nombre || user.alias)}</div>
                        <small class="text-secondary">${escHtml(user.email || (user.alias + '@fitlife.com'))}</small>
                    </div>
                </div>
            </td>
            <td>${user.rol || 'General'}</td>
            <td>${user.sede || 'Casa Central'}</td>
            <td>${user.ultimoAcceso || 'Reciente'}</td>
            <td><span class="badge text-bg-${user.estado === 'Inactivo' ? 'secondary' : 'success'}">${user.estado || 'Activo'}</span></td>
            <td>
                <button class="btn btn-sm btn-outline-primary me-2" data-action="edit" data-id="${user.id}">Editar</button>
                ${user.estado === 'Inactivo'
                    ? `<button class="btn btn-sm btn-outline-success" data-action="activate" data-id="${user.id}">Reactivar</button>`
                    : `<button class="btn btn-sm btn-outline-danger" data-action="deactivate" data-id="${user.id}">Inactivar</button>`}
            </td>
        </tr>
    `).join('') || '<tr><td colspan="6" class="text-center text-secondary py-4">Sin usuarios registrados</td></tr>';

    tbody.querySelectorAll('[data-action="edit"]').forEach(btn => {
        btn.addEventListener('click', () => {
            const id = Number(btn.getAttribute('data-id'));
            const user = currentUsers.find(u => u.id === id);
            if (user) openEditUserModal(user);
        });
    });

    tbody.querySelectorAll('[data-action="deactivate"]').forEach(btn => {
        btn.addEventListener('click', async () => {
            // El id sale del boton presionado: se inactiva exactamente ese usuario.
            const id = Number(btn.getAttribute('data-id'));
            const user = currentUsers.find(u => u.id === id);
            if (confirm(`¿Desea inactivar al usuario "${user?.alias || user?.nombre || id}"? No podrá iniciar sesión hasta que se lo reactive.`)) {
                await setUserActive(id, false);
            }
        });
    });

    tbody.querySelectorAll('[data-action="activate"]').forEach(btn => {
        btn.addEventListener('click', async () => {
            const id = Number(btn.getAttribute('data-id'));
            await setUserActive(id, true);
        });
    });
}

function openEditUserModal(user) {
    editingUserId = user.id;
    document.getElementById('usuarioNombre').value = user.nombre || '';
    document.getElementById('usuarioAlias').value = user.alias || user.nombre || '';
    document.getElementById('usuarioEmail').value = user.email || '';
    if (document.getElementById('usuarioRol')) document.getElementById('usuarioRol').value = user.rol || 'Gerente General';
    if (document.getElementById('usuarioSede')) document.getElementById('usuarioSede').value = user.sede || 'Casa Central';
    if (document.getElementById('usuarioEstado')) document.getElementById('usuarioEstado').value = user.estado || 'Activo';

    const modalEl = document.getElementById('modalUsuario');
    if (modalEl) {
        const titleEl = modalEl.querySelector('.modal-title');
        if (titleEl) titleEl.textContent = 'Editar usuario';
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();
    }
}

async function fetchUsers() {
    try {
        const res = await fetch('/api/Usuarios', { headers: getAuthHeader() });
        if (res.ok) {
            currentUsers = await res.json();
            return currentUsers;
        }
        throw new Error(await readError(res, `Error ${res.status}`));
    } catch (e) {
        console.warn('Error fetching /api/Usuarios', e);
        currentUsers = [];
        const tbody = document.getElementById('usuariosTableBody');
        if (tbody) {
            tbody.innerHTML = `<tr><td colspan="6" class="text-center text-danger py-4">No se pudieron cargar los usuarios: ${escHtml(e.message)}</td></tr>`;
        }
        return currentUsers;
    }
}

// Inactiva / reactiva (NO elimina) exactamente al usuario indicado por id.
async function setUserActive(id, activo) {
    try {
        const res = await fetch(`/api/Usuarios/${id}/${activo ? 'activar' : 'inactivar'}`, {
            method: 'PUT',
            headers: getAuthHeader()
        });
        if (!res.ok) {
            throw new Error(await readError(res, `Error ${res.status}`));
        }
        const updated = await res.json();
        const idx = currentUsers.findIndex(u => u.id === id);
        if (idx >= 0) currentUsers[idx] = { ...currentUsers[idx], ...updated };
        alert(activo ? 'Usuario reactivado con éxito' : 'Usuario inactivado con éxito');
    } catch (e) {
        console.warn('Error cambiando estado del usuario', e);
        alert('No se pudo cambiar el estado del usuario: ' + e.message);
    }
    refreshUsersView();
}

function initUsuarios() {
    const search = document.getElementById('searchUsuario');
    const filterRol = document.getElementById('filterRol');
    const filterEstado = document.getElementById('filterEstado');
    const form = document.getElementById('usuarioForm');
    const modalEl = document.getElementById('modalUsuario');

    modalEl?.addEventListener('show.bs.modal', (e) => {
        // If triggered by the "Nuevo usuario" button
        if (!editingUserId) {
            form.reset();
            const titleEl = modalEl.querySelector('.modal-title');
            if (titleEl) titleEl.textContent = 'Alta de usuario';
        }
    });

    modalEl?.addEventListener('hidden.bs.modal', () => {
        editingUserId = null;
        form.reset();
    });

    function applyFilters() {
        const query = (search?.value || '').trim().toLowerCase();
        const rol = filterRol?.value || 'Todos';
        const estado = filterEstado?.value || 'Todos';

        const filtered = currentUsers.filter((user) => {
            const matchesQuery = !query || [user.nombre, user.alias, user.email].join(' ').toLowerCase().includes(query);
            const matchesRol = rol === 'Todos' || user.rol === rol;
            const matchesEstado = estado === 'Todos' || user.estado === estado;
            return matchesQuery && matchesRol && matchesEstado;
        });

        renderUsers(filtered);
    }

    refreshUsersView = applyFilters;

    search?.addEventListener('input', applyFilters);
    filterRol?.addEventListener('change', applyFilters);
    filterEstado?.addEventListener('change', applyFilters);

    form?.addEventListener('submit', async (event) => {
        event.preventDefault();
        const payload = {
            nombre: document.getElementById('usuarioNombre').value.trim(),
            alias: document.getElementById('usuarioAlias').value.trim(),
            email: document.getElementById('usuarioEmail').value.trim(),
            rol: document.getElementById('usuarioRol')?.value || 'Gerente General',
            sede: document.getElementById('usuarioSede')?.value || 'Casa Central',
            estado: document.getElementById('usuarioEstado')?.value || 'Activo',
            contrasena: '123456'
        };

        if (!payload.nombre || !payload.alias || !payload.email) {
            alert('Complete nombre, usuario y email.');
            return;
        }

        try {
            if (editingUserId) {
                const res = await fetch(`/api/Usuarios/${editingUserId}`, {
                    method: 'PUT',
                    headers: getAuthHeader(),
                    body: JSON.stringify(payload)
                });
                if (!res.ok) throw new Error(await readError(res, 'Error al actualizar usuario'));
                const updated = await res.json();
                const idx = currentUsers.findIndex(u => u.id === editingUserId);
                if (idx >= 0) currentUsers[idx] = updated;
            } else {
                const res = await fetch('/api/Usuarios', {
                    method: 'POST',
                    headers: getAuthHeader(),
                    body: JSON.stringify(payload)
                });
                if (!res.ok) throw new Error(await readError(res, 'Error al crear usuario'));
                const created = await res.json();
                currentUsers.push(created);
            }
        } catch (e) {
            console.error('Error saving user', e);
            alert('No se pudo guardar el usuario: ' + e.message);
            return;
        }

        editingUserId = null;
        form.reset();
        bootstrap.Modal.getInstance(modalEl)?.hide();
        applyFilters();
    });

    fetchUsers().then(() => applyFilters());
}

function renderAudit(rows) {
    const tbody = document.getElementById('auditoriaTableBody');
    if (!tbody) return;

    tbody.innerHTML = rows.map((entry) => `
        <tr>
            <td>${entry.fecha || entry.fechaHora || '-'}</td>
            <td>${entry.usuario || entry.socio || '-'}</td>
            <td>${entry.accion || entry.motivoRechazo || 'Acceso'}</td>
            <td>${entry.modulo || entry.sedeDelIntento || 'Miembros'}</td>
            <td><span class="badge text-bg-${entry.resultado === 'Error' || entry.intentosDenegados ? 'danger' : 'success'}">${entry.resultado || 'Rechazado'}</span></td>
        </tr>
    `).join('') || '<tr><td colspan="5" class="text-center text-secondary py-4">Sin eventos</td></tr>';
}

function renderReports(rows) {
    const tbody = document.getElementById('reportesTableBody');
    if (!tbody) return;

    tbody.innerHTML = rows.map((row) => `
        <tr>
            <td>${row.nombre}</td>
            <td>${row.periodo}</td>
            <td>${row.valor}</td>
            <td>${row.cambio}</td>
            <td><span class="badge text-bg-${row.estado === 'Atención' ? 'warning' : 'success'}">${row.estado}</span></td>
        </tr>
    `).join('') || '<tr><td colspan="5" class="text-center text-secondary py-4">Sin reportes</td></tr>';
}

function renderConfig(rows) {
    const tbody = document.getElementById('configTableBody');
    if (!tbody) return;

    tbody.innerHTML = rows.map((row) => `
        <tr>
            <td>${row.nombre}</td>
            <td>${row.valor}</td>
            <td>${row.ubicacion}</td>
            <td><span class="badge text-bg-success">${row.estado}</span></td>
        </tr>
    `).join('') || '<tr><td colspan="4" class="text-center text-secondary py-4">Sin configuraciones</td></tr>';
}

async function initAuditoria() {
    try {
        const res = await fetch('/api/Auditoria/accesos-denegados', { headers: getAuthHeader() });
        if (res.ok) {
            const data = await res.json();
            if (Array.isArray(data) && data.length) {
                renderAudit(data);
                return;
            }
        }
    } catch (e) {
        console.warn('Using auditSeed', e);
    }
    renderAudit(auditSeed);
}

function initReportes() {
    renderReports(reportsSeed);
}

function initConfiguracion() {
    renderConfig(configSeed);
}

document.addEventListener('DOMContentLoaded', () => {
    const page = document.body.dataset.page || window.location.pathname.split('/').pop();

    if (page === 'usuarios.html') initUsuarios();
    if (page === 'auditoria.html') initAuditoria();
    if (page === 'reportes.html') initReportes();
    if (page === 'configuracion.html') initConfiguracion();
});
