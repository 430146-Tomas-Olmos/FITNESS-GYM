const usersSeed = [
    { nombre: 'María López', alias: 'mlopez', email: 'mlopez@fitlife.com', rol: 'Gerente General', sede: 'Casa Central', estado: 'Activo', ultimoAcceso: '2026-11-10 09:42' },
    { nombre: 'Luis Pérez', alias: 'lperez', email: 'lperez@fitlife.com', rol: 'Gerente Miembros', sede: 'Belgrano', estado: 'Activo', ultimoAcceso: '2026-11-10 08:55' },
    { nombre: 'Sofía Costa', alias: 'scosta', email: 'scosta@fitlife.com', rol: 'Gerente Ventas', sede: 'San Isidro', estado: 'Activo', ultimoAcceso: '2026-11-10 09:10' },
    { nombre: 'Diego Romero', alias: 'dromero', email: 'dromero@fitlife.com', rol: 'Técnico', sede: 'Casa Central', estado: 'Activo', ultimoAcceso: '2026-11-10 07:30' },
    { nombre: 'Ana Morales', alias: 'amorales', email: 'amorales@fitlife.com', rol: 'Vendedor', sede: 'Belgrano', estado: 'Inactivo', ultimoAcceso: '2026-11-08 18:15' }
];

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
                        <div class="fw-semibold">${user.nombre}</div>
                        <small class="text-secondary">${user.email}</small>
                    </div>
                </div>
            </td>
            <td>${user.rol}</td>
            <td>${user.sede}</td>
            <td>${user.ultimoAcceso}</td>
            <td><span class="badge text-bg-${user.estado === 'Activo' ? 'success' : 'secondary'}">${user.estado}</span></td>
            <td>
                <button class="btn btn-sm btn-outline-primary me-2" data-action="edit" data-index="${usersSeed.indexOf(user)}">Editar</button>
                <button class="btn btn-sm btn-outline-danger" data-action="delete" data-index="${usersSeed.indexOf(user)}">Inactivar</button>
            </td>
        </tr>
    `).join('') || '<tr><td colspan="6" class="text-center text-secondary py-4">Sin usuarios</td></tr>';
}

function renderAudit(rows) {
    const tbody = document.getElementById('auditoriaTableBody');
    if (!tbody) return;

    tbody.innerHTML = rows.map((entry) => `
        <tr>
            <td>${entry.fecha}</td>
            <td>${entry.usuario}</td>
            <td>${entry.accion}</td>
            <td>${entry.modulo}</td>
            <td><span class="badge text-bg-${entry.resultado === 'Error' ? 'danger' : 'success'}">${entry.resultado}</span></td>
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

function initUsuarios() {
    const search = document.getElementById('searchUsuario');
    const filterRol = document.getElementById('filterRol');
    const filterEstado = document.getElementById('filterEstado');
    const form = document.getElementById('usuarioForm');

    function applyFilters() {
        const query = (search?.value || '').trim().toLowerCase();
        const rol = filterRol?.value || 'Todos';
        const estado = filterEstado?.value || 'Todos';

        const filtered = usersSeed.filter((user) => {
            const matchesQuery = !query || [user.nombre, user.alias, user.email].join(' ').toLowerCase().includes(query);
            const matchesRol = rol === 'Todos' || user.rol === rol;
            const matchesEstado = estado === 'Todos' || user.estado === estado;
            return matchesQuery && matchesRol && matchesEstado;
        });

        renderUsers(filtered);
    }

    search?.addEventListener('input', applyFilters);
    filterRol?.addEventListener('change', applyFilters);
    filterEstado?.addEventListener('change', applyFilters);

    form?.addEventListener('submit', (event) => {
        event.preventDefault();
        const entry = {
            nombre: document.getElementById('usuarioNombre').value.trim(),
            alias: document.getElementById('usuarioAlias').value.trim(),
            email: document.getElementById('usuarioEmail').value.trim(),
            rol: document.getElementById('usuarioRol').value,
            sede: document.getElementById('usuarioSede').value,
            estado: document.getElementById('usuarioEstado').value,
            ultimoAcceso: '2026-11-10 12:00'
        };

        if (!entry.nombre || !entry.alias || !entry.email) {
            alert('Complete nombre, usuario y email.');
            return;
        }

        usersSeed.unshift(entry);
        form.reset();
        bootstrap.Modal.getInstance(document.getElementById('modalUsuario'))?.hide();
        applyFilters();
    });

    renderUsers(usersSeed);
}

function initAuditoria() {
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
