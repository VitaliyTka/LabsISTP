using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_ISTP
{
    public partial class TypesPackings
    {
        public TypesPackings()
        {
            Packings = new HashSet<Packings>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Тип упаковки")]
        public string TypePacking { get; set; }

        public virtual ICollection<Packings> Packings { get; set; }
    }
}
