using System;

namespace ShoppingCartApi.Model
{
    public class Percent
    {
        public static Percent Zero = new Percent(0);
        public static Percent OneHundred = new Percent(100);

        public Percent(double value)
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
            var other = (Percent) obj;
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