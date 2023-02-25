﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;

namespace AMBER.Pages.Users.Faculty
{
    public partial class SelfEvaluation : System.Web.UI.Page
    {
        string connDB = ConfigurationManager.ConnectionStrings["amberDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["term"] == null || CheckifWeightisFilled())
            {
                Response.Redirect("/Pages/Users/NoEvaluationAvail.aspx");
            }
            if (Session["id"] == null && Session["role"] == null)
            {
                Response.Redirect("/Pages/Users/UsersLogin.aspx");
            }
            if (!IsPostBack)
            {
                GVbind();
                GVbind2();
            }
            getsemesterinfo();
            IsEvaluated();
        }
        public void IsEvaluated()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT rate FROM EVALUATION_TABLE WHERE evaluatee_id = '" + Session["insID"] + "' AND evaluator_id = '" + Session["insID"] + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows == true)
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "toast()", true);
                            while (reader.Read())
                            {
                                btnSubmit.Enabled = false;
                                chkConfirm.Enabled = false;
                                GridView1.Visible = false;
                                GridView2.Visible = true;
                                GVbind2();
                                txtComment.InnerText = getComments();
                            }
                        }
                        else
                        {
                            btnSubmit.Enabled = true;
                            chkConfirm.Enabled = true;
                            GridView1.Visible = true;
                            GridView2.Visible = false;
                            
                        }

                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
        }

        public string getComments()
        {
            string res = "";
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT DISTINCT comment FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id = INDICATOR_TABLE.indicator_Id WHERE EVALUATION_TABLE.school_id=202251298 AND EVALUATION_TABLE.evaluator_id = '"+ Session["insID"].ToString() + "' AND EVALUATION_TABLE.evaluatee_id = '" + Session["insID"].ToString() + "' AND INDICATOR_TABLE.isDeleted IS NULL AND comment IS NOT NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            res = reader["comment"].ToString();

                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
            return res;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AutoRedirect();
        }

        public void AutoRedirect()
        {
            int int_MilliSecondsTimeOut = this.SessionLengthMinutes * 60000;
            var sch_id = Session["school"].ToString();
            string str_Script = @"
               <script type='text/javascript'> 
                   intervalset = window.setInterval('Redirect()'," +
                       int_MilliSecondsTimeOut.ToString() + @");
                   function Redirect()
                   {
                       window.location.href='https://localhost:44311/Pages/Users/UsersLogin.aspx?school_id=" + AMBER.URLEncryption.GetencryptedQueryString(sch_id) + @"';
                   }
               </script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", str_Script);
        }

        public int SessionLengthMinutes
        {
            get { return Session.Timeout; }
        }

        bool CheckifWeightisFilled()
        {
            bool chkr = false;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM CONSTRUCTOR_TABLE WHERE weight = 0 AND school_id = '" + Session["school"].ToString() + "' AND isDeleted IS NULL";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            chkr = true;
                        }
                        else
                        {
                            chkr = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "test", "error()", true);
                Response.Write("<pre>" + ex.ToString() + "</pre>");
            }
            return chkr;
        }
        protected void GVbind()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY constructor_id)row_num,constructor_id,constructor_name,description FROM CONSTRUCTOR_TABLE WHERE school_id=@school_id AND isDeleted IS NULL AND role='instructor'", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView1.DataSource = dr;
                    GridView1.DataBind();
                }
            }
        }
        protected void GVbind2()
        {
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY constructor_id)row_num,constructor_id,constructor_name,description FROM CONSTRUCTOR_TABLE WHERE school_id=@school_id AND isDeleted IS NULL AND role='instructor'", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    GridView2.DataSource = dr;
                    GridView2.DataBind();
                }
            }
        }
        void getsemesterinfo()
        {
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM SEMESTER_TABLE WHERE status='Active' AND school_id= '" + Session["school"].ToString() + "'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblHeader.Text = "Self Evaluation " + reader["description"].ToString() + " year " + reader["year"].ToString();

                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int count = CategoryCount();
            if (IsRadioButtonListFilled())
            {
                if (chkConfirm.Checked == true)
                {
                    for (int i = 0; i < count; i++)
                    {
                        GridViewRow MainGvRow = GridView1.Rows[i];
                        GridView ChildGV = MainGvRow.FindControl("GridviewIndicator") as GridView;
                        int index = 0;
                        foreach (GridViewRow row in ChildGV.Rows)
                        {
                            int indicator_id = Convert.ToInt32(ChildGV.DataKeys[index++].Value);
                            RadioButtonList RBLrate = (row.Cells[0].FindControl("rblScore") as RadioButtonList);
                            var rate = RBLrate.SelectedValue;
                            try
                            {
                                using (var db = new SqlConnection(connDB))
                                {
                                    db.Open();
                                    using (var cmd = db.CreateCommand())
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        if (txtComment.InnerText == String.Empty)
                                        {
                                            cmd.CommandText = "INSERT INTO EVALUATION_TABLE(evaluator_id, evaluatee_id, indicator_id, rate, datetime_eval, dept_id, school_id)"
                                            + " VALUES("
                                            + "@evaluator,"
                                            + "@evaluatee,"
                                            + "@indicator,"
                                            + "@rate,"
                                            + "@datetime,"
                                            + "@dept,"
                                            + "@school)";
                                            cmd.Parameters.AddWithValue("@evaluator", Session["insID"]);
                                            cmd.Parameters.AddWithValue("@evaluatee", Session["insID"]);
                                            cmd.Parameters.AddWithValue("@indicator", indicator_id);
                                            cmd.Parameters.AddWithValue("@rate", rate);
                                            cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@dept", Session["dept"].ToString());
                                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                        }
                                        else
                                        {
                                            cmd.CommandText = "INSERT INTO EVALUATION_TABLE(evaluator_id, evaluatee_id, indicator_id, rate, comment, datetime_eval, dept_id, school_id)"
                                            + " VALUES("
                                            + "@evaluator,"
                                            + "@evaluatee,"
                                            + "@indicator,"
                                            + "@rate,"
                                            + "@comment,"
                                            + "@datetime,"
                                            + "@dept,"
                                            + "@school)";
                                            cmd.Parameters.AddWithValue("@evaluator", Session["insID"]);
                                            cmd.Parameters.AddWithValue("@evaluatee", Session["insID"]);
                                            cmd.Parameters.AddWithValue("@indicator", indicator_id);
                                            cmd.Parameters.AddWithValue("@rate", rate);
                                            cmd.Parameters.AddWithValue("@comment", txtComment.InnerText);
                                            cmd.Parameters.AddWithValue("@datetime", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@dept", Session["dept"].ToString());
                                            cmd.Parameters.AddWithValue("@school", Session["school"].ToString());
                                        }
                                        var ctr = cmd.ExecuteNonQuery();
                                        if (ctr >= 1)
                                        {
                                            txtComment.InnerText = String.Empty;
                                            chkConfirm.Checked = false;

                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
                                Response.Write("<pre>" + ex.ToString() + "</pre>");
                            }
                        }

                        btnSubmit.Enabled = false;
                        chkConfirm.Enabled = false;
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Success!','Thank you for participating', 'success')", true);
                    }
                    GridView1.Visible = false;
                    GridView2.Visible = true;
                    GVbind2();
                    txtComment.InnerText = getComments();
                    GVbind();
                }
                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please confirm before submitting your evaluation', 'warning')", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "swal.fire('Something went wrong!','Please Fill all rate', 'warning')", true);
            }
        }
        public int CategoryCount()
        {
            int count = 0;
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(constructor_id) AS COUNT FROM CONSTRUCTOR_TABLE WHERE school_id='" + Session["school"].ToString() + "' AND isDeleted IS NULL AND role='instructor'";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            count = Convert.ToInt32(reader["COUNT"]);
                        }
                        db.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sweetalert", "error()", true);
            }
            return count;
        }
        public bool IsRadioButtonListFilled()
        {
            int constructor = CategoryCount();
            int count = 0;
            bool chkr = true;
            var rate = "";
            for (int i = 0; i < constructor; i++)
            {
                GridViewRow MainGvRow = GridView1.Rows[i];
                GridView ChildGV = MainGvRow.FindControl("GridviewIndicator") as GridView;
                foreach (GridViewRow row in ChildGV.Rows)
                {
                    RadioButtonList RBLrate = (row.Cells[0].FindControl("rblScore") as RadioButtonList);
                    for (int j = 0; j < RBLrate.Items.Count; j++, count++)
                    {
                        if (RBLrate.Items[j].Selected)
                        {
                            rate = RBLrate.Items[j].Value;
                            count = 0;
                            break;
                        }
                    }
                    if (count == RBLrate.Items.Count)
                    {
                        chkr = false;
                        break;
                    }
                }
            }
            return chkr;
        }
        public static string convertToRoman(int n)
        {
            string[] roman_symbol = { "MMM", "MM", "M", "CM", "DCCC", "DCC", "DC", "D", "CD", "CCC", "CC", "C", "XC", "LXXX", "LXX", "LX", "L", "XL", "XXX", "XX", "X", "IX", "VIII", "VII", "VI", "V", "IV", "III", "II", "I" };
            int[] int_value = { 3000, 2000, 1000, 900, 800, 700, 600, 500, 400, 300, 200, 100, 90, 80, 70, 60, 50, 40, 30, 20, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };

            var roman_numerals = new StringBuilder();
            var index_num = 0;
            while (n != 0)
            {
                if (n >= int_value[index_num])
                {
                    n -= int_value[index_num];
                    roman_numerals.Append(roman_symbol[index_num]);
                }
                else
                {
                    index_num++;
                }
            }

            return roman_numerals.ToString();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Label num = (Label)e.Row.FindControl("lblNumber");

            num.Text = "Part " + convertToRoman(Int32.Parse(num.Text)) + ".";

            GridView gvindicator = (GridView)e.Row.FindControl("GridviewIndicator");
            int category = Int32.Parse(GridView1.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY indicator_id)row_num,indicator_id,indicator_name FROM INDICATOR_TABLE WHERE school_id=@school_id AND isDeleted IS NULL AND constructor_id='" + category + "'", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    gvindicator.DataSource = dr;
                    gvindicator.DataBind();
                }
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            Label num = (Label)e.Row.FindControl("lblNumber");

            num.Text = "Part " + convertToRoman(Int32.Parse(num.Text)) + ".";

            GridView gvindicator = (GridView)e.Row.FindControl("GridviewIndicator");
            int category = Int32.Parse(GridView1.DataKeys[e.Row.RowIndex].Value.ToString());
            using (SqlConnection db = new SqlConnection(connDB))
            {
                db.Open();
                SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER(ORDER BY INDICATOR_TABLE.indicator_id)row_num,INDICATOR_TABLE.indicator_id,INDICATOR_TABLE.indicator_name,EVALUATION_TABLE.rate FROM EVALUATION_TABLE JOIN INDICATOR_TABLE ON EVALUATION_TABLE.indicator_id = INDICATOR_TABLE.indicator_Id WHERE EVALUATION_TABLE.school_id=@school_id AND EVALUATION_TABLE.evaluator_id = '" + Session["insID"].ToString() + "' AND EVALUATION_TABLE.evaluatee_id = '" + Session["insID"].ToString() + "' AND INDICATOR_TABLE.isDeleted IS NULL AND INDICATOR_TABLE.constructor_id='" + category + "'", db);
                cmd.Parameters.AddWithValue("@school_id", Session["school"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows == true)
                {
                    gvindicator.DataSource = dr;
                    gvindicator.DataBind();
                }
            }
        }
    }
}