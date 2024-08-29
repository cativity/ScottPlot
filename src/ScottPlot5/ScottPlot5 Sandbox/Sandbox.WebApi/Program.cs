using ScottPlot;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
WebApplication? app = builder.Build();

app.MapGet("/",
           static async context =>
           {
               const string html = "<html><body><img src='random.png'></body></html>";
               context.Response.ContentType = "text/html";
               await context.Response.WriteAsync(html);
           });

app.MapGet("/random.png",
           static async context =>
           {
               // create a plot and fill it with sample data
               Plot myPlot = new Plot();
               double[] dataX = Generate.Consecutive(100);
               double[] dataY = Generate.RandomWalk(100);
               myPlot.Add.Scatter(dataX, dataY);

               // render the plot as an image and serve it
               byte[] imageBytes = myPlot.GetImageBytes(600, 400, ImageFormat.Png);
               context.Response.ContentType = "image/png";
               await context.Response.Body.WriteAsync(imageBytes, 0, imageBytes.Length);
           });

app.MapGet("/inlineSVG",
           static async context =>
           {
               // create a plot and fill it with sample data
               Plot myPlot = new Plot();
               double[] dataX = Generate.Consecutive(100);
               double[] dataY = Generate.RandomWalk(100);
               myPlot.Add.Scatter(dataX, dataY);

               // render the plot as a SVG string and serve it inside HTML
               string svg = myPlot.GetSvgXml(600, 400);
               string html = $"<html><body>{svg}</body></html>";
               context.Response.ContentType = "text/html";
               await context.Response.WriteAsync(html);
           });

app.MapGet("/inlinePNG",
           static async context =>
           {
               // create a plot and fill it with sample data
               Plot myPlot = new Plot();
               double[] dataX = Generate.Consecutive(100);
               double[] dataY = Generate.RandomWalk(100);
               myPlot.Add.Scatter(dataX, dataY);

               // render the plot as a PNG and encode its bytes in HTML
               byte[] imgBytes = myPlot.GetImageBytes(600, 400, ImageFormat.Png);
               string b64 = Convert.ToBase64String(imgBytes);
               string png = $"<img src='data:image/png;base64,{b64}'>";
               string html = $"<html><body>{png}</body></html>";
               context.Response.ContentType = "text/html";
               await context.Response.WriteAsync(html);
           });

app.Run();
