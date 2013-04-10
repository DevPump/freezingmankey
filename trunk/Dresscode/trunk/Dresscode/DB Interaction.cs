﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace Dresscode
{
    class DB_Interaction
    {
        globals gl = new globals();
        public string dgvselectioncommand(string sql, string firstname, string lastname, string studentid, string teacher, string infraction, string frmname, string dgn)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Clear();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, gl.oleconnection);
                if (teacher != "")
                    dataAdapter.SelectCommand.Parameters.Add("teacher", OleDbType.VarChar, 50).Value = teacher;
                if (infraction != "")
                    dataAdapter.SelectCommand.Parameters.Add("infraction", OleDbType.VarChar, 20).Value = infraction;
                if (firstname != "")
                {
                    dataAdapter.SelectCommand.Parameters.Add("firstname", OleDbType.VarChar, 20).Value = firstname;
                    dataAdapter.SelectCommand.Parameters.Add("lastname", OleDbType.VarChar, 20).Value = lastname;
                    dataAdapter.SelectCommand.Parameters.Add("studentid", OleDbType.VarChar, 20).Value = studentid;
                }
                dataAdapter.SelectCommand.CommandType = CommandType.Text;
                DataGridView dgv = Application.OpenForms[frmname].Controls[dgn] as DataGridView;
                dataAdapter.Fill(ds);
                dgv.DataSource = (ds.Tables[0]);
                if (frmname == "Reports")
                {
                    for (int i = 0; i <= 12; i++)
                    {
                        if (i <= 1)
                            dgv.Columns[i].Visible = false;
                        if (i != 11)
                            dgv.Columns[i].ReadOnly = true;
                    }
                }
                if (frmname == "Teacher")
                {
                    dgv.Columns[0].Visible = false;
                    dgv.Columns[1].Visible = false;
                }
                if (frmname == "Teacher_Editor")
                {
                    dgv.Columns[1].Visible = false;
                }
                if (frmname == "Email")
                {
                    for (int i = 0; i <= 12; i++)
                    {
                        if (i <= 1)
                            dgv.Columns[i].Visible = false;
                        dgv.Columns[i].ReadOnly = true;
                    }
                }
                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception x)
            {
                if (x.Message != "Operation is not valid because it results in a reentrant call to the SetCurrentCellAddressCore function")
                    MessageBox.Show(x.Message, "Error");
            }
            finally
            {
                gl.oleconnection.Close();
            }
            return null;
        }
        public void dbcommands(string sql, string frmname, string teacherid, string firstname, string lastname, string studentid, string teacher, string infraction, string details, string email, string grade)
        {
            if (gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
            OleDbDataAdapter oledbAdapter = new OleDbDataAdapter();
            oledbAdapter.InsertCommand = new OleDbCommand(sql, gl.oleconnection);
            if (frmname == "Teacher")
            {
                oledbAdapter.InsertCommand.Parameters.AddWithValue("teacherid", teacherid);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("studentid", studentid);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("firstname", firstname);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("lastname", lastname);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("teacher", teacher);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("infraction", infraction);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("details", details);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("grade", grade);
            }
            if (frmname == "Student_Editor")
            {
                oledbAdapter.InsertCommand.Parameters.AddWithValue("studentid", studentid);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("lastname", lastname);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("firstname", firstname);
                oledbAdapter.InsertCommand.Parameters.AddWithValue("grade", grade);
            }
            if (frmname == "Infractions_List")
            {
                oledbAdapter.InsertCommand.Parameters.AddWithValue("infraction", infraction);
            }
            oledbAdapter.InsertCommand.ExecuteNonQuery();
            if (gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close();
        }
        public void testcommands(string sql, string[] parameter, string[] text)
        {
            for (int i =0; i<parameter.Length; i++)
            {
                if (parameter[i] != "")
                {
                    if (gl.oleconnection.State == ConnectionState.Closed) gl.oleconnection.Open();
                    OleDbDataAdapter oledbAdapter = new OleDbDataAdapter();
                    oledbAdapter.InsertCommand = new OleDbCommand("", gl.oleconnection);
                    oledbAdapter.InsertCommand.Parameters.AddWithValue(parameter[i], text[i]);
                    if (gl.oleconnection.State == ConnectionState.Open) gl.oleconnection.Close();
                }
            }
        }
    }
}
