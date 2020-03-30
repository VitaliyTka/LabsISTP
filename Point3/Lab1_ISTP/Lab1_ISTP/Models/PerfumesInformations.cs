using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_ISTP
{
    public partial class PerfumesInformations
    {
        public PerfumesInformations()
        {
            Packings = new HashSet<Packings>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Рік створення")]
        public int? YearOfIssue { get; set; }
        public int? ClassificationPerfumeId { get; set; }
        public int? ManufacturerId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Шлях зображення")]
        public string PicturePath { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва парфуму")]
        public string PerfumeName { get; set; }

        public virtual ClassificationsPerfumes ClassificationPerfume { get; set; }
        public virtual Manufacturers Manufacturer { get; set; }
        public virtual ICollection<Packings> Packings { get; set; }
    }
}
