using OOOSubs.BL.Model;
using OOOSubs.BL.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OOOSubs.CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            var LIST_OF_SUBSCRIBERS = new List<Subscriber>();
            var CurrentSub = new Subscriber();
            var Controller = new SubsController();
            string bigLine = "\\//\\\\//\\\\//\\\\//\\\\//\\\\//" +
                "\\\\//\\\\//\\\\//\\\\//\\\\//\\\\//\\";

            Console.WriteLine("ООО. Абоненты АТС (с).");
            Console.WriteLine("Добро пожаловать!");

            // db
            #region start
            try
            {
                using (Stream stream = File.Open("data.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    var subscribers2 = (List<Subscriber>)bin.Deserialize(stream);

                    foreach (Subscriber sub in subscribers2)
                    {
                        Subscriber subToAdd = new Subscriber(sub.Name, sub.Number, sub.Tariff, sub.Balance);
                        LIST_OF_SUBSCRIBERS.Add(subToAdd);
                    }
                    
                }
            }
            catch (IOException)
            {
            }
            Console.WriteLine(bigLine);
            #endregion

            // cmd
            while (true)
            {
                Console.WriteLine("\nПожалуйста, выберите комманду: ");
                Console.WriteLine("'1' := Посмотреть список абонентов.\n" +
                    "'2' := Добавить абонента.\n" +
                    "'3' := Выбрать абонента.\n" +
                    "'4' := Пополнить баланс.\n" +
                    "'5' := Сменить тариф.\n" +
                    "'6' := Удалить абонента.\n" +
                    "'7' := Отсортировать список абонентов.\n" +
                    "'8' := Списание повременной платы.\n" +
                    "'9' := Списание абонентской платы.\n" +
                    "'0' := Просмотр выбранного абонента.");

                switch (Console.ReadLine())
                {
                    #region '1' Список Абонентов
                    // Показать список абонентов
                    case "1":
                        try
                        {
                            using (Stream stream = File.Open("data.bin", FileMode.Open))
                            {
                                BinaryFormatter bin = new BinaryFormatter();

                                var subscribers2 = (List<Subscriber>)bin.Deserialize(stream);

                                Console.WriteLine("Список абонентов:");
                                int i = 1;
                                foreach (Subscriber sub in subscribers2)
                                {
                                    Console.WriteLine("{0}. Имя:{1},\tНомер:{2},\tТариф:{3},\tБаланс:{4}",
                                        i,
                                        sub.Name,
                                        sub.Number,
                                        sub.Tariff,
                                        sub.Balance);
                                    i++;
                                }
                            }
                        }
                        catch (IOException)
                        {
                        }
                        if (LIST_OF_SUBSCRIBERS.Count == 0) Console.WriteLine("База данных абонентов пуста.");

                        break;
                    #endregion

                    #region '2' Добавить Абонента
                    // Добавление нового абонента
                    case "2":
                        Console.WriteLine("Введите имя: ");
                        string name = Console.ReadLine();

                        Console.WriteLine("Введите номер: ");
                        string number = Console.ReadLine();

                        Console.WriteLine("Выберите тариф ('1', '2', '3'): ");
                        int value1;
                        try
                        {
                            int value2 = int.Parse(Console.ReadLine());
                            value1 = value2;
                        }
                        catch
                        {
                            Console.WriteLine("[Ошибка]: Неправильно введенное число.");
                            value1 = 0;
                        }
                        Tariff tariff = new Tariff(value1);

                        Subscriber newSub = new Subscriber(name, number, tariff, 0.0);
                        if (newSub.Tariff.tariff_id == 0 || newSub.Tariff.tariff_id == 1
                            || newSub.Tariff.tariff_id == 2 || newSub.Tariff.tariff_id == 3)
                        {
                            LIST_OF_SUBSCRIBERS.Add(newSub);
                            Controller.AddNewSub(LIST_OF_SUBSCRIBERS);
                            Console.WriteLine("[Выполнено]: Добавлен новый абонент.");
                        }
                        else Console.WriteLine("[Ошибка]: Абонент не добавлен.");
                        break;
                    #endregion

                    #region '3' Выбрать Абонента
                    // Выбор абонента
                    case "3":
                        Console.WriteLine("Введите номер телефона пользователя для поиска.");
                        string toFind = Console.ReadLine();
                        if (Controller.FindSub(toFind, LIST_OF_SUBSCRIBERS, ref CurrentSub))
                        {
                            Console.WriteLine("Абонент найден. Текущий абонент:");
                            Console.WriteLine(CurrentSub.ToString());
                        }
                        else Console.WriteLine("Номер телефона не найден.");

                        break;
                    #endregion

                    #region '4' Пополнить Баланс
                    // Положить деньги на счёт
                    case "4":
                        string phone = CurrentSub.Number;

                        if (LIST_OF_SUBSCRIBERS.Exists(s => s.Number == phone))
                        {
                            var somesub = LIST_OF_SUBSCRIBERS.Find(s => s.Number == phone);
                            Console.WriteLine("Абонент найден.");
                            double cash;

                            Console.WriteLine("Введите сумму:");
                            try
                            {
                                double money = Double.Parse(Console.ReadLine());
                                cash = money;
                            }
                            catch
                            {
                                Console.WriteLine("[Ошибка]: Неправильно введенная сумма.");
                                break;
                            }

                            Console.WriteLine("Оплата прошла успешно.");
                            Console.WriteLine("Имя:{0},\tНомер:{1},\tТариф:{2},\tБаланс:{3}",
                                       CurrentSub.Name,
                                       CurrentSub.Number,
                                       CurrentSub.Tariff,
                                       CurrentSub.Balance += cash);

                            Controller.Save(LIST_OF_SUBSCRIBERS);
                        }
                        else
                        {
                            Console.WriteLine("Для пополнения счета выберите абонента.");
                        }
                        break;
                    #endregion

                    #region '5' Поменять Тариф
                    // Смена тарифа
                    case "5":
                        if (CurrentSub == null)
                        {
                            Console.WriteLine("Абонент не выбран.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("'0' := Убрать тариф.\n" +
                                    "'1' := Стандарт - Абонентская плата 300$/Мес (Минута = 5$).\n" +
                                    "'2' := Премиум - Абонентская плата 570$/Мес (Минута = 15$).\n" +
                                    "'3' := Посекундный - Абонентская плата 440$/Мес (Секунда = 0.15$).\n" +
                                    "'b' := Вернуться в меню.");
                            CurrentSub.ToString();

                            var currentSub = CurrentSub;
                            string enter = Console.ReadLine();
                            switch (enter)
                            {
                                case "0":
                                case "1":
                                case "2":
                                case "3":
                                    var subToChange = LIST_OF_SUBSCRIBERS.Find(s => s.Number == CurrentSub.Number);

                                    int tarif = int.Parse(enter);
                                    Tariff tarifOld = new Tariff(subToChange.Tariff.tariff_id);

                                    if (subToChange.Tariff.tariff_id == tarif)
                                    {
                                        Console.WriteLine("Данный тариф уже подключен.");
                                        break;
                                    }
                                    if (tarif == 1 || tarif == 2 || tarif == 3 || tarif == 0)
                                    {
                                        subToChange.Tariff.tariff_id = tarif;
                                        Controller.Save(LIST_OF_SUBSCRIBERS);
                                        Console.WriteLine($"[Выполнено]: Тариф изменен с {tarifOld} на {CurrentSub.Tariff}");
                                        CurrentSub.ToString();
                                    }
                                    break;

                                case "b":
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region '6' Удалить Абонент
                    //Удалить Абонент
                    case "6":
                        if (CurrentSub == null)
                        {
                            Console.WriteLine("Абонент не выбран.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Текущий абонент: ");
                            CurrentSub.ToString();
                            Console.WriteLine("'y' := Подтвердить удаление. \n" +
                                    "'n' := Отмена.");
                            switch (Console.ReadLine())
                            {
                                case "y":
                                    LIST_OF_SUBSCRIBERS.Remove(CurrentSub);
                                    Controller.Save(LIST_OF_SUBSCRIBERS);
                                    CurrentSub = null;
                                    Console.WriteLine("[Выполнено]: Абонент удален.");
                                    break;

                                case "n":
                                    break;
                            }
                        }
                        break;
                    #endregion

                    #region '7' Сортировка
                    // Отсортировать список
                    case "7":
                        Console.WriteLine("Отсортировать по: " +
                                    "'1' := Номеру Телефона.\n" +
                                    "'2' := Имени." +
                                    "'3' := Тарифному Плану.\n" +
                                    "'4' := Балансу.\n" +
                                    "'5' := Вернуться в меню.");
                        string input = Console.ReadLine();

                        switch (input)
                        {
                            case "1":
                                LIST_OF_SUBSCRIBERS.Sort((x, y) => x.Number.CompareTo(y.Number));
                                Controller.Save(LIST_OF_SUBSCRIBERS);
                                Console.WriteLine("[Выполнено]: Список абонентов отсортирова по Номеру Телефона.");
                                break;

                            case "2":
                                LIST_OF_SUBSCRIBERS.Sort((x, y) => x.Name.CompareTo(y.Name));
                                Controller.Save(LIST_OF_SUBSCRIBERS);
                                Console.WriteLine("[Выполнено]: Список абонентов отсортирова по Имени.");
                                break;

                            case "3":
                                LIST_OF_SUBSCRIBERS.Sort((x, y) => x.Tariff.tariff_id.CompareTo(y.Tariff.tariff_id));
                                Controller.Save(LIST_OF_SUBSCRIBERS);
                                Console.WriteLine("[Выполнено]: Список абонентов отсортирова по Тарифному плану.");
                                break;

                            case "4":
                                LIST_OF_SUBSCRIBERS.Sort((x, y) => x.Balance.CompareTo(y.Balance));
                                Controller.Save(LIST_OF_SUBSCRIBERS);
                                Console.WriteLine("[Выполнено]: Список абонентов отсортирова по Балансу.");
                                break;

                            case "5":
                                break;
                        }
                        break;
                    #endregion

                    #region '8' Повременная Плата
                    // Списание повременной платы
                    case "8":
                        string phone2 = CurrentSub.Number;

                        if (LIST_OF_SUBSCRIBERS.Exists(s => s.Number == phone2))
                        {
                            var somesub = LIST_OF_SUBSCRIBERS.Find(s => s.Number == phone2);
                            if (!(somesub.Tariff.tariff_id == 1 || somesub.Tariff.tariff_id == 2 || somesub.Tariff.tariff_id == 3)) 
                            {
                                Console.WriteLine("Услуга не подключена. Звонок не доступен.");
                                break;
                            }
                            if (somesub.Balance < 0)
                            {
                                Console.WriteLine("Отрицательный Баланс. Звонок не доступен.");
                                break;
                            }
                            
                            int seconds;
                            Console.WriteLine("Введите количество секунд:");
                            try
                            {
                                int secs = int.Parse(Console.ReadLine());
                                seconds = secs;
                            }
                            catch
                            {
                                Console.WriteLine("[Ошибка]: Неправильно введенная сумма.");
                                break;
                            }

                            Controller.CalculatePay(somesub, seconds);
                            Console.WriteLine("Текущий Баланс: {0}", CurrentSub.Balance);

                            Controller.Save(LIST_OF_SUBSCRIBERS);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Номер не найден.");
                            break;
                        }
                    #endregion

                    #region '9' Абонентская плата
                    // Списание абонентской плата
                    case "9":
                        if (LIST_OF_SUBSCRIBERS.Count == 0) Console.WriteLine("База данных абонентов пуста.");
                        else
                        {
                            Controller.MonthPay(LIST_OF_SUBSCRIBERS);
                            Controller.Save(LIST_OF_SUBSCRIBERS);
                            Console.WriteLine("[Выполнено]: Списание абонентской платы за месяц.");
                        }
                        break;
                    #endregion

                    #region '0' Просмотр Выбранного Абонента
                    case "0":
                        if (CurrentSub == null || (CurrentSub.Name) == null && CurrentSub.Number == null) Console.WriteLine("Абонент не выбран.");
                        else
                        {
                            Console.WriteLine("Текущий абонент:");
                            Console.WriteLine(CurrentSub.ToString());
                        }
                        break;
                        #endregion
                }
            }
        }
    }
}