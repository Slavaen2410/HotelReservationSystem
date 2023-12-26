using System;
using System.Collections.Generic;
using System.IO;
using HotelReservationSystem.Models;
using HotelReservationSystem.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;

namespace HotelReservationSystem.Services
{
    public class HotelService
    {
        // Интерфейс для логирования событий в HotelService
        private readonly ILogger _logger;

        // Путь к файлу с данными о комнатах
        private readonly string roomsFilePath;

        // Путь к файлу с данными о бронированиях
        private readonly string bookingsFilePath;

        // Список объектов Room, представляющих комнаты в гостинице
        private List<Room> rooms;

        // Список объектов Booking, представляющих информацию о бронированиях в гостинице
        private List<Booking> bookings;

        // Счетчик для генерации уникальных идентификаторов бронирований
        private int bookingIdCounter;

        public HotelService()
        {
            // Создание и настройка экземпляра логгера Serilog
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            // Загрузка настроек из конфигурационного файла
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Получение путей к файлам из конфигурации
            roomsFilePath = configuration.GetSection("FilePaths:Rooms").Value;
            bookingsFilePath = configuration.GetSection("FilePaths:Bookings").Value;

            // Загрузка данных
            LoadData();
        }

        public void ViewAllRooms()
        {
            // Вывод заголовка
            Console.WriteLine("All Rooms:");

            // Проверка наличия комнат в списке
            if (rooms != null && rooms.Any())
            {
                // Перебор всех комнат
                foreach (var room in rooms)
                {
                    // Проверка, забронирована ли комната
                    var isBooked = bookings.Any(booking => booking.RoomNumber == room.Number);

                    // Вывод информации о комнате (номер, тип, цена, статус бронирования)
                    Console.WriteLine($"Room Number: {room.Number}, Type: {room.Type}, Price: {room.Price}, Booked: {isBooked}");
                }
            }
            else
            {
                // Вывод сообщения, если нет доступных комнат
                Console.WriteLine("No rooms available.");
            }

            // Пустая строка для улучшения читаемости вывода
            Console.WriteLine();
        }



        public void ReserveRoom(string guestName, int roomNumber, DateTime checkInDate, DateTime checkOutDate)
{
    try
    {
        // Логирование информации о бронировании комнаты для гостя
        _logger.Information($"Reserving room for {guestName}, RoomNumber: {roomNumber}, CheckInDate: {checkInDate}, CheckOutDate: {checkOutDate}.");

        // Проверка доступности комнаты на указанные даты
        if (!IsRoomAvailable(roomNumber, checkInDate, checkOutDate))
        {
            Console.WriteLine($"Error: Room {roomNumber} is not available for the specified dates.");
            _logger.Warning($"Room {roomNumber} is not available for the specified dates.");
            return;
        }

        // Создание нового бронирования
        Booking newBooking = new Booking
        {
            BookingId = bookingIdCounter++,
            GuestName = guestName,
            RoomNumber = roomNumber,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };

        // Добавление бронирования в список
        bookings.Add(newBooking);

        // Пометка комнаты как забронированной
        Room bookedRoom = rooms.Find(room => room.Number == roomNumber);
        if (bookedRoom != null)
        {
            bookedRoom.IsBooked = true;
        }

        // Вывод сообщения об успешном бронировании
        Console.WriteLine($"Success: Room {roomNumber} has been successfully booked for {guestName}.");

        // Логирование информации о бронировании
        _logger.Information($"Room {roomNumber} booked for {guestName}.");

        // Сохранение обновленных данных
        SaveData();
    }
    catch (Exception ex)
    {
        // Вывод сообщения об ошибке при бронировании комнаты
        Console.WriteLine($"Error: Failed to reserve room. {ex.Message}");

        // Логирование ошибки при бронировании комнаты
        _logger.Error($"Failed to reserve room: {ex.Message}");
    }
}


        public void CancelBooking(int bookingId)
        {
            try
            {
                // Логирование информации об отмене бронирования с указанным ID
                _logger.Information($"Canceling booking with ID: {bookingId}.");

                // Поиск бронирования по указанному ID
                Booking bookingToCancel = bookings.Find(booking => booking.BookingId == bookingId);

                // Если бронирование не найдено, вывод сообщения об ошибке
                if (bookingToCancel == null)
                {
                    Console.WriteLine($"Error: Booking with ID {bookingId} not found.");
                    _logger.Warning($"Booking with ID {bookingId} not found.");
                    return;
                }

                // Нахождение комнаты, связанной с отменяемым бронированием
                Room bookedRoom = rooms.Find(room => room.Number == bookingToCancel.RoomNumber);

                // Если комната найдена, пометить её как доступную
                if (bookedRoom != null)
                {
                    bookedRoom.IsBooked = false;
                }

                // Удаление бронирования из списка
                bookings.Remove(bookingToCancel);

                // Вывод сообщения об успешной отмене бронирования
                Console.WriteLine($"Success: Booking with ID {bookingId} has been successfully canceled.");

                // Логирование информации об отмене бронирования
                _logger.Information($"Booking with ID {bookingId} canceled.");

                // Сохранение обновленных данных
                SaveData();
            }
            catch (Exception ex)
            {
                // Вывод сообщения об ошибке при отмене бронирования
                Console.WriteLine($"Error: Failed to cancel booking. {ex.Message}");

                // Логирование ошибки при отмене бронирования
                _logger.Error($"Failed to cancel booking: {ex.Message}");
            }
        }


