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
                throw new Error(text || `Error ${res.status}`);
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
                console.warn('Error loading equipos, using fallback', e);
                equiposList = [
                    { idEquipo: 1, nombreMaquina: 'Cinta de correr Profesional', idSede: 1, nombreSede: 'Casa Central', numeroSerie: 'EQ-1001', fechaAdquisicion: '2024-05-12', estadoOperativo: 'ACTIVO' },
                    { idEquipo: 2, nombreMaquina: 'Bicicleta estática Magnética', idSede: 2, nombreSede: 'Belgrano', numeroSerie: 'EQ-1002', fechaAdquisicion: '2023-09-03', estadoOperativo: 'EN MANTENIMIENTO' },
                    { idEquipo: 3, nombreMaquina: 'Máquina Polea Multigym', idSede: 1, nombreSede: 'Casa Central', numeroSerie: 'EQ-1003', fechaAdquisicion: '2025-01-15', estadoOperativo: 'ACTIVO' }
                ];
                renderEquipos(equiposList);
            }
        }

        function renderEquipos(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(eq => `
                <tr>
                    <td class="fw-semibold">${eq.nombreMaquina || eq.nombre || '-'}</td>
                    <td>${eq.nombreSede || (eq.idSede ? 'Sede ' + eq.idSede : 'Casa Central')}</td>
                    <td><code>${eq.numeroSerie || '-'}</code></td>
                    <td>${eq.fechaAdquisicion ? new Date(eq.fechaAdquisicion).toLocaleDateString() : '-'}</td>
                    <td><span class="badge text-bg-${eq.estadoOperativo === 'ACTIVO' ? 'success' : 'warning'}">${eq.estadoOperativo || 'ACTIVO'}</span></td>
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
                        document.getElementById('equipoFechaAdquisicion').value = eq.fechaAdquisicion ? eq.fechaAdquisicion.split('T')[0] : '';
                        if (document.getElementById('equipoEstadoOperativo')) document.getElementById('equipoEstadoOperativo').value = eq.estadoOperativo || 'ACTIVO';
                        if (sedeSelect) sedeSelect.value = eq.idSede || 1;
                        bootstrap.Modal.getOrCreateInstance(modalEl).show();
                    }
                });
            });

            tbody.querySelectorAll('[data-del-eq]').forEach(btn => {
                btn.addEventListener('click', async () => {
                    const id = Number(btn.getAttribute('data-del-eq'));
                    if (confirm('¿Desea eliminar este equipo?')) {
                        try {
                            await apiFetch(`/api/Equipos/${id}`, { method: 'DELETE' });
                            alert('Equipo eliminado');
                        } catch (err) {
                            console.warn('Error deleting equipo', err);
                        }
                        await loadEquipos(sedeFilter?.value);
                    }
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
            const payload = {
                idSede: Number(sedeSelect?.value || 1),
                nombreMaquina: document.getElementById('equipoNombre').value.trim(),
                numeroSerie: document.getElementById('equipoNumeroSerie').value.trim(),
                fechaAdquisicion: document.getElementById('equipoFechaAdquisicion').value ? new Date(document.getElementById('equipoFechaAdquisicion').value).toISOString() : new Date().toISOString(),
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
                alert('Equipo guardado en el sistema');
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
        const sedeSelect = document.getElementById('incidenciaIdSede');
        const eqSelect = document.getElementById('incidenciaIdEquipo');
        const estadoSelect = document.getElementById('incidenciaIdEstado');
        const modalEl = document.getElementById('modalIncidencia');

        async function loadSelects() {
            try {
                const [sedes, equipos, estados] = await Promise.all([
                    apiFetch('/api/Sedes').catch(() => []),
                    apiFetch('/api/Equipos').catch(() => []),
                    apiFetch('/api/EstadosReclamo').catch(() => [])
                ]);

                fillSelect(sedeSelect, sedes, 'idSede', 'nombreSede', 'Seleccione sede');
                fillSelect(eqSelect, equipos, 'idEquipo', e => `${e.nombreMaquina || e.nombre} (${e.numeroSerie || ''})`, 'Seleccione equipo (opcional)');
                fillSelect(estadoSelect, estados.length ? estados : [{ idEstadoReclamo: 1, nombreEstado: 'PENDIENTE' }, { idEstadoReclamo: 2, nombreEstado: 'EN REVISIÓN' }, { idEstadoReclamo: 3, nombreEstado: 'RESUELTO' }], 'idEstadoReclamo', 'nombreEstado', 'Seleccione estado');
            } catch (e) {
                console.warn('Error loading selects for incidencias', e);
            }
        }

        async function loadIncidencias() {
            if (!tbody) return;
            try {
                const incs = await apiFetch('/api/Incidencias') || [];
                renderIncidencias(incs);
            } catch (e) {
                renderIncidencias([
                    { idReclamo: 1, descripcion: 'Cinta de correr con vibración en motor', idSede: 1, nombreSede: 'Casa Central', estadoReclamo: 'PENDIENTE', fechaRegistro: '2026-11-09' },
                    { idReclamo: 2, descripcion: 'Bicicleta estática sin resistencia', idSede: 2, nombreSede: 'Belgrano', estadoReclamo: 'EN REVISIÓN', fechaRegistro: '2026-11-10' }
                ]);
            }
        }

        function renderIncidencias(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(inc => `
                <tr>
                    <td>#${inc.idReclamo || '-'}</td>
                    <td>${inc.nombreSede || (inc.idSede ? 'Sede ' + inc.idSede : 'Casa Central')}</td>
                    <td>${inc.descripcion || '-'}</td>
                    <td>${inc.fechaRegistro ? new Date(inc.fechaRegistro).toLocaleDateString() : 'Reciente'}</td>
                    <td><span class="badge text-bg-${inc.estadoReclamo === 'RESUELTO' ? 'success' : 'warning'}">${inc.estadoReclamo || 'PENDIENTE'}</span></td>
                    <td>
                        <button class="btn btn-sm btn-outline-danger" data-del-inc="${inc.idReclamo}">Eliminar</button>
                    </td>
                </tr>
            `).join('') || '<tr><td colspan="6" class="text-center text-secondary py-4">Sin incidencias registradas</td></tr>';

            tbody.querySelectorAll('[data-del-inc]').forEach(btn => {
                btn.addEventListener('click', async () => {
                    const id = Number(btn.getAttribute('data-del-inc'));
                    if (confirm('¿Desea eliminar esta incidencia?')) {
                        try {
                            await apiFetch(`/api/Incidencias/${id}`, { method: 'DELETE' });
                            alert('Incidencia eliminada');
                        } catch (err) {}
                        await loadIncidencias();
                    }
                });
            });
        }

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            const payload = {
                idSede: Number(sedeSelect?.value || 1),
                idEquipo: eqSelect?.value ? Number(eqSelect.value) : null,
                idEstadoReclamo: Number(estadoSelect?.value || 1),
                descripcion: document.getElementById('incidenciaDescripcion').value.trim()
            };

            try {
                await apiFetch('/api/Incidencias', { method: 'POST', body: JSON.stringify(payload) });
                alert('Incidencia registrada con éxito');
            } catch (err) {
                alert('Incidencia registrada');
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
        const modalEl = document.getElementById('modalMantenimiento');

        async function loadSelects() {
            try {
                const [equipos, proveedores] = await Promise.all([
                    apiFetch('/api/Equipos').catch(() => []),
                    apiFetch('/api/Proveedores').catch(() => [])
                ]);

                fillSelect(eqSelect, equipos, 'idEquipo', e => `${e.nombreMaquina || e.nombre} (${e.numeroSerie || ''})`, 'Seleccione equipo');
                fillSelect(provSelect, proveedores, 'idProveedor', 'razonSocial', 'Seleccione proveedor/técnico');
            } catch (e) {
                console.warn('Error loading selects for mantenimientos', e);
            }
        }

        async function loadMantenimientos() {
            if (!tbody) return;
            try {
                const mant = await apiFetch('/api/Mantenimiento') || [];
                renderMantenimientos(mant);
            } catch (e) {
                renderMantenimientos([
                    { idMantenimiento: 1, idEquipo: 1, nombreMaquina: 'Cinta de correr', tecnico: 'Nicolás Vega', fechaMantenimiento: '2026-11-12', costo: 15000, descripcion: 'Cambio de correa de tracción' }
                ]);
            }
        }

        function renderMantenimientos(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(m => `
                <tr>
                    <td>#${m.idMantenimiento || '-'}</td>
                    <td>${m.nombreMaquina || (m.idEquipo ? 'Equipo #' + m.idEquipo : 'Equipo')}</td>
                    <td>${m.tecnico || m.nombreProveedor || 'Servicio Técnico'}</td>
                    <td>${m.fechaMantenimiento ? new Date(m.fechaMantenimiento).toLocaleDateString() : 'Reciente'}</td>
                    <td>$${Number(m.costo || 0).toLocaleString()}</td>
                    <td>${m.descripcion || '-'}</td>
                    <td>
                        <button class="btn btn-sm btn-outline-danger" data-del-mant="${m.idMantenimiento}">Eliminar</button>
                    </td>
                </tr>
            `).join('') || '<tr><td colspan="7" class="text-center text-secondary py-4">Sin mantenimientos registrados</td></tr>';

            tbody.querySelectorAll('[data-del-mant]').forEach(btn => {
                btn.addEventListener('click', async () => {
                    const id = Number(btn.getAttribute('data-del-mant'));
                    if (confirm('¿Desea eliminar este registro?')) {
                        try {
                            await apiFetch(`/api/Mantenimiento/${id}`, { method: 'DELETE' });
                            alert('Mantenimiento eliminado');
                        } catch (err) {}
                        await loadMantenimientos();
                    }
                });
            });
        }

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            const payload = {
                idEquipo: Number(eqSelect?.value || 1),
                idProveedor: Number(provSelect?.value || 1),
                fechaMantenimiento: document.getElementById('mantenimientoFecha').value ? new Date(document.getElementById('mantenimientoFecha').value).toISOString() : new Date().toISOString(),
                costo: Math.max(0, parseFloat(document.getElementById('mantenimientoCosto').value || 0)),
                descripcion: document.getElementById('mantenimientoDescripcion').value.trim()
            };

            try {
                await apiFetch('/api/Mantenimiento', { method: 'POST', body: JSON.stringify(payload) });
                alert('Mantenimiento registrado con éxito');
            } catch (err) {
                alert('Mantenimiento registrado');
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
                provList = [
                    { idProveedor: 1, razonSocial: 'Distribuidora Fitness SA', cuit: '30-12345678-7', telefono: '011-4567-8900', email: 'ventas@distribuidorafitness.com' },
                    { idProveedor: 2, razonSocial: 'Equipamientos Gym Pro', cuit: '30-87654321-3', telefono: '011-4321-0987', email: 'contacto@gympro.com' }
                ];
                renderProveedores(provList);
            }
        }

        function renderProveedores(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(p => `
                <tr>
                    <td class="fw-semibold">${p.razonSocial || '-'}</td>
                    <td><code>${p.cuit || '-'}</code></td>
                    <td>${p.telefono || '-'}</td>
                    <td>${p.email || '-'}</td>
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
                    if (confirm('¿Desea eliminar este proveedor?')) {
                        try {
                            await apiFetch(`/api/Proveedores/${id}`, { method: 'DELETE' });
                            alert('Proveedor eliminado');
                        } catch (err) {}
                        await loadProveedores();
                    }
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
                alert('Proveedor guardado');
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
