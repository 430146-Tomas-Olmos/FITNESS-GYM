USE master
GO

CREATE DATABASE FITLIFENETWORKdb
GO

USE FITLIFENETWORKdb
GO

SET DATEFORMAT dmy
GO

-- ----------------------------------------------------------------------------
-- CREACION DE TABLAS - FITLIFENETWORKdb
-- ----------------------------------------------------------------------------

-- 1. Tabla ROLES_SISTEMA (Roles para el control de acceso del sistema web)
CREATE TABLE ROLES_SISTEMA (
    id INT IDENTITY(1,1) NOT NULL,
    rol VARCHAR(50) NOT NULL,
    descripcion VARCHAR(255) NULL,
    CONSTRAINT PK_ROLES_SISTEMA PRIMARY KEY (id)
);
GO

-- ----------------------------------------------------------------------------
-- 2. Tabla USUARIOS (Autenticación web simplificada: usuario y contraseña)
CREATE TABLE USUARIOS (
    id INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(50) NOT NULL,
    contrasena VARCHAR(255) NOT NULL,
    CONSTRAINT PK_USUARIOS PRIMARY KEY (id)
);
GO

-- 3. Tabla ROLUSUARIO (Relación N:M entre usuarios y roles del sistema)
CREATE TABLE ROLUSUARIO (
    idusuario INT NOT NULL,
    idrol INT NOT NULL,
    CONSTRAINT PK_ROLUSUARIO PRIMARY KEY (idusuario, idrol),
    CONSTRAINT FK_ROLUSUARIO_USUARIOS FOREIGN KEY (idusuario) REFERENCES USUARIOS(id) ON DELETE CASCADE,
    CONSTRAINT FK_ROLUSUARIO_ROLES_SISTEMA FOREIGN KEY (idrol) REFERENCES ROLES_SISTEMA(id) ON DELETE CASCADE
);
GO

CREATE TABLE PROVINCIAS (
    id_provincia INT IDENTITY(1,1) NOT NULL,
    nombre_provincia VARCHAR(50) NOT NULL,
    CONSTRAINT PK_PROVINCIAS PRIMARY KEY (id_provincia)
);
GO

CREATE TABLE FORMAS_PAGO (
    id_forma_pago INT IDENTITY(1,1) NOT NULL,
    nombre_forma_pago VARCHAR(50) NOT NULL,
    CONSTRAINT PK_FORMAS_PAGO PRIMARY KEY (id_forma_pago)
);
GO

CREATE TABLE CONVENIOS_EMPRESA (
    id_convenio INT IDENTITY(1,1) NOT NULL,
    razon_social VARCHAR(100) NOT NULL,
    cuit VARCHAR(20) NOT NULL,
    porcentaje_descuento DECIMAL(5,2) NOT NULL,
    dia_cierre_facturacion INT NOT NULL,
    telefono VARCHAR(50) NULL,
    email VARCHAR(100) NULL,
    CONSTRAINT PK_CONVENIOS_EMPRESA PRIMARY KEY (id_convenio)
);
GO

CREATE TABLE PLANES (
    id_plan INT IDENTITY(1,1) NOT NULL,
    nombre_plan VARCHAR(50) NOT NULL,
    descripcion VARCHAR(255) NOT NULL,
    precio_actual DECIMAL(10,2) NOT NULL,
    duracion_meses INT NOT NULL,
    permite_multisede BIT NOT NULL,
    CONSTRAINT PK_PLANES PRIMARY KEY (id_plan)
);
GO

CREATE TABLE DISCIPLINAS (
    id_disciplina INT IDENTITY(1,1) NOT NULL,
    nombre_disciplina VARCHAR(50) NOT NULL,
    descripcion VARCHAR(255) NOT NULL,
    CONSTRAINT PK_DISCIPLINAS PRIMARY KEY (id_disciplina)
);
GO

CREATE TABLE CATEGORIAS_PRODUCTOS (
    id_categoria INT IDENTITY(1,1) NOT NULL,
    nombre_categoria VARCHAR(50) NOT NULL,
    CONSTRAINT PK_CATEGORIAS_PRODUCTOS PRIMARY KEY (id_categoria)
);
GO

CREATE TABLE ROLES (
    id_rol INT IDENTITY(1,1) NOT NULL,
    nombre_rol VARCHAR(50) NOT NULL,
    CONSTRAINT PK_ROLES PRIMARY KEY (id_rol)
);
GO

CREATE TABLE ESTADOS_TURNO (
    id_estado_turno INT IDENTITY(1,1) NOT NULL,
    nombre_estado VARCHAR(50) NOT NULL,
    CONSTRAINT PK_ESTADOS_TURNO PRIMARY KEY (id_estado_turno)
);
GO

CREATE TABLE ESTADOS_RECLAMO (
    id_estado_reclamo INT IDENTITY(1,1) NOT NULL,
    nombre_estado VARCHAR(50) NOT NULL,
    CONSTRAINT PK_ESTADOS_RECLAMO PRIMARY KEY (id_estado_reclamo)
);
GO

CREATE TABLE PREMIOS_CATALOGO (
    id_premio INT IDENTITY(1,1) NOT NULL,
    nombre_premio VARCHAR(100) NOT NULL,
    descripcion VARCHAR(255) NOT NULL,
    meses_antiguedad_requeridos INT NOT NULL,
    CONSTRAINT PK_PREMIOS_CATALOGO PRIMARY KEY (id_premio)
);
GO

CREATE TABLE PROVEEDORES (
    id_proveedor INT IDENTITY(1,1) NOT NULL,
    razon_social VARCHAR(100) NOT NULL,
    cuit VARCHAR(20) NOT NULL,
    telefono VARCHAR(50) NULL,
    email VARCHAR(100) NULL,
    CONSTRAINT PK_PROVEEDORES PRIMARY KEY (id_proveedor)
);
GO

CREATE TABLE EJERCICIOS (
    id_ejercicio INT IDENTITY(1,1) NOT NULL,
    nombre_ejercicio VARCHAR(100) NOT NULL,
    grupo_muscular VARCHAR(50) NOT NULL,
    descripcion VARCHAR(255) NOT NULL,
    CONSTRAINT PK_EJERCICIOS PRIMARY KEY (id_ejercicio)
);
GO

CREATE TABLE PERSONAS (
    id_persona INT IDENTITY(1,1) NOT NULL,
    dni VARCHAR(20) NOT NULL,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    estado_civil VARCHAR(20) NOT NULL,
    ocupacion VARCHAR(50) NOT NULL,
    telefono VARCHAR(50) NULL,
    email VARCHAR(100) NULL,
    CONSTRAINT PK_PERSONAS PRIMARY KEY (id_persona)
);
GO

CREATE TABLE LOCALIDADES (
    id_localidad INT IDENTITY(1,1) NOT NULL,
    id_provincia INT NOT NULL,
    nombre_localidad VARCHAR(50) NOT NULL,
    CONSTRAINT PK_LOCALIDADES PRIMARY KEY (id_localidad),
    CONSTRAINT LOCALIDADES_x_PROVINCIAS FOREIGN KEY (id_provincia) REFERENCES PROVINCIAS(id_provincia)
);
GO

CREATE TABLE BARRIOS (
    id_barrio INT IDENTITY(1,1) NOT NULL,
    id_localidad INT NOT NULL,
    nombre_barrio VARCHAR(50) NOT NULL,
    CONSTRAINT PK_BARRIOS PRIMARY KEY (id_barrio),
    CONSTRAINT BARRIOS_x_LOCALIDADES FOREIGN KEY (id_localidad) REFERENCES LOCALIDADES(id_localidad)
);
GO

CREATE TABLE CALLES (
    id_calle INT IDENTITY(1,1) NOT NULL,
    id_barrio INT NOT NULL,
    nombre_calle VARCHAR(100) NOT NULL,
    numeracion INT NOT NULL,
    CONSTRAINT PK_CALLES PRIMARY KEY (id_calle),
    CONSTRAINT CALLES_x_BARRIOS FOREIGN KEY (id_barrio) REFERENCES BARRIOS(id_barrio)
);
GO

CREATE TABLE HISTORIAL_PRECIOS_PLANES (
    id_historial_plan INT IDENTITY(1,1) NOT NULL,
    id_plan INT NOT NULL,
    precio_anterior DECIMAL(10,2) NOT NULL,
    precio_nuevo DECIMAL(10,2) NOT NULL,
    fecha_cambio DATETIME NOT NULL,
    CONSTRAINT PK_HISTORIAL_PRECIOS_PLANES PRIMARY KEY (id_historial_plan),
    CONSTRAINT HISTORIAL_PRECIOS_PLANES_x_PLANES FOREIGN KEY (id_plan) REFERENCES PLANES(id_plan)
);
GO

CREATE TABLE PRODUCTOS (
    id_producto INT IDENTITY(1,1) NOT NULL,
    id_categoria INT NOT NULL,
    nombre_producto VARCHAR(100) NOT NULL,
    codigo_barras VARCHAR(50) NOT NULL,
    precio_actual DECIMAL(10,2) NOT NULL,
    stock_minimo INT NOT NULL,
    CONSTRAINT PK_PRODUCTOS PRIMARY KEY (id_producto),
    CONSTRAINT PRODUCTOS_x_CATEGORIAS_PRODUCTOS FOREIGN KEY (id_categoria) REFERENCES CATEGORIAS_PRODUCTOS(id_categoria)
);
GO

CREATE TABLE HISTORIAL_PRECIOS_PRODUCTOS (
    id_historial_prod INT IDENTITY(1,1) NOT NULL,
    id_producto INT NOT NULL,
    precio_anterior DECIMAL(10,2) NOT NULL,
    precio_nuevo DECIMAL(10,2) NOT NULL,
    fecha_cambio DATETIME NOT NULL,
    CONSTRAINT PK_HISTORIAL_PRECIOS_PRODUCTOS PRIMARY KEY (id_historial_prod),
    CONSTRAINT HISTORIAL_PRECIOS_PRODUCTOS_x_PRODUCTOS FOREIGN KEY (id_producto) REFERENCES PRODUCTOS(id_producto)
);
GO

CREATE TABLE SEDES (
    id_sede INT IDENTITY(1,1) NOT NULL,
    id_calle INT NOT NULL,
    nombre_sede VARCHAR(100) NOT NULL,
    telefono VARCHAR(50) NULL,
    email VARCHAR(100) NULL,
    CONSTRAINT PK_SEDES PRIMARY KEY (id_sede),
    CONSTRAINT SEDES_x_CALLES FOREIGN KEY (id_calle) REFERENCES CALLES(id_calle)
);
GO

