using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Repositorys.Interfaces;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using FITLIFENETWORKPROYECTO.Models;

namespace FITLIFENETWORKPROYECTO.Services
{
    public class ServicesFIT : IServices
    {
        private readonly IRepository repo;

        public ServicesFIT(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<List<AuditoriaAptoMedico>> GetAccesoDenegadoApto(DateTime? desde, DateTime? hasta)
        {
            ValidarRangoFechas(desde, hasta);
            return await repo.GetAccesoDenegadoApto(desde, hasta);
        }

        private static void ValidarRangoFechas(DateTime? desde, DateTime? hasta)
        {
            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
            {
                throw new ArgumentException("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.");
            }
        }

        public async Task<List<AuditoriaAccesoDto>> GetAuditoriaAccesosDenegadosAsync()
        {
            var registros = await repo.GetAuditoriaAccesosDenegadosAsync();
            return registros.Select(r => new AuditoriaAccesoDto
            {
                IdLog = r.IdLog,
                IdSocio = r.IdSocio,
                NombreSocio = r.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{r.IdSocioNavigation.IdPersonaNavigation.Apellido}, {r.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdSede = r.IdSede,
                NombreSede = r.IdSedeNavigation?.NombreSede ?? string.Empty,
                FechaHora = r.FechaHora,
                MotivoBloqueo = r.MotivoBloqueo
            }).ToList();
        }

        public async Task<List<AuditoriaEstadoSocioDto>> GetAuditoriaCambiosEstadoSocioAsync()
        {
            var registros = await repo.GetAuditoriaCambiosEstadoSocioAsync();
            return registros.Select(r => new AuditoriaEstadoSocioDto
            {
                IdLogEstado = r.IdLogEstado,
                IdSocio = r.IdSocio,
                NombreSocio = r.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{r.IdSocioNavigation.IdPersonaNavigation.Apellido}, {r.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                EstadoAnterior = r.EstadoAnterior,
                EstadoNuevo = r.EstadoNuevo,
                FechaCambio = r.FechaCambio,
                UsuarioSistema = r.UsuarioSistema
            }).ToList();
        }

        public async Task<List<AuditoriaEquipamientoDto>> GetAuditoriaEquipamientoAsync()
        {
            var registros = await repo.GetAuditoriaEquipamientoAsync();
            return registros.Select(r => new AuditoriaEquipamientoDto
            {
                IdAuditoria = r.IdAuditoria,
                IdEquipo = r.IdEquipo,
                NombreEquipo = r.IdEquipoNavigation?.NombreMaquina ?? string.Empty,
                EstadoAnterior = r.EstadoAnterior,
                EstadoNuevo = r.EstadoNuevo,
                FechaCambio = r.FechaCambio,
                UsuarioSistema = r.UsuarioSistema
            }).ToList();
        }

        public async Task<List<ControlAptoMedico>> GetControlAptosMedicos(string? estados)
        {
            var validos = new[] { "VENCIDO", "PROXIMO A VENCER", "VIGENTE", "SIN APTA MEDICA" };
            var lista = (estados ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(e => e.ToUpperInvariant())
                .ToList();

            if (lista.Any(e => !validos.Contains(e)))
            {
                throw new ArgumentException("Estado de apto médico inválido. Use: " + string.Join(", ", validos) + ".");
            }
            return await repo.GetControlAptosMedicos(lista);
        }

        public async Task<List<EvaluacionEstadistica>> GetEvaluacionesEstadistica(DateTime desde, DateTime hasta)
        {
            ValidarRangoFechas(desde, hasta);
            return await repo.GetEvaluacionesEstadistica(desde, hasta);
        }

        public async Task<List<PremiosCanjeados>> GetPremiosCanjeados(string? estadoCanje, int minMeses)
        {
            if (minMeses < 0)
            {
                throw new ArgumentException("Los meses de antigüedad no pueden ser negativos.");
            }
            var estado = string.IsNullOrWhiteSpace(estadoCanje) ? null : estadoCanje.Trim().ToUpperInvariant();
            return await repo.GetPremiosCanjeados(estado, minMeses);
        }

        public async Task<List<ReporteRecaudacionSede>> GetRecaudacionSede(int? año, int? mes)
        {
            var anioActual = año ?? DateTime.Today.Year;
            var mesActual = mes ?? DateTime.Today.Month;

            if (anioActual > DateTime.Today.Year)
            {
                throw new ArgumentException();
            }
            if (mesActual < 1 || mesActual > 12)
            {
                throw new ArgumentException();
            }
            return await repo.GetRecaudacionSede(anioActual, mesActual);
        }

        public async Task<List<RentabilidadConvenio>> GetRentabilidadConvenios(string? Estado)
        {
            return await repo.GetRentabilidadConvenios(Estado);
        }

        public async Task<List<ReporteReservaTT>> GetReporteReserva(DateTime desde, DateTime hasta, string disciplina1, string disciplina2)
        {
            if (string.IsNullOrWhiteSpace(disciplina1))
            {
                throw new ArgumentException("Debe indicar al menos una disciplina.");
            }
            if (desde.Date > hasta.Date)
            {
                throw new ArgumentException("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.");
            }
            var d1 = disciplina1.Trim();
            var d2 = string.IsNullOrWhiteSpace(disciplina2) ? d1 : disciplina2.Trim();
            return await repo.GetReporteReserva(desde, hasta, d1, d2);
        }

        public async Task<List<Top3Productos>> GetTop3Productos(int? año)
        {
            var anioActual = año ?? DateTime.Today.Year;
            if (anioActual > DateTime.Today.Year)
            {
                throw new ArgumentException();
            }
            return await repo.GetTop3Productos(anioActual);
        }

        public async Task<List<SocioDto>> GetSociosAsync()
        {
            var socios = await repo.GetSociosAsync();
            return socios.Select(s =>
            {
                var contratoActivo = s.ContratosMembresia
                    .OrderByDescending(c => c.FechaVencimiento)
                    .FirstOrDefault(c => c.EstadoContrato == "ACTIVO" || c.FechaVencimiento >= DateOnly.FromDateTime(DateTime.Today));

                return new SocioDto
                {
                    IdSocio = s.IdSocio,
                    Dni = s.IdPersonaNavigation?.Dni ?? string.Empty,
                    Nombre = s.IdPersonaNavigation?.Nombre ?? string.Empty,
                    Apellido = s.IdPersonaNavigation?.Apellido ?? string.Empty,
                    Email = s.IdPersonaNavigation?.Email,
                    Telefono = s.IdPersonaNavigation?.Telefono,
                    EstadoCuenta = s.EstadoCuenta,
                    FechaIngresoAlta = s.FechaIngresoAlta,
                    IdSede = contratoActivo?.IdSede,
                    NombreSede = contratoActivo?.IdSedeNavigation?.NombreSede,
                    Plan = contratoActivo?.IdPlanNavigation?.NombrePlan
                };
            }).ToList();
        }

        public async Task<SocioDetalleDto?> GetSocioByIdAsync(int idSocio)
        {
            var socio = await repo.GetSocioByIdAsync(idSocio);
            if (socio == null)
            {
                return null;
            }

            return new SocioDetalleDto
            {
                IdSocio = socio.IdSocio,
                IdPersona = socio.IdPersona,
                Dni = socio.IdPersonaNavigation?.Dni ?? string.Empty,
                Nombre = socio.IdPersonaNavigation?.Nombre ?? string.Empty,
                Apellido = socio.IdPersonaNavigation?.Apellido ?? string.Empty,
                Email = socio.IdPersonaNavigation?.Email,
                Telefono = socio.IdPersonaNavigation?.Telefono,
                EstadoCivil = socio.IdPersonaNavigation?.EstadoCivil,
                Ocupacion = socio.IdPersonaNavigation?.Ocupacion,
                FechaNacimiento = socio.IdPersonaNavigation?.FechaNacimiento,
                FechaIngresoAlta = socio.FechaIngresoAlta,
                EstadoCuenta = socio.EstadoCuenta,
                IdCalle = socio.IdCalle,
                IdConvenio = socio.IdConvenio
            };
        }

        public async Task<SocioDto> CreateSocioAsync(SocioCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.Dni))
            {
                throw new ArgumentException("El DNI es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Apellido))
            {
                throw new ArgumentException("Nombre y apellido son obligatorios.");
            }

            var socios = await repo.GetSociosAsync();
            if (socios.Any(s => s.IdPersonaNavigation != null && s.IdPersonaNavigation.Dni == dto.Dni.Trim()))
            {
                throw new InvalidOperationException("Ya existe un socio con ese DNI.");
            }

            var entidad = new Models.Tablas.Socio
            {
                IdCalle = dto.IdCalle,
                IdConvenio = dto.IdConvenio,
                FechaIngresoAlta = dto.FechaIngresoAlta,
                EstadoCuenta = string.IsNullOrWhiteSpace(dto.EstadoCuenta) ? "ACTIVO" : dto.EstadoCuenta,
                IdPersonaNavigation = new Models.Tablas.Persona
                {
                    Dni = dto.Dni,
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    FechaNacimiento = dto.FechaNacimiento,
                    EstadoCivil = dto.EstadoCivil,
                    Ocupacion = dto.Ocupacion,
                    Telefono = dto.Telefono,
                    Email = dto.Email
                }
            };

            var guardado = await repo.CreateSocioAsync(entidad);
            return new SocioDto
            {
                IdSocio = guardado.IdSocio,
                Dni = guardado.IdPersonaNavigation?.Dni ?? dto.Dni,
                Nombre = guardado.IdPersonaNavigation?.Nombre ?? dto.Nombre,
                Apellido = guardado.IdPersonaNavigation?.Apellido ?? dto.Apellido,
                Email = guardado.IdPersonaNavigation?.Email ?? dto.Email,
                Telefono = guardado.IdPersonaNavigation?.Telefono ?? dto.Telefono,
                EstadoCuenta = guardado.EstadoCuenta,
                FechaIngresoAlta = guardado.FechaIngresoAlta
            };
        }

        public async Task<SocioContratoResponseDto> CreateSocioConContratoAsync(SocioConContratoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Socio == null)
            {
                throw new ArgumentException("Los datos del socio son obligatorios.");
            }

            if (dto.Contrato == null)
            {
                throw new ArgumentException("Los datos del contrato son obligatorios.");
            }

            var entidadSocio = new Models.Tablas.Socio
            {
                IdCalle = dto.Socio.IdCalle,
                IdConvenio = dto.Socio.IdConvenio,
                FechaIngresoAlta = dto.Socio.FechaIngresoAlta,
                EstadoCuenta = string.IsNullOrWhiteSpace(dto.Socio.EstadoCuenta) ? "ACTIVO" : dto.Socio.EstadoCuenta,
                IdPersonaNavigation = new Models.Tablas.Persona
                {
                    Dni = dto.Socio.Dni,
                    Nombre = dto.Socio.Nombre,
                    Apellido = dto.Socio.Apellido,
                    FechaNacimiento = dto.Socio.FechaNacimiento,
                    EstadoCivil = dto.Socio.EstadoCivil,
                    Ocupacion = dto.Socio.Ocupacion,
                    Telefono = dto.Socio.Telefono,
                    Email = dto.Socio.Email
                }
            };

            var entidadContrato = new Models.Tablas.ContratosMembresium
            {
                IdPlan = dto.Contrato.IdPlan,
                IdSede = dto.Contrato.IdSede,
                FechaInicio = dto.Contrato.FechaInicio,
                FechaVencimiento = dto.Contrato.FechaVencimiento,
                EstadoContrato = string.IsNullOrWhiteSpace(dto.Contrato.EstadoContrato) ? "ACTIVO" : dto.Contrato.EstadoContrato
            };

            var resultado = await repo.CreateSocioConContratoAsync(entidadSocio, entidadContrato, dto.IdFormaPago);
            return new SocioContratoResponseDto
            {
                IdSocio = resultado.Socio.IdSocio,
                IdContrato = resultado.Contrato.IdContrato,
                CuotasGeneradas = resultado.Cuotas.Count
            };
        }

        public async Task<SocioDetalleDto?> UpdateSocioAsync(int idSocio, SocioCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var socioActual = await repo.GetSocioByIdAsync(idSocio);
            if (socioActual == null)
            {
                return null;
            }

            var socios = await repo.GetSociosAsync();
            if (socios.Any(s => s.IdSocio != idSocio && s.IdPersonaNavigation != null && s.IdPersonaNavigation.Dni == dto.Dni.Trim()))
            {
                throw new InvalidOperationException("Ya existe otro socio con ese DNI.");
            }

            var actualizado = await repo.UpdateSocioAsync(idSocio, new Models.Tablas.Socio
            {
                IdSocio = idSocio,
                IdCalle = dto.IdCalle,
                IdConvenio = dto.IdConvenio,
                FechaIngresoAlta = dto.FechaIngresoAlta,
                EstadoCuenta = dto.EstadoCuenta,
                IdPersonaNavigation = new Models.Tablas.Persona
                {
                    IdPersona = socioActual.IdPersona,
                    Dni = dto.Dni,
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    FechaNacimiento = dto.FechaNacimiento,
                    EstadoCivil = dto.EstadoCivil,
                    Ocupacion = dto.Ocupacion,
                    Telefono = dto.Telefono,
                    Email = dto.Email
                }
            });

            if (actualizado == null)
            {
                return null;
            }

            return new SocioDetalleDto
            {
                IdSocio = actualizado.IdSocio,
                IdPersona = actualizado.IdPersona,
                Dni = actualizado.IdPersonaNavigation?.Dni ?? dto.Dni,
                Nombre = actualizado.IdPersonaNavigation?.Nombre ?? dto.Nombre,
                Apellido = actualizado.IdPersonaNavigation?.Apellido ?? dto.Apellido,
                Email = actualizado.IdPersonaNavigation?.Email ?? dto.Email,
                Telefono = actualizado.IdPersonaNavigation?.Telefono ?? dto.Telefono,
                EstadoCivil = actualizado.IdPersonaNavigation?.EstadoCivil,
                Ocupacion = actualizado.IdPersonaNavigation?.Ocupacion,
                FechaNacimiento = actualizado.IdPersonaNavigation?.FechaNacimiento,
                FechaIngresoAlta = actualizado.FechaIngresoAlta,
                EstadoCuenta = actualizado.EstadoCuenta,
                IdCalle = actualizado.IdCalle,
                IdConvenio = actualizado.IdConvenio
            };
        }

        public async Task<bool> DeleteSocioAsync(int idSocio)
        {
            var socio = await repo.GetSocioByIdAsync(idSocio);
            if (socio == null)
            {
                return false;
            }

            var contratos = await repo.GetContratosAsync(idSocio);
            if (contratos.Any(c => c.EstadoContrato == "ACTIVO" || c.FechaVencimiento >= DateOnly.FromDateTime(DateTime.Today)))
            {
                throw new InvalidOperationException("No se puede eliminar un socio con contratos activos o vigentes.");
            }

            return await repo.DeleteSocioAsync(idSocio);
        }

        public async Task<List<PersonaDto>> GetPersonasAsync()
        {
            var personas = await repo.GetPersonasAsync();
            return personas.Select(p => new PersonaDto
            {
                IdPersona = p.IdPersona,
                Dni = p.Dni,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                FechaNacimiento = p.FechaNacimiento,
                EstadoCivil = p.EstadoCivil,
                Ocupacion = p.Ocupacion,
                Telefono = p.Telefono,
                Email = p.Email
            }).ToList();
        }

        public async Task<PersonaDto?> GetPersonaByIdAsync(int idPersona)
        {
            var persona = await repo.GetPersonaByIdAsync(idPersona);
            if (persona == null)
            {
                return null;
            }

            return new PersonaDto
            {
                IdPersona = persona.IdPersona,
                Dni = persona.Dni,
                Nombre = persona.Nombre,
                Apellido = persona.Apellido,
                FechaNacimiento = persona.FechaNacimiento,
                EstadoCivil = persona.EstadoCivil,
                Ocupacion = persona.Ocupacion,
                Telefono = persona.Telefono,
                Email = persona.Email
            };
        }

        public async Task<PersonaDto> CreatePersonaAsync(PersonaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var entidad = new Models.Tablas.Persona
            {
                Dni = dto.Dni.Trim(),
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                FechaNacimiento = dto.FechaNacimiento,
                EstadoCivil = string.IsNullOrWhiteSpace(dto.EstadoCivil) ? "SOLTERO" : dto.EstadoCivil.Trim(),
                Ocupacion = string.IsNullOrWhiteSpace(dto.Ocupacion) ? "SIN_DATO" : dto.Ocupacion.Trim(),
                Telefono = dto.Telefono,
                Email = dto.Email
            };

            var creada = await repo.CreatePersonaAsync(entidad);
            return new PersonaDto
            {
                IdPersona = creada.IdPersona,
                Dni = creada.Dni,
                Nombre = creada.Nombre,
                Apellido = creada.Apellido,
                FechaNacimiento = creada.FechaNacimiento,
                EstadoCivil = creada.EstadoCivil,
                Ocupacion = creada.Ocupacion,
                Telefono = creada.Telefono,
                Email = creada.Email
            };
        }

        public async Task<List<SedeDto>> GetSedesAsync()
        {
            var sedes = await repo.GetSedesAsync();
            return sedes.Select(s => new SedeDto
            {
                IdSede = s.IdSede,
                IdCalle = s.IdCalle,
                NombreCalle = s.IdCalleNavigation?.NombreCalle ?? string.Empty,
                NombreSede = s.NombreSede,
                Telefono = s.Telefono,
                Email = s.Email
            }).ToList();
        }

        public async Task<SedeDto?> GetSedeByIdAsync(int idSede)
        {
            var sede = await repo.GetSedeByIdAsync(idSede);
            if (sede == null)
            {
                return null;
            }

            return new SedeDto
            {
                IdSede = sede.IdSede,
                IdCalle = sede.IdCalle,
                NombreCalle = sede.IdCalleNavigation?.NombreCalle ?? string.Empty,
                NombreSede = sede.NombreSede,
                Telefono = sede.Telefono,
                Email = sede.Email
            };
        }

        public async Task<SedeDto> CreateSedeAsync(SedeCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var creada = await repo.CreateSedeAsync(new Models.Tablas.Sede
            {
                IdCalle = dto.IdCalle,
                NombreSede = dto.NombreSede.Trim(),
                Telefono = dto.Telefono,
                Email = dto.Email
            });

            return new SedeDto
            {
                IdSede = creada.IdSede,
                IdCalle = creada.IdCalle,
                NombreCalle = creada.IdCalleNavigation?.NombreCalle ?? string.Empty,
                NombreSede = creada.NombreSede,
                Telefono = creada.Telefono,
                Email = creada.Email
            };
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            var roles = await repo.GetRolesAsync();
            return roles.Select(r => new RoleDto
            {
                IdRol = r.IdRol,
                NombreRol = r.NombreRol
            }).ToList();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int idRol)
        {
            var rol = await repo.GetRoleByIdAsync(idRol);
            if (rol == null)
            {
                return null;
            }

            return new RoleDto
            {
                IdRol = rol.IdRol,
                NombreRol = rol.NombreRol
            };
        }

        public async Task<RoleDto> CreateRoleAsync(RoleCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var created = await repo.CreateRoleAsync(new Models.Tablas.Role
            {
                NombreRol = dto.NombreRol.Trim()
            });

            return new RoleDto
            {
                IdRol = created.IdRol,
                NombreRol = created.NombreRol
            };
        }

        public async Task<List<CategoriaProductoDto>> GetCategoriasProductosAsync()
        {
            var categorias = await repo.GetCategoriasProductosAsync();
            return categorias.Select(c => new CategoriaProductoDto
            {
                IdCategoria = c.IdCategoria,
                NombreCategoria = c.NombreCategoria
            }).ToList();
        }

        public async Task<CategoriaProductoDto?> GetCategoriaProductoByIdAsync(int idCategoria)
        {
            var categoria = await repo.GetCategoriaProductoByIdAsync(idCategoria);
            if (categoria == null)
            {
                return null;
            }

            return new CategoriaProductoDto
            {
                IdCategoria = categoria.IdCategoria,
                NombreCategoria = categoria.NombreCategoria
            };
        }

        public async Task<CategoriaProductoDto> CreateCategoriaProductoAsync(CategoriaProductoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.NombreCategoria))
            {
                throw new ArgumentException("El nombre de la categoría es obligatorio.");
            }

            var creada = await repo.CreateCategoriaProductoAsync(new Models.Tablas.CategoriasProducto
            {
                NombreCategoria = dto.NombreCategoria.Trim()
            });

            return new CategoriaProductoDto
            {
                IdCategoria = creada.IdCategoria,
                NombreCategoria = creada.NombreCategoria
            };
        }

        public async Task<List<FormaPagoDto>> GetFormasPagoAsync()
        {
            var formas = await repo.GetFormasPagoAsync();
            return formas.Select(f => new FormaPagoDto
            {
                IdFormaPago = f.IdFormaPago,
                NombreFormaPago = f.NombreFormaPago
            }).ToList();
        }

        public async Task<FormaPagoDto?> GetFormaPagoByIdAsync(int idFormaPago)
        {
            var forma = await repo.GetFormaPagoByIdAsync(idFormaPago);
            if (forma == null)
            {
                return null;
            }

            return new FormaPagoDto
            {
                IdFormaPago = forma.IdFormaPago,
                NombreFormaPago = forma.NombreFormaPago
            };
        }

        public async Task<FormaPagoDto> CreateFormaPagoAsync(FormaPagoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.NombreFormaPago))
            {
                throw new ArgumentException("El nombre de la forma de pago es obligatorio.");
            }

