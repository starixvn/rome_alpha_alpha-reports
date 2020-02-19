namespace Alpha.Reports.Site.Helpers
{
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Web.Mvc;

    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; }

        public Formatting Formatting { get; }

        public JsonNetResult(object data) : this(data, Formatting.None, new JsonSerializerSettings())
        {
        }

        public JsonNetResult(object data,
            Formatting formatting,
            JsonSerializerSettings settings)
        {
            Data = data;
            SerializerSettings = settings;
            Formatting = formatting;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;
            response.ContentType = ContentType ?? "application/json";
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                using (var writer = new JsonTextWriter(response.Output)
                {
                    Formatting = Formatting
                })
                {
                    var serializer = JsonSerializer.Create(SerializerSettings);
                    serializer.Serialize(writer, Data);
                    writer.Flush();
                }
            }
        }
    }
}