CREATE TABLE HORARIOS_SEDE (
    id_horario_sede INT IDENTITY(1,1) NOT NULL,
    id_sede INT NOT NULL,
    dia_semana VARCHAR(20) NOT NULL,
    hora_apertura TIME NOT NULL,
    hora_cierre TIME NOT NULL,
    CONSTRAINT PK_HORARIOS_SEDE PRIMARY KEY (id_horario_sede),
    CONSTRAINT HORARIOS_SEDE_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede)
);
GO

CREATE TABLE SALONES (
    id_salon INT IDENTITY(1,1) NOT NULL,
    id_sede INT NOT NULL,
    nombre_salon VARCHAR(50) NOT NULL,
    capacidad_maxima INT NOT NULL,
    CONSTRAINT PK_SALONES PRIMARY KEY (id_salon),
    CONSTRAINT SALONES_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede)
);
GO

CREATE TABLE EQUIPAMIENTO (
    id_equipo INT IDENTITY(1,1) NOT NULL,
    id_sede INT NOT NULL,
    nombre_maquina VARCHAR(100) NOT NULL,
    numero_serie VARCHAR(50) NOT NULL,
    fecha_adquisicion DATE NOT NULL,
    estado_operativo VARCHAR(50) NOT NULL,
    CONSTRAINT PK_EQUIPAMIENTO PRIMARY KEY (id_equipo),
    CONSTRAINT EQUIPAMIENTO_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede)
);
GO

CREATE TABLE MANTENIMIENTO_EQUIPAMIENTO (
    id_mantenimiento INT IDENTITY(1,1) NOT NULL,
    id_equipo INT NOT NULL,
    id_proveedor INT NULL,
    fecha_mantenimiento DATE NOT NULL,
    descripcion VARCHAR(255) NOT NULL,
    costo DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_MANTENIMIENTO_EQUIPAMIENTO PRIMARY KEY (id_mantenimiento),
    CONSTRAINT MANTENIMIENTO_EQUIPAMIENTO_x_EQUIPAMIENTO FOREIGN KEY (id_equipo) REFERENCES EQUIPAMIENTO(id_equipo),
    CONSTRAINT MANTENIMIENTO_EQUIPAMIENTO_x_PROVEEDORES FOREIGN KEY (id_proveedor) REFERENCES PROVEEDORES(id_proveedor)
);
GO

CREATE TABLE STOCK_SEDE (
    id_stock_sede INT IDENTITY(1,1) NOT NULL,
    id_sede INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad_actual INT NOT NULL,
    fila_version ROWVERSION,
    CONSTRAINT PK_STOCK_SEDE PRIMARY KEY (id_stock_sede),
    CONSTRAINT STOCK_SEDE_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede),
    CONSTRAINT STOCK_SEDE_x_PRODUCTOS FOREIGN KEY (id_producto) REFERENCES PRODUCTOS(id_producto)
);
GO

CREATE TABLE COMPRAS_STOCK (
    id_compra INT IDENTITY(1,1) NOT NULL,
    id_proveedor INT NOT NULL,
    id_sede INT NOT NULL,
    fecha_compra DATETIME NOT NULL,
    CONSTRAINT PK_COMPRAS_STOCK PRIMARY KEY (id_compra),
    CONSTRAINT COMPRAS_STOCK_x_PROVEEDORES FOREIGN KEY (id_proveedor) REFERENCES PROVEEDORES(id_proveedor),
    CONSTRAINT COMPRAS_STOCK_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede)
);
GO

CREATE TABLE DETALLE_COMPRAS_STOCK (
    id_detalle_compra INT IDENTITY(1,1) NOT NULL,
    id_compra INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_costo_unitario DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_DETALLE_COMPRAS_STOCK PRIMARY KEY (id_detalle_compra),
    CONSTRAINT DETALLE_COMPRAS_STOCK_x_COMPRAS_STOCK FOREIGN KEY (id_compra) REFERENCES COMPRAS_STOCK(id_compra),
    CONSTRAINT DETALLE_COMPRAS_STOCK_x_PRODUCTOS FOREIGN KEY (id_producto) REFERENCES PRODUCTOS(id_producto)
);
GO

CREATE TABLE SOCIOS (
    id_socio INT IDENTITY(1,1) NOT NULL,
    id_persona INT NOT NULL,
    id_calle INT NOT NULL,
    id_convenio INT NULL,
    fecha_ingreso_alta DATE NOT NULL,
    estado_cuenta VARCHAR(20) NOT NULL,
    CONSTRAINT PK_SOCIOS PRIMARY KEY (id_socio),
    CONSTRAINT SOCIOS_x_PERSONAS FOREIGN KEY (id_persona) REFERENCES PERSONAS(id_persona),
    CONSTRAINT SOCIOS_x_CALLES FOREIGN KEY (id_calle) REFERENCES CALLES(id_calle),
    CONSTRAINT SOCIOS_x_CONVENIOS_EMPRESA FOREIGN KEY (id_convenio) REFERENCES CONVENIOS_EMPRESA(id_convenio)
);
GO

CREATE TABLE EMPLEADOS (
    id_empleado INT IDENTITY(1,1) NOT NULL,
    id_persona INT NOT NULL,
    id_sede INT NOT NULL,
    id_rol INT NOT NULL,
    legajo VARCHAR(20) NOT NULL,
    fecha_ingreso DATE NOT NULL,
    sueldo_base DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_EMPLEADOS PRIMARY KEY (id_empleado),
    CONSTRAINT EMPLEADOS_x_PERSONAS FOREIGN KEY (id_persona) REFERENCES PERSONAS(id_persona),
    CONSTRAINT EMPLEADOS_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede),
    CONSTRAINT EMPLEADOS_x_ROLES FOREIGN KEY (id_rol) REFERENCES ROLES(id_rol)
);
GO

CREATE TABLE LIQUIDACIONES_SUELDOS (
    id_liquidacion INT IDENTITY(1,1) NOT NULL,
    id_empleado INT NOT NULL,
    periodo_mes INT NOT NULL,
    periodo_anio INT NOT NULL,
    fecha_pago DATE NOT NULL,
    monto_neto DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_LIQUIDACIONES_SUELDOS PRIMARY KEY (id_liquidacion),
    CONSTRAINT LIQUIDACIONES_SUELDOS_x_EMPLEADOS FOREIGN KEY (id_empleado) REFERENCES EMPLEADOS(id_empleado)
);
GO

CREATE TABLE PLANES_DISCIPLINAS (
    id_plan_disciplina INT IDENTITY(1,1) NOT NULL,
    id_plan INT NOT NULL,
    id_disciplina INT NOT NULL,
    CONSTRAINT PK_PLANES_DISCIPLINAS PRIMARY KEY (id_plan_disciplina),
    CONSTRAINT PLANES_DISCIPLINAS_x_PLANES FOREIGN KEY (id_plan) REFERENCES PLANES(id_plan),
    CONSTRAINT PLANES_DISCIPLINAS_x_DISCIPLINAS FOREIGN KEY (id_disciplina) REFERENCES DISCIPLINAS(id_disciplina)
);
GO

CREATE TABLE EMPLEADOS_DISCIPLINAS (
    id_empleado_disciplina INT IDENTITY(1,1) NOT NULL,
    id_empleado INT NOT NULL,
    id_disciplina INT NOT NULL,
    CONSTRAINT PK_EMPLEADOS_DISCIPLINAS PRIMARY KEY (id_empleado_disciplina),
    CONSTRAINT EMPLEADOS_DISCIPLINAS_x_EMPLEADOS FOREIGN KEY (id_empleado) REFERENCES EMPLEADOS(id_empleado),
    CONSTRAINT EMPLEADOS_DISCIPLINAS_x_DISCIPLINAS FOREIGN KEY (id_disciplina) REFERENCES DISCIPLINAS(id_disciplina)
);
GO

CREATE TABLE APTOS_MEDICOS (
    id_apto INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    fecha_emision DATE NOT NULL,
    fecha_vencimiento DATE NOT NULL,
    medico_firmante VARCHAR(100) NOT NULL,
    matricula_medico VARCHAR(50) NOT NULL,
    observaciones VARCHAR(255) NOT NULL,
    CONSTRAINT PK_APTOS_MEDICOS PRIMARY KEY (id_apto),
    CONSTRAINT APTOS_MEDICOS_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio)
);
GO

CREATE TABLE PREMIOS_OTORGADOS (
    id_otorgamiento INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    id_premio INT NOT NULL,
    fecha_otorgamiento DATETIME NOT NULL,
    estado_canje VARCHAR(20) NOT NULL,
    CONSTRAINT PK_PREMIOS_OTORGADOS PRIMARY KEY (id_otorgamiento),
    CONSTRAINT PREMIOS_OTORGADOS_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT PREMIOS_OTORGADOS_x_PREMIOS_CATALOGO FOREIGN KEY (id_premio) REFERENCES PREMIOS_CATALOGO(id_premio)
);
GO

CREATE TABLE LOG_CAMBIOS_ESTADO_SOCIO (
    id_log_estado INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    estado_anterior VARCHAR(20) NOT NULL,
    estado_nuevo VARCHAR(20) NOT NULL,
    fecha_cambio DATETIME NOT NULL,
    usuario_sistema VARCHAR(50) NOT NULL,
    CONSTRAINT PK_LOG_CAMBIOS_ESTADO_SOCIO PRIMARY KEY (id_log_estado),
    CONSTRAINT LOG_CAMBIOS_ESTADO_SOCIO_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio)
);
GO

CREATE TABLE CONTRATOS_MEMBRESIA (
    id_contrato INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    id_plan INT NOT NULL,
    id_sede INT NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_vencimiento DATE NOT NULL,
    estado_contrato VARCHAR(20) NOT NULL,
    fila_version ROWVERSION,
    CONSTRAINT PK_CONTRATOS_MEMBRESIA PRIMARY KEY (id_contrato),
    CONSTRAINT CONTRATOS_MEMBRESIA_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT CONTRATOS_MEMBRESIA_x_PLANES FOREIGN KEY (id_plan) REFERENCES PLANES(id_plan),
    CONSTRAINT CONTRATOS_MEMBRESIA_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede)
);
GO

