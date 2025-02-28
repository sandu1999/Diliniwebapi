﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPI.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration , IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]

        public JsonResult Get()
        {
            string query = @"
                        select EmployeeId,EmployeeName,Department,
                        convert (Varchar(10),DateOfJoining,120) as DateOfJoining
                        ,PhotoFileName
                        from dbo.Employee
                                ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"
                      insert into dbo.Employee 
                       (EmployeeName,Department,DateOfJoining,PhotoFileName)
                        Values
                         (
                             '" + emp.EmployeeName + @"'
                             ,'" + emp.Department + @"'
                             ,'" + emp.DateOfJoining + @"'
                             ,'" + emp.PhotoFileName + @"'
                                )
                         ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfuly");

        }


        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"
                      update dbo.Employee set 
                      EmployeeName ='" + emp.EmployeeName + @"'
                       ,Department ='" + emp.Department + @"'
                       ,DateOfJoining ='" + emp.DateOfJoining + @"'
                       ,PhotoFileName ='" + emp.PhotoFileName + @"'
                      where EmployeeId = " + emp.EmployeeId + @"
                     
                         ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfuly");

        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                      delete from dbo.Employee
                      where EmployeeId = " + id + @"
                     
                         ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfuly");

        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;
                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);

            }

            catch (Exception)
            {
                return new JsonResult("annonymous.png");
            }
            
        }

        [Route("GetallDepartmentNames")]

        public JsonResult GetAllDepartmentNames()
        {
            string query = @"
                        select DepartmentName from dbo.Department;
                                ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myCon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);

        }
    }
}
