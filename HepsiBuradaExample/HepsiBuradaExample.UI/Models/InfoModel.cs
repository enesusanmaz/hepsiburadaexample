using System;
using System.Collections.Generic;
using System.Text;

namespace HepsiBuradaExample.UI.Models
{
    public class InfoModel : IInfoModel
    {
        public string Message { get; set; }
        public string GetInfo()
        {
            string returnedString = "";
            returnedString += Environment.NewLine;
            returnedString += "-------------------------------------------------------------------------------------------------";
            returnedString += Environment.NewLine;
            returnedString += Message;
            returnedString += Environment.NewLine;
            returnedString += "-------------------------------------------------------------------------------------------------";
            returnedString += Environment.NewLine;
            return returnedString;
        }
    }
}
