/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 08/01/2017
 * Time: 16:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;

namespace CsPotrace.BezierToBiarc
{


  /// <summary>Rappresenta un numero complesso.</summary>
  
  [Serializable]
  public struct Complex : IEquatable<Complex>, IFormattable
  {
    /// <summary>Restituisce una nuova istanza di <see cref="T:System.Numerics.Complex" /> con un numero reale uguale a zero e un numero immaginario uguale a zero.</summary>
    
    public static readonly Complex Zero = new Complex(0.0, 0.0);
    /// <summary>Restituisce una nuova istanza di <see cref="T:System.Numerics.Complex" /> con un numero reale uguale a uno e un numero immaginario uguale a zero.</summary>
    
    public static readonly Complex One = new Complex(1.0, 0.0);
    /// <summary>Restituisce una nuova istanza di <see cref="T:System.Numerics.Complex" /> con un numero reale uguale a zero e un numero immaginario uguale a uno.</summary>
    
    public static readonly Complex ImaginaryOne = new Complex(0.0, 1.0);
    private double m_real;
    private double m_imaginary;
    private const double LOG_10_INV = 0.43429448190325;

    /// <summary>Ottiene il componente reale dell'oggetto <see cref="T:System.Numerics.Complex" /> corrente.</summary>
    /// <returns>Componente reale di un numero complesso.</returns>
    
    public double Real
    {
       get
      {
        return this.m_real;
      }
    }

    /// <summary>Ottiene il componente immaginario dell'oggetto <see cref="T:System.Numerics.Complex" /> corrente.</summary>
    /// <returns>Componente immaginario di un numero complesso.</returns>
    
    public double Imaginary
    {
       get
      {
        return this.m_imaginary;
      }
    }

    /// <summary>Ottiene la grandezza (o valore assoluto) di un numero complesso.</summary>
    /// <returns>Grandezza dell'istanza corrente.</returns>
    
    public double Magnitude
    {
       get
      {
        return Complex.Abs(this);
      }
    }

    /// <summary>Ottiene la fase di un numero complesso.</summary>
    /// <returns>Fase di un numero complesso, in radianti.</returns>
    
    public double Phase
    {
       get
      {
        return Math.Atan2(this.m_imaginary, this.m_real);
      }
    }

    /// <summary>Inizializza una nuova istanza della struttura <see cref="T:System.Numerics.Complex" /> usando i valori reali e immaginari specificati.</summary>
    /// <param name="real">Parte reale del numero complesso.</param>
    /// <param name="imaginary">Parte immaginaria del numero complesso.</param>
    
    public Complex(double real, double imaginary)
    {
      this.m_real = real;
      this.m_imaginary = imaginary;
    }

    /// <summary>Definisce una conversione implicita di un Intero con segno a 16 bit in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(short value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Definisce una conversione implicita di un Intero con segno a 32 bit in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(int value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Definisce una conversione implicita di un Intero con segno a 64 bit in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(long value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Definisce una conversione implicita di un intero senza segno a 16 bit in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(ushort value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Definisce una conversione implicita di un intero senza segno a 32 bit in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(uint value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Definisce una conversione implicita di un intero senza segno a 64 bit in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(ulong value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Definisce una conversione implicita di un byte con segno in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(sbyte value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Definisce una conversione implicita di un byte senza segno in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(byte value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Definisce una conversione implicita di un numero a virgola mobile a precisione singola in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(float value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Definisce una conversione implicita di un numero a virgola mobile a precisione doppia in un numero complesso.</summary>
    /// <returns>Oggetto contenente il valore del parametro <paramref name="value" /> come parte reale e zero come parte immaginaria.</returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static implicit operator Complex(double value)
    {
      return new Complex(value, 0.0);
    }

//    /// <summary>Definisce una conversione esplicita di un valore <see cref="T:System.Numerics.BigInteger" /> in un numero complesso. </summary>
//    /// <returns>Numero complesso contenente un componente reale uguale a <paramref name="value" /> e un componente immaginario uguale a zero. </returns>
//    /// <param name="value">Valore da convertire in un numero complesso.</param>
//    
//    public static explicit operator Complex(BigInteger value)
//    {
//      return new Complex((double) value, 0.0);
//    }

