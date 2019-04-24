using System;

namespace OOOSubs.BL.Model
{
    [Serializable()]
    public class Tariff
    {
        /// <summary>
        /// Номер тарифа (1,2,3).
        /// </summary>
        public int tariff_id { get; set; }

        public Tariff(int t)
        {
            tariff_id = t;
        }

        public Tariff()
        {
            tariff_id = 0;
        }
        
        public override string ToString()
        {
            if (tariff_id == 1) return "'[1]Стандарт'";
            else if (tariff_id == 2) return "'[2]Премиум'";
            else if (tariff_id == 3) return "'[3]Посекундный'";
            else return "'[0]Тариф не выбран'";
        }
    }
}