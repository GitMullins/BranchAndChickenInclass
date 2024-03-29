﻿using System;
using System.Collections.Generic;
using System.Linq;
using BranchAndChicken.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;

namespace BranchAndChicken.Api.DataAccess
{
    public class TrainerRepository
    {
        string _connectionString = "Server=localhost;Database=BranchAndChicken;Trusted_Connection=True;";

        public List<Trainer> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var trainers = db.Query<Trainer>("Select * From Trainer");

                //var cmd = db.CreateCommand();
                //cmd.CommandText = @"Select * 
                //                From Trainer";

                //var dataReader = cmd.ExecuteReader();

                //var trainers = new List<Trainer>();

                //while (dataReader.Read())
                //{
                //    trainers.Add(GetTrainerFromDataReader(dataReader));
                //}
                return trainers.ToList();
            }
        }

        public Trainer Get(string name)
        {
            using (var db = new SqlConnection(_connectionString))
            {

                var sql = @"select *
                            from Trainer
                            where Trainer.Name = @trainerName";

                var parameters = new { trainerName = name };

                var trainer = db.QueryFirst<Trainer>(sql, parameters);

                //connection.Open();

                //var cmd = connection.CreateCommand();
                //cmd.CommandText = @"select * 
                //                    from Trainer
                //                    where Trainer.Name = @trainerName";

                //cmd.Parameters.AddWithValue("trainerName", name);

                //var reader = cmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    return GetTrainerFromDataReader(reader);
                //}
                return trainer;
            }
        }

        public bool Remove(string name)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"delete
                            from trainer
                            where [name] = @name";

                return db.Execute(sql, new { name }) == 1;

                //connection.Open();

                //var cmd = connection.CreateCommand();
                //cmd.CommandText = @"delete
                //                    from trainer
                //                    where [name] = @name";

                //cmd.Parameters.AddWithValue("name", name);

                //return cmd.ExecuteNonQuery() == 1;

            }
        }

        public ActionResult<Trainer> GetSpecialty(string specialty)
        {
            throw new NotImplementedException();
        }

        public Trainer Update(Trainer updatedTrainer, int id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"
                           UPDATE [Trainer]
                           SET [Name] = @name,
                           [YearsOfExperience] = @yearsOfExperience,
                           [Specialty] = @specialty
                           output inserted.*
                           WHERE id = @id";

                //var parameters = new
                //{
                //    Id = id,
                //    Name = updatedTrainer.Name,
                //    YearsOfExperience = updatedTrainer.YearsOfExperience,
                //    Specialty = updatedTrainer.Specialty
                //};

                updatedTrainer.Id = id;

                var trainer = db.QueryFirst<Trainer>(sql,updatedTrainer);

                return trainer;

                //connection.Open();

                //var cmd = connection.CreateCommand();
                //cmd.CommandText = @"
                //                UPDATE [Trainer]
                //                SET [Name] = @name,
                //                [YearsOfExperience] = @yearsOfExperience,
                //                [Specialty] = @specialty
                //                output inserted.*
                //                WHERE id = @id";

                //cmd.Parameters.AddWithValue("name", updatedTrainer.Name);
                //cmd.Parameters.AddWithValue("yearsOfExperience", updatedTrainer.YearsOfExperience);
                //cmd.Parameters.AddWithValue("specialty", updatedTrainer.Specialty);
                //cmd.Parameters.AddWithValue("id", id);

                //var reader = cmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    return GetTrainerFromDataReader(reader);
                //}
                //return null;
            }
        }


        public Trainer Add(Trainer newTrainer)
        {
            using (var db = new SqlConnection(_connectionString))
            {

                var sql = @"
                            Insert into Trainer
                            [Name],
                            YearsOfExperience,
                            Specialty)
                            output inserted.*
                            values
                            (@name,
                            @yearsOfExperience,
                            @specialty)";

                return db.QueryFirst<Trainer>(sql, newTrainer);

                //connection.Open();

                //var cmd = connection.CreateCommand();
                //cmd.CommandText = @"
                //                Insert into Trainer
                //                [Name],
                //                YearsOfExperience,
                //                Specialty)
                //                output inserted.*
                //                values
                //                (@name,
                //                @yearsOfExperience,
                //                @specialty)";

                //cmd.Parameters.AddWithValue("name", newTrainer.Name);
                //cmd.Parameters.AddWithValue("yearsOfExperience", newTrainer.YearsOfExperience);
                //cmd.Parameters.AddWithValue("specialty", newTrainer.Specialty);

                //var reader = cmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    return GetTrainerFromDataReader(reader);
                //}
            }
            //return null;
        }

        Trainer GetTrainerFromDataReader(SqlDataReader reader)
        {
            //explicit cast
            var id = (int)reader["Id"];
            //implicit cast
            var returnedName = reader["name"] as string;
            //convert to
            var yearsOfExperience = Convert.ToInt32(reader["YearsOfExperience"]);
            //try parse
            Enum.TryParse<Specialty>(reader["specialty"].ToString(), out var specialty);

            var trainer = new Trainer
            {
                Specialty = specialty,
                Id = id,
                Name = returnedName,
                YearsOfExperience = yearsOfExperience
            };

            return trainer;

        }
    }
}
