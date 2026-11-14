/* =============================================================================
   FitLife Network - Reportes de gestión (TPI Bases de Datos I)
   Muestra las 8 consultas del TPI. Cada una tiene sus filtros, que el usuario
   completa en pantalla, y devuelve una tabla.
   Requiere sesión iniciada: usa el token JWT que guarda index.html en localStorage.
   ============================================================================= */
(function () {
    'use strict';

    const TOKEN_KEY = 'fitlife_token';
    const USER_KEY = 'fitlife_user';
    const API = '/api/Dashboard/';

    /* ---------- Utilidades de fecha ---------- */
    function pad(n) { return String(n).padStart(2, '0'); }
    function isoFecha(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function sumarDias(d, n) { const r = new Date(d.getTime()); r.setDate(r.getDate() + n); return r; }

    const MESES = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
    const DISCIPLINAS = ['Spinning', 'Pilates', 'Crossfit', 'Yoga', 'Zumba', 'Funcional', 'Stretching', 'Boxeo Fitness'];

    /* ---------- Definición de los 8 reportes ---------- */
    const REPORTES = [
        {
            id: 'c1', numero: 1, titulo: 'Recaudación por sede y plan', endpoint: 'recaudacion-sede',
            test: 'Comparación: año y mes',
            descripcion: 'Cuántas cuotas se cobraron en el mes elegido, cuánto se recaudó y qué parte representa cada plan dentro de su sede.',
            tecnicas: ['JOIN', 'CTE', 'GROUP BY', 'Porcentaje'],
            campos: [
                { name: 'anio', label: 'Año', type: 'number', min: 2000, max: () => new Date().getFullYear(), value: () => new Date().getFullYear() },
                { name: 'mes', label: 'Mes', type: 'select', options: MESES.map((m, i) => ({ value: String(i + 1), label: m })), value: () => String(new Date().getMonth() + 1) }
            ],
            columnas: [
                { key: 'sede', label: 'Sede' },
                { key: 'planMembresia', label: 'Plan' },
                { key: 'cuotasCobradas', label: 'Cuotas cobradas', tipo: 'int' },
                { key: 'totalRecaudado', label: 'Total recaudado', tipo: 'money' },
                { key: 'montoPromedio', label: 'Promedio por cuota', tipo: 'money' },
                { key: 'participacionSedePorcentaje', label: '% de la sede', tipo: 'pct' }
            ]
        },
        {
            id: 'c2', numero: 2, titulo: 'Accesos rechazados por apto médico', endpoint: 'accesos-denegados',
            test: 'Rango de fechas',
            descripcion: 'Socios a los que el molinete les rechazó el ingreso por apto médico vencido o inexistente: cuántos intentos hicieron, cuándo fue el último y hasta cuándo valía su apto.',
            tecnicas: ['JOIN', 'LEFT JOIN', 'GROUP BY', 'HAVING', 'LIKE'],
            campos: [
                { name: 'desde', label: 'Desde', type: 'date', value: () => isoFecha(sumarDias(new Date(), -365)) },
                { name: 'hasta', label: 'Hasta', type: 'date', value: () => isoFecha(new Date()) }
            ],
            columnas: [
                { key: 'idSocio', label: 'Socio N°', tipo: 'int' },
                { key: 'dni', label: 'DNI' },
                { key: 'socio', label: 'Socio' },
                { key: 'sedeDelIntento', label: 'Sede del intento' },
                { key: 'intentosDenegados', label: 'Intentos rechazados', tipo: 'int' },
                { key: 'ultimoIntentoDenegado', label: 'Último intento', tipo: 'fechahora' },
                { key: 'vencimientoAptoMedico', label: 'Vencimiento del apto', tipo: 'fecha' },
                { key: 'medicoRegistrado', label: 'Médico firmante' }
            ]
        },
        {
            id: 'c3', numero: 3, titulo: 'Vigencia de aptos médicos', endpoint: 'aptos-medicos',
            test: 'Pertenencia (IN)',
            descripcion: 'Estado de cada apto médico, para saber a qué socios pedirles la renovación. Marcá los estados que querés ver; si no marcás ninguno se muestran todos.',
            tecnicas: ['Vista', 'CASE', 'IN'],
            campos: [
                {
                    name: 'estados', label: 'Estados a mostrar', type: 'checks', wide: true,
                    options: [
                        { value: 'VENCIDO', label: 'Vencido', checked: true },
                        { value: 'PROXIMO A VENCER', label: 'Próximo a vencer (30 días)', checked: true },
                        { value: 'VIGENTE', label: 'Vigente', checked: false }
                    ]
                }
            ],
            columnas: [
                { key: 'numeroSocio', label: 'Socio N°', tipo: 'int' },
                { key: 'dni', label: 'DNI' },
                { key: 'nombreCompleto', label: 'Socio' },
                { key: 'telefono', label: 'Teléfono' },
                { key: 'fechaEmision', label: 'Emisión', tipo: 'fecha' },
                { key: 'fechaVencimiento', label: 'Vencimiento', tipo: 'fecha' },
                { key: 'estadoAptoMedico', label: 'Estado', tipo: 'estado' }
            ]
        },
        {
            id: 'c4', numero: 4, titulo: 'Rentabilidad de convenios con empresas', endpoint: 'rentabilidad-convenios',
            test: 'Comparación: estado de cuenta',
            descripcion: 'Rendimiento de cada convenio: socios adheridos, contratos, lo facturado, lo cobrado y lo que está en mora. Podés limitarlo a socios con un estado de cuenta.',
            tecnicas: ['JOIN', 'GROUP BY', 'SUM + CASE'],
            campos: [
                {
                    name: 'estado', label: 'Estado de cuenta del socio', type: 'select', value: () => '',
                    options: [
                        { value: '', label: 'Todos' }, { value: 'ACTIVO', label: 'Activo' }, { value: 'MOROSO', label: 'Moroso' },
                        { value: 'SUSPENDIDO', label: 'Suspendido' }, { value: 'INACTIVO', label: 'Inactivo' }
                    ]
                }
            ],
            columnas: [
                { key: 'empresaConvenio', label: 'Empresa' },
                { key: 'cuit', label: 'CUIT' },
                { key: 'porcentajeDescuentoAcordado', label: 'Descuento', tipo: 'pct' },
                { key: 'sociosAdheridos', label: 'Socios', tipo: 'int' },
                { key: 'contratosGenerados', label: 'Contratos', tipo: 'int' },
                { key: 'montoTotalFacturado', label: 'Facturado', tipo: 'money' },
                { key: 'montoCobrado', label: 'Cobrado', tipo: 'money' },
                { key: 'montoEnMora', label: 'En mora', tipo: 'money' }
            ]
        },
        {
            id: 'c5', numero: 5, titulo: 'Premios por fidelización', endpoint: 'premios-canjeados',
            test: 'Comparación: estado y antigüedad',
            descripcion: 'Premios otorgados según la antigüedad del socio y el estado del canje. Se obtiene con el procedimiento almacenado sp_AuditoriaPremiosCanjeados.',
            tecnicas: ['Stored procedure', 'Parámetros', 'DATEDIFF'],
            campos: [
                {
                    name: 'estado', label: 'Estado del canje', type: 'select', value: () => 'PENDIENTE',
                    options: [
                        { value: '', label: 'Todos' }, { value: 'PENDIENTE', label: 'Pendiente' },
                        { value: 'CANJEADO', label: 'Canjeado' }, { value: 'VENCIDO', label: 'Vencido' }
                    ]
                },
                { name: 'minMeses', label: 'Antigüedad mínima (meses)', type: 'number', min: 0, max: 600, value: () => 6 }
            ],
            columnas: [
                { key: 'idSocio', label: 'Socio N°', tipo: 'int' },
                { key: 'dni', label: 'DNI' },
                { key: 'socio', label: 'Socio' },
                { key: 'fechaAlta', label: 'Alta', tipo: 'fecha' },
                { key: 'antiguedadRealMeses', label: 'Antigüedad (meses)', tipo: 'int' },
                { key: 'premioCatalogo', label: 'Premio' },
                { key: 'mesesExigidos', label: 'Meses exigidos', tipo: 'int' },
                { key: 'fechaOtorgado', label: 'Otorgado', tipo: 'fecha' },
                { key: 'estadoCanje', label: 'Estado', tipo: 'estado' },
                { key: 'emailContacto', label: 'Email' }
            ]
        },
        {
            id: 'c6', numero: 6, titulo: 'Top 3 de productos por sede', endpoint: 'top-productos',
            test: 'Comparación: año',
            descripcion: 'Los 3 productos con mayor facturación de cada sede en el año elegido, con el porcentaje que se imputó a cuenta corriente.',
            tecnicas: ['Vista', 'CTE', 'ROW_NUMBER'],
            campos: [
                { name: 'anio', label: 'Año', type: 'number', min: 2000, max: () => new Date().getFullYear(), value: () => new Date().getFullYear() }
            ],
            columnas: [
                { key: 'sede', label: 'Sede' },
                { key: 'puestoRanking', label: 'Puesto', tipo: 'int' },
                { key: 'producto', label: 'Producto' },
                { key: 'categoría', label: 'Categoría' },
                { key: 'unidadesVendidas', label: 'Unidades', tipo: 'int' },
                { key: 'facturacionTotal', label: 'Facturación', tipo: 'money' },
                { key: 'montoCtaCte', label: 'En cuenta corriente', tipo: 'money' },
                { key: 'pctImputadoCtaCte', label: '% en cta. cte.', tipo: 'pct' }
            ]
        },
        {
            id: 'c7', numero: 7, titulo: 'Participación en clases grupales', endpoint: 'reporte-reservas',
            test: 'Rango (BETWEEN) y pertenencia (IN)',
            descripcion: 'Reservas de clases grupales de las disciplinas elegidas dentro del rango de fechas, para analizar la participación de los socios.',
            tecnicas: ['JOIN', 'BETWEEN', 'IN'],
            campos: [
                { name: 'desde', label: 'Clases desde', type: 'date', value: () => isoFecha(sumarDias(new Date(), -14)) },
                { name: 'hasta', label: 'Clases hasta', type: 'date', value: () => isoFecha(sumarDias(new Date(), 10)) },
                { name: 'disciplina1', label: 'Disciplina', type: 'select', value: () => 'Spinning', options: DISCIPLINAS.map(d => ({ value: d, label: d })) },
                {
                    name: 'disciplina2', label: 'Otra disciplina', type: 'select', value: () => 'Yoga',
                    options: [{ value: '', label: 'Ninguna' }].concat(DISCIPLINAS.map(d => ({ value: d, label: d })))
                }
            ],
            columnas: [
                { key: 'idReserva', label: 'Reserva N°', tipo: 'int' },
                { key: 'nombre', label: 'Nombre' },
                { key: 'apellido', label: 'Apellido' },
                { key: 'instructorName', label: 'Instructor' },
                { key: 'fechaClase', label: 'Fecha de la clase', tipo: 'fecha' },
                { key: 'horaInicio', label: 'Inicio', tipo: 'hora' },
                { key: 'nombreDisciplina', label: 'Disciplina' },
                { key: 'estadoReserva', label: 'Estado', tipo: 'estado' }
            ]
        },
        {
            id: 'c8', numero: 8, titulo: 'Evaluaciones físicas por profesional', endpoint: 'evaluaciones',
            test: 'Rango de fechas',
            descripcion: 'Por sede y profesional: cantidad de turnos atendidos con evaluación y promedios de peso, grasa corporal y masa muscular.',
            tecnicas: ['JOIN', 'GROUP BY', 'HAVING', 'AVG'],
            campos: [
                { name: 'desde', label: 'Turnos desde', type: 'date', value: () => isoFecha(sumarDias(new Date(), -365)) },
                { name: 'hasta', label: 'Turnos hasta', type: 'date', value: () => isoFecha(new Date()) }
            ],
            columnas: [
                { key: 'sedeConsultorio', label: 'Sede' },
                { key: 'profesionalACargo', label: 'Profesional' },
                { key: 'especialidad', label: 'Especialidad' },
                { key: 'evaluacionesRealizadas', label: 'Evaluaciones', tipo: 'int' },
                { key: 'pesoPromedioKg', label: 'Peso promedio (kg)', tipo: 'num' },
                { key: 'porcentajeGrasaPromedio', label: '% grasa promedio', tipo: 'pct' },
                { key: 'porcentajeMasaMuscularPromedio', label: '% masa muscular promedio', tipo: 'pct' }
            ]
        }
    ];

    /* ---------- Formatos ---------- */
    const nf0 = new Intl.NumberFormat('es-AR');
    const nf2 = new Intl.NumberFormat('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    const nfMoney = new Intl.NumberFormat('es-AR', { style: 'currency', currency: 'ARS', minimumFractionDigits: 2 });
    const TIPOS_NUMERICOS = ['int', 'num', 'money', 'pct'];
    const ESTADO_CLASE = {
        'VIGENTE': 'success', 'PROXIMO A VENCER': 'warning', 'VENCIDO': 'danger', 'SIN APTA MEDICA': 'secondary',
        'CANJEADO': 'success', 'PENDIENTE': 'warning',
        'ASISTIO': 'success', 'CONFIRMADA': 'primary', 'CANCELADA': 'secondary', 'AUSENTE': 'danger'
    };

    function esc(valor) {
        return String(valor === null || valor === undefined ? '' : valor).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    }

    function partesFecha(v) {
        const m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(v || '');
        return m ? { y: m[1], mo: m[2], d: m[3], h: m[4], mi: m[5] } : null;
    }
    function fmtFecha(v) { const p = partesFecha(v); return p ? p.d + '/' + p.mo + '/' + p.y : ''; }
    function fmtFechaHora(v) {
        const p = partesFecha(v);
        if (!p) return '';
        return p.h ? p.d + '/' + p.mo + '/' + p.y + ' ' + p.h + ':' + p.mi : p.d + '/' + p.mo + '/' + p.y;
    }
    function fmtHora(v) { const m = /^(\d{2}):(\d{2})/.exec(v || ''); return m ? m[1] + ':' + m[2] : ''; }

    /** Texto plano de una celda (para CSV y para tablas). */
    function valorPlano(col, fila) {
        const v = fila[col.key];
        if (v === null || v === undefined) return '';
        switch (col.tipo) {
            case 'int': return nf0.format(v);
            case 'num': return nf2.format(v);
            case 'money': return nfMoney.format(v);
            case 'pct': return nf2.format(v) + ' %';
            case 'fecha': return fmtFecha(v);
            case 'fechahora': return fmtFechaHora(v);
            case 'hora': return fmtHora(v);
            default: return String(v);
        }
    }

    /** HTML de una celda (escapado). */
    function celdaHtml(col, fila) {
        const texto = valorPlano(col, fila);
        if (col.tipo === 'estado' && texto) {
            const clase = ESTADO_CLASE[texto] || 'secondary';
            return '<span class="badge text-bg-' + clase + '">' + esc(texto) + '</span>';
        }
        return esc(texto);
    }

    function csvCelda(texto) {
        const t = String(texto);
        return /[";\n\r]/.test(t) ? '"' + t.replace(/"/g, '""') + '"' : t;
    }
    function armarCsv(reporte, filas) {
        const cabecera = reporte.columnas.map(c => csvCelda(c.label)).join(';');
        const cuerpo = filas.map(f => reporte.columnas.map(c => csvCelda(valorPlano(c, f))).join(';'));
        return '\ufeff' + [cabecera].concat(cuerpo).join('\r\n');
    }

    /* ---------- Formulario ---------- */
    function valorInicial(campo) { return typeof campo.value === 'function' ? campo.value() : campo.value; }
    function limite(campo, nombre) { return typeof campo[nombre] === 'function' ? campo[nombre]() : campo[nombre]; }

    function campoHtml(campo, reporteId) {
        const id = reporteId + '-' + campo.name;
        const ancho = campo.wide ? 'col-12' : 'col-sm-6 col-lg-3';
        if (campo.type === 'select') {
            const actual = valorInicial(campo);
            const opciones = campo.options.map(function (o) {
                return '<option value="' + esc(o.value) + '"' + (o.value === actual ? ' selected' : '') + '>' + esc(o.label) + '</option>';
            }).join('');
            return '<div class="' + ancho + '"><label class="form-label" for="' + id + '">' + esc(campo.label) + '</label>' +
                '<select class="form-select" id="' + id + '" name="' + campo.name + '">' + opciones + '</select></div>';
        }
        if (campo.type === 'checks') {
            const checks = campo.options.map(function (o, i) {
                return '<div class="form-check form-check-inline"><input class="form-check-input" type="checkbox" name="' + campo.name +
                    '" id="' + id + '-' + i + '" value="' + esc(o.value) + '"' + (o.checked ? ' checked' : '') + '>' +
                    '<label class="form-check-label" for="' + id + '-' + i + '">' + esc(o.label) + '</label></div>';
            }).join('');
            return '<div class="' + ancho + '"><span class="form-label d-block">' + esc(campo.label) + '</span>' + checks + '</div>';
        }
        const extra = campo.type === 'number'
            ? ' min="' + limite(campo, 'min') + '" max="' + limite(campo, 'max') + '" step="1"' : '';
        return '<div class="' + ancho + '"><label class="form-label" for="' + id + '">' + esc(campo.label) + '</label>' +
            '<input class="form-control" type="' + campo.type + '" id="' + id + '" name="' + campo.name +
            '" value="' + esc(valorInicial(campo)) + '"' + extra + ' required></div>';
    }

    /** Lee el formulario y devuelve { params, error }. */
    function leerFormulario(reporte, form) {
        const params = new URLSearchParams();
        reporte.campos.forEach(function (campo) {
            if (campo.type === 'checks') {
                const marcados = Array.prototype.slice.call(form.querySelectorAll('input[name="' + campo.name + '"]:checked'))
                    .map(i => i.value);
                if (marcados.length) params.set(campo.name, marcados.join(','));
            } else {
                const el = form.elements[campo.name];
                const valor = el ? String(el.value).trim() : '';
                if (valor !== '') params.set(campo.name, valor);
            }
        });
        return { params: params, error: validar(reporte, params) };
    }

    function validar(reporte, params) {
        const desde = params.get('desde');
        const hasta = params.get('hasta');
        if (reporte.campos.some(c => c.name === 'desde') && (!desde || !hasta)) return 'Completá las dos fechas.';
        if (desde && hasta && desde > hasta) return 'La fecha "desde" no puede ser posterior a "hasta".';
        const anio = params.get('anio');
        if (reporte.campos.some(c => c.name === 'anio')) {
            const n = Number(anio);
            if (!anio || !Number.isInteger(n) || n < 2000 || n > new Date().getFullYear()) {
                return 'Ingresá un año entre 2000 y ' + new Date().getFullYear() + '.';
            }
        }
        const meses = params.get('minMeses');
        if (reporte.campos.some(c => c.name === 'minMeses')) {
            const n = Number(meses);
            if (meses === null || !Number.isInteger(n) || n < 0) return 'La antigüedad mínima debe ser un número entero de 0 en adelante.';
        }
        if (reporte.id === 'c7' && !params.get('disciplina1')) return 'Elegí al menos una disciplina.';
        return '';
    }

    /* ---------- Pantalla ---------- */
    let reporteActual = null;
    let ultimasFilas = [];
    let solicitud = 0;

    function el(id) { return document.getElementById(id); }
    function token() { try { return localStorage.getItem(TOKEN_KEY); } catch (e) { return null; } }
    function cerrarSesion() {
        try { localStorage.removeItem(TOKEN_KEY); localStorage.removeItem(USER_KEY); } catch (e) { /* sin almacenamiento */ }
        window.location.href = 'index.html';
    }

    function dibujarLista() {
        el('listaReportes').innerHTML = REPORTES.map(function (r) {
            return '<button type="button" class="list-group-item list-group-item-action" data-id="' + r.id + '">' +
                '<span class="fw-semibold">' + r.numero + '. ' + esc(r.titulo) + '</span>' +
                '<small class="d-block text-secondary">' + esc(r.test) + '</small></button>';
        }).join('');
    }

    function marcarActivo(id) {
        Array.prototype.forEach.call(el('listaReportes').children, function (b) {
            const activo = b.getAttribute('data-id') === id;
            b.classList.toggle('active', activo);
            if (activo) b.setAttribute('aria-current', 'true'); else b.removeAttribute('aria-current');
        });
    }

    function seleccionar(id, ejecutarAhora) {
        const reporte = REPORTES.find(r => r.id === id) || REPORTES[0];
        reporteActual = reporte;
        marcarActivo(reporte.id);
        if (window.history && window.history.replaceState) window.history.replaceState(null, '', '#' + reporte.id);

        el('repTitulo').textContent = 'Consulta ' + reporte.numero + ': ' + reporte.titulo;
        el('repDescripcion').textContent = reporte.descripcion;
        el('repTecnicas').innerHTML = reporte.tecnicas.map(t => '<span class="badge text-bg-light border">' + esc(t) + '</span>').join(' ');
        el('repCampos').innerHTML = reporte.campos.map(c => campoHtml(c, reporte.id)).join('');
        limpiarResultado();
        if (ejecutarAhora) ejecutar();
    }

    function limpiarResultado() {
        ultimasFilas = [];
        el('repAviso').innerHTML = '';
        el('repResumen').textContent = '';
        el('repTabla').innerHTML = '';
        el('btnCsv').disabled = true;
    }

    function avisoHtml(tipo, texto, accionHtml) {
        return '<div class="alert alert-' + tipo + ' mb-3" role="alert">' + esc(texto) + (accionHtml || '') + '</div>';
    }

    function dibujarTabla(reporte, filas) {
        if (!filas.length) {
            el('repTabla').innerHTML = '<div class="text-center text-secondary py-5 px-3">' +
                '<div class="fw-semibold mb-1">No hay filas para estos filtros</div>' +
                '<div>Ampliá el rango de fechas o cambiá el estado y volvé a ejecutar.</div></div>';
            return;
        }
        const cabecera = reporte.columnas.map(function (c) {
            const num = TIPOS_NUMERICOS.indexOf(c.tipo) !== -1;
            return '<th scope="col" class="' + (num ? 'text-end' : '') + '">' + esc(c.label) + '</th>';
        }).join('');
        const cuerpo = filas.map(function (f) {
            return '<tr>' + reporte.columnas.map(function (c) {
                const num = TIPOS_NUMERICOS.indexOf(c.tipo) !== -1;
                return '<td class="' + (num ? 'text-end' : '') + '">' + celdaHtml(c, f) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        el('repTabla').innerHTML = '<div class="tabla-scroll"><table class="table table-hover table-sm align-middle mb-0 tabla-resultado">' +
            '<thead><tr>' + cabecera + '</tr></thead><tbody>' + cuerpo + '</tbody></table></div>';
    }

    async function ejecutar() {
        const reporte = reporteActual;
        if (!reporte) return;
        const lectura = leerFormulario(reporte, el('repForm'));
        el('repAviso').innerHTML = '';
        if (lectura.error) {
            el('repAviso').innerHTML = avisoHtml('warning', lectura.error);
            return;
        }
        const jwt = token();
        if (!jwt) { cerrarSesion(); return; }

        const miSolicitud = ++solicitud;
        const boton = el('btnEjecutar');
        boton.disabled = true;
        boton.innerHTML = '<span class="spinner-border spinner-border-sm me-2" aria-hidden="true"></span>Consultando…';
        el('repResumen').textContent = '';
        el('btnCsv').disabled = true;

        const inicio = performance.now();
        try {
            const consulta = lectura.params.toString();
            const resp = await fetch(API + reporte.endpoint + (consulta ? '?' + consulta : ''), {
                headers: { 'Authorization': 'Bearer ' + jwt, 'Accept': 'application/json' }
            });
            if (miSolicitud !== solicitud) return;           // llegó una respuesta vieja

            if (resp.status === 401) {
                limpiarResultado();
                el('repAviso').innerHTML = avisoHtml('warning', 'Tu sesión no es válida o venció. Volvé a iniciar sesión.',
                    ' <button type="button" class="btn btn-sm btn-warning ms-2" id="btnReingresar">Iniciar sesión</button>');
                el('btnReingresar').addEventListener('click', cerrarSesion);
                return;
            }
            if (resp.status === 403) {
                limpiarResultado();
                el('repAviso').innerHTML = avisoHtml('warning', 'Tu usuario no tiene permiso para ver estos reportes. Ingresá con un usuario gerente, por ejemplo gerente.general.');
                return;
            }
            if (!resp.ok) {
                const cuerpo = await resp.text();
                let mensaje = cuerpo;
                try { const j = JSON.parse(cuerpo); mensaje = j.message || j.title || cuerpo; } catch (e) { /* texto plano */ }
                limpiarResultado();
                el('repAviso').innerHTML = avisoHtml('danger', 'No se pudo ejecutar la consulta: ' + (mensaje || ('error ' + resp.status)));
                return;
            }

            const datos = await resp.json();
            if (miSolicitud !== solicitud) return;
            ultimasFilas = Array.isArray(datos) ? datos : [];
            dibujarTabla(reporte, ultimasFilas);
            const seg = ((performance.now() - inicio) / 1000).toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            el('repResumen').textContent = ultimasFilas.length + (ultimasFilas.length === 1 ? ' fila' : ' filas') + ' · ' + seg + ' s';
            el('btnCsv').disabled = ultimasFilas.length === 0;
        } catch (err) {
            if (miSolicitud !== solicitud) return;
            limpiarResultado();
            el('repAviso').innerHTML = avisoHtml('danger', 'No se pudo conectar con el servidor. Revisá que la aplicación esté en ejecución y la base de datos disponible.');
        } finally {
            if (miSolicitud === solicitud) {
                boton.disabled = false;
                boton.innerHTML = '<i class="bi bi-play-fill me-1"></i>Ejecutar consulta';
            }
        }
    }

    function descargarCsv() {
        if (!reporteActual || !ultimasFilas.length) return;
        const blob = new Blob([armarCsv(reporteActual, ultimasFilas)], { type: 'text/csv;charset=utf-8' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'consulta-' + reporteActual.numero + '-' + isoFecha(new Date()) + '.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }

    function iniciar() {
        if (token() && token().endsWith('.demo')) { cerrarSesion(); return; }
        if (!token()) { window.location.href = 'index.html'; return; }

        let usuario = null;
        try { usuario = JSON.parse(localStorage.getItem(USER_KEY) || 'null'); } catch (e) { usuario = null; }
        el('userName').textContent = (usuario && usuario.nombre) || 'Usuario';
        el('userRole').textContent = usuario && usuario.roles && usuario.roles.length ? usuario.roles.join(', ') : 'Sesión iniciada';
        el('logoutBtn').addEventListener('click', cerrarSesion);

        dibujarLista();
        el('listaReportes').addEventListener('click', function (ev) {
            const boton = ev.target.closest('button[data-id]');
            if (boton) seleccionar(boton.getAttribute('data-id'), true);
        });
        el('repForm').addEventListener('submit', function (ev) { ev.preventDefault(); ejecutar(); });
        el('btnCsv').addEventListener('click', descargarCsv);

        const desdeHash = (window.location.hash || '').replace('#', '');
        seleccionar(REPORTES.some(r => r.id === desdeHash) ? desdeHash : REPORTES[0].id, true);
    }

    /* Exporta las funciones puras para poder probarlas con Node. */
    if (typeof module !== 'undefined' && module.exports) {
        module.exports = { REPORTES, esc, fmtFecha, fmtFechaHora, fmtHora, valorPlano, celdaHtml, armarCsv, validar, isoFecha, sumarDias };
    }
    if (typeof document !== 'undefined') {
        document.addEventListener('DOMContentLoaded', iniciar);
    }
})();