    /// <summary>Definisce una conversione esplicita di un valore <see cref="T:System.Decimal" /> in un numero complesso.</summary>
    /// <returns>Numero complesso contenente un componente reale uguale a <paramref name="value" /> e un componente immaginario uguale a zero. </returns>
    /// <param name="value">Valore da convertire in un numero complesso.</param>
    
    public static explicit operator Complex(Decimal value)
    {
      return new Complex((double) value, 0.0);
    }

    /// <summary>Restituisce l'inverso additivo di un numero complesso specificato.</summary>
    /// <returns>Risultato dei componenti <see cref="P:System.Numerics.Complex.Real" /> e <see cref="P:System.Numerics.Complex.Imaginary" /> del parametro <paramref name="value" /> moltiplicato per -1.</returns>
    /// <param name="value">Valore da negare.</param>
    
    public static Complex operator -(Complex value)
    {
      return new Complex(-value.m_real, -value.m_imaginary);
    }

    /// <summary>Somma due numeri complessi.</summary>
    /// <returns>Somma di <paramref name="left" /> e <paramref name="right" />.</returns>
    /// <param name="left">Primo valore da sommare.</param>
    /// <param name="right">Secondo valore da sommare.</param>
    
    public static Complex operator +(Complex left, Complex right)
    {
      return new Complex(left.m_real + right.m_real, left.m_imaginary + right.m_imaginary);
    }

    /// <summary>Sottrae un numero complesso da un altro numero complesso.</summary>
    /// <returns>Risultato della sottrazione di <paramref name="right" /> da <paramref name="left" />.</returns>
    /// <param name="left">Valore da cui sottrarre (minuendo).</param>
    /// <param name="right">Valore da sottrarre (sottraendo).</param>
    
    public static Complex operator -(Complex left, Complex right)
    {
      return new Complex(left.m_real - right.m_real, left.m_imaginary - right.m_imaginary);
    }

    /// <summary>Moltiplica due numeri complessi specificati.</summary>
    /// <returns>Prodotto di <paramref name="left" /> e <paramref name="right" />.</returns>
    /// <param name="left">Primo valore da moltiplicare.</param>
    /// <param name="right">Secondo valore da moltiplicare.</param>
    
    public static Complex operator *(Complex left, Complex right)
    {
      return new Complex(left.m_real * right.m_real - left.m_imaginary * right.m_imaginary, left.m_imaginary * right.m_real + left.m_real * right.m_imaginary);
    }

    /// <summary>Divide un numero complesso specificato per un altro numero complesso specificato.</summary>
    /// <returns>Risultato della divisione di <paramref name="left" /> in base a <paramref name="right" />.</returns>
    /// <param name="left">Valore da dividere.</param>
    /// <param name="right">Valore per cui dividere.</param>
    
    public static Complex operator /(Complex left, Complex right)
    {
      double real1 = left.m_real;
      double imaginary1 = left.m_imaginary;
      double real2 = right.m_real;
      double imaginary2 = right.m_imaginary;
      if (Math.Abs(imaginary2) < Math.Abs(real2))
      {
        double num = imaginary2 / real2;
        return new Complex((real1 + imaginary1 * num) / (real2 + imaginary2 * num), (imaginary1 - real1 * num) / (real2 + imaginary2 * num));
      }
      double num1 = real2 / imaginary2;
      return new Complex((imaginary1 + real1 * num1) / (imaginary2 + real2 * num1), (-real1 + imaginary1 * num1) / (imaginary2 + real2 * num1));
    }

