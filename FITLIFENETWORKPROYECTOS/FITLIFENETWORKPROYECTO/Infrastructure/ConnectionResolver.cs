using Microsoft.Data.SqlClient;

namespace FITLIFENETWORKPROYECTO.Infrastructure;

/// <summary>
/// Busca automáticamente un SQL Server que tenga la base FITLIFENETWORKdb,
/// probando varios nombres de servidor comunes. Así el proyecto abre en cualquier PC
/// sin tener que editar la cadena de conexión.
/// </summary>
public static class ConnectionResolver
{
    public static string ServidorUsado { get; private set; } = "(ninguno)";
    public static List<string> Intentos { get; } = new();

    public static string Resolve(IConfiguration config)
    {
        var candidatas = new List<string>();

        // 1) Variable de entorno (por si alguien quiere forzar una conexión)
        var env = Environment.GetEnvironmentVariable("FITLIFE_CONNECTION");
        if (!string.IsNullOrWhiteSpace(env)) candidatas.Add(env);

        // 2) Cadena principal del appsettings.json
        var principal = config.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(principal)) candidatas.Add(principal);

        // 3) Lista de servidores comunes
        var servidores = config.GetSection("DatabaseServers").Get<string[]>() ?? Array.Empty<string>();
        var db = config["DatabaseName"] ?? "FITLIFENETWORKdb";
        foreach (var s in servidores)
        {
            candidatas.Add($"Data Source={s};Initial Catalog={db};Integrated Security=True;Encrypt=False;TrustServerCertificate=True");
        }

        foreach (var cadena in candidatas.Distinct())
        {
            string servidor = "?";
            try
            {
                var sb = new SqlConnectionStringBuilder(cadena) { ConnectTimeout = 3 };
                servidor = sb.DataSource;
                using var cn = new SqlConnection(sb.ConnectionString);
                cn.Open();
                ServidorUsado = $"{sb.DataSource} / {sb.InitialCatalog}";
                Intentos.Add($"OK    -> {servidor}");
                Console.WriteLine($"[FitLife] Conectado a la base de datos en: {ServidorUsado}");
                return cadena;
            }
            catch (Exception ex)
            {
                var msg = ex.Message.Split('\n')[0].Trim();
                Intentos.Add($"FALLÓ -> {servidor}: {msg}");
                Console.WriteLine($"[FitLife] No se pudo conectar a '{servidor}': {msg}");
            }
        }

        // ---- Descubrimiento: ¿la base existe con otro nombre, o falta permiso? ----
        foreach (var servidor in servidores.Distinct())
        {
            try
            {
                var sb = new SqlConnectionStringBuilder
                {
                    DataSource = servidor,
                    InitialCatalog = "master",
                    IntegratedSecurity = true,
                    Encrypt = false,
                    TrustServerCertificate = true,
                    ConnectTimeout = 3
                };
                var nombres = new List<string>();
                using (var cn = new SqlConnection(sb.ConnectionString))
                {
                    cn.Open();
                    using var cmd = cn.CreateCommand();
                    cmd.CommandText = "SELECT name FROM sys.databases WHERE name NOT IN ('master','tempdb','model','msdb') ORDER BY name";
                    using var rd = cmd.ExecuteReader();
                    while (rd.Read()) nombres.Add(rd.GetString(0));
                }

                var lista = nombres.Count == 0 ? "(ninguna)" : string.Join(", ", nombres);
                Console.WriteLine($"[FitLife] El servidor '{servidor}' responde. Bases de datos que se ven: {lista}");
                Intentos.Add($"SERVIDOR RESPONDE -> {servidor}. Bases: {lista}");

                var parecida = nombres.FirstOrDefault(n => n.Contains("fitlife", StringComparison.OrdinalIgnoreCase));
                if (parecida != null)
                {
                    sb.InitialCatalog = parecida;
                    sb.ConnectTimeout = 15;
                    try
                    {
                        using var cn2 = new SqlConnection(sb.ConnectionString);
                        cn2.Open();
                        ServidorUsado = $"{servidor} / {parecida}";
                        Intentos.Add($"OK (base con otro nombre) -> {ServidorUsado}");
                        Console.WriteLine($"[FitLife] Usando la base '{parecida}' en '{servidor}'.");
                        return sb.ConnectionString;
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"[FitLife] La base '{parecida}' EXISTE en '{servidor}' pero tu usuario de Windows no tiene permiso: {ex2.Message.Split('\n')[0].Trim()}");
                    }
                }
                else
                {
                    Console.WriteLine($"[FitLife] En '{servidor}' NO hay ninguna base con 'fitlife' en el nombre: hay que restaurarla/crearla ahí.");
                }
            }
            catch
            {
                // servidor no accesible: ya figura en los intentos anteriores
            }
        }

        Console.WriteLine("[FitLife] ATENCIÓN: no se encontró ningún SQL Server con la base. Revisá appsettings.json (DatabaseServers).");
        // Se devuelve la principal para que la app igual arranque y /api/health/db muestre el detalle.
        return candidatas.FirstOrDefault()
            ?? throw new InvalidOperationException("No hay ninguna cadena de conexión configurada.");
    }
}
