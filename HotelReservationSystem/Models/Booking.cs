using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservationSystem.Models
{
    public class Booking
    {
        // Идентификатор бронирования
        public int BookingId { get; set; }

        // Имя гостя, может быть null
        public string? GuestName { get; set; }

        // Номер комнаты
        public int RoomNumber { get; set; }

        // Дата заселения
        public DateTime CheckInDate { get; set; }

        // Дата выселения
        public DateTime CheckOutDate { get; set; }

        // Переопределение метода ToString для получения текстового представления объекта
        public override string ToString()
        {
            return $"Booking ID: {BookingId}, Guest: {GuestName}, Room: {RoomNumber}, " +
                   $"Check-in: {CheckInDate.ToShortDateString()}, Check-out: {CheckOutDate.ToShortDateString()}";
        }
    }
}