CREATE TABLE CLASES (
    id_clase INT IDENTITY(1,1) NOT NULL,
    id_salon INT NOT NULL,
    id_disciplina INT NOT NULL,
    id_instructor INT NOT NULL,
    fecha_clase DATE NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    cupo_limite INT NOT NULL,
    CONSTRAINT PK_CLASES PRIMARY KEY (id_clase),
    CONSTRAINT CLASES_x_SALONES FOREIGN KEY (id_salon) REFERENCES SALONES(id_salon),
    CONSTRAINT CLASES_x_DISCIPLINAS FOREIGN KEY (id_disciplina) REFERENCES DISCIPLINAS(id_disciplina),
    CONSTRAINT CLASES_x_EMPLEADOS FOREIGN KEY (id_instructor) REFERENCES EMPLEADOS(id_empleado)
);
GO

CREATE TABLE RUTINAS (
    id_rutina INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    id_entrenador INT NOT NULL,
    fecha_creacion DATE NOT NULL,
    objetivo VARCHAR(255) NOT NULL,
    CONSTRAINT PK_RUTINAS PRIMARY KEY (id_rutina),
    CONSTRAINT RUTINAS_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT RUTINAS_x_EMPLEADOS FOREIGN KEY (id_entrenador) REFERENCES EMPLEADOS(id_empleado)
);
GO

CREATE TABLE CUOTAS_COBROS (
    id_cobro INT IDENTITY(1,1) NOT NULL,
    id_contrato INT NOT NULL,
    id_forma_pago INT NOT NULL,
    numero_cuota INT NOT NULL,
    fecha_vencimiento DATE NOT NULL,
    fecha_pago DATETIME NULL,
    monto DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_CUOTAS_COBROS PRIMARY KEY (id_cobro),
    CONSTRAINT CUOTAS_COBROS_x_CONTRATOS_MEMBRESIA FOREIGN KEY (id_contrato) REFERENCES CONTRATOS_MEMBRESIA(id_contrato),
    CONSTRAINT CUOTAS_COBROS_x_FORMAS_PAGO FOREIGN KEY (id_forma_pago) REFERENCES FORMAS_PAGO(id_forma_pago)
);
GO

CREATE TABLE ACCESOS_LOG (
    id_acceso INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    id_sede INT NOT NULL,
    fecha_hora_marcado DATETIME NOT NULL,
    acceso_permitido BIT NOT NULL,
    motivo_rechazo VARCHAR(100) NULL,
    CONSTRAINT PK_ACCESOS_LOG PRIMARY KEY (id_acceso),
    CONSTRAINT ACCESOS_LOG_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT ACCESOS_LOG_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede)
);
GO

CREATE TABLE LOG_ACCESOS_DENEGADOS (
    id_log INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    id_sede INT NOT NULL,
    fecha_hora DATETIME NOT NULL,
    motivo_bloqueo VARCHAR(100) NOT NULL,
    CONSTRAINT PK_LOG_ACCESOS_DENEGADOS PRIMARY KEY (id_log),
    CONSTRAINT LOG_ACCESOS_DENEGADOS_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT LOG_ACCESOS_DENEGADOS_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede)
);
GO

CREATE TABLE RESERVAS (
    id_reserva INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    id_clase INT NOT NULL,
    fecha_reserva DATETIME NOT NULL,
    estado_reserva VARCHAR(20) NOT NULL,
    CONSTRAINT PK_RESERVAS PRIMARY KEY (id_reserva),
    CONSTRAINT RESERVAS_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT RESERVAS_x_CLASES FOREIGN KEY (id_clase) REFERENCES CLASES(id_clase)
);
GO

CREATE TABLE ASISTENCIAS_CLASES (
    id_asistencia INT IDENTITY(1,1) NOT NULL,
    id_reserva INT NOT NULL,
    fecha_hora_asistencia DATETIME NOT NULL,
    presente BIT NOT NULL,
    CONSTRAINT PK_ASISTENCIAS_CLASES PRIMARY KEY (id_asistencia),
    CONSTRAINT ASISTENCIAS_CLASES_x_RESERVAS FOREIGN KEY (id_reserva) REFERENCES RESERVAS(id_reserva)
);
GO

CREATE TABLE TURNOS (
    id_turno INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    id_profesional INT NOT NULL,
    id_sede INT NOT NULL,
    id_estado_turno INT NOT NULL,
    fecha_turno DATE NOT NULL,
    hora_turno TIME NOT NULL,
    CONSTRAINT PK_TURNOS PRIMARY KEY (id_turno),
    CONSTRAINT TURNOS_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT TURNOS_x_EMPLEADOS FOREIGN KEY (id_profesional) REFERENCES EMPLEADOS(id_empleado),
    CONSTRAINT TURNOS_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede),
    CONSTRAINT TURNOS_x_ESTADOS_TURNO FOREIGN KEY (id_estado_turno) REFERENCES ESTADOS_TURNO(id_estado_turno)
);
GO

CREATE TABLE VENTAS (
    id_venta INT IDENTITY(1,1) NOT NULL,
    id_socio INT NULL,
    id_sede INT NOT NULL,
    id_vendedor INT NOT NULL,
    id_forma_pago INT NOT NULL,
    fecha_hora_venta DATETIME NOT NULL,
    imputado_a_cuenta BIT NOT NULL,
    CONSTRAINT PK_VENTAS PRIMARY KEY (id_venta),
    CONSTRAINT VENTAS_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT VENTAS_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede),
    CONSTRAINT VENTAS_x_EMPLEADOS FOREIGN KEY (id_vendedor) REFERENCES EMPLEADOS(id_empleado),
    CONSTRAINT VENTAS_x_FORMAS_PAGO FOREIGN KEY (id_forma_pago) REFERENCES FORMAS_PAGO(id_forma_pago)
);
GO

CREATE TABLE DETALLE_RUTINAS (
    id_detalle_rutina INT IDENTITY(1,1) NOT NULL,
    id_rutina INT NOT NULL,
    id_ejercicio INT NOT NULL,
    series INT NOT NULL,
    repeticiones INT NOT NULL,
    descanso_segundos INT NOT NULL,
    CONSTRAINT PK_DETALLE_RUTINAS PRIMARY KEY (id_detalle_rutina),
    CONSTRAINT DETALLE_RUTINAS_x_RUTINAS FOREIGN KEY (id_rutina) REFERENCES RUTINAS(id_rutina),
    CONSTRAINT DETALLE_RUTINAS_x_EJERCICIOS FOREIGN KEY (id_ejercicio) REFERENCES EJERCICIOS(id_ejercicio)
);
GO

CREATE TABLE EVALUACIONES_FISICAS (
    id_evaluacion INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    id_turno INT NOT NULL,
    peso_kg DECIMAL(5,2) NOT NULL,
    porcentaje_grasa DECIMAL(5,2) NOT NULL,
    porcentaje_masa_muscular DECIMAL(5,2) NOT NULL,
    objetivos_asignados VARCHAR(255) NOT NULL,
    plan_alimentario VARCHAR(255) NOT NULL,
    CONSTRAINT PK_EVALUACIONES_FISICAS PRIMARY KEY (id_evaluacion),
    CONSTRAINT EVALUACIONES_FISICAS_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT EVALUACIONES_FISICAS_x_TURNOS FOREIGN KEY (id_turno) REFERENCES TURNOS(id_turno)
);
GO

CREATE TABLE DETALLE_VENTAS (
    id_detalle_venta INT IDENTITY(1,1) NOT NULL,
    id_venta INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario_historico DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_DETALLE_VENTAS PRIMARY KEY (id_detalle_venta),
    CONSTRAINT DETALLE_VENTAS_x_VENTAS FOREIGN KEY (id_venta) REFERENCES VENTAS(id_venta),
    CONSTRAINT DETALLE_VENTAS_x_PRODUCTOS FOREIGN KEY (id_producto) REFERENCES PRODUCTOS(id_producto)
);
GO

CREATE TABLE INCIDENCIAS_RECLAMOS (
    id_reclamo INT IDENTITY(1,1) NOT NULL,
    id_socio INT NOT NULL,
    id_equipo INT NULL,
    id_sede INT NOT NULL,
    id_tecnico_asignado INT NULL,
    id_estado_reclamo INT NOT NULL,
    fecha_registro DATETIME NOT NULL,
    descripcion VARCHAR(255) NOT NULL,
    CONSTRAINT PK_INCIDENCIAS_RECLAMOS PRIMARY KEY (id_reclamo),
    CONSTRAINT INCIDENCIAS_RECLAMOS_x_SOCIOS FOREIGN KEY (id_socio) REFERENCES SOCIOS(id_socio),
    CONSTRAINT INCIDENCIAS_RECLAMOS_x_EQUIPAMIENTO FOREIGN KEY (id_equipo) REFERENCES EQUIPAMIENTO(id_equipo),
    CONSTRAINT INCIDENCIAS_RECLAMOS_x_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede),
    CONSTRAINT INCIDENCIAS_RECLAMOS_x_EMPLEADOS FOREIGN KEY (id_tecnico_asignado) REFERENCES EMPLEADOS(id_empleado),
    CONSTRAINT INCIDENCIAS_RECLAMOS_x_ESTADOS_RECLAMO FOREIGN KEY (id_estado_reclamo) REFERENCES ESTADOS_RECLAMO(id_estado_reclamo)
);
GO

-- ESTRUCTURAS DE SOPORTE, RESTRICCIONES E INDICES
-- ----------------------------------------------------------------------------

-- Tabla auxiliar: PAGOS_CUOTAS. Registra pagos parciales y el historial de cobros.
    CREATE TABLE PAGOS_CUOTAS (
        id_pago INT IDENTITY(1,1) NOT NULL,
        id_cobro INT NOT NULL,
        id_forma_pago INT NOT NULL,
        importe DECIMAL(10,2) NOT NULL,
        fecha_pago DATETIME NOT NULL CONSTRAINT DF_PAGOS_CUOTAS_FECHA DEFAULT GETDATE(),
        usuario_sistema VARCHAR(50) NULL,
        CONSTRAINT PK_PAGOS_CUOTAS PRIMARY KEY (id_pago),
        CONSTRAINT FK_PAGOS_CUOTAS_CUOTAS FOREIGN KEY (id_cobro) REFERENCES CUOTAS_COBROS(id_cobro),
        CONSTRAINT FK_PAGOS_CUOTAS_FORMAS FOREIGN KEY (id_forma_pago) REFERENCES FORMAS_PAGO(id_forma_pago),
        CONSTRAINT CK_PAGOS_CUOTAS_IMPORTE CHECK (importe > 0)
    );
GO

