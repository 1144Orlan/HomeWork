using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcCreditApp.Models
{
    public class Credit
    {
        //ID кредита
        public virtual int CreditId { get; set; }

        //Название
        [DisplayName("Название кредита")]
        public virtual string Head { get; set; }

        //Период, на который выдается кредит
        [Required]
        [DisplayName("Период кредита")]
        public virtual int Period { get; set; }

        //Максимальная сумма кредита
        [DisplayName("Максимальная сумма")]
        public virtual int Sum { get; set; }

        //Процентная ставка
        [Required]
        [DisplayName("Ставка")]
        public virtual int Procent { get; set; }
    }
}
