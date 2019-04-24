using System;

namespace OOOSubs.BL.Model
{
    [Serializable()]
    public class Subscriber
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Тариф (1,2,3).
        /// </summary>
        public Tariff Tariff { get; set; }
        
        /// <summary>
        /// Баланс.
        /// </summary>
        public double Balance { get; set; }

        public Subscriber(string n, string N, Tariff t, double b)
        {
            Name = n;
            Number = N;
            Tariff = t;
            Balance = b;
        }

        public Subscriber()
        {
            Name = null;
            Number = null;
            Tariff = null;
            Balance = 0;
        }

        public override string ToString()
        {
            return $"Имя:{Name},\tНомер:{Number},\tТариф:{Tariff},\tБаланс:{Balance}";
        }
    }
}