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

  function getFallbackData(url) {
    const normalized = url.split('?')[0];
    const demo = {
      '/api/Productos': [
        { idProducto: 1, nombreProducto: 'Prote??na Whey', codigoBarras: '778899001', precioActual: 4200, stockMinimo: 15, activo: true, idCategoria: 1, nombreCategoria: 'Suplementos' },
        { idProducto: 2, nombreProducto: 'Mancuerna 12kg', codigoBarras: '778899002', precioActual: 6800, stockMinimo: 8, activo: true, idCategoria: 2, nombreCategoria: 'Accesorios' },
        { idProducto: 3, nombreProducto: 'Banda de resistencia', codigoBarras: '778899003', precioActual: 2600, stockMinimo: 12, activo: true, idCategoria: 2, nombreCategoria: 'Accesorios' }
      ],
      '/api/Compras': [
        { idCompra: 1, proveedor: 'Distribuidora Fit', fecha: '2026-11-05', total: 240000, estado: 'CONFIRMADA' },
        { idCompra: 2, proveedor: 'Power Supply', fecha: '2026-11-07', total: 98000, estado: 'PENDIENTE' }
      ],
      '/api/Ventas': [
        { idVenta: 1, fecha: '2026-11-10', total: 15450, estado: 'CONFIRMADA' },
        { idVenta: 2, fecha: '2026-11-10', total: 29300, estado: 'PENDIENTE' }
      ],
      '/api/Stock': [
        { idStock: 1, nombreProducto: 'Prote??na Whey', sede: 'Sede Centro', cantidad: 48, stockMinimo: 15 },
        { idStock: 2, nombreProducto: 'Mancuerna 12kg', sede: 'Sede Norte', cantidad: 17, stockMinimo: 8 }
      ],
      '/api/CategoriasProductos': [
        { idCategoria: 1, nombreCategoria: 'Suplementos' },
        { idCategoria: 2, nombreCategoria: 'Accesorios' },
        { idCategoria: 3, nombreCategoria: 'Ropa' }
      ],
      '/api/Equipos': [
        { idEquipo: 1, nombreMaquina: 'Cinta de correr', idSede: 1, nombreSede: 'Sede Centro', numeroSerie: 'EQ-1001', fechaAdquisicion: '2024-05-12', estadoOperativo: 'ACTIVO' },
        { idEquipo: 2, nombreMaquina: 'Bicicleta est??tica', idSede: 2, nombreSede: 'Sede Norte', numeroSerie: 'EQ-1002', fechaAdquisicion: '2023-09-03', estadoOperativo: 'EN MANTENIMIENTO' }
      ],
      '/api/Incidencias': [
        { idReclamo: 1, descripcion: 'Cinta de correr con vibraci??n', idSede: 1, nombreSede: 'Sede Centro', estadoReclamo: 'PENDIENTE', fechaRegistro: '2026-11-09' }
      ],
      '/api/Mantenimientos': [
        { idMantenimiento: 1, idEquipo: 1, descripcion: 'Cambio de correa', tecnico: 'Nicol??s', estado: 'PENDIENTE', fecha: '2026-11-12' }
      ],
      '/api/Proveedores': [
        { idProveedor: 1, razonSocial: 'Distribuidora Fit', cuit: '30-12345678-7', email: 'ventas@distribuidorafit.com', activo: true }
      ],
      '/api/Sedes': [
        { idSede: 1, nombreSede: 'Sede Centro' },
        { idSede: 2, nombreSede: 'Sede Norte' }
      ]
    };

    const saved = getDemoStore()[normalized];
    if (Array.isArray(saved) && saved.length) return saved;
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
                headers: { 'Content-Type': 'application/json', ...headers, Authorization: `Bearer ${token}` }
            });
            if (!response.ok) {
                const fallback = getFallbackData(url);
                if (fallback) return fallback;
                throw new Error(`API Error: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.warn('API Error:', error);
            const fallback = getFallbackData(url);
            if (fallback) return fallback;
            throw error;
        }
    }

    window.apiFetch = apiFetch;
})();