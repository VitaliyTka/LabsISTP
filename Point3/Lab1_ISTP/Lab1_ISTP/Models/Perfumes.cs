using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_ISTP
{
    public partial class Perfumes
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name = "Упаковка")]
        public int? PackingId { get; set; }
        [Display(Name = "Ціна")]
        public int? PriceId { get; set; }

        public virtual Packings Packing { get; set; }
        public virtual Prices Price { get; set; }
    }
}
