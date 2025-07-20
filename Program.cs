using System;
using System.Threading.Tasks;
using Hidrobo_Semana8.DATA;
using Hidrobo_Semana8.Models;
using Microsoft.EntityFrameworkCore;

namespace Hidrobo_Semana8
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE CLIENTES ===\n");

            Console.WriteLine("1. Transferir saldo");
            Console.WriteLine("2. Eliminar cliente");
            Console.Write("Seleccione una opción (1 o 2): ");
            string opcion = Console.ReadLine();

            if (opcion == "1")
            {
                Console.Write("ID Cliente Origen: ");
                int idOrigen = int.Parse(Console.ReadLine());

                Console.Write("ID Cliente Destino: ");
                int idDestino = int.Parse(Console.ReadLine());

                Console.Write("Monto a transferir: ");
                decimal monto = decimal.Parse(Console.ReadLine());

                await TransferirSaldo(idOrigen, idDestino, monto);
            }
            else if (opcion == "2")
            {
                Console.Write("ID del Cliente a eliminar: ");
                int idEliminar = int.Parse(Console.ReadLine());

                await EliminarCliente(idEliminar);
            }
            else
            {
                Console.WriteLine("❌ Opción no válida.");
            }

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        static async Task TransferirSaldo(int idOrigen, int idDestino, decimal monto)
        {
            using var context = new ClientesContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var origen = await context.Clientes.FindAsync(idOrigen);
                var destino = await context.Clientes.FindAsync(idDestino);

                if (origen == null || destino == null)
                    throw new Exception("Uno de los clientes no existe.");

                if (origen.Saldo < monto)
                    throw new Exception("Saldo insuficiente.");

                origen.Saldo -= monto;
                destino.Saldo += monto;

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                Console.WriteLine("✅ Transferencia realizada con éxito.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        static async Task EliminarCliente(int idCliente)
        {
            using var context = new ClientesContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var cliente = await context.Clientes.FindAsync(idCliente);
                if (cliente == null)
                    throw new Exception("Cliente no encontrado.");

                if (cliente.Saldo != 0)
                    throw new Exception("No se puede eliminar: el saldo debe ser 0.");

                context.Clientes.Remove(cliente);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                Console.WriteLine("✅ Cliente eliminado correctamente.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}