using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.Text;

namespace Spark.Engine.Validation
{
  public static class CustomValidation
  {
    public static string ErrorMessages { get; private set; }
    public static bool ValidateJson(string json)
    {
      JObject jObject = CreateJsonObject(json);

      if (jObject.GetValue("resourceType") == null)
        return true;// will validate in other middleware
      
      string resourceName = jObject.GetValue("resourceType").ToString().ToLower();
      JSchema jSchema = GetJsonSchema(resourceName);

      IList < string > errorMessages;
      bool isValid = jObject.IsValid(jSchema, out errorMessages);

      foreach (string err in errorMessages)
      {
        ErrorMessages += err.Replace("{",@"<").Replace("}",">")+"\r\n";
      }

      return isValid;
    }
    
    private static JSchema GetJsonSchema(string resourceName)
    {
      return JSchema.Parse((string)Resources.JsonValidSchema(resourceName));
    }

    private static JObject CreateJsonObject(string json)
    {
      return JObject.Parse(json);
    }
  }
}