-- ----------------------------------------------------------------------------
-- Tabla auxiliar: MOVIMIENTOS_STOCK. Guarda informacion necesaria para la trazabilidad.
    CREATE TABLE MOVIMIENTOS_STOCK (
        id_movimiento INT IDENTITY(1,1) NOT NULL,
        id_sede INT NOT NULL,
        id_producto INT NOT NULL,
        tipo_movimiento VARCHAR(30) NOT NULL,
        cantidad INT NOT NULL,
        fecha_movimiento DATETIME NOT NULL CONSTRAINT DF_MOVIMIENTOS_STOCK_FECHA DEFAULT GETDATE(),
        id_venta INT NULL,
        usuario_sistema VARCHAR(50) NULL,
        observaciones VARCHAR(255) NULL,
        CONSTRAINT PK_MOVIMIENTOS_STOCK PRIMARY KEY (id_movimiento),
        CONSTRAINT FK_MOVIMIENTOS_STOCK_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede),
        CONSTRAINT FK_MOVIMIENTOS_STOCK_PRODUCTOS FOREIGN KEY (id_producto) REFERENCES PRODUCTOS(id_producto),
        CONSTRAINT FK_MOVIMIENTOS_STOCK_VENTAS FOREIGN KEY (id_venta) REFERENCES VENTAS(id_venta),
        CONSTRAINT CK_MOVIMIENTOS_STOCK_CANTIDAD CHECK (cantidad <> 0)
    );
GO

-- Tabla auxiliar: DEVOLUCIONES_VENTA. Guarda informacion necesaria para la trazabilidad.
    CREATE TABLE DEVOLUCIONES_VENTA (
        id_devolucion INT IDENTITY(1,1) NOT NULL,
        id_venta INT NOT NULL,
        fecha_devolucion DATETIME NOT NULL CONSTRAINT DF_DEVOLUCIONES_VENTA_FECHA DEFAULT GETDATE(),
        motivo VARCHAR(255) NOT NULL,
        id_empleado INT NOT NULL,
        importe_reintegrado DECIMAL(10,2) NOT NULL,
        CONSTRAINT PK_DEVOLUCIONES_VENTA PRIMARY KEY (id_devolucion),
        CONSTRAINT FK_DEVOLUCIONES_VENTA_VENTAS FOREIGN KEY (id_venta) REFERENCES VENTAS(id_venta),
        CONSTRAINT FK_DEVOLUCIONES_VENTA_EMPLEADOS FOREIGN KEY (id_empleado) REFERENCES EMPLEADOS(id_empleado),
        CONSTRAINT CK_DEVOLUCIONES_VENTA_IMPORTE CHECK (importe_reintegrado >= 0)
    );
GO

-- Tabla auxiliar: DETALLE_DEVOLUCIONES_VENTA. Guarda informacion necesaria para la trazabilidad.
    CREATE TABLE DETALLE_DEVOLUCIONES_VENTA (
        id_detalle_devolucion INT IDENTITY(1,1) NOT NULL,
        id_devolucion INT NOT NULL,
        id_producto INT NOT NULL,
        cantidad INT NOT NULL,
        precio_unitario DECIMAL(10,2) NOT NULL,
        CONSTRAINT PK_DETALLE_DEVOLUCIONES_VENTA PRIMARY KEY (id_detalle_devolucion),
        CONSTRAINT FK_DETALLE_DEVOLUCIONES_DEVOLUCIONES FOREIGN KEY (id_devolucion) REFERENCES DEVOLUCIONES_VENTA(id_devolucion),
        CONSTRAINT FK_DETALLE_DEVOLUCIONES_PRODUCTOS FOREIGN KEY (id_producto) REFERENCES PRODUCTOS(id_producto),
        CONSTRAINT CK_DETALLE_DEVOLUCIONES_CANTIDAD CHECK (cantidad > 0)
    );
GO

-- Tabla auxiliar: TRANSFERENCIAS_STOCK. Guarda informacion necesaria para la trazabilidad.
    CREATE TABLE TRANSFERENCIAS_STOCK (
        id_transferencia INT IDENTITY(1,1) NOT NULL,
        id_sede_origen INT NOT NULL,
        id_sede_destino INT NOT NULL,
        fecha_solicitud DATETIME NOT NULL CONSTRAINT DF_TRANSFERENCIAS_FECHA DEFAULT GETDATE(),
        estado VARCHAR(20) NOT NULL CONSTRAINT DF_TRANSFERENCIAS_ESTADO DEFAULT 'PENDIENTE',
        usuario_sistema VARCHAR(50) NULL,
        CONSTRAINT PK_TRANSFERENCIAS_STOCK PRIMARY KEY (id_transferencia),
        CONSTRAINT FK_TRANSFERENCIAS_ORIGEN FOREIGN KEY (id_sede_origen) REFERENCES SEDES(id_sede),
        CONSTRAINT FK_TRANSFERENCIAS_DESTINO FOREIGN KEY (id_sede_destino) REFERENCES SEDES(id_sede),
        CONSTRAINT CK_TRANSFERENCIAS_SEDES CHECK (id_sede_origen <> id_sede_destino),
        CONSTRAINT CK_TRANSFERENCIAS_ESTADO CHECK (estado IN ('PENDIENTE','EN TRANSITO','RECIBIDA','CANCELADA'))
    );
GO

-- Tabla auxiliar: DETALLE_TRANSFERENCIAS. Guarda informacion necesaria para la trazabilidad.
    CREATE TABLE DETALLE_TRANSFERENCIAS (
        id_detalle_transferencia INT IDENTITY(1,1) NOT NULL,
        id_transferencia INT NOT NULL,
        id_producto INT NOT NULL,
        cantidad INT NOT NULL,
        CONSTRAINT PK_DETALLE_TRANSFERENCIAS PRIMARY KEY (id_detalle_transferencia),
        CONSTRAINT FK_DETALLE_TRANSFERENCIAS_TRANSFERENCIAS FOREIGN KEY (id_transferencia) REFERENCES TRANSFERENCIAS_STOCK(id_transferencia),
        CONSTRAINT FK_DETALLE_TRANSFERENCIAS_PRODUCTOS FOREIGN KEY (id_producto) REFERENCES PRODUCTOS(id_producto),
        CONSTRAINT CK_DETALLE_TRANSFERENCIAS_CANTIDAD CHECK (cantidad > 0)
    );
GO

-- Tabla auxiliar: CAJAS. Guarda informacion necesaria para la trazabilidad.
    CREATE TABLE CAJAS (
        id_caja INT IDENTITY(1,1) NOT NULL,
        id_sede INT NOT NULL,
        id_empleado INT NOT NULL,
        fecha_apertura DATETIME NOT NULL CONSTRAINT DF_CAJAS_APERTURA DEFAULT GETDATE(),
        saldo_inicial DECIMAL(10,2) NOT NULL,
        fecha_cierre DATETIME NULL,
        estado VARCHAR(20) NOT NULL CONSTRAINT DF_CAJAS_ESTADO DEFAULT 'ABIERTA',
        CONSTRAINT PK_CAJAS PRIMARY KEY (id_caja),
        CONSTRAINT FK_CAJAS_SEDES FOREIGN KEY (id_sede) REFERENCES SEDES(id_sede),
        CONSTRAINT FK_CAJAS_EMPLEADOS FOREIGN KEY (id_empleado) REFERENCES EMPLEADOS(id_empleado),
        CONSTRAINT CK_CAJAS_SALDO CHECK (saldo_inicial >= 0),
        CONSTRAINT CK_CAJAS_ESTADO CHECK (estado IN ('ABIERTA','CERRADA'))
    );
GO

-- Tabla auxiliar: CIERRES_CAJA. Guarda informacion necesaria para la trazabilidad.
    CREATE TABLE CIERRES_CAJA (
        id_cierre INT IDENTITY(1,1) NOT NULL,
        id_caja INT NOT NULL,
        fecha_cierre DATETIME NOT NULL CONSTRAINT DF_CIERRES_CAJA_FECHA DEFAULT GETDATE(),
        total_ventas DECIMAL(10,2) NOT NULL,
        saldo_esperado DECIMAL(10,2) NOT NULL,
        saldo_real DECIMAL(10,2) NOT NULL,
        diferencia DECIMAL(10,2) NOT NULL,
        observaciones VARCHAR(255) NULL,
        CONSTRAINT PK_CIERRES_CAJA PRIMARY KEY (id_cierre),
        CONSTRAINT FK_CIERRES_CAJA_CAJAS FOREIGN KEY (id_caja) REFERENCES CAJAS(id_caja)
    );
GO

-- Tabla auxiliar: AUDITORIA_EQUIPAMIENTO. Guarda informacion necesaria para la trazabilidad.
    CREATE TABLE AUDITORIA_EQUIPAMIENTO (
        id_auditoria INT IDENTITY(1,1) NOT NULL,
        id_equipo INT NOT NULL,
        estado_anterior VARCHAR(50) NOT NULL,
        estado_nuevo VARCHAR(50) NOT NULL,
        fecha_cambio DATETIME NOT NULL CONSTRAINT DF_AUDITORIA_EQUIPO_FECHA DEFAULT GETDATE(),
        usuario_sistema VARCHAR(50) NULL,
        CONSTRAINT PK_AUDITORIA_EQUIPAMIENTO PRIMARY KEY (id_auditoria),
        CONSTRAINT FK_AUDITORIA_EQUIPAMIENTO_EQUIPO FOREIGN KEY (id_equipo) REFERENCES EQUIPAMIENTO(id_equipo)
    );
GO

-- VISTAS DE CONSULTA Y DASHBOARD
-- ----------------------------------------------------------------------------

-- Vista: vw_ControlAptosMedicos. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_ControlAptosMedicos
AS
WITH UltimoApto AS
(
    SELECT am.*, ROW_NUMBER() OVER (PARTITION BY am.id_socio ORDER BY am.fecha_vencimiento DESC, am.id_apto DESC) AS nro
    FROM APTOS_MEDICOS am
)
SELECT s.id_socio, p.dni, p.nombre, p.apellido,
       ua.id_apto, ua.fecha_emision, ua.fecha_vencimiento,
       CASE WHEN ua.id_apto IS NULL THEN 'SIN APTO'
            WHEN ua.fecha_vencimiento < CAST(GETDATE() AS DATE) THEN 'VENCIDO'
            WHEN ua.fecha_vencimiento <= DATEADD(DAY, 30, CAST(GETDATE() AS DATE)) THEN 'PROXIMO A VENCER'
            ELSE 'VIGENTE' END AS estado_apto
FROM SOCIOS s
JOIN PERSONAS p ON p.id_persona = s.id_persona
LEFT JOIN UltimoApto ua ON ua.id_socio = s.id_socio AND ua.nro = 1;
GO

