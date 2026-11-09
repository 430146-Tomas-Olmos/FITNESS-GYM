using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class DetalleTransferencia
{
    public int IdDetalleTransferencia { get; set; }

    public int IdTransferencia { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual TransferenciasStock IdTransferenciaNavigation { get; set; } = null!;
}
