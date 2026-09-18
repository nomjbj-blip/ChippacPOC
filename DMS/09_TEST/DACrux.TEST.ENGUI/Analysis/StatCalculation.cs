using System;
using System.Data;
using System.Collections;

namespace DACrux.TEST.ENGUI
{
	/// <summary>
	/// Stat에 대한 요약 설명입니다.
	/// </summary>
	public class Stat
	{

		public static int Missing(double[] data1)
		{
			if(data1.Length == 0)
				return -1;

			int count = 0;
			for(int i =0; i<data1.Length; i++)
			{
				if(data1[i].Equals( null) || data1[i].Equals(double.NaN) )
					count++;
			}

			return count;
		}

		public static double Sum(double[] data1)
		{
			if(data1.Length == 0)
				return double.NaN;

			double sum = 0;

			for(int i =0; i<data1.Length; i++)
			{
				if(data1[i].Equals( null) || data1[i].Equals(double.NaN) )
					continue;
		
				sum += data1[i];
			}

			return sum;

		}
	
		public static double Min(double[] data1)
		{
			if(data1.Length == 0)
				return double.NaN;

			double min = double.NaN;
			for(int i=0; i<data1.Length; i++)
			{
				if(!double.IsNaN(data1[i]))
				{
					min = data1[i];
					break;
				}
			}

			for(int i=1; i<data1.Length; i++)
			{
				if(data1[i].Equals(null) || data1[i].Equals(double.NaN))
					continue;

				if(data1[i] < min)
					min = data1[i];
			}

			return min;
		}
	
		public static double Max(double[] data1)
		{
			if(data1.Length == 0)
				return double.NaN;

			double max = double.NaN;

			for(int i=0; i<data1.Length; i++)
			{
				if(!double.IsNaN(data1[i]))
				{
					max = data1[i];
					break;
				}
			}

			for(int i=1; i<data1.Length; i++)
			{
				if(data1[i].Equals(null) || data1[i].Equals(double.NaN))
					continue;

				if(data1[i] > max)
					max = data1[i];
			}

			return max;
		}

		/**
			* 상관분석
			* 
			* 
			* */

		public static double Correlation(double[] data1, double[] data2)
		{
			double Rsquare = 0.0;
			double sxy  = 0.0;
			double mean1 = Stat.mean(data1);
			double mean2 = Stat.mean(data2);

			if(data1.Length != data2.Length)
				return double.NaN;

			for(int i =0; i< data1.Length; i++)
			{
				if( data1[i].Equals( null) || data1[i].Equals(double.NaN) || data2[i].Equals(null) || data2[i].Equals(double.NaN) )
					continue;

				sxy += ( data1[i] - mean1 ) * (data2[i] - mean2);
			}

			Rsquare = sxy / Math.Sqrt(Stat.sumofDeviationFromMean(data1) * Stat.sumofDeviationFromMean(data2));

			return Rsquare;		
		}



		/**
		* 해당 배열의 평균을 구한다.
		* 
		* */
		public static double mean(double[] data1) 
		{
			double mean = 0.0;

			if(data1.Length == 0)
				return double.NaN;

			mean = Stat.Sum(data1) / ( data1.Length - Stat.Missing(data1) ) ;

			return mean;
		}

		/**
		* 해당 배열의 분산을 구한다.
		* 
		* */

		public static double variance(double[] data1)
		{
			double variance = 0.0;

			if(data1.Length == 0)
				return double.NaN;
		
			variance = Stat.sumofDeviationFromMean(data1) / (data1.Length - Stat.Missing(data1) -1);
		
			return variance;
		}


		/**
		* 해당 배열의 SumOfSuqre를 구한다.
		* 
		* */
		private static double sumofDeviationFromMean(double[] data1)
		{
			double sumofSqure = 0.0;

			if(data1.Length == 0)
				return double.NaN;

			double mean = Stat.mean(data1);
			for(int i =0; i < data1.Length; i++)
			{
				if(data1[i].Equals( null) || data1[i].Equals(double.NaN) )
					continue;

				sumofSqure += Math.Pow(data1[i] - mean,2);			
			}	

			return sumofSqure;
		}


		public static double StdDeviance(double[] data1)
		{
			return Math.Sqrt(Stat.variance(data1));
		}

		public static double Cpk(double LSL, double USL, double [] data1)
		{
			double dblMean = Stat.mean(data1);
			double dblStd = Stat.StdDeviance(data1);
			double dblCPL;
			double dblCPU;
			double dblCPK;

			if(LSL != double.NaN && USL == double.NaN)
			{
				dblCPL = (dblMean - LSL) / (3 * dblStd);
				dblCPK = dblCPL;
			}
			else if(LSL == double.NaN && USL != double.NaN)
			{
				dblCPU = (USL - dblMean) / (3 * dblStd);
				dblCPK = dblCPU;
			}
			else
			{
				dblCPL = (dblMean - LSL) / (3 * dblStd);
				dblCPU = (USL - dblMean) / (3 * dblStd);
				dblCPK = Math.Min(dblCPL, dblCPU);
			}

			return Math.Round(dblCPK, 4);
		}
	}
}
