using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1_ISTP
{
    public partial class Prices
    {
        public Prices()
        {
            Perfumes = new HashSet<Perfumes>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ціна")]
        public int? Price { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Дата сотворення")]
        public DateTime? DateCreation { get; set; }
        [Display(Name = "Валюта")]
        public int? CurrencyId { get; set; }

        public virtual Currencys Currency { get; set; }
        public virtual ICollection<Perfumes> Perfumes { get; set; }
    }
}