        public void ViewAllBookings()
        {
            try
            {
                // Логирование информации о просмотре всех бронирований
                _logger.Information("Viewing all bookings.");

                // Вывод информации о всех бронированиях
                Console.WriteLine("All Bookings:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine(booking);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                // Логирование ошибки, если что-то пошло не так при просмотре всех бронирований
                _logger.Error($"Error occurred while viewing all bookings: {ex.Message}");
            }
        }


        public List<Booking> GetBookingsForRoom(int roomNumber)
        {
            // Возвращает список бронирований для указанной комнаты
            return bookings.FindAll(booking => booking.RoomNumber == roomNumber);
        }

        public void ViewBookingsForRoom(int roomNumber)
        {
            try
            {
                // Логирование информации о просмотре бронирований для указанной комнаты
                _logger.Information($"Viewing bookings for Room {roomNumber}.");

                // Получение бронирований для указанной комнаты
                var roomBookings = GetBookingsForRoom(roomNumber);

                // Вывод информации о бронированиях для указанной комнаты
                Console.WriteLine($"Bookings for Room {roomNumber}:");
                foreach (var booking in roomBookings)
                {
                    Console.WriteLine(booking);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                // Логирование ошибки, если что-то пошло не так при просмотре бронирований
                _logger.Error($"Error occurred while viewing bookings for Room {roomNumber}: {ex.Message}");
            }
        }


        public List<Room> GetRooms()
        {
            // Возвращает список комнат
            return rooms;
        }


        private void LoadData()
        {
            try
            {
                // Логгирование информации о начале загрузки данных
                _logger.Information("Loading data from files...");

                // Чтение JSON-строки из файла с информацией о комнатах
                string roomsJson = File.ReadAllText(roomsFilePath);

                // Десериализация JSON в коллекцию объектов Room
                rooms = JsonConvert.DeserializeObject<List<Room>>(roomsJson);

                // Чтение JSON-строки из файла с информацией о бронированиях
                string bookingsJson = File.ReadAllText(bookingsFilePath);

                // Десериализация JSON в коллекцию объектов Booking
                bookings = JsonConvert.DeserializeObject<List<Booking>>(bookingsJson,
                    new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" });

                // Установка счетчика идентификаторов бронирований на следующий доступный идентификатор
                bookingIdCounter = bookings.Count + 1;
            }
            catch (FileNotFoundException)
            {
                // Обработка исключения, если файлы данных не найдены
                Console.WriteLine("Data files not found. Creating new collections.");
                _logger.Warning("Data files not found. Creating new collections.");

                // Создание новых пустых коллекций в случае отсутствия файлов данных
                rooms = new List<Room>();
                bookings = new List<Booking>();
            }
            catch (Exception ex)
            {
                // Обработка общего исключения при возникновении ошибок загрузки данных
                Console.WriteLine($"Failed to load data: {ex.Message}");
                _logger.Error($"Failed to load data: {ex.Message}");

                // Создание новых пустых коллекций в случае ошибки загрузки данных
                rooms = new List<Room>();
                bookings = new List<Booking>();
            }
        }

        private void SaveData()
        {
            try
            {
                // Логгируем начало процесса сохранения данных
                _logger.Information("Сохранение данных в файлы...");

                // Сериализуем данные о комнатах в формат JSON с отступами
                string roomsJson = JsonConvert.SerializeObject(rooms, Formatting.Indented);

                // Сериализуем данные о бронированиях в формат JSON с указанием формата даты
                string bookingsJson = JsonConvert.SerializeObject(bookings, Formatting.Indented,
                    new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" });

                // Записываем данные о комнатах в соответствующий файл
                File.WriteAllText(roomsFilePath, roomsJson);

                // Записываем данные о бронированиях в соответствующий файл
                File.WriteAllText(bookingsFilePath, bookingsJson);

                // Выводим сообщение о успешном сохранении данных в консоль
                Console.WriteLine("Данные сохранены в файлы.");
            }
            catch (Exception ex)
            {
                // В случае ошибки выводим сообщение об ошибке в консоль
                Console.WriteLine($"Не удалось сохранить данные: {ex.Message}");

                // Записываем сообщение об ошибке в лог
                _logger.Error($"Не удалось сохранить данные: {ex.Message}");
            }
        }

        private bool IsRoomAvailable(int roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            // Проходим по всем бронированиям
            foreach (var booking in bookings)
            {
                // Проверяем, соответствует ли номер комнаты
                // и период бронирования текущему запросу
                if (booking.RoomNumber == roomNumber &&
                    (checkInDate >= booking.CheckInDate && checkInDate < booking.CheckOutDate ||
                     checkOutDate > booking.CheckInDate && checkOutDate <= booking.CheckOutDate))
                {
                    // Если найдено пересечение периодов, комната недоступна
                    return false;
                }
            }

            // Если не найдено пересечений, комната доступна
            return true;
        }

    }
}
