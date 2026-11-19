using MasterFinder.Domain.Entities;
using MasterFinder.ValueObjects;

namespace MasterFinder.ConsoleTests;

class Program
{
    static void Main()
    {
        Console.WriteLine("Тест\n");

        try
        {
            // 1 Создаем заказчика
            var customer = new Customer(new Username("Екатерина"), new PhoneNumber("+79941554567"));
            Console.WriteLine($"Заказчик: {customer.Username.Value}");

            // 2 Создаем исполнителя
            var executor = new Executor(
                new Username("Иван"),
                new PhoneNumber("+79991165233"),
                new Specialization("сантехник"));
            Console.WriteLine($"Исполнитель: {executor.Username.Value} (специальность: {executor.Specialization.Value})");

            // 3 Заказчик создает заказ
            var order = customer.CreateOrder(new OrderTitle("Починить кран"), new OrderDescription("Вода капает"));
            Console.WriteLine($"Заказ: {order.Title.Value} (статус: {order.Status})");

            // 4 Исполнитель откликается
            var comment = ResponseComment.Create("Могу приехать завтра");
            var response = executor.RespondToOrder(order, comment);
            Console.WriteLine($"Отклик: статус {response.Status}");

            // 5 Заказчик принимает отклик
            order.AcceptResponse(response,customer);
            Console.WriteLine($"Отклик принят: статус {response.Status}");

            // 6 Исполнитель начинает выполнение
            order.StartExecution(executor);
            Console.WriteLine($"Выполнение начато: статус заказа {order.Status}");

            // 7 Исполнитель завершает заказ
            order.Complete(executor);
            Console.WriteLine($"Заказ выполнен: статус {order.Status}");

            Console.WriteLine("\nВсе успешно!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}