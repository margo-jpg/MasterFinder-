using MasterFinder.Domain.Entities;
using MasterFinder.Domain.Enums;
using MasterFinder.Domain.Exceptions;
using MasterFinder.Domain.ValueObject;
using MasterFinder.Domain.ValueObject.Exceptions;

namespace MasterFinder.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" Тестирование домена MasterFinder \n");

            // 1. Тестируем создание заказчика
            TestCustomerCreation();

            // 2. Тестируем создание исполнителя
            TestExecutorCreation();

            // 3. Тестируем создание заказа
            TestOrderCreation();

            // 4. Тестируем отклик на заказ
            TestResponseCreation();

            // 5. Тестируем полный цикл
            TestFullFlow();

            // 6. Тестируем ошибки валидации
            TestValidationErrors();

            Console.WriteLine("\n Все тесты завершены ");
            Console.ReadKey();
        }

        static void TestCustomerCreation()
        {
            Console.WriteLine(" 1. Создание заказчика ");

            try
            {
                var customer = new Customer("Анна", "+79001234567");
                Console.WriteLine($" Заказчик создан: {customer.UserName.Value}, тел: {customer.Phone.Value}");

                var order = customer.CreateOrder("Починить кран", "Горячая вода капает");
                Console.WriteLine($" Создан заказ: {order.Title.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.Message}");
            }
            Console.WriteLine();
        }

        static void TestExecutorCreation()
        {
            Console.WriteLine(" 2. Создание исполнителя ");

            try
            {
                var executor = new Executor("Иван", "+79001112233", "сантехник");
                Console.WriteLine($" Исполнитель создан: {executor.UserName.Value}, специальность: {executor.Specialization.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.Message}");
            }
            Console.WriteLine();
        }

        static void TestOrderCreation()
        {
            Console.WriteLine(" 3. Создание заказа ");

            try
            {
                var customer = new Customer("Анна", "+79001234567");
                var order = customer.CreateOrder("Починить кран", "Горячая вода капает");

                Console.WriteLine($" Заказ создан:");
                Console.WriteLine($"    Название: {order.Title.Value}");
                Console.WriteLine($"    Описание: {order.Description.Value}");
                Console.WriteLine($"    Статус: {order.Status}");
                Console.WriteLine($"    Создан: {order.CreatedAt}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.Message}");
            }
            Console.WriteLine();
        }

        static void TestResponseCreation()
        {
            Console.WriteLine(" 4. Отклик на заказ ");

            try
            {
                var customer = new Customer("Анна", "+79001234567");
                var order = customer.CreateOrder("Починить кран", "Горячая вода капает");

                var executor = new Executor("Иван", "+79001112233", "сантехник");
                var response = executor.RespondToOrder(order, "Могу приехать завтра");

                Console.WriteLine($" Отклик создан:");
                Console.WriteLine($"    Заказ: {response.OrderId}");
                Console.WriteLine($"    Исполнитель: {response.ExecutorId}");
                Console.WriteLine($"    Статус: {response.Status}");
                Console.WriteLine($"    Комментарий: {response.Comment}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.Message}");
            }
            Console.WriteLine();
        }

        static void TestFullFlow()
        {
            Console.WriteLine(" 5. Полный цикл: заказ, отклик, принятие, выполнение \n");

            try
            {
                // 1. Заказчик создаёт заказ
                var customer = new Customer("Анна", "+79001234567");
                var order = customer.CreateOrder("Починить кран", "Горячая вода капает");
                Console.WriteLine($"1) Заказ создан (статус: {order.Status})");

                // 2. Исполнитель откликается
                var executor = new Executor("Иван", "+79001112233", "сантехник");
                var response = executor.RespondToOrder(order, "Могу завтра");
                Console.WriteLine($"2) Исполнитель откликнулся (статус: {response.Status})");

                // 3. Принимаем отклик
                response.Accept();
                Console.WriteLine($"3) Отклик принят (статус: {response.Status})");

                //  ВАЖНО: Добавляем отклик в заказ вручную
                // Используем рефлексию, чтобы добавить отклик в приватное поле _responses
                var field = typeof(Order).GetField("_responses",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

                var responses = field?.GetValue(order) as System.Collections.IList;
                responses?.Add(response);

                // 4. Начинаем выполнение
                order.StartExecution(executor.Id);
                Console.WriteLine($"4) Начато выполнение (статус заказа: {order.Status})");

                // 5. Завершаем заказ
                order.Complete();
                Console.WriteLine($"5) Заказ выполнен (статус заказа: {order.Status})");

                Console.WriteLine("\n Весь цикл прошёл успешно!");
            }
            catch (BusinessRuleViolationException ex)
            {
                Console.WriteLine($" Ошибка бизнес-правила: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.Message}");
            }
            Console.WriteLine();
        }

        static void TestValidationErrors()
        {
            Console.WriteLine(" 6. Тестирование ошибок валидации \n");

            // Ошибка: пустое имя
            try
            {
                var customer = new Customer("", "+79001234567");
                Console.WriteLine(" Должна была быть ошибка, но её нет");
            }
            catch (ValueObjectValidationException ex)
            {
                Console.WriteLine($" Ошибка при пустом имени: {ex.Message}");
            }

            // Ошибка: имя слишком короткое
            try
            {
                var customer = new Customer("A", "+79001234567");
                Console.WriteLine(" Должна была быть ошибка, но её нет");
            }
            catch (ValueObjectValidationException ex)
            {
                Console.WriteLine($" Ошибка при коротком имени: {ex.Message}");
            }

            // Ошибка: неверный телефон
            try
            {
                var customer = new Customer("Анна", "123");
                Console.WriteLine(" Должна была быть ошибка, но её нет");
            }
            catch (ValueObjectValidationException ex)
            {
                Console.WriteLine($" Ошибка при неверном телефоне: {ex.Message}");
            }

            // Ошибка: пустая специализация
            try
            {
                var executor = new Executor("Иван", "+79001112233", "");
                Console.WriteLine(" Должна была быть ошибка, но её нет");
            }
            catch (ValueObjectValidationException ex)
            {
                Console.WriteLine($" Ошибка при пустой специализации: {ex.Message}");
            }

            // Ошибка: пустое название заказа
            try
            {
                var customer = new Customer("Анна", "+79001234567");
                var order = customer.CreateOrder("", "Описание");
                Console.WriteLine(" Должна была быть ошибка, но её нет");
            }
            catch (ValueObjectValidationException ex)
            {
                Console.WriteLine($" Ошибка при пустом названии заказа: { ex.Message} ");
            }

            // Ошибка: отклик на закрытый заказ
            try
            {
                var customer = new Customer("Анна", "+79001234567");
                var order = customer.CreateOrder("Заказ", "Описание");
                order.Complete(); // закрываем заказ

                var executor = new Executor("Иван", "+79001112233", "сантехник");
                executor.RespondToOrder(order); // пытаемся откликнуться

                Console.WriteLine(" Должна была быть ошибка, но её нет");
            }
            catch (BusinessRuleViolationException ex)
            {
                Console.WriteLine($" Ошибка при отклике на закрытый заказ: {ex.Message}");
            }

            Console.WriteLine();
        }
    }
}




