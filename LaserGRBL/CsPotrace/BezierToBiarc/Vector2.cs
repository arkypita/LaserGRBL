
using System;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Text;

namespace CsPotrace.BezierToBiarc
{
  public struct Vector2 : IEquatable<Vector2>, IFormattable
  {
    /// <summary>Componente X del vettore. </summary>
    
    public float X;
    /// <summary>Componente Y del vettore. </summary>
    
    public float Y;

    /// <summary>Restituisce un vettore i cui 2 elementi sono uguali a zero. </summary>
    /// <returns>Vettore i cui due elementi sono uguali a zero (ovvero, restituisce il vettore (0,0)). </returns>
    
    public static Vector2 Zero
    {
       get
      {
        return new Vector2();
      }
    }

    /// <summary>Ottiene un vettore i cui 2 elementi sono uguali a uno. </summary>
    /// <returns>Vettore i cui due elementi sono uguali a uno (ovvero, restituisce il vettore (1,1)).</returns>
    
    public static Vector2 One
    {
       get
      {
        return new Vector2(1f, 1f);
      }
    }

    /// <summary>Ottiene il vettore (1,0). </summary>
    /// <returns>Vettore (1,0). </returns>
    
    public static Vector2 UnitX
    {
       get
      {
        return new Vector2(1f, 0.0f);
      }
    }

    /// <summary>Ottiene il vettore (0,1).</summary>
    /// <returns>Vettore (0,1).</returns>
    
    public static Vector2 UnitY
    {
       get
      {
        return new Vector2(0.0f, 1f);
      }
    }

    /// <summary>Crea un nuovo oggetto <see cref="T:System.Numerics.Vector2" /> i cui due elementi hanno lo stesso valore.</summary>
    /// <param name="value">Valore da assegnare a entrambi gli elementi. </param>
    
    
    public Vector2(float value)
    {
      this = new Vector2(value, value);
    }

    /// <summary>Crea un vettore i cui elementi hanno i valori specificati. </summary>
    /// <param name="x">Valore da assegnare al campo <see cref="F:System.Numerics.Vector2.X" />. </param>
    /// <param name="y">Valore da assegnare al campo <see cref="F:System.Numerics.Vector2.Y" />. </param>
    
    
    public Vector2(float x, float y)
    {
      this.X = x;
      this.Y = y;
    }

    /// <summary>Somma due vettori. </summary>
    /// <returns>Vettore sommato. </returns>
    /// <param name="left">Primo vettore da sommare. </param>
    /// <param name="right">Secondo vettore da sommare. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X + right.X, left.Y + right.Y);
    }

