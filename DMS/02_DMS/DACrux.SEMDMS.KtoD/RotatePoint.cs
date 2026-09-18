using System;
using System.Drawing;
using DACrux.SEMDMS.DSL;
using System.Collections;
using DACrux.Base;

namespace DACrux.SEMDMS.KtoD
{
	/// <summary>
	/// RotatePoint에 대한 요약 설명입니다.
	/// </summary>
	public class RotatePoint
	{
		const int		RK = 1000;
		public RotatePoint()
		{

		}
		/// ====================================================================
		///	- 수정자 : 미라콤 임영신
		///	- 수정일 : 2005-06-11
		///	- 변경로그 : SampleTest의 배열 리스트를 Rotate.	
		//public void RotateKLARF(ref KLARF_HEADER_TAG KData)
        public void RotateKLARF(ref KLARF_HEADER_TAG KData, ref ArrayList sampleTestPlan)
		/// ====================================================================	
		{
			float Angle = 0;
			KLARF_HEADER_TAG orgKHeader = KData;
			int recX = -1;
			int recY = -1;
			int recIX = -1;
			int recIY = -1;
			int recSX = -1;
			int recSY = -1;
			int CtoXREL = 0;
			int CtoYREL = 0;
//			int CenterIndexX = 0;
//			int CenterIndexY = 0;
			double tempDoubleX = 0.0;
			double tempDoubleY = 0.0;

			try
			{
				for(int i=0;i<KData.DefectRecordSpec.Length; i++)
				{
					if(KData.DefectRecordSpec[i].Equals("XREL")) recX = i;
					if(KData.DefectRecordSpec[i].Equals("YREL")) recY = i;
					if(KData.DefectRecordSpec[i].Equals("XINDEX")) recIX = i;
					if(KData.DefectRecordSpec[i].Equals("YINDEX")) recIY = i;
					if(KData.DefectRecordSpec[i].Equals("XSIZE")) recSX = i;
					if(KData.DefectRecordSpec[i].Equals("YSIZE")) recSY = i;
				}

				switch(KData.OrientationMarkLocation)
				{
					case "DOWN":
						return;
					case "LEFT":
						Angle = -90;
						break;
					case "RIGHT":
						Angle = 90;						
						break;
					case "UP":
					case "TOP":
						Angle = 180;
						break;
				}

				/// Die Size는 삼각함수를 이용하여 돌리고 2차원 길이는 음수가 없으므로 ABS를 취하면 됨
				Rotate(Angle,ref KData.DiePitchX ,ref KData.DiePitchY); 
				KData.DiePitchX = Math.Abs(KData.DiePitchX);
				KData.DiePitchY = Math.Abs(KData.DiePitchY);

				/// Origin 좌표는 삼각함수를 이용하여 돌리면 됨
				/// 

				tempDoubleX = -1 * KData.SampleCenterLocationX;
				tempDoubleY = -1 * KData.SampleCenterLocationY;

				Rotate(Angle,ref tempDoubleX ,ref tempDoubleY);

				switch(KData.OrientationMarkLocation)
				{
					case "DOWN":
						return;
					case "LEFT":
						KData.SampleCenterLocationX = KData.DiePitchX - tempDoubleX;
						KData.SampleCenterLocationY = tempDoubleY;
						break;				
					case "RIGHT":
						KData.SampleCenterLocationX = -1 * tempDoubleX;
						KData.SampleCenterLocationY = KData.DiePitchY + tempDoubleY;
						break;
					case "UP":
					case "TOP":
						KData.SampleCenterLocationX = KData.DiePitchX - tempDoubleX;
						KData.SampleCenterLocationY = KData.DiePitchY - tempDoubleY;
						break;
				}




				/// 절대 좌표는 삼각함수를 이용하여 돌리면 됨
				/// 
				for(int i=0;i<KData.Defects.Length;i++)
				{
					if(KData.Defects[i].DValue != null && KData.Defects[i].DValue.Length>0)
					{
						CtoXREL = (int)(orgKHeader.DiePitchX * RK) * orgKHeader.Defects[i].DValue[recIX] - (int)(orgKHeader.SampleCenterLocationX * RK) + orgKHeader.Defects[i].DValue[recX];
						CtoYREL = (int)(orgKHeader.DiePitchY * RK) * orgKHeader.Defects[i].DValue[recIY] - (int)(orgKHeader.SampleCenterLocationY * RK) + orgKHeader.Defects[i].DValue[recY];
						Rotate(Angle,ref CtoXREL,ref CtoYREL);


						/// Die Start에서 시작 됨으로 Offset값을 없앤다.
						/// 그리고 나머지를 구한다.
						KData.Defects[i].DValue[recX] = (int)((CtoXREL + (KData.SampleCenterLocationX * RK)) % (KData.DiePitchX * RK));
						KData.Defects[i].DValue[recY] = (int)((CtoYREL + (KData.SampleCenterLocationY * RK)) % (KData.DiePitchY * RK)); 


						/// Index 계산
						/// 
						KData.Defects[i].DValue[recIX] = (int)Math.Floor( (CtoXREL + (KData.SampleCenterLocationX * RK)) / (KData.DiePitchX * RK));
						KData.Defects[i].DValue[recIY] = (int)Math.Floor( (CtoYREL + (KData.SampleCenterLocationY * RK)) / (KData.DiePitchY * RK));

						/// 나머지를 구하고 음수이면 Die Size만큼 더한다.- 시작 포인트를 왼쪽하단으로 잡기위해
						/// Die에서의 Offset을 없앰
						if(KData.Defects[i].DValue[recX]<0)
						{
							KData.Defects[i].DValue[recX] += (int)(KData.DiePitchX * RK);
							//KData.Defects[i].DValue[recIX] += -1;
						}

						if(KData.Defects[i].DValue[recY]<0)
						{
							KData.Defects[i].DValue[recY] += (int)(KData.DiePitchY * RK);
							//KData.Defects[i].DValue[recIY] += -1;
						}

						/// Defect XSize YSize 계산
						Rotate(Angle,ref KData.Defects[i].DValue[recSX],ref KData.Defects[i].DValue[recSY]);
						KData.Defects[i].DValue[recSX] = Math.Abs(KData.Defects[i].DValue[recSX]);
						KData.Defects[i].DValue[recSY] = Math.Abs(KData.Defects[i].DValue[recSY]);					
					}
				}


				/// Die 좌표도 삼각함수를 이용하여 돌리면 됨
				
				/// ====================================================================
				///	- 수정자 : 미라콤 임영신
				///	- 수정일 : 2005-06-11
				///	- 변경로그 : SampleTest의 배열 리스트를 Rotate.	
				for(int z = 0; z < sampleTestPlan.Count; z++)
				{
					DIEINFO_TAG[] testDieInfo = (DIEINFO_TAG[])sampleTestPlan[z];

					for(int i=0;i<testDieInfo.Length;i++)
					{
						Rotate(Angle,ref testDieInfo[i].DX  ,ref testDieInfo[i].DY); 
					}
				}
//				for(int i=0;i<KData.TestDieInfo.Length;i++)
//				{
//					Rotate(Angle,ref KData.TestDieInfo[i].DX  ,ref KData.TestDieInfo[i].DY); 
//				}
				/// ====================================================================
				//전부 돌렸으므로 Notch는 0으로 맞춤
				KData.OrientationMarkLocation = "DOWN";
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public void Rotate(float Angle,ref double X, ref double Y)
		{
			double fRot = (double)((Angle/180)*Math.PI);
			double frX,frY;
			try
			{
				frX = X * Math.Cos(fRot) + Y * Math.Sin(fRot);
				frY = Y * Math.Cos(fRot) + X * Math.Sin(fRot);
				X = frX;
				Y = frY;
			}
			catch
			{
			}
		}

		public void Rotate(float Angle,ref float X, ref float Y)
		{
			float fRot = (float)((Angle/180)*Math.PI);
			float frX,frY;
			try
			{
				frX = (float)(X * Math.Cos(fRot) + Y * Math.Sin(fRot));
				frY = (float)(Y * Math.Cos(fRot) + X * Math.Sin(fRot));
				X = frX;
				Y = frY;
			}
			catch
			{
			}
		}

		public void Rotate(float Angle,ref int X, ref int Y)
		{
			double fRot = (double)((Angle/180)*Math.PI);
			double frX,frY;
			try
			{
				frX = X * Math.Cos(fRot) + Y * Math.Sin(fRot);
				frY = Y * Math.Cos(fRot) - X * Math.Sin(fRot);
				X = (int)Math.Round(frX);
				Y = (int)Math.Round(frY);
			}
			catch
			{
			}
		}

		public PointF Rotate(float Angle,RectangleF RectF)
		{
			PointF[] pntFour = null;
			float minX = 300.0f;
			float minY = 300.0f;
			PointF rtnPoint = PointF.Empty;
			float tmpX, tmpY;
			try
			{
				pntFour = new PointF[4];
				pntFour[0] = new PointF(RectF.X,RectF.Y);
				pntFour[1] = new PointF(RectF.X + RectF.Width,RectF.Y);
				pntFour[2] = new PointF(RectF.X + RectF.Width,RectF.Y + RectF.Height);
				pntFour[3] = new PointF(RectF.X,RectF.Y  + RectF.Height);
				
				for(int i=0;i<4;i++)
				{
					tmpX = pntFour[i].X;
					tmpY = pntFour[i].Y;

					Rotate(Angle,ref tmpX,ref tmpY);
					pntFour[i] = new PointF(tmpX,tmpY);

					minX = Math.Min(pntFour[i].X,minX);
					minY = Math.Min(pntFour[i].Y,minY);
				}
				
				rtnPoint = new PointF(minX ,minY);
				return rtnPoint;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(pntFour != null) pntFour = null;
			}
		}

	}
}
