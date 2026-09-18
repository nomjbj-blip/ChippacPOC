using System;

namespace DACrux.Framework.Interface
{
    public interface iDACruxStatusBar
	{
		void SetMainStatusBarMsg(string strMessage);
		void SetMainStatusBarProgress(int iMaxValue, int iValue);
    }
}