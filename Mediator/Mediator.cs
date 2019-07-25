using System;

namespace Mediator
{
    class Program
    {
        static void Main(string[] args)
        {
            ManagerMediator mediator = new ManagerMediator();
            Colleague customer = new CustomerColleague(mediator);
            Colleague stock = new StockColleague(mediator);
            Colleague courier = new CourierColleague(mediator);
            mediator.Customer = customer;
            mediator.Programmer = stock;
            mediator.Tester = courier;
            customer.Send("Есть заказ, надо упаковать товар");
            stock.Send("Товар упакован, нужно отвезти товар");
            courier.Send("Товар доставлен и оплачен");

            Console.Read();
        }
    }

    abstract class Mediator
    {
        public abstract void Send(string msg, Colleague colleague);
    }
    abstract class Colleague
    {
        protected Mediator mediator;

        public Colleague(Mediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual void Send(string message)
        {
            mediator.Send(message, this);
        }
        public abstract void Notify(string message);
    }
    // класс заказчика
    class CustomerColleague : Colleague
    {
        public CustomerColleague(Mediator mediator)
        : base(mediator)
        { }

        public override void Notify(string message)
        {
            Console.WriteLine("Сообщение заказчику: " + message + "\n");
        }
    }
    // класс программиста
    class StockColleague : Colleague
    {
        public StockColleague(Mediator mediator)
        : base(mediator)
        { }

        public override void Notify(string message)
        {
            Console.WriteLine("Сообщение складу: " + message + "\n");
        }
    }
    // класс тестера
    class CourierColleague : Colleague
    {
        public CourierColleague(Mediator mediator)
        : base(mediator)
        { }

        public override void Notify(string message)
        {
            Console.WriteLine("Сообщение курьеру: " + message + "\n");
        }
    }

    class ManagerMediator : Mediator
    {
        public Colleague Customer { get; set; }
        public Colleague Programmer { get; set; }
        public Colleague Tester { get; set; }
        public override void Send(string msg, Colleague colleague)
        {
            if (Customer == colleague)
                Programmer.Notify(msg);
            else if (Programmer == colleague)
                Tester.Notify(msg);
            else if (Tester == colleague)
                Customer.Notify(msg);
        }
    }
}