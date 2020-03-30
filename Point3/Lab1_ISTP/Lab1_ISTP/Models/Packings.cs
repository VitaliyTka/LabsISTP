using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_ISTP
{
    public partial class Packings
    {
        public Packings()
        {
            Perfumes = new HashSet<Perfumes>();
        }

        public int Id { get; set; }
        [Display(Name = "Тип пакунку Id")]
        public int? TypePackingId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Об'єм")]
        public int? Volume { get; set; }
        [Display(Name = "Інформація парфуму Id")]
        public int? PerfumeInformationId { get; set; }

        public virtual PerfumesInformations PerfumeInformation { get; set; }
        public virtual TypesPackings TypePacking { get; set; }
        public virtual ICollection<Perfumes> Perfumes { get; set; }
    }
}
