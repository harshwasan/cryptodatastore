using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows;

using System.Net.Mail;


namespace cryptodatastore
{
    public partial class datacall : System.Web.UI.Page
    { 

        protected Model obj = new Model();
        public string ErrorString { get; private set; } 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                savingdata.Visible = false;
                saveddata.Visible = false;
                searchusers.Visible = false;
                sendingmails.Visible = false;
                mailssent.Visible = false;
            }
            
        }

        #region save data to database
        //-------------saving data into datatable and calling check function
        protected void hdn_fld_ValueChanged()
        {
            savingdata.Visible = true;
            DataTable dt = JsonStringToDataTable(hdn_fld.Value);//converting json to datatable
            obj.coindatastore(dt);//storing datatable to database
            saveddata.Visible= true;           
            searchusers.Visible = true;
            checkcoin();//checking and sending mails
           
           
        }
        #endregion
        #region check for users incurring losses
        //-------------------checking for users incurring losses---------------------
        public void checkcoin() {
            var myHT = new Hashtable();
            DataTable dt1= new DataTable();
            DataTable dt2 = new DataTable();
            dt2 =obj.cchecker();
            dt1 = obj.uchecker();
            //----------- looping for each user's every coin and checking its value---------------
            for (var j = 0; j < dt1.Rows.Count; j++)
            {
                var s = dt1.Rows[j]["coinname"];
                for (var i = 0; i < dt2.Rows.Count; i++)
                {
                    var p = dt2.Rows[i]["currency"];
                    if (dt1.Rows[j]["coinname"].ToString() ==dt2.Rows[i]["currency"].ToString())
                    {
                        if (Convert.ToDouble(dt1.Rows[j]["cost"]) > 0)
                        {
                            if (Convert.ToDouble(dt1.Rows[j]["cost"]) >= Convert.ToDouble(dt2.Rows[i]["price"]))
                            {
                                //-----------storing value and user id in hashtable----------
                                if (myHT.Contains(dt1.Rows[j]["userid"]))
                                {
                                    string strValue1 = (string)myHT[dt1.Rows[j]["userid"]];

                                    myHT[dt1.Rows[j]["userid"]] = strValue1 + ", " + dt2.Rows[i]["currency"].ToString() + "( Notification limit set at:" + dt1.Rows[j]["cost"].ToString() + ", Current Price:" + dt2.Rows[i]["price"].ToString() + " )";

                                }
                                else
                                {
                                    myHT.Add(dt1.Rows[j]["userid"], " " + dt2.Rows[i]["currency"].ToString() + "( Notification limit set at:" + dt1.Rows[j]["cost"].ToString() + ", Current Price:"+ dt2.Rows[i]["price"] .ToString()+ " )") ;
                                    string strValue1 = (string)myHT[dt1.Rows[j]["userid"]];
                                }

                            }
                        
                        else {
                                var safety = (Convert.ToDouble(dt1.Rows[j]["costboughtat"]) + ((Convert.ToDouble(dt1.Rows[j]["costboughtat"]) * 20) / 100));
                                if (safety >= Convert.ToDouble(dt2.Rows[i]["price"]))
                                 {
                                   

                                    if (myHT.Contains(dt1.Rows[j]["userid"]))
                                    {
                                        string strValue1 = (string)myHT[dt1.Rows[j]["userid"]];

                                        myHT[dt1.Rows[j]["userid"]] = strValue1 + ", " + dt2.Rows[i]["currency"].ToString() + "( Bought at:" + dt1.Rows[j]["costboughtat"].ToString() + ", Current Price:" + dt2.Rows[i]["price"].ToString() + " )";

                                    }
                                    else
                                    {
                                          myHT.Add(dt1.Rows[j]["userid"], "" + dt2.Rows[i]["currency"].ToString() + "( Bought at:" + dt1.Rows[j]["costboughtat"].ToString() + ", Current Price:" + dt2.Rows[i]["price"].ToString() + " )");
                                          string strValue1 = (string)myHT[dt1.Rows[j]["userid"]];
                                    }

                                }


                         }


                        }

                    }

                }
 
               
            }
            ICollection c = myHT.Keys;
           
            sendingmails.Visible = true;
            
            foreach (string str in c)
            {
                var Array = myHT[str].ToString();
                DataTable dt3 = new DataTable();
                dt3= obj.returnusersandmailids(str.ToString());
                //------------returning user details ---------------

            try
            {
                    //----------------sending mails-----------------
                var fromAddress = new MailAddress("cryptonotificationmailer@gmail.com", "crypto");
                var toAddress = new MailAddress(dt3.Rows[0]["email"].ToString(), dt3.Rows[0]["username"].ToString());
                const string fromPassword = "cryptomailer";
                const string subject = "test";
                string body = "<p>Greetings " + dt3.Rows[0]["username"] + "</p><br><p>The prices of your invested coins namely " + Array + " are currently decreasing below the defined lower limit</p><br><p>This mail was sent to you to notify the change in the prices of the coins<br> Please make your decision accordingly to reduce your losses</p><p>With Regards,<br>Crpto<br>Customer Support Team</p>";

                    var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        IsBodyHtml = true,
                        Subject = subject,
                        Body = body
                    }
                       
                    )
                        
                {
                    smtp.Send(message);
                }
                
                // MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.ToString());
            }
            }

            mailssent.Visible = true;
        }
        //----------------------------------------
#endregion
        #region json to datatable
        //---------------converting json string to datatable--------------------
        public DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }
        //---------------------------------
        #endregion
        #region callfunction
        //---------------calling function-------------
        protected void button_Click(object sender, EventArgs e)

        {
            hdn_fld_ValueChanged();
        }
        //---------------------------------------------  
        #endregion
    }

}
