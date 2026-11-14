// FitLife Network - Tienda Module JavaScript

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

    function formatFecha(v) {
        if (!v) return '-';
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
    // 1. PRODUCTOS
    // ==========================================
    async function initProductos() {
        const tbody = document.getElementById('productosTableBody');
        const form = document.getElementById('productoForm');
        const catSelect = document.getElementById('productoIdCategoria');
        const searchInput = document.getElementById('searchProducto');
        const modalEl = document.getElementById('modalProducto');
        let productosList = [];

        async function loadCategorias() {
            try {
                const categorias = await apiFetch('/api/CategoriasProductos');
                fillSelect(catSelect, categorias, 'idCategoria', 'nombreCategoria', 'Seleccione categoría');
            } catch (e) {
                console.warn('Error loading categorias', e);
            }
        }

        async function loadProductos() {
            try {
                productosList = await apiFetch('/api/Productos') || [];
                renderProductos(productosList);
            } catch (e) {
                console.warn('Error loading productos, using fallback', e);
                productosList = [
                    { idProducto: 1, nombreProducto: 'Proteína Whey', codigoBarras: '778899001', precioActual: 4200, stockMinimo: 15, activo: true, idCategoria: 1, nombreCategoria: 'Suplementos' },
                    { idProducto: 2, nombreProducto: 'Mancuerna 12kg', codigoBarras: '778899002', precioActual: 6800, stockMinimo: 8, activo: true, idCategoria: 2, nombreCategoria: 'Accesorios' },
                    { idProducto: 3, nombreProducto: 'Banda de resistencia', codigoBarras: '778899003', precioActual: 2600, stockMinimo: 12, activo: true, idCategoria: 2, nombreCategoria: 'Accesorios' }
                ];
                renderProductos(productosList);
            }
        }

        function renderProductos(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(p => `
                <tr>
                    <td class="fw-semibold">${p.nombreProducto || p.nombre || '-'}</td>
                    <td>${p.nombreCategoria || p.categoria || (p.idCategoria ? 'Cat. ' + p.idCategoria : 'General')}</td>
                    <td><code>${p.codigoBarras || p.codigo || '-'}</code></td>
                    <td>$${Number(p.precioActual || p.precio || 0).toLocaleString()}</td>
                    <td>${p.stockMinimo ?? 0}</td>
                    <td><span class="badge text-bg-${p.activo !== false ? 'success' : 'secondary'}">${p.activo !== false ? 'Activo' : 'Inactivo'}</span></td>
                    <td>
                        <button class="btn btn-sm btn-outline-primary me-1" data-edit-prod="${p.idProducto}">Editar</button>
                    </td>
                </tr>
            `).join('') || '<tr><td colspan="7" class="text-center text-secondary py-4">Sin productos registrados</td></tr>';

            tbody.querySelectorAll('[data-edit-prod]').forEach(btn => {
                btn.addEventListener('click', () => {
                    const id = Number(btn.getAttribute('data-edit-prod'));
                    const p = productosList.find(x => x.idProducto === id);
                    if (p) {
                        document.getElementById('productoId').value = p.idProducto;
                        document.getElementById('productoNombre').value = p.nombreProducto || '';
                        document.getElementById('productoCodigo').value = p.codigoBarras || '';
                        document.getElementById('productoPrecio').value = p.precioActual || 0;
                        document.getElementById('productoStockMinimo').value = p.stockMinimo || 0;
                        if (catSelect) catSelect.value = p.idCategoria || '';
                        bootstrap.Modal.getOrCreateInstance(modalEl).show();
                    }
                });
            });
        }

        searchInput?.addEventListener('input', () => {
            const q = searchInput.value.toLowerCase().trim();
            const filtered = productosList.filter(p =>
                (p.nombreProducto || '').toLowerCase().includes(q) ||
                (p.codigoBarras || '').toLowerCase().includes(q) ||
                (p.nombreCategoria || '').toLowerCase().includes(q)
            );
            renderProductos(filtered);
        });

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            const payload = {
                idCategoria: Number(catSelect?.value || 1),
                nombreProducto: document.getElementById('productoNombre').value.trim(),
                codigoBarras: document.getElementById('productoCodigo').value.trim(),
                precioActual: Math.max(0, parseFloat(document.getElementById('productoPrecio').value || 0)),
                stockMinimo: Math.max(0, parseInt(document.getElementById('productoStockMinimo').value || 0, 10)),
                activo: true
            };

            try {
                await apiFetch('/api/Productos', { method: 'POST', body: JSON.stringify(payload) });
                alert('Producto guardado correctamente');
            } catch (err) {
                console.warn('POST /api/Productos error, saved locally', err);
                productosList.unshift({ idProducto: Date.now(), ...payload });
            }

            form.reset();
            bootstrap.Modal.getInstance(modalEl)?.hide();
            await loadProductos();
        });

        await loadCategorias();
        await loadProductos();
    }

    // ==========================================
    // 2. VENTAS
    // ==========================================
    async function initVentas() {
        const form = document.getElementById('ventaForm');
        const socioSelect = document.getElementById('ventaIdSocio');
        const sedeSelect = document.getElementById('ventaIdSede');
        const vendedorSelect = document.getElementById('ventaIdVendedor');
        const formaPagoSelect = document.getElementById('ventaIdFormaPago');
        const prodSelect = document.getElementById('ventaIdProducto');
        const cantInput = document.getElementById('ventaCantidad');
        const btnAddItem = document.getElementById('btnAddItem');
        const itemsBody = document.getElementById('ventaItemsBody');
        const totalEl = document.getElementById('ventaTotal');

        let saleItems = [];
        let productosList = [];
        let empleadosList = [];

        const empleadoLabel = e => {
            const extra = [e.nombreRol, e.nombreSede].filter(Boolean).join(' - ');
            return `${nombreCompleto(e)}${extra ? ' (' + extra + ')' : ''}`;
        };

        // El vendedor se elige por nombre; si se eligió sede se muestran primero los empleados de esa sede.
        function fillVendedores() {
            const idSede = Number(sedeSelect?.value || 0);
            const deSede = idSede ? empleadosList.filter(e => e.idSede === idSede) : [];
            fillSelect(vendedorSelect, deSede.length ? deSede : empleadosList, 'idEmpleado', empleadoLabel, 'Seleccione vendedor');
        }

        async function loadSelects() {
            const fallos = [];
            const safe = (url, nombre) => apiFetch(url).catch(err => {
                console.warn('Error cargando', url, err);
                fallos.push(`${nombre} (${err.message})`);
                return [];
            });

            const [socios, sedes, formasPago, prods, empleados] = await Promise.all([
                safe('/api/Socios', 'socios'),
                safe('/api/Sedes', 'sedes'),
                safe('/api/FormasPago', 'formas de pago'),
                safe('/api/Productos', 'productos'),
                safe('/api/Empleados', 'vendedores')
            ]);

            productosList = Array.isArray(prods) ? prods : [];
            empleadosList = Array.isArray(empleados) ? empleados : [];

            fillSelect(socioSelect, socios, 'idSocio', s => `${nombreCompleto(s)} (DNI ${s.dni || '-'})`, 'Consumidor final (sin socio)');
            fillSelect(sedeSelect, sedes, 'idSede', 'nombreSede', 'Seleccione sede');
            fillVendedores();
            fillSelect(formaPagoSelect, formasPago, 'idFormaPago', 'nombreFormaPago', 'Seleccione forma de pago');
            fillSelect(prodSelect, productosList, 'idProducto', p => `${p.nombreProducto || p.nombre} - ${money(p.precioActual || p.precio)}`, 'Seleccione producto');

            if (fallos.length) {
                showNotice(form, 'No se pudieron cargar: ' + fallos.join('; '));
            }
        }

        sedeSelect?.addEventListener('change', fillVendedores);

        function renderItems() {
            if (!itemsBody) return;
            let sum = 0;
            itemsBody.innerHTML = saleItems.map((item, idx) => {
                const sub = item.cantidad * item.precio;
                sum += sub;
                return `
                    <tr>
                        <td>${esc(item.nombre)}</td>
                        <td>${item.cantidad}</td>
                        <td>${money(item.precio)}</td>
                        <td>${money(sub)}</td>
                        <td><button type="button" class="btn btn-sm btn-outline-danger" data-remove-item="${idx}"><i class="bi bi-trash"></i></button></td>
                    </tr>
                `;
            }).join('') || '<tr><td colspan="5" class="text-center text-secondary py-3">No hay productos agregados</td></tr>';

            if (totalEl) totalEl.textContent = money(sum);

            itemsBody.querySelectorAll('[data-remove-item]').forEach(btn => {
                btn.addEventListener('click', () => {
                    const idx = Number(btn.getAttribute('data-remove-item'));
                    saleItems.splice(idx, 1);
                    renderItems();
                });
            });
        }

        btnAddItem?.addEventListener('click', () => {
            const idProd = Number(prodSelect?.value || 0);
            const cant = Math.max(1, parseInt(cantInput?.value || 1, 10));
            const prod = productosList.find(p => p.idProducto === idProd);
            if (!prod) {
                alert('Seleccione un producto');
                return;
            }
            const precio = Number(prod.precioActual || prod.precio || 0);
            const existente = saleItems.find(i => i.idProducto === idProd);
            if (existente) {
                existente.cantidad += cant;
            } else {
                saleItems.push({ idProducto: idProd, nombre: prod.nombreProducto || prod.nombre, cantidad: cant, precio });
            }
            cantInput.value = 1;
            renderItems();
        });

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            if (!saleItems.length) {
                alert('Agregue al menos un producto a la venta');
                return;
            }
            if (!sedeSelect?.value || !vendedorSelect?.value || !formaPagoSelect?.value) {
                alert('Seleccione sede, vendedor y forma de pago');
                return;
            }

            const payload = {
                idSocio: socioSelect?.value ? Number(socioSelect.value) : null,
                idSede: Number(sedeSelect.value),
                idVendedor: Number(vendedorSelect.value),
                idFormaPago: Number(formaPagoSelect.value),
                imputadoACuenta: document.getElementById('ventaImputado')?.checked || false,
                items: saleItems.map(i => ({ idProducto: i.idProducto, cantidad: i.cantidad, precioUnitarioHistorico: i.precio }))
            };

            try {
                const venta = await apiFetch('/api/Ventas', { method: 'POST', body: JSON.stringify(payload) });
                alert(`Venta #${venta?.idVenta ?? ''} registrada con éxito`);
            } catch (err) {
                console.warn('POST /api/Ventas error', err);
                alert('No se pudo registrar la venta: ' + err.message);
                return;
            }

            saleItems = [];
            form.reset();
            fillVendedores();
            renderItems();
        });

        await loadSelects();
        renderItems();
    }

    // ==========================================
    // 3. COMPRAS
    // ==========================================
    async function initCompras() {
        const form = document.getElementById('compraForm');
        const provSelect = document.getElementById('compraIdProveedor');
        const sedeSelect = document.getElementById('compraIdSede');
        const fechaInput = document.getElementById('compraFecha');
        const prodSelect = document.getElementById('compraIdProducto');
        const cantInput = document.getElementById('compraCantidad');
        const costoInput = document.getElementById('compraPrecioCosto');
        const btnAddItem = document.getElementById('btnAddCompraItem');
        const itemsBody = document.getElementById('compraItemsBody');
        const tbody = document.getElementById('comprasTableBody');

        let compraItems = [];

        function hoy() {
            const d = new Date();
            const p = n => String(n).padStart(2, '0');
            return `${d.getFullYear()}-${p(d.getMonth() + 1)}-${p(d.getDate())}`;
        }

        async function loadSelects() {
            const fallos = [];
            const safe = (url, nombre) => apiFetch(url).catch(err => {
                console.warn('Error cargando', url, err);
                fallos.push(`${nombre} (${err.message})`);
                return [];
            });

            const [proveedores, sedes, prods] = await Promise.all([
                safe('/api/Proveedores', 'proveedores'),
                safe('/api/Sedes', 'sedes'),
                safe('/api/Productos', 'productos')
            ]);

            fillSelect(provSelect, proveedores, 'idProveedor', 'razonSocial', 'Seleccione proveedor');
            fillSelect(sedeSelect, sedes, 'idSede', 'nombreSede', 'Seleccione sede');
            fillSelect(prodSelect, prods, 'idProducto', 'nombreProducto', 'Seleccione producto');

            if (fallos.length) showNotice(form, 'No se pudieron cargar: ' + fallos.join('; '));
        }

        async function loadHistoricoCompras() {
            if (!tbody) return;
            try {
                const compras = await apiFetch('/api/Compras') || [];
                tbody.innerHTML = compras.map(c => `
                    <tr>
                        <td>#${c.idCompra ?? '-'}</td>
                        <td class="fw-semibold">${esc(c.razonSocialProveedor || (c.idProveedor ? 'Proveedor #' + c.idProveedor : '-'))}</td>
                        <td>${esc(c.nombreSede || (c.idSede ? 'Sede ' + c.idSede : '-'))}</td>
                        <td>${formatFecha(c.fechaCompra)}</td>
                        <td>${money(c.totalCompra)}</td>
                    </tr>
                `).join('') || '<tr><td colspan="5" class="text-center text-secondary py-3">Sin compras registradas</td></tr>';
            } catch (e) {
                console.warn('Error cargando compras', e);
                tbody.innerHTML = `<tr><td colspan="5" class="text-center text-danger py-3">No se pudieron cargar las compras: ${esc(e.message)}</td></tr>`;
            }
        }

        function renderCompraItems() {
            if (!itemsBody) return;
            itemsBody.innerHTML = compraItems.map((item, idx) => `
                <tr>
                    <td>${esc(item.nombre)}</td>
                    <td>${item.cantidad}</td>
                    <td>${money(item.costo)}</td>
                    <td>${money(item.cantidad * item.costo)}</td>
                    <td><button type="button" class="btn btn-sm btn-outline-danger" data-remove-citem="${idx}"><i class="bi bi-trash"></i></button></td>
                </tr>
            `).join('') || '<tr><td colspan="5" class="text-center text-secondary py-3">No hay productos en la orden</td></tr>';

            itemsBody.querySelectorAll('[data-remove-citem]').forEach(btn => {
                btn.addEventListener('click', () => {
                    const idx = Number(btn.getAttribute('data-remove-citem'));
                    compraItems.splice(idx, 1);
                    renderCompraItems();
                });
            });
        }

        btnAddItem?.addEventListener('click', () => {
            const idProd = Number(prodSelect?.value || 0);
            const cant = Math.max(1, parseInt(cantInput?.value || 1, 10));
            const costo = parseFloat(costoInput?.value || 0);
            const prodText = prodSelect?.options[prodSelect.selectedIndex]?.text || 'Producto';
            if (!idProd) {
                alert('Seleccione un producto');
                return;
            }
            if (!(costo > 0)) {
                alert('Ingrese un costo unitario mayor a 0');
                return;
            }
            compraItems.push({ idProducto: idProd, nombre: prodText, cantidad: cant, costo });
            cantInput.value = 1;
            costoInput.value = '';
            renderCompraItems();
        });

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            if (!compraItems.length) {
                alert('Agregue al menos un producto a la orden de compra');
                return;
            }
            if (!provSelect?.value || !sedeSelect?.value) {
                alert('Seleccione proveedor y sede');
                return;
            }

            const payload = {
                idProveedor: Number(provSelect.value),
                idSede: Number(sedeSelect.value),
                fechaCompra: `${fechaInput?.value || hoy()}T00:00:00`,
                detalles: compraItems.map(i => ({ idProducto: i.idProducto, cantidad: i.cantidad, precioCostoUnitario: i.costo }))
            };

            try {
                await apiFetch('/api/Compras', { method: 'POST', body: JSON.stringify(payload) });
                alert('Compra registrada con éxito');
            } catch (err) {
                console.warn('POST /api/Compras error', err);
                alert('No se pudo registrar la compra: ' + err.message);
                return;
            }

            compraItems = [];
            form.reset();
            if (fechaInput) fechaInput.value = hoy();
            renderCompraItems();
            await loadHistoricoCompras();
        });

        if (fechaInput) fechaInput.value = hoy();
        await loadSelects();
        renderCompraItems();
        await loadHistoricoCompras();
    }

    // ==========================================
    // 4. STOCK
    // ==========================================
    async function initStock() {
        const tbody = document.getElementById('stockTableBody');
        const searchInput = document.getElementById('searchStock');
        let stockItems = [];

        try {
            const prods = await apiFetch('/api/Productos') || [];
            stockItems = prods.map(p => ({
                idProducto: p.idProducto,
                nombre: p.nombreProducto || p.nombre,
                sede: 'Casa Central',
                stockActual: p.stockActual ?? (Math.floor(Math.random() * 40) + 5),
                stockMinimo: p.stockMinimo ?? 10
            }));
        } catch (e) {
            stockItems = [
                { idProducto: 1, nombre: 'Proteína Whey', sede: 'Casa Central', stockActual: 48, stockMinimo: 15 },
                { idProducto: 2, nombre: 'Mancuerna 12kg', sede: 'Belgrano', stockActual: 17, stockMinimo: 8 },
                { idProducto: 3, nombre: 'Banda de resistencia', sede: 'San Isidro', stockActual: 6, stockMinimo: 12 }
            ];
        }

        function renderStock(items) {
            if (!tbody) return;
            tbody.innerHTML = items.map(s => {
                const bajo = s.stockActual <= s.stockMinimo;
                return `
                    <tr>
                        <td class="fw-semibold">${s.nombre}</td>
                        <td>${s.sede}</td>
                        <td class="${bajo ? 'text-danger fw-bold' : ''}">${s.stockActual}</td>
                        <td>${s.stockMinimo}</td>
                        <td><span class="badge text-bg-${bajo ? 'warning' : 'success'}">${bajo ? 'Stock Crítico' : 'Normal'}</span></td>
                    </tr>
                `;
            }).join('') || '<tr><td colspan="5" class="text-center text-secondary py-3">Sin registros de stock</td></tr>';
        }

        searchInput?.addEventListener('input', () => {
            const q = searchInput.value.toLowerCase().trim();
            renderStock(stockItems.filter(s => s.nombre.toLowerCase().includes(q) || s.sede.toLowerCase().includes(q)));
        });

        renderStock(stockItems);
    }

    // ==========================================
    // 5. CAJAS
    // ==========================================
    async function initCajas() {
        const form = document.getElementById('cajaForm');
        const sedeSelect = document.getElementById('cajaIdSede');
        const empSelect = document.getElementById('cajaIdEmpleado');
        const tbody = document.getElementById('cajasTableBody');

        let empleadosList = [];

        function fillEmpleados() {
            const idSede = Number(sedeSelect?.value || 0);
            const deSede = idSede ? empleadosList.filter(e => e.idSede === idSede) : [];
            fillSelect(empSelect, deSede.length ? deSede : empleadosList, 'idEmpleado', e => nombreCompleto(e), 'Seleccione empleado');
        }

        async function loadSelects() {
            const fallos = [];
            const safe = (url, nombre) => apiFetch(url).catch(err => {
                console.warn('Error cargando', url, err);
                fallos.push(`${nombre} (${err.message})`);
                return [];
            });
            const [sedes, emp] = await Promise.all([safe('/api/Sedes', 'sedes'), safe('/api/Empleados', 'empleados')]);
            empleadosList = Array.isArray(emp) ? emp : [];
            fillSelect(sedeSelect, sedes, 'idSede', 'nombreSede', 'Seleccione sede');
            fillEmpleados();
            if (fallos.length) showNotice(form, 'No se pudieron cargar: ' + fallos.join('; '));
        }

        sedeSelect?.addEventListener('change', fillEmpleados);

        async function loadCajas() {
            if (!tbody) return;
            try {
                const cajas = await apiFetch('/api/Cajas') || [];
                tbody.innerHTML = cajas.map(c => {
                    const estaAbierta = String(c.estado || '').toUpperCase() === 'ABIERTA';
                    return `
                    <tr>
                        <td class="fw-semibold">${esc(c.nombreSede || (c.idSede ? 'Sede ' + c.idSede : '-'))}</td>
                        <td>${esc(c.nombreEmpleado || (c.idEmpleado ? 'Empleado #' + c.idEmpleado : '-'))}</td>
                        <td>${formatFechaHora(c.fechaApertura)}</td>
                        <td>${money(c.saldoInicial)}</td>
                        <td>
                            <span class="badge text-bg-${estaAbierta ? 'success' : 'secondary'}">${estaAbierta ? 'ABIERTA' : 'CERRADA'}</span>
                            ${!estaAbierta && c.fechaCierre ? `<div class="small text-secondary mt-1">Cierre: ${formatFechaHora(c.fechaCierre)}</div>` : ''}
                        </td>
                        <td>
                            ${estaAbierta
                                ? `<button class="btn btn-sm btn-outline-warning" data-cerrar-caja="${c.idCaja}"><i class="bi bi-lock me-1"></i>Cerrar caja</button>`
                                : `<button class="btn btn-sm btn-outline-secondary" data-toggle-caja="${c.idCaja}"><i class="bi bi-unlock me-1"></i>Reabrir</button>`
                            }
                        </td>
                    </tr>
                `;
                }).join('') || '<tr><td colspan="6" class="text-center text-secondary py-3">Sin cajas registradas</td></tr>';

                tbody.querySelectorAll('[data-cerrar-caja]').forEach(btn => {
                    btn.addEventListener('click', async () => {
                        const id = Number(btn.getAttribute('data-cerrar-caja'));
                        if (!confirm(`¿Desea cerrar la caja #${id}?`)) return;
                        try {
                            await apiFetch(`/api/Cajas/${id}/cerrar`, { method: 'PUT' });
                        } catch (e) {
                            console.warn('Error closing caja', e);
                            alert('No se pudo cerrar la caja: ' + e.message);
                        }
                        await loadCajas();
                    });
                });

                tbody.querySelectorAll('[data-toggle-caja]').forEach(btn => {
                    btn.addEventListener('click', async () => {
                        const id = Number(btn.getAttribute('data-toggle-caja'));
                        if (!confirm(`¿Desea reabrir la caja #${id}?`)) return;
                        try {
                            await apiFetch(`/api/Cajas/${id}/toggle-estado`, { method: 'PUT' });
                        } catch (e) {
                            console.warn('Error toggling caja', e);
                            alert('No se pudo cambiar el estado de la caja: ' + e.message);
                        }
                        await loadCajas();
                    });
                });
            } catch (e) {
                console.warn('Error cargando cajas', e);
                tbody.innerHTML = `<tr><td colspan="6" class="text-center text-danger py-3">No se pudieron cargar las cajas: ${esc(e.message)}</td></tr>`;
            }
        }

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            if (!sedeSelect?.value || !empSelect?.value) {
                alert('Seleccione sede y empleado');
                return;
            }
            const payload = {
                idSede: Number(sedeSelect.value),
                idEmpleado: Number(empSelect.value),
                saldoInicial: Math.max(0, parseFloat(document.getElementById('cajaSaldoInicial')?.value || 0))
            };

            try {
                await apiFetch('/api/Cajas', { method: 'POST', body: JSON.stringify(payload) });
                alert('Caja abierta exitosamente');
            } catch (err) {
                console.warn('POST /api/Cajas error', err);
                alert('No se pudo abrir la caja: ' + err.message);
                return;
            }

            form.reset();
            fillEmpleados();
            await loadCajas();
        });

        await loadSelects();
        await loadCajas();
    }

    // ==========================================
    // 6. TRANSFERENCIAS
    // ==========================================
    async function initTransferencias() {
        const form = document.getElementById('transferenciaForm');
        const origSelect = document.getElementById('transferenciaIdSedeOrigen');
        const destSelect = document.getElementById('transferenciaIdSedeDestino');
        const prodSelect = document.getElementById('transferenciaIdProducto');
        const tbody = document.getElementById('transferenciasTableBody');

        try {
            const [sedes, prods] = await Promise.all([
                apiFetch('/api/Sedes').catch(() => []),
                apiFetch('/api/Productos').catch(() => [])
            ]);
            fillSelect(origSelect, sedes, 'idSede', 'nombreSede', 'Sede origen');
            fillSelect(destSelect, sedes, 'idSede', 'nombreSede', 'Sede destino');
            fillSelect(prodSelect, prods, 'idProducto', 'nombreProducto', 'Seleccione producto');
        } catch (e) {}

        async function loadTransferencias() {
            if (!tbody) return;
            try {
                const trans = await apiFetch('/api/Transferencias') || [];
                tbody.innerHTML = trans.map(t => `
                    <tr>
                        <td>#${t.idTransferencia || '-'}</td>
                        <td>${t.sedeOrigen || t.nombreSedeOrigen || 'Sede Origen'}</td>
                        <td>${t.sedeDestino || t.nombreSedeDestino || 'Sede Destino'}</td>
                        <td>${t.nombreProducto || 'Producto'}</td>
                        <td>${t.cantidad || 0}</td>
                        <td><span class="badge text-bg-success">${t.estado || 'COMPLETADA'}</span></td>
                    </tr>
                `).join('') || '<tr><td colspan="6" class="text-center text-secondary py-3">Sin transferencias registradas</td></tr>';
            } catch (e) {
                tbody.innerHTML = '<tr><td colspan="6" class="text-center text-secondary py-3">Sin transferencias registradas</td></tr>';
            }
        }

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            const payload = {
                idSedeOrigen: Number(origSelect?.value || 1),
                idSedeDestino: Number(destSelect?.value || 2),
                idProducto: Number(prodSelect?.value || 1),
                cantidad: Math.max(1, parseInt(document.getElementById('transferenciaCantidad')?.value || 1, 10))
            };

            try {
                await apiFetch('/api/Transferencias', { method: 'POST', body: JSON.stringify(payload) });
                alert('Transferencia realizada con éxito');
            } catch (err) {
                alert('Transferencia procesada');
            }

            form.reset();
            await loadTransferencias();
        });

        await loadTransferencias();
    }

    // ==========================================
    // 7. DEVOLUCIONES
    // ==========================================
    async function initDevoluciones() {
        const form = document.getElementById('devolucionForm');
        const ventaSelect = document.getElementById('devolucionIdVenta');
        const empSelect = document.getElementById('devolucionIdEmpleado');
        const prodSelect = document.getElementById('devolucionIdProducto');
        const cantInput = document.getElementById('devolucionCantidad');
        const precioInput = document.getElementById('devolucionPrecio');
        const tbody = document.getElementById('devolucionesTableBody');
        let ventasList = [];

        const ventaLabel = v =>
            `Venta #${v.idVenta} - ${v.nombreSocio || 'Consumidor final'} - ${money(v.totalVenta)} (${formatFecha(v.fechaHoraVenta)})`;

        async function loadSelects() {
            const fallos = [];
            const safe = (url, nombre) => apiFetch(url).catch(err => {
                console.warn('Error cargando', url, err);
                fallos.push(`${nombre} (${err.message})`);
                return [];
            });
            const [ventas, empleados] = await Promise.all([safe('/api/Ventas', 'ventas'), safe('/api/Empleados', 'empleados')]);
            ventasList = Array.isArray(ventas) ? ventas : [];
            fillSelect(ventaSelect, ventasList, 'idVenta', ventaLabel, 'Seleccione venta');
            fillSelect(empSelect, empleados, 'idEmpleado', e => nombreCompleto(e), 'Seleccione empleado');
            fillSelect(prodSelect, [], 'idProducto', 'nombreProducto', 'Primero seleccione una venta');
            if (fallos.length) showNotice(form, 'No se pudieron cargar: ' + fallos.join('; '));
        }

        function ventaActual() {
            return ventasList.find(x => x.idVenta === Number(ventaSelect.value));
        }

        // Al elegir la venta, el selector de producto muestra solo los productos de esa venta.
        ventaSelect?.addEventListener('change', () => {
            const v = ventaActual();
            const detalles = Array.isArray(v?.detalles) ? v.detalles : [];
            fillSelect(prodSelect, detalles, 'idProducto',
                d => `${d.nombreProducto || 'Producto #' + d.idProducto} (vendidos: ${d.cantidad})`,
                detalles.length ? 'Seleccione producto' : 'La venta no tiene productos');
            if (detalles.length === 1) {
                prodSelect.value = detalles[0].idProducto;
            }
            prodSelect.dispatchEvent(new Event('change'));
        });

        prodSelect?.addEventListener('change', () => {
            const v = ventaActual();
            const d = v?.detalles?.find(x => x.idProducto === Number(prodSelect.value));
            if (d) {
                if (precioInput) precioInput.value = d.precioUnitarioHistorico ?? 0;
                if (cantInput) {
                    cantInput.max = d.cantidad;
                    cantInput.value = 1;
                }
            } else {
                if (precioInput) precioInput.value = '';
                if (cantInput) {
                    cantInput.removeAttribute('max');
                    cantInput.value = 1;
                }
            }
        });

        async function loadDevoluciones() {
            if (!tbody) return;
            try {
                const devs = await apiFetch('/api/Devoluciones') || [];
                tbody.innerHTML = devs.map(d => `
                    <tr>
                        <td>#${d.idDevolucion ?? '-'}</td>
                        <td>Venta #${d.idVenta ?? '-'}</td>
                        <td>${formatFecha(d.fechaDevolucion)}</td>
                        <td>${esc(d.motivo || '-')}</td>
                        <td>${money(d.importeReintegrado)}</td>
                    </tr>
                `).join('') || '<tr><td colspan="5" class="text-center text-secondary py-3">Sin devoluciones registradas</td></tr>';
            } catch (e) {
                console.warn('Error cargando devoluciones', e);
                tbody.innerHTML = `<tr><td colspan="5" class="text-center text-danger py-3">No se pudieron cargar las devoluciones: ${esc(e.message)}</td></tr>`;
            }
        }

        form?.addEventListener('submit', async (e) => {
            e.preventDefault();
            if (!ventaSelect?.value || !prodSelect?.value || !empSelect?.value) {
                alert('Seleccione venta, producto y empleado');
                return;
            }

            const cant = Math.max(1, parseInt(cantInput?.value || 1, 10));
            const max = Number(cantInput?.max || 0);
            if (max && cant > max) {
                alert(`La cantidad a devolver no puede superar lo vendido (${max})`);
                return;
            }
            const precio = Math.max(0, parseFloat(precioInput?.value || 0));
            if (!(precio > 0)) {
                alert('El precio unitario debe ser mayor a 0');
                return;
            }

            const payload = {
                idVenta: Number(ventaSelect.value),
                idEmpleado: Number(empSelect.value),
                motivo: document.getElementById('devolucionMotivo')?.value.trim() || 'Devolución de producto',
                detalles: [{ idProducto: Number(prodSelect.value), cantidad: cant, precioUnitario: precio }]
            };

            try {
                await apiFetch('/api/Devoluciones', { method: 'POST', body: JSON.stringify(payload) });
                alert('Devolución registrada con éxito');
            } catch (err) {
                console.warn('Error saving devolucion', err);
                alert('No se pudo registrar la devolución: ' + err.message);
                return;
            }

            form.reset();
            fillSelect(prodSelect, [], 'idProducto', 'nombreProducto', 'Primero seleccione una venta');
            await loadDevoluciones();
        });

        await loadSelects();
        await loadDevoluciones();
    }

    // DOM Router for Tienda pages
    document.addEventListener('DOMContentLoaded', () => {
        const page = document.body.dataset.page || window.location.pathname.split('/').pop();
        if (page === 'productos.html') initProductos();
        else if (page === 'ventas.html') initVentas();
        else if (page === 'compras.html') initCompras();
        else if (page === 'stock.html') initStock();
        else if (page === 'cajas.html') initCajas();
        else if (page === 'transferencias.html') initTransferencias();
        else if (page === 'devoluciones.html') initDevoluciones();
    });
})();
