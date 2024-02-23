namespace Quantum_Computing
{

    // Complex Number Class
    public class ComplexNumber
    {
        // a + bi
        public double a { get; set; }
        public double b { get; set; }

        // Constructor
        public ComplexNumber()
        {
            a = 0;
            b = 0;
        }
        public ComplexNumber(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        // Operator overloading
        // Overloading the + operator
        public static ComplexNumber operator +(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.a + c2.a, c1.b + c2.b);
        }

        // Overloading the - operator
        public static ComplexNumber operator -(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.a - c2.a, c1.b - c2.b);
        }

        // Overloading the * operator
        public static ComplexNumber operator *(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.a * c2.a - c1.b * c2.b, c1.a * c2.b + c1.b * c2.a);
        }

        // Overloading the / operator
        public static ComplexNumber operator /(ComplexNumber c1, ComplexNumber c2)
        {
            double denominator = c2.a * c2.a + c2.b * c2.b;
            if(denominator == 0) // Check if the denominator is zero
            {
                throw new DivideByZeroException("Denominator cannot be zero");
            }
            
            return new ComplexNumber((c1.a * c2.a + c1.b * c2.b) / denominator, (c1.b * c2.a - c1.a * c2.b) / denominator);
        }

        // Overloading the == operator
        public static bool operator ==(ComplexNumber c1, ComplexNumber c2)
        {
            if (ReferenceEquals(c1, null))
            {
                return ReferenceEquals(c2, null);
            }
            else if (ReferenceEquals(c2, null))
            {
                return false;
            }
            return (c1.a == c2.a) && (c1.b == c2.b);
        }

        // Overloading the != operator
        public static bool operator !=(ComplexNumber c1, ComplexNumber c2)
        {
            if (ReferenceEquals(c1, null))
            {
                return !ReferenceEquals(c2, null);
            }
            else if (ReferenceEquals(c2, null))
            {
                return true;
            }
            return (c1.a != c2.a) || (c1.b != c2.b);
        }

        // Overloading the unary - operator
        public static ComplexNumber operator -(ComplexNumber c)
        {
            return new ComplexNumber(-c.a, -c.b);
        }

        // Overloading the ++ operator
        public static ComplexNumber operator ++(ComplexNumber c)
        {
            return new ComplexNumber(c.a + 1, c.b);
        }

        // Overloading the -- operator
        public static ComplexNumber operator --(ComplexNumber c)
        {
            return new ComplexNumber(c.a - 1, c.b);
        }

        // Overriding the Equals method
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            ComplexNumber c = (ComplexNumber)obj;
            return (a == c.a) && (b == c.b);
        }

        // Overriding the GetHashCode method
        public override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode();
        }

        // Overriding the ToString method
        public override string ToString()
        {
            return b >= 0 ? a + " + " + b + "i" : a + " - " + (-b) + "i";
        }

        // Functions
        // Conjugate
        public ComplexNumber Conjugate()
        {
            return new ComplexNumber(a, -b);
        }

        // Modulus

        public double Modulus()
        {
            return Math.Sqrt(a * a + b * b);
        }

        // Phase
        public double Phase()
        {
            return Math.Atan2(b, a);
        }

    }


}
