using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace RestSharpEmployeeTest
{
    public class EmployeePayroll
    {
        public int id { get; set; }

        public string name { get; set; }

        public string salary { get; set; }

    }

    [TestClass]
    public class UnitTest1
    {
        RestClient restClient;

        /// <summary>
        /// Creates Client and by passing root URL, establishes connection with the server
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            restClient = new RestClient("http://localhost:3000");
        }

        /// <summary>
        /// TC1: Retrieves All employee details by GET method
        /// </summary>
        [TestMethod]
        public void RetrieveEmployeeData()
        {
            IRestResponse response = GetEmployeeDetails();
            List<EmployeePayroll> records = JsonConvert.DeserializeObject<List<EmployeePayroll>>(response.Content);
            Assert.AreEqual(5, records.Count);
            foreach (EmployeePayroll employee in records)
            {
                Console.WriteLine("EmployeeID: " + employee.id + ", EmployeeName: " + employee.name + ", Salary: " + employee.salary);
            }
        }
        public IRestResponse GetEmployeeDetails()
        {
            //Creates GET request for accessing "/employees", for sending the request to the server
            RestRequest request = new RestRequest("/employees", Method.GET);
            //Sends the request to the server and in return gets the response which is collected by IRestResponse 
            IRestResponse response = restClient.Execute(request);
            return response;
        }
        /// <summary>
        /// TC2: Add a new Employee data into JSON server
        /// </summary>
        [TestMethod]
        public void AddaEmployeeIntoJSONServer()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            //Creating new Emp Json data for adding into server
            JObject jObject = new JObject();
            jObject.Add("id", "8");
            jObject.Add("name", "Mohit");
            jObject.Add("salary", "3000");
            request.AddParameter("application/json", jObject, ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
             //StatusCode for Adding data is 201
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            EmployeePayroll employees = JsonConvert.DeserializeObject<EmployeePayroll>(response.Content);
            Assert.AreEqual("Kunal", employees.name);
            Assert.AreEqual("20000", employees.salary);
            Console.WriteLine(response.Content);

        }
        /// <summary>
        /// TC4: Update Salary of Employee
        /// </summary>
        [TestMethod]
        public void UpdateSalaryIntoJSONServer()
        {
            RestRequest request = new RestRequest("/employees/8", Method.PUT);
            JObject jObject = new JObject();
            jObject.Add("name", "Rohit");
            jObject.Add("salary", "50000");
            request.AddParameter("application/json", jObject, ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            EmployeePayroll employee = JsonConvert.DeserializeObject<EmployeePayroll>(response.Content);
            Assert.AreEqual("Rohit", employee.name);
            Assert.AreEqual("50000", employee.salary);
            Console.WriteLine(response.Content);

        }
        /// <summary>
        /// TC5: Delete an Employee Data from JSON Server
        /// </summary>
        [TestMethod]
        public void DeleteEmployeeDataFromJSONServer()
        {
            RestRequest request = new RestRequest("employees/3", Method.DELETE);
            IRestResponse response = restClient.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Console.WriteLine(response.Content);

        }

    }
}
