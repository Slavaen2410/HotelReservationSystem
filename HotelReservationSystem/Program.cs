using System;
using HotelReservationSystem.Services;

namespace HotelReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создание экземпляра HotelService
            HotelService hotelService = new HotelService();

            // Бесконечный цикл для интерактивного взаимодействия с пользователем
            while (true)
            {
                // Вывод меню
                Console.WriteLine("1. View All Rooms");
                Console.WriteLine("2. Reserve a Room");
                Console.WriteLine("3. Cancel Booking");
                Console.WriteLine("4. View All Bookings");
                Console.WriteLine("5. View Bookings for Room");
                Console.WriteLine("0. Exit");

                // Запрос ввода пользователя
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                // Обработка выбора пользователя
                switch (choice)
                {
                    case "1":
                        // Просмотр всех комнат
                        hotelService.ViewAllRooms();
                        break;
                    case "2":
                        // Бронирование комнаты
                        ReserveRoom(hotelService);
                        break;
                    case "3":
                        // Отмена бронирования
                        CancelBooking(hotelService);
                        break;
                    case "4":
                        // Просмотр всех бронирований
                        hotelService.ViewAllBookings();
                        break;
                    case "5":
                        // Просмотр бронирований для определенной комнаты
                        ViewBookingsForRoom(hotelService);
                        break;
                    case "0":
                        // Выход из программы
                        Environment.Exit(0);
                        break;
                    default:
                        // Вывод сообщения об ошибке при неверном выборе
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        static void ReserveRoom(HotelService hotelService)
        {
            // Запрос имени гостя
            Console.Write("Enter guest name: ");
            string guestName = Console.ReadLine();

            // Запрос номера комнаты с обработкой ошибок ввода
            Console.Write("Enter room number: ");
            int roomNumber;
            while (!int.TryParse(Console.ReadLine(), out roomNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid room number.");
            }

            // Запрос даты заезда с обработкой ошибок ввода
            Console.Write("Enter check-in date (yyyy-MM-dd): ");
            DateTime checkInDate;
            while (!DateTime.TryParse(Console.ReadLine(), out checkInDate))
            {
                Console.WriteLine("Invalid input. Please enter a valid date (yyyy-MM-dd).");
            }

            // Запрос даты выезда с обработкой ошибок ввода
            Console.Write("Enter check-out date (yyyy-MM-dd): ");
            DateTime checkOutDate;
            while (!DateTime.TryParse(Console.ReadLine(), out checkOutDate))
            {
                Console.WriteLine("Invalid input. Please enter a valid date (yyyy-MM-dd).");
            }

            // Вызов метода бронирования комнаты в HotelService
            hotelService.ReserveRoom(guestName, roomNumber, checkInDate, checkOutDate);
        }


        static void CancelBooking(HotelService hotelService)
        {
            // Запрос ID бронирования с обработкой ошибок ввода
            Console.Write("Enter booking ID: ");
            int bookingId;
            while (!int.TryParse(Console.ReadLine(), out bookingId))
            {
                Console.WriteLine("Invalid input. Please enter a valid booking ID.");
            }

            // Вызов метода отмены бронирования в HotelService
            hotelService.CancelBooking(bookingId);
        }

        static void ViewBookingsForRoom(HotelService hotelService)
        {
            // Запрос номера комнаты с обработкой ошибок ввода
            Console.Write("Enter room number: ");
            int roomNumber;
            while (!int.TryParse(Console.ReadLine(), out roomNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid room number.");
            }

            // Вызов метода просмотра бронирований для указанной комнаты в HotelService
            hotelService.ViewBookingsForRoom(roomNumber);
        }

    }
}