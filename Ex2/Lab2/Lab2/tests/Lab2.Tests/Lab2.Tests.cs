using Lab2.Domain;
using Lab2.Exceptions;
using Lab2.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Lab2.Tests;

public class Tests
{
        [Fact]
        public void AddItem_ValidBook_ItemAdded()
        {
            var service = new LibraryService();
            var book = new Book(1, "Test Book", "Author", "1234567890");
            
            service.AddItem(book);
            
            var items = service.GetAllItems();
            Assert.Contains(book, items);
        }

        [Fact]
        public void CreateReservation_AvailableItem_ReservationCreated()
        {
            var service = new LibraryService();
            var book = new Book(1, "Test Book", "Author", "1234567890");
            service.AddItem(book);
            service.RegisterUser("test@email.com");
            
            var reservation = service.CreateReservation(1, "test@email.com", 
                DateTime.Now, DateTime.Now.AddDays(7));
            
            Assert.NotNull(reservation);
            Assert.False(book.IsActive);
        }
        

        [Fact]
        public void CreateReservation_ConflictingDates_ThrowsReservationConflictException()
        {
            var service = new LibraryService();
            var book = new Book(1, "Test Book", "Author", "1234567890");
            service.AddItem(book);
            service.RegisterUser("test@email.com");

            var from = DateTime.Now;
            var to = from.AddDays(7);

            service.CreateReservation(1, "test@email.com", from, to);
            
            Assert.Throws<ReservationConflictException>(() => 
                service.CreateReservation(1, "test@email.com", from.AddDays(1), to.AddDays(1)));
        }

        [Fact]
        public void CancelReservation_ValidId_ReservationCancelled()
        {
            var service = new LibraryService();
            var book = new Book(1, "Test Book", "Author", "1234567890");
            service.AddItem(book);
            service.RegisterUser("test@email.com");
            var reservation = service.CreateReservation(1, "test@email.com", 
                DateTime.Now, DateTime.Now.AddDays(7));

            bool eventFired = false;
            service.OnReservationCancelled += r => eventFired = true;
            
            service.CancelReservation(reservation.Id);

            Assert.True(book.IsActive);
            Assert.True(eventFired);
        }
}