    /// <summary>Restituisce un valore che indica se due numeri complessi sono uguali.</summary>
    /// <returns>true se i parametri <paramref name="left" /> e <paramref name="right" /> presentano lo stesso valore; in caso contrario, false.</returns>
    /// <param name="left">Primo numero complesso da confrontare.</param>
    /// <param name="right">Secondo numero complesso da confrontare.</param>
    
    public static bool operator ==(Complex left, Complex right)
    {
      if (left.m_real == right.m_real)
        return left.m_imaginary == right.m_imaginary;
      return false;
    }

    /// <summary>Restituisce un valore che indica se due numeri complessi non sono uguali.</summary>
    /// <returns>true se <paramref name="left" /> e <paramref name="right" /> non sono uguali; in caso contrario, false.</returns>
    /// <param name="left">Primo valore da confrontare.</param>
    /// <param name="right">Secondo valore da confrontare.</param>
    
    public static bool operator !=(Complex left, Complex right)
    {
      if (left.m_real == right.m_real)
        return left.m_imaginary != right.m_imaginary;
      return true;
    }

    /// <summary>Crea un numero complesso dalle coordinate polari di un punto.</summary>
    /// <returns>Numero complesso.</returns>
    /// <param name="magnitude">La grandezza che è la distanza dall'origine (l'intersezione dell'asse x con l'asse y) al numero.</param>
    /// <param name="phase">La fase che è l'angolo dalla riga all'asse orizzontale, espresso nei radianti.</param>
    
    public static Complex FromPolarCoordinates(double magnitude, double phase)
    {
      return new Complex(magnitude * Math.Cos(phase), magnitude * Math.Sin(phase));
    }

    /// <summary>Restituisce l'inverso additivo di un numero complesso specificato.</summary>
    /// <returns>Risultato dei componenti <see cref="P:System.Numerics.Complex.Real" /> e <see cref="P:System.Numerics.Complex.Imaginary" /> del parametro <paramref name="value" /> moltiplicato per -1.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Negate(Complex value)
    {
      return -value;
    }

    /// <summary>Somma due numeri complessi e restituisce il risultato.</summary>
    /// <returns>Somma di <paramref name="left" /> e <paramref name="right" />.</returns>
    /// <param name="left">Primo numero complesso da sommare.</param>
    /// <param name="right">Secondo numero complesso da sommare.</param>
    
    public static Complex Add(Complex left, Complex right)
    {
      return left + right;
    }

    /// <summary>Sottrae un numero complesso da un altro e restituisce il risultato.</summary>
    /// <returns>Risultato della sottrazione di <paramref name="right" /> da <paramref name="left" />.</returns>
    /// <param name="left">Valore da cui sottrarre (minuendo).</param>
    /// <param name="right">Valore da sottrarre (sottraendo).</param>
    
    public static Complex Subtract(Complex left, Complex right)
    {
      return left - right;
    }

    /// <summary>Restituisce il prodotto di due numeri complessi.</summary>
    /// <returns>Prodotto dei parametri <paramref name="left" /> e <paramref name="right" />.</returns>
    /// <param name="left">Primo numero complesso da moltiplicare.</param>
    /// <param name="right">Secondo numero complesso da moltiplicare.</param>
    
    public static Complex Multiply(Complex left, Complex right)
    {
      return left * right;
    }

    /// <summary>Divide un numero complesso per un altro e restituisce il risultato.</summary>
    /// <returns>Quoziente della divisione.</returns>
    /// <param name="dividend">Numero complesso da dividere.</param>
    /// <param name="divisor">Numero complesso per cui dividere.</param>
    
    public static Complex Divide(Complex dividend, Complex divisor)
    {
      return dividend / divisor;
    }

    /// <summary>Ottiene il valore assoluto (o grandezza) di un numero complesso.</summary>
    /// <returns>Valore assoluto di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static double Abs(Complex value)
    {
      if (double.IsInfinity(value.m_real) || double.IsInfinity(value.m_imaginary))
        return double.PositiveInfinity;
      double num1 = Math.Abs(value.m_real);
      double num2 = Math.Abs(value.m_imaginary);
      if (num1 > num2)
      {
        double num3 = num2 / num1;
        return num1 * Math.Sqrt(1.0 + num3 * num3);
      }
      if (num2 == 0.0)
        return num1;
      double num4 = num1 / num2;
      return num2 * Math.Sqrt(1.0 + num4 * num4);
    }