-- Vista: vw_VentasDetalladasProductos. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_VentasDetalladasProductos
AS
SELECT v.id_venta, v.id_sede, se.nombre_sede, v.id_vendedor,
       v.id_socio, v.fecha_hora_venta, v.imputado_a_cuenta,
       dv.id_producto, pr.nombre_producto, cp.nombre_categoria,
       dv.cantidad, dv.precio_unitario_historico,
       dv.cantidad * dv.precio_unitario_historico AS importe_linea,
       fp.nombre_forma_pago
FROM VENTAS v
JOIN DETALLE_VENTAS dv ON dv.id_venta = v.id_venta
JOIN PRODUCTOS pr ON pr.id_producto = dv.id_producto
JOIN CATEGORIAS_PRODUCTOS cp ON cp.id_categoria = pr.id_categoria
JOIN SEDES se ON se.id_sede = v.id_sede
JOIN FORMAS_PAGO fp ON fp.id_forma_pago = v.id_forma_pago;
GO

-- Vista: vw_EstadoCuentaSocios. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_EstadoCuentaSocios
AS
SELECT s.id_socio, p.dni, p.nombre, p.apellido,
       COUNT(DISTINCT cc.id_cobro) AS cuotas_emitidas,
       ISNULL(SUM(cc.monto), 0) AS importe_emitido,
       ISNULL((SELECT SUM(pc.importe) FROM PAGOS_CUOTAS pc
               JOIN CUOTAS_COBROS ccp ON ccp.id_cobro = pc.id_cobro
               JOIN CONTRATOS_MEMBRESIA cmp ON cmp.id_contrato = ccp.id_contrato
               WHERE cmp.id_socio = s.id_socio), 0) AS importe_pagado,
       ISNULL(SUM(cc.monto), 0) - ISNULL((SELECT SUM(pc.importe) FROM PAGOS_CUOTAS pc
               JOIN CUOTAS_COBROS ccp ON ccp.id_cobro = pc.id_cobro
               JOIN CONTRATOS_MEMBRESIA cmp ON cmp.id_contrato = ccp.id_contrato
               WHERE cmp.id_socio = s.id_socio), 0) AS saldo_pendiente
FROM SOCIOS s
JOIN PERSONAS p ON p.id_persona = s.id_persona
LEFT JOIN CONTRATOS_MEMBRESIA cm ON cm.id_socio = s.id_socio
LEFT JOIN CUOTAS_COBROS cc ON cc.id_contrato = cm.id_contrato
GROUP BY s.id_socio, p.dni, p.nombre, p.apellido;
GO

-- Vista: vw_MembresiasVigentes. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_MembresiasVigentes
AS
SELECT cm.id_contrato, cm.id_socio, p.nombre, p.apellido,
       cm.id_plan, pl.nombre_plan, cm.id_sede, se.nombre_sede,
       cm.fecha_inicio, cm.fecha_vencimiento, cm.estado_contrato
FROM CONTRATOS_MEMBRESIA cm
JOIN SOCIOS s ON s.id_socio = cm.id_socio
JOIN PERSONAS p ON p.id_persona = s.id_persona
JOIN PLANES pl ON pl.id_plan = cm.id_plan
JOIN SEDES se ON se.id_sede = cm.id_sede
WHERE cm.estado_contrato = 'ACTIVO'
  AND CAST(GETDATE() AS DATE) BETWEEN cm.fecha_inicio AND cm.fecha_vencimiento;
GO

-- Vista: vw_CuotasPendientes. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_CuotasPendientes
AS
SELECT cc.id_cobro, cc.id_contrato, cm.id_socio, p.nombre, p.apellido,
       cc.numero_cuota, cc.fecha_vencimiento, cc.monto,
       ISNULL(SUM(pc.importe), 0) AS importe_pagado,
       cc.monto - ISNULL(SUM(pc.importe), 0) AS saldo,
       CASE WHEN cc.fecha_vencimiento < CAST(GETDATE() AS DATE) THEN 'VENCIDA' ELSE 'PENDIENTE' END AS estado_cuota
FROM CUOTAS_COBROS cc
JOIN CONTRATOS_MEMBRESIA cm ON cm.id_contrato = cc.id_contrato
JOIN SOCIOS s ON s.id_socio = cm.id_socio
JOIN PERSONAS p ON p.id_persona = s.id_persona
LEFT JOIN PAGOS_CUOTAS pc ON pc.id_cobro = cc.id_cobro
GROUP BY cc.id_cobro, cc.id_contrato, cm.id_socio, p.nombre, p.apellido,
         cc.numero_cuota, cc.fecha_vencimiento, cc.monto
HAVING cc.monto > ISNULL(SUM(pc.importe), 0);
GO

-- Vista: vw_StockDisponiblePorSede. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_StockDisponiblePorSede
AS
SELECT ss.id_sede, se.nombre_sede, ss.id_producto, pr.nombre_producto,
       ss.cantidad_actual, pr.stock_minimo,
       CASE WHEN ss.cantidad_actual <= pr.stock_minimo THEN 1 ELSE 0 END AS stock_critico
FROM STOCK_SEDE ss
JOIN SEDES se ON se.id_sede = ss.id_sede
JOIN PRODUCTOS pr ON pr.id_producto = ss.id_producto;
GO

-- Vista: vw_IncidenciasActivas. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_IncidenciasActivas
AS
SELECT ir.id_reclamo, ir.id_equipo, eq.nombre_maquina, ir.id_sede,
       se.nombre_sede, ir.id_tecnico_asignado, ir.id_estado_reclamo,
       er.nombre_estado, ir.fecha_registro, ir.descripcion
FROM INCIDENCIAS_RECLAMOS ir
JOIN ESTADOS_RECLAMO er ON er.id_estado_reclamo = ir.id_estado_reclamo
JOIN SEDES se ON se.id_sede = ir.id_sede
LEFT JOIN EQUIPAMIENTO eq ON eq.id_equipo = ir.id_equipo
WHERE er.nombre_estado NOT IN ('RESUELTO', 'CERRADO', 'CANCELADO');
GO

-- Vista: vw_CostosMantenimientoEquipamiento. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_CostosMantenimientoEquipamiento
AS
SELECT eq.id_equipo, eq.nombre_maquina, eq.id_sede, se.nombre_sede,
       COUNT(me.id_mantenimiento) AS cantidad_intervenciones,
       ISNULL(SUM(me.costo), 0) AS costo_total,
       MAX(me.fecha_mantenimiento) AS ultimo_mantenimiento
FROM EQUIPAMIENTO eq
JOIN SEDES se ON se.id_sede = eq.id_sede
LEFT JOIN MANTENIMIENTO_EQUIPAMIENTO me ON me.id_equipo = eq.id_equipo
GROUP BY eq.id_equipo, eq.nombre_maquina, eq.id_sede, se.nombre_sede;
GO

-- Vista: vw_RentabilidadConvenios. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_RentabilidadConvenios
AS
SELECT ce.id_convenio, ce.razon_social, ce.porcentaje_descuento,
       COUNT(DISTINCT s.id_socio) AS socios_asociados,
       ISNULL(SUM(cc.monto), 0) AS importe_facturado,
       ISNULL(SUM(pc.importe), 0) AS importe_cobrado,
       ISNULL(SUM(cc.monto), 0) - ISNULL(SUM(pc.importe), 0) AS deuda,
       CASE WHEN ISNULL(SUM(cc.monto), 0) = 0 THEN 0
            ELSE CAST(ISNULL(SUM(pc.importe), 0) * 100.0 / SUM(cc.monto) AS DECIMAL(10,2)) END AS porcentaje_cobrabilidad
FROM CONVENIOS_EMPRESA ce
LEFT JOIN SOCIOS s ON s.id_convenio = ce.id_convenio
LEFT JOIN CONTRATOS_MEMBRESIA cm ON cm.id_socio = s.id_socio
LEFT JOIN CUOTAS_COBROS cc ON cc.id_contrato = cm.id_contrato
LEFT JOIN PAGOS_CUOTAS pc ON pc.id_cobro = cc.id_cobro
GROUP BY ce.id_convenio, ce.razon_social, ce.porcentaje_descuento;
GO

-- Vista: vw_OcupacionClasesGrupales. Centraliza una consulta frecuente del sistema.
CREATE VIEW vw_OcupacionClasesGrupales
AS
SELECT c.id_clase, c.fecha_clase, c.hora_inicio, c.hora_fin,
       se.id_sede, se.nombre_sede, d.nombre_disciplina,
       c.cupo_limite,
       SUM(CASE WHEN r.estado_reserva IN ('CONFIRMADA','ASISTIO') THEN 1 ELSE 0 END) AS cupos_ocupados,
       c.cupo_limite - SUM(CASE WHEN r.estado_reserva IN ('CONFIRMADA','ASISTIO') THEN 1 ELSE 0 END) AS cupos_disponibles
FROM CLASES c
JOIN SALONES sa ON sa.id_salon = c.id_salon
JOIN SEDES se ON se.id_sede = sa.id_sede
JOIN DISCIPLINAS d ON d.id_disciplina = c.id_disciplina
LEFT JOIN RESERVAS r ON r.id_clase = c.id_clase
GROUP BY c.id_clase, c.fecha_clase, c.hora_inicio, c.hora_fin,
         se.id_sede, se.nombre_sede, d.nombre_disciplina, c.cupo_limite;
GO

-- ----------------------------------------------------------------------------
-- TIPOS DE TABLA PARA OPERACIONES MASIVAS
-- ----------------------------------------------------------------------------

-- Tipo de tabla: dbo.DetalleVentaType. Permite enviar varios detalles a un procedimiento.
CREATE TYPE dbo.DetalleVentaType AS TABLE (id_producto INT NOT NULL, cantidad INT NOT NULL, precio_unitario DECIMAL(10,2) NULL);
GO
-- Tipo de tabla: dbo.DetalleOperacionStockType. Permite enviar varios detalles a un procedimiento.
CREATE TYPE dbo.DetalleOperacionStockType AS TABLE (id_producto INT NOT NULL, cantidad INT NOT NULL);
GO

-- ----------------------------------------------------------------------------
-- PROCEDIMIENTOS DEL MODULO DE MIEMBROS
-- ----------------------------------------------------------------------------

