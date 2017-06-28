using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DataBasesAndModels.Models
{
    public class TownContext
    {
        string connectionString = ConfigurationManager.ConnectionStrings["TownContext"].ConnectionString;//ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;



        public Character getCharacter(int id)
        {
            if (id <= 0) return null;
            Character res = new Character();
            res.Id = id;
            string sql = "SELECT * FROM CHARACTERS WHERE id = " + res.Id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataReader rdr = new SqlCommand(sql, connection).ExecuteReader();
                
                if (rdr.HasRows)
                {
                        rdr.Read();
                        res.Name = (string)rdr.GetValue(1);
                        res.Surname = (string)rdr.GetValue(2);
                        res.Age = (int)rdr.GetValue(3);
                        res.Gender = (int)rdr.GetValue(4);
                        res.FatherId = (int)rdr.GetValue(5);
                        res.MotherId = (int)rdr.GetValue(6);
                        res.CoupleId = (int)rdr.GetValue(7);
                        res.HouseId = (int)rdr.GetValue(8);
                        res.StreetId = (rdr[9].GetType().ToString() != "System.DBNull") ? (int)rdr.GetValue(9) : -1;

                        res.Street = getStreet((int)res.StreetId);
                }
                else{
                    res.Name = res.Surname = "NULL";
                    res.Age =res.Gender = res.FatherId = res.MotherId = res.CoupleId = res.HouseId = -1;
                    res.StreetId = null;

                    res.Street = null;
                }
            }
            return res;
        }

        public List<Character> getCharacters()
        {
            return getCharacters(null);
        }

        public List<Character> getCharacters(int? streetId)
        {
            List<Character> result;
            string sql = "SELECT * FROM CHARACTERS";
            if (streetId != null) sql += " WHERE STREETId=" + (int)streetId;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataReader rdr = new SqlCommand(sql, connection).ExecuteReader();

                result = new List<Character>();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Character tch = new Character();
                        tch.Id = (int)rdr.GetValue(0);
                        tch.Name = (string)rdr.GetValue(1);
                        tch.Surname = (string)rdr.GetValue(2);
                        tch.Age = (int)rdr.GetValue(3);
                        tch.Gender = (int)rdr.GetValue(4);
                        tch.FatherId = (int)rdr.GetValue(5);
                        tch.MotherId = (int)rdr.GetValue(6);
                        tch.CoupleId = (int)rdr.GetValue(7);
                        tch.HouseId = (int)rdr.GetValue(8);
                        tch.StreetId = (rdr[9].GetType().ToString()!="System.DBNull")?(int)rdr.GetValue(9):-1;

                        tch.Street = streetId == null ? tch.StreetId==null?null:getStreet((int)tch.StreetId) : null;

                        result.Add(tch);
                    }
                }
            }
            return result;
        }

        public Street getStreet(int id)
        {
            if (id <= 0) return null;
            Street str = new Street();
            str.Name = "";
            str.CreationAge = -1;
            str.Id = (int)id;
            str.Characters = new List<Character>();

            string sql = "SELECT * FROM STREETS WHERE id = "+str.Id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataReader rdr = new SqlCommand(sql, connection).ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    str.Name = (string)rdr["Name"];
                    str.CreationAge = (int)rdr["CreationAge"];
                }
            }
            return str;
        }

        public List<Street> getStreets()
        {
            List<Street> result;
            string sql = "SELECT * FROM STREETS";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataReader rdr = new SqlCommand(sql, connection).ExecuteReader();

                result = new List<Street>();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Street str = new Street();
                        str.Id = (int)rdr["Id"];
                        str.Name = (string)rdr["Name"];

                        str.CreationAge = (int)rdr["CreationAge"];

                        str.Characters = getCharacters(str.Id);

                        result.Add(str);
                    }
                }
            }
            return result;
        }

        public void addCharacter(Character ch)
        {
            string sql = "INSERT INTO CHARACTERS (Name,Surname,Age,Gender,FatherId,MotherId,CoupleId,HouseId,StreetId) VALUES (";
            sql += "'"+ch.Name + "',";
            sql += "'" + ch.Surname + "',";
            sql += ch.Age + ",";
            sql += ch.Gender + ",";
            sql += ch.FatherId + ",";
            sql += ch.MotherId + ",";
            sql += ch.CoupleId + ",";
            sql += ch.HouseId + ",";
            sql += ch.StreetId + ")";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

            }
        }

        public void addStreet(Street st)
        {
            string sql = "INSERT INTO STREETS (Name,CreationAge) VALUES (";
            sql += "'" + st.Name + "' ,";
            sql += st.CreationAge + ")";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

            }
        }

        public void updateCharacter(Character ch)
        {
            string sql = "UPDATE CHARACTERS SET " +
                "Name = '"+ch.Name+"'," +
                "Surname = '"+ch.Surname+"'," +
                "Age = "+ch.Age+"," +
                "Gender = "+ch.Gender+"," +
                "FatherId = "+ch.FatherId+"," +
                "MotherId = "+ch.MotherId+"," +
                "CoupleId = "+ch.CoupleId+"," +
                "HouseId = "+ch.HouseId+"," +
                "StreetId = "+ch.StreetId+"" +
                " WHERE Id = "+ ch.Id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

            }
        }

        public void updateStreet(Street st)
        {
            string sql = "UPDATE STREETS  SET Name = '"+st.Name+"',CreationAge = "+st.CreationAge+") WHERE Id = "+st.Id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

            }
        }

        public void deleteCharacter(int id)
        {
            string sql = "DELETE FROM CHARACTERS WHERE Id = "+id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

            }
        }

        public void deleteStreet(int id)
        {
            string sql = "DELETE FROM STREETS WHERE Id = " + id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

            }
        }

    }


}