    /// <summary>Calcola il coniugato di un numero complesso e restituisce il risultato.</summary>
    /// <returns>Coniugato di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Conjugate(Complex value)
    {
      return new Complex(value.m_real, -value.m_imaginary);
    }

    /// <summary>Restituisce il reciproco moltiplicativo di un numero complesso.</summary>
    /// <returns>Reciproco di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Reciprocal(Complex value)
    {
      if (value.m_real == 0.0 && value.m_imaginary == 0.0)
        return Complex.Zero;
      return Complex.One / value;
    }

    /// <summary>Restituisce un valore che indica se l'istanza corrente e un oggetto specificato hanno lo stesso valore. </summary>
    /// <returns>true se il parametro <paramref name="obj" /> è un oggetto <see cref="T:System.Numerics.Complex" /> o un tipo in grado di eseguire la conversione implicita in un oggetto <see cref="T:System.Numerics.Complex" /> e il relativo valore è uguale all'oggetto <see cref="T:System.Numerics.Complex" /> corrente. In caso contrario, false.</returns>
    /// <param name="obj">Oggetto da confrontare.</param>
    
    public override bool Equals(object obj)
    {
      if (!(obj is Complex))
        return false;
      return this == (Complex) obj;
    }

    /// <summary>Restituisce un valore che indica se l'istanza corrente e un numero complesso specificato hanno lo stesso valore.</summary>
    /// <returns>true se questo numero complesso e <paramref name="value" /> presentano lo stesso valore. In caso contrario, false.</returns>
    /// <param name="value">Numero complesso da confrontare.</param>
    
    public bool Equals(Complex value)
    {
      if (this.m_real.Equals(value.m_real))
        return this.m_imaginary.Equals(value.m_imaginary);
      return false;
    }

