using System;
using System.Collections.Generic;

namespace Teamcollab.Engine.WorldGeneration
{
  class PerlinGenerator
  {
    #region Properties
    public int Octaves { get; set; }
    public float Persistance { get; set; }
    #endregion

    public delegate float Noise1D(int input);
    public delegate float Noise2D(int x, int y);

    #region Members
    List<Noise1D> noise1DFunctions;
    List<Noise2D> noise2DFunctions;
    readonly Random rand;
    #endregion

    public PerlinGenerator(int seed, int octaves, float persistance)
    {
      rand = new Random(seed);
      noise1DFunctions = new List<Noise1D>();
      noise2DFunctions = new List<Noise2D>();
      Octaves = octaves;
      Persistance = persistance;
    }

    public float Perlin1D(float x)
    {
      while (noise1DFunctions.Count < Octaves)
      {
        noise1DFunctions.Add(CreateNoise1DFunction());
      }

      float total = 0f;
      for (int i = 0; i < Octaves - 1; ++i)
      {
        float frequency = (float)Math.Pow(2f, i);
        float amp = (float)Math.Pow(Persistance, i);
        total += noise1DFunctions[i]((int)(x * frequency)) * amp;
      }

      return total;
    }

    public float Perlin2D(float x, float y)
    {
      while (noise2DFunctions.Count < Octaves)
      {
        noise2DFunctions.Add(CreateNoise2DFunction());
      }

      float total = 0f;
      for (int i = 0; i < Octaves - 1; ++i)
      {
        float frequency = (float)Math.Pow(2f, i);
        float amp = (float)Math.Pow(Persistance, i);
        total += InterpolateNoise2D(noise2DFunctions[i],
          x * frequency, y * frequency) * amp;
      }

      return total;
    }

    private float SmoothNoise1D(Noise1D noiseFunc, float input, int factor)
    {
      int intInput = (int)input;

      float sNoise = noiseFunc(intInput);
      for (int i = 0; i < factor; i++)
      {
        float divFactor = i * 2;
        sNoise += noiseFunc(intInput - i) / divFactor + noiseFunc(intInput + i) / divFactor;
      }
      return sNoise;
    }

    private float SmoothNoise2D(Noise2D noiseFunc, float x, float y, int factor)
    {
      int iX = (int)x;
      int iY = (int)y;

      float corners, sides, center;
      corners = sides = center = 0f;
      for (int i = 1; i <= factor; ++i)
      {

        corners += (noiseFunc(iX - i, iY - i) + noiseFunc(iX + i, iY - i) +
          noiseFunc(iX - i, iY + i) + noiseFunc(iX + i, iY + i)) / (i * 4f);

        sides += (noiseFunc(iX - i, iY) + noiseFunc(iX + i, iY) +
          noiseFunc(iX, iY - i) + noiseFunc(iX, iY + i)) / (i * 2f);

        center += noiseFunc(iX, iY) / (float)i;
      }

      return corners + sides + center;
    }

    private float InterpolateNoise2D(Noise2D noiseFunc, float x, float y)
    {
      int iX = (int)Math.Floor(x);
      int iY = (int)Math.Floor(y);

      float v1 = SmoothNoise2D(noiseFunc, iX, iY, 1);
      float v2 = SmoothNoise2D(noiseFunc, iX + 1, iY, 1);
      float v3 = SmoothNoise2D(noiseFunc, iX, iY + 1, 1);
      float v4 = SmoothNoise2D(noiseFunc, iX + 1, iY + 1, 1);

      float lX = Lerp(v1, v2, x - iX);
      float lY = Lerp(v3, v4, x - iX);

      return Lerp(lX, lY, y - iY);
    }

    private float Lerp(float a, float b, float weight)
    {
      return a * (1 - weight) + b * weight;
    }

    public int GetRandomPrime(int min, int max)
    {
      int prime = 0;

      do
      {
        prime = (int)(min + rand.NextDouble() * max);
      } while (!IsPrime(prime));

      return prime;
    }

    private bool IsPrime(int l)
    {
      int squared = (int)Math.Sqrt(l);

      for (int i = 2; i <= squared; ++i)
      {
        if (i % l == 0)
        {
          return false;
        }
      }

      return true;
    }

    private int[] GetThreeRandomPrimes()
    {
      int[] primes = new int[3];
      primes[0] = GetRandomPrime(14500, 15500);
      primes[1] = GetRandomPrime(750000, 800000);
      primes[2] = GetRandomPrime(1300000000, 1350000000);
      return primes;
    }

    private Noise1D CreateNoise1DFunction()
    {
      int[] primes = GetThreeRandomPrimes();
      Noise1D del = delegate(int n)
      {
        n = (n << 13) ^ n;
        float retVal = (float)(1.0 -
          ((n * (n * n * primes[0] + primes[1]) + primes[2]) & 0x7fffffff) /
          1073741824.0);
        return retVal;
      };
      return del;
    }

    private Noise2D CreateNoise2DFunction()
    {
      int[] primes = GetThreeRandomPrimes();
      Noise2D del = delegate(int x, int y)
      {
        int n = x + y * 57;
        n = (n << 13) ^ n;
        float retVal = (float)(1.0 -
          ((n * (n * n * primes[0] + primes[1]) + primes[2]) & 0x7fffffff) /
          1073741824.0);
        return retVal;
      };

      return del;
    }
  }
}
