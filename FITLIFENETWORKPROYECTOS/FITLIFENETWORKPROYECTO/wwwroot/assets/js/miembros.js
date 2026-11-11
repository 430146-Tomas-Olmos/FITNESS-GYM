(function () {
  const FALLBACK_KEY = 'fitlife_demo_data';

  function ensureAuth() {
    const token = localStorage.getItem('fitlife_token');
    if (!token) {
      window.location.href = '../../index.html';
      return null;
    }
    return token;
  }

  function getDemoStore() {
    try {
      const raw = localStorage.getItem(FALLBACK_KEY);
      return raw ? JSON.parse(raw) : {};
    } catch (error) {
      return {};
    }
  }

  function setDemoStore(key, value) {
    const store = getDemoStore();
    store[key] = value;
    localStorage.setItem(FALLBACK_KEY, JSON.stringify(store));
  }

  function getSocioLabel(socio) {
    if (!socio) return 'Sin socio';
    const nombre = [socio.nombre, socio.apellido].filter(Boolean).join(' ').trim();
    const dni = socio.dni ? ` - DNI ${socio.dni}` : '';
    if (nombre) return `${nombre}${dni}`;
    if (socio.idSocio) return `Socio ${socio.idSocio}${dni}`;
    return 'Sin socio';
  }

  function normalizeArrayPayload(payload) {
    if (Array.isArray(payload)) return payload;
    if (!payload || typeof payload !== 'object') return [];
    const candidates = [payload.items, payload.data, payload.result, payload.results, payload.records, payload.value];
    for (const candidate of candidates) {
      if (Array.isArray(candidate)) return candidate;
    }
    return [];
  }

  function getFallbackData(url) {
    const normalized = url.split('?')[0];
    const demo = {
      '/api/Socios': [
        { idSocio: 1, nombre: 'Ana', apellido: 'García', dni: '30111222', email: 'ana@fitlife.com', telefono: '1122334455', estadoCuenta: 'ACTIVO', nombreSede: 'Sede Centro', plan: 'Premium', idSede: 1 },
        { idSocio: 2, nombre: 'Lucas', apellido: 'Pérez', dni: '32123456', email: 'lucas@fitlife.com', telefono: '1144556677', estadoCuenta: 'ACTIVO', nombreSede: 'Sede Norte', plan: 'Gold', idSede: 2 },
        { idSocio: 3, nombre: 'María', apellido: 'López', dni: '29123456', email: 'maria@fitlife.com', telefono: '1155667788', estadoCuenta: 'SUSPENDIDO', nombreSede: 'Sede Centro', plan: 'Classic', idSede: 1 }
      ],
      '/api/Contratos': [
        { idContrato: 1, idSocio: 1, socioNombre: 'Ana García', idPlan: 1, nombrePlan: 'Premium', idSede: 1, nombreSede: 'Sede Centro', fechaInicio: '2025-01-10', fechaVencimiento: '2026-01-10', estadoContrato: 'ACTIVO' },
        { idContrato: 2, idSocio: 2, socioNombre: 'Lucas Pérez', idPlan: 2, nombrePlan: 'Gold', idSede: 2, nombreSede: 'Sede Norte', fechaInicio: '2025-04-12', fechaVencimiento: '2026-04-12', estadoContrato: 'ACTIVO' }
      ],
      '/api/Cuotas/pendientes': [
        { idCobro: 1, idSocio: 1, socioNombre: 'Ana García', numeroCuota: 'CUOTA-001', monto: 8500, importeAbonado: 0, saldoPendiente: 8500, fechaVencimiento: '2026-11-15', estado: 'PENDIENTE' },
        { idCobro: 2, idSocio: 2, socioNombre: 'Lucas Pérez', numeroCuota: 'CUOTA-017', monto: 6400, importeAbonado: 3200, saldoPendiente: 3200, fechaVencimiento: '2026-11-20', estado: 'PARCIAL' }
      ],
      '/api/Accesos/log': [
        { idAcceso: 1, idSocio: 1, socioNombre: 'Ana Garc�a', sede: 'Sede Centro', fechaHoraMarcado: '2026-11-10T09:15:00', accesoPermitido: true, resultado: 'PERMITIDO', motivo: 'Membres�a vigente' },
        { idAcceso: 2, idSocio: 2, socioNombre: 'Lucas P�rez', sede: 'Sede Norte', fechaHoraMarcado: '2026-11-09T18:40:00', accesoPermitido: false, resultado: 'RECHAZADO', motivo: 'Apto m�dico vencido' }
      ],
      '/api/Reservas/clases': [
        { idClase: 1, disciplina: 'CrossFit', instructor: 'Jorge', fechaClase: '2026-11-10', horaInicio: '18:00', cupoLimite: 12, cuposUsados: 10 },
        { idClase: 2, disciplina: 'Yoga', instructor: 'Cecilia', fechaClase: '2026-11-11', horaInicio: '19:00', cupoLimite: 8, cuposUsados: 6 }
      ],
      '/api/Convenios': [
        { idConvenio: 1, razonSocial: 'ACME S.A.', cuit: '30-12345678-9', porcentajeDescuento: 15, diaCierreFacturacion: 10, email: 'rrhh@acme.com' },
        { idConvenio: 2, razonSocial: 'Grupo Sol', cuit: '30-98765432-1', porcentajeDescuento: 10, diaCierreFacturacion: 15, email: 'contabilidad@gruposol.com' }
      ],
      '/api/AptosMedicos': [
        { idApto: 1, idSocio: 1, medicoFirmante: 'Dr. Ram????rez', fechaEmision: '2026-01-15', fechaVencimiento: '2026-12-31', estado: 'VIGENTE' },
        { idApto: 2, idSocio: 2, medicoFirmante: 'Dr. Torres', fechaEmision: '2026-02-10', fechaVencimiento: '2026-11-05', estado: 'VENCIDO' }
      ],
      '/api/Sedes': [
        { idSede: 1, nombreSede: 'Sede Centro' },
        { idSede: 2, nombreSede: 'Sede Norte' }
      ],
      '/api/Planes': [
        { idPlan: 1, nombrePlan: 'Premium' },
        { idPlan: 2, nombrePlan: 'Gold' },
        { idPlan: 3, nombrePlan: 'Classic' }
      ],
      '/api/Geografia/calles': [
        { idCalle: 1, nombreCalle: 'Av. Corrientes 1234' },
        { idCalle: 2, nombreCalle: 'Calle Florida 245' }
      ],
      '/api/FormasPago': [
        { idFormaPago: 1, nombreFormaPago: 'Efectivo' },
        { idFormaPago: 2, nombreFormaPago: 'Tarjeta' },
        { idFormaPago: 3, nombreFormaPago: 'Transferencia' }
      ]
    };

    const store = getDemoStore();
    const saved = store[normalized];
    if (Array.isArray(saved) && saved.length) {
      return saved;
    }

    if (normalized.startsWith('/api/Accesos/historial/')) {
      const idSocio = Number(normalized.split('/').pop() || 0);
      return {
        contratos: idSocio ? [{ idContrato: 1, idSocio, nombrePlan: 'Premium', fechaInicio: '2025-01-10', estadoContrato: 'ACTIVO' }] : [],
        accesos: [
          { idAcceso: 1, accesoPermitido: true, fechaHoraMarcado: '2026-11-10T09:15:00' },
          { idAcceso: 2, accesoPermitido: false, fechaHoraMarcado: '2026-11-09T18:40:00' }
        ],
        reservas: idSocio ? [{ idReserva: 1, idSocio, fechaReserva: '2026-11-08', estadoReserva: 'CONFIRMADA' }] : [],
        pagos: idSocio ? [{ idPago: 1, idSocio, fechaPago: '2026-11-01', importe: 8500 }] : []
      };
    }

    const fallback = demo[normalized];
    if (fallback) {
      setDemoStore(normalized, fallback);
      return fallback;
    }

    return null;
  }

  async function apiFetch(url, options = {}) {
    const token = ensureAuth();
    if (!token) return null;
    const { headers = {}, ...rest } = options;
    try {
      const response = await fetch(url, {
        ...rest,
        headers: {
          'Content-Type': 'application/json',
          Accept: 'application/json',
          ...headers,
          Authorization: `Bearer ${token}`
        }
      });
      if (response.status === 401 || response.status === 403) {
        localStorage.removeItem('fitlife_token');
        localStorage.removeItem('fitlife_user');
        window.location.href = '../../index.html';
        return null;
      }
      if (response.status === 204) return null;
      const text = await response.text();
      if (!text) return null;
      try {
        return JSON.parse(text);
      } catch (error) {
        return text;
      }
    } catch (error) {
      return getFallbackData(url);
    }
  }
  function fillSelect(selectEl, options, valueKey, labelKey, placeholder, customLabel) {
    if (!selectEl) return;
    const items = normalizeArrayPayload(options);
    selectEl.innerHTML = '<option value="">' + placeholder + '</option>';
    for (const option of items) {
      const value = option?.[valueKey];
      const computedLabel = customLabel ? customLabel(option) : (labelKey ? option?.[labelKey] : '') || option?.nombre || option?.razonSocial || option?.nombreSede || option?.nombrePlan || option?.nombreFormaPago || option?.nombreCalle || option?.nombreDisciplina || option?.disciplina || option?.nombreCategoria || option?.nombreProducto || option?.descripcion || value;
      const label = computedLabel ?? value ?? '';
      const opt = document.createElement('option');
      opt.value = value ?? '';
      opt.textContent = label;
      selectEl.appendChild(opt);
    }
  }
function formatDate(value) {
    if (!value) return '-';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return value;
    return date.toLocaleDateString('es-AR', { day: '2-digit', month: '2-digit', year: 'numeric' });
  }

  function formatCurrency(value) {
    const numeric = Number(value ?? 0);
    return new Intl.NumberFormat('es-AR', {
      style: 'currency',
      currency: 'ARS',
      maximumFractionDigits: 2
    }).format(numeric);
  }

  function badgeClassByStatus(status) {
    const normalized = String(status || '').toUpperCase();
    if (normalized.includes('ACTIVO') || normalized.includes('VIGENTE') || normalized.includes('OK') || normalized.includes('CONFIRMADA')) return 'success';
    if (normalized.includes('PENDIENTE') || normalized.includes('EN PROCESO') || normalized.includes('PARCIAL')) return 'warning';
    if (normalized.includes('INACTIVO') || normalized.includes('CERRADO') || normalized.includes('RECHAZADO') || normalized.includes('VENCIDO') || normalized.includes('SUSPENDIDO')) return 'danger';
    return 'secondary';
  }

  function showMessage(message, type = 'info') {
    const resolved = type === 'error' ? 'Error' : type === 'success' ? 'Éxito' : 'Información';
    window.alert(`${resolved}: ${message}`);
  }

  async function loadSociosPage() {
    const tbody = document.getElementById('sociosTableBody');
    const filterInput = document.getElementById('searchSocio');
    const filterState = document.getElementById('estadoSocio');
    const filterSede = document.getElementById('sedeSocio');
    const form = document.getElementById('socioForm');
    const modalEl = document.getElementById('modalSocio');
    if (!tbody) return;

    async function loadCatalogs() {
      const [calles, sedes, convenios, planes] = await Promise.all([
        apiFetch('/api/Geografia/calles'),
        apiFetch('/api/Sedes'),
        apiFetch('/api/Convenios'),
        apiFetch('/api/Planes')
      ]);
      fillSelect(document.getElementById('socioIdCalle'), Array.isArray(calles) ? calles : [], 'idCalle', 'nombreCalle', 'Seleccione calle');
      fillSelect(document.getElementById('socioIdSede'), Array.isArray(sedes) ? sedes : [], 'idSede', 'nombreSede', 'Seleccione sede');
      fillSelect(document.getElementById('socioPlan'), Array.isArray(planes) ? planes : [], 'idPlan', 'nombrePlan', 'Seleccione plan');
      fillSelect(filterSede, Array.isArray(sedes) ? sedes : [], 'idSede', 'nombreSede', 'Todas');
      fillSelect(document.getElementById('socioIdConvenio'), Array.isArray(convenios) ? convenios : [], 'idConvenio', 'razonSocial', 'Sin convenio');
    }

    function normalizeRow(socio) {
      const nombreCompleto = [socio.nombre, socio.apellido].filter(Boolean).join(' ');
      return { ...socio, nombreCompleto, sedeLabel: socio.nombreSede || socio.sede || 'Sin sede', estadoCuenta: socio.estadoCuenta || 'ACTIVO' };
    }

    async function loadSocios() {
      const data = await apiFetch('/api/Socios');
      const items = Array.isArray(data) ? data : [];
      const filtro = (filterInput?.value || '').trim().toLowerCase();
      const estado = (filterState?.value || '').toUpperCase();
      const sedeSeleccionada = String(filterSede?.value || '');

      const filtered = items.map(normalizeRow).filter((socio) => {
        const matchTexto = !filtro || [socio.nombreCompleto, socio.dni, socio.email, socio.telefono].join(' ').toLowerCase().includes(filtro);
        const matchEstado = !estado || estado === 'TODOS' || (socio.estadoCuenta || '').toUpperCase() === estado;
        const matchSede = !sedeSeleccionada || sedeSeleccionada === 'TODOS' || String(socio.idSede || '').trim() === sedeSeleccionada || socio.sedeLabel === sedeSeleccionada;
        return matchTexto && matchEstado && matchSede;
      });

      tbody.innerHTML = filtered.map((socio) => `
        <tr data-id="${socio.idSocio}">
          <td>
            <div class="d-flex align-items-center gap-3">
              <div class="rounded-circle bg-primary-subtle text-primary d-flex align-items-center justify-content-center" style="width: 36px; height: 36px;"><i class="bi bi-person"></i></div>
              <div>
                <div class="fw-semibold">${socio.nombreCompleto}</div>
                <small class="text-secondary">${socio.email || 'Sin email'}</small>
              </div>
            </div>
          </td>
          <td>${socio.dni || '-'}</td>
          <td>${socio.sedeLabel}</td>
          <td><span class="badge text-bg-${badgeClassByStatus(socio.estadoCuenta)}">${socio.estadoCuenta}</span></td>
          <td>${socio.plan || 'Sin plan'}</td>
          <td>${formatDate(socio.fechaIngresoAlta || new Date().toISOString())}</td>
          <td>
            <button class="btn btn-sm btn-outline-primary me-2" data-action="edit" data-id="${socio.idSocio}">Editar</button>
            <button class="btn btn-sm btn-outline-danger" data-action="delete" data-id="${socio.idSocio}">Baja</button>
          </td>
        </tr>
      `).join('') || '<tr><td colspan="7" class="text-center text-secondary py-4">Sin resultados</td></tr>';

      tbody.querySelectorAll('[data-action="edit"]').forEach((button) => {
        button.addEventListener('click', () => {
          const id = Number(button.dataset.id);
          const item = items.find((socio) => socio.idSocio === id);
          if (!item) return;
          document.getElementById('socioId').value = item.idSocio || '';
          document.getElementById('socioNombre').value = item.nombre || '';
          document.getElementById('socioApellido').value = item.apellido || '';
          document.getElementById('socioDni').value = item.dni || '';
          document.getElementById('socioTelefono').value = item.telefono || '';
          document.getElementById('socioEmail').value = item.email || '';
          document.getElementById('socioIdSede').value = item.idSede || '';
          document.getElementById('socioPlan').value = item.plan || '';
          document.getElementById('socioEstadoCuenta').value = item.estadoCuenta || 'ACTIVO';
          bootstrap.Modal.getOrCreateInstance(modalEl).show();
        });
      });

      tbody.querySelectorAll('[data-action="delete"]').forEach((button) => {
        button.addEventListener('click', async () => {
          const id = Number(button.dataset.id);
          const motivo = window.prompt('Ingrese el motivo de baja:', 'Cambio de plan / baja por decisión del cliente');
          if (motivo === null) return;
          if (!window.confirm('¿Desea dar de baja a este socio?')) return;
          const response = await apiFetch(`/api/Socios/${id}`, { method: 'DELETE' });
          if (response === null) {
            showMessage('No se pudo completar la baja.', 'error');
            return;
          }
          showMessage(`Socio dado de baja correctamente. Motivo: ${motivo}`, 'success');
          await loadSocios();
        });
      });
    }

    form?.addEventListener('submit', async (event) => {
      event.preventDefault();
      const id = Number(document.getElementById('socioId').value || 0);
      const payload = {
        dni: document.getElementById('socioDni').value.trim(),
        nombre: document.getElementById('socioNombre').value.trim(),
        apellido: document.getElementById('socioApellido').value.trim(),
        telefono: document.getElementById('socioTelefono').value.trim(),
        email: document.getElementById('socioEmail').value.trim(),
        fechaNacimiento: document.getElementById('socioFechaNacimiento')?.value || '2000-01-01',
        idCalle: Number(document.getElementById('socioIdCalle').value || 1),
        idSede: Number(document.getElementById('socioIdSede').value || 0),
        idConvenio: Number(document.getElementById('socioIdConvenio').value || 0),
        plan: document.getElementById('socioPlan').value || '',
        estadoCuenta: document.getElementById('socioEstadoCuenta').value || 'ACTIVO',
        observacion: document.getElementById('socioObservacion')?.value || ''
      };

      if (!payload.dni || !payload.nombre || !payload.apellido) {
        showMessage('Complete nombre, apellido y DNI.', 'error');
        return;
      }

      const result = id
        ? await apiFetch(`/api/Socios/${id}`, { method: 'PUT', body: JSON.stringify(payload) })
        : await apiFetch('/api/Socios', { method: 'POST', body: JSON.stringify(payload) });

      if (!result) {
        showMessage('No se pudo guardar el socio.', 'error');
        return;
      }

      showMessage(id ? 'Socio actualizado correctamente.' : 'Socio creado correctamente.', 'success');
      form.reset();
      document.getElementById('socioId').value = '';
      bootstrap.Modal.getInstance(modalEl)?.hide();
      await loadSocios();
    });

    document.getElementById('newSocioBtn')?.addEventListener('click', () => {
      form?.reset();
      document.getElementById('socioId').value = '';
      bootstrap.Modal.getOrCreateInstance(modalEl).show();
    });

    filterInput?.addEventListener('input', loadSocios);
    filterState?.addEventListener('change', loadSocios);
    filterSede?.addEventListener('change', loadSocios);

    await loadCatalogs();
    await loadSocios();
  }

  async function loadContratosPage() {
    const tbody = document.getElementById('contratosTableBody');
    const form = document.getElementById('contratoForm');
    const modalEl = document.getElementById('modalContrato');
    if (!tbody) return;

    async function loadCatalogs() {
      const [socios, planes, sedes] = await Promise.all([
        apiFetch('/api/Socios'),
        apiFetch('/api/Planes'),
        apiFetch('/api/Sedes')
      ]);
      fillSelect(document.getElementById('contratoIdSocio'), Array.isArray(socios) ? socios : [], 'idSocio', 'nombre', 'Seleccione socio');
      fillSelect(document.getElementById('contratoIdPlan'), Array.isArray(planes) ? planes : [], 'idPlan', 'nombrePlan', 'Seleccione plan');
      fillSelect(document.getElementById('contratoIdSede'), Array.isArray(sedes) ? sedes : [], 'idSede', 'nombreSede', 'Seleccione sede');
    }

    async function loadContratos() {
      const contratos = await apiFetch('/api/Contratos');
      const items = Array.isArray(contratos) ? contratos : [];
      tbody.innerHTML = items.map((contrato) => `
        <tr>
          <td>${contrato.socioNombre || contrato.idSocio || '-'}</td>
          <td>${contrato.nombrePlan || contrato.idPlan || '-'}</td>
          <td>${contrato.nombreSede || contrato.idSede || '-'}</td>
          <td>${formatDate(contrato.fechaInicio)}</td>
          <td>${formatDate(contrato.fechaVencimiento)}</td>
          <td><span class="badge text-bg-${badgeClassByStatus(contrato.estadoContrato)}">${contrato.estadoContrato || 'ACTIVO'}</span></td>
        </tr>
      `).join('') || '<tr><td colspan="6" class="text-center text-secondary py-4">Sin contratos</td></tr>';
    }

    form?.addEventListener('submit', async (event) => {
      event.preventDefault();
      const payload = {
        idSocio: Number(document.getElementById('contratoIdSocio').value || 0),
        idPlan: Number(document.getElementById('contratoIdPlan').value || 0),
        idSede: Number(document.getElementById('contratoIdSede').value || 0),
        fechaInicio: document.getElementById('contratoFechaInicio').value,
        fechaVencimiento: document.getElementById('contratoFechaFin').value,
        estadoContrato: document.getElementById('contratoEstado').value || 'ACTIVO'
      };

      if (!payload.idSocio || !payload.idPlan || !payload.idSede || !payload.fechaInicio || !payload.fechaVencimiento) {
        showMessage('Complete todos los datos del contrato.', 'error');
        return;
      }

      const result = await apiFetch('/api/Contratos', { method: 'POST', body: JSON.stringify(payload) });
      if (!result) {
        showMessage('No se pudo guardar el contrato.', 'error');
        return;
      }
      showMessage('Contrato registrado correctamente.', 'success');
      form.reset();
      bootstrap.Modal.getInstance(modalEl)?.hide();
      await loadContratos();
    });

    document.getElementById('newContratoBtn')?.addEventListener('click', () => {
      form?.reset();
      bootstrap.Modal.getOrCreateInstance(modalEl).show();
    });

    await loadCatalogs();
    await loadContratos();
  }

  async function loadCuotasPage() {
    const tbody = document.getElementById('cuotasTableBody');
    const form = document.getElementById('cobroForm');
    const socioSelect = document.getElementById('cobroSocio');
    const cuotaSelect = document.getElementById('cobroIdCobro');
    if (!tbody) return;

    const [formasPago, cuotas, socios] = await Promise.all([
      apiFetch('/api/FormasPago'),
      apiFetch('/api/Cuotas/pendientes'),
      apiFetch('/api/Socios')
    ]);

    const cuotasList = Array.isArray(cuotas) ? cuotas : [];
    const sociosList = Array.isArray(socios) ? socios : [];

    fillSelect(document.getElementById('cobroFormaPago'), Array.isArray(formasPago) ? formasPago : [], 'idFormaPago', 'nombreFormaPago', 'Seleccione forma de pago');
    fillSelect(socioSelect, sociosList, 'idSocio', 'nombre', 'Seleccione socio', (socio) => getSocioLabel(socio));

    function updateCuotasBySocio() {
      const idSocio = Number(socioSelect?.value || 0);
      const filtered = idSocio ? cuotasList.filter((cuota) => Number(cuota.idSocio) === idSocio) : cuotasList;
      if (cuotaSelect) {
        fillSelect(cuotaSelect, filtered.map((cuota) => ({
          ...cuota,
          label: `${cuota.numeroCuota || 'Cuota'} - ${formatCurrency(cuota.saldoPendiente || cuota.monto || 0)}`
        })), 'idCobro', 'label', 'Seleccione cuota');
      }
    }

    socioSelect?.addEventListener('change', updateCuotasBySocio);
    updateCuotasBySocio();

    tbody.innerHTML = cuotasList.map((cuota) => `
      <tr>
        <td>${cuota.socioNombre || cuota.idSocio || '-'}</td>
        <td>${cuota.numeroCuota || '-'}</td>
        <td>${formatDate(cuota.fechaVencimiento)}</td>
        <td>${formatCurrency(cuota.monto)}</td>
        <td>${formatCurrency(cuota.importeAbonado || 0)}</td>
        <td>${formatCurrency(cuota.saldoPendiente || cuota.monto || 0)}</td>
        <td><span class="badge text-bg-${badgeClassByStatus(cuota.estado)}">${cuota.estado || 'PENDIENTE'}</span></td>
      </tr>
    `).join('') || '<tr><td colspan="7" class="text-center text-secondary py-4">Sin cuotas pendientes</td></tr>';

    form?.addEventListener('submit', async (event) => {
      event.preventDefault();
      const idCobro = Number(cuotaSelect?.value || 0);
      const idSocio = Number(socioSelect?.value || 0);
      const monto = Number(document.getElementById('cobroImporte')?.value || 0);
      const idFormaPago = Number(document.getElementById('cobroFormaPago')?.value || 0);
      const payload = { idCobro, idSocio, monto, idFormaPago };
      if (!payload.idCobro || !payload.idSocio || !payload.monto || !payload.idFormaPago) {
        showMessage('Complete cuota, socio, monto y forma de pago.', 'error');
        return;
      }
      const result = await apiFetch('/api/Cuotas/registrar-pago', { method: 'POST', body: JSON.stringify(payload) });
      if (!result) {
        showMessage('No se pudo registrar el pago.', 'error');
        return;
      }
      showMessage('Pago registrado correctamente.', 'success');
      form.reset();
      bootstrap.Modal.getInstance(document.getElementById('modalCobro'))?.hide();
      window.location.reload();
    });
  }
  async function loadAccesosPage() {
    const tbody = document.getElementById('accesosTableBody');
    const form = document.getElementById('validarAccesoForm');
    const selectorSocio = document.getElementById('accesoIdSocio');
    const selectorSede = document.getElementById('accesoIdSede');
    const resultado = document.getElementById('accesoResultado');
    if (!tbody) return;

    let sociosDisponibles = [];

    function hasActiveMembershipForSede(socio, idSede, sedeNombre, contratos) {
      const contratoActivo = contratos.find((contrato) => {
        const mismoSocio = Number(contrato.idSocio) === Number(socio.idSocio);
        const mismoSede = Number(contrato.idSede) === idSede || contrato.nombreSede === sedeNombre;
        const estado = String(contrato.estadoContrato || '').toUpperCase();
        return mismoSocio && mismoSede && (estado === 'ACTIVO' || estado === 'VIGENTE' || estado === 'ALTA');
      });

      return Boolean(contratoActivo || (Number(socio.idSede) === idSede && String(socio.estadoCuenta || '').toUpperCase() === 'ACTIVO'));
    }

    async function populateSelectors() {
      const [socios, sedes, contratos] = await Promise.all([
        apiFetch('/api/Socios'),
        apiFetch('/api/Sedes'),
        apiFetch('/api/Contratos')
      ]);
      sociosDisponibles = Array.isArray(socios) ? socios : [];
      const contratosDisponibles = Array.isArray(contratos) ? contratos : [];
      fillSelect(selectorSede, Array.isArray(sedes) ? sedes : [], 'idSede', 'nombreSede', 'Seleccione sede');
      selectorSocio.innerHTML = '<option value="">Seleccione un socio</option>';
      selectorSocio.disabled = true;
      selectorSede.onchange = () => {
        const idSede = Number(selectorSede.value || 0);
        const sedeNombre = (Array.isArray(sedes) ? sedes : []).find((sede) => Number(sede.idSede) === idSede)?.nombreSede;
        const activos = sociosDisponibles.filter((socio) => {
          const estado = String(socio.estadoCuenta || '').toUpperCase();
          const mismoSede = Number(socio.idSede) === idSede || socio.nombreSede === sedeNombre;
          const membresiaVigente = hasActiveMembershipForSede(socio, idSede, sedeNombre, contratosDisponibles);
          return mismoSede && membresiaVigente && (estado === 'ACTIVO' || estado === 'VIGENTE' || !estado);
        });
        fillSelect(selectorSocio, activos, 'idSocio', 'nombre', 'Seleccione socio', (socio) => getSocioLabel(socio));
        selectorSocio.disabled = activos.length === 0;
        if (!activos.length) {
          showMessage('No hay socios activos con membresía vigente para esa sede.', 'error');
        }
      };
    }

    async function renderAccesos() {
      const data = await apiFetch('/api/Accesos/log');
      const logs = normalizeArrayPayload(data);
      tbody.innerHTML = logs.map((acceso) => {
        const estado = acceso.resultado || (acceso.accesoPermitido === false ? 'RECHAZADO' : 'PERMITIDO');
        return `
          <tr>
            <td>${acceso.socioNombre || acceso.nombreSocio || acceso.idSocio || '-'}</td>
            <td>${acceso.sede || acceso.nombreSede || '-'}</td>
            <td>${formatDate(acceso.fechaHora || acceso.fecha || acceso.fechaHoraMarcado || '-')}</td>
            <td><span class="badge text-bg-${badgeClassByStatus(estado)}">${estado}</span></td>
            <td>${acceso.motivo || acceso.motivoRechazo || '-'}</td>
          </tr>
        `;
      }).join('') || '<tr><td colspan="5" class="text-center text-secondary py-4">Sin accesos registrados</td></tr>';
    }

    form?.addEventListener('submit', async (event) => {
      event.preventDefault();
      const idSocio = Number(selectorSocio?.value || 0);
      const idSede = Number(selectorSede?.value || 0);
      if (!idSocio || !idSede) {
        showMessage('Seleccione primero una sede y luego un socio activo.', 'error');
        return;
      }

      const payload = {
        idSocio,
        idSede,
        fechaHoraMarcado: new Date().toISOString()
      };

      const result = await apiFetch('/api/Accesos/validar', { method: 'POST', body: JSON.stringify(payload) });
      const permitido = result?.permitido === true || result?.accesoPermitido === true || result?.resultado === 'PERMITIDO';
      const texto = result?.motivo || result?.motivoRechazo || (permitido ? 'Acceso autorizado' : 'Acceso denegado');
      const estado = permitido ? 'PERMITIDO' : 'RECHAZADO';
      if (resultado) {
        resultado.textContent = `${estado}: ${texto}`;
        resultado.className = `fw-semibold ${permitido ? 'text-success' : 'text-danger'}`;
      }
      showMessage(texto, permitido ? 'success' : 'error');
      await renderAccesos();
    });

    document.getElementById('filtrarAccesosBtn')?.addEventListener('click', async () => {
      await renderAccesos();
    });

    await populateSelectors();
    await renderAccesos();
  }
  async function loadReservasPage() {
    const tbody = document.getElementById('clasesTableBody');
    const waitlistList = document.getElementById('waitlistList');
    const form = document.getElementById('reservaForm');
    const selectorSocio = document.getElementById('reservaIdSocio');
    const selectorClase = document.getElementById('reservaIdClase');
    if (!tbody) return;

    const disciplinaMap = { 1: 'CrossFit', 2: 'Yoga', 3: 'Pilates', 4: 'Spinning', 5: 'Funcional', 6: 'Boxeo', 7: 'BodyCombat', 8: 'Zumba' };
    const instructorMap = { 1: 'Jorge Ramírez', 2: 'Cecilia Ruiz', 3: 'Nicolás Vega', 4: 'Laura Díaz', 5: 'Martín Silva' };

    function resolveDisciplinaName(disciplina) {
      if (disciplina === null || disciplina === undefined || disciplina === '') return 'Disciplina';
      if (typeof disciplina === 'string') {
        const normalized = disciplina.trim();
        if (!normalized) return 'Disciplina';
        const numeric = Number(normalized);
        if (!Number.isNaN(numeric) && disciplinaMap[numeric]) return disciplinaMap[numeric];
        return normalized;
      }
      if (typeof disciplina === 'number') {
        return disciplinaMap[disciplina] || `Disciplina ${disciplina}`;
      }
      return disciplinaMap[Number(disciplina) || 0] || 'Disciplina';
    }

    function resolveInstructorName(idInstructor) {
      if (!idInstructor) return 'Sin instructor';
      if (typeof idInstructor === 'string' && !/^\d+$/.test(idInstructor.trim())) return idInstructor;
      const id = Number(idInstructor || 0);
      return instructorMap[id] || `Instructor ${id || 'N/A'}`;
    }

    function getReservationStore() {
      try {
        const raw = localStorage.getItem('fitlife_reservas_demo');
        return raw ? JSON.parse(raw) : [];
      } catch (error) {
        return [];
      }
    }

    function saveReservationStore(items) {
      localStorage.setItem('fitlife_reservas_demo', JSON.stringify(items));
    }

    function renderWaitlist() {
      if (!waitlistList) return;
      const registros = getReservationStore().filter((reserva) => String(reserva.estadoReserva || '').toUpperCase() === 'LISTA_ESPERA');
      if (!registros.length) {
        waitlistList.innerHTML = '<li class="list-group-item text-secondary">Sin socios en lista de espera</li>';
        return;
      }

      waitlistList.innerHTML = registros.map((reserva) => `
        <li class="list-group-item d-flex justify-content-between align-items-center gap-3">
          <div>
            <div class="fw-semibold">${reserva.socioNombre || `Socio ${reserva.idSocio}`}</div>
            <small class="text-secondary">${resolveDisciplinaName(reserva.disciplina || reserva.nombreDisciplina || reserva.idClase)}</small>
          </div>
          <button class="btn btn-sm btn-outline-danger" data-remove-waitlist="${reserva.idReserva}">Quitar</button>
        </li>
      `).join('');

      waitlistList.querySelectorAll('[data-remove-waitlist]').forEach((button) => {
        button.addEventListener('click', () => {
          const idReserva = Number(button.getAttribute('data-remove-waitlist'));
          const reservas = getReservationStore().filter((reserva) => Number(reserva.idReserva) !== idReserva);
          saveReservationStore(reservas);
          renderWaitlist();
          renderClases();
        });
      });
    }

    async function populateSelectors() {
      const [socios, clases] = await Promise.all([
        apiFetch('/api/Socios'),
        apiFetch('/api/Reservas/clases')
      ]);
      fillSelect(selectorSocio, Array.isArray(socios) ? socios : [], 'idSocio', 'nombre', 'Seleccione socio', (socio) => getSocioLabel(socio));
      fillSelect(selectorClase, Array.isArray(clases) ? clases : [], 'idClase', 'idClase', 'Seleccione clase', (clase) => {
        const disciplina = resolveDisciplinaName(clase?.disciplina || clase?.nombreDisciplina || clase?.idDisciplina);
        const fecha = clase?.fechaClase || '';
        const hora = clase?.horaInicio || '';
        const instructor = resolveInstructorName(clase?.idInstructor || clase?.instructor);
        return [disciplina, instructor, fecha, hora].filter(Boolean).join(' - ');
      });
    }

    async function renderClases() {
      const data = await apiFetch('/api/Reservas/clases');
      const clases = normalizeArrayPayload(data);
      const reservas = getReservationStore();

      tbody.innerHTML = clases.map((clase) => {
        const idClase = Number(clase.idClase || 0);
        const reservasClase = reservas.filter((reserva) => Number(reserva.idClase) === idClase);
        const reservasConfirmadas = reservasClase.filter((reserva) => String(reserva.estadoReserva || '').toUpperCase() !== 'LISTA_ESPERA');
        const cuposUsados = Math.max(Number(clase.cuposUsados || 0), reservasConfirmadas.length);
        const cupoLimite = Number(clase.cupoLimite || 0);
        const ocupacion = cupoLimite ? Math.min(100, Math.round((cuposUsados / cupoLimite) * 100)) : 0;
        const disciplinaLabel = resolveDisciplinaName(clase.disciplina || clase.nombreDisciplina || clase.idDisciplina);
        const instructorLabel = resolveInstructorName(clase.idInstructor || clase.instructor);
        return `
          <tr>
            <td>${clase.nombreClase || disciplinaLabel}</td>
            <td>${disciplinaLabel}</td>
            <td>${instructorLabel}</td>
            <td>${clase.fechaClase || '-'} ${clase.horaInicio || ''}</td>
            <td>${cuposUsados}/${cupoLimite} · ${ocupacion}%</td>
            <td><span class="badge text-bg-${badgeClassByStatus(clase.estado || 'ACTIVA')}">${clase.estado || 'ACTIVA'}</span></td>
          </tr>
        `;
      }).join('') || '<tr><td colspan="6" class="text-center text-secondary py-4">Sin clases programadas</td></tr>';

      renderWaitlist();
    }

    form?.addEventListener('submit', async (event) => {
      event.preventDefault();
      const idSocio = Number(selectorSocio?.value || 0);
      const idClase = Number(selectorClase?.value || 0);
      const estadoReserva = document.getElementById('reservaEstado')?.value || 'CONFIRMADA';
      if (!idSocio || !idClase) {
        showMessage('Seleccione un socio y una clase.', 'error');
        return;
      }

      const payload = { idSocio, idClase, estadoReserva };
      const result = await apiFetch('/api/Reservas', { method: 'POST', body: JSON.stringify(payload) });
      if (!result) {
        showMessage('No se pudo guardar la reserva.', 'error');
        return;
      }

      const socios = await apiFetch('/api/Socios');
      const socio = normalizeArrayPayload(socios).find((item) => Number(item.idSocio) === idSocio) || {};
      const reservas = getReservationStore();
      reservas.push({
        idReserva: Date.now(),
        idSocio,
        idClase,
        estadoReserva,
        socioNombre: getSocioLabel(socio),
        disciplina: selectorClase.selectedOptions[0]?.textContent || ''
      });
      saveReservationStore(reservas);

      showMessage('Éxito: Reserva creada correctamente.', 'success');
      form.reset();
      bootstrap.Modal.getInstance(document.getElementById('modalReserva'))?.hide();
      await renderClases();
    });

    await populateSelectors();
    await renderClases();
  }
  async function loadConveniosPage() {
    const tbody = document.getElementById('conveniosTableBody');
    if (!tbody) return;
    const data = await apiFetch('/api/Convenios');
    tbody.innerHTML = (Array.isArray(data) ? data : []).map((convenio) => `
      <tr>
        <td>${convenio.razonSocial || '-'}</td>
        <td>${convenio.cuit || '-'}</td>
        <td>${convenio.porcentajeDescuento || 0}%</td>
        <td>${convenio.diaCierreFacturacion || '-'}</td>
        <td>${convenio.email || '-'}</td>
      </tr>
    `).join('') || '<tr><td colspan="5" class="text-center text-secondary py-4">Sin convenios</td></tr>';
  }

  async function loadAptosPage() {
    const tbody = document.getElementById('aptosTableBody');
    const form = document.getElementById('aptoForm');
    const selectorSocio = document.getElementById('aptoIdSocio');
    if (!tbody) return;

    async function populateSelectors() {
      const socios = await apiFetch('/api/Socios');
      fillSelect(selectorSocio, Array.isArray(socios) ? socios : [], 'idSocio', 'nombre', 'Seleccione socio', (socio) => getSocioLabel(socio));
    }

    async function renderAptos() {
      const data = await apiFetch('/api/AptosMedicos');
      const aptos = normalizeArrayPayload(data);
      const socios = await apiFetch('/api/Socios');
      const socioMap = new Map(normalizeArrayPayload(socios).map((socio) => [Number(socio.idSocio), socio]));

      tbody.innerHTML = aptos.map((apto) => {
        const socio = socioMap.get(Number(apto.idSocio));
        const nombreSocio = socio ? getSocioLabel(socio) : `Socio ${apto.idSocio ?? '-'}`;
        const estado = apto.fechaVencimiento && new Date(apto.fechaVencimiento) < new Date() ? 'VENCIDO' : 'VIGENTE';
        return `
          <tr>
            <td>${nombreSocio}</td>
            <td>${apto.medicoFirmante || '-'}</td>
            <td>${formatDate(apto.fechaEmision)}</td>
            <td>${formatDate(apto.fechaVencimiento)}</td>
            <td><span class="badge text-bg-${badgeClassByStatus(estado)}">${estado}</span></td>
          </tr>
        `;
      }).join('') || '<tr><td colspan="5" class="text-center text-secondary py-4">Sin aptos m??dicos</td></tr>';
    }

    form?.addEventListener('submit', async (event) => {
      event.preventDefault();
      const payload = {
        idSocio: Number(selectorSocio?.value || 0),
        fechaEmision: document.getElementById('aptoFechaEmision')?.value || new Date().toISOString().slice(0, 10),
        fechaVencimiento: document.getElementById('aptoFechaVencimiento')?.value || new Date(Date.now() + 86400000).toISOString().slice(0, 10),
        medicoFirmante: document.getElementById('aptoMedico')?.value.trim() || '',
        matriculaMedico: document.getElementById('aptoMatricula')?.value.trim() || '',
        observaciones: document.getElementById('aptoObservaciones')?.value.trim() || ''
      };

      if (!payload.idSocio || !payload.medicoFirmante || !payload.matriculaMedico || !payload.fechaEmision || !payload.fechaVencimiento) {
        showMessage('Complete todos los campos del apto m??dico.', 'error');
        return;
      }

      const result = await apiFetch('/api/AptosMedicos', { method: 'POST', body: JSON.stringify(payload) });
      if (!result) {
        showMessage('No se pudo guardar el apto m??dico.', 'error');
        return;
      }

      showMessage('Apto m??dico registrado correctamente.', 'success');
      form.reset();
      bootstrap.Modal.getInstance(document.getElementById('modalApto'))?.hide();
      await renderAptos();
    });

    await populateSelectors();
    await renderAptos();
  }
  async function loadHistorialPage() {
    const selector = document.getElementById('historialSocioSelect');
    const title = document.querySelector('h1');
    const contratosList = document.getElementById('historialContratos');
    const pagosList = document.getElementById('historialPagos');
    const accesosList = document.getElementById('historialAccesos');
    const reservasList = document.getElementById('historialReservas');
    if (!selector || !contratosList || !pagosList || !accesosList || !reservasList) return;

    async function renderHistorial(idSocio) {
      if (!idSocio) {
        contratosList.innerHTML = '<li class="list-group-item text-secondary">Sin contratos</li>';
        pagosList.innerHTML = '<li class="list-group-item text-secondary">Sin pagos</li>';
        accesosList.innerHTML = '<li class="list-group-item text-secondary">Sin accesos</li>';
        reservasList.innerHTML = '<li class="list-group-item text-secondary">Sin reservas</li>';
        return;
      }

      const data = await apiFetch(`/api/Accesos/historial/${idSocio}`);
      const historial = data && typeof data === 'object' && !Array.isArray(data) ? data : {};
      const contratos = Array.isArray(historial.contratos) ? historial.contratos : [];
      const pagos = Array.isArray(historial.pagos) ? historial.pagos : [];
      const accesos = Array.isArray(historial.accesos) ? historial.accesos : [];
      const reservas = Array.isArray(historial.reservas) ? historial.reservas : [];

      contratosList.innerHTML = contratos.length
        ? contratos.map((contrato) => `<li class="list-group-item">${contrato.nombrePlan || 'Plan'} - ${formatDate(contrato.fechaInicio)}</li>`).join('')
        : '<li class="list-group-item text-secondary">Sin contratos</li>';

      pagosList.innerHTML = pagos.length
        ? pagos.map((pago) => `<li class="list-group-item">$${Number(pago.importe || 0)} - ${formatDate(pago.fechaPago)}</li>`).join('')
        : '<li class="list-group-item text-secondary">Sin pagos</li>';

      accesosList.innerHTML = accesos.length
        ? accesos.map((acceso) => `<li class="list-group-item">${acceso.accesoPermitido ? 'Autorizado' : 'Rechazado'} - ${formatDate(acceso.fechaHoraMarcado)}</li>`).join('')
        : '<li class="list-group-item text-secondary">Sin accesos</li>';

      reservasList.innerHTML = reservas.length
        ? reservas.map((reserva) => `<li class="list-group-item">${reserva.estadoReserva || 'CONFIRMADA'} - ${formatDate(reserva.fechaReserva)}</li>`).join('')
        : '<li class="list-group-item text-secondary">Sin reservas</li>';
    }

    const socios = await apiFetch('/api/Socios');
    const lista = Array.isArray(socios) ? socios : [];
    fillSelect(selector, lista, 'idSocio', 'nombre', 'Seleccione un socio', (socio) => getSocioLabel(socio));

    selector.addEventListener('change', async () => {
      const id = Number(selector.value || 0);
      const socio = lista.find((item) => Number(item.idSocio) === id);
      if (title) {
        title.textContent = socio ? `Socio: ${getSocioLabel(socio)}` : 'Historial integral';
      }
      await renderHistorial(id);
    });

    if (lista.length) {
      selector.value = String(lista[0].idSocio);
      const primerSocio = lista[0];
      if (title) title.textContent = `Socio: ${getSocioLabel(primerSocio)}`;
      await renderHistorial(Number(primerSocio.idSocio));
    }
  }
  document.addEventListener('DOMContentLoaded', async () => {
    const page = document.body.dataset.page || window.location.pathname.split('/').pop();
    if (page === 'socios.html') await loadSociosPage();
    if (page === 'contratos.html') await loadContratosPage();
    if (page === 'cuotas.html') await loadCuotasPage();
    if (page === 'accesos.html') await loadAccesosPage();
    if (page === 'reservas.html') await loadReservasPage();
    if (page === 'convenios.html') await loadConveniosPage();
    if (page === 'aptos-medicos.html') await loadAptosPage();
    if (page === 'historial.html') await loadHistorialPage();
  });
})();




