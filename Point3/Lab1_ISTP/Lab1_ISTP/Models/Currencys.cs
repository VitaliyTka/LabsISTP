using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Lab1_ISTP
{
    public partial class Currencys
    {
        public Currencys()
        {
            Prices = new HashSet<Prices>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Валюта")]
        public string Currency { get; set; }

        public virtual ICollection<Prices> Prices { get; set; }
    }
}