    /// <summary>Sottrae il secondo vettore dal primo. </summary>
    /// <returns>Vettore risultante dalla sottrazione di <paramref name="right" /> da <paramref name="left" />. </returns>
    /// <param name="left">Primo vettore. </param>
    /// <param name="right">Secondo vettore. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X - right.X, left.Y - right.Y);
    }

    /// <summary>Moltiplica due vettori. </summary>
    /// <returns>Vettore prodotto. </returns>
    /// <param name="left">Primo vettore. </param>
    /// <param name="right">Secondo vettore. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X * right.X, left.Y * right.Y);
    }

    /// <summary>Moltiplica il valore scalare per il vettore specificato. </summary>
    /// <returns>Vettore scalato. </returns>
    /// <param name="left">Vettore. </param>
    /// <param name="right">Valore scalare. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(float left, Vector2 right)
    {
      return new Vector2(left, left) * right;
    }

    /// <summary>Moltiplica il vettore specificato per il valore scalare specificato. </summary>
    /// <returns>Vettore scalato. </returns>
    /// <param name="left">Vettore. </param>
    /// <param name="right">Valore scalare. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 left, float right)
    {
      return left * new Vector2(right, right);
    }

    /// <summary>Divide il primo vettore per il secondo. </summary>
    /// <returns>Vettore risultante dalla divisione di <paramref name="left" /> per <paramref name="right" />. </returns>
    /// <param name="left">Primo vettore. </param>
    /// <param name="right">Secondo vettore. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 left, Vector2 right)
    {
      return new Vector2(left.X / right.X, left.Y / right.Y);
    }

    /// <summary>Divide il vettore specificato per un valore scalare specificato.</summary>
    /// <returns>Risultato della divisione. </returns>
    /// <param name="value1">Vettore. </param>
    /// <param name="value2">Valore scalare. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 value1, float value2)
    {
      float num = 1f / value2;
      return new Vector2(value1.X * num, value1.Y * num);
    }

    /// <summary>Nega il vettore specificato. </summary>
    /// <returns>Vettore negato. </returns>
    /// <param name="value">Vettore da negare. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 value)
    {
      return Vector2.Zero - value;
    }

    /// <summary>Restituisce un valore che indica se le coppie di elementi in due vettori specificati sono uguali.  </summary>
    /// <returns>true se <paramref name="left" /> e <paramref name="right" /> sono uguali; in caso contrario, false.</returns>
    /// <param name="left">Primo vettore da confrontare. </param>
    /// <param name="right">Secondo vettore da confrontare. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector2 left, Vector2 right)
    {
      return left.Equals(right);
    }

    /// <summary>Restituisce un valore che indica se due vettori specificati non sono uguali.  </summary>
    /// <returns>true se <paramref name="left" /> e <paramref name="right" /> non sono uguali; in caso contrario, false. </returns>
    /// <param name="left">Primo vettore da confrontare. </param>
    /// <param name="right">Secondo vettore da confrontare. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Vector2 left, Vector2 right)
    {
      return !(left == right);
    }

    /// <summary>Restituisce il codice hash per l'istanza. </summary>
    /// <returns>Codice hash. </returns>
    
    public override int GetHashCode()
    {
      return this.X.GetHashCode() ^ this.Y.GetHashCode();
    }

    /// <summary>Restituisce un valore che indica se questa istanza è uguale a un oggetto specificato.</summary>
    /// <returns>true se l'istanza corrente è uguale a <paramref name="obj" />; in caso contrario, false. Se <paramref name="obj" /> è null, il metodo restituisce false. </returns>
    /// <param name="obj">Oggetto da confrontare con l'istanza corrente. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object obj)
    {
      if (!(obj is Vector2))
        return false;
      return this.Equals((Vector2) obj);
    }

    /// <summary>Restituisce la rappresentazione di stringa dell'istanza corrente usando la formattazione predefinita. </summary>
    /// <returns>Rappresentazione di stringa dell'istanza corrente. </returns>
    
    public override string ToString()
    {
      return this.ToString("G", (IFormatProvider) CultureInfo.CurrentCulture);
    }

    /// <summary>Restituisce la rappresentazione di stringa dell'istanza corrente usando la stringa di formato specificata per formattare i singoli elementi. </summary>
    /// <returns>Rappresentazione di stringa dell'istanza corrente. </returns>
    /// <param name="format">Stringa di formato standard o numerico personalizzato che definisce il formato dei singoli elementi.</param>
    
    public string ToString(string format)
    {
      return this.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    /// <summary>Restituisce la rappresentazione di stringa dell'istanza corrente usando la stringa di formato specificata per formattare i singoli elementi e il provider di formato specificato per definire la formattazione specifica delle impostazioni cultura.</summary>
    /// <returns>Rappresentazione di stringa dell'istanza corrente. </returns>
    /// <param name="format">Stringa di formato standard o numerico personalizzato che definisce il formato dei singoli elementi. </param>
    /// <param name="formatProvider">Provider di formato che fornisce informazioni di formattazione specifiche delle impostazioni cultura. </param>
    
    public string ToString(string format, IFormatProvider formatProvider)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
      stringBuilder.Append('<');
      stringBuilder.Append(this.X.ToString(format, formatProvider));
      stringBuilder.Append(numberGroupSeparator);
      stringBuilder.Append(' ');
      stringBuilder.Append(this.Y.ToString(format, formatProvider));
      stringBuilder.Append('>');
      return stringBuilder.ToString();
    }

    /// <summary>Restituisce la lunghezza del vettore. </summary>
    /// <returns>Lunghezza del vettore. </returns>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Length()
    {
      return (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
    }

    /// <summary>Restituisce la lunghezza del vettore al quadrato. </summary>
    /// <returns>Lunghezza al quadrato del vettore. </returns>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float LengthSquared()
    {
      return (float) ((double) this.X * (double) this.X + (double) this.Y * (double) this.Y);
    }

    /// <summary>Calcola la distanza euclidea tra due punti specificati. </summary>
    /// <returns>Distanza. </returns>
    /// <param name="value1">Primo punto. </param>
    /// <param name="value2">Secondo punto. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector2 value1, Vector2 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    /// <summary>Restituisce la distanza euclidea quadratica tra due punti specificati. </summary>
    /// <returns>Distanza quadratica. </returns>
    /// <param name="value1">Primo punto. </param>
    /// <param name="value2">Secondo punto. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquared(Vector2 value1, Vector2 value2)
    {
      float num1 = value1.X - value2.X;
      float num2 = value1.Y - value2.Y;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    /// <summary>Restituisce un vettore con la stessa direzione del vettore specificato, ma con una lunghezza di uno. </summary>
    /// <returns>Vettore normalizzato. </returns>
    /// <param name="value">Vettore da normalizzare. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Normalize(Vector2 value)
    {
      float num1 = 1f / (float) Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y);
      return new Vector2(value.X * num1, value.Y * num1);
    }

    /// <summary>Restituisce la reflection di un vettore da una superficie con la normale specificata. </summary>
    /// <returns>Vettore riflesso. </returns>
    /// <param name="vector">Vettore di origine. </param>
    /// <param name="normal">Normale della superficie riflessa. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Reflect(Vector2 vector, Vector2 normal)
    {
      float num1 = (float) ((double) vector.X * (double) normal.X + (double) vector.Y * (double) normal.Y);
      return new Vector2(vector.X - 2f * num1 * normal.X, vector.Y - 2f * num1 * normal.Y);
    }

    /// <summary>Limita un vettore tra un valore minimo e un valore massimo. </summary>
    /// <returns>Vettore limitato. </returns>
    /// <param name="value1">Vettore da limitare. </param>
    /// <param name="min">Valore minimo. </param>
    /// <param name="max">Valore massimo. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
    {
      float x1 = value1.X;
      float num1 = (double) x1 > (double) max.X ? max.X : x1;
      float x2 = (double) num1 < (double) min.X ? min.X : num1;
      float y1 = value1.Y;
      float num2 = (double) y1 > (double) max.Y ? max.Y : y1;
      float y2 = (double) num2 < (double) min.Y ? min.Y : num2;
      return new Vector2(x2, y2);
    }

    /// <summary>Esegue un'interpolazione lineare tra due vettori in base al peso specificato. </summary>
    /// <returns>Vettore interpolato. </returns>
    /// <param name="value1">Primo vettore. </param>
    /// <param name="value2">Secondo vettore. </param>
    /// <param name="amount">Valore compreso tra 0 e 1 che indica il peso di <paramref name="value2" />. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
    {
      return new Vector2(value1.X + (value2.X - value1.X) * amount, value1.Y + (value2.Y - value1.Y) * amount);
    }

//    /// <summary>Trasforma un vettore in base a una matrice 3x2 specificata. </summary>
//    /// <returns>Vettore trasformato. </returns>
//    /// <param name="position">Vettore da trasformare. </param>
//    /// <param name="matrix">Matrice di trasformazione. </param>
//    
//    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static Vector2 Transform(Vector2 position, Matrix3x2 matrix)
//    {
//      return new Vector2((float) ((double) position.X * (double) matrix.M11 + (double) position.Y * (double) matrix.M21) + matrix.M31, (float) ((double) position.X * (double) matrix.M12 + (double) position.Y * (double) matrix.M22) + matrix.M32);
//    }

//    /// <summary>Trasforma un vettore in base a una matrice 4x4 specificata. </summary>
//    /// <returns>Vettore trasformato. </returns>
//    /// <param name="position">Vettore da trasformare. </param>
//    /// <param name="matrix">Matrice di trasformazione. </param>
//    
//    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static Vector2 Transform(Vector2 position, Matrix4x4 matrix)
//    {
//      return new Vector2((float) ((double) position.X * (double) matrix.M11 + (double) position.Y * (double) matrix.M21) + matrix.M41, (float) ((double) position.X * (double) matrix.M12 + (double) position.Y * (double) matrix.M22) + matrix.M42);
//    }

//    /// <summary>Trasforma la normale di un vettore in base alla matrice 3x2 specificata. </summary>
//    /// <returns>Vettore trasformato. </returns>
//    /// <param name="normal">Vettore di origine. </param>
//    /// <param name="matrix">Matrice. </param>
//    
//    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static Vector2 TransformNormal(Vector2 normal, Matrix3x2 matrix)
//    {
//      return new Vector2((float) ((double) normal.X * (double) matrix.M11 + (double) normal.Y * (double) matrix.M21), (float) ((double) normal.X * (double) matrix.M12 + (double) normal.Y * (double) matrix.M22));
//    }

//    /// <summary>Trasforma la normale di un vettore in base alla matrice 4x4 specificata. </summary>
//    /// <returns>Vettore trasformato. </returns>
//    /// <param name="normal">Vettore di origine. </param>
//    /// <param name="matrix">Matrice. </param>
//    
//    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static Vector2 TransformNormal(Vector2 normal, Matrix4x4 matrix)
//    {
//      return new Vector2((float) ((double) normal.X * (double) matrix.M11 + (double) normal.Y * (double) matrix.M21), (float) ((double) normal.X * (double) matrix.M12 + (double) normal.Y * (double) matrix.M22));
//    }

    /// <summary>Trasforma un vettore in base al valore di rotazione Quaternion specificato. </summary>
    /// <returns>Vettore trasformato. </returns>
    /// <param name="value">Vettore da ruotare. </param>
    /// <param name="rotation">Rotazione da applicare. </param>
    
//    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static Vector2 Transform(Vector2 value, Quaternion rotation)
//    {
//      float num1 = rotation.X + rotation.X;
//      float num2 = rotation.Y + rotation.Y;
//      float num3 = rotation.Z + rotation.Z;
//      float num4 = rotation.W * num3;
//      float num5 = rotation.X * num1;
//      float num6 = rotation.X * num2;
//      float num7 = rotation.Y * num2;
//      float num8 = rotation.Z * num3;
//      return new Vector2((float) ((double) value.X * (1.0 - (double) num7 - (double) num8) + (double) value.Y * ((double) num6 - (double) num4)), (float) ((double) value.X * ((double) num6 + (double) num4) + (double) value.Y * (1.0 - (double) num5 - (double) num8)));
//    }

    /// <summary>Somma due vettori. </summary>
    /// <returns>Vettore sommato. </returns>
    /// <param name="left">Primo vettore da sommare. </param>
    /// <param name="right">Secondo vettore da sommare. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Add(Vector2 left, Vector2 right)
    {
      return left + right;
    }

    /// <summary>Sottrae il secondo vettore dal primo. </summary>
    /// <returns>Vettore differenza. </returns>
    /// <param name="left">Primo vettore. </param>
    /// <param name="right">Secondo vettore. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Subtract(Vector2 left, Vector2 right)
    {
      return left - right;
    }

    /// <summary>Moltiplica due vettori. </summary>
    /// <returns>Vettore prodotto. </returns>
    /// <param name="left">Primo vettore. </param>
    /// <param name="right">Secondo vettore. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Multiply(Vector2 left, Vector2 right)
    {
      return left * right;
    }

    /// <summary>Moltiplica un vettore per un valore scalare specificato. </summary>
    /// <returns>Vettore scalato. </returns>
    /// <param name="left">Vettore da moltiplicare. </param>
    /// <param name="right">Valore scalare. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Multiply(Vector2 left, float right)
    {
      return left * right;
    }

    /// <summary>Moltiplica un valore scalare per un vettore specificato.</summary>
    /// <returns>Vettore scalato. </returns>
    /// <param name="left">Valore scalato. </param>
    /// <param name="right">Vettore. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Multiply(float left, Vector2 right)
    {
      return left * right;
    }

    /// <summary>Divide il primo vettore per il secondo. </summary>
    /// <returns>Vettore risultante dalla divisione. </returns>
    /// <param name="left">Primo vettore. </param>
    /// <param name="right">Secondo vettore. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Divide(Vector2 left, Vector2 right)
    {
      return left / right;
    }

    /// <summary>Divide il vettore specificato per un valore scalare specificato. </summary>
    /// <returns>Vettore risultante dalla divisione. </returns>
    /// <param name="left">Vettore. </param>
    /// <param name="divisor">Valore scalare. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Divide(Vector2 left, float divisor)
    {
      return left / divisor;
    }

    /// <summary>Nega un vettore specificato. </summary>
    /// <returns>Vettore negato. </returns>
    /// <param name="value">Vettore da negare. </param>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Negate(Vector2 value)
    {
      return -value;
    }

    /// <summary>Copia gli elementi del vettore nella matrice specificata. </summary>
    /// <param name="array">Matrice di destinazione. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> è null. </exception>
    /// <exception cref="T:System.ArgumentException">Il numero di elementi nell'istanza corrente è maggiore della matrice. </exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> è multidimensionale.</exception>
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(float[] array)
    {
      this.CopyTo(array, 0);
    }

    /// <summary>Copia gli elementi del vettore nella matrice specificata, partendo dalla posizione dell'indice specificata.</summary>
    /// <param name="array">Matrice di destinazione.</param>
    /// <param name="index">Indice in cui copiare il primo elemento del vettore. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> è null. </exception>
    /// <exception cref="T:System.ArgumentException">Il numero di elementi nell'istanza corrente è maggiore della matrice. </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> è minore di zero.-oppure-<paramref name="index" /> è maggiore o uguale alla lunghezza della matrice. </exception>
    /// <exception cref="T:System.RankException">
    /// <paramref name="array" /> è multidimensionale.</exception>
    
    public void CopyTo(float[] array, int index)
    {
      if (array == null)
        throw new NullReferenceException("Arg_NullArgumentNullRef");
      if (index < 0 || index >= array.Length)
        throw new ArgumentOutOfRangeException("Arg_ArgumentOutOfRangeException");
      if (array.Length - index < 2)
        throw new ArgumentException("Arg_ElementsInSourceIsGreaterThanDestination");
      array[index] = this.X;
      array[index + 1] = this.Y;
    }

    /// <summary>Restituisce un valore che indica se questa istanza è uguale a un altro vettore. </summary>
    /// <returns>true se i due vettori sono uguali; in caso contrario, false. </returns>
    /// <param name="other">L'altro vettore. </param>
    
    
    public bool Equals(Vector2 other)
    {
      if ((double) this.X == (double) other.X)
        return (double) this.Y == (double) other.Y;
      return false;
    }

    /// <summary>Restituisce il prodotto scalare di due vettori. </summary>
    /// <returns>Prodotto scalare. </returns>
    /// <param name="value1">Primo vettore. </param>
    /// <param name="value2">Secondo vettore. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(Vector2 value1, Vector2 value2)
    {
      return (float) ((double) value1.X * (double) value2.X + (double) value1.Y * (double) value2.Y);
    }

    /// <summary>Restituisce un vettore che contiene il valore più basso da ognuna delle coppie di elementi nei due vettori specificati.</summary>
    /// <returns>Vettore minimizzato. </returns>
    /// <param name="value1">Primo vettore. </param>
    /// <param name="value2">Secondo vettore. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Min(Vector2 value1, Vector2 value2)
    {
      return new Vector2((double) value1.X < (double) value2.X ? value1.X : value2.X, (double) value1.Y < (double) value2.Y ? value1.Y : value2.Y);
    }

    /// <summary>Restituisce un vettore che contiene il valore più alto da ognuna delle coppie di elementi nei due vettori specificati.</summary>
    /// <returns>Vettore massimizzato. </returns>
    /// <param name="value1">Primo vettore. </param>
    /// <param name="value2">Secondo vettore. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Max(Vector2 value1, Vector2 value2)
    {
      return new Vector2((double) value1.X > (double) value2.X ? value1.X : value2.X, (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y);
    }

    /// <summary>Restituisce un vettore i cui elementi sono i valori assoluti di ognuno degli elementi del vettore specificato. </summary>
    /// <returns>Valore assoluto del vettore. </returns>
    /// <param name="value">Vettore. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Abs(Vector2 value)
    {
      return new Vector2(Math.Abs(value.X), Math.Abs(value.Y));
    }

    /// <summary>Restituisce un vettore i cui elementi sono la radice quadrata di ognuno degli elementi del vettore specificato.</summary>
    /// <returns>Vettore radice quadrata. </returns>
    /// <param name="value">Vettore. </param>
    
    
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 SquareRoot(Vector2 value)
    {
      return new Vector2((float) Math.Sqrt((double) value.X), (float) Math.Sqrt((double) value.Y));
    }
  }
}

