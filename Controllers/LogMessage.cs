using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.Controllers
{
    public static class LogMessage
    {
        public static string GetAll(string objectName)
        {
            return $"Returned all {objectName}s from the database.";
        }
        public static string GetById(string objectName, int? id)
        {
            return $"Returned a {objectName} with ID={id} from the database.";
        }

        public static string ExistsId(string objectName, int? id)
        {
            return $"The {objectName} with ID={id} already exists in the database.";
        }

        public static string Updated(string objectName, int? id)
        {
            return $"The {objectName} with ID={id} was updated successfully.";
        }

        public static string Created(string objectName, int? id)
        {
            return $"The {objectName} with ID={id} was created successfully.";
        }

        public static string Deleted(string objectName, int? id)
        {
            return $"{objectName} ID={id} was deleted successfully.";
        }

        public static string Error(string methodName)
        {
            return $"Something went wrong inside {methodName}";
        }

        public static string NotFound(string objectName, int? id)
        {
            return $"The {objectName} with ID={id} couldn't be found in the database.";
        }

        public static string ObjectNull(string objectName)
        {
            return $"The {objectName} object sent from client is null.";
        }

        public static string InvalidModel(string objectName)
        {
            return $"The {objectName} object sent from client is invalid.";
        }

        public static string InvalidId(string objectName)
        {
            return $"The {objectName} ID must be non-empty and greater than 0.";
        }

        public static string IdNotMatch()
        {
            return $"The Id in the URL doesn't match the object's Id in the request body.";
        }

        public static string DeleteError(string objectName, int Id, string relatedObjectName)
        {
            return $"Cannot delete {objectName} with ID={Id}. It has related {relatedObjectName}s.";
        }
    }
}
