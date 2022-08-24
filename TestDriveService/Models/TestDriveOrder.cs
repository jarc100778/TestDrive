using System.ComponentModel.DataAnnotations;

namespace TestDriveService.Models
{
    /// <summary>
    /// Заявка на тест-драйв 
    /// </summary>
    public class TestDriveOrder
    {
        /// <summary>
        /// ИД заявки
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// ФИО клиента
        /// </summary>
        [Required]
        public string FIO { get; set; }
        /// <summary>
        /// Телефон клиента
        /// </summary>
        [Required]
        public string Phone { get; set; }
        /// <summary>
        /// Дата и время тест-драйва заявленное клиентом.
        /// </summary>
        [Required]
        public DateTime TestDriveDT { get; set; }
        /// <summary>
        /// ссылка на машину
        /// </summary>
        [Required]
        public int CarId { get; set; }
        /// <summary>
        /// ссылка на машину
        /// </summary>
        [Required]
        public Car Car { get; set; }

    }
}
