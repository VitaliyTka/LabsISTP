using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_ISTP
{
    public partial class ClassificationsPerfumes
    {
        public ClassificationsPerfumes()
        {
            PerfumesInformations = new HashSet<PerfumesInformations>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Класифікація парфуму")]
        public string ClassificationPerfume { get; set; }

        public virtual ICollection<PerfumesInformations> PerfumesInformations { get; set; }
    }
}