-- Procedimiento: sp_RegistrarContratoMembresia. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_RegistrarContratoMembresia
    @id_socio INT,
    @id_plan INT,
    @id_sede INT,
    @fecha_inicio DATE,
    @id_forma_pago INT,
    @id_contrato INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        IF @fecha_inicio IS NULL OR @fecha_inicio < CAST(GETDATE() AS DATE)
            THROW 51000, 'La fecha de inicio del contrato no es valida.', 1;
        IF NOT EXISTS (SELECT 1 FROM SOCIOS WHERE id_socio = @id_socio AND estado_cuenta = 'ACTIVO')
            THROW 51001, 'El socio no existe o no se encuentra activo.', 1;
        IF NOT EXISTS (SELECT 1 FROM PLANES WHERE id_plan = @id_plan AND precio_actual > 0 AND duracion_meses > 0)
            THROW 51002, 'El plan no existe o no tiene una configuracion valida.', 1;
        IF NOT EXISTS (SELECT 1 FROM SEDES WHERE id_sede = @id_sede)
            THROW 51003, 'La sede indicada no existe.', 1;
        IF NOT EXISTS (SELECT 1 FROM FORMAS_PAGO WHERE id_forma_pago = @id_forma_pago)
            THROW 51004, 'La forma de pago indicada no existe.', 1;

        BEGIN TRANSACTION;
        DECLARE @duracion INT, @precio DECIMAL(10,2), @fecha_fin DATE;
        SELECT @duracion = duracion_meses, @precio = precio_actual FROM PLANES WHERE id_plan = @id_plan;
        SET @fecha_fin = DATEADD(MONTH, @duracion, @fecha_inicio);

        INSERT INTO CONTRATOS_MEMBRESIA(id_socio, id_plan, id_sede, fecha_inicio, fecha_vencimiento, estado_contrato)
        VALUES (@id_socio, @id_plan, @id_sede, @fecha_inicio, @fecha_fin, 'ACTIVO');
        SET @id_contrato = SCOPE_IDENTITY();

        ;WITH Numeros AS
        (
            SELECT 1 AS numero
            UNION ALL
            SELECT numero + 1 FROM Numeros WHERE numero < @duracion
        )
        INSERT INTO CUOTAS_COBROS(id_contrato, id_forma_pago, numero_cuota, fecha_vencimiento, monto)
        SELECT @id_contrato, @id_forma_pago, numero,
               DATEADD(MONTH, numero - 1, @fecha_inicio), @precio
        FROM Numeros
        OPTION (MAXRECURSION 120);

        COMMIT TRANSACTION;
        SELECT @id_contrato AS id_contrato, @fecha_fin AS fecha_vencimiento;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedimiento: sp_RegistrarPagoCuota. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_RegistrarPagoCuota
    @id_cobro INT,
    @importe DECIMAL(10,2),
    @id_forma_pago INT,
    @usuario_sistema VARCHAR(50) = NULL,
    @id_pago INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        IF @importe <= 0 THROW 51010, 'El importe del pago debe ser mayor a cero.', 1;
        BEGIN TRANSACTION;
        DECLARE @monto DECIMAL(10,2), @pagado DECIMAL(10,2);
        SELECT @monto = monto FROM CUOTAS_COBROS WITH (UPDLOCK, ROWLOCK) WHERE id_cobro = @id_cobro;
        IF @monto IS NULL THROW 51011, 'La cuota indicada no existe.', 1;
        IF NOT EXISTS (SELECT 1 FROM FORMAS_PAGO WHERE id_forma_pago = @id_forma_pago)
            THROW 51012, 'La forma de pago indicada no existe.', 1;
        SELECT @pagado = ISNULL(SUM(importe), 0) FROM PAGOS_CUOTAS WHERE id_cobro = @id_cobro;
        IF @pagado + @importe > @monto
            THROW 51013, 'El pago supera el saldo pendiente de la cuota.', 1;

        INSERT INTO PAGOS_CUOTAS(id_cobro, id_forma_pago, importe, usuario_sistema)
        VALUES (@id_cobro, @id_forma_pago, @importe, @usuario_sistema);
        SET @id_pago = SCOPE_IDENTITY();
        IF @pagado + @importe = @monto
            UPDATE CUOTAS_COBROS SET fecha_pago = GETDATE(), id_forma_pago = @id_forma_pago WHERE id_cobro = @id_cobro;

        COMMIT TRANSACTION;
        SELECT @id_pago AS id_pago, @id_cobro AS id_cobro, @monto - @pagado - @importe AS saldo_pendiente;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedimiento: sp_RenovarMembresia. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_RenovarMembresia
    @id_contrato_anterior INT,
    @id_plan INT = NULL,
    @id_forma_pago INT = NULL,
    @id_contrato_nuevo INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @socio INT, @sede INT, @planAnterior INT, @fin DATE;
        SELECT @socio = id_socio, @sede = id_sede, @planAnterior = id_plan, @fin = fecha_vencimiento
        FROM CONTRATOS_MEMBRESIA WITH (UPDLOCK) WHERE id_contrato = @id_contrato_anterior;
        IF @socio IS NULL THROW 51020, 'El contrato anterior no existe.', 1;
        IF EXISTS (SELECT 1 FROM vw_CuotasPendientes WHERE id_contrato = @id_contrato_anterior)
            THROW 51021, 'No se puede renovar un contrato con cuotas pendientes.', 1;
        SET @id_plan = ISNULL(@id_plan, @planAnterior);
        SET @id_forma_pago = ISNULL(@id_forma_pago, (SELECT TOP 1 id_forma_pago FROM CUOTAS_COBROS WHERE id_contrato = @id_contrato_anterior ORDER BY numero_cuota DESC));
        IF @id_forma_pago IS NULL THROW 51022, 'Debe indicar una forma de pago para la renovacion.', 1;

        UPDATE CONTRATOS_MEMBRESIA SET estado_contrato = 'RENOVADO' WHERE id_contrato = @id_contrato_anterior;
        DECLARE @inicio DATE = CASE WHEN @fin < CAST(GETDATE() AS DATE) THEN CAST(GETDATE() AS DATE) ELSE DATEADD(DAY, 1, @fin) END;
        EXEC sp_RegistrarContratoMembresia @socio, @id_plan, @sede, @inicio, @id_forma_pago, @id_contrato_nuevo OUTPUT;
        COMMIT TRANSACTION;
        SELECT @id_contrato_nuevo AS id_contrato_nuevo;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedimiento: sp_RegistrarAccesoSocio. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_RegistrarAccesoSocio
    @id_socio INT,
    @id_sede INT,
    @acceso_permitido BIT OUTPUT,
    @motivo_rechazo VARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @acceso_permitido = 0;
    SET @motivo_rechazo = NULL;
    IF NOT EXISTS (SELECT 1 FROM SOCIOS WHERE id_socio = @id_socio)
        THROW 51030, 'El socio indicado no existe.', 1;
    IF NOT EXISTS (SELECT 1 FROM SEDES WHERE id_sede = @id_sede)
        THROW 51031, 'La sede indicada no existe.', 1;
    IF EXISTS (SELECT 1 FROM SOCIOS WHERE id_socio = @id_socio AND estado_cuenta <> 'ACTIVO')
        SET @motivo_rechazo = 'SOCIO INACTIVO';
    ELSE IF NOT EXISTS (SELECT 1 FROM CONTRATOS_MEMBRESIA WHERE id_socio = @id_socio AND estado_contrato = 'ACTIVO' AND CAST(GETDATE() AS DATE) BETWEEN fecha_inicio AND fecha_vencimiento AND (id_sede = @id_sede OR EXISTS (SELECT 1 FROM PLANES p WHERE p.id_plan = CONTRATOS_MEMBRESIA.id_plan AND p.permite_multisede = 1)))
        SET @motivo_rechazo = 'MEMBRESIA NO VIGENTE';
    ELSE IF NOT EXISTS (SELECT 1 FROM APTOS_MEDICOS WHERE id_socio = @id_socio AND CAST(GETDATE() AS DATE) BETWEEN fecha_emision AND fecha_vencimiento)
        SET @motivo_rechazo = 'APTO MEDICO VENCIDO';
    ELSE
        SET @acceso_permitido = 1;

    INSERT INTO ACCESOS_LOG(id_socio, id_sede, fecha_hora_marcado, acceso_permitido, motivo_rechazo)
    VALUES (@id_socio, @id_sede, GETDATE(), @acceso_permitido, @motivo_rechazo);
    IF @acceso_permitido = 0
        INSERT INTO LOG_ACCESOS_DENEGADOS(id_socio, id_sede, fecha_hora, motivo_bloqueo)
        VALUES (@id_socio, @id_sede, GETDATE(), @motivo_rechazo);
    SELECT @acceso_permitido AS acceso_permitido, @motivo_rechazo AS motivo_rechazo;
END;
GO

-- ----------------------------------------------------------------------------
-- PROCEDIMIENTOS DEL MODULO DE TIENDA
-- ----------------------------------------------------------------------------

