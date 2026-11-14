// FitLife Network - Equipamiento Module JavaScript

(function () {
    function getAuthToken() {
        return localStorage.getItem('fitlife_token') || localStorage.getItem('token');
    }

    function getAuthHeader() {
        const token = getAuthToken();
        return token
            ? { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json', 'Accept': 'application/json' }
            : { 'Content-Type': 'application/json', 'Accept': 'application/json' };
    }

    async function apiFetch(url, options = {}) {
        const headers = { ...getAuthHeader(), ...(options.headers || {}) };
        try {
            const res = await fetch(url, { ...options, headers });
            if (res.status === 401 || res.status === 403) {
                console.warn('Unauthorized request to', url);
            }
            if (!res.ok) {
                const text = await res.text();
                let msg = text;
                try {
                    const j = JSON.parse(text);
                    if (typeof j === 'string') msg = j;
                    else if (j && j.errors) msg = Object.values(j.errors).flat().join(' ');
                    else if (j) msg = j.error || j.message || j.title || text;
                } catch (e) { /* texto plano */ }
                if (res.status === 401) msg = 'Sesión expirada o no iniciada. Vuelva a iniciar sesión.';
                if (res.status === 403) msg = 'No tiene permisos para realizar esta acción.';
                throw new Error(msg || `Error ${res.status}`);
            }
            if (res.status === 204) return null;
            return await res.json();
        } catch (err) {
            console.error('API Error:', err);
            throw err;
        }
    }

    window.apiFetch = apiFetch;

    function fillSelect(selectEl, items, valueField, textField, defaultLabel) {
        if (!selectEl) return;
        selectEl.innerHTML = defaultLabel ? `<option value="">${defaultLabel}</option>` : '';
        if (!Array.isArray(items)) return;
        items.forEach(item => {
            const val = item[valueField];
            const text = typeof textField === 'function' ? textField(item) : item[textField];
            const opt = document.createElement('option');
            opt.value = val;
            opt.textContent = text;
            selectEl.appendChild(opt);
        });
    }

    function esc(v) {
        return String(v ?? '').replace(/[&<>"']/g, ch => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[ch]));
    }

    function nombreCompleto(p) {
        return [p?.apellido, p?.nombre].filter(Boolean).join(', ') || '-';
    }

    function hoy() {
        const d = new Date();
        const p = n => String(n).padStart(2, '0');
        return `${d.getFullYear()}-${p(d.getMonth() + 1)}-${p(d.getDate())}`;
    }

    // Fechas "solo fecha" (yyyy-MM-dd): se formatean sin pasar por Date para evitar el corrimiento de un dia por zona horaria.
    function formatFechaSolo(v) {
        if (!v) return '-';
        const m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v));
        if (m) return `${m[3]}/${m[2]}/${m[1]}`;
        const d = new Date(v);
        return isNaN(d) ? '-' : d.toLocaleDateString('es-AR');
    }

    function formatFechaHora(v) {
        if (!v) return '-';
        const d = new Date(v);
        if (isNaN(d)) return '-';
        return d.toLocaleDateString('es-AR') + ' ' + d.toLocaleTimeString('es-AR', { hour: '2-digit', minute: '2-digit' });
    }

    function money(n) {
        return '$' + Number(n || 0).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function errorRow(cols, msg) {
        return `<tr><td colspan="${cols}" class="text-center text-danger py-4">${esc(msg)}</td></tr>`;
    }

    // Muestra un aviso (alert de Bootstrap) justo antes del elemento indicado.
    function showNotice(beforeEl, message, type = 'warning') {
        if (!beforeEl || !beforeEl.parentNode) return;
        const prev = beforeEl.parentNode.querySelector('[data-notice]');
        if (prev) prev.remove();
        if (!message) return;
        const div = document.createElement('div');
        div.setAttribute('data-notice', '1');
        div.className = `alert alert-${type} py-2`;
        div.textContent = message;
        beforeEl.parentNode.insertBefore(div, beforeEl);
    }

    // ==========================================
    // 1. EQUIPOS
    // ==========================================
    async function initEquipos() {
        const tbody = document.getElementById('equiposTableBody');
        const sedeFilter = document.getElementById('equipoSedeFilter');
        const form = document.getElementById('equipoForm');
        const sedeSelect = document.getElementById('equipoIdSede');
        const modalEl = document.getElementById('modalEquipo');
        let editingEquipoId = null;
        let equiposList = [];

        async function loadSedes() {
            try {
                const sedes = await apiFetch('/api/Sedes') || [];
                fillSelect(sedeSelect, sedes, 'idSede', 'nombreSede', 'Seleccione sede');
                fillSelect(sedeFilter, sedes, 'idSede', 'nombreSede', 'Todas las sedes');
            } catch (e) {
                console.warn('Error loading sedes', e);
            }
        }

        async function loadEquipos(idSede = null) {
            try {
                const url = idSede ? `/api/Equipos?idSede=${idSede}` : '/api/Equipos';
                equiposList = await apiFetch(url) || [];
                renderEquipos(equiposList);
            } catch (e) {
                console.warn('Error loading equipos', e);
                equiposList = [];
                if (tbody) tbody.innerHTML = errorRow(6, 'No se pudieron cargar los equipos: ' + e.message);
            }
        }

        function renderEquipos(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(eq => `
                <tr>
                    <td class="fw-semibold">${esc(eq.nombreMaquina || eq.nombre || '-')}</td>
                    <td>${esc(eq.nombreSede || (eq.idSede ? 'Sede ' + eq.idSede : '-'))}</td>
                    <td><code>${esc(eq.numeroSerie || '-')}</code></td>
                    <td>${formatFechaSolo(eq.fechaAdquisicion)}</td>
                    <td><span class="badge text-bg-${eq.estadoOperativo === 'ACTIVO' ? 'success' : 'warning'}">${esc(eq.estadoOperativo || 'ACTIVO')}</span></td>
                    <td>
                        <button class="btn btn-sm btn-outline-primary me-1" data-edit-eq="${eq.idEquipo}">Editar</button>
                        <button class="btn btn-sm btn-outline-danger" data-del-eq="${eq.idEquipo}">Eliminar</button>
                    </td>
                </tr>
            `).join('') || '<tr><td colspan="6" class="text-center text-secondary py-4">Sin equipos registrados</td></tr>';

            tbody.querySelectorAll('[data-edit-eq]').forEach(btn => {
                btn.addEventListener('click', () => {
                    const id = Number(btn.getAttribute('data-edit-eq'));
                    const eq = equiposList.find(x => x.idEquipo === id);
                    if (eq) {
                        editingEquipoId = eq.idEquipo;
                        document.getElementById('equipoId').value = eq.idEquipo;
                        document.getElementById('equipoNombre').value = eq.nombreMaquina || '';
                        document.getElementById('equipoNumeroSerie').value = eq.numeroSerie || '';
                        document.getElementById('equipoFechaAdquisicion').value = eq.fechaAdquisicion ? String(eq.fechaAdquisicion).split('T')[0] : '';
                        if (document.getElementById('equipoEstadoOperativo')) document.getElementById('equipoEstadoOperativo').value = eq.estadoOperativo || 'ACTIVO';
                        if (sedeSelect) sedeSelect.value = eq.idSede || '';
                        bootstrap.Modal.getOrCreateInstance(modalEl).show();
                    }
                });
            });

            tbody.querySelectorAll('[data-del-eq]').forEach(btn => {
                btn.addEventListener('click', async () => {
                    const id = Number(btn.getAttribute('data-del-eq'));
                    const eq = equiposList.find(x => x.idEquipo === id);
                    const msg = `¿Desea eliminar el equipo "${eq?.nombreMaquina || '#' + id}"?\n\nSe eliminarán también sus mantenimientos registrados y se desvincularán sus incidencias.`;
                    if (!confirm(msg)) return;
                    try {
                        await apiFetch(`/api/Equipos/${id}`, { method: 'DELETE' });
                        alert('Equipo eliminado');
                    } catch (err) {
                        console.warn('Error deleting equipo', err);
                        alert('No se pudo eliminar el equipo: ' + err.message);
                    }
                    await loadEquipos(sedeFilter?.value);
                });
            });
        }

        sedeFilter?.addEventListener('change', () => {
            loadEquipos(sedeFilter.value);
        });

        modalEl?.addEventListener('hidden.bs.modal', () => {
            editingEquipoId = null;
            form.reset();
        });

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            if (!sedeSelect?.value) {
                alert('Seleccione la sede');
                return;
            }
            const payload = {
                idSede: Number(sedeSelect.value),
                nombreMaquina: document.getElementById('equipoNombre').value.trim(),
                numeroSerie: document.getElementById('equipoNumeroSerie').value.trim(),
                fechaAdquisicion: document.getElementById('equipoFechaAdquisicion').value || hoy(),
                estadoOperativo: document.getElementById('equipoEstadoOperativo')?.value || 'ACTIVO'
            };

            try {
                if (editingEquipoId) {
                    await apiFetch(`/api/Equipos/${editingEquipoId}`, { method: 'PUT', body: JSON.stringify(payload) });
                    alert('Equipo actualizado');
                } else {
                    await apiFetch('/api/Equipos', { method: 'POST', body: JSON.stringify(payload) });
                    alert('Equipo creado');
                }
            } catch (err) {
                console.warn('Error saving equipo', err);
                alert('No se pudo guardar el equipo: ' + err.message);
                return;
            }

            editingEquipoId = null;
            form.reset();
            bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadEquipos(sedeFilter?.value);
        });

        await loadSedes();
        await loadEquipos();
    }

    // ==========================================
    // 2. INCIDENCIAS
    // ==========================================
    async function initIncidencias() {
        const tbody = document.getElementById('incidenciasTableBody');
        const form = document.getElementById('incidenciaForm');
        const socioSelect = document.getElementById('incidenciaIdSocio');
        const sedeSelect = document.getElementById('incidenciaIdSede');
        const eqSelect = document.getElementById('incidenciaIdEquipo');
        const estadoSelect = document.getElementById('incidenciaIdEstado');
        const modalEl = document.getElementById('modalIncidencia');

        async function loadSelects() {
            const fallos = [];
            const safe = (url, nombre) => apiFetch(url).catch(err => {
                console.warn('Error cargando', url, err);
                fallos.push(`${nombre} (${err.message})`);
                return [];
            });

            const [socios, sedes, equipos, estados] = await Promise.all([
                safe('/api/Socios', 'socios'),
                safe('/api/Sedes', 'sedes'),
                safe('/api/Equipos', 'equipos'),
                safe('/api/EstadosReclamo', 'estados')
            ]);

            fillSelect(socioSelect, socios, 'idSocio', s => `${nombreCompleto(s)} (DNI ${s.dni || '-'})`, 'Seleccione socio');
            fillSelect(sedeSelect, sedes, 'idSede', 'nombreSede', 'Seleccione sede');
            fillSelect(eqSelect, equipos, 'idEquipo', e => `${e.nombreMaquina || e.nombre} (${e.numeroSerie || ''})`, 'Sin equipo / no aplica');
            fillSelect(estadoSelect, estados, 'idEstadoReclamo', 'nombreEstado', 'Seleccione estado');

            if (fallos.length) showNotice(document.querySelector('main .card'), 'No se pudieron cargar: ' + fallos.join('; '));
        }

        async function loadIncidencias() {
            if (!tbody) return;
            try {
                const incs = await apiFetch('/api/Incidencias') || [];
                renderIncidencias(incs);
            } catch (e) {
                console.warn('Error loading incidencias', e);
                tbody.innerHTML = errorRow(7, 'No se pudieron cargar las incidencias: ' + e.message);
            }
        }

        function renderIncidencias(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(inc => `
                <tr>
                    <td>#${inc.idReclamo ?? '-'}</td>
                    <td>${esc(inc.nombreSede || (inc.idSede ? 'Sede ' + inc.idSede : '-'))}</td>
                    <td>${esc(inc.nombreEquipo || '-')}</td>
                    <td>${esc(inc.descripcion || '-')}</td>
                    <td>${formatFechaHora(inc.fechaRegistro)}</td>
                    <td><span class="badge text-bg-${inc.estadoReclamo === 'RESUELTO' ? 'success' : 'warning'}">${esc(inc.estadoReclamo || '-')}</span></td>
                    <td>
                        <button class="btn btn-sm btn-outline-danger" data-del-inc="${inc.idReclamo}">Eliminar</button>
                    </td>
                </tr>
            `).join('') || '<tr><td colspan="7" class="text-center text-secondary py-4">Sin incidencias registradas</td></tr>';

            tbody.querySelectorAll('[data-del-inc]').forEach(btn => {
                btn.addEventListener('click', async () => {
                    const id = Number(btn.getAttribute('data-del-inc'));
                    if (!confirm('¿Desea eliminar esta incidencia?')) return;
                    try {
                        await apiFetch(`/api/Incidencias/${id}`, { method: 'DELETE' });
                        alert('Incidencia eliminada');
                    } catch (err) {
                        alert('No se pudo eliminar la incidencia: ' + err.message);
                    }
                    await loadIncidencias();
                });
            });
        }

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            if (!socioSelect?.value || !sedeSelect?.value || !estadoSelect?.value) {
                alert('Seleccione socio, sede y estado');
                return;
            }
            const payload = {
                idSocio: Number(socioSelect.value),
                idSede: Number(sedeSelect.value),
                idEquipo: eqSelect?.value ? Number(eqSelect.value) : null,
                idEstadoReclamo: Number(estadoSelect.value),
                descripcion: document.getElementById('incidenciaDescripcion').value.trim()
            };

            try {
                await apiFetch('/api/Incidencias', { method: 'POST', body: JSON.stringify(payload) });
                alert('Incidencia registrada con éxito');
            } catch (err) {
                console.warn('POST /api/Incidencias error', err);
                alert('No se pudo registrar la incidencia: ' + err.message);
                return;
            }

            form.reset();
            bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadIncidencias();
        });

        await loadSelects();
        await loadIncidencias();
    }

    // ==========================================
    // 3. MANTENIMIENTOS
    // ==========================================
    async function initMantenimientos() {
        const tbody = document.getElementById('mantenimientosTableBody');
        const form = document.getElementById('mantenimientoForm');
        const eqSelect = document.getElementById('mantenimientoIdEquipo');
        const provSelect = document.getElementById('mantenimientoIdProveedor');
        const fechaInput = document.getElementById('mantenimientoFecha');
        const modalEl = document.getElementById('modalMantenimiento');

        async function loadSelects() {
            const fallos = [];
            const safe = (url, nombre) => apiFetch(url).catch(err => {
                console.warn('Error cargando', url, err);
                fallos.push(`${nombre} (${err.message})`);
                return [];
            });

            const [equipos, proveedores] = await Promise.all([
                safe('/api/Equipos', 'equipos'),
                safe('/api/Proveedores', 'proveedores')
            ]);

            fillSelect(eqSelect, equipos, 'idEquipo', e => `${e.nombreMaquina || e.nombre} (${e.numeroSerie || ''})`, 'Seleccione equipo');
            fillSelect(provSelect, proveedores, 'idProveedor', 'razonSocial', 'Sin proveedor / técnico interno');

            if (fallos.length) showNotice(document.querySelector('main .card'), 'No se pudieron cargar: ' + fallos.join('; '));
        }

        async function loadMantenimientos() {
            if (!tbody) return;
            try {
                const mant = await apiFetch('/api/Mantenimiento') || [];
                renderMantenimientos(mant);
            } catch (e) {
                console.warn('Error loading mantenimientos', e);
                tbody.innerHTML = errorRow(7, 'No se pudieron cargar los mantenimientos: ' + e.message);
            }
        }

        function renderMantenimientos(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(m => `
                <tr>
                    <td>#${m.idMantenimiento ?? '-'}</td>
                    <td>${esc(m.nombreEquipo || (m.idEquipo ? 'Equipo #' + m.idEquipo : '-'))}</td>
                    <td>${esc(m.nombreProveedor || 'Sin proveedor')}</td>
                    <td>${formatFechaSolo(m.fechaMantenimiento)}</td>
                    <td>${money(m.costo)}</td>
                    <td>${esc(m.descripcion || '-')}</td>
                    <td>
                        <button class="btn btn-sm btn-outline-danger" data-del-mant="${m.idMantenimiento}">Eliminar</button>
                    </td>
                </tr>
            `).join('') || '<tr><td colspan="7" class="text-center text-secondary py-4">Sin mantenimientos registrados</td></tr>';

            tbody.querySelectorAll('[data-del-mant]').forEach(btn => {
                btn.addEventListener('click', async () => {
                    const id = Number(btn.getAttribute('data-del-mant'));
                    if (!confirm('¿Desea eliminar este registro?')) return;
                    try {
                        await apiFetch(`/api/Mantenimiento/${id}`, { method: 'DELETE' });
                        alert('Mantenimiento eliminado');
                    } catch (err) {
                        alert('No se pudo eliminar el mantenimiento: ' + err.message);
                    }
                    await loadMantenimientos();
                });
            });
        }

        modalEl?.addEventListener('show.bs.modal', () => {
            if (fechaInput && !fechaInput.value) fechaInput.value = hoy();
        });

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            if (!eqSelect?.value) {
                alert('Seleccione el equipo');
                return;
            }
            const payload = {
                idEquipo: Number(eqSelect.value),
                idProveedor: provSelect?.value ? Number(provSelect.value) : null,
                // El backend espera una fecha (yyyy-MM-dd); enviar un ISO con hora provocaba un error de formato.
                fechaMantenimiento: fechaInput?.value || hoy(),
                costo: Math.max(0, parseFloat(document.getElementById('mantenimientoCosto').value || 0)),
                descripcion: document.getElementById('mantenimientoDescripcion').value.trim()
            };

            try {
                await apiFetch('/api/Mantenimiento', { method: 'POST', body: JSON.stringify(payload) });
                alert('Mantenimiento registrado con éxito');
            } catch (err) {
                console.warn('POST /api/Mantenimiento error', err);
                alert('No se pudo registrar el mantenimiento: ' + err.message);
                return;
            }

            form.reset();
            bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadMantenimientos();
        });

        await loadSelects();
        await loadMantenimientos();
    }

    // ==========================================
    // 4. PROVEEDORES
    // ==========================================
    async function initProveedores() {
        const tbody = document.getElementById('proveedoresTableBody');
        const form = document.getElementById('proveedorForm');
        const modalEl = document.getElementById('modalProveedor');
        let editingProvId = null;
        let provList = [];

        async function loadProveedores() {
            if (!tbody) return;
            try {
                provList = await apiFetch('/api/Proveedores') || [];
                renderProveedores(provList);
            } catch (e) {
                console.warn('Error loading proveedores', e);
                provList = [];
                tbody.innerHTML = errorRow(5, 'No se pudieron cargar los proveedores: ' + e.message);
            }
        }

        function renderProveedores(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(p => `
                <tr>
                    <td class="fw-semibold">${esc(p.razonSocial || '-')}</td>
                    <td><code>${esc(p.cuit || '-')}</code></td>
                    <td>${esc(p.telefono || '-')}</td>
                    <td>${esc(p.email || '-')}</td>
                    <td>
                        <button class="btn btn-sm btn-outline-primary me-1" data-edit-prov="${p.idProveedor}">Editar</button>
                        <button class="btn btn-sm btn-outline-danger" data-del-prov="${p.idProveedor}">Eliminar</button>
                    </td>
                </tr>
            `).join('') || '<tr><td colspan="5" class="text-center text-secondary py-4">Sin proveedores registrados</td></tr>';

            tbody.querySelectorAll('[data-edit-prov]').forEach(btn => {
                btn.addEventListener('click', () => {
                    const id = Number(btn.getAttribute('data-edit-prov'));
                    const p = provList.find(x => x.idProveedor === id);
                    if (p) {
                        editingProvId = p.idProveedor;
                        document.getElementById('proveedorRazonSocial').value = p.razonSocial || '';
                        document.getElementById('proveedorCuit').value = p.cuit || '';
                        document.getElementById('proveedorTelefono').value = p.telefono || '';
                        document.getElementById('proveedorEmail').value = p.email || '';
                        bootstrap.Modal.getOrCreateInstance(modalEl).show();
                    }
                });
            });

            tbody.querySelectorAll('[data-del-prov]').forEach(btn => {
                btn.addEventListener('click', async () => {
                    const id = Number(btn.getAttribute('data-del-prov'));
                    const p = provList.find(x => x.idProveedor === id);
                    if (!confirm(`¿Desea eliminar al proveedor "${p?.razonSocial || '#' + id}"?`)) return;
                    try {
                        const res = await apiFetch(`/api/Proveedores/${id}`, { method: 'DELETE' });
                        alert(res?.message || 'Proveedor eliminado');
                    } catch (err) {
                        console.warn('Error deleting proveedor', err);
                        alert('No se pudo eliminar el proveedor: ' + err.message);
                    }
                    await loadProveedores();
                });
            });
        }

        modalEl?.addEventListener('hidden.bs.modal', () => {
            editingProvId = null;
            form.reset();
        });

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            const payload = {
                razonSocial: document.getElementById('proveedorRazonSocial').value.trim(),
                cuit: document.getElementById('proveedorCuit').value.trim(),
                telefono: document.getElementById('proveedorTelefono').value.trim(),
                email: document.getElementById('proveedorEmail').value.trim()
            };

            try {
                if (editingProvId) {
                    await apiFetch(`/api/Proveedores/${editingProvId}`, { method: 'PUT', body: JSON.stringify(payload) });
                    alert('Proveedor actualizado');
                } else {
                    await apiFetch('/api/Proveedores', { method: 'POST', body: JSON.stringify(payload) });
                    alert('Proveedor creado');
                }
            } catch (err) {
                console.warn('Error saving proveedor', err);
                alert('No se pudo guardar el proveedor: ' + err.message);
                return;
            }

            editingProvId = null;
            form.reset();
            bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadProveedores();
        });

        await loadProveedores();
    }

    // DOM Router for Equipamiento pages
    document.addEventListener('DOMContentLoaded', () => {
        const page = document.body.dataset.page || window.location.pathname.split('/').pop();
        if (page === 'equipos.html') initEquipos();
        else if (page === 'incidencias.html') initIncidencias();
        else if (page === 'mantenimientos.html') initMantenimientos();
        else if (page === 'proveedores.html') initProveedores();
    });
})();
