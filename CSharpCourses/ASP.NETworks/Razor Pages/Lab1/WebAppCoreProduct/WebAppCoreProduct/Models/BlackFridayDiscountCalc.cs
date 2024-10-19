using System.Reflection;

namespace WebAppCoreProduct.Models
{
	public interface IBFDiscountCalc
	{
		decimal? BFDiscountCalculation(decimal? price);
	}
	public class BlackFridayDiscountCalc : IBFDiscountCalc
	{
		public decimal? BFDiscountCalculation(decimal? price) => price * 2;
	}
}