-- Procedimiento: sp_RegistrarVentaPOS. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_RegistrarVentaPOS
    @id_socio INT = NULL,
    @id_sede INT,
    @id_vendedor INT,
    @id_forma_pago INT,
    @imputado_a_cuenta BIT = 0,
    @detalles dbo.DetalleVentaType READONLY,
    @id_venta INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM @detalles WHERE cantidad > 0) THROW 51040, 'La venta debe contener al menos un detalle valido.', 1;
        IF EXISTS (SELECT 1 FROM @detalles WHERE cantidad <= 0) THROW 51041, 'Las cantidades deben ser mayores a cero.', 1;
        IF @imputado_a_cuenta = 1 AND (@id_socio IS NULL OR NOT EXISTS (SELECT 1 FROM SOCIOS WHERE id_socio = @id_socio AND estado_cuenta = 'ACTIVO'))
            THROW 51042, 'Solo un socio activo puede utilizar cuenta corriente.', 1;
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM SEDES WHERE id_sede = @id_sede) THROW 51043, 'La sede no existe.', 1;
        IF NOT EXISTS (SELECT 1 FROM EMPLEADOS WHERE id_empleado = @id_vendedor) THROW 51044, 'El vendedor no existe.', 1;
        INSERT INTO VENTAS(id_socio, id_sede, id_vendedor, id_forma_pago, fecha_hora_venta, imputado_a_cuenta)
        VALUES (@id_socio, @id_sede, @id_vendedor, @id_forma_pago, GETDATE(), @imputado_a_cuenta);
        SET @id_venta = SCOPE_IDENTITY();
        INSERT INTO DETALLE_VENTAS(id_venta, id_producto, cantidad, precio_unitario_historico)
        SELECT @id_venta, d.id_producto, d.cantidad, ISNULL(d.precio_unitario, p.precio_actual)
        FROM @detalles d JOIN PRODUCTOS p ON p.id_producto = d.id_producto;
        IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @detalles)
            THROW 51045, 'Uno o mas productos de la venta no existen.', 1;
        COMMIT TRANSACTION;
        SELECT @id_venta AS id_venta,
               (SELECT SUM(cantidad * precio_unitario_historico) FROM DETALLE_VENTAS WHERE id_venta = @id_venta) AS total;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedimiento: sp_RegistrarDevolucionVenta. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_RegistrarDevolucionVenta
    @id_venta INT,
    @id_empleado INT,
    @motivo VARCHAR(255),
    @detalles dbo.DetalleOperacionStockType READONLY,
    @id_devolucion INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM VENTAS WHERE id_venta = @id_venta) THROW 51050, 'La venta no existe.', 1;
        IF NOT EXISTS (SELECT 1 FROM @detalles WHERE cantidad > 0) THROW 51051, 'La devolucion no tiene detalles validos.', 1;
        BEGIN TRANSACTION;
        DECLARE @total DECIMAL(10,2), @sede INT;
        SELECT @sede = id_sede FROM VENTAS WHERE id_venta = @id_venta;
        SELECT @total = SUM(d.cantidad * dv.precio_unitario_historico)
        FROM @detalles d JOIN DETALLE_VENTAS dv ON dv.id_venta = @id_venta AND dv.id_producto = d.id_producto;
        IF @total IS NULL THROW 51052, 'Los productos devueltos no pertenecen a la venta.', 1;
        INSERT INTO DEVOLUCIONES_VENTA(id_venta, motivo, id_empleado, importe_reintegrado)
        VALUES (@id_venta, @motivo, @id_empleado, @total);
        SET @id_devolucion = SCOPE_IDENTITY();
        INSERT INTO DETALLE_DEVOLUCIONES_VENTA(id_devolucion, id_producto, cantidad, precio_unitario)
        SELECT @id_devolucion, d.id_producto, d.cantidad, dv.precio_unitario_historico
        FROM @detalles d JOIN DETALLE_VENTAS dv ON dv.id_venta = @id_venta AND dv.id_producto = d.id_producto;
        UPDATE ss SET cantidad_actual = ss.cantidad_actual + d.cantidad
        FROM STOCK_SEDE ss JOIN @detalles d ON d.id_producto = ss.id_producto AND ss.id_sede = @sede;
        COMMIT TRANSACTION;
        SELECT @id_devolucion AS id_devolucion, @total AS importe_reintegrado;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedimiento: sp_TransferirStockEntreSedes. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_TransferirStockEntreSedes
    @id_sede_origen INT,
    @id_sede_destino INT,
    @detalles dbo.DetalleOperacionStockType READONLY,
    @usuario_sistema VARCHAR(50) = NULL,
    @id_transferencia INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        IF @id_sede_origen = @id_sede_destino THROW 51060, 'La sede de origen y destino deben ser distintas.', 1;
        IF EXISTS (SELECT 1 FROM @detalles WHERE cantidad <= 0) THROW 51061, 'Las cantidades deben ser mayores a cero.', 1;
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM @detalles d LEFT JOIN STOCK_SEDE ss WITH (UPDLOCK, HOLDLOCK) ON ss.id_producto = d.id_producto AND ss.id_sede = @id_sede_origen WHERE ss.id_stock_sede IS NULL OR ss.cantidad_actual < d.cantidad)
            THROW 51062, 'Stock insuficiente en la sede de origen.', 1;
        INSERT INTO TRANSFERENCIAS_STOCK(id_sede_origen, id_sede_destino, estado, usuario_sistema)
        VALUES (@id_sede_origen, @id_sede_destino, 'RECIBIDA', @usuario_sistema);
        SET @id_transferencia = SCOPE_IDENTITY();
        INSERT INTO DETALLE_TRANSFERENCIAS(id_transferencia, id_producto, cantidad)
        SELECT @id_transferencia, id_producto, cantidad FROM @detalles;
        UPDATE ss SET cantidad_actual = ss.cantidad_actual - d.cantidad
        FROM STOCK_SEDE ss JOIN @detalles d ON d.id_producto = ss.id_producto AND ss.id_sede = @id_sede_origen;
        MERGE STOCK_SEDE AS destino
        USING @detalles AS d ON destino.id_sede = @id_sede_destino AND destino.id_producto = d.id_producto
        WHEN MATCHED THEN UPDATE SET cantidad_actual = destino.cantidad_actual + d.cantidad
        WHEN NOT MATCHED THEN INSERT(id_sede, id_producto, cantidad_actual) VALUES (@id_sede_destino, d.id_producto, d.cantidad);
        COMMIT TRANSACTION;
        SELECT @id_transferencia AS id_transferencia;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedimiento: sp_CerrarCajaDiaria. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_CerrarCajaDiaria
    @id_caja INT,
    @saldo_real DECIMAL(10,2),
    @observaciones VARCHAR(255) = NULL,
    @id_cierre INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @apertura DATETIME, @saldo_inicial DECIMAL(10,2), @total DECIMAL(10,2), @esperado DECIMAL(10,2);
        SELECT @apertura = fecha_apertura, @saldo_inicial = saldo_inicial FROM CAJAS WITH (UPDLOCK) WHERE id_caja = @id_caja AND estado = 'ABIERTA';
        IF @apertura IS NULL THROW 51070, 'La caja no existe o ya fue cerrada.', 1;
        SELECT @total = ISNULL(SUM(vw.importe_linea), 0) FROM vw_VentasDetalladasProductos vw
        JOIN VENTAS v ON v.id_venta = vw.id_venta
        WHERE v.fecha_hora_venta >= @apertura AND v.id_sede = (SELECT id_sede FROM CAJAS WHERE id_caja = @id_caja);
        SET @esperado = @saldo_inicial + @total;
        INSERT INTO CIERRES_CAJA(id_caja, total_ventas, saldo_esperado, saldo_real, diferencia, observaciones)
        VALUES (@id_caja, @total, @esperado, @saldo_real, @saldo_real - @esperado, @observaciones);
        SET @id_cierre = SCOPE_IDENTITY();
        UPDATE CAJAS SET estado = 'CERRADA', fecha_cierre = GETDATE() WHERE id_caja = @id_caja;
        COMMIT TRANSACTION;
        SELECT @id_cierre AS id_cierre, @esperado AS saldo_esperado, @saldo_real - @esperado AS diferencia;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- ----------------------------------------------------------------------------
-- PROCEDIMIENTOS DEL MODULO DE EQUIPAMIENTO Y REPORTES
-- ----------------------------------------------------------------------------

-- Procedimiento: sp_RegistrarMantenimiento. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_RegistrarMantenimiento
    @id_equipo INT,
    @id_proveedor INT = NULL,
    @fecha_mantenimiento DATE,
    @descripcion VARCHAR(255),
    @costo DECIMAL(10,2),
    @id_reclamo INT = NULL,
    @id_mantenimiento INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        IF @costo < 0 THROW 51080, 'El costo no puede ser negativo.', 1;
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM EQUIPAMIENTO WHERE id_equipo = @id_equipo) THROW 51081, 'El equipo no existe.', 1;
        INSERT INTO MANTENIMIENTO_EQUIPAMIENTO(id_equipo, id_proveedor, fecha_mantenimiento, descripcion, costo)
        VALUES (@id_equipo, @id_proveedor, @fecha_mantenimiento, @descripcion, @costo);
        SET @id_mantenimiento = SCOPE_IDENTITY();
        UPDATE EQUIPAMIENTO SET estado_operativo = 'OPERATIVO' WHERE id_equipo = @id_equipo;
        IF @id_reclamo IS NOT NULL
            UPDATE ir SET id_estado_reclamo = (SELECT TOP 1 id_estado_reclamo FROM ESTADOS_RECLAMO WHERE nombre_estado = 'RESUELTO')
            FROM INCIDENCIAS_RECLAMOS ir WHERE ir.id_reclamo = @id_reclamo;
        COMMIT TRANSACTION;
        SELECT @id_mantenimiento AS id_mantenimiento;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedimiento: sp_GenerarMantenimientosPreventivos. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_GenerarMantenimientosPreventivos
    @dias_anticipacion INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    SELECT eq.id_equipo, eq.nombre_maquina, eq.id_sede,
           MAX(me.fecha_mantenimiento) AS ultimo_mantenimiento,
           DATEADD(MONTH, 6, MAX(me.fecha_mantenimiento)) AS proximo_mantenimiento,
           CASE WHEN DATEADD(MONTH, 6, MAX(me.fecha_mantenimiento)) < CAST(GETDATE() AS DATE) THEN 'ATRASADO' ELSE 'PROXIMO' END AS estado
    FROM EQUIPAMIENTO eq
    LEFT JOIN MANTENIMIENTO_EQUIPAMIENTO me ON me.id_equipo = eq.id_equipo
    GROUP BY eq.id_equipo, eq.nombre_maquina, eq.id_sede
    HAVING MAX(me.fecha_mantenimiento) IS NULL
        OR DATEADD(MONTH, 6, MAX(me.fecha_mantenimiento)) <= DATEADD(DAY, @dias_anticipacion, CAST(GETDATE() AS DATE));
END;
GO

-- Procedimiento: sp_AuditoriaPremiosCanjeados. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_AuditoriaPremiosCanjeados
    @estado VARCHAR(20) = NULL,
    @minMeses INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SELECT po.id_otorgamiento, po.id_socio, p.nombre, p.apellido,
           pc.nombre_premio, pc.meses_antiguedad_requeridos,
           po.fecha_otorgamiento, po.estado_canje,
           DATEDIFF(MONTH, s.fecha_ingreso_alta, po.fecha_otorgamiento) AS meses_antiguedad
    FROM PREMIOS_OTORGADOS po
    JOIN SOCIOS s ON s.id_socio = po.id_socio
    JOIN PERSONAS p ON p.id_persona = s.id_persona
    JOIN PREMIOS_CATALOGO pc ON pc.id_premio = po.id_premio
    WHERE (@estado IS NULL OR po.estado_canje = @estado)
      AND DATEDIFF(MONTH, s.fecha_ingreso_alta, po.fecha_otorgamiento) >= @minMeses
    ORDER BY po.fecha_otorgamiento DESC;
END;
GO

-- Procedimiento: sp_ReporteRentabilidadConvenios. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_ReporteRentabilidadConvenios
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM vw_RentabilidadConvenios ORDER BY porcentaje_cobrabilidad DESC, importe_facturado DESC;
END;
GO

