import http from "http";

export const simpleWebServer = http.createServer((req, res) => {
  switch (req.url) {
    case "/":
      res.statusCode = 200;
      res.setHeader("Content-Type", "text/plain");
      res.end("Hello World\n");
      break;
    case "/users":
      switch (req.method) {
        case "GET":
          res.statusCode = 200;
          res.setHeader("Content-Type", "application/json");
          res.end(
            JSON.stringify([
              { id: 1, name: "John" },
              { id: 2, name: "Jane" },
            ])
          );
          break;
        case "POST":
          res.statusCode = 201;
          res.setHeader("Content-Type", "application/json");
          let body = "";
          req.on("data", (chunk: any) => {
            body += chunk.toString();
          });
          req.on("end", () => {
            const user = JSON.parse(body);
            res.end(JSON.stringify(user));
          });
          break;
        default: //Method not allowed
          res.statusCode = 405;
          res.end();
      }
      break;
    default:
      res.statusCode = 404;
      res.end();
  }
});