            var creada = await repo.CreateFormaPagoAsync(new Models.Tablas.FormasPago
            {
                NombreFormaPago = dto.NombreFormaPago.Trim()
            });

            return new FormaPagoDto
            {
                IdFormaPago = creada.IdFormaPago,
                NombreFormaPago = creada.NombreFormaPago
            };
        }

        public async Task<List<EstadoReclamoDto>> GetEstadosReclamoAsync()
        {
            var estados = await repo.GetEstadosReclamoAsync();
            return estados.Select(e => new EstadoReclamoDto
            {
                IdEstadoReclamo = e.IdEstadoReclamo,
                NombreEstado = e.NombreEstado
            }).ToList();
        }

        public async Task<EstadoReclamoDto?> GetEstadoReclamoByIdAsync(int idEstadoReclamo)
        {
            var estado = await repo.GetEstadoReclamoByIdAsync(idEstadoReclamo);
            if (estado == null)
            {
                return null;
            }

            return new EstadoReclamoDto
            {
                IdEstadoReclamo = estado.IdEstadoReclamo,
                NombreEstado = estado.NombreEstado
            };
        }

        public async Task<EstadoReclamoDto> CreateEstadoReclamoAsync(EstadoReclamoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.NombreEstado))
            {
                throw new ArgumentException("El nombre del estado es obligatorio.");
            }

            var creada = await repo.CreateEstadoReclamoAsync(new Models.Tablas.EstadosReclamo
            {
                NombreEstado = dto.NombreEstado.Trim()
            });

            return new EstadoReclamoDto
            {
                IdEstadoReclamo = creada.IdEstadoReclamo,
                NombreEstado = creada.NombreEstado
            };
        }

        public async Task<List<ProvinciaDto>> GetProvinciasAsync()
        {
            var provincias = await repo.GetProvinciasAsync();
            return provincias.Select(p => new ProvinciaDto
            {
                IdProvincia = p.IdProvincia,
                NombreProvincia = p.NombreProvincia
            }).ToList();
        }

        public async Task<ProvinciaDto?> GetProvinciaByIdAsync(int idProvincia)
        {
            var provincia = await repo.GetProvinciaByIdAsync(idProvincia);
            if (provincia == null)
            {
                return null;
            }

            return new ProvinciaDto
            {
                IdProvincia = provincia.IdProvincia,
                NombreProvincia = provincia.NombreProvincia
            };
        }

        public async Task<ProvinciaDto> CreateProvinciaAsync(ProvinciaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.NombreProvincia))
            {
                throw new ArgumentException("El nombre de la provincia es obligatorio.");
            }

            var creada = await repo.CreateProvinciaAsync(new Models.Tablas.Provincia
            {
                NombreProvincia = dto.NombreProvincia.Trim()
            });

            return new ProvinciaDto
            {
                IdProvincia = creada.IdProvincia,
                NombreProvincia = creada.NombreProvincia
            };
        }

        public async Task<List<LocalidadDto>> GetLocalidadesAsync(int? idProvincia = null)
        {
            var localidades = await repo.GetLocalidadesAsync(idProvincia);
            return localidades.Select(l => new LocalidadDto
            {
                IdLocalidad = l.IdLocalidad,
                IdProvincia = l.IdProvincia,
                NombreProvincia = l.IdProvinciaNavigation?.NombreProvincia ?? string.Empty,
                NombreLocalidad = l.NombreLocalidad
            }).ToList();
        }

        public async Task<LocalidadDto?> GetLocalidadByIdAsync(int idLocalidad)
        {
            var localidad = await repo.GetLocalidadByIdAsync(idLocalidad);
            if (localidad == null)
            {
                return null;
            }

            return new LocalidadDto
            {
                IdLocalidad = localidad.IdLocalidad,
                IdProvincia = localidad.IdProvincia,
                NombreProvincia = localidad.IdProvinciaNavigation?.NombreProvincia ?? string.Empty,
                NombreLocalidad = localidad.NombreLocalidad
            };
        }

        public async Task<LocalidadDto> CreateLocalidadAsync(LocalidadCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.IdProvincia <= 0)
            {
                throw new ArgumentException("La provincia es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(dto.NombreLocalidad))
            {
                throw new ArgumentException("El nombre de la localidad es obligatorio.");
            }

            var creada = await repo.CreateLocalidadAsync(new Models.Tablas.Localidade
            {
                IdProvincia = dto.IdProvincia,
                NombreLocalidad = dto.NombreLocalidad.Trim()
            });

            return new LocalidadDto
            {
                IdLocalidad = creada.IdLocalidad,
                IdProvincia = creada.IdProvincia,
                NombreProvincia = creada.IdProvinciaNavigation?.NombreProvincia ?? string.Empty,
                NombreLocalidad = creada.NombreLocalidad
            };
        }

        public async Task<List<BarrioDto>> GetBarriosAsync(int? idLocalidad = null)
        {
            var barrios = await repo.GetBarriosAsync(idLocalidad);
            return barrios.Select(b => new BarrioDto
            {
                IdBarrio = b.IdBarrio,
                IdLocalidad = b.IdLocalidad,
                NombreLocalidad = b.IdLocalidadNavigation?.NombreLocalidad ?? string.Empty,
                NombreBarrio = b.NombreBarrio
            }).ToList();
        }

        public async Task<BarrioDto?> GetBarrioByIdAsync(int idBarrio)
        {
            var barrio = await repo.GetBarrioByIdAsync(idBarrio);
            if (barrio == null)
            {
                return null;
            }

            return new BarrioDto
            {
                IdBarrio = barrio.IdBarrio,
                IdLocalidad = barrio.IdLocalidad,
                NombreLocalidad = barrio.IdLocalidadNavigation?.NombreLocalidad ?? string.Empty,
                NombreBarrio = barrio.NombreBarrio
            };
        }

        public async Task<BarrioDto> CreateBarrioAsync(BarrioCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.IdLocalidad <= 0)
            {
                throw new ArgumentException("La localidad es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(dto.NombreBarrio))
            {
                throw new ArgumentException("El nombre del barrio es obligatorio.");
            }

            var creada = await repo.CreateBarrioAsync(new Models.Tablas.Barrio
            {
                IdLocalidad = dto.IdLocalidad,
                NombreBarrio = dto.NombreBarrio.Trim()
            });

            return new BarrioDto
            {
                IdBarrio = creada.IdBarrio,
                IdLocalidad = creada.IdLocalidad,
                NombreLocalidad = creada.IdLocalidadNavigation?.NombreLocalidad ?? string.Empty,
                NombreBarrio = creada.NombreBarrio
            };
        }

        public async Task<List<CalleDto>> GetCallesAsync(int? idBarrio = null)
        {
            var calles = await repo.GetCallesAsync(idBarrio);
            return calles.Select(c => new CalleDto
            {
                IdCalle = c.IdCalle,
                IdBarrio = c.IdBarrio,
                NombreBarrio = c.IdBarrioNavigation?.NombreBarrio ?? string.Empty,
                NombreCalle = c.NombreCalle,
                Numeracion = c.Numeracion
            }).ToList();
        }

        public async Task<CalleDto?> GetCalleByIdAsync(int idCalle)
        {
            var calle = await repo.GetCalleByIdAsync(idCalle);
            if (calle == null)
            {
                return null;
            }

            return new CalleDto
            {
                IdCalle = calle.IdCalle,
                IdBarrio = calle.IdBarrio,
                NombreBarrio = calle.IdBarrioNavigation?.NombreBarrio ?? string.Empty,
                NombreCalle = calle.NombreCalle,
                Numeracion = calle.Numeracion
            };
        }

        public async Task<CalleDto> CreateCalleAsync(CalleCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.IdBarrio <= 0)
            {
                throw new ArgumentException("El barrio es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.NombreCalle))
            {
                throw new ArgumentException("El nombre de la calle es obligatorio.");
            }

            var creada = await repo.CreateCalleAsync(new Models.Tablas.Calle
            {
                IdBarrio = dto.IdBarrio,
                NombreCalle = dto.NombreCalle.Trim(),
                Numeracion = dto.Numeracion
            });

            return new CalleDto
            {
                IdCalle = creada.IdCalle,
                IdBarrio = creada.IdBarrio,
                NombreBarrio = creada.IdBarrioNavigation?.NombreBarrio ?? string.Empty,
                NombreCalle = creada.NombreCalle,
                Numeracion = creada.Numeracion
            };
        }

        public async Task<List<PlanDto>> GetPlanesAsync()
        {
            var planes = await repo.GetPlanesAsync();
            return planes.Select(p => new PlanDto
            {
                IdPlan = p.IdPlan,
                NombrePlan = p.NombrePlan,
                Descripcion = p.Descripcion,
                PrecioActual = p.PrecioActual,
                DuracionMeses = p.DuracionMeses,
                PermiteMultisede = p.PermiteMultisede
            }).ToList();
        }

        public async Task<List<CuotaCobroDto>> GetCuotasPendientesAsync(int? idSocio = null)
        {
            var cuotas = await repo.GetCuotasPendientesAsync(idSocio);
            return cuotas.Select(c => new CuotaCobroDto
            {
                IdCobro = c.IdCobro,
                IdContrato = c.IdContrato,
                NumeroCuota = c.NumeroCuota,
                Monto = c.Monto,
                FechaVencimiento = c.FechaVencimiento,
                Estado = c.FechaPago.HasValue ? "CANCELADA" : "PENDIENTE",
                SocioNombre = c.IdContratoNavigation?.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{c.IdContratoNavigation.IdSocioNavigation.IdPersonaNavigation.Apellido}, {c.IdContratoNavigation.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdFormaPago = c.IdFormaPago
            }).ToList();
        }

        public async Task<List<PagoCuotaHistoricoDto>> GetPagosCuotaAsync(int? idCobro = null, int? idSocio = null)
        {
            var pagos = await repo.GetPagosCuotaAsync(idCobro, idSocio);
            return pagos.Select(p => new PagoCuotaHistoricoDto
            {
                IdPago = p.IdPago,
                IdCobro = p.IdCobro,
                NumeroCuota = p.IdCobroNavigation?.NumeroCuota ?? 0,
                IdSocio = p.IdCobroNavigation?.IdContratoNavigation?.IdSocio ?? 0,
                NombreSocio = p.IdCobroNavigation?.IdContratoNavigation?.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{p.IdCobroNavigation.IdContratoNavigation.IdSocioNavigation.IdPersonaNavigation.Apellido}, {p.IdCobroNavigation.IdContratoNavigation.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdFormaPago = p.IdFormaPago,
                NombreFormaPago = p.IdFormaPagoNavigation?.NombreFormaPago ?? string.Empty,
                Importe = p.Importe,
                FechaPago = p.FechaPago,
                UsuarioSistema = p.UsuarioSistema
            }).ToList();
        }

        public async Task<PagoCuotaResponseDto> RegistrarPagoCuotaAsync(PagoCuotaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Importe <= 0)
            {
                throw new ArgumentException("El importe debe ser mayor a cero.");
            }

            var pago = await repo.RegistrarPagoCuotaAsync(dto.IdCobro, dto.IdFormaPago, dto.Importe, dto.UsuarioSistema);

            return new PagoCuotaResponseDto
            {
                IdPago = pago.IdPago,
                IdCobro = pago.IdCobro,
                Importe = pago.Importe,
                FechaPago = pago.FechaPago,
                UsuarioSistema = pago.UsuarioSistema,
                CuotaPendiente = 0m
            };
        }

        public async Task<List<AptoMedicoDto>> GetAptosMedicosAsync()
        {
            var aptos = await repo.GetAptosMedicosAsync();
            return aptos.Select(a => new AptoMedicoDto
            {
                IdApto = a.IdApto,
                IdSocio = a.IdSocio,
                FechaEmision = a.FechaEmision,
                FechaVencimiento = a.FechaVencimiento,
                MedicoFirmante = a.MedicoFirmante,
                MatriculaMedico = a.MatriculaMedico,
                Observaciones = a.Observaciones
            }).ToList();
        }

        public async Task<AptoMedicoDto> CreateAptoMedicoAsync(AptoMedicoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.FechaVencimiento <= dto.FechaEmision)
            {
                throw new ArgumentException("La fecha de vencimiento debe ser posterior a la fecha de emisión.");
            }

            var creado = await repo.CreateAptoMedicoAsync(new Models.Tablas.AptosMedico
            {
                IdSocio = dto.IdSocio,
                FechaEmision = dto.FechaEmision,
                FechaVencimiento = dto.FechaVencimiento,
                MedicoFirmante = dto.MedicoFirmante,
                MatriculaMedico = dto.MatriculaMedico,
                Observaciones = dto.Observaciones
            });

            return new AptoMedicoDto
            {
                IdApto = creado.IdApto,
                IdSocio = creado.IdSocio,
                FechaEmision = creado.FechaEmision,
                FechaVencimiento = creado.FechaVencimiento,
                MedicoFirmante = creado.MedicoFirmante,
                MatriculaMedico = creado.MatriculaMedico,
                Observaciones = creado.Observaciones
            };
        }

        public async Task<AccesoValidacionResponseDto> ValidarAccesoAsync(int idSocio, int idSede, DateTime fechaHora)
        {
            if (idSocio <= 0)
            {
                throw new ArgumentException("El socio es obligatorio.");
            }

            if (idSede <= 0)
            {
                throw new ArgumentException("La sede es obligatoria.");
            }

            return await repo.ValidarAccesoAsync(idSocio, idSede, fechaHora);
        }

        public async Task<List<AccesoLogDto>> GetAccesosLogsAsync(int? idSede = null)
        {
            var logs = await repo.GetAccesosLogsAsync(idSede);
            return logs.Select(a => new AccesoLogDto
            {
                IdAcceso = a.IdAcceso,
                IdSocio = a.IdSocio,
                IdSede = a.IdSede,
                NombreSede = a.IdSedeNavigation?.NombreSede ?? string.Empty,
                FechaHoraMarcado = a.FechaHoraMarcado,
                AccesoPermitido = a.AccesoPermitido,
                MotivoRechazo = a.MotivoRechazo,
                SocioNombre = a.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{a.IdSocioNavigation.IdPersonaNavigation.Nombre} {a.IdSocioNavigation.IdPersonaNavigation.Apellido}".Trim()
                    : $"Socio {a.IdSocio}"
            }).ToList();
        }

        public async Task<HistorialSocioDto> GetHistorialSocioAsync(int idSocio)
        {
            if (idSocio <= 0)
            {
                throw new ArgumentException("El socio es obligatorio.");
            }

            return await repo.GetHistorialSocioAsync(idSocio);
        }

        public async Task<List<ReservaDto>> GetReservasEnListaEsperaAsync(int idClase)
        {
            if (idClase <= 0)
            {
                throw new ArgumentException("La clase es obligatoria.");
            }

            var reservas = await repo.GetReservasEnListaEsperaAsync(idClase);
            return reservas.Select(r => new ReservaDto
            {
                IdReserva = r.IdReserva,
                IdSocio = r.IdSocio,
                IdClase = r.IdClase,
                FechaReserva = r.FechaReserva,
                EstadoReserva = r.EstadoReserva
            }).ToList();
        }

        public async Task<ReservaDto?> PromoverSiguienteListaEsperaAsync(int idClase)
        {
            if (idClase <= 0)
            {
                throw new ArgumentException("La clase es obligatoria.");
            }

            var reserva = await repo.PromoverSiguienteListaEsperaAsync(idClase);
            if (reserva == null)
            {
                return null;
            }

            return new ReservaDto
            {
                IdReserva = reserva.IdReserva,
                IdSocio = reserva.IdSocio,
                IdClase = reserva.IdClase,
                FechaReserva = reserva.FechaReserva,
                EstadoReserva = reserva.EstadoReserva
            };
        }

        public async Task<List<ClaseDto>> GetClasesAsync()
        {
            var clases = await repo.GetClasesAsync();
            return clases.Select(c => new ClaseDto
            {
                IdClase = c.IdClase,
                IdDisciplina = c.IdDisciplina,
                NombreDisciplina = c.IdDisciplinaNavigation?.NombreDisciplina,
                IdInstructor = c.IdInstructor,
                NombreInstructor = c.IdInstructorNavigation?.IdPersonaNavigation != null
                    ? $"{c.IdInstructorNavigation.IdPersonaNavigation.Apellido}, {c.IdInstructorNavigation.IdPersonaNavigation.Nombre}"
                    : null,
                IdSalon = c.IdSalon,
                FechaClase = c.FechaClase,
                HoraInicio = c.HoraInicio,
                HoraFin = c.HoraFin,
                CupoLimite = c.CupoLimite
            }).ToList();
        }

        public async Task<List<ReservaDto>> GetReservasAsync(int? idSocio = null)
        {
            var reservas = await repo.GetReservasAsync(idSocio);
            return reservas.Select(r => new ReservaDto
            {
                IdReserva = r.IdReserva,
                IdSocio = r.IdSocio,
                IdClase = r.IdClase,
                FechaReserva = r.FechaReserva,
                EstadoReserva = r.EstadoReserva
            }).ToList();
        }

        public async Task<ReservaDto?> GetReservaByIdAsync(int idReserva)
        {
            var reserva = await repo.GetReservaByIdAsync(idReserva);
            if (reserva == null)
            {
                return null;
            }

            return new ReservaDto
            {
                IdReserva = reserva.IdReserva,
                IdSocio = reserva.IdSocio,
                IdClase = reserva.IdClase,
                FechaReserva = reserva.FechaReserva,
                EstadoReserva = reserva.EstadoReserva
            };
        }

        public async Task<ReservaDto> CreateReservaAsync(ReservaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var creada = await repo.CreateReservaAsync(new Models.Tablas.Reserva
            {
                IdSocio = dto.IdSocio,
                IdClase = dto.IdClase,
                EstadoReserva = dto.EstadoReserva,
                FechaReserva = DateTime.Now
            });

            return new ReservaDto
            {
                IdReserva = creada.IdReserva,
                IdSocio = creada.IdSocio,
                IdClase = creada.IdClase,
                FechaReserva = creada.FechaReserva,
                EstadoReserva = creada.EstadoReserva
            };
        }

        public async Task<ReservaDto?> UpdateReservaAsync(int idReserva, ReservaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var actual = await repo.GetReservaByIdAsync(idReserva);
            if (actual == null)
            {
                return null;
            }

            var actualizado = await repo.UpdateReservaAsync(idReserva, new Models.Tablas.Reserva
            {
                IdSocio = dto.IdSocio,
                IdClase = dto.IdClase,
                EstadoReserva = dto.EstadoReserva,
                FechaReserva = DateTime.Now
            });

            if (actualizado == null)
            {
                return null;
            }

            return new ReservaDto
            {
                IdReserva = actualizado.IdReserva,
                IdSocio = actualizado.IdSocio,
                IdClase = actualizado.IdClase,
                FechaReserva = actualizado.FechaReserva,
                EstadoReserva = actualizado.EstadoReserva
            };
        }

        public async Task<bool> DeleteReservaAsync(int idReserva)
        {
            if (idReserva <= 0)
            {
                throw new ArgumentException("El id de la reserva es inválido.");
            }

            return await repo.DeleteReservaAsync(idReserva);
        }

        public async Task<List<ProductoDto>> GetProductosAsync(int? idSede = null)
        {
            var productos = await repo.GetProductosAsync(idSede);
            return productos.Select(p => new ProductoDto
            {
                IdProducto = p.IdProducto,
                IdCategoria = p.IdCategoria,
                NombreCategoria = p.IdCategoriaNavigation?.NombreCategoria ?? string.Empty,
                NombreProducto = p.NombreProducto,
                CodigoBarras = p.CodigoBarras,
                PrecioActual = p.PrecioActual,
                StockMinimo = p.StockMinimo
            }).ToList();
        }

        public async Task<ProductoDto?> GetProductoByIdAsync(int idProducto)
        {
            var producto = await repo.GetProductoByIdAsync(idProducto);
            if (producto == null)
            {
                return null;
            }

            return new ProductoDto
            {
                IdProducto = producto.IdProducto,
                IdCategoria = producto.IdCategoria,
                NombreCategoria = producto.IdCategoriaNavigation?.NombreCategoria ?? string.Empty,
                NombreProducto = producto.NombreProducto,
                CodigoBarras = producto.CodigoBarras,
                PrecioActual = producto.PrecioActual,
                StockMinimo = producto.StockMinimo
            };
        }

        public async Task<ProductoDto> CreateProductoAsync(ProductoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.IdCategoria <= 0)
            {
                throw new ArgumentException("La categoría es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(dto.NombreProducto))
            {
                throw new ArgumentException("El nombre del producto es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.CodigoBarras))
            {
                throw new ArgumentException("El código de barras es obligatorio.");
            }

            if (dto.PrecioActual <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor a cero.");
            }

            if (dto.StockMinimo < 0)
            {
                throw new ArgumentException("El stock mínimo no puede ser negativo.");
            }

            var productoCreado = await repo.CreateProductoAsync(new Models.Tablas.Producto
            {
                IdCategoria = dto.IdCategoria,
                NombreProducto = dto.NombreProducto.Trim(),
                CodigoBarras = dto.CodigoBarras.Trim(),
                PrecioActual = dto.PrecioActual,
                StockMinimo = dto.StockMinimo
            });

            return new ProductoDto
            {
                IdProducto = productoCreado.IdProducto,
                IdCategoria = productoCreado.IdCategoria,
                NombreCategoria = productoCreado.IdCategoriaNavigation?.NombreCategoria ?? string.Empty,
                NombreProducto = productoCreado.NombreProducto,
                CodigoBarras = productoCreado.CodigoBarras,
                PrecioActual = productoCreado.PrecioActual,
                StockMinimo = productoCreado.StockMinimo
            };
        }

        public async Task<ProductoDto?> UpdateProductoAsync(int idProducto, ProductoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var actual = await repo.GetProductoByIdAsync(idProducto);
            if (actual == null)
            {
                return null;
            }

            var actualizado = await repo.UpdateProductoAsync(idProducto, new Models.Tablas.Producto
            {
                IdProducto = idProducto,
                IdCategoria = dto.IdCategoria,
                NombreProducto = dto.NombreProducto,
                CodigoBarras = dto.CodigoBarras,
                PrecioActual = dto.PrecioActual,
                StockMinimo = dto.StockMinimo
            });

            if (actualizado == null)
            {
                return null;
            }

            return new ProductoDto
            {
                IdProducto = actualizado.IdProducto,
                IdCategoria = actualizado.IdCategoria,
                NombreCategoria = actualizado.IdCategoriaNavigation?.NombreCategoria ?? string.Empty,
                NombreProducto = actualizado.NombreProducto,
                CodigoBarras = actualizado.CodigoBarras,
                PrecioActual = actualizado.PrecioActual,
                StockMinimo = actualizado.StockMinimo
            };
        }

        public async Task<bool> DeleteProductoAsync(int idProducto)
        {
            if (idProducto <= 0)
            {
                throw new ArgumentException("El id del producto es inválido.");
            }

            return await repo.DeleteProductoAsync(idProducto);
        }

        public async Task<List<StockSedeDto>> GetStockPorSedeAsync(int? idSede = null)
        {
            var stock = await repo.GetStockPorSedeAsync(idSede);
            return stock.Select(s => new StockSedeDto
            {
                IdStockSede = s.IdStockSede,
                IdSede = s.IdSede,
                NombreSede = s.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdProducto = s.IdProducto,
                NombreProducto = s.IdProductoNavigation?.NombreProducto ?? string.Empty,
                CantidadActual = s.CantidadActual
            }).ToList();
        }

        public async Task<VentaDto> RegistrarVentaAsync(VentaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Items == null || !dto.Items.Any())
            {
                throw new ArgumentException("Debe enviar al menos un producto para registrar la venta.");
            }

            if (dto.Items.Any(i => i.Cantidad <= 0 || i.PrecioUnitarioHistorico < 0))
            {
                throw new ArgumentException("Las cantidades y precios de venta no pueden ser negativos o cero.");
            }

            var venta = new Models.Tablas.Venta
            {
                IdSocio = dto.IdSocio,
                IdSede = dto.IdSede,
                IdVendedor = dto.IdVendedor,
                IdFormaPago = dto.IdFormaPago,
                ImputadoACuenta = dto.ImputadoACuenta,
                FechaHoraVenta = DateTime.Now,
                DetalleVenta = dto.Items.Select(i => new Models.Tablas.DetalleVenta
                {
                    IdProducto = i.IdProducto,
                    Cantidad = i.Cantidad,
                    PrecioUnitarioHistorico = i.PrecioUnitarioHistorico > 0 ? i.PrecioUnitarioHistorico : 0m
                }).ToList()
            };

            var creada = await repo.CrearVentaAsync(venta);
            var total = creada.DetalleVenta.Sum(d => d.Cantidad * d.PrecioUnitarioHistorico);

            return new VentaDto
            {
                IdVenta = creada.IdVenta,
                IdSocio = creada.IdSocio,
                IdSede = creada.IdSede,
                IdVendedor = creada.IdVendedor,
                IdFormaPago = creada.IdFormaPago,
                FechaHoraVenta = creada.FechaHoraVenta,
                ImputadoACuenta = creada.ImputadoACuenta,
                TotalVenta = total,
                Detalles = creada.DetalleVenta.Select(d => new VentaDetalleDto
                {
                    IdDetalleVenta = d.IdDetalleVenta,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = d.Cantidad,
                    PrecioUnitarioHistorico = d.PrecioUnitarioHistorico
                }).ToList()
            };
        }

        public async Task<List<VentaDto>> GetVentasAsync(int? idSede = null, int? idSocio = null)
        {
            var ventas = await repo.GetVentasAsync(idSede, idSocio);
            return ventas.Select(v => new VentaDto
            {
                IdVenta = v.IdVenta,
                IdSocio = v.IdSocio,
                IdSede = v.IdSede,
                IdVendedor = v.IdVendedor,
                IdFormaPago = v.IdFormaPago,
                FechaHoraVenta = v.FechaHoraVenta,
                ImputadoACuenta = v.ImputadoACuenta,
                TotalVenta = v.DetalleVenta.Sum(d => d.Cantidad * d.PrecioUnitarioHistorico),
                Detalles = v.DetalleVenta.Select(d => new VentaDetalleDto
                {
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = d.Cantidad,
                    PrecioUnitarioHistorico = d.PrecioUnitarioHistorico
                }).ToList()
            }).ToList();
        }

        public async Task<List<EquipoDto>> GetEquiposAsync(int? idSede = null)
        {
            var equipos = await repo.GetEquiposAsync(idSede);
            return equipos.Select(e => new EquipoDto
            {
                IdEquipo = e.IdEquipo,
                IdSede = e.IdSede,
                NombreSede = e.IdSedeNavigation?.NombreSede ?? string.Empty,
                NombreMaquina = e.NombreMaquina,
                NumeroSerie = e.NumeroSerie,
                FechaAdquisicion = e.FechaAdquisicion,
                EstadoOperativo = e.EstadoOperativo
            }).ToList();
        }

        public async Task<EquipoDto> CreateEquipoAsync(EquipoCreateDto dto)
        {
            var equipo = new Models.Tablas.Equipamiento
            {
                IdSede = dto.IdSede,
                NombreMaquina = dto.NombreMaquina.Trim(),
                NumeroSerie = dto.NumeroSerie.Trim(),
                FechaAdquisicion = DateOnly.FromDateTime(dto.FechaAdquisicion),
                EstadoOperativo = string.IsNullOrWhiteSpace(dto.EstadoOperativo) ? "ACTIVO" : dto.EstadoOperativo.Trim().ToUpper()
            };

            var creado = await repo.CreateEquipoAsync(equipo);
            return new EquipoDto
            {
                IdEquipo = creado.IdEquipo,
                IdSede = creado.IdSede,
                NombreSede = creado.IdSedeNavigation?.NombreSede ?? string.Empty,
                NombreMaquina = creado.NombreMaquina,
                NumeroSerie = creado.NumeroSerie,
                FechaAdquisicion = creado.FechaAdquisicion,
                EstadoOperativo = creado.EstadoOperativo
            };
        }

        public async Task<EquipoDto?> UpdateEquipoAsync(int idEquipo, EquipoCreateDto dto)
        {
            var equipo = new Models.Tablas.Equipamiento
            {
                IdSede = dto.IdSede,
                NombreMaquina = dto.NombreMaquina.Trim(),
                NumeroSerie = dto.NumeroSerie.Trim(),
                FechaAdquisicion = DateOnly.FromDateTime(dto.FechaAdquisicion),
                EstadoOperativo = dto.EstadoOperativo
            };

            var actualizado = await repo.UpdateEquipoAsync(idEquipo, equipo);
            if (actualizado == null) return null;

            return new EquipoDto
            {
                IdEquipo = actualizado.IdEquipo,
                IdSede = actualizado.IdSede,
                NombreSede = actualizado.IdSedeNavigation?.NombreSede ?? string.Empty,
                NombreMaquina = actualizado.NombreMaquina,
                NumeroSerie = actualizado.NumeroSerie,
                FechaAdquisicion = actualizado.FechaAdquisicion,
                EstadoOperativo = actualizado.EstadoOperativo
            };
        }

        public async Task<bool> DeleteEquipoAsync(int idEquipo)
        {
            return await repo.DeleteEquipoAsync(idEquipo);
        }

        public async Task<List<IncidenciaDto>> GetIncidenciasAsync(int? idSede = null)
        {
            var incidencias = await repo.GetIncidenciasAsync(idSede);
            return incidencias.Select(i => new IncidenciaDto
            {
                IdReclamo = i.IdReclamo,
                IdSocio = i.IdSocio,
                IdEquipo = i.IdEquipo,
                IdSede = i.IdSede,
                IdTecnicoAsignado = i.IdTecnicoAsignado,
                IdEstadoReclamo = i.IdEstadoReclamo,
                EstadoReclamo = i.IdEstadoReclamoNavigation?.NombreEstado ?? string.Empty,
                FechaRegistro = i.FechaRegistro,
                Descripcion = i.Descripcion
            }).ToList();
        }

        public async Task<IncidenciaDto> CreateIncidenciaAsync(IncidenciaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var creada = await repo.CreateIncidenciaAsync(new Models.Tablas.IncidenciasReclamo
            {
                IdSocio = dto.IdSocio ?? 0,
                IdEquipo = dto.IdEquipo,
                IdSede = dto.IdSede,
                IdTecnicoAsignado = dto.IdTecnicoAsignado,
                IdEstadoReclamo = dto.IdEstadoReclamo,
                Descripcion = dto.Descripcion,
                FechaRegistro = DateTime.Now
            });

            return new IncidenciaDto
            {
                IdReclamo = creada.IdReclamo,
                IdSocio = creada.IdSocio == 0 ? null : creada.IdSocio,
                IdEquipo = creada.IdEquipo,
                IdSede = creada.IdSede,
                IdTecnicoAsignado = creada.IdTecnicoAsignado,
                IdEstadoReclamo = creada.IdEstadoReclamo,
                EstadoReclamo = creada.IdEstadoReclamoNavigation?.NombreEstado ?? string.Empty,
                FechaRegistro = creada.FechaRegistro,
                Descripcion = creada.Descripcion
            };
        }

        public async Task<List<ContratoMembresiaDto>> GetContratosAsync(int? idSocio = null)
        {
            var contratos = await repo.GetContratosAsync(idSocio);
            return contratos.Select(c => new ContratoMembresiaDto
            {
                IdContrato = c.IdContrato,
                IdSocio = c.IdSocio,
                SocioNombre = c.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{c.IdSocioNavigation.IdPersonaNavigation.Apellido}, {c.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdPlan = c.IdPlan,
                NombrePlan = c.IdPlanNavigation?.NombrePlan ?? string.Empty,
                IdSede = c.IdSede,
                NombreSede = c.IdSedeNavigation?.NombreSede ?? string.Empty,
                FechaInicio = c.FechaInicio,
                FechaVencimiento = c.FechaVencimiento,
                EstadoContrato = c.EstadoContrato
            }).ToList();
        }

        public async Task<ContratoMembresiaDto?> GetContratoByIdAsync(int idContrato)
        {
            var contrato = await repo.GetContratoByIdAsync(idContrato);
            if (contrato == null)
            {
                return null;
            }

            return new ContratoMembresiaDto
            {
                IdContrato = contrato.IdContrato,
                IdSocio = contrato.IdSocio,
                SocioNombre = contrato.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{contrato.IdSocioNavigation.IdPersonaNavigation.Apellido}, {contrato.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdPlan = contrato.IdPlan,
                NombrePlan = contrato.IdPlanNavigation?.NombrePlan ?? string.Empty,
                IdSede = contrato.IdSede,
                NombreSede = contrato.IdSedeNavigation?.NombreSede ?? string.Empty,
                FechaInicio = contrato.FechaInicio,
                FechaVencimiento = contrato.FechaVencimiento,
                EstadoContrato = contrato.EstadoContrato
            };
        }

        public async Task<ContratoMembresiaDto> CreateContratoAsync(ContratoMembresiaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.IdSocio <= 0)
            {
                throw new ArgumentException("El socio es obligatorio.");
            }

            if (dto.IdPlan <= 0)
            {
                throw new ArgumentException("El plan es obligatorio.");
            }

            if (dto.IdSede <= 0)
            {
                throw new ArgumentException("La sede es obligatoria.");
            }

            if (dto.FechaVencimiento <= dto.FechaInicio)
            {
                throw new ArgumentException("La fecha de vencimiento debe ser posterior a la fecha de inicio.");
            }

            var creado = await repo.CreateContratoAsync(new Models.Tablas.ContratosMembresium
            {
                IdSocio = dto.IdSocio,
                IdPlan = dto.IdPlan,
                IdSede = dto.IdSede,
                FechaInicio = dto.FechaInicio,
                FechaVencimiento = dto.FechaVencimiento,
                EstadoContrato = dto.EstadoContrato
            });

            return new ContratoMembresiaDto
            {
                IdContrato = creado.IdContrato,
                IdSocio = creado.IdSocio,
                IdPlan = creado.IdPlan,
                IdSede = creado.IdSede,
                FechaInicio = creado.FechaInicio,
                FechaVencimiento = creado.FechaVencimiento,
                EstadoContrato = creado.EstadoContrato
            };
        }

        public async Task<ContratoMembresiaDto> RenovarContratoAsync(int idContrato, ContratoMembresiaCreateDto dto, int idFormaPago = 1)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (idContrato <= 0)
            {
                throw new ArgumentException("El id del contrato es inválido.");
            }

            var existente = await repo.GetContratoByIdAsync(idContrato)
                ?? throw new KeyNotFoundException("El contrato a renovar no existe.");

            var renovado = await repo.RenovarContratoAsync(idContrato, new Models.Tablas.ContratosMembresium
            {
                IdSocio = existente.IdSocio,
                IdPlan = dto.IdPlan,
                IdSede = dto.IdSede,
                FechaInicio = dto.FechaInicio,
                FechaVencimiento = dto.FechaVencimiento,
                EstadoContrato = dto.EstadoContrato
            }, idFormaPago);

            return new ContratoMembresiaDto
            {
                IdContrato = renovado.IdContrato,
                IdSocio = renovado.IdSocio,
                SocioNombre = renovado.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{renovado.IdSocioNavigation.IdPersonaNavigation.Apellido}, {renovado.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdPlan = renovado.IdPlan,
                NombrePlan = renovado.IdPlanNavigation?.NombrePlan ?? string.Empty,
                IdSede = renovado.IdSede,
                NombreSede = renovado.IdSedeNavigation?.NombreSede ?? string.Empty,
                FechaInicio = renovado.FechaInicio,
                FechaVencimiento = renovado.FechaVencimiento,
                EstadoContrato = renovado.EstadoContrato
            };
        }

        public async Task<ContratoMembresiaDto?> UpdateContratoAsync(int idContrato, ContratoMembresiaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (idContrato <= 0)
            {
                throw new ArgumentException("El id del contrato es inválido.");
            }

            var actual = await repo.GetContratoByIdAsync(idContrato);
            if (actual == null)
            {
                return null;
            }

            var actualizado = await repo.UpdateContratoAsync(idContrato, new Models.Tablas.ContratosMembresium
            {
                IdSocio = dto.IdSocio,
                IdPlan = dto.IdPlan,
                IdSede = dto.IdSede,
                FechaInicio = dto.FechaInicio,
                FechaVencimiento = dto.FechaVencimiento,
                EstadoContrato = dto.EstadoContrato
            });

            if (actualizado == null)
            {
                return null;
            }

            return new ContratoMembresiaDto
            {
                IdContrato = actualizado.IdContrato,
                IdSocio = actualizado.IdSocio,
                SocioNombre = actualizado.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{actualizado.IdSocioNavigation.IdPersonaNavigation.Apellido}, {actualizado.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdPlan = actualizado.IdPlan,
                NombrePlan = actualizado.IdPlanNavigation?.NombrePlan ?? string.Empty,
                IdSede = actualizado.IdSede,
                NombreSede = actualizado.IdSedeNavigation?.NombreSede ?? string.Empty,
                FechaInicio = actualizado.FechaInicio,
                FechaVencimiento = actualizado.FechaVencimiento,
                EstadoContrato = actualizado.EstadoContrato
            };
        }

        public async Task<bool> DeleteContratoAsync(int idContrato)
        {
            if (idContrato <= 0)
            {
                throw new ArgumentException("El id del contrato es inválido.");
            }

            return await repo.DeleteContratoAsync(idContrato);
        }

        public async Task<List<ConvenioEmpresaDto>> GetConveniosAsync()
        {
            var convenios = await repo.GetConveniosAsync();
            return convenios.Select(c => new ConvenioEmpresaDto
            {
                IdConvenio = c.IdConvenio,
                RazonSocial = c.RazonSocial,
                Cuit = c.Cuit,
                PorcentajeDescuento = c.PorcentajeDescuento,
                DiaCierreFacturacion = c.DiaCierreFacturacion,
                Telefono = c.Telefono,
                Email = c.Email
            }).ToList();
        }

        public async Task<ConvenioEmpresaDto?> GetConvenioByIdAsync(int idConvenio)
        {
            var convenio = await repo.GetConvenioByIdAsync(idConvenio);
            if (convenio == null)
            {
                return null;
            }

            return new ConvenioEmpresaDto
            {
                IdConvenio = convenio.IdConvenio,
                RazonSocial = convenio.RazonSocial,
                Cuit = convenio.Cuit,
                PorcentajeDescuento = convenio.PorcentajeDescuento,
                DiaCierreFacturacion = convenio.DiaCierreFacturacion,
                Telefono = convenio.Telefono,
                Email = convenio.Email
            };
        }

        public async Task<ConvenioEmpresaDto> CreateConvenioAsync(ConvenioEmpresaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.RazonSocial))
            {
                throw new ArgumentException("La razón social es obligatoria.");
            }

            var creado = await repo.CreateConvenioAsync(new Models.Tablas.ConveniosEmpresa
            {
                RazonSocial = dto.RazonSocial,
                Cuit = dto.Cuit,
                PorcentajeDescuento = dto.PorcentajeDescuento,
                DiaCierreFacturacion = dto.DiaCierreFacturacion,
                Telefono = dto.Telefono,
                Email = dto.Email
            });

            return new ConvenioEmpresaDto
            {
                IdConvenio = creado.IdConvenio,
                RazonSocial = creado.RazonSocial,
                Cuit = creado.Cuit,
                PorcentajeDescuento = creado.PorcentajeDescuento,
                DiaCierreFacturacion = creado.DiaCierreFacturacion,
                Telefono = creado.Telefono,
                Email = creado.Email
            };
        }

        public async Task<ConvenioEmpresaDto?> UpdateConvenioAsync(int idConvenio, ConvenioEmpresaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var actualizado = await repo.UpdateConvenioAsync(idConvenio, new Models.Tablas.ConveniosEmpresa
            {
                RazonSocial = dto.RazonSocial,
                Cuit = dto.Cuit,
                PorcentajeDescuento = dto.PorcentajeDescuento,
                DiaCierreFacturacion = dto.DiaCierreFacturacion,
                Telefono = dto.Telefono,
                Email = dto.Email
            });

            if (actualizado == null)
            {
                return null;
            }

            return new ConvenioEmpresaDto
            {
                IdConvenio = actualizado.IdConvenio,
                RazonSocial = actualizado.RazonSocial,
                Cuit = actualizado.Cuit,
                PorcentajeDescuento = actualizado.PorcentajeDescuento,
                DiaCierreFacturacion = actualizado.DiaCierreFacturacion,
                Telefono = actualizado.Telefono,
                Email = actualizado.Email
            };
        }

        public async Task<bool> DeleteConvenioAsync(int idConvenio)
        {
            if (idConvenio <= 0)
            {
                throw new ArgumentException("El id del convenio es inválido.");
            }

            return await repo.DeleteConvenioAsync(idConvenio);
        }

        public async Task<List<ProveedorDto>> GetProveedoresAsync()
        {
            var proveedores = await repo.GetProveedoresAsync();
            return proveedores.Select(p => new ProveedorDto
            {
                IdProveedor = p.IdProveedor,
                RazonSocial = p.RazonSocial,
                Cuit = p.Cuit,
                Telefono = p.Telefono,
                Email = p.Email
            }).ToList();
        }

        public async Task<ProveedorDto> CreateProveedorAsync(ProveedorCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var creado = await repo.CreateProveedorAsync(new Models.Tablas.Proveedore
            {
                RazonSocial = dto.RazonSocial,
                Cuit = dto.Cuit,
                Telefono = dto.Telefono,
                Email = dto.Email
            });

            return new ProveedorDto
            {
                IdProveedor = creado.IdProveedor,
                RazonSocial = creado.RazonSocial,
                Cuit = creado.Cuit,
                Telefono = creado.Telefono,
                Email = creado.Email
            };
        }

        public async Task<List<CompraStockDto>> GetComprasAsync(int? idSede = null)
        {
            var compras = await repo.GetComprasAsync(idSede);
            return compras.Select(c => new CompraStockDto
            {
                IdCompra = c.IdCompra,
                IdProveedor = c.IdProveedor,
                RazonSocialProveedor = c.IdProveedorNavigation?.RazonSocial ?? string.Empty,
                IdSede = c.IdSede,
                NombreSede = c.IdSedeNavigation?.NombreSede ?? string.Empty,
                FechaCompra = c.FechaCompra,
                TotalCompra = c.DetalleComprasStocks.Sum(d => d.Cantidad * d.PrecioCostoUnitario),
                Detalles = c.DetalleComprasStocks.Select(d => new CompraDetalleStockDto
                {
                    IdDetalleCompra = d.IdDetalleCompra,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = d.Cantidad,
                    PrecioCostoUnitario = d.PrecioCostoUnitario
                }).ToList()
            }).ToList();
        }

        public async Task<CompraStockDto> CreateCompraAsync(CompraStockCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Detalles == null || !dto.Detalles.Any())
            {
                throw new ArgumentException("La compra debe incluir al menos un detalle.");
            }

            if (dto.Detalles.Any(d => d.Cantidad <= 0 || d.PrecioCostoUnitario < 0))
            {
                throw new ArgumentException("Las cantidades y precios de costo no pueden ser negativos o cero.");
            }

            var creada = await repo.CreateCompraAsync(new Models.Tablas.ComprasStock
            {
                IdProveedor = dto.IdProveedor,
                IdSede = dto.IdSede,
                FechaCompra = dto.FechaCompra,
                DetalleComprasStocks = dto.Detalles.Select(d => new Models.Tablas.DetalleComprasStock
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    PrecioCostoUnitario = d.PrecioCostoUnitario
                }).ToList()
            });

            return new CompraStockDto
            {
                IdCompra = creada.IdCompra,
                IdProveedor = creada.IdProveedor,
                RazonSocialProveedor = creada.IdProveedorNavigation?.RazonSocial ?? string.Empty,
                IdSede = creada.IdSede,
                NombreSede = creada.IdSedeNavigation?.NombreSede ?? string.Empty,
                FechaCompra = creada.FechaCompra,
                TotalCompra = creada.DetalleComprasStocks.Sum(d => d.Cantidad * d.PrecioCostoUnitario),
                Detalles = creada.DetalleComprasStocks.Select(d => new CompraDetalleStockDto
                {
                    IdDetalleCompra = d.IdDetalleCompra,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = d.Cantidad,
                    PrecioCostoUnitario = d.PrecioCostoUnitario
                }).ToList()
            };
        }

        public async Task<List<CajaDto>> GetCajasAsync(int? idSede = null)
        {
            var cajas = await repo.GetCajasAsync(idSede);
            return cajas.Select(c => new CajaDto
            {
                IdCaja = c.IdCaja,
                IdSede = c.IdSede,
                NombreSede = c.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEmpleado = c.IdEmpleado,
                NombreEmpleado = c.IdEmpleadoNavigation?.IdPersonaNavigation != null
                    ? $"{c.IdEmpleadoNavigation.IdPersonaNavigation.Apellido}, {c.IdEmpleadoNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                FechaApertura = c.FechaApertura,
                SaldoInicial = c.SaldoInicial,
                FechaCierre = c.FechaCierre,
                Estado = c.Estado
            }).ToList();
        }

        public async Task<CajaDto> CreateCajaAsync(CajaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.SaldoInicial < 0)
            {
                throw new ArgumentException("El saldo inicial no puede ser negativo.");
            }

            var creada = await repo.CreateCajaAsync(new Models.Tablas.Caja
            {
                IdSede = dto.IdSede,
                IdEmpleado = dto.IdEmpleado,
                FechaApertura = DateTime.Now,
                SaldoInicial = dto.SaldoInicial,
                Estado = "ABIERTA"
            });

            return new CajaDto
            {
                IdCaja = creada.IdCaja,
                IdSede = creada.IdSede,
                NombreSede = creada.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEmpleado = creada.IdEmpleado,
                NombreEmpleado = creada.IdEmpleadoNavigation?.IdPersonaNavigation != null
                    ? $"{creada.IdEmpleadoNavigation.IdPersonaNavigation.Apellido}, {creada.IdEmpleadoNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                FechaApertura = creada.FechaApertura,
                SaldoInicial = creada.SaldoInicial,
                FechaCierre = creada.FechaCierre,
                Estado = creada.Estado
            };
        }

        public async Task<List<TransferenciaStockDto>> GetTransferenciasAsync(int? idSede = null)
        {
            var transferencias = await repo.GetTransferenciasAsync(idSede);
            return transferencias.Select(t => new TransferenciaStockDto
            {
                IdTransferencia = t.IdTransferencia,
                IdSedeOrigen = t.IdSedeOrigen,
                NombreSedeOrigen = t.IdSedeOrigenNavigation?.NombreSede ?? string.Empty,
                IdSedeDestino = t.IdSedeDestino,
                NombreSedeDestino = t.IdSedeDestinoNavigation?.NombreSede ?? string.Empty,
                FechaSolicitud = t.FechaSolicitud,
                Estado = t.Estado,
                UsuarioSistema = t.UsuarioSistema,
                Detalles = t.DetalleTransferencia.Select(d => new DetalleTransferenciaDto
                {
                    IdDetalleTransferencia = d.IdDetalleTransferencia,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = d.Cantidad
                }).ToList()
            }).ToList();
        }

        public async Task<TransferenciaStockDto?> GetTransferenciaByIdAsync(int idTransferencia)
        {
            var transferencia = await repo.GetTransferenciaByIdAsync(idTransferencia);
            if (transferencia == null)
            {
                return null;
            }

            return new TransferenciaStockDto
            {
                IdTransferencia = transferencia.IdTransferencia,
                IdSedeOrigen = transferencia.IdSedeOrigen,
                NombreSedeOrigen = transferencia.IdSedeOrigenNavigation?.NombreSede ?? string.Empty,
                IdSedeDestino = transferencia.IdSedeDestino,
                NombreSedeDestino = transferencia.IdSedeDestinoNavigation?.NombreSede ?? string.Empty,
                FechaSolicitud = transferencia.FechaSolicitud,
                Estado = transferencia.Estado,
                UsuarioSistema = transferencia.UsuarioSistema,
                Detalles = transferencia.DetalleTransferencia.Select(d => new DetalleTransferenciaDto
                {
                    IdDetalleTransferencia = d.IdDetalleTransferencia,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = d.Cantidad
                }).ToList()
            };
        }

        public async Task<TransferenciaStockDto> CreateTransferenciaAsync(TransferenciaStockCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var creada = await repo.CreateTransferenciaAsync(new Models.Tablas.TransferenciasStock
            {
                IdSedeOrigen = dto.IdSedeOrigen,
                IdSedeDestino = dto.IdSedeDestino,
                FechaSolicitud = DateTime.Now,
                Estado = "PENDIENTE",
                UsuarioSistema = dto.UsuarioSistema,
                DetalleTransferencia = dto.Detalles.Select(d => new Models.Tablas.DetalleTransferencia
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad
                }).ToList()
            });

            return new TransferenciaStockDto
            {
                IdTransferencia = creada.IdTransferencia,
                IdSedeOrigen = creada.IdSedeOrigen,
                NombreSedeOrigen = creada.IdSedeOrigenNavigation?.NombreSede ?? string.Empty,
                IdSedeDestino = creada.IdSedeDestino,
                NombreSedeDestino = creada.IdSedeDestinoNavigation?.NombreSede ?? string.Empty,
                FechaSolicitud = creada.FechaSolicitud,
                Estado = creada.Estado,
                UsuarioSistema = creada.UsuarioSistema,
                Detalles = creada.DetalleTransferencia.Select(d => new DetalleTransferenciaDto
                {
                    IdDetalleTransferencia = d.IdDetalleTransferencia,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = d.Cantidad
                }).ToList()
            };
        }

        public async Task<TransferenciaStockDto?> UpdateTransferenciaAsync(int idTransferencia, TransferenciaStockCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var actualizado = await repo.UpdateTransferenciaAsync(idTransferencia, new Models.Tablas.TransferenciasStock
            {
                IdSedeOrigen = dto.IdSedeOrigen,
                IdSedeDestino = dto.IdSedeDestino,
                UsuarioSistema = dto.UsuarioSistema,
                Estado = "PENDIENTE",
                DetalleTransferencia = dto.Detalles.Select(d => new Models.Tablas.DetalleTransferencia
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad
                }).ToList()
            });

            if (actualizado == null)
            {
                return null;
            }

            return new TransferenciaStockDto
            {
                IdTransferencia = actualizado.IdTransferencia,
                IdSedeOrigen = actualizado.IdSedeOrigen,
                NombreSedeOrigen = actualizado.IdSedeOrigenNavigation?.NombreSede ?? string.Empty,
                IdSedeDestino = actualizado.IdSedeDestino,
                NombreSedeDestino = actualizado.IdSedeDestinoNavigation?.NombreSede ?? string.Empty,
                FechaSolicitud = actualizado.FechaSolicitud,
                Estado = actualizado.Estado,
                UsuarioSistema = actualizado.UsuarioSistema,
                Detalles = actualizado.DetalleTransferencia.Select(d => new DetalleTransferenciaDto
                {
                    IdDetalleTransferencia = d.IdDetalleTransferencia,
                    IdProducto = d.IdProducto,
                    NombreProducto = d.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = d.Cantidad
                }).ToList()
            };
        }

        public async Task<bool> DeleteTransferenciaAsync(int idTransferencia)
        {
            if (idTransferencia <= 0)
            {
                throw new ArgumentException("El id de la transferencia es inválido.");
            }

            return await repo.DeleteTransferenciaAsync(idTransferencia);
        }

        public async Task<List<DevolucionVentaDto>> GetDevolucionesAsync(int? idVenta = null)
        {
            var devoluciones = await repo.GetDevolucionesAsync(idVenta);
            return devoluciones.Select(d => new DevolucionVentaDto
            {
                IdDevolucion = d.IdDevolucion,
                IdVenta = d.IdVenta,
                FechaDevolucion = d.FechaDevolucion,
                Motivo = d.Motivo,
                IdEmpleado = d.IdEmpleado,
                NombreEmpleado = d.IdEmpleadoNavigation?.IdPersonaNavigation != null
                    ? $"{d.IdEmpleadoNavigation.IdPersonaNavigation.Apellido}, {d.IdEmpleadoNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                ImporteReintegrado = d.ImporteReintegrado,
                Detalles = d.DetalleDevolucionesVenta.Select(dd => new DetalleDevolucionVentaDto
                {
                    IdDetalleDevolucion = dd.IdDetalleDevolucion,
                    IdProducto = dd.IdProducto,
                    NombreProducto = dd.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = dd.Cantidad,
                    PrecioUnitario = dd.PrecioUnitario
                }).ToList()
            }).ToList();
        }

        public async Task<DevolucionVentaDto> CreateDevolucionAsync(DevolucionVentaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Detalles == null || !dto.Detalles.Any())
            {
                throw new ArgumentException("La devolución debe incluir al menos un producto.");
            }

            var creada = await repo.CreateDevolucionAsync(new Models.Tablas.DevolucionesVentum
            {
                IdVenta = dto.IdVenta,
                FechaDevolucion = DateTime.Now,
                Motivo = dto.Motivo,
                IdEmpleado = dto.IdEmpleado,
                ImporteReintegrado = dto.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario),
                DetalleDevolucionesVenta = dto.Detalles.Select(d => new Models.Tablas.DetalleDevolucionesVentum
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            });

            return new DevolucionVentaDto
            {
                IdDevolucion = creada.IdDevolucion,
                IdVenta = creada.IdVenta,
                FechaDevolucion = creada.FechaDevolucion,
                Motivo = creada.Motivo,
                IdEmpleado = creada.IdEmpleado,
                NombreEmpleado = creada.IdEmpleadoNavigation?.IdPersonaNavigation != null
                    ? $"{creada.IdEmpleadoNavigation.IdPersonaNavigation.Apellido}, {creada.IdEmpleadoNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                ImporteReintegrado = creada.ImporteReintegrado,
                Detalles = creada.DetalleDevolucionesVenta.Select(dd => new DetalleDevolucionVentaDto
                {
                    IdDetalleDevolucion = dd.IdDetalleDevolucion,
                    IdProducto = dd.IdProducto,
                    NombreProducto = dd.IdProductoNavigation?.NombreProducto ?? string.Empty,
                    Cantidad = dd.Cantidad,
                    PrecioUnitario = dd.PrecioUnitario
                }).ToList()
            };
        }

        public async Task<List<PromocionDto>> GetPromocionesAsync()
        {
            var productos = await repo.GetProductosAsync();
            var promociones = productos
                .Take(5)
                .Select((p, index) => new PromocionDto
                {
                    IdPromocion = index + 1,
                    Nombre = $"Oferta {p.NombreProducto}",
                    Tipo = "PRODUCTO",
                    IdProducto = p.IdProducto,
                    DescuentoPorcentaje = index % 2 == 0 ? 10m : 15m,
                    MontoFijo = 0m,
                    FechaDesde = DateOnly.FromDateTime(DateTime.Today.AddDays(-7)),
                    FechaHasta = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
                    Activa = true
                })
                .ToList();

            return promociones;
        }

        public async Task<PromocionDto> CreatePromocionAsync(PromocionCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                throw new ArgumentException("El nombre de la promoción es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Tipo))
            {
                throw new ArgumentException("El tipo de promoción es obligatorio.");
            }

            var fechaDesde = dto.FechaDesde ?? DateOnly.FromDateTime(DateTime.Today);
            var fechaHasta = dto.FechaHasta ?? DateOnly.FromDateTime(DateTime.Today.AddDays(30));

            if (fechaHasta <= fechaDesde)
            {
                throw new ArgumentException("La fecha de finalización debe ser posterior a la de inicio.");
            }

            return new PromocionDto
            {
                IdPromocion = DateTime.UtcNow.Millisecond + (dto.IdProducto ?? 0),
                Nombre = dto.Nombre,
                Tipo = dto.Tipo,
                IdProducto = dto.IdProducto,
                IdCategoria = dto.IdCategoria,
                DescuentoPorcentaje = dto.DescuentoPorcentaje,
                MontoFijo = dto.MontoFijo,
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                Activa = true
            };
        }

        public async Task<List<MovimientoStockDto>> GetMovimientosStockAsync(int? idSede = null, int? idProducto = null)
        {
            var movimientos = await repo.GetMovimientosStockAsync(idSede, idProducto);
            return movimientos.Select(m => new MovimientoStockDto
            {
                IdMovimiento = m.IdMovimiento,
                IdSede = m.IdSede,
                NombreSede = m.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdProducto = m.IdProducto,
                NombreProducto = m.IdProductoNavigation?.NombreProducto ?? string.Empty,
                TipoMovimiento = m.TipoMovimiento,
                Cantidad = m.Cantidad,
                FechaMovimiento = m.FechaMovimiento,
                IdVenta = m.IdVenta,
                UsuarioSistema = m.UsuarioSistema,
                Observaciones = m.Observaciones
            }).ToList();
        }

        public async Task<MovimientoStockDto> CreateMovimientoStockAsync(MovimientoStockCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.TipoMovimiento))
            {
                throw new ArgumentException("El tipo de movimiento es obligatorio.");
            }

            var creado = await repo.CreateMovimientoStockAsync(new Models.Tablas.MovimientosStock
            {
                IdSede = dto.IdSede,
                IdProducto = dto.IdProducto,
                TipoMovimiento = dto.TipoMovimiento,
                Cantidad = dto.Cantidad,
                IdVenta = dto.IdVenta,
                UsuarioSistema = dto.UsuarioSistema,
                Observaciones = dto.Observaciones,
                FechaMovimiento = DateTime.Now
            });

            return new MovimientoStockDto
            {
                IdMovimiento = creado.IdMovimiento,
                IdSede = creado.IdSede,
                NombreSede = creado.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdProducto = creado.IdProducto,
                NombreProducto = creado.IdProductoNavigation?.NombreProducto ?? string.Empty,
                TipoMovimiento = creado.TipoMovimiento,
                Cantidad = creado.Cantidad,
                FechaMovimiento = creado.FechaMovimiento,
                IdVenta = creado.IdVenta,
                UsuarioSistema = creado.UsuarioSistema,
                Observaciones = creado.Observaciones
            };
        }

        public async Task<List<CierreCajaDto>> GetCierresCajaAsync(int? idCaja = null)
        {
            var cierres = await repo.GetCierresCajaAsync(idCaja);
            return cierres.Select(c => new CierreCajaDto
            {
                IdCierre = c.IdCierre,
                IdCaja = c.IdCaja,
                NombreSede = c.IdCajaNavigation?.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEmpleado = c.IdCajaNavigation?.IdEmpleado ?? 0,
                NombreEmpleado = c.IdCajaNavigation?.IdEmpleadoNavigation?.IdPersonaNavigation != null
                    ? $"{c.IdCajaNavigation.IdEmpleadoNavigation.IdPersonaNavigation.Apellido}, {c.IdCajaNavigation.IdEmpleadoNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                FechaCierre = c.FechaCierre,
                TotalVentas = c.TotalVentas,
                SaldoEsperado = c.SaldoEsperado,
                SaldoReal = c.SaldoReal,
                Diferencia = c.Diferencia,
                Observaciones = c.Observaciones
            }).ToList();
        }

        public async Task<CierreCajaDto?> GetCierreCajaByIdAsync(int idCierre)
        {
            var cierre = await repo.GetCierreCajaByIdAsync(idCierre);
            if (cierre == null)
            {
                return null;
            }

            return new CierreCajaDto
            {
                IdCierre = cierre.IdCierre,
                IdCaja = cierre.IdCaja,
                NombreSede = cierre.IdCajaNavigation?.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEmpleado = cierre.IdCajaNavigation?.IdEmpleado ?? 0,
                NombreEmpleado = cierre.IdCajaNavigation?.IdEmpleadoNavigation?.IdPersonaNavigation != null
                    ? $"{cierre.IdCajaNavigation.IdEmpleadoNavigation.IdPersonaNavigation.Apellido}, {cierre.IdCajaNavigation.IdEmpleadoNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                FechaCierre = cierre.FechaCierre,
                TotalVentas = cierre.TotalVentas,
                SaldoEsperado = cierre.SaldoEsperado,
                SaldoReal = cierre.SaldoReal,
                Diferencia = cierre.Diferencia,
                Observaciones = cierre.Observaciones
            };
        }

        public async Task<CierreCajaDto> CreateCierreCajaAsync(CierreCajaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var creado = await repo.CreateCierreCajaAsync(new Models.Tablas.CierresCaja
            {
                IdCaja = dto.IdCaja,
                FechaCierre = dto.FechaCierre,
                SaldoReal = dto.SaldoReal,
                Observaciones = dto.Observaciones
            });

            return new CierreCajaDto
            {
                IdCierre = creado.IdCierre,
                IdCaja = creado.IdCaja,
                NombreSede = creado.IdCajaNavigation?.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEmpleado = creado.IdCajaNavigation?.IdEmpleado ?? 0,
                NombreEmpleado = creado.IdCajaNavigation?.IdEmpleadoNavigation?.IdPersonaNavigation != null
                    ? $"{creado.IdCajaNavigation.IdEmpleadoNavigation.IdPersonaNavigation.Apellido}, {creado.IdCajaNavigation.IdEmpleadoNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                FechaCierre = creado.FechaCierre,
                TotalVentas = creado.TotalVentas,
                SaldoEsperado = creado.SaldoEsperado,
                SaldoReal = creado.SaldoReal,
                Diferencia = creado.Diferencia,
                Observaciones = creado.Observaciones
            };
        }

        public async Task<CierreCajaDto?> UpdateCierreCajaAsync(int idCierre, CierreCajaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var actualizado = await repo.UpdateCierreCajaAsync(idCierre, new Models.Tablas.CierresCaja
            {
                IdCaja = dto.IdCaja,
                FechaCierre = dto.FechaCierre,
                SaldoReal = dto.SaldoReal,
                Observaciones = dto.Observaciones
            });

            if (actualizado == null)
            {
                return null;
            }

            return new CierreCajaDto
            {
                IdCierre = actualizado.IdCierre,
                IdCaja = actualizado.IdCaja,
                NombreSede = actualizado.IdCajaNavigation?.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEmpleado = actualizado.IdCajaNavigation?.IdEmpleado ?? 0,
                NombreEmpleado = actualizado.IdCajaNavigation?.IdEmpleadoNavigation?.IdPersonaNavigation != null
                    ? $"{actualizado.IdCajaNavigation.IdEmpleadoNavigation.IdPersonaNavigation.Apellido}, {actualizado.IdCajaNavigation.IdEmpleadoNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                FechaCierre = actualizado.FechaCierre,
                TotalVentas = actualizado.TotalVentas,
                SaldoEsperado = actualizado.SaldoEsperado,
                SaldoReal = actualizado.SaldoReal,
                Diferencia = actualizado.Diferencia,
                Observaciones = actualizado.Observaciones
            };
        }

        public async Task<bool> DeleteCierreCajaAsync(int idCierre)
        {
            if (idCierre <= 0)
            {
                throw new ArgumentException("El id del cierre es inválido.");
            }

            return await repo.DeleteCierreCajaAsync(idCierre);
        }

        public async Task<List<EmpleadoDto>> GetEmpleadosAsync(int? idSede = null, int? idRol = null)
        {
            var empleados = await repo.GetEmpleadosAsync(idSede, idRol);
            return empleados.Select(e => new EmpleadoDto
            {
                IdEmpleado = e.IdEmpleado,
                IdPersona = e.IdPersona,
                Dni = e.IdPersonaNavigation?.Dni ?? string.Empty,
                Nombre = e.IdPersonaNavigation?.Nombre ?? string.Empty,
                Apellido = e.IdPersonaNavigation?.Apellido ?? string.Empty,
                Email = e.IdPersonaNavigation?.Email,
                Telefono = e.IdPersonaNavigation?.Telefono,
                IdSede = e.IdSede,
                NombreSede = e.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdRol = e.IdRol,
                NombreRol = e.IdRolNavigation?.NombreRol ?? string.Empty,
                Legajo = e.Legajo,
                FechaIngreso = e.FechaIngreso,
                SueldoBase = e.SueldoBase
            }).ToList();
        }

        public async Task<EmpleadoDto> CreateEmpleadoAsync(EmpleadoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.Dni))
            {
                throw new ArgumentException("El DNI es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Apellido))
            {
                throw new ArgumentException("Nombre y apellido son obligatorios.");
            }

            if (string.IsNullOrWhiteSpace(dto.Legajo))
            {
                throw new ArgumentException("El legajo es obligatorio.");
            }

            if (dto.SueldoBase <= 0)
            {
                throw new ArgumentException("El sueldo base debe ser mayor a cero.");
            }

            var creado = await repo.CreateEmpleadoAsync(new Models.Tablas.Empleado
            {
                IdSede = dto.IdSede,
                IdRol = dto.IdRol,
                Legajo = dto.Legajo,
                FechaIngreso = dto.FechaIngreso,
                SueldoBase = dto.SueldoBase,
                IdPersonaNavigation = new Models.Tablas.Persona
                {
                    Dni = dto.Dni,
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    FechaNacimiento = dto.FechaNacimiento,
                    EstadoCivil = dto.EstadoCivil,
                    Ocupacion = dto.Ocupacion,
                    Telefono = dto.Telefono,
                    Email = dto.Email
                }
            });

            return new EmpleadoDto
            {
                IdEmpleado = creado.IdEmpleado,
                IdPersona = creado.IdPersona,
                Dni = creado.IdPersonaNavigation?.Dni ?? dto.Dni,
                Nombre = creado.IdPersonaNavigation?.Nombre ?? dto.Nombre,
                Apellido = creado.IdPersonaNavigation?.Apellido ?? dto.Apellido,
                Email = creado.IdPersonaNavigation?.Email ?? dto.Email,
                Telefono = creado.IdPersonaNavigation?.Telefono ?? dto.Telefono,
                IdSede = creado.IdSede,
                NombreSede = creado.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdRol = creado.IdRol,
                NombreRol = creado.IdRolNavigation?.NombreRol ?? string.Empty,
                Legajo = creado.Legajo,
                FechaIngreso = creado.FechaIngreso,
                SueldoBase = creado.SueldoBase
            };
        }

        public async Task<List<RutinaDto>> GetRutinasAsync(int? idSocio = null, int? idEntrenador = null)
        {
            var rutinas = await repo.GetRutinasAsync(idSocio, idEntrenador);
            return rutinas.Select(r => new RutinaDto
            {
                IdRutina = r.IdRutina,
                IdSocio = r.IdSocio,
                NombreSocio = r.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{r.IdSocioNavigation.IdPersonaNavigation.Apellido}, {r.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdEntrenador = r.IdEntrenador,
                NombreEntrenador = r.IdEntrenadorNavigation?.IdPersonaNavigation != null
                    ? $"{r.IdEntrenadorNavigation.IdPersonaNavigation.Apellido}, {r.IdEntrenadorNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                FechaCreacion = r.FechaCreacion,
                Objetivo = r.Objetivo,
                Detalles = r.DetalleRutinas.Select(d => new DetalleRutinaDto
                {
                    IdDetalleRutina = d.IdDetalleRutina,
                    IdEjercicio = d.IdEjercicio,
                    Series = d.Series,
                    Repeticiones = d.Repeticiones,
                    DescansoSegundos = d.DescansoSegundos
                }).ToList()
            }).ToList();
        }

        public async Task<RutinaDto> CreateRutinaAsync(RutinaCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.IdSocio <= 0 || dto.IdEntrenador <= 0)
            {
                throw new ArgumentException("El socio y el entrenador son obligatorios.");
            }

            if (string.IsNullOrWhiteSpace(dto.Objetivo))
            {
                throw new ArgumentException("El objetivo de la rutina es obligatorio.");
            }

            if (dto.Detalles == null || !dto.Detalles.Any())
            {
                throw new ArgumentException("La rutina debe incluir al menos un ejercicio.");
            }

            var creada = await repo.CreateRutinaAsync(new Models.Tablas.Rutina
            {
                IdSocio = dto.IdSocio,
                IdEntrenador = dto.IdEntrenador,
                FechaCreacion = dto.FechaCreacion,
                Objetivo = dto.Objetivo,
                DetalleRutinas = dto.Detalles.Select(d => new Models.Tablas.DetalleRutina
                {
                    IdEjercicio = d.IdEjercicio,
                    Series = d.Series,
                    Repeticiones = d.Repeticiones,
                    DescansoSegundos = d.DescansoSegundos
                }).ToList()
            });

            return new RutinaDto
            {
                IdRutina = creada.IdRutina,
                IdSocio = creada.IdSocio,
                NombreSocio = creada.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{creada.IdSocioNavigation.IdPersonaNavigation.Apellido}, {creada.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdEntrenador = creada.IdEntrenador,
                NombreEntrenador = creada.IdEntrenadorNavigation?.IdPersonaNavigation != null
                    ? $"{creada.IdEntrenadorNavigation.IdPersonaNavigation.Apellido}, {creada.IdEntrenadorNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                FechaCreacion = creada.FechaCreacion,
                Objetivo = creada.Objetivo,
                Detalles = creada.DetalleRutinas.Select(d => new DetalleRutinaDto
                {
                    IdDetalleRutina = d.IdDetalleRutina,
                    IdEjercicio = d.IdEjercicio,
                    Series = d.Series,
                    Repeticiones = d.Repeticiones,
                    DescansoSegundos = d.DescansoSegundos
                }).ToList()
            };
        }

        public async Task<List<TurnoDto>> GetTurnosAsync(int? idSocio = null, int? idProfesional = null)
        {
            var turnos = await repo.GetTurnosAsync(idSocio, idProfesional);
            return turnos.Select(t => new TurnoDto
            {
                IdTurno = t.IdTurno,
                IdSocio = t.IdSocio,
                NombreSocio = t.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{t.IdSocioNavigation.IdPersonaNavigation.Apellido}, {t.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdProfesional = t.IdProfesional,
                NombreProfesional = t.IdProfesionalNavigation?.IdPersonaNavigation != null
                    ? $"{t.IdProfesionalNavigation.IdPersonaNavigation.Apellido}, {t.IdProfesionalNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdSede = t.IdSede,
                NombreSede = t.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEstadoTurno = t.IdEstadoTurno,
                EstadoTurno = t.IdEstadoTurnoNavigation?.NombreEstado ?? string.Empty,
                FechaTurno = t.FechaTurno,
                HoraTurno = t.HoraTurno
            }).ToList();
        }

        public async Task<TurnoDto?> GetTurnoByIdAsync(int idTurno)
        {
            var turno = await repo.GetTurnoByIdAsync(idTurno);
            if (turno == null)
            {
                return null;
            }

            return new TurnoDto
            {
                IdTurno = turno.IdTurno,
                IdSocio = turno.IdSocio,
                NombreSocio = turno.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{turno.IdSocioNavigation.IdPersonaNavigation.Apellido}, {turno.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdProfesional = turno.IdProfesional,
                NombreProfesional = turno.IdProfesionalNavigation?.IdPersonaNavigation != null
                    ? $"{turno.IdProfesionalNavigation.IdPersonaNavigation.Apellido}, {turno.IdProfesionalNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdSede = turno.IdSede,
                NombreSede = turno.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEstadoTurno = turno.IdEstadoTurno,
                EstadoTurno = turno.IdEstadoTurnoNavigation?.NombreEstado ?? string.Empty,
                FechaTurno = turno.FechaTurno,
                HoraTurno = turno.HoraTurno
            };
        }

        public async Task<TurnoDto> CreateTurnoAsync(TurnoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.IdSocio <= 0 || dto.IdProfesional <= 0 || dto.IdSede <= 0)
            {
                throw new ArgumentException("Socio, profesional y sede son obligatorios.");
            }

            var creado = await repo.CreateTurnoAsync(new Models.Tablas.Turno
            {
                IdSocio = dto.IdSocio,
                IdProfesional = dto.IdProfesional,
                IdSede = dto.IdSede,
                IdEstadoTurno = dto.IdEstadoTurno,
                FechaTurno = dto.FechaTurno,
                HoraTurno = dto.HoraTurno
            });

            return new TurnoDto
            {
                IdTurno = creado.IdTurno,
                IdSocio = creado.IdSocio,
                NombreSocio = creado.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{creado.IdSocioNavigation.IdPersonaNavigation.Apellido}, {creado.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdProfesional = creado.IdProfesional,
                NombreProfesional = creado.IdProfesionalNavigation?.IdPersonaNavigation != null
                    ? $"{creado.IdProfesionalNavigation.IdPersonaNavigation.Apellido}, {creado.IdProfesionalNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdSede = creado.IdSede,
                NombreSede = creado.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEstadoTurno = creado.IdEstadoTurno,
                EstadoTurno = creado.IdEstadoTurnoNavigation?.NombreEstado ?? string.Empty,
                FechaTurno = creado.FechaTurno,
                HoraTurno = creado.HoraTurno
            };
        }

        public async Task<TurnoDto?> UpdateTurnoAsync(int idTurno, TurnoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var actualizado = await repo.UpdateTurnoAsync(idTurno, new Models.Tablas.Turno
            {
                IdSocio = dto.IdSocio,
                IdProfesional = dto.IdProfesional,
                IdSede = dto.IdSede,
                IdEstadoTurno = dto.IdEstadoTurno,
                FechaTurno = dto.FechaTurno,
                HoraTurno = dto.HoraTurno
            });

            if (actualizado == null)
            {
                return null;
            }

            return new TurnoDto
            {
                IdTurno = actualizado.IdTurno,
                IdSocio = actualizado.IdSocio,
                NombreSocio = actualizado.IdSocioNavigation?.IdPersonaNavigation != null
                    ? $"{actualizado.IdSocioNavigation.IdPersonaNavigation.Apellido}, {actualizado.IdSocioNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdProfesional = actualizado.IdProfesional,
                NombreProfesional = actualizado.IdProfesionalNavigation?.IdPersonaNavigation != null
                    ? $"{actualizado.IdProfesionalNavigation.IdPersonaNavigation.Apellido}, {actualizado.IdProfesionalNavigation.IdPersonaNavigation.Nombre}"
                    : string.Empty,
                IdSede = actualizado.IdSede,
                NombreSede = actualizado.IdSedeNavigation?.NombreSede ?? string.Empty,
                IdEstadoTurno = actualizado.IdEstadoTurno,
                EstadoTurno = actualizado.IdEstadoTurnoNavigation?.NombreEstado ?? string.Empty,
                FechaTurno = actualizado.FechaTurno,
                HoraTurno = actualizado.HoraTurno
            };
        }

        public async Task<bool> DeleteTurnoAsync(int idTurno)
        {
            if (idTurno <= 0)
            {
                throw new ArgumentException("El id del turno es inválido.");
            }

            return await repo.DeleteTurnoAsync(idTurno);
        }

        public async Task<List<MantenimientoEquipamientoDto>> GetMantenimientosAsync(int? idEquipo = null)
        {
            var mantenimientos = await repo.GetMantenimientosEquipamientoAsync(idEquipo);
            return mantenimientos.Select(m => new MantenimientoEquipamientoDto
            {
                IdMantenimiento = m.IdMantenimiento,
                IdEquipo = m.IdEquipo,
                NombreEquipo = m.IdEquipoNavigation?.NombreMaquina ?? string.Empty,
                IdProveedor = m.IdProveedor,
                NombreProveedor = m.IdProveedorNavigation?.RazonSocial ?? string.Empty,
                FechaMantenimiento = m.FechaMantenimiento,
                Descripcion = m.Descripcion,
                Costo = m.Costo
            }).ToList();
        }

        public async Task<MantenimientoEquipamientoDto?> GetMantenimientoByIdAsync(int idMantenimiento)
        {
            var mantenimiento = await repo.GetMantenimientoEquipamientoByIdAsync(idMantenimiento);
            if (mantenimiento == null)
            {
                return null;
            }

            return new MantenimientoEquipamientoDto
            {
                IdMantenimiento = mantenimiento.IdMantenimiento,
                IdEquipo = mantenimiento.IdEquipo,
                NombreEquipo = mantenimiento.IdEquipoNavigation?.NombreMaquina ?? string.Empty,
                IdProveedor = mantenimiento.IdProveedor,
                NombreProveedor = mantenimiento.IdProveedorNavigation?.RazonSocial ?? string.Empty,
                FechaMantenimiento = mantenimiento.FechaMantenimiento,
                Descripcion = mantenimiento.Descripcion,
                Costo = mantenimiento.Costo
            };
        }

        public async Task<MantenimientoEquipamientoDto> CreateMantenimientoAsync(MantenimientoEquipamientoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.IdEquipo <= 0)
            {
                throw new ArgumentException("El equipo es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Descripcion))
            {
                throw new ArgumentException("La descripción del mantenimiento es obligatoria.");
            }

            if (dto.Costo < 0)
            {
                throw new ArgumentException("El costo no puede ser negativo.");
            }

            var creado = await repo.CreateMantenimientoEquipamientoAsync(new Models.Tablas.MantenimientoEquipamiento
            {
                IdEquipo = dto.IdEquipo,
                IdProveedor = dto.IdProveedor,
                FechaMantenimiento = dto.FechaMantenimiento,
                Descripcion = dto.Descripcion,
                Costo = dto.Costo
            });

            return new MantenimientoEquipamientoDto
            {
                IdMantenimiento = creado.IdMantenimiento,
                IdEquipo = creado.IdEquipo,
                NombreEquipo = creado.IdEquipoNavigation?.NombreMaquina ?? string.Empty,
                IdProveedor = creado.IdProveedor,
                NombreProveedor = creado.IdProveedorNavigation?.RazonSocial ?? string.Empty,
                FechaMantenimiento = creado.FechaMantenimiento,
                Descripcion = creado.Descripcion,
                Costo = creado.Costo
            };
        }

        public async Task<MantenimientoEquipamientoDto?> UpdateMantenimientoAsync(int idMantenimiento, MantenimientoEquipamientoCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var actual = await repo.GetMantenimientoEquipamientoByIdAsync(idMantenimiento);
            if (actual == null)
            {
                return null;
            }

            var actualizado = await repo.UpdateMantenimientoEquipamientoAsync(idMantenimiento, new Models.Tablas.MantenimientoEquipamiento
            {
                IdMantenimiento = idMantenimiento,
                IdEquipo = dto.IdEquipo,
                IdProveedor = dto.IdProveedor,
                FechaMantenimiento = dto.FechaMantenimiento,
                Descripcion = dto.Descripcion,
                Costo = dto.Costo
            });

            if (actualizado == null)
            {
                return null;
            }

            return new MantenimientoEquipamientoDto
            {
                IdMantenimiento = actualizado.IdMantenimiento,
                IdEquipo = actualizado.IdEquipo,
                NombreEquipo = actualizado.IdEquipoNavigation?.NombreMaquina ?? string.Empty,
                IdProveedor = actualizado.IdProveedor,
                NombreProveedor = actualizado.IdProveedorNavigation?.RazonSocial ?? string.Empty,
                FechaMantenimiento = actualizado.FechaMantenimiento,
                Descripcion = actualizado.Descripcion,
                Costo = actualizado.Costo
            };
        }

        public async Task<bool> DeleteMantenimientoAsync(int idMantenimiento)
        {
            if (idMantenimiento <= 0)
            {
                throw new ArgumentException("El id del mantenimiento es inválido.");
            }

            return await repo.DeleteMantenimientoEquipamientoAsync(idMantenimiento);
        }

        public async Task<List<EstadoCuentaSocioDto>> GetEstadoCuentaSociosAsync()
        {
            var rows = await repo.GetEstadoCuentaSociosAsync();
            return rows.Select(r => new EstadoCuentaSocioDto
            {
                IdSocio = r.IdSocio,
                Dni = r.Dni,
                Nombre = r.Nombre,
                Apellido = r.Apellido,
                CuotasEmitidas = r.CuotasEmitidas ?? 0,
                ImporteEmitido = r.ImporteEmitido,
                ImportePagado = r.ImportePagado,
                SaldoPendiente = r.SaldoPendiente ?? 0m
            }).ToList();
        }

        public async Task<List<MembresiaVigenteDto>> GetMembresiasVigentesAsync()
        {
            var rows = await repo.GetMembresiasVigentesAsync();
            return rows.Select(r => new MembresiaVigenteDto
            {
                IdContrato = r.IdContrato,
                IdSocio = r.IdSocio,
                Nombre = r.Nombre,
                Apellido = r.Apellido,
                IdPlan = r.IdPlan,
                NombrePlan = r.NombrePlan,
                IdSede = r.IdSede,
                NombreSede = r.NombreSede,
                FechaInicio = r.FechaInicio,
                FechaVencimiento = r.FechaVencimiento,
                EstadoContrato = r.EstadoContrato
            }).ToList();
        }

        public async Task<List<OcupacionClaseDto>> GetOcupacionClasesAsync()
        {
            var rows = await repo.GetOcupacionClasesAsync();
            return rows.Select(r => new OcupacionClaseDto
            {
                IdClase = r.IdClase,
                FechaClase = r.FechaClase,
                HoraInicio = r.HoraInicio,
                HoraFin = r.HoraFin,
                IdSede = r.IdSede,
                NombreSede = r.NombreSede,
                NombreDisciplina = r.NombreDisciplina,
                CupoLimite = r.CupoLimite,
                CuposOcupados = r.CuposOcupados ?? 0,
                CuposDisponibles = r.CuposDisponibles ?? 0
            }).ToList();
        }

        public async Task<List<CostoMantenimientoEquipoDto>> GetCostosMantenimientoAsync()
        {
            var rows = await repo.GetCostosMantenimientoEquipamientoAsync();
            return rows.Select(r => new CostoMantenimientoEquipoDto
            {
                IdEquipo = r.IdEquipo,
                NombreMaquina = r.NombreMaquina,
                IdSede = r.IdSede,
                NombreSede = r.NombreSede,
                CantidadIntervenciones = r.CantidadIntervenciones ?? 0,
                CostoTotal = r.CostoTotal,
                UltimoMantenimiento = r.UltimoMantenimiento
            }).ToList();
        }
    }
}
