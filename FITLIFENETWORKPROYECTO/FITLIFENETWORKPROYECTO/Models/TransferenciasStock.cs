using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class TransferenciasStock
{
    public int IdTransferencia { get; set; }

    public int IdSedeOrigen { get; set; }

    public int IdSedeDestino { get; set; }

    public DateTime FechaSolicitud { get; set; }

    public string Estado { get; set; } = null!;

    public string? UsuarioSistema { get; set; }

    public virtual ICollection<DetalleTransferencia> DetalleTransferencia { get; set; } = new List<DetalleTransferencia>();

    public virtual Sede IdSedeDestinoNavigation { get; set; } = null!;

    public virtual Sede IdSedeOrigenNavigation { get; set; } = null!;
}