    /// <summary>Converte il valore del numero complesso corrente nella relativa rappresentazione di stringa equivalente in formato cartesiano.</summary>
    /// <returns>Rappresentazione di stringa dell'istanza corrente in formato cartesiano.</returns>
    
    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "({0}, {1})", new object[2]
      {
        (object) this.m_real,
        (object) this.m_imaginary
      });
    }

    /// <summary>Converte il valore del numero complesso corrente nella relativa rappresentazione di stringa equivalente in formato cartesiano usando il formato specificato per le parti reale e immaginaria.</summary>
    /// <returns>Rappresentazione di stringa dell'istanza corrente in formato cartesiano.</returns>
    /// <param name="format">Stringa di formato numerico standard o personalizzato.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> non è una stringa in formato valido.</exception>
    
    public string ToString(string format)
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "({0}, {1})", new object[2]
      {
        (object) this.m_real.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture),
        (object) this.m_imaginary.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture)
      });
    }

    /// <summary>Converte il valore del numero complesso corrente nella relativa rappresentazione di stringa equivalente in formato cartesiano usando le informazioni di formattazione relative alle impostazioni cultura specificate.</summary>
    /// <returns>Rappresentazione di stringa dell'istanza corrente in formato cartesiano, come specificato da <paramref name="provider" />.</returns>
    /// <param name="provider">Oggetto che fornisce informazioni di formattazione specifiche delle impostazioni cultura.</param>
    
    public string ToString(IFormatProvider provider)
    {
      return string.Format(provider, "({0}, {1})", new object[2]
      {
        (object) this.m_real,
        (object) this.m_imaginary
      });
    }

    /// <summary>Converte il valore del numero complesso corrente nella relativa rappresentazione di stringa equivalente in formato cartesiano usando il formato specificato e le informazioni sul formato relative alle impostazioni cultura per le parti reale e immaginaria.</summary>
    /// <returns>Rappresentazione di stringa dell'istanza corrente in formato cartesiano, come specificato da <paramref name="format" /> e da <paramref name="provider" />.</returns>
    /// <param name="format">Stringa di formato numerico standard o personalizzato.</param>
    /// <param name="provider">Oggetto che fornisce informazioni di formattazione specifiche delle impostazioni cultura.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> non è una stringa in formato valido.</exception>
    
    public string ToString(string format, IFormatProvider provider)
    {
      return string.Format(provider, "({0}, {1})", new object[2]
      {
        (object) this.m_real.ToString(format, provider),
        (object) this.m_imaginary.ToString(format, provider)
      });
    }

    /// <summary>Restituisce il codice hash per l'oggetto <see cref="T:System.Numerics.Complex" /> corrente.</summary>
    /// <returns>Codice hash di un intero con segno a 32 bit.</returns>
    
    public override int GetHashCode()
    {
      return this.m_real.GetHashCode() % 99999997 ^ this.m_imaginary.GetHashCode();
    }

    /// <summary>Restituisce il seno del numero complesso specificato.</summary>
    /// <returns>Seno di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Sin(Complex value)
    {
      double real = value.m_real;
      double imaginary = value.m_imaginary;
      return new Complex(Math.Sin(real) * Math.Cosh(imaginary), Math.Cos(real) * Math.Sinh(imaginary));
    }

    /// <summary>Restituisce il seno iperbolico del numero complesso specificato.</summary>
    /// <returns>Seno iperbolico di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Sinh(Complex value)
    {
      double real = value.m_real;
      double imaginary = value.m_imaginary;
      return new Complex(Math.Sinh(real) * Math.Cos(imaginary), Math.Cosh(real) * Math.Sin(imaginary));
    }

    /// <summary>Restituisce l'angolo che costituisce l'arcoseno del numero complesso specificato.</summary>
    /// <returns>Angolo che costituisce l'arcoseno di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Asin(Complex value)
    {
      return -Complex.ImaginaryOne * Complex.Log(Complex.ImaginaryOne * value + Complex.Sqrt(Complex.One - value * value));
    }

    /// <summary>Restituisce il coseno del numero complesso specificato.</summary>
    /// <returns>Coseno di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Cos(Complex value)
    {
      double real = value.m_real;
      double imaginary = value.m_imaginary;
      return new Complex(Math.Cos(real) * Math.Cosh(imaginary), -(Math.Sin(real) * Math.Sinh(imaginary)));
    }

    /// <summary>Restituisce il coseno iperbolico del numero complesso specificato.</summary>
    /// <returns>Coseno iperbolico di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Cosh(Complex value)
    {
      double real = value.m_real;
      double imaginary = value.m_imaginary;
      return new Complex(Math.Cosh(real) * Math.Cos(imaginary), Math.Sinh(real) * Math.Sin(imaginary));
    }

    /// <summary>Restituisce l'angolo che costituisce l'arcocoseno del numero complesso specificato.</summary>
    /// <returns>Angolo espresso in radianti che costituisce l'arcocoseno di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso che rappresenta un coseno.</param>
    
    public static Complex Acos(Complex value)
    {
      return -Complex.ImaginaryOne * Complex.Log(value + Complex.ImaginaryOne * Complex.Sqrt(Complex.One - value * value));
    }

    /// <summary>Restituisce la tangente del numero complesso specificato.</summary>
    /// <returns>Tangente di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Tan(Complex value)
    {
      return Complex.Sin(value) / Complex.Cos(value);
    }

    /// <summary>Restituisce la tangente iperbolica del numero complesso specificato.</summary>
    /// <returns>Tangente iperbolica di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Tanh(Complex value)
    {
      return Complex.Sinh(value) / Complex.Cosh(value);
    }

    /// <summary>Restituisce l'angolo che costituisce l'arcotangente del numero complesso specificato.</summary>
    /// <returns>Angolo che costituisce l'arcotangente di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Atan(Complex value)
    {
      Complex complex = new Complex(2.0, 0.0);
      return Complex.ImaginaryOne / complex * (Complex.Log(Complex.One - Complex.ImaginaryOne * value) - Complex.Log(Complex.One + Complex.ImaginaryOne * value));
    }

    /// <summary>Restituisce e, la base del logaritmo naturale del numero complesso specificato.</summary>
    /// <returns>Logaritmo naturale (base e) di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Log(Complex value)
    {
      return new Complex(Math.Log(Complex.Abs(value)), Math.Atan2(value.m_imaginary, value.m_real));
    }

    /// <summary>Restituisce il logaritmo del numero complesso specificato nella base specificata.</summary>
    /// <returns>Logaritmo di <paramref name="value" /> in base <paramref name="baseValue" />.</returns>
    /// <param name="value">Numero complesso.</param>
    /// <param name="baseValue">Base del logaritmo.</param>
    
    public static Complex Log(Complex value, double baseValue)
    {
      return Complex.Log(value) / Complex.Log((Complex) baseValue);
    }

    /// <summary>Restituisce il logaritmo in base 10 del numero complesso specificato.</summary>
    /// <returns>Logaritmo in base 10 di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Log10(Complex value)
    {
      return Complex.Scale(Complex.Log(value), 0.43429448190325);
    }

    /// <summary>Restituisce e elevato alla potenza specificata da un numero complesso.</summary>
    /// <returns>Numero e elevato alla potenza <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso che specifica una potenza.</param>
    
    public static Complex Exp(Complex value)
    {
      double num = Math.Exp(value.m_real);
      return new Complex(num * Math.Cos(value.m_imaginary), num * Math.Sin(value.m_imaginary));
    }

    /// <summary>Restituisce la radice quadrata del numero complesso specificato.</summary>
    /// <returns>Radice quadrata di <paramref name="value" />.</returns>
    /// <param name="value">Numero complesso.</param>
    
    public static Complex Sqrt(Complex value)
    {
      return Complex.FromPolarCoordinates(Math.Sqrt(value.Magnitude), value.Phase / 2.0);
    }

    /// <summary>Restituisce un numero complesso specificato elevato a una potenza specificata da un numero complesso.</summary>
    /// <returns>Numero complesso <paramref name="value" /> elevato alla potenza <paramref name="power" />.</returns>
    /// <param name="value">Numero complesso da elevare a una potenza.</param>
    /// <param name="power">Numero complesso che specifica una potenza.</param>
    
    public static Complex Pow(Complex value, Complex power)
    {
      if (power == Complex.Zero)
        return Complex.One;
      if (value == Complex.Zero)
        return Complex.Zero;
      double real1 = value.m_real;
      double imaginary1 = value.m_imaginary;
      double real2 = power.m_real;
      double imaginary2 = power.m_imaginary;
      double num1 = Complex.Abs(value);
      double num2 = Math.Atan2(imaginary1, real1);
      double num3 = real2 * num2 + imaginary2 * Math.Log(num1);
      double num4 = Math.Pow(num1, real2) * Math.Pow(Math.E, -imaginary2 * num2);
      return new Complex(num4 * Math.Cos(num3), num4 * Math.Sin(num3));
    }

    /// <summary>Restituisce un numero complesso specificato elevato a una potenza specificata da un numero a virgola mobile a precisione doppia.</summary>
    /// <returns>Numero complesso <paramref name="value" /> elevato alla potenza <paramref name="power" />.</returns>
    /// <param name="value">Numero complesso da elevare a una potenza.</param>
    /// <param name="power">Numero a virgola mobile a precisione doppia che specifica una potenza.</param>
    
    public static Complex Pow(Complex value, double power)
    {
      return Complex.Pow(value, new Complex(power, 0.0));
    }

    private static Complex Scale(Complex value, double factor)
    {
      return new Complex(factor * value.m_real, factor * value.m_imaginary);
    }
  }
}
