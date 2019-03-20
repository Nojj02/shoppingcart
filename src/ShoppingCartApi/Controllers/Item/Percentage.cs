using System;

namespace ShoppingCartApi.Controllers.Item
{
    public class Percentage
    {
        public static Percentage Zero = new Percentage(0);
        public static Percentage OneHundred = new Percentage(100);

        public Percentage(double value)
        {
            Value = value;
        }

        public double Value { get; }

        /// <summary>
        /// Value in decimal representation (NOT decimal type)
        /// </summary>
        public double DecimalValue => Value / 100;

        public override bool Equals(object obj)
        {
            var other = (Percentage) obj;
            return Math.Abs(Value - other.Value) < Double.Epsilon;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public decimal Of(decimal value)
        {
            return (decimal)DecimalValue * value;
        }

        public double Of(double value)
        {
            return DecimalValue * value;
        }
    }
}