-- Procedimiento: sp_ReporteRentabilidadSedes. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_ReporteRentabilidadSedes
    @fecha_desde DATE = NULL,
    @fecha_hasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @fecha_desde = ISNULL(@fecha_desde, DATEFROMPARTS(YEAR(GETDATE()), 1, 1));
    SET @fecha_hasta = ISNULL(@fecha_hasta, CAST(GETDATE() AS DATE));
    SELECT v.id_sede, se.nombre_sede,
           ISNULL(SUM(dv.cantidad * dv.precio_unitario_historico), 0) AS ingresos,
           ISNULL((SELECT SUM(me.costo) FROM MANTENIMIENTO_EQUIPAMIENTO me JOIN EQUIPAMIENTO eq ON eq.id_equipo = me.id_equipo WHERE eq.id_sede = v.id_sede AND me.fecha_mantenimiento BETWEEN @fecha_desde AND @fecha_hasta), 0) AS costos_mantenimiento,
           ISNULL(SUM(dv.cantidad * dv.precio_unitario_historico), 0) - ISNULL((SELECT SUM(me.costo) FROM MANTENIMIENTO_EQUIPAMIENTO me JOIN EQUIPAMIENTO eq ON eq.id_equipo = me.id_equipo WHERE eq.id_sede = v.id_sede AND me.fecha_mantenimiento BETWEEN @fecha_desde AND @fecha_hasta), 0) AS resultado
    FROM VENTAS v JOIN SEDES se ON se.id_sede = v.id_sede
    LEFT JOIN DETALLE_VENTAS dv ON dv.id_venta = v.id_venta
    WHERE CAST(v.fecha_hora_venta AS DATE) BETWEEN @fecha_desde AND @fecha_hasta
    GROUP BY v.id_sede, se.nombre_sede;
END;
GO

-- Procedimiento: sp_ReporteRetencionSocios. Ejecuta una operacion de negocio o reporte.
CREATE PROCEDURE sp_ReporteRetencionSocios
    @periodo_desde DATE,
    @periodo_hasta DATE
AS
BEGIN
    SET NOCOUNT ON;
    IF @periodo_desde >= @periodo_hasta THROW 51090, 'El periodo inicial debe ser anterior al final.', 1;
    DECLARE @inicio INT, @fin INT, @retuvieron INT;
    SELECT @inicio = COUNT(DISTINCT id_socio) FROM CONTRATOS_MEMBRESIA WHERE fecha_inicio <= @periodo_desde AND fecha_vencimiento >= @periodo_desde;
    SELECT @fin = COUNT(DISTINCT id_socio) FROM CONTRATOS_MEMBRESIA WHERE fecha_inicio <= @periodo_hasta AND fecha_vencimiento >= @periodo_hasta;
    SELECT @retuvieron = COUNT(DISTINCT a.id_socio)
    FROM CONTRATOS_MEMBRESIA a
    WHERE a.fecha_inicio <= @periodo_desde AND a.fecha_vencimiento >= @periodo_desde
      AND EXISTS (SELECT 1 FROM CONTRATOS_MEMBRESIA b WHERE b.id_socio = a.id_socio AND b.fecha_inicio <= @periodo_hasta AND b.fecha_vencimiento >= @periodo_hasta);
    SELECT @inicio AS socios_inicio, @fin AS socios_fin, @retuvieron AS socios_retenidos,
           CAST(CASE WHEN @inicio = 0 THEN 0 ELSE @retuvieron * 100.0 / @inicio END AS DECIMAL(10,2)) AS tasa_retencion,
           CAST(CASE WHEN @inicio = 0 THEN 0 ELSE (@inicio - @retuvieron) * 100.0 / @inicio END AS DECIMAL(10,2)) AS tasa_desercion;
END;
GO

-- TRIGGERS DE INTEGRIDAD, STOCK Y AUDITORIA
-- ----------------------------------------------------------------------------

-- Trigger: TR_ActualizarStockVenta. Automatiza un control o una auditoria de datos.
CREATE TRIGGER TR_ActualizarStockVenta
ON DETALLE_VENTAS
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @salidas TABLE (id_sede INT, id_producto INT, cantidad INT, id_venta INT);
    INSERT INTO @salidas(id_sede, id_producto, cantidad, id_venta)
    SELECT v.id_sede, i.id_producto, SUM(i.cantidad), MIN(v.id_venta)
    FROM inserted i JOIN VENTAS v ON v.id_venta = i.id_venta
    GROUP BY v.id_sede, i.id_producto;

    IF EXISTS
    (
        SELECT 1
        FROM @salidas x
        LEFT JOIN STOCK_SEDE ss WITH (UPDLOCK, HOLDLOCK)
          ON ss.id_sede = x.id_sede AND ss.id_producto = x.id_producto
        WHERE ss.id_stock_sede IS NULL OR ss.cantidad_actual < x.cantidad
    )
        THROW 51100, 'Stock insuficiente para completar la venta.', 1;

    UPDATE ss SET cantidad_actual = ss.cantidad_actual - x.cantidad
    FROM STOCK_SEDE ss JOIN @salidas x ON x.id_sede = ss.id_sede AND x.id_producto = ss.id_producto;
END;
GO

-- Trigger: TR_ControlarStockNegativo. Automatiza un control o una auditoria de datos.
CREATE TRIGGER TR_ControlarStockNegativo
ON STOCK_SEDE
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted WHERE cantidad_actual < 0)
        THROW 51101, 'La operacion no puede generar stock negativo.', 1;
END;
GO

-- Trigger: TR_RegistrarMovimientosStock. Automatiza un control o una auditoria de datos.
CREATE TRIGGER TR_RegistrarMovimientosStock
ON STOCK_SEDE
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO MOVIMIENTOS_STOCK(id_sede, id_producto, tipo_movimiento, cantidad, usuario_sistema, observaciones)
    SELECT i.id_sede, i.id_producto,
           CASE WHEN ISNULL(d.cantidad_actual, 0) < i.cantidad_actual THEN 'ENTRADA'
                WHEN ISNULL(d.cantidad_actual, 0) > i.cantidad_actual THEN 'SALIDA'
                ELSE 'ALTA' END,
           CASE WHEN d.id_stock_sede IS NULL THEN i.cantidad_actual ELSE i.cantidad_actual - d.cantidad_actual END,
           CONVERT(VARCHAR(50), SESSION_CONTEXT(N'usuario')),
           'Movimiento generado automaticamente por cambio de stock'
    FROM inserted i
    LEFT JOIN deleted d ON d.id_stock_sede = i.id_stock_sede
    WHERE d.id_stock_sede IS NULL OR i.cantidad_actual <> d.cantidad_actual;
END;
GO

-- Trigger: TR_AuditarCambiosPrecios. Automatiza un control o una auditoria de datos.
CREATE TRIGGER TR_AuditarCambiosPrecios
ON PRODUCTOS
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO HISTORIAL_PRECIOS_PRODUCTOS(id_producto, precio_anterior, precio_nuevo, fecha_cambio)
    SELECT d.id_producto, d.precio_actual, i.precio_actual, GETDATE()
    FROM inserted i JOIN deleted d ON d.id_producto = i.id_producto
    WHERE d.precio_actual <> i.precio_actual;
END;
GO

-- Trigger: TR_AuditarCambiosEstadoSocio. Automatiza un control o una auditoria de datos.
CREATE TRIGGER TR_AuditarCambiosEstadoSocio
ON SOCIOS
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO LOG_CAMBIOS_ESTADO_SOCIO(id_socio, estado_anterior, estado_nuevo, fecha_cambio, usuario_sistema)
    SELECT d.id_socio, d.estado_cuenta, i.estado_cuenta, GETDATE(),
           ISNULL(CONVERT(VARCHAR(50), SESSION_CONTEXT(N'usuario')), SYSTEM_USER)
    FROM inserted i JOIN deleted d ON d.id_socio = i.id_socio
    WHERE d.estado_cuenta <> i.estado_cuenta;
END;
GO

-- Trigger: TR_AuditarCambiosEquipamiento. Automatiza un control o una auditoria de datos.
CREATE TRIGGER TR_AuditarCambiosEquipamiento
ON EQUIPAMIENTO
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO AUDITORIA_EQUIPAMIENTO(id_equipo, estado_anterior, estado_nuevo, usuario_sistema)
    SELECT d.id_equipo, d.estado_operativo, i.estado_operativo,
           ISNULL(CONVERT(VARCHAR(50), SESSION_CONTEXT(N'usuario')), SYSTEM_USER)
    FROM inserted i JOIN deleted d ON d.id_equipo = i.id_equipo
    WHERE d.estado_operativo <> i.estado_operativo;
END;
GO

-- Indice: UX_PERSONAS_DNI. Mejora las consultas y evita duplicados cuando corresponde.
    CREATE UNIQUE INDEX UX_PERSONAS_DNI ON PERSONAS(dni);
-- Indice: UX_PRODUCTOS_CODIGO_BARRAS. Mejora las consultas y evita duplicados cuando corresponde.
    CREATE UNIQUE INDEX UX_PRODUCTOS_CODIGO_BARRAS ON PRODUCTOS(codigo_barras);
-- Indice: UX_STOCK_SEDE_PRODUCTO. Mejora las consultas y evita duplicados cuando corresponde.
    CREATE UNIQUE INDEX UX_STOCK_SEDE_PRODUCTO ON STOCK_SEDE(id_sede, id_producto);
-- Indice: UX_PLANES_DISCIPLINAS. Mejora las consultas y evita duplicados cuando corresponde.
    CREATE UNIQUE INDEX UX_PLANES_DISCIPLINAS ON PLANES_DISCIPLINAS(id_plan, id_disciplina);
-- Indice: UX_RESERVAS_SOCIO_CLASE. Mejora las consultas y evita duplicados cuando corresponde.
    CREATE UNIQUE INDEX UX_RESERVAS_SOCIO_CLASE ON RESERVAS(id_socio, id_clase);
-- Indice: IX_CUOTAS_CONTRATO_VENCIMIENTO. Mejora las consultas y evita duplicados cuando corresponde.
    CREATE INDEX IX_CUOTAS_CONTRATO_VENCIMIENTO ON CUOTAS_COBROS(id_contrato, fecha_vencimiento);
-- Indice: IX_MOVIMIENTOS_STOCK_PRODUCTO_SEDE. Mejora las consultas y evita duplicados cuando corresponde.
    CREATE INDEX IX_MOVIMIENTOS_STOCK_PRODUCTO_SEDE ON MOVIMIENTOS_STOCK(id_producto, id_sede, fecha_movimiento);
GO



