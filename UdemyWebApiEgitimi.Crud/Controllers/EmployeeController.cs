using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UdemyWebApiEgitimi.Crud.Controllers
{
    public class EmployeeController : ApiController
    {
        EmployeeDbEntities db = new EmployeeDbEntities();
        public HttpResponseMessage Get(String gender = "all", int? top = 0)
        {
            IQueryable<Employee> query = db.Employees;
            gender = gender.ToLower();
            switch (gender)
            {
                case "all":
                    query = query.Where(x => x.Gender.ToLower() == gender);
                    break;
                case "male":
                    query = query.Where(x => x.Gender.ToLower() == gender);
                    break;
                case "female":
                    query = query.Where(x => x.Gender.ToLower() == gender);
                    break;
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"");
            }
            if (top.HasValue && top.Value > 0)
            {
                query = query.Take(top.Value);
            }
            return Request.CreateResponse(HttpStatusCode.OK, query.ToList());
        }
        
        public HttpResponseMessage Get(int id)
        {
            var v = db.Employees.FirstOrDefault(x => x.id == id);
            if (v == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"{id} id'li veri Bulunamadı.");
            }
            else { return Request.CreateResponse(HttpStatusCode.OK, v); }
        }

        public HttpResponseMessage Post(Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                int i = db.SaveChanges();

                if (i > 0)
                {
                    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, employee);
                    httpResponseMessage.Headers.Location = new Uri(Request.RequestUri + "/" + employee.id);
                    return httpResponseMessage;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Veri ekleme yapılamadı");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        public HttpResponseMessage Put(Employee employee)
        {
            try
            {

                Employee emp = db.Employees.FirstOrDefault(x => x.id == employee.id);
                if (emp != null)
                {

                    emp.Name = employee.Name;
                    emp.Surname = employee.Surname;
                    emp.Gender = employee.Gender;
                    emp.Salary = employee.Salary;
                    int i = db.SaveChanges();

                    if (i > 0)
                    {
                        HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.Created, employee);
                        httpResponseMessage.Headers.Location = new Uri(Request.RequestUri + "/" + employee.id);
                        return httpResponseMessage;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Veri ekleme yapılamadı");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Veri Bulunamadı");
                }


            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                Employee emp = db.Employees.FirstOrDefault(x => x.id == id);
                if (emp != null)
                {
                    db.Employees.Remove(emp);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Başariyla Silindi");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Veri ekleme yapılamadı");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bulunamadı");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
