namespace MinefieldExperiments
{
    public interface IRandomIntegerGenerator
    {
        public int NextValue(int fromNumber, int toNumber);
    }

    /***
     * Basic pseudo-random number generator using "X = ( A * (X-previous) + C ) mod M" algorithm
     ***/
    public class RandomIntegerGenerator : IRandomIntegerGenerator
    {
        // for simplicity, range is only 16 bits 
        const int minValue = 0;
        const int maxValue = 65535;
        const int modulus = 65536;
        const int seed = 128;

        private int multiplier;
        private int increment;
        private int currentValue;

        public RandomIntegerGenerator(int multiplier = 7, int increment = 1)
        {
            if (multiplier < 1 || multiplier >= modulus || increment < 1 || increment >= modulus)
                throw new ArgumentException("Multiplier or increment values out of range");

            if (!IsPrimeNumber(multiplier))  // prime number will greatly reduce chance of repeating sequences
                throw new ArgumentException("Multiplier has to be a prime number");

            this.multiplier = multiplier;
            this.increment = increment;

            this.currentValue = ((seed * this.multiplier) + this.increment) % modulus;
            //Console.WriteLine("MULTIPLIER {0}, INCREMENT is {1}, currentValue is {2}", this.multiplier, this.increment, this.currentValue);
        }

        public int NextValue(int fromNumber = 0, int toNumber = maxValue)
        {
            int range = Math.Abs(toNumber - fromNumber);  // abs to handle negative from/to
            // range has to > 2
            if (range < 2)
                throw new ArgumentException("Invalid from/to arguments");

            // get value between min and max, use abs for safety
            currentValue = Math.Abs(((currentValue * multiplier) + increment) % modulus);
            // fit the range
            int returnValue = fromNumber + (currentValue % range);

            return returnValue;
        }

        private bool IsPrimeNumber(int value)
        {
            int n = 0;
            // algorithm to check prime number
            for (int i = 2; i < (value / 2 + 1); i++)
            {
                if (value % i == 0)
                {
                    n++;
                    break;
                }
            }
            return n == 0;
        }

    }
}
