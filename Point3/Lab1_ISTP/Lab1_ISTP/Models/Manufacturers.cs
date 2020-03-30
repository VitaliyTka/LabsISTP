using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_ISTP
{
    public partial class Manufacturers
    {
        public Manufacturers()
        {
            PerfumesInformations = new HashSet<PerfumesInformations>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Виробник")]
        public string Manufacturer { get; set; }

        public virtual ICollection<PerfumesInformations> PerfumesInformations { get; set; }
    }
}
