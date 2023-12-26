using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservationSystem.Models
{
    public class Room
    {
        // Свойство, представляющее номер комнаты
        public int Number { get; set; }

        // Свойство, представляющее тип комнаты
        public string? Type { get; set; }

        // Свойство, представляющее цену за проживание в комнате
        public decimal Price { get; set; }

        // Свойство, указывающее, забронирована ли комната или нет
        public bool IsBooked { get; set; }

        // Переопределенный метод ToString для удобного вывода информации о комнате
        public override string ToString()
        {
            return $"Room Number: {Number}, Type: {Type}, Price: {Price}, Booked: {IsBooked}";
        }
    }
}