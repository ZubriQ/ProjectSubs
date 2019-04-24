using OOOSubs.BL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OOOSubs.BL.Controller
{
    public class SubsController
    {
        // Сохранение списка абонентов
        public void Save(List<Subscriber> subList)
        {
            try
            {
                using (Stream stream = File.Open("data.bin", FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, subList);
                    stream.Close();
                }
            }
            catch (IOException)
            {
            }
        }

        // Добавление нового абонента
        public void AddNewSub(List<Subscriber> subscribers)
        {
            try
            {
                using (Stream stream = File.Open("data.bin", FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, subscribers);
                    stream.Close();
                }
            }
            catch (IOException)
            {
            }
        }

        // Поиск/Выбор абонента
        public bool FindSub(string number, List<Subscriber> subs, ref Subscriber currentSub)
        {
            if (subs.Exists(s => s.Number == number))
            {
                currentSub = subs.Find(s => s.Number == number);
                return true;
            }
            else return false;
        }

        // Повременная плата
        public void CalculatePay(Subscriber sub, int seconds)
        {
            if (sub.Tariff.tariff_id == 1)
            {
                sub.Balance -= Math.Ceiling((double)seconds / 60) * 5;
            }
            else if (sub.Tariff.tariff_id == 2)
            {
                sub.Balance -= Math.Ceiling((double)seconds / 60) * 15;
            }
            else if (sub.Tariff.tariff_id == 3)
            {
                sub.Balance -= Math.Ceiling(seconds * 0.15);
            }
            else
            {
                throw new Exception("[Ошибка]");
            }
        }
        
        // Абонентская плата
        public void MonthPay(List<Subscriber> sub)
        {
            foreach (Subscriber Sub in sub)
            {
                if (Sub.Tariff.tariff_id == 1)
                {
                    Sub.Balance -= 300;
                }
                else if (Sub.Tariff.tariff_id == 2)
                {
                    Sub.Balance -= 570;
                }
                else if (Sub.Tariff.tariff_id == 3)
                {
                    Sub.Balance -= 440;
                }
            }
        }
    }

    
}
