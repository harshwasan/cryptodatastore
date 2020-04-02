using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;


public class Model
{
    readonly string strcon = ConfigurationManager.ConnectionStrings["sqlcon"].ConnectionString;
    readonly string strcon1 = ConfigurationManager.ConnectionStrings["sqlcon1"].ConnectionString;

    public string ErrorString { get; private set; }
    public Model()
    {

    }
    public DataTable returnusersandmailids(string uid)
    {

        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(strcon);
        SqlCommand cmd = new SqlCommand("sp_returnuserdetails", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@uid", SqlDbType.VarChar, 50).Value = uid;


        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {

            da.Fill(dt);
        }

        return dt;

    }

    public DataTable uchecker()
    {

        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(strcon);
        SqlCommand cmd = new SqlCommand("sp_returnuserboughtlist", con);
        cmd.CommandType = CommandType.StoredProcedure;
        //cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = username;


        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {

            da.Fill(dt);
        }

        return dt;

    }
    public DataTable cchecker()
    {

        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(strcon);
        SqlCommand cmd = new SqlCommand("returncoinlastdata", con);
        cmd.CommandType = CommandType.StoredProcedure;
        //cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = username;


        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {

            da.Fill(dt);
        }

        return dt;

    }

    public void coindatastore(DataTable dt)
    {

       // DataTable dt = new DataTable();
       
        for (var i =0; i <dt.Rows.Count; i++)
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("sp_storecoindata", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@1", SqlDbType.VarChar, 50).Value = dt.Rows[i]["id"];
            cmd.Parameters.Add("@2", SqlDbType.VarChar, 50).Value = dt.Rows[i]["currency"];
            cmd.Parameters.Add("@3", SqlDbType.VarChar, 50).Value = dt.Rows[i]["symbol"];
            cmd.Parameters.Add("@4", SqlDbType.VarChar, 50).Value = dt.Rows[i]["name"];
            cmd.Parameters.Add("@5", SqlDbType.VarChar, 50).Value = dt.Rows[i]["logo_url"];
            cmd.Parameters.Add("@6", SqlDbType.VarChar, 50).Value = dt.Rows[i]["rank"];
            cmd.Parameters.Add("@7", SqlDbType.VarChar, 50).Value = dt.Rows[i]["price"];
            cmd.Parameters.Add("@8", SqlDbType.VarChar, 50).Value = dt.Rows[i]["price_date"];
            cmd.Parameters.Add("@9", SqlDbType.VarChar, 50).Value = dt.Rows[i]["market_cap"];
            cmd.Parameters.Add("@10", SqlDbType.VarChar, 50).Value = dt.Rows[i]["circulating_supply"]; 
            cmd.Parameters.Add("@11", SqlDbType.VarChar, 50).Value = dt.Rows[i]["max_supply"];
            cmd.Parameters.Add("@12", SqlDbType.VarChar, 50).Value = dt.Rows[i]["1d"];
            cmd.Parameters.Add("@13", SqlDbType.VarChar, 50).Value = dt.Rows[i]["price_change_pct"];
            cmd.Parameters.Add("@14", SqlDbType.VarChar, 50).Value = dt.Rows[i]["volume"];
            cmd.Parameters.Add("@15", SqlDbType.VarChar, 50).Value = dt.Rows[i]["volume_change"];
            cmd.Parameters.Add("@16", SqlDbType.VarChar, 50).Value = dt.Rows[i]["volume_change_pct"];
            cmd.Parameters.Add("@17", SqlDbType.VarChar, 50).Value = dt.Rows[i]["market_cap_change"];
            cmd.Parameters.Add("@18", SqlDbType.VarChar, 50).Value = dt.Rows[i]["market_cap_change_pct"];
            cmd.Parameters.Add("@19", SqlDbType.VarChar, 50).Value = dt.Rows[i]["high"];
            cmd.Parameters.Add("@20", SqlDbType.VarChar, 50).Value = dt.Rows[i]["high_timestamp"];
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
       
        

    }
}