using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Console = System.Console;

namespace Thelema.Clientes.API.Application.Events
{
    public class ClienteEventHandler : INotificationHandler<ClienteRegistradoEvent>
    {
        public Task Handle(ClienteRegistradoEvent notification, CancellationToken cancellationToken)
        {
            // Enviar evento de confirmação
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*****************************************************************");
            Console.WriteLine($"O evento do agregado {notification.AggregateId} foi manipulado!");
            Console.WriteLine("*****************************************************************");
            Console.ForegroundColor = ConsoleColor.White;

            return Task.CompletedTask;
        }
    }
}