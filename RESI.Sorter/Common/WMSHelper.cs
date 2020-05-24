using Newtonsoft.Json;
using RESI.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RESI.Sorter.Common
{
    public class WMSHelper
    {

        string Host = "http://testxa.360scm.com:81/";
        string ApiKey = "B16F9359703641DEAD310B957A608990";
        string LoginUrl = "SCM.TMS7.WebApi/Oauth/GetToken";
        string token = "";
        string GetInfoUrl = "SCM.WMS7.WebApi/WMSCustomization/GetShippingPackDetailToDps";
        string UpdateWeightUrl = "SCM.WMS7.WebApi/WMSCustomization/UpdateShippingPackDetailFromDps";


        HttpHelper Http = new HttpHelper();
        private static object obj = new object();
        private static WMSHelper _ins;

        public static WMSHelper Ins
        {
            get
            {
                lock (obj)
                {
                    if (_ins == null)
                    {
                        _ins = new WMSHelper();
                    }
                }
                return _ins;
            }
        }

        public WMSHelper()
        {

        }
        public void Login()
        {
            token = Http.Get(Host + LoginUrl + "?apikey=" + ApiKey);
            dynamic data = JsonConvert.DeserializeObject(token);
            var resultCode = data.resultCode;
            if (resultCode == 0)
            {
                token = data.token;
            }
        }
        public void GetInfo(DateTime startDate, DateTime endDate)
        {
            string Start_Weight_Date = startDate.ToString();
            string End_Weight_Date = endDate.ToString();
            var content = new { Mach_Code = "LISHI_01", Start_Weight_Date, End_Weight_Date };
            List<object> detail = new List<object>();
            detail.Add(content);
            var data = new { whgid = "slhk.wh1", token, detail };
            string dataJson = JsonConvert.SerializeObject(data);
            string datastr = Http.Post(Host + GetInfoUrl, dataJson);
            BaseResult result = JsonConvert.DeserializeObject<BaseResult>(datastr);
        }
        public void UpdateWeight(string barcode, double Carton_Weight, int i = 0)
        {
            var content = new { Carton_No = barcode, Carton_Weight };
            List<object> detail = new List<object>();
            detail.Add(content);
            var data = new { whgid = "slhk.wh1", token, detail };
            string dataJson = JsonConvert.SerializeObject(data);
            string datastr = Http.Post(Host + UpdateWeightUrl, dataJson);

            dynamic ResultData = JsonConvert.DeserializeObject(datastr);
            var resultCode = ResultData.code;
            if (resultCode != "SUCCESS")
            {
                Thread.Sleep(20 * 1000);
                i++;
                if (i < 5)
                    UpdateWeight(barcode, Carton_Weight);
            }

        }
    }
}
