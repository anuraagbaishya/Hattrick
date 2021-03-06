﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Hattrick
{
    public partial class BowlerForm : Form
    {
        string teamid;
        string connectionString = "your_connection_string";
        public BowlerForm(string teamid)
        {
            InitializeComponent();
            this.teamid = teamid;
        }

        private void BowlerForm_Load(object sender, EventArgs e)
        {
            string sql = "select name, overs, runs, wickets, economy, avrg from playerperformance_bowler natural join (playerinfo) where teamid='" + teamid + "';";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand sCommand = new MySqlCommand(sql, connection);
            MySqlDataAdapter sAdapter = new MySqlDataAdapter(sCommand);
            MySqlCommandBuilder sBuilder = new MySqlCommandBuilder(sAdapter);
            DataSet sDs = new DataSet();
            sAdapter.Fill(sDs, "playerperformance_bowler");
            DataTable sTable = sDs.Tables["playerperformance_bowler"];
            connection.Close();
            performanceGrid.DataSource = sDs.Tables["playerperformance_bowler"];
            performanceGrid.ReadOnly = true;
            performanceGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            BindingSource bSource = new BindingSource();
            bSource.DataSource = sTable;
            performanceGrid.DataSource = bSource;
        }

        private string getTeamName()
        {
            string sql = "select teamname from teaminfo where teamid=\"" + teamid + "\";";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand sCommand = new MySqlCommand(sql, connection);
            return (string)sCommand.ExecuteScalar();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            this.Close();
            new TeamDataForm(getTeamName()).Show();
        }
    }
}
