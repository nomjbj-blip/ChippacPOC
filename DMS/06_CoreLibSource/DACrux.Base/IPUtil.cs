using System;
using System.Net;
using System.Management;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace DACrux.Base
{
    /// <summary>
    /// 클래스  명: IPUtil<br/>
    /// 클래스요약: Clinet IP / Mac Address 관련 클래스<br/>
    /// 작  성  자: YS Lim<br/>
    /// 최초작성일: 2011-01-24<br/>
    /// 최종수정자: <br/>
    /// 최종수정일: <br/>
    /// 상세  설명: <br/>
    /// 변경  내용: <br/>
    /// </summary>
    public class IPUtil
    {
        /// <summary>
        /// 사용자 PC의 IP Address 를 읽어서 반환한다.
        /// </summary>
        /// <returns>IP Address</returns>
        public static string GetLocalIPAddress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }

            return "unknown ip";
        }


        public static string GetMacAddress()
        {
            string ip = GetLocalIPAddress();
            string mac = GetMacAddress(ip);

            return mac;
        }

        /// <summary>
        /// IP Address에 해당하는 Mac Address를 가져온다.
        /// </summary>
        /// <param name="ip">찾고자 하는 IP Address</param>
        /// <returns>Mac Address</returns>
        public static string GetMacAddress(string ip)
        {
            string rtn = string.Empty;
            ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='TRUE'");
            ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq);
            foreach (ManagementObject mo in query1.Get())
            {
                string[] address = (string[])mo["IPAddress"];


                try
                {
                    if (address[0].StartsWith(ip) || ip.StartsWith(address[0]) || address[1].StartsWith(ip) || ip.StartsWith(address[1]))
                        if (mo["MACAddress"] != null)
                        {
                            rtn = mo["MACAddress"].ToString();
                            break;
                        }
                }
                catch
                {
                    continue;
                }
            }
            return rtn;
        }


        /// <summary>
        /// 첫번째 IPv4 값을 가져온다.
        /// </summary>
        /// <returns>못찾으면 null</returns>
        public string GetFirstIPv4()
        {
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

            foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList)
            {
                if (regex.IsMatch(ip.ToString()))
                {
                    return ip.ToString();
                }
            }

            return null;
        }

    }
}
