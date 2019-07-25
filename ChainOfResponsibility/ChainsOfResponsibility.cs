using System;
using System.Collections.Generic;

namespace ChainOfResponsibility
{// Интерфейс Обработчика объявляет метод построения цепочки
 // обработчиков. Он также объявляет метод для выполнения запроса.
    public interface IHandler
    {
        IHandler Next(IHandler handler);

        object Handle(object request);
    }

    // Поведение цепочки по умолчанию может быть реализовано внутри базового
    // класса обработчика.
    abstract class AbstractHandler : IHandler
    {
        private IHandler _nextHandler;

        public IHandler Next(IHandler handler)
        {
            this._nextHandler = handler;

            // Возврат обработчика отсюда позволит связать обработчики
            // простым способом, вот так:
            // monkey.SetNext(squirrel).SetNext(dog);
            return handler;
        }

        public virtual object Handle(object request)
        {
            if (this._nextHandler != null)
            {
                return this._nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }

    class Mistake1 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if ((request as string) == "200")
            {
                return $"Answer: 200 OK — успешный запрос. Если клиентом были запрошены какие-либо данные, то они находятся в заголовке и/или теле сообщения. Появился в HTTP/1.0.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class Mistake2 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "400")
            {
                return $"Answer: 400 Bad Request — сервер обнаружил в запросе клиента синтаксическую ошибку. Появился в HTTP/1.0.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class Mistake3 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "404")
            {
                return $"Answer: 404 Not Found[19] — самая распространённая ошибка при пользовании Интернетом, основная причина" +
                    $" — ошибка в написании адреса Web-страницы. Сервер понял запрос, но не нашёл соответствующего ресурса по указанному URL. " +
                    $"Если серверу известно, что по этому адресу был документ, то ему желательно использовать код 410. Ответ 404 может " +
                    $"использоваться вместо 403, если требуется тщательно скрыть от посторонних глаз определённые ресурсы. Появился в HTTP/1.0.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }

    class Mistake4 : AbstractHandler
    {
        public override object Handle(object request)
        {
            if (request.ToString() == "501")
            {
                return $"Answer: 501 Not Implemented — сервер не поддерживает возможностей, необходимых для обработки запроса. Типичный ответ " +
                    $"для случаев, когда сервер не понимает указанный в запросе метод. Если же метод серверу известен, но он не применим к данному" +
                    $" ресурсу, то нужно вернуть ответ 405. Появился в HTTP/1.0.\n";
            }
            else
            {
                return base.Handle(request);
            }
        }
    }


    class Client
    {
        // Обычно клиентский код приспособлен для работы с единственным
        // обработчиком. В большинстве случаев клиенту даже неизвестно, что этот
        // обработчик является частью цепочки.
        public static void ClientCode(AbstractHandler handler)
        {
            foreach (var mistakeNumber in new List<string> { "200", "400", "404", "501" })
            {
                Console.WriteLine($"\nClient: Что значит ошибка {mistakeNumber}? ");

                var result = handler.Handle(mistakeNumber);

                if (result != null)
                {
                    Console.Write($"   {result}");
                }
                else
                {
                    Console.WriteLine($"\n Ошибка  {mistakeNumber} не существует");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Другая часть клиентского кода создает саму цепочку.
            var mistake1 = new Mistake1();
            var mistake2 = new Mistake2();
            var mistake3 = new Mistake3();
            var mistake4 = new Mistake4();

            mistake1.Next(mistake2).Next(mistake3).Next(mistake4);

            // Клиент должен иметь возможность отправлять запрос любому
            // обработчику, а не только первому в цепочке.
            Client.ClientCode(mistake1);
            Console.WriteLine();
        }